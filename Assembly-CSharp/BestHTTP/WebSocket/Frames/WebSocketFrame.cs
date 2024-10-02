using System;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;

namespace BestHTTP.WebSocket.Frames
{
	// Token: 0x020001BB RID: 443
	public sealed class WebSocketFrame
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0009E27C File Offset: 0x0009C47C
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x0009E284 File Offset: 0x0009C484
		public WebSocketFrameTypes Type { get; private set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x0009E28D File Offset: 0x0009C48D
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x0009E295 File Offset: 0x0009C495
		public bool IsFinal { get; private set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0009E29E File Offset: 0x0009C49E
		// (set) Token: 0x06001064 RID: 4196 RVA: 0x0009E2A6 File Offset: 0x0009C4A6
		public byte Header { get; private set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x0009E2AF File Offset: 0x0009C4AF
		// (set) Token: 0x06001066 RID: 4198 RVA: 0x0009E2B7 File Offset: 0x0009C4B7
		public byte[] Data { get; private set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x0009E2C0 File Offset: 0x0009C4C0
		// (set) Token: 0x06001068 RID: 4200 RVA: 0x0009E2C8 File Offset: 0x0009C4C8
		public int DataLength { get; private set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x0009E2D1 File Offset: 0x0009C4D1
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x0009E2D9 File Offset: 0x0009C4D9
		public bool UseExtensions { get; private set; }

		// Token: 0x0600106B RID: 4203 RVA: 0x0009E2E2 File Offset: 0x0009C4E2
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data) : this(webSocket, type, data, true)
		{
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0009E2EE File Offset: 0x0009C4EE
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool useExtensions) : this(webSocket, type, data, 0UL, (ulong)((data != null) ? ((long)data.Length) : 0L), true, useExtensions)
		{
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0009E309 File Offset: 0x0009C509
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool isFinal, bool useExtensions) : this(webSocket, type, data, 0UL, (ulong)((data != null) ? ((long)data.Length) : 0L), isFinal, useExtensions)
		{
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0009E328 File Offset: 0x0009C528
		public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, ulong pos, ulong length, bool isFinal, bool useExtensions)
		{
			this.Type = type;
			this.IsFinal = isFinal;
			this.UseExtensions = useExtensions;
			this.DataLength = (int)length;
			if (data != null)
			{
				this.Data = VariableSizedBufferPool.Get((long)this.DataLength, true);
				Array.Copy(data, (int)pos, this.Data, 0, this.DataLength);
			}
			else
			{
				data = VariableSizedBufferPool.NoData;
			}
			byte b = this.IsFinal ? 128 : 0;
			this.Header = (b | (byte)this.Type);
			if (this.UseExtensions && webSocket != null && webSocket.Extensions != null)
			{
				for (int i = 0; i < webSocket.Extensions.Length; i++)
				{
					IExtension extension = webSocket.Extensions[i];
					if (extension != null)
					{
						this.Header |= extension.GetFrameHeader(this, this.Header);
						byte[] array = extension.Encode(this);
						if (array != this.Data)
						{
							VariableSizedBufferPool.Release(this.Data);
							this.Data = array;
							this.DataLength = array.Length;
						}
					}
				}
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0009E42C File Offset: 0x0009C62C
		public RawFrameData Get()
		{
			if (this.Data == null)
			{
				this.Data = VariableSizedBufferPool.NoData;
			}
			RawFrameData result;
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream(this.DataLength + 9))
			{
				bufferPoolMemoryStream.WriteByte(this.Header);
				if (this.DataLength < 126)
				{
					bufferPoolMemoryStream.WriteByte(128 | (byte)this.DataLength);
				}
				else if (this.DataLength < 65535)
				{
					bufferPoolMemoryStream.WriteByte(254);
					byte[] bytes = BitConverter.GetBytes((ushort)this.DataLength);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse(bytes, 0, bytes.Length);
					}
					bufferPoolMemoryStream.Write(bytes, 0, bytes.Length);
				}
				else
				{
					bufferPoolMemoryStream.WriteByte(byte.MaxValue);
					byte[] bytes2 = BitConverter.GetBytes((ulong)((long)this.DataLength));
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse(bytes2, 0, bytes2.Length);
					}
					bufferPoolMemoryStream.Write(bytes2, 0, bytes2.Length);
				}
				byte[] bytes3 = BitConverter.GetBytes(this.GetHashCode());
				bufferPoolMemoryStream.Write(bytes3, 0, bytes3.Length);
				for (int i = 0; i < this.DataLength; i++)
				{
					bufferPoolMemoryStream.WriteByte(this.Data[i] ^ bytes3[i % 4]);
				}
				result = new RawFrameData(bufferPoolMemoryStream.ToArray(true), (int)bufferPoolMemoryStream.Length);
			}
			return result;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0009E574 File Offset: 0x0009C774
		public WebSocketFrame[] Fragment(ushort maxFragmentSize)
		{
			if (this.Data == null)
			{
				return null;
			}
			if (this.Type != WebSocketFrameTypes.Binary && this.Type != WebSocketFrameTypes.Text)
			{
				return null;
			}
			if (this.DataLength <= (int)maxFragmentSize)
			{
				return null;
			}
			this.IsFinal = false;
			this.Header &= 127;
			int num = this.DataLength / (int)maxFragmentSize + ((this.DataLength % (int)maxFragmentSize == 0) ? -1 : 0);
			WebSocketFrame[] array = new WebSocketFrame[num];
			ulong num3;
			for (ulong num2 = (ulong)maxFragmentSize; num2 < (ulong)((long)this.DataLength); num2 += num3)
			{
				num3 = Math.Min((ulong)maxFragmentSize, (ulong)((long)this.DataLength - (long)num2));
				array[array.Length - num--] = new WebSocketFrame(null, WebSocketFrameTypes.Continuation, this.Data, num2, num3, num2 + num3 >= (ulong)((long)this.DataLength), false);
			}
			this.DataLength = (int)maxFragmentSize;
			return array;
		}
	}
}
