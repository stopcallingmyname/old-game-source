using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;

namespace BestHTTP.WebSocket.Frames
{
	// Token: 0x020001BC RID: 444
	public struct WebSocketFrameReader
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0009E636 File Offset: 0x0009C836
		// (set) Token: 0x06001072 RID: 4210 RVA: 0x0009E63E File Offset: 0x0009C83E
		public byte Header { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0009E647 File Offset: 0x0009C847
		// (set) Token: 0x06001074 RID: 4212 RVA: 0x0009E64F File Offset: 0x0009C84F
		public bool IsFinal { get; private set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x0009E658 File Offset: 0x0009C858
		// (set) Token: 0x06001076 RID: 4214 RVA: 0x0009E660 File Offset: 0x0009C860
		public WebSocketFrameTypes Type { get; private set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x0009E669 File Offset: 0x0009C869
		// (set) Token: 0x06001078 RID: 4216 RVA: 0x0009E671 File Offset: 0x0009C871
		public bool HasMask { get; private set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0009E67A File Offset: 0x0009C87A
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x0009E682 File Offset: 0x0009C882
		public ulong Length { get; private set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x0009E68B File Offset: 0x0009C88B
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x0009E693 File Offset: 0x0009C893
		public byte[] Data { get; private set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0009E69C File Offset: 0x0009C89C
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x0009E6A4 File Offset: 0x0009C8A4
		public string DataAsText { get; private set; }

		// Token: 0x0600107F RID: 4223 RVA: 0x0009E6B0 File Offset: 0x0009C8B0
		internal void Read(Stream stream)
		{
			this.Header = this.ReadByte(stream);
			this.IsFinal = ((this.Header & 128) > 0);
			this.Type = (WebSocketFrameTypes)(this.Header & 15);
			byte b = this.ReadByte(stream);
			this.HasMask = ((b & 128) > 0);
			this.Length = (ulong)((long)(b & 127));
			if (this.Length == 126UL)
			{
				byte[] array = VariableSizedBufferPool.Get(2L, true);
				stream.ReadBuffer(array, 2);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(array, 0, 2);
				}
				this.Length = (ulong)BitConverter.ToUInt16(array, 0);
				VariableSizedBufferPool.Release(array);
			}
			else if (this.Length == 127UL)
			{
				byte[] array2 = VariableSizedBufferPool.Get(8L, true);
				stream.ReadBuffer(array2, 8);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(array2, 0, 8);
				}
				this.Length = BitConverter.ToUInt64(array2, 0);
				VariableSizedBufferPool.Release(array2);
			}
			byte[] array3 = null;
			if (this.HasMask)
			{
				array3 = VariableSizedBufferPool.Get(4L, true);
				if (stream.Read(array3, 0, 4) < array3.Length)
				{
					throw ExceptionHelper.ServerClosedTCPStream();
				}
			}
			if (this.Type == WebSocketFrameTypes.Text || this.Type == WebSocketFrameTypes.Continuation)
			{
				this.Data = VariableSizedBufferPool.Get((long)this.Length, true);
			}
			else if (this.Length == 0UL)
			{
				this.Data = VariableSizedBufferPool.NoData;
			}
			else
			{
				this.Data = new byte[this.Length];
			}
			if (this.Length == 0UL)
			{
				return;
			}
			uint num = 0U;
			for (;;)
			{
				int num2 = stream.Read(this.Data, (int)num, (int)(this.Length - (ulong)num));
				if (num2 <= 0)
				{
					break;
				}
				num += (uint)num2;
				if ((ulong)num >= this.Length)
				{
					goto Block_11;
				}
			}
			throw ExceptionHelper.ServerClosedTCPStream();
			Block_11:
			if (this.HasMask)
			{
				uint num3 = 0U;
				while ((ulong)num3 < this.Length)
				{
					this.Data[(int)num3] = (this.Data[(int)num3] ^ array3[(int)(num3 % 4U)]);
					num3 += 1U;
				}
				VariableSizedBufferPool.Release(array3);
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0009E885 File Offset: 0x0009CA85
		private byte ReadByte(Stream stream)
		{
			int num = stream.ReadByte();
			if (num < 0)
			{
				throw ExceptionHelper.ServerClosedTCPStream();
			}
			return (byte)num;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0009E898 File Offset: 0x0009CA98
		public void Assemble(List<WebSocketFrameReader> fragments)
		{
			fragments.Add(this);
			ulong num = 0UL;
			for (int i = 0; i < fragments.Count; i++)
			{
				num += fragments[i].Length;
			}
			byte[] array = (fragments[0].Type == WebSocketFrameTypes.Text) ? VariableSizedBufferPool.Get((long)num, true) : new byte[num];
			ulong num2 = 0UL;
			for (int j = 0; j < fragments.Count; j++)
			{
				Array.Copy(fragments[j].Data, 0, array, (int)num2, (int)fragments[j].Length);
				VariableSizedBufferPool.Release(fragments[j].Data);
				num2 += fragments[j].Length;
			}
			this.Type = fragments[0].Type;
			this.Header = fragments[0].Header;
			this.Length = num;
			this.Data = array;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0009E9A4 File Offset: 0x0009CBA4
		public void DecodeWithExtensions(WebSocket webSocket)
		{
			if (webSocket.Extensions != null)
			{
				for (int i = 0; i < webSocket.Extensions.Length; i++)
				{
					IExtension extension = webSocket.Extensions[i];
					if (extension != null)
					{
						byte[] array = extension.Decode(this.Header, this.Data, (int)this.Length);
						if (this.Data != array)
						{
							VariableSizedBufferPool.Release(this.Data);
							this.Data = array;
							this.Length = (ulong)((long)array.Length);
						}
					}
				}
			}
			if (this.Type == WebSocketFrameTypes.Text && this.Data != null)
			{
				this.DataAsText = Encoding.UTF8.GetString(this.Data, 0, (int)this.Length);
				VariableSizedBufferPool.Release(this.Data);
				this.Data = null;
			}
		}
	}
}
