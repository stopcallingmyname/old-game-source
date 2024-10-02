using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x02000189 RID: 393
	public class HTTPResponse : IDisposable
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x000948D7 File Offset: 0x00092AD7
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x000948DF File Offset: 0x00092ADF
		public int VersionMajor { get; protected set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x000948E8 File Offset: 0x00092AE8
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x000948F0 File Offset: 0x00092AF0
		public int VersionMinor { get; protected set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x000948F9 File Offset: 0x00092AF9
		// (set) Token: 0x06000E4B RID: 3659 RVA: 0x00094901 File Offset: 0x00092B01
		public int StatusCode { get; protected set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0009490A File Offset: 0x00092B0A
		public bool IsSuccess
		{
			get
			{
				return (this.StatusCode >= 200 && this.StatusCode < 300) || this.StatusCode == 304;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x00094935 File Offset: 0x00092B35
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x0009493D File Offset: 0x00092B3D
		public string Message { get; protected set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x00094946 File Offset: 0x00092B46
		// (set) Token: 0x06000E50 RID: 3664 RVA: 0x0009494E File Offset: 0x00092B4E
		public bool IsStreamed { get; protected set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x00094957 File Offset: 0x00092B57
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x0009495F File Offset: 0x00092B5F
		public bool IsStreamingFinished { get; internal set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x00094968 File Offset: 0x00092B68
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x00094970 File Offset: 0x00092B70
		public bool IsFromCache { get; internal set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x00094979 File Offset: 0x00092B79
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x00094981 File Offset: 0x00092B81
		public HTTPCacheFileInfo CacheFileInfo { get; internal set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0009498A File Offset: 0x00092B8A
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00094992 File Offset: 0x00092B92
		public bool IsCacheOnly { get; private set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0009499B File Offset: 0x00092B9B
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x000949A3 File Offset: 0x00092BA3
		public Dictionary<string, List<string>> Headers { get; protected set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x000949AC File Offset: 0x00092BAC
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x000949B4 File Offset: 0x00092BB4
		public byte[] Data { get; internal set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x000949BD File Offset: 0x00092BBD
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x000949C5 File Offset: 0x00092BC5
		public bool IsUpgraded { get; protected set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x000949CE File Offset: 0x00092BCE
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x000949D6 File Offset: 0x00092BD6
		public List<Cookie> Cookies { get; internal set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x000949E0 File Offset: 0x00092BE0
		public string DataAsText
		{
			get
			{
				if (this.Data == null)
				{
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(this.dataAsText))
				{
					return this.dataAsText;
				}
				return this.dataAsText = Encoding.UTF8.GetString(this.Data, 0, this.Data.Length);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x00094A34 File Offset: 0x00092C34
		public Texture2D DataAsTexture2D
		{
			get
			{
				if (this.Data == null)
				{
					return null;
				}
				if (this.texture != null)
				{
					return this.texture;
				}
				this.texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
				this.texture.LoadImage(this.Data);
				return this.texture;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00094A87 File Offset: 0x00092C87
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x00094A8F File Offset: 0x00092C8F
		public bool IsClosedManually { get; protected set; }

		// Token: 0x06000E65 RID: 3685 RVA: 0x00094A98 File Offset: 0x00092C98
		public HTTPResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache)
		{
			this.baseRequest = request;
			this.Stream = stream;
			this.IsStreamed = isStreamed;
			if (this.IsStreamed)
			{
				this.rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
			}
			this.IsFromCache = isFromCache;
			this.IsCacheOnly = request.CacheOnly;
			this.IsClosedManually = false;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00094AF0 File Offset: 0x00092CF0
		public virtual bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
		{
			string text = string.Empty;
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("Receive. forceReadRawContentLength: '{0:N0}', readPayloadData: '{1:N0}'", forceReadRawContentLength, readPayloadData));
			}
			try
			{
				text = HTTPResponse.ReadTo(this.Stream, 32);
			}
			catch
			{
				if (!this.baseRequest.DisableRetry)
				{
					HTTPManager.Logger.Warning("HTTPResponse", string.Format("{0} - Failed to read Status Line! Retry is enabled, returning with false.", this.baseRequest.CurrentUri.ToString()));
					return false;
				}
				HTTPManager.Logger.Warning("HTTPResponse", string.Format("{0} - Failed to read Status Line! Retry is disabled, re-throwing exception.", this.baseRequest.CurrentUri.ToString()));
				throw;
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("Status Line: '{0}'", text));
			}
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[]
				{
					'/',
					'.'
				});
				this.VersionMajor = int.Parse(array[1]);
				this.VersionMinor = int.Parse(array[2]);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("HTTP Version: '{0}.{1}'", this.VersionMajor.ToString(), this.VersionMinor.ToString()));
				}
				string text2 = HTTPResponse.NoTrimReadTo(this.Stream, 32, 10);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("Status Code: '{0}'", text2));
				}
				int statusCode;
				if (this.baseRequest.DisableRetry)
				{
					statusCode = int.Parse(text2);
				}
				else if (!int.TryParse(text2, out statusCode))
				{
					return false;
				}
				this.StatusCode = statusCode;
				if (text2.Length > 0 && (byte)text2[text2.Length - 1] != 10 && (byte)text2[text2.Length - 1] != 13)
				{
					this.Message = HTTPResponse.ReadTo(this.Stream, 10);
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging(string.Format("Status Message: '{0}'", this.Message));
					}
				}
				else
				{
					HTTPManager.Logger.Warning("HTTPResponse", string.Format("{0} - Skipping Status Message reading!", this.baseRequest.CurrentUri.ToString()));
					this.Message = string.Empty;
				}
				this.ReadHeaders(this.Stream);
				this.IsUpgraded = (this.StatusCode == 101 && (this.HasHeaderWithValue("connection", "upgrade") || this.HasHeader("upgrade")));
				if (this.IsUpgraded && HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging("Request Upgraded!");
				}
				return !readPayloadData || this.ReadPayload(forceReadRawContentLength);
			}
			if (!this.baseRequest.DisableRetry)
			{
				return false;
			}
			throw new Exception("Remote server closed the connection before sending response header!");
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00094DC0 File Offset: 0x00092FC0
		protected bool ReadPayload(int forceReadRawContentLength)
		{
			if (forceReadRawContentLength != -1)
			{
				this.IsFromCache = true;
				this.ReadRaw(this.Stream, (long)forceReadRawContentLength);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging("ReadPayload Finished!");
				}
				return true;
			}
			if ((this.StatusCode >= 100 && this.StatusCode < 200) || this.StatusCode == 204 || this.StatusCode == 304 || this.baseRequest.MethodType == HTTPMethods.Head)
			{
				return true;
			}
			if (this.HasHeaderWithValue("transfer-encoding", "chunked"))
			{
				this.ReadChunked(this.Stream);
			}
			else
			{
				List<string> headerValues = this.GetHeaderValues("content-length");
				List<string> headerValues2 = this.GetHeaderValues("content-range");
				if (headerValues != null && headerValues2 == null)
				{
					this.ReadRaw(this.Stream, long.Parse(headerValues[0]));
				}
				else if (headerValues2 != null)
				{
					if (headerValues != null)
					{
						this.ReadRaw(this.Stream, long.Parse(headerValues[0]));
					}
					else
					{
						HTTPRange range = this.GetRange();
						this.ReadRaw(this.Stream, (long)(range.LastBytePos - range.FirstBytePos + 1));
					}
				}
				else
				{
					this.ReadUnknownSize(this.Stream);
				}
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging("ReadPayload Finished!");
			}
			return true;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00094F04 File Offset: 0x00093104
		protected void ReadHeaders(Stream stream)
		{
			string text = HTTPResponse.ReadTo(stream, 58, 10);
			while (text != string.Empty)
			{
				string text2 = HTTPResponse.ReadTo(stream, 10);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("Header - '{0}': '{1}'", text, text2));
				}
				this.AddHeader(text, text2);
				text = HTTPResponse.ReadTo(stream, 58, 10);
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00094F68 File Offset: 0x00093168
		protected void AddHeader(string name, string value)
		{
			name = name.ToLower();
			if (this.Headers == null)
			{
				this.Headers = new Dictionary<string, List<string>>();
			}
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list))
			{
				this.Headers.Add(name, list = new List<string>(1));
			}
			list.Add(value);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00094FBC File Offset: 0x000931BC
		public List<string> GetHeaderValues(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			name = name.ToLower();
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list) || list.Count == 0)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00094FF8 File Offset: 0x000931F8
		public string GetFirstHeaderValue(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			name = name.ToLower();
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list) || list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00095038 File Offset: 0x00093238
		public bool HasHeaderWithValue(string headerName, string value)
		{
			List<string> headerValues = this.GetHeaderValues(headerName);
			if (headerValues == null)
			{
				return false;
			}
			for (int i = 0; i < headerValues.Count; i++)
			{
				if (string.Compare(headerValues[i], value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00095076 File Offset: 0x00093276
		public bool HasHeader(string headerName)
		{
			return this.GetHeaderValues(headerName) != null;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00095084 File Offset: 0x00093284
		public HTTPRange GetRange()
		{
			List<string> headerValues = this.GetHeaderValues("content-range");
			if (headerValues == null)
			{
				return null;
			}
			string[] array = headerValues[0].Split(new char[]
			{
				' ',
				'-',
				'/'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array[1] == "*")
			{
				return new HTTPRange(int.Parse(array[2]));
			}
			return new HTTPRange(int.Parse(array[1]), int.Parse(array[2]), (array[3] != "*") ? int.Parse(array[3]) : -1);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00095110 File Offset: 0x00093310
		public static string ReadTo(Stream stream, byte blocker)
		{
			byte[] array = VariableSizedBufferPool.Get(1024L, true);
			string @string;
			try
			{
				int num = 0;
				for (int num2 = stream.ReadByte(); num2 != (int)blocker; num2 = stream.ReadByte())
				{
					if (num2 == -1)
					{
						break;
					}
					if (num2 > 127)
					{
						num2 = 63;
					}
					if (array.Length <= num)
					{
						VariableSizedBufferPool.Resize(ref array, array.Length * 2, true);
					}
					if (num > 0 || !char.IsWhiteSpace((char)num2))
					{
						array[num++] = (byte)num2;
					}
				}
				while (num > 0 && char.IsWhiteSpace((char)array[num - 1]))
				{
					num--;
				}
				@string = Encoding.UTF8.GetString(array, 0, num);
			}
			finally
			{
				VariableSizedBufferPool.Release(array);
			}
			return @string;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000951B4 File Offset: 0x000933B4
		public static string ReadTo(Stream stream, byte blocker1, byte blocker2)
		{
			byte[] array = VariableSizedBufferPool.Get(1024L, true);
			string @string;
			try
			{
				int num = 0;
				int num2 = stream.ReadByte();
				while (num2 != (int)blocker1 && num2 != (int)blocker2)
				{
					if (num2 == -1)
					{
						break;
					}
					if (num2 > 127)
					{
						num2 = 63;
					}
					if (array.Length <= num)
					{
						VariableSizedBufferPool.Resize(ref array, array.Length * 2, true);
					}
					if (num > 0 || !char.IsWhiteSpace((char)num2))
					{
						array[num++] = (byte)num2;
					}
					num2 = stream.ReadByte();
				}
				while (num > 0 && char.IsWhiteSpace((char)array[num - 1]))
				{
					num--;
				}
				@string = Encoding.UTF8.GetString(array, 0, num);
			}
			finally
			{
				VariableSizedBufferPool.Release(array);
			}
			return @string;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0009525C File Offset: 0x0009345C
		public static string NoTrimReadTo(Stream stream, byte blocker1, byte blocker2)
		{
			byte[] array = VariableSizedBufferPool.Get(1024L, true);
			string @string;
			try
			{
				int num = 0;
				int num2 = stream.ReadByte();
				while (num2 != (int)blocker1 && num2 != (int)blocker2 && num2 != -1)
				{
					if (num2 > 127)
					{
						num2 = 63;
					}
					if (array.Length <= num)
					{
						VariableSizedBufferPool.Resize(ref array, array.Length * 2, true);
					}
					if (num > 0 || !char.IsWhiteSpace((char)num2))
					{
						array[num++] = (byte)num2;
					}
					num2 = stream.ReadByte();
				}
				@string = Encoding.UTF8.GetString(array, 0, num);
			}
			finally
			{
				VariableSizedBufferPool.Release(array);
			}
			return @string;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x000952F0 File Offset: 0x000934F0
		protected int ReadChunkLength(Stream stream)
		{
			string text = HTTPResponse.ReadTo(stream, 10).Split(new char[]
			{
				';'
			})[0];
			int result;
			if (int.TryParse(text, NumberStyles.AllowHexSpecifier, null, out result))
			{
				return result;
			}
			throw new Exception(string.Format("Can't parse '{0}' as a hex number!", text));
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0009533C File Offset: 0x0009353C
		protected void ReadChunked(Stream stream)
		{
			this.BeginReceiveStreamFragments();
			string firstHeaderValue = this.GetFirstHeaderValue("Content-Length");
			bool flag = !string.IsNullOrEmpty(firstHeaderValue);
			int num = 0;
			if (flag)
			{
				flag = int.TryParse(firstHeaderValue, out num);
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("ReadChunked - hasContentLengthHeader: {0}, contentLengthHeader: {1} realLength: {2:N0}", flag.ToString(), firstHeaderValue, num));
			}
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream())
			{
				int num2 = this.ReadChunkLength(stream);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("chunkLength: {0:N0}", num2));
				}
				byte[] array = VariableSizedBufferPool.Get((long)Mathf.NextPowerOfTwo(num2), true);
				int num3 = 0;
				this.baseRequest.DownloadLength = (long)(flag ? num : num2);
				this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
				string text = this.IsFromCache ? null : this.GetFirstHeaderValue("content-encoding");
				bool flag2 = !string.IsNullOrEmpty(text) && text == "gzip";
				while (num2 != 0)
				{
					if (array.Length < num2)
					{
						VariableSizedBufferPool.Resize(ref array, num2, true);
					}
					int num4 = 0;
					do
					{
						int num5 = stream.Read(array, num4, num2 - num4);
						if (num5 <= 0)
						{
							goto Block_11;
						}
						num4 += num5;
						this.baseRequest.Downloaded += (long)num5;
						this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					}
					while (num4 < num2);
					if (this.baseRequest.UseStreaming)
					{
						if (flag2)
						{
							byte[] array2 = this.Decompress(array, 0, num4, false);
							if (array2 != null)
							{
								this.FeedStreamFragment(array2, 0, array2.Length);
							}
						}
						else
						{
							this.FeedStreamFragment(array, 0, num4);
						}
					}
					else
					{
						bufferPoolMemoryStream.Write(array, 0, num4);
					}
					HTTPResponse.ReadTo(stream, 10);
					num3 += num4;
					num2 = this.ReadChunkLength(stream);
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging(string.Format("chunkLength: {0:N0}", num2));
					}
					if (!flag)
					{
						this.baseRequest.DownloadLength += (long)num2;
					}
					this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					continue;
					Block_11:
					throw ExceptionHelper.ServerClosedTCPStream();
				}
				VariableSizedBufferPool.Release(array);
				if (this.baseRequest.UseStreaming)
				{
					if (flag2)
					{
						byte[] array3 = this.Decompress(null, 0, 0, true);
						if (array3 != null)
						{
							this.FeedStreamFragment(array3, 0, array3.Length);
						}
					}
					this.FlushRemainingFragmentBuffer();
				}
				this.ReadHeaders(stream);
				if (!this.baseRequest.UseStreaming)
				{
					this.Data = this.DecodeStream(bufferPoolMemoryStream);
				}
			}
			this.CloseDecompressors();
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00095604 File Offset: 0x00093804
		internal void ReadRaw(Stream stream, long contentLength)
		{
			this.BeginReceiveStreamFragments();
			this.baseRequest.DownloadLength = contentLength;
			this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging(string.Format("ReadRaw - contentLength: {0:N0}", contentLength));
			}
			string text = this.IsFromCache ? null : this.GetFirstHeaderValue("content-encoding");
			bool flag = !string.IsNullOrEmpty(text) && text == "gzip";
			if (!this.baseRequest.UseStreaming && contentLength > 2147483646L)
			{
				throw new OverflowException("You have to use STREAMING to download files bigger than 2GB!");
			}
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream(this.baseRequest.UseStreaming ? 0 : ((int)contentLength)))
			{
				byte[] array = VariableSizedBufferPool.Get((long)Math.Max(this.baseRequest.StreamFragmentSize, 4096), true);
				while (contentLength > 0L)
				{
					int num = 0;
					do
					{
						int val = (int)Math.Min(2147483646U, (uint)contentLength);
						int num2 = stream.Read(array, num, Math.Min(val, array.Length - num));
						if (num2 <= 0)
						{
							goto Block_10;
						}
						num += num2;
						contentLength -= (long)num2;
						this.baseRequest.Downloaded += (long)num2;
						this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					}
					while (num < array.Length && contentLength > 0L);
					if (!this.baseRequest.UseStreaming)
					{
						bufferPoolMemoryStream.Write(array, 0, num);
						continue;
					}
					if (!flag)
					{
						this.FeedStreamFragment(array, 0, num);
						continue;
					}
					byte[] array2 = this.Decompress(array, 0, num, false);
					if (array2 != null)
					{
						this.FeedStreamFragment(array2, 0, array2.Length);
						continue;
					}
					continue;
					Block_10:
					throw ExceptionHelper.ServerClosedTCPStream();
				}
				VariableSizedBufferPool.Release(array);
				if (this.baseRequest.UseStreaming)
				{
					if (flag)
					{
						byte[] array3 = this.Decompress(null, 0, 0, true);
						if (array3 != null)
						{
							this.FeedStreamFragment(array3, 0, array3.Length);
						}
					}
					this.FlushRemainingFragmentBuffer();
				}
				if (!this.baseRequest.UseStreaming)
				{
					this.Data = this.DecodeStream(bufferPoolMemoryStream);
				}
			}
			this.CloseDecompressors();
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0009583C File Offset: 0x00093A3C
		protected void ReadUnknownSize(Stream stream)
		{
			string text = this.IsFromCache ? null : this.GetFirstHeaderValue("content-encoding");
			bool flag = !string.IsNullOrEmpty(text) && text == "gzip";
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream())
			{
				byte[] array = VariableSizedBufferPool.Get((long)Math.Max(this.baseRequest.StreamFragmentSize, 4096), false);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("ReadUnknownSize - buffer size: {0:N0}", array.Length));
				}
				int num2;
				do
				{
					int num = 0;
					do
					{
						num2 = 0;
						NetworkStream networkStream = stream as NetworkStream;
						if (networkStream != null && this.baseRequest.EnableSafeReadOnUnknownContentLength)
						{
							for (int i = num; i < array.Length; i++)
							{
								if (!networkStream.DataAvailable)
								{
									break;
								}
								int num3 = stream.ReadByte();
								if (num3 < 0)
								{
									break;
								}
								array[i] = (byte)num3;
								num2++;
							}
						}
						else
						{
							num2 = stream.Read(array, num, array.Length - num);
						}
						num += num2;
						this.baseRequest.Downloaded += (long)num2;
						this.baseRequest.DownloadLength = this.baseRequest.Downloaded;
						this.baseRequest.DownloadProgressChanged = (this.IsSuccess || this.IsFromCache);
					}
					while (num < array.Length && num2 > 0);
					if (this.baseRequest.UseStreaming)
					{
						if (flag)
						{
							byte[] array2 = this.Decompress(array, 0, num, false);
							if (array2 != null)
							{
								this.FeedStreamFragment(array2, 0, array2.Length);
							}
						}
						else
						{
							this.FeedStreamFragment(array, 0, num);
						}
					}
					else
					{
						bufferPoolMemoryStream.Write(array, 0, num);
					}
				}
				while (num2 > 0);
				VariableSizedBufferPool.Release(array);
				if (this.baseRequest.UseStreaming)
				{
					if (flag)
					{
						byte[] array3 = this.Decompress(null, 0, 0, true);
						if (array3 != null)
						{
							this.FeedStreamFragment(array3, 0, array3.Length);
						}
					}
					this.FlushRemainingFragmentBuffer();
				}
				if (!this.baseRequest.UseStreaming)
				{
					this.Data = this.DecodeStream(bufferPoolMemoryStream);
				}
			}
			this.CloseDecompressors();
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00095A58 File Offset: 0x00093C58
		protected byte[] DecodeStream(BufferPoolMemoryStream streamToDecode)
		{
			streamToDecode.Seek(0L, SeekOrigin.Begin);
			List<string> list = this.IsFromCache ? null : this.GetHeaderValues("content-encoding");
			if (list == null)
			{
				return streamToDecode.ToArray();
			}
			string a = list[0];
			Stream stream;
			if (!(a == "gzip"))
			{
				if (!(a == "deflate"))
				{
					return streamToDecode.ToArray();
				}
				stream = new DeflateStream(streamToDecode, CompressionMode.Decompress);
			}
			else
			{
				stream = new GZipStream(streamToDecode, CompressionMode.Decompress);
			}
			byte[] result;
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream((int)streamToDecode.Length))
			{
				byte[] array = VariableSizedBufferPool.Get(1024L, true);
				int count;
				while ((count = stream.Read(array, 0, array.Length)) > 0)
				{
					bufferPoolMemoryStream.Write(array, 0, count);
				}
				VariableSizedBufferPool.Release(array);
				stream.Dispose();
				result = bufferPoolMemoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00095B44 File Offset: 0x00093D44
		private void CloseDecompressors()
		{
			if (this.decompressorGZipStream != null)
			{
				this.decompressorGZipStream.Dispose();
			}
			this.decompressorGZipStream = null;
			if (this.decompressorInputStream != null)
			{
				this.decompressorInputStream.Dispose();
			}
			this.decompressorInputStream = null;
			if (this.decompressorOutputStream != null)
			{
				this.decompressorOutputStream.Dispose();
			}
			this.decompressorOutputStream = null;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00095BA0 File Offset: 0x00093DA0
		private byte[] Decompress(byte[] data, int offset, int count, bool forceDecompress = false)
		{
			if (this.decompressorInputStream == null)
			{
				this.decompressorInputStream = new BufferPoolMemoryStream(count);
			}
			if (data != null)
			{
				this.decompressorInputStream.Write(data, offset, count);
			}
			if (!forceDecompress && this.decompressorInputStream.Length < 256L)
			{
				return null;
			}
			this.decompressorInputStream.Position = 0L;
			if (this.decompressorGZipStream == null)
			{
				this.decompressorGZipStream = new GZipStream(this.decompressorInputStream, CompressionMode.Decompress, BestHTTP.Decompression.Zlib.CompressionLevel.Default, true);
				this.decompressorGZipStream.FlushMode = FlushType.Sync;
			}
			if (this.decompressorOutputStream == null)
			{
				this.decompressorOutputStream = new BufferPoolMemoryStream();
			}
			this.decompressorOutputStream.SetLength(0L);
			byte[] array = VariableSizedBufferPool.Get(1024L, true);
			int count2;
			while ((count2 = this.decompressorGZipStream.Read(array, 0, array.Length)) != 0)
			{
				this.decompressorOutputStream.Write(array, 0, count2);
			}
			VariableSizedBufferPool.Release(array);
			this.decompressorGZipStream.SetLength(0L);
			return this.decompressorOutputStream.ToArray();
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00095C90 File Offset: 0x00093E90
		protected void BeginReceiveStreamFragments()
		{
			if (!this.baseRequest.DisableCache && this.baseRequest.UseStreaming && !this.IsFromCache && HTTPCacheService.IsCacheble(this.baseRequest.CurrentUri, this.baseRequest.MethodType, this))
			{
				this.cacheStream = HTTPCacheService.PrepareStreamed(this.baseRequest.CurrentUri, this);
			}
			this.allFragmentSize = 0;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00095CFC File Offset: 0x00093EFC
		protected void FeedStreamFragment(byte[] buffer, int pos, int length)
		{
			if (buffer == null || length == 0)
			{
				return;
			}
			this.WaitWhileFragmentQueueIsFull();
			if (this.fragmentBuffer == null)
			{
				this.fragmentBuffer = VariableSizedBufferPool.Get((long)this.baseRequest.StreamFragmentSize, false);
				this.fragmentBufferDataLength = 0;
			}
			if (this.fragmentBufferDataLength + length <= this.baseRequest.StreamFragmentSize)
			{
				Array.Copy(buffer, pos, this.fragmentBuffer, this.fragmentBufferDataLength, length);
				this.fragmentBufferDataLength += length;
				if (this.fragmentBufferDataLength == this.baseRequest.StreamFragmentSize)
				{
					this.AddStreamedFragment(this.fragmentBuffer);
					this.fragmentBuffer = null;
					this.fragmentBufferDataLength = 0;
					return;
				}
			}
			else
			{
				int num = this.baseRequest.StreamFragmentSize - this.fragmentBufferDataLength;
				this.FeedStreamFragment(buffer, pos, num);
				this.FeedStreamFragment(buffer, pos + num, length - num);
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00095DCC File Offset: 0x00093FCC
		protected void FlushRemainingFragmentBuffer()
		{
			if (this.fragmentBuffer != null)
			{
				VariableSizedBufferPool.Resize(ref this.fragmentBuffer, this.fragmentBufferDataLength, false);
				this.AddStreamedFragment(this.fragmentBuffer);
				this.fragmentBuffer = null;
				this.fragmentBufferDataLength = 0;
			}
			if (this.cacheStream != null)
			{
				this.cacheStream.Dispose();
				this.cacheStream = null;
				HTTPCacheService.SetBodyLength(this.baseRequest.CurrentUri, this.allFragmentSize);
			}
			AutoResetEvent autoResetEvent = this.fragmentWaitEvent;
			this.fragmentWaitEvent = null;
			if (autoResetEvent != null)
			{
				((IDisposable)autoResetEvent).Dispose();
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00095E5C File Offset: 0x0009405C
		protected void AddStreamedFragment(byte[] buffer)
		{
			this.rwLock.EnterWriteLock();
			try
			{
				if (!this.IsCacheOnly)
				{
					if (this.streamedFragments == null)
					{
						this.streamedFragments = new List<byte[]>();
					}
					this.streamedFragments.Add(buffer);
				}
				if (HTTPManager.Logger.Level == Loglevels.All && buffer != null && this.streamedFragments != null)
				{
					this.VerboseLogging(string.Format("AddStreamedFragment buffer length: {0:N0} streamedFragments: {1:N0}", buffer.Length, this.streamedFragments.Count));
				}
				if (this.cacheStream != null)
				{
					this.cacheStream.Write(buffer, 0, buffer.Length);
					this.allFragmentSize += buffer.Length;
				}
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00095F20 File Offset: 0x00094120
		protected void WaitWhileFragmentQueueIsFull()
		{
			while (this.baseRequest.State == HTTPRequestStates.Processing && this.baseRequest.UseStreaming && this.FragmentQueueIsFull())
			{
				this.VerboseLogging("WaitWhileFragmentQueueIsFull");
				if (this.fragmentWaitEvent == null)
				{
					this.fragmentWaitEvent = new AutoResetEvent(false);
				}
				this.fragmentWaitEvent.WaitOne(16);
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00095F88 File Offset: 0x00094188
		private bool FragmentQueueIsFull()
		{
			this.rwLock.EnterReadLock();
			bool result;
			try
			{
				bool flag = this.streamedFragments != null && this.streamedFragments.Count >= this.baseRequest.MaxFragmentQueueLength;
				if (flag && HTTPManager.Logger.Level == Loglevels.All)
				{
					this.VerboseLogging(string.Format("HasFragmentsInQueue - {0} / {1}", this.streamedFragments.Count, this.baseRequest.MaxFragmentQueueLength));
				}
				result = flag;
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00096028 File Offset: 0x00094228
		public List<byte[]> GetStreamedFragments()
		{
			this.rwLock.EnterWriteLock();
			List<byte[]> result;
			try
			{
				if (this.streamedFragments == null || this.streamedFragments.Count == 0)
				{
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging("GetStreamedFragments - no fragments, returning with null");
					}
					result = null;
				}
				else
				{
					List<byte[]> list = new List<byte[]>(this.streamedFragments);
					this.streamedFragments.Clear();
					if (this.fragmentWaitEvent != null)
					{
						this.fragmentWaitEvent.Set();
					}
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						this.VerboseLogging(string.Format("GetStreamedFragments - returning with {0:N0} fragments", list.Count.ToString()));
					}
					result = list;
				}
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
			return result;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x000960EC File Offset: 0x000942EC
		internal bool HasStreamedFragments()
		{
			this.rwLock.EnterReadLock();
			bool result;
			try
			{
				result = (this.streamedFragments != null && this.streamedFragments.Count >= this.baseRequest.MaxFragmentQueueLength);
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0009614C File Offset: 0x0009434C
		internal void FinishStreaming()
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				this.VerboseLogging("FinishStreaming");
			}
			this.IsStreamingFinished = true;
			this.Dispose();
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00096172 File Offset: 0x00094372
		private void VerboseLogging(string str)
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPResponse", "'" + this.baseRequest.CurrentUri.ToString() + "' - " + str);
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x000961B0 File Offset: 0x000943B0
		public void Dispose()
		{
			if (this.Stream != null && this.Stream is ReadOnlyBufferedStream)
			{
				((IDisposable)this.Stream).Dispose();
			}
			this.Stream = null;
			if (this.cacheStream != null)
			{
				this.cacheStream.Dispose();
				this.cacheStream = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x04001328 RID: 4904
		internal const byte CR = 13;

		// Token: 0x04001329 RID: 4905
		internal const byte LF = 10;

		// Token: 0x0400132A RID: 4906
		public const int MinBufferSize = 4096;

		// Token: 0x04001338 RID: 4920
		protected string dataAsText;

		// Token: 0x04001339 RID: 4921
		protected Texture2D texture;

		// Token: 0x0400133B RID: 4923
		internal HTTPRequest baseRequest;

		// Token: 0x0400133C RID: 4924
		protected Stream Stream;

		// Token: 0x0400133D RID: 4925
		protected List<byte[]> streamedFragments;

		// Token: 0x0400133E RID: 4926
		private ReaderWriterLockSlim rwLock;

		// Token: 0x0400133F RID: 4927
		protected byte[] fragmentBuffer;

		// Token: 0x04001340 RID: 4928
		protected int fragmentBufferDataLength;

		// Token: 0x04001341 RID: 4929
		protected Stream cacheStream;

		// Token: 0x04001342 RID: 4930
		protected int allFragmentSize;

		// Token: 0x04001343 RID: 4931
		private BufferPoolMemoryStream decompressorInputStream;

		// Token: 0x04001344 RID: 4932
		private BufferPoolMemoryStream decompressorOutputStream;

		// Token: 0x04001345 RID: 4933
		private GZipStream decompressorGZipStream;

		// Token: 0x04001346 RID: 4934
		private const int MinLengthToDecompress = 256;

		// Token: 0x04001347 RID: 4935
		private volatile AutoResetEvent fragmentWaitEvent;
	}
}
