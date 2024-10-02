using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP.Extensions;
using BestHTTP.PlatformSupport.FileSystem;

namespace BestHTTP.Caching
{
	// Token: 0x02000816 RID: 2070
	public class HTTPCacheFileInfo : IComparable<HTTPCacheFileInfo>
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x060049AB RID: 18859 RVA: 0x001A2C16 File Offset: 0x001A0E16
		// (set) Token: 0x060049AC RID: 18860 RVA: 0x001A2C1E File Offset: 0x001A0E1E
		internal Uri Uri { get; set; }

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x060049AD RID: 18861 RVA: 0x001A2C27 File Offset: 0x001A0E27
		// (set) Token: 0x060049AE RID: 18862 RVA: 0x001A2C2F File Offset: 0x001A0E2F
		internal DateTime LastAccess { get; set; }

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x060049AF RID: 18863 RVA: 0x001A2C38 File Offset: 0x001A0E38
		// (set) Token: 0x060049B0 RID: 18864 RVA: 0x001A2C40 File Offset: 0x001A0E40
		public int BodyLength { get; set; }

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x060049B1 RID: 18865 RVA: 0x001A2C49 File Offset: 0x001A0E49
		// (set) Token: 0x060049B2 RID: 18866 RVA: 0x001A2C51 File Offset: 0x001A0E51
		private string ETag { get; set; }

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x060049B3 RID: 18867 RVA: 0x001A2C5A File Offset: 0x001A0E5A
		// (set) Token: 0x060049B4 RID: 18868 RVA: 0x001A2C62 File Offset: 0x001A0E62
		private string LastModified { get; set; }

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x060049B5 RID: 18869 RVA: 0x001A2C6B File Offset: 0x001A0E6B
		// (set) Token: 0x060049B6 RID: 18870 RVA: 0x001A2C73 File Offset: 0x001A0E73
		private DateTime Expires { get; set; }

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x060049B7 RID: 18871 RVA: 0x001A2C7C File Offset: 0x001A0E7C
		// (set) Token: 0x060049B8 RID: 18872 RVA: 0x001A2C84 File Offset: 0x001A0E84
		private long Age { get; set; }

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x001A2C8D File Offset: 0x001A0E8D
		// (set) Token: 0x060049BA RID: 18874 RVA: 0x001A2C95 File Offset: 0x001A0E95
		private long MaxAge { get; set; }

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x001A2C9E File Offset: 0x001A0E9E
		// (set) Token: 0x060049BC RID: 18876 RVA: 0x001A2CA6 File Offset: 0x001A0EA6
		private DateTime Date { get; set; }

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x001A2CAF File Offset: 0x001A0EAF
		// (set) Token: 0x060049BE RID: 18878 RVA: 0x001A2CB7 File Offset: 0x001A0EB7
		private bool MustRevalidate { get; set; }

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x060049BF RID: 18879 RVA: 0x001A2CC0 File Offset: 0x001A0EC0
		// (set) Token: 0x060049C0 RID: 18880 RVA: 0x001A2CC8 File Offset: 0x001A0EC8
		private DateTime Received { get; set; }

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x001A2CD1 File Offset: 0x001A0ED1
		// (set) Token: 0x060049C2 RID: 18882 RVA: 0x001A2CD9 File Offset: 0x001A0ED9
		private string ConstructedPath { get; set; }

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x001A2CE2 File Offset: 0x001A0EE2
		// (set) Token: 0x060049C4 RID: 18884 RVA: 0x001A2CEA File Offset: 0x001A0EEA
		internal ulong MappedNameIDX { get; set; }

		// Token: 0x060049C5 RID: 18885 RVA: 0x001A2CF3 File Offset: 0x001A0EF3
		internal HTTPCacheFileInfo(Uri uri) : this(uri, DateTime.UtcNow, -1)
		{
		}

