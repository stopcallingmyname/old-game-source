using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using BestHTTP.Authentication;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using BestHTTP.PlatformSupport.TcpClient.General;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Tls;

namespace BestHTTP
{
	// Token: 0x02000177 RID: 375
	internal sealed class HTTPConnection : ConnectionBase
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00090C60 File Offset: 0x0008EE60
		public override bool IsRemovable
		{
			get
			{
				return base.IsRemovable || (base.IsFree && this.KeepAlive != null && DateTime.UtcNow - this.LastProcessTime >= this.KeepAlive.TimeOut);
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00090800 File Offset: 0x0008EA00
		internal HTTPConnection(string serverAddress) : base(serverAddress)
		{
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00090CAC File Offset: 0x0008EEAC
		protected override void ThreadFunc()
		{
			bool flag = false;
			bool flag2 = false;
			RetryCauses retryCauses = RetryCauses.None;
			try
			{
				if (!this.TryLoadAllFromCache())
				{
					if (this.Client != null && !this.Client.IsConnected())
					{
						this.Close();
					}
					for (;;)
					{
						if (retryCauses == RetryCauses.Reconnect)
						{
							this.Close();
							Thread.Sleep(100);
						}
						base.LastProcessedUri = base.CurrentRequest.CurrentUri;
						retryCauses = RetryCauses.None;
						this.Connect();
						if (base.State == HTTPConnectionStates.AbortRequested)
						{
							break;
						}
						if (!base.CurrentRequest.DisableCache)
						{
							HTTPCacheService.SetHeaders(base.CurrentRequest);
						}
						bool flag3 = false;
						try
						{
							this.Client.NoDelay = base.CurrentRequest.TryToMinimizeTCPLatency;
							base.CurrentRequest.SendOutTo(this.Stream);
							flag3 = true;
						}
						catch (Exception ex)
						{
							this.Close();
							if (base.State == HTTPConnectionStates.TimedOut || base.State == HTTPConnectionStates.AbortRequested)
							{
								throw new Exception("AbortRequested");
							}
							if (flag || base.CurrentRequest.DisableRetry)
							{
								throw ex;
							}
							flag = true;
							retryCauses = RetryCauses.Reconnect;
						}
						if (flag3)
						{
							bool flag4 = this.Receive();
							if (base.State == HTTPConnectionStates.TimedOut || base.State == HTTPConnectionStates.AbortRequested)
							{
								goto IL_10F;
							}
							if (!flag4 && !flag && !base.CurrentRequest.DisableRetry)
							{
								flag = true;
								retryCauses = RetryCauses.Reconnect;
							}
							if (base.CurrentRequest.Response != null)
							{
								if (base.CurrentRequest.IsCookiesEnabled)
								{
									CookieJar.Set(base.CurrentRequest.Response);
									CookieJar.Persist();
								}
								int num = base.CurrentRequest.Response.StatusCode;
								if (num <= 308)
								{
									if (num - 301 <= 1 || num - 307 <= 1)
									{
										if (base.CurrentRequest.RedirectCount < base.CurrentRequest.MaxRedirects)
										{
											HTTPRequest currentRequest = base.CurrentRequest;
											num = currentRequest.RedirectCount;
											currentRequest.RedirectCount = num + 1;
											string firstHeaderValue = base.CurrentRequest.Response.GetFirstHeaderValue("location");
											if (string.IsNullOrEmpty(firstHeaderValue))
											{
												goto IL_413;
											}
											Uri redirectUri = this.GetRedirectUri(firstHeaderValue);
											if (HTTPManager.Logger.Level == Loglevels.All)
											{
												HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Redirected to Location: '{1}' redirectUri: '{1}'", base.CurrentRequest.CurrentUri.ToString(), firstHeaderValue, redirectUri));
											}
											if (!base.CurrentRequest.CallOnBeforeRedirection(redirectUri))
											{
												HTTPManager.Logger.Information("HTTPConnection", "OnBeforeRedirection returned False");
											}
											else
											{
												base.CurrentRequest.RemoveHeader("Host");
												base.CurrentRequest.SetHeader("Referer", base.CurrentRequest.CurrentUri.ToString());
												base.CurrentRequest.RedirectUri = redirectUri;
												base.CurrentRequest.Response = null;
												bool flag5 = base.CurrentRequest.IsRedirected = true;
												flag2 = flag5;
											}
										}
									}
								}
								else if (num != 401)
								{
									if (num == 407)
									{
										if (base.CurrentRequest.HasProxy)
										{
											string text = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("proxy-authenticate"));
											if (!string.IsNullOrEmpty(text))
											{
												Digest orCreate = DigestStore.GetOrCreate(base.CurrentRequest.Proxy.Address);
												orCreate.ParseChallange(text);
												if (base.CurrentRequest.Proxy.Credentials != null && orCreate.IsUriProtected(base.CurrentRequest.Proxy.Address) && (!base.CurrentRequest.HasHeader("Proxy-Authorization") || orCreate.Stale))
												{
													retryCauses = RetryCauses.ProxyAuthenticate;
												}
											}
										}
									}
								}
								else
								{
									string text2 = DigestStore.FindBest(base.CurrentRequest.Response.GetHeaderValues("www-authenticate"));
									if (!string.IsNullOrEmpty(text2))
									{
										Digest orCreate2 = DigestStore.GetOrCreate(base.CurrentRequest.CurrentUri);
										orCreate2.ParseChallange(text2);
										if (base.CurrentRequest.Credentials != null && orCreate2.IsUriProtected(base.CurrentRequest.CurrentUri) && (!base.CurrentRequest.HasHeader("Authorization") || orCreate2.Stale))
										{
											retryCauses = RetryCauses.Authenticate;
										}
									}
								}
								this.TryStoreInCache();
								if (base.CurrentRequest.Response == null || !base.CurrentRequest.Response.IsClosedManually)
								{
									bool flag6 = base.CurrentRequest.Response == null || base.CurrentRequest.Response.HasHeaderWithValue("connection", "close");
									bool flag7 = !base.CurrentRequest.IsKeepAlive;
									if (flag6 || flag7)
									{
										this.Close();
									}
									else if (base.CurrentRequest.Response != null)
									{
										List<string> headerValues = base.CurrentRequest.Response.GetHeaderValues("keep-alive");
										if (headerValues != null && headerValues.Count > 0)
										{
											if (this.KeepAlive == null)
											{
												this.KeepAlive = new KeepAliveHeader();
											}
											this.KeepAlive.Parse(headerValues);
										}
									}
								}
							}
						}
						if (retryCauses == RetryCauses.None)
						{
							goto Block_43;
						}
					}
					throw new Exception("AbortRequested");
					IL_10F:
					throw new Exception("AbortRequested");
					IL_413:
					throw new MissingFieldException(string.Format("Got redirect status({0}) without 'location' header!", base.CurrentRequest.Response.StatusCode.ToString()));
					Block_43:;
				}
			}
			catch (TimeoutException exception)
			{
				base.CurrentRequest.Response = null;
				base.CurrentRequest.Exception = exception;
				base.CurrentRequest.State = HTTPRequestStates.ConnectionTimedOut;
				this.Close();
			}
			catch (Exception exception2)
			{
				if (base.CurrentRequest != null)
				{
					if (base.CurrentRequest.UseStreaming)
					{
						HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri, true);
					}
					base.CurrentRequest.Response = null;
					switch (base.State)
					{
					case HTTPConnectionStates.AbortRequested:
					case HTTPConnectionStates.Closed:
						base.CurrentRequest.State = HTTPRequestStates.Aborted;
						break;
					case HTTPConnectionStates.TimedOut:
						base.CurrentRequest.State = HTTPRequestStates.TimedOut;
						break;
					default:
						base.CurrentRequest.Exception = exception2;
						base.CurrentRequest.State = HTTPRequestStates.Error;
						break;
					}
				}
				this.Close();
			}
			finally
			{
				if (base.CurrentRequest != null)
				{
					object locker = HTTPManager.Locker;
					lock (locker)
					{
						if (base.CurrentRequest != null && base.CurrentRequest.Response != null && base.CurrentRequest.Response.IsUpgraded)
						{
							base.State = HTTPConnectionStates.Upgraded;
						}
						else
						{
							base.State = (flag2 ? HTTPConnectionStates.Redirected : ((this.Client == null) ? HTTPConnectionStates.Closed : HTTPConnectionStates.WaitForRecycle));
						}
						if (base.CurrentRequest.State == HTTPRequestStates.Processing && (base.State == HTTPConnectionStates.Closed || base.State == HTTPConnectionStates.WaitForRecycle))
						{
							if (base.CurrentRequest.Response != null)
							{
								base.CurrentRequest.State = HTTPRequestStates.Finished;
							}
							else
							{
								base.CurrentRequest.Exception = new Exception(string.Format("Remote server closed the connection before sending response header! Previous request state: {0}. Connection state: {1}", base.CurrentRequest.State.ToString(), base.State.ToString()));
								base.CurrentRequest.State = HTTPRequestStates.Error;
							}
						}
						if (base.CurrentRequest.State == HTTPRequestStates.ConnectionTimedOut)
						{
							base.State = HTTPConnectionStates.Closed;
						}
						this.LastProcessTime = DateTime.UtcNow;
						if (this.OnConnectionRecycled != null)
						{
							base.RecycleNow();
						}
					}
				}
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0009143C File Offset: 0x0008F63C
		private void Connect()
		{
			Uri uri = base.CurrentRequest.HasProxy ? base.CurrentRequest.Proxy.Address : base.CurrentRequest.CurrentUri;
			if (this.Client == null)
			{
				this.Client = new TcpClient();
			}
			if (!this.Client.Connected)
			{
				this.Client.ConnectTimeout = base.CurrentRequest.ConnectTimeout;
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPConnection", string.Format("'{0}' - Connecting to {1}:{2}", base.CurrentRequest.CurrentUri.ToString(), uri.Host, uri.Port.ToString()));
				}
				this.Client.SendBufferSize = HTTPManager.SendBufferSize;
				this.Client.ReceiveBufferSize = HTTPManager.ReceiveBufferSize;
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPConnection", string.Format("'{0}' - Buffer sizes - Send: {1} Receive: {2} Blocking: {3}", new object[]
					{
						base.CurrentRequest.CurrentUri.ToString(),
						this.Client.SendBufferSize.ToString(),
						this.Client.ReceiveBufferSize.ToString(),
						this.Client.Client.Blocking.ToString()
					}));
				}
				this.Client.Connect(uri.Host, uri.Port);
				if (HTTPManager.Logger.Level <= Loglevels.Information)
				{
					HTTPManager.Logger.Information("HTTPConnection", "Connected to " + uri.Host + ":" + uri.Port.ToString());
				}
			}
			else if (HTTPManager.Logger.Level <= Loglevels.Information)
			{
				HTTPManager.Logger.Information("HTTPConnection", "Already connected to " + uri.Host + ":" + uri.Port.ToString());
			}
			base.StartTime = DateTime.UtcNow;
			if (this.Stream == null)
			{
				bool flag = HTTPProtocolFactory.IsSecureProtocol(base.CurrentRequest.CurrentUri);
				this.Stream = this.Client.GetStream();
				if (base.CurrentRequest.Proxy != null)
				{
					base.CurrentRequest.Proxy.Connect(this.Stream, base.CurrentRequest);
				}
				if (flag)
				{
					if (base.CurrentRequest.UseAlternateSSL)
					{
						TlsClientProtocol tlsClientProtocol = new TlsClientProtocol(this.Client.GetStream(), new SecureRandom());
						List<string> list = base.CurrentRequest.CustomTLSServerNameList;
						if ((list == null || list.Count == 0) && !base.CurrentRequest.CurrentUri.IsHostIsAnIPAddress())
						{
							list = new List<string>(1);
							list.Add(base.CurrentRequest.CurrentUri.Host);
						}
						TlsClientProtocol tlsClientProtocol2 = tlsClientProtocol;
						Uri currentUri = base.CurrentRequest.CurrentUri;
						ICertificateVerifyer verifyer;
						if (base.CurrentRequest.CustomCertificateVerifyer != null)
						{
							verifyer = base.CurrentRequest.CustomCertificateVerifyer;
						}
						else
						{
							ICertificateVerifyer certificateVerifyer = new AlwaysValidVerifyer();
							verifyer = certificateVerifyer;
						}
						tlsClientProtocol2.Connect(new LegacyTlsClient(currentUri, verifyer, base.CurrentRequest.CustomClientCredentialsProvider, list));
						this.Stream = tlsClientProtocol.Stream;
						return;
					}
					SslStream sslStream = new SslStream(this.Client.GetStream(), false, (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => base.CurrentRequest.CallCustomCertificationValidator(cert, chain));
					if (!sslStream.IsAuthenticated)
					{
						sslStream.AuthenticateAsClient(base.CurrentRequest.CurrentUri.Host);
					}
					this.Stream = sslStream;
				}
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000917A4 File Offset: 0x0008F9A4
		private bool Receive()
		{
			SupportedProtocols protocol = (base.CurrentRequest.ProtocolHandler == SupportedProtocols.Unknown) ? HTTPProtocolFactory.GetProtocolFromUri(base.CurrentRequest.CurrentUri) : base.CurrentRequest.ProtocolHandler;
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Receive - protocol: {1}", base.CurrentRequest.CurrentUri.ToString(), protocol.ToString()));
			}
			base.CurrentRequest.Response = HTTPProtocolFactory.Get(protocol, base.CurrentRequest, this.Stream, base.CurrentRequest.UseStreaming, false);
			if (!base.CurrentRequest.Response.Receive(-1, true))
			{
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Receive - Failed! Response will be null, returning with false.", base.CurrentRequest.CurrentUri.ToString()));
				}
				base.CurrentRequest.Response = null;
				return false;
			}
			if (base.CurrentRequest.Response.StatusCode == 304 && !base.CurrentRequest.DisableCache)
			{
				if (base.CurrentRequest.IsRedirected)
				{
					if (!this.LoadFromCache(base.CurrentRequest.RedirectUri))
					{
						this.LoadFromCache(base.CurrentRequest.Uri);
					}
				}
				else
				{
					this.LoadFromCache(base.CurrentRequest.Uri);
				}
			}
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - Receive - Finished Successfully!", base.CurrentRequest.CurrentUri.ToString()));
			}
			return true;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00091940 File Offset: 0x0008FB40
		private bool LoadFromCache(Uri uri)
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - LoadFromCache for Uri: {1}", base.CurrentRequest.CurrentUri.ToString(), uri.ToString()));
			}
			HTTPCacheFileInfo entity = HTTPCacheService.GetEntity(uri);
			if (entity == null)
			{
				HTTPManager.Logger.Warning("HTTPConnection", string.Format("{0} - LoadFromCache for Uri: {1} - Cached entity not found!", base.CurrentRequest.CurrentUri.ToString(), uri.ToString()));
				return false;
			}
			base.CurrentRequest.Response.CacheFileInfo = entity;
			int num;
			using (Stream bodyStream = entity.GetBodyStream(out num))
			{
				if (bodyStream == null)
				{
					return false;
				}
				if (!base.CurrentRequest.Response.HasHeader("content-length"))
				{
					base.CurrentRequest.Response.Headers.Add("content-length", new List<string>(1)
					{
						num.ToString()
					});
				}
				base.CurrentRequest.Response.IsFromCache = true;
				if (!base.CurrentRequest.CacheOnly)
				{
					base.CurrentRequest.Response.ReadRaw(bodyStream, (long)num);
				}
			}
			return true;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00091A7C File Offset: 0x0008FC7C
		private bool TryLoadAllFromCache()
		{
			if (base.CurrentRequest.DisableCache || !HTTPCacheService.IsSupported)
			{
				return false;
			}
			try
			{
				if (HTTPCacheService.IsCachedEntityExpiresInTheFuture(base.CurrentRequest))
				{
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						HTTPManager.Logger.Verbose("HTTPConnection", string.Format("{0} - TryLoadAllFromCache - whole response loading from cache", base.CurrentRequest.CurrentUri.ToString()));
					}
					base.CurrentRequest.Response = HTTPCacheService.GetFullResponse(base.CurrentRequest);
					if (base.CurrentRequest.Response != null)
					{
						return true;
					}
				}
			}
			catch
			{
				HTTPCacheService.DeleteEntity(base.CurrentRequest.CurrentUri, true);
			}
			return false;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00091B34 File Offset: 0x0008FD34
		private void TryStoreInCache()
		{
			if (!base.CurrentRequest.UseStreaming && !base.CurrentRequest.DisableCache && base.CurrentRequest.Response != null && HTTPCacheService.IsSupported && HTTPCacheService.IsCacheble(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response))
			{
				if (base.CurrentRequest.IsRedirected)
				{
					HTTPCacheService.Store(base.CurrentRequest.Uri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
				}
				else
				{
					HTTPCacheService.Store(base.CurrentRequest.CurrentUri, base.CurrentRequest.MethodType, base.CurrentRequest.Response);
				}
				HTTPCacheService.SaveLibrary();
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00091C08 File Offset: 0x0008FE08
		private Uri GetRedirectUri(string location)
		{
			Uri uri = null;
			try
			{
				uri = new Uri(location);
				if (uri.IsFile || uri.AbsolutePath == location)
				{
					uri = null;
				}
			}
			catch (UriFormatException)
			{
				uri = null;
			}
			if (uri == null)
			{
				Uri uri2 = base.CurrentRequest.Uri;
				uri = new UriBuilder(uri2.Scheme, uri2.Host, uri2.Port, location).Uri;
			}
			return uri;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00091C84 File Offset: 0x0008FE84
		internal override void Abort(HTTPConnectionStates newState)
		{
			base.State = newState;
			HTTPConnectionStates state = base.State;
			if (state == HTTPConnectionStates.TimedOut)
			{
				base.TimedOutStart = DateTime.UtcNow;
			}
			if (this.Stream != null)
			{
				try
				{
					this.Stream.Dispose();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00091CD8 File Offset: 0x0008FED8
		private void Close()
		{
			this.KeepAlive = null;
			base.LastProcessedUri = null;
			if (this.Client != null)
			{
				try
				{
					this.Client.Close();
				}
				catch
				{
				}
				finally
				{
					this.Stream = null;
					this.Client = null;
				}
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00091D38 File Offset: 0x0008FF38
		protected override void Dispose(bool disposing)
		{
			this.Close();
			base.Dispose(disposing);
		}

		// Token: 0x040012A2 RID: 4770
		private TcpClient Client;

		// Token: 0x040012A3 RID: 4771
		private Stream Stream;

		// Token: 0x040012A4 RID: 4772
		private KeepAliveHeader KeepAlive;
	}
}
