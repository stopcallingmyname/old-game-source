using System;
using System.IO;
using System.Text;
using BestHTTP.Authentication;
using BestHTTP.Extensions;
using BestHTTP.Logger;

namespace BestHTTP
{
	// Token: 0x0200017E RID: 382
	public sealed class HTTPProxy : Proxy
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00092D0E File Offset: 0x00090F0E
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x00092D16 File Offset: 0x00090F16
		public bool IsTransparent { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x00092D1F File Offset: 0x00090F1F
		// (set) Token: 0x06000D81 RID: 3457 RVA: 0x00092D27 File Offset: 0x00090F27
		public bool SendWholeUri { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00092D30 File Offset: 0x00090F30
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x00092D38 File Offset: 0x00090F38
		public bool NonTransparentForHTTPS { get; set; }

		// Token: 0x06000D84 RID: 3460 RVA: 0x00092D41 File Offset: 0x00090F41
		public HTTPProxy(Uri address) : this(address, null, false)
		{
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00092D4C File Offset: 0x00090F4C
		public HTTPProxy(Uri address, Credentials credentials) : this(address, credentials, false)
		{
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00092D57 File Offset: 0x00090F57
		public HTTPProxy(Uri address, Credentials credentials, bool isTransparent) : this(address, credentials, isTransparent, true)
		{
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00092D63 File Offset: 0x00090F63
		public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri) : this(address, credentials, isTransparent, sendWholeUri, true)
		{
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00092D71 File Offset: 0x00090F71
		public HTTPProxy(Uri address, Credentials credentials, bool isTransparent, bool sendWholeUri, bool nonTransparentForHTTPS) : base(address, credentials)
		{
			this.IsTransparent = isTransparent;
			this.SendWholeUri = sendWholeUri;
			this.NonTransparentForHTTPS = nonTransparentForHTTPS;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00092D92 File Offset: 0x00090F92
		internal override string GetRequestPath(Uri uri)
		{
			if (!this.SendWholeUri)
			{
				return uri.GetRequestPathAndQueryURL();
			}
			return uri.OriginalString;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00092DAC File Offset: 0x00090FAC
		internal override void Connect(Stream stream, HTTPRequest request)
		{
			bool flag = HTTPProtocolFactory.IsSecureProtocol(request.CurrentUri);
			if (!this.IsTransparent || (flag && this.NonTransparentForHTTPS))
			{
				using (WriteOnlyBufferedStream writeOnlyBufferedStream = new WriteOnlyBufferedStream(stream, HTTPRequest.UploadChunkSize))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(writeOnlyBufferedStream, Encoding.UTF8))
					{
						for (;;)
						{
							bool flag2 = false;
							string text = string.Format("CONNECT {0}:{1} HTTP/1.1", request.CurrentUri.Host, request.CurrentUri.Port.ToString());
							HTTPManager.Logger.Information("HTTPConnection", "Sending " + text);
							binaryWriter.SendAsASCII(text);
							binaryWriter.Write(HTTPRequest.EOL);
							binaryWriter.SendAsASCII("Proxy-Connection: Keep-Alive");
							binaryWriter.Write(HTTPRequest.EOL);
							binaryWriter.SendAsASCII("Connection: Keep-Alive");
							binaryWriter.Write(HTTPRequest.EOL);
							binaryWriter.SendAsASCII(string.Format("Host: {0}:{1}", request.CurrentUri.Host, request.CurrentUri.Port.ToString()));
							binaryWriter.Write(HTTPRequest.EOL);
							if (base.Credentials != null)
							{
								switch (base.Credentials.Type)
								{
								case AuthenticationTypes.Unknown:
								case AuthenticationTypes.Digest:
								{
									Digest digest = DigestStore.Get(base.Address);
									if (digest != null)
									{
										string text2 = digest.GenerateResponseHeader(request, base.Credentials, true);
										if (!string.IsNullOrEmpty(text2))
										{
											string text3 = string.Format("Proxy-Authorization: {0}", text2);
											if (HTTPManager.Logger.Level <= Loglevels.Information)
											{
												HTTPManager.Logger.Information("HTTPConnection", "Sending proxy authorization header: " + text3);
											}
											byte[] asciibytes = text3.GetASCIIBytes();
											binaryWriter.Write(asciibytes);
											binaryWriter.Write(HTTPRequest.EOL);
											VariableSizedBufferPool.Release(asciibytes);
										}
									}
									break;
								}
								case AuthenticationTypes.Basic:
									binaryWriter.Write(string.Format("Proxy-Authorization: {0}", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(base.Credentials.UserName + ":" + base.Credentials.Password))).GetASCIIBytes());
									binaryWriter.Write(HTTPRequest.EOL);
									break;
								}
							}
							binaryWriter.Write(HTTPRequest.EOL);
							binaryWriter.Flush();
							request.ProxyResponse = new HTTPResponse(request, stream, false, false);
							if (!request.ProxyResponse.Receive(-1, true))
							{
								break;
							}
							if (HTTPManager.Logger.Level <= Loglevels.Information)
							{
								HTTPManager.Logger.Information("HTTPConnection", string.Concat(new object[]
								{
									"Proxy returned - status code: ",
									request.ProxyResponse.StatusCode,
									" message: ",
									request.ProxyResponse.Message,
									" Body: ",
									request.ProxyResponse.DataAsText
								}));
							}
							int statusCode = request.ProxyResponse.StatusCode;
							if (statusCode == 407)
							{
								string text4 = DigestStore.FindBest(request.ProxyResponse.GetHeaderValues("proxy-authenticate"));
								if (!string.IsNullOrEmpty(text4))
								{
									Digest orCreate = DigestStore.GetOrCreate(base.Address);
									orCreate.ParseChallange(text4);
									if (base.Credentials != null && orCreate.IsUriProtected(base.Address) && (!request.HasHeader("Proxy-Authorization") || orCreate.Stale))
									{
										flag2 = true;
									}
								}
								if (!flag2)
								{
									goto Block_18;
								}
							}
							else if (!request.ProxyResponse.IsSuccess)
							{
								goto Block_19;
							}
							if (!flag2)
							{
								goto Block_20;
							}
						}
						throw new Exception("Connection to the Proxy Server failed!");
						Block_18:
						throw new Exception(string.Format("Can't authenticate Proxy! Status Code: \"{0}\", Message: \"{1}\" and Response: {2}", request.ProxyResponse.StatusCode, request.ProxyResponse.Message, request.ProxyResponse.DataAsText));
						Block_19:
						throw new Exception(string.Format("Proxy returned Status Code: \"{0}\", Message: \"{1}\" and Response: {2}", request.ProxyResponse.StatusCode, request.ProxyResponse.Message, request.ProxyResponse.DataAsText));
						Block_20:;
					}
				}
			}
		}
	}
}
