using System;
using System.Collections.Generic;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket.Extensions
{
	// Token: 0x020001BF RID: 447
	public sealed class PerMessageCompression : IExtension
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x0009EA58 File Offset: 0x0009CC58
		// (set) Token: 0x06001089 RID: 4233 RVA: 0x0009EA60 File Offset: 0x0009CC60
		public bool ClientNoContextTakeover { get; private set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0009EA69 File Offset: 0x0009CC69
		// (set) Token: 0x0600108B RID: 4235 RVA: 0x0009EA71 File Offset: 0x0009CC71
		public bool ServerNoContextTakeover { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x0009EA7A File Offset: 0x0009CC7A
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x0009EA82 File Offset: 0x0009CC82
		public int ClientMaxWindowBits { get; private set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x0009EA8B File Offset: 0x0009CC8B
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x0009EA93 File Offset: 0x0009CC93
		public int ServerMaxWindowBits { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x0009EA9C File Offset: 0x0009CC9C
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x0009EAA4 File Offset: 0x0009CCA4
		public CompressionLevel Level { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x0009EAAD File Offset: 0x0009CCAD
		// (set) Token: 0x06001093 RID: 4243 RVA: 0x0009EAB5 File Offset: 0x0009CCB5
		public int MinimumDataLegthToCompress { get; set; }

		// Token: 0x06001094 RID: 4244 RVA: 0x0009EABE File Offset: 0x0009CCBE
		public PerMessageCompression() : this(CompressionLevel.Default, false, false, 15, 15, 256)
		{
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0009EAD2 File Offset: 0x0009CCD2
		public PerMessageCompression(CompressionLevel level, bool clientNoContextTakeover, bool serverNoContextTakeover, int desiredClientMaxWindowBits, int desiredServerMaxWindowBits, int minDatalengthToCompress)
		{
			this.Level = level;
			this.ClientNoContextTakeover = clientNoContextTakeover;
			this.ServerNoContextTakeover = this.ServerNoContextTakeover;
			this.ClientMaxWindowBits = desiredClientMaxWindowBits;
			this.ServerMaxWindowBits = desiredServerMaxWindowBits;
			this.MinimumDataLegthToCompress = minDatalengthToCompress;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0009EB0C File Offset: 0x0009CD0C
		public void AddNegotiation(HTTPRequest request)
		{
			string text = "permessage-deflate";
			if (this.ServerNoContextTakeover)
			{
				text += "; server_no_context_takeover";
			}
			if (this.ClientNoContextTakeover)
			{
				text += "; client_no_context_takeover";
			}
			if (this.ServerMaxWindowBits != 15)
			{
				text = text + "; server_max_window_bits=" + this.ServerMaxWindowBits.ToString();
			}
			else
			{
				this.ServerMaxWindowBits = 15;
			}
			if (this.ClientMaxWindowBits != 15)
			{
				text = text + "; client_max_window_bits=" + this.ClientMaxWindowBits.ToString();
			}
			else
			{
				text += "; client_max_window_bits";
				this.ClientMaxWindowBits = 15;
			}
			request.AddHeader("Sec-WebSocket-Extensions", text);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0009EBBC File Offset: 0x0009CDBC
		public bool ParseNegotiation(WebSocketResponse resp)
		{
			List<string> headerValues = resp.GetHeaderValues("Sec-WebSocket-Extensions");
			if (headerValues == null)
			{
				return false;
			}
			for (int i = 0; i < headerValues.Count; i++)
			{
				HeaderParser headerParser = new HeaderParser(headerValues[i]);
				for (int j = 0; j < headerParser.Values.Count; j++)
				{
					HeaderValue headerValue = headerParser.Values[i];
					if (!string.IsNullOrEmpty(headerValue.Key) && headerValue.Key.StartsWith("permessage-deflate", StringComparison.OrdinalIgnoreCase))
					{
						HTTPManager.Logger.Information("PerMessageCompression", "Enabled with header: " + headerValues[i]);
						HeaderValue headerValue2;
						if (headerValue.TryGetOption("client_no_context_takeover", out headerValue2))
						{
							this.ClientNoContextTakeover = true;
						}
						if (headerValue.TryGetOption("server_no_context_takeover", out headerValue2))
						{
							this.ServerNoContextTakeover = true;
						}
						int clientMaxWindowBits;
						if (headerValue.TryGetOption("client_max_window_bits", out headerValue2) && headerValue2.HasValue && int.TryParse(headerValue2.Value, out clientMaxWindowBits))
						{
							this.ClientMaxWindowBits = clientMaxWindowBits;
						}
						int serverMaxWindowBits;
						if (headerValue.TryGetOption("server_max_window_bits", out headerValue2) && headerValue2.HasValue && int.TryParse(headerValue2.Value, out serverMaxWindowBits))
						{
							this.ServerMaxWindowBits = serverMaxWindowBits;
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0009ED03 File Offset: 0x0009CF03
		public byte GetFrameHeader(WebSocketFrame writer, byte inFlag)
		{
			if ((writer.Type == WebSocketFrameTypes.Binary || writer.Type == WebSocketFrameTypes.Text) && writer.Data != null && writer.Data.Length >= this.MinimumDataLegthToCompress)
			{
				return inFlag | 64;
			}
			return inFlag;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0009ED36 File Offset: 0x0009CF36
		public byte[] Encode(WebSocketFrame writer)
		{
			if (writer.Data == null)
			{
				return VariableSizedBufferPool.NoData;
			}
			if ((writer.Header & 64) != 0)
			{
				return this.Compress(writer.Data, writer.DataLength);
			}
			return writer.Data;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0009ED6A File Offset: 0x0009CF6A
		public byte[] Decode(byte header, byte[] data, int length)
		{
			if ((header & 64) != 0)
			{
				return this.Decompress(data, length);
			}
			return data;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0009ED7C File Offset: 0x0009CF7C
		private byte[] Compress(byte[] data, int length)
		{
			if (this.compressorOutputStream == null)
			{
				this.compressorOutputStream = new BufferPoolMemoryStream();
			}
			this.compressorOutputStream.SetLength(0L);
			if (this.compressorDeflateStream == null)
			{
				this.compressorDeflateStream = new DeflateStream(this.compressorOutputStream, CompressionMode.Compress, this.Level, true, this.ClientMaxWindowBits);
				this.compressorDeflateStream.FlushMode = FlushType.Sync;
			}
			byte[] result = null;
			try
			{
				this.compressorDeflateStream.Write(data, 0, length);
				this.compressorDeflateStream.Flush();
				this.compressorOutputStream.Position = 0L;
				this.compressorOutputStream.SetLength(this.compressorOutputStream.Length - 4L);
				result = this.compressorOutputStream.ToArray();
			}
			finally
			{
				if (this.ClientNoContextTakeover)
				{
					this.compressorDeflateStream.Dispose();
					this.compressorDeflateStream = null;
				}
			}
			return result;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0009EE58 File Offset: 0x0009D058
		private byte[] Decompress(byte[] data, int length)
		{
			if (this.decompressorInputStream == null)
			{
				this.decompressorInputStream = new BufferPoolMemoryStream(data.Length + 4);
			}
			this.decompressorInputStream.Write(data, 0, length);
			this.decompressorInputStream.Write(PerMessageCompression.Trailer, 0, PerMessageCompression.Trailer.Length);
			this.decompressorInputStream.Position = 0L;
			if (this.decompressorDeflateStream == null)
			{
				this.decompressorDeflateStream = new DeflateStream(this.decompressorInputStream, CompressionMode.Decompress, CompressionLevel.Default, true, this.ServerMaxWindowBits);
				this.decompressorDeflateStream.FlushMode = FlushType.Sync;
			}
			if (this.decompressorOutputStream == null)
			{
				this.decompressorOutputStream = new BufferPoolMemoryStream();
			}
			this.decompressorOutputStream.SetLength(0L);
			byte[] array = VariableSizedBufferPool.Get(1024L, true);
			int count;
			while ((count = this.decompressorDeflateStream.Read(array, 0, array.Length)) != 0)
			{
				this.decompressorOutputStream.Write(array, 0, count);
			}
			VariableSizedBufferPool.Release(array);
			this.decompressorDeflateStream.SetLength(0L);
			byte[] result = this.decompressorOutputStream.ToArray();
			if (this.ServerNoContextTakeover)
			{
				this.decompressorDeflateStream.Dispose();
				this.decompressorDeflateStream = null;
			}
			return result;
		}

		// Token: 0x0400144D RID: 5197
		public const int MinDataLengthToCompressDefault = 256;

		// Token: 0x0400144E RID: 5198
		private static readonly byte[] Trailer = new byte[]
		{
			0,
			0,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x04001455 RID: 5205
		private BufferPoolMemoryStream compressorOutputStream;

		// Token: 0x04001456 RID: 5206
		private DeflateStream compressorDeflateStream;

		// Token: 0x04001457 RID: 5207
		private BufferPoolMemoryStream decompressorInputStream;

		// Token: 0x04001458 RID: 5208
		private BufferPoolMemoryStream decompressorOutputStream;

		// Token: 0x04001459 RID: 5209
		private DeflateStream decompressorDeflateStream;
	}
}