		// Token: 0x060049C6 RID: 18886 RVA: 0x001A2D02 File Offset: 0x001A0F02
		internal HTTPCacheFileInfo(Uri uri, DateTime lastAcces, int bodyLength)
		{
			this.Uri = uri;
			this.LastAccess = lastAcces;
			this.BodyLength = bodyLength;
			this.MaxAge = -1L;
			this.MappedNameIDX = HTTPCacheService.GetNameIdx();
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x001A2D34 File Offset: 0x001A0F34
		internal HTTPCacheFileInfo(Uri uri, BinaryReader reader, int version)
		{
			this.Uri = uri;
			this.LastAccess = DateTime.FromBinary(reader.ReadInt64());
			this.BodyLength = reader.ReadInt32();
			if (version != 1)
			{
				if (version != 2)
				{
					return;
				}
				this.MappedNameIDX = reader.ReadUInt64();
			}
			this.ETag = reader.ReadString();
			this.LastModified = reader.ReadString();
			this.Expires = DateTime.FromBinary(reader.ReadInt64());
			this.Age = reader.ReadInt64();
			this.MaxAge = reader.ReadInt64();
			this.Date = DateTime.FromBinary(reader.ReadInt64());
			this.MustRevalidate = reader.ReadBoolean();
			this.Received = DateTime.FromBinary(reader.ReadInt64());
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x001A2DF0 File Offset: 0x001A0FF0
		internal void SaveTo(BinaryWriter writer)
		{
			writer.Write(this.LastAccess.ToBinary());
			writer.Write(this.BodyLength);
			writer.Write(this.MappedNameIDX);
			writer.Write(this.ETag);
			writer.Write(this.LastModified);
			writer.Write(this.Expires.ToBinary());
			writer.Write(this.Age);
			writer.Write(this.MaxAge);
			writer.Write(this.Date.ToBinary());
			writer.Write(this.MustRevalidate);
			writer.Write(this.Received.ToBinary());
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x001A2EA4 File Offset: 0x001A10A4
		public string GetPath()
		{
			if (this.ConstructedPath != null)
			{
				return this.ConstructedPath;
			}
			return this.ConstructedPath = Path.Combine(HTTPCacheService.CacheFolder, this.MappedNameIDX.ToString("X"));
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x001A2EE6 File Offset: 0x001A10E6
		public bool IsExists()
		{
			return HTTPCacheService.IsSupported && HTTPManager.IOService.FileExists(this.GetPath());
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x001A2F04 File Offset: 0x001A1104
		internal void Delete()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			string path = this.GetPath();
			try
			{
				HTTPManager.IOService.FileDelete(path);
			}
			catch
			{
			}
			finally
			{
				this.Reset();
			}
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x001A2F54 File Offset: 0x001A1154
		private void Reset()
		{
			this.BodyLength = -1;
			this.ETag = string.Empty;
			this.Expires = DateTime.FromBinary(0L);
			this.LastModified = string.Empty;
			this.Age = 0L;
			this.MaxAge = -1L;
			this.Date = DateTime.FromBinary(0L);
			this.MustRevalidate = false;
			this.Received = DateTime.FromBinary(0L);
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x001A2FBC File Offset: 0x001A11BC
		private void SetUpCachingValues(HTTPResponse response)
		{
			response.CacheFileInfo = this;
			this.ETag = response.GetFirstHeaderValue("ETag").ToStrOrEmpty();
			this.Expires = response.GetFirstHeaderValue("Expires").ToDateTime(DateTime.FromBinary(0L));
			this.LastModified = response.GetFirstHeaderValue("Last-Modified").ToStrOrEmpty();
			this.Age = response.GetFirstHeaderValue("Age").ToInt64(0L);
			this.Date = response.GetFirstHeaderValue("Date").ToDateTime(DateTime.FromBinary(0L));
			string firstHeaderValue = response.GetFirstHeaderValue("cache-control");
			if (!string.IsNullOrEmpty(firstHeaderValue))
			{
				string[] array = firstHeaderValue.FindOption("max-age");
				double num;
				if (array != null && double.TryParse(array[1], out num))
				{
					this.MaxAge = (long)((int)num);
				}
				this.MustRevalidate = firstHeaderValue.ToLower().Contains("must-revalidate");
			}
			this.Received = DateTime.UtcNow;
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x001A30A8 File Offset: 0x001A12A8
		internal bool WillExpireInTheFuture()
		{
			if (!this.IsExists())
			{
				return false;
			}
			if (this.MustRevalidate)
			{
				return false;
			}
			if (this.MaxAge != -1L)
			{
				long num = Math.Max(Math.Max(0L, (long)(this.Received - this.Date).TotalSeconds), this.Age);
				long num2 = (long)(DateTime.UtcNow - this.Date).TotalSeconds;
				return num + num2 < this.MaxAge;
			}
			return this.Expires > DateTime.UtcNow;
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x001A3134 File Offset: 0x001A1334
		internal void SetUpRevalidationHeaders(HTTPRequest request)
		{
			if (!this.IsExists())
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.ETag))
			{
				request.SetHeader("If-None-Match", this.ETag);
			}
			if (!string.IsNullOrEmpty(this.LastModified))
			{
				request.SetHeader("If-Modified-Since", this.LastModified);
			}
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x001A3186 File Offset: 0x001A1386
		public Stream GetBodyStream(out int length)
		{
			if (!this.IsExists())
			{
				length = 0;
				return null;
			}
			length = this.BodyLength;
			this.LastAccess = DateTime.UtcNow;
			Stream stream = HTTPManager.IOService.CreateFileStream(this.GetPath(), FileStreamModes.Open);
			stream.Seek((long)(-(long)length), SeekOrigin.End);
			return stream;
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x001A31C8 File Offset: 0x001A13C8
		internal HTTPResponse ReadResponseTo(HTTPRequest request)
		{
			if (!this.IsExists())
			{
				return null;
			}
			this.LastAccess = DateTime.UtcNow;
			HTTPResponse result;
			using (Stream stream = HTTPManager.IOService.CreateFileStream(this.GetPath(), FileStreamModes.Open))
			{
				HTTPResponse httpresponse = new HTTPResponse(request, stream, request.UseStreaming, true);
				httpresponse.CacheFileInfo = this;
				httpresponse.Receive(this.BodyLength, true);
				result = httpresponse;
			}
			return result;
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x001A3240 File Offset: 0x001A1440
		internal void Store(HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			string path = this.GetPath();
			if (path.Length > HTTPManager.MaxPathLength)
			{
				return;
			}
			if (HTTPManager.IOService.FileExists(path))
			{
				this.Delete();
			}
			using (Stream stream = HTTPManager.IOService.CreateFileStream(this.GetPath(), FileStreamModes.Create))
			{
				stream.WriteLine("HTTP/1.1 {0} {1}", new object[]
				{
					response.StatusCode,
					response.Message
				});
				foreach (KeyValuePair<string, List<string>> keyValuePair in response.Headers)
				{
					for (int i = 0; i < keyValuePair.Value.Count; i++)
					{
						stream.WriteLine("{0}: {1}", new object[]
						{
							keyValuePair.Key,
							keyValuePair.Value[i]
						});
					}
				}
				stream.WriteLine();
				stream.Write(response.Data, 0, response.Data.Length);
			}
			this.BodyLength = response.Data.Length;
			this.LastAccess = DateTime.UtcNow;
			this.SetUpCachingValues(response);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x001A3390 File Offset: 0x001A1590
		internal Stream GetSaveStream(HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			this.LastAccess = DateTime.UtcNow;
			string path = this.GetPath();
			if (HTTPManager.IOService.FileExists(path))
			{
				this.Delete();
			}
			if (path.Length > HTTPManager.MaxPathLength)
			{
				return null;
			}
			using (Stream stream = HTTPManager.IOService.CreateFileStream(this.GetPath(), FileStreamModes.Create))
			{
				stream.WriteLine("HTTP/1.1 {0} {1}", new object[]
				{
					response.StatusCode,
					response.Message
				});
				foreach (KeyValuePair<string, List<string>> keyValuePair in response.Headers)
				{
					for (int i = 0; i < keyValuePair.Value.Count; i++)
					{
						stream.WriteLine("{0}: {1}", new object[]
						{
							keyValuePair.Key,
							keyValuePair.Value[i]
						});
					}
				}
				stream.WriteLine();
			}
			if (response.IsFromCache && !response.Headers.ContainsKey("content-length"))
			{
				response.Headers.Add("content-length", new List<string>
				{
					this.BodyLength.ToString()
				});
			}
			this.SetUpCachingValues(response);
			return HTTPManager.IOService.CreateFileStream(this.GetPath(), FileStreamModes.Append);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x001A3514 File Offset: 0x001A1714
		public int CompareTo(HTTPCacheFileInfo other)
		{
			return this.LastAccess.CompareTo(other.LastAccess);
		}
	}
}
