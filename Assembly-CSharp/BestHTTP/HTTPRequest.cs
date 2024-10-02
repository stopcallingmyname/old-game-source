using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using BestHTTP.Authentication;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Forms;
using BestHTTP.Logger;
using Org.BouncyCastle.Crypto.Tls;

namespace BestHTTP
{
	// Token: 0x02000187 RID: 391
	public sealed class HTTPRequest : IEnumerator, IEnumerator<HTTPRequest>, IDisposable
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x000932D1 File Offset: 0x000914D1
		// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x000932D9 File Offset: 0x000914D9
		public Uri Uri { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x000932E2 File Offset: 0x000914E2
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x000932EA File Offset: 0x000914EA
		public HTTPMethods MethodType { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x000932F3 File Offset: 0x000914F3
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x000932FB File Offset: 0x000914FB
		public byte[] RawData { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00093304 File Offset: 0x00091504
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x0009330C File Offset: 0x0009150C
		public Stream UploadStream { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00093315 File Offset: 0x00091515
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0009331D File Offset: 0x0009151D
		public bool DisposeUploadStream { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00093326 File Offset: 0x00091526
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x0009332E File Offset: 0x0009152E
		public bool UseUploadStreamLength { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00093337 File Offset: 0x00091537
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x0009333F File Offset: 0x0009153F
		public bool IsKeepAlive
		{
			get
			{
				return this.isKeepAlive;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the IsKeepAlive property while processing the request is not supported.");
				}
				this.isKeepAlive = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0009335C File Offset: 0x0009155C
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x00093364 File Offset: 0x00091564
		public bool DisableCache
		{
			get
			{
				return this.disableCache;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the DisableCache property while processing the request is not supported.");
				}
				this.disableCache = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00093381 File Offset: 0x00091581
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x00093389 File Offset: 0x00091589
		public bool CacheOnly
		{
			get
			{
				return this.cacheOnly;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the CacheOnly property while processing the request is not supported.");
				}
				this.cacheOnly = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x000933A6 File Offset: 0x000915A6
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x000933AE File Offset: 0x000915AE
		public bool UseStreaming
		{
			get
			{
				return this.useStreaming;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the UseStreaming property while processing the request is not supported.");
				}
				this.useStreaming = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x000933CB File Offset: 0x000915CB
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x000933D3 File Offset: 0x000915D3
		public int StreamFragmentSize
		{
			get
			{
				return this.streamFragmentSize;
			}
			set
			{
				if (this.State == HTTPRequestStates.Processing)
				{
					throw new NotSupportedException("Changing the StreamFragmentSize property while processing the request is not supported.");
				}
				if (value < 1)
				{
					throw new ArgumentException("StreamFragmentSize must be at least 1.");
				}
				this.streamFragmentSize = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x000933FF File Offset: 0x000915FF
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x00093407 File Offset: 0x00091607
		public int MaxFragmentQueueLength { get; set; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00093410 File Offset: 0x00091610
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x00093418 File Offset: 0x00091618
		public OnRequestFinishedDelegate Callback { get; set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00093421 File Offset: 0x00091621
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x00093429 File Offset: 0x00091629
		public bool DisableRetry { get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00093432 File Offset: 0x00091632
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x0009343A File Offset: 0x0009163A
		public bool IsRedirected { get; internal set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00093443 File Offset: 0x00091643
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x0009344B File Offset: 0x0009164B
		public Uri RedirectUri { get; internal set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x00093454 File Offset: 0x00091654
		public Uri CurrentUri
		{
			get
			{
				if (!this.IsRedirected)
				{
					return this.Uri;
				}
				return this.RedirectUri;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x0009346B File Offset: 0x0009166B
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00093473 File Offset: 0x00091673
		public HTTPResponse Response { get; internal set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0009347C File Offset: 0x0009167C
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00093484 File Offset: 0x00091684
		public HTTPResponse ProxyResponse { get; internal set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0009348D File Offset: 0x0009168D
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x00093495 File Offset: 0x00091695
		public Exception Exception { get; internal set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x0009349E File Offset: 0x0009169E
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x000934A6 File Offset: 0x000916A6
		public object Tag { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x000934AF File Offset: 0x000916AF
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x000934B7 File Offset: 0x000916B7
		public Credentials Credentials { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x000934C0 File Offset: 0x000916C0
		public bool HasProxy
		{
			get
			{
				return this.Proxy != null;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x000934CB File Offset: 0x000916CB
		// (set) Token: 0x06000DDC RID: 3548 RVA: 0x000934D3 File Offset: 0x000916D3
		public Proxy Proxy { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x000934DC File Offset: 0x000916DC
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x000934E4 File Offset: 0x000916E4
		public int MaxRedirects { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x000934ED File Offset: 0x000916ED
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x000934F5 File Offset: 0x000916F5
		public bool UseAlternateSSL { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x000934FE File Offset: 0x000916FE
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00093506 File Offset: 0x00091706
		public bool IsCookiesEnabled { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0009350F File Offset: 0x0009170F
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x0009352A File Offset: 0x0009172A
		public List<Cookie> Cookies
		{
			get
			{
				if (this.customCookies == null)
				{
					this.customCookies = new List<Cookie>();
				}
				return this.customCookies;
			}
			set
			{
				this.customCookies = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x00093533 File Offset: 0x00091733
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0009353B File Offset: 0x0009173B
		public HTTPFormUsage FormUsage { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00093544 File Offset: 0x00091744
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x0009354C File Offset: 0x0009174C
		public HTTPRequestStates State { get; internal set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00093555 File Offset: 0x00091755
		// (set) Token: 0x06000DEA RID: 3562 RVA: 0x0009355D File Offset: 0x0009175D
		public int RedirectCount { get; internal set; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000DEB RID: 3563 RVA: 0x00093568 File Offset: 0x00091768
		// (remove) Token: 0x06000DEC RID: 3564 RVA: 0x000935A0 File Offset: 0x000917A0
		public event Func<HTTPRequest, X509Certificate, X509Chain, bool> CustomCertificationValidator;

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x000935D5 File Offset: 0x000917D5
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x000935DD File Offset: 0x000917DD
		public TimeSpan ConnectTimeout { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x000935E6 File Offset: 0x000917E6
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x000935EE File Offset: 0x000917EE
		public TimeSpan Timeout { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x000935F7 File Offset: 0x000917F7
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x000935FF File Offset: 0x000917FF
		public bool EnableTimoutForStreaming { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00093608 File Offset: 0x00091808
		// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x00093610 File Offset: 0x00091810
		public bool EnableSafeReadOnUnknownContentLength { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x00093619 File Offset: 0x00091819
		// (set) Token: 0x06000DF6 RID: 3574 RVA: 0x00093621 File Offset: 0x00091821
		public int Priority { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0009362A File Offset: 0x0009182A
		// (set) Token: 0x06000DF8 RID: 3576 RVA: 0x00093632 File Offset: 0x00091832
		public ICertificateVerifyer CustomCertificateVerifyer { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x0009363B File Offset: 0x0009183B
		// (set) Token: 0x06000DFA RID: 3578 RVA: 0x00093643 File Offset: 0x00091843
		public IClientCredentialsProvider CustomClientCredentialsProvider { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x0009364C File Offset: 0x0009184C
		// (set) Token: 0x06000DFC RID: 3580 RVA: 0x00093654 File Offset: 0x00091854
		public List<string> CustomTLSServerNameList { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x0009365D File Offset: 0x0009185D
		// (set) Token: 0x06000DFE RID: 3582 RVA: 0x00093665 File Offset: 0x00091865
		public SupportedProtocols ProtocolHandler { get; set; }

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000DFF RID: 3583 RVA: 0x0009366E File Offset: 0x0009186E
		// (remove) Token: 0x06000E00 RID: 3584 RVA: 0x00093687 File Offset: 0x00091887
		public event OnBeforeRedirectionDelegate OnBeforeRedirection
		{
			add
			{
				this.onBeforeRedirection = (OnBeforeRedirectionDelegate)Delegate.Combine(this.onBeforeRedirection, value);
			}
			remove
			{
				this.onBeforeRedirection = (OnBeforeRedirectionDelegate)Delegate.Remove(this.onBeforeRedirection, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000E01 RID: 3585 RVA: 0x000936A0 File Offset: 0x000918A0
		// (remove) Token: 0x06000E02 RID: 3586 RVA: 0x000936B9 File Offset: 0x000918B9
		public event OnBeforeHeaderSendDelegate OnBeforeHeaderSend
		{
			add
			{
				this._onBeforeHeaderSend = (OnBeforeHeaderSendDelegate)Delegate.Combine(this._onBeforeHeaderSend, value);
			}
			remove
			{
				this._onBeforeHeaderSend = (OnBeforeHeaderSendDelegate)Delegate.Remove(this._onBeforeHeaderSend, value);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x000936D2 File Offset: 0x000918D2
		// (set) Token: 0x06000E04 RID: 3588 RVA: 0x000936DA File Offset: 0x000918DA
		public bool TryToMinimizeTCPLatency { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x000936E3 File Offset: 0x000918E3
		// (set) Token: 0x06000E06 RID: 3590 RVA: 0x000936EB File Offset: 0x000918EB
		internal long Downloaded { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x000936F4 File Offset: 0x000918F4
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x000936FC File Offset: 0x000918FC
		internal long DownloadLength { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x00093705 File Offset: 0x00091905
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x0009370D File Offset: 0x0009190D
		internal bool DownloadProgressChanged { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00093718 File Offset: 0x00091918
		internal long UploadStreamLength
		{
			get
			{
				if (this.UploadStream == null || !this.UseUploadStreamLength)
				{
					return -1L;
				}
				long result;
				try
				{
					result = this.UploadStream.Length;
				}
				catch
				{
					result = -1L;
				}
				return result;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x00093760 File Offset: 0x00091960
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x00093768 File Offset: 0x00091968
		internal long Uploaded { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x00093771 File Offset: 0x00091971
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x00093779 File Offset: 0x00091979
		internal long UploadLength { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00093782 File Offset: 0x00091982
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0009378A File Offset: 0x0009198A
		internal bool UploadProgressChanged { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00093793 File Offset: 0x00091993
		// (set) Token: 0x06000E13 RID: 3603 RVA: 0x0009379B File Offset: 0x0009199B
		private Dictionary<string, List<string>> Headers { get; set; }

		// Token: 0x06000E14 RID: 3604 RVA: 0x000937A4 File Offset: 0x000919A4
		public HTTPRequest(Uri uri) : this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, null)
		{
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000937B9 File Offset: 0x000919B9
		public HTTPRequest(Uri uri, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled, callback)
		{
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000937CE File Offset: 0x000919CE
		public HTTPRequest(Uri uri, bool isKeepAlive, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, isKeepAlive, HTTPManager.IsCachingDisabled, callback)
		{
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000937DF File Offset: 0x000919DF
		public HTTPRequest(Uri uri, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback) : this(uri, HTTPMethods.Get, isKeepAlive, disableCache, callback)
		{
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000937ED File Offset: 0x000919ED
		public HTTPRequest(Uri uri, HTTPMethods methodType) : this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || methodType > HTTPMethods.Get, null)
		{
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0009380B File Offset: 0x00091A0B
		public HTTPRequest(Uri uri, HTTPMethods methodType, OnRequestFinishedDelegate callback) : this(uri, methodType, HTTPManager.KeepAliveDefaultValue, HTTPManager.IsCachingDisabled || methodType > HTTPMethods.Get, callback)
		{
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00093829 File Offset: 0x00091A29
		public HTTPRequest(Uri uri, HTTPMethods methodType, bool isKeepAlive, OnRequestFinishedDelegate callback) : this(uri, methodType, isKeepAlive, HTTPManager.IsCachingDisabled || methodType > HTTPMethods.Get, callback)
		{
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00093844 File Offset: 0x00091A44
		public HTTPRequest(Uri uri, HTTPMethods methodType, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback)
		{
			this.Uri = uri;
			this.MethodType = methodType;
			this.IsKeepAlive = isKeepAlive;
			this.DisableCache = disableCache;
			this.Callback = callback;
			this.StreamFragmentSize = 4096;
			this.MaxFragmentQueueLength = 10;
			this.DisableRetry = (methodType > HTTPMethods.Get);
			this.MaxRedirects = int.MaxValue;
			this.RedirectCount = 0;
			this.IsCookiesEnabled = HTTPManager.IsCookiesEnabled;
			this.Downloaded = (this.DownloadLength = 0L);
			this.DownloadProgressChanged = false;
			this.State = HTTPRequestStates.Initial;
			this.ConnectTimeout = HTTPManager.ConnectTimeout;
			this.Timeout = HTTPManager.RequestTimeout;
			this.EnableTimoutForStreaming = false;
			this.EnableSafeReadOnUnknownContentLength = true;
			this.Proxy = HTTPManager.Proxy;
			this.UseUploadStreamLength = true;
			this.DisposeUploadStream = true;
			this.CustomCertificateVerifyer = HTTPManager.DefaultCertificateVerifyer;
			this.CustomClientCredentialsProvider = HTTPManager.DefaultClientCredentialsProvider;
			this.UseAlternateSSL = HTTPManager.UseAlternateSSLDefaultValue;
			this.CustomCertificationValidator += HTTPManager.DefaultCertificationValidator;
			this.TryToMinimizeTCPLatency = HTTPManager.TryToMinimizeTCPLatency;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00093949 File Offset: 0x00091B49
		public void AddField(string fieldName, string value)
		{
			this.AddField(fieldName, value, Encoding.UTF8);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00093958 File Offset: 0x00091B58
		public void AddField(string fieldName, string value, Encoding e)
		{
			if (this.FieldCollector == null)
			{
				this.FieldCollector = new HTTPFormBase();
			}
			this.FieldCollector.AddField(fieldName, value, e);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0009397B File Offset: 0x00091B7B
		public void AddBinaryData(string fieldName, byte[] content)
		{
			this.AddBinaryData(fieldName, content, null, null);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00093987 File Offset: 0x00091B87
		public void AddBinaryData(string fieldName, byte[] content, string fileName)
		{
			this.AddBinaryData(fieldName, content, fileName, null);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00093993 File Offset: 0x00091B93
		public void AddBinaryData(string fieldName, byte[] content, string fileName, string mimeType)
		{
			if (this.FieldCollector == null)
			{
				this.FieldCollector = new HTTPFormBase();
			}
			this.FieldCollector.AddBinaryData(fieldName, content, fileName, mimeType);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x000939B8 File Offset: 0x00091BB8
		public void SetForm(HTTPFormBase form)
		{
			this.FormImpl = form;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x000939C1 File Offset: 0x00091BC1
		public List<HTTPFieldData> GetFormFields()
		{
			if (this.FieldCollector == null || this.FieldCollector.IsEmpty)
			{
				return null;
			}
			return new List<HTTPFieldData>(this.FieldCollector.Fields);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000939EA File Offset: 0x00091BEA
		public void ClearForm()
		{
			this.FormImpl = null;
			this.FieldCollector = null;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000939FC File Offset: 0x00091BFC
		private HTTPFormBase SelectFormImplementation()
		{
			if (this.FormImpl != null)
			{
				return this.FormImpl;
			}
			if (this.FieldCollector == null)
			{
				return null;
			}
			switch (this.FormUsage)
			{
			case HTTPFormUsage.Automatic:
				if (this.FieldCollector.HasBinary || this.FieldCollector.HasLongValue)
				{
					goto IL_5F;
				}
				break;
			case HTTPFormUsage.UrlEncoded:
				break;
			case HTTPFormUsage.Multipart:
				goto IL_5F;
			case HTTPFormUsage.RawJSon:
				this.FormImpl = new RawJsonForm();
				goto IL_77;
			default:
				goto IL_77;
			}
			this.FormImpl = new HTTPUrlEncodedForm();
			goto IL_77;
			IL_5F:
			this.FormImpl = new HTTPMultiPartForm();
			IL_77:
			this.FormImpl.CopyFrom(this.FieldCollector);
			return this.FormImpl;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00093A98 File Offset: 0x00091C98
		public void AddHeader(string name, string value)
		{
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

		// Token: 0x06000E26 RID: 3622 RVA: 0x00093AE4 File Offset: 0x00091CE4
		public void SetHeader(string name, string value)
		{
			if (this.Headers == null)
			{
				this.Headers = new Dictionary<string, List<string>>();
			}
			List<string> list;
			if (!this.Headers.TryGetValue(name, out list))
			{
				this.Headers.Add(name, list = new List<string>(1));
			}
			list.Clear();
			list.Add(value);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00093B35 File Offset: 0x00091D35
		public bool RemoveHeader(string name)
		{
			return this.Headers != null && this.Headers.Remove(name);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00093B4D File Offset: 0x00091D4D
		public bool HasHeader(string name)
		{
			return this.Headers != null && this.Headers.ContainsKey(name);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00093B68 File Offset: 0x00091D68
		public string GetFirstHeaderValue(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			List<string> list = null;
			if (this.Headers.TryGetValue(name, out list) && list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00093BA4 File Offset: 0x00091DA4
		public List<string> GetHeaderValues(string name)
		{
			if (this.Headers == null)
			{
				return null;
			}
			List<string> list = null;
			if (this.Headers.TryGetValue(name, out list) && list.Count > 0)
			{
				return list;
			}
			return null;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00093BD9 File Offset: 0x00091DD9
		public void RemoveHeaders()
		{
			if (this.Headers == null)
			{
				return;
			}
			this.Headers.Clear();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00093BEF File Offset: 0x00091DEF
		public void SetRangeHeader(int firstBytePos)
		{
			this.SetHeader("Range", string.Format("bytes={0}-", firstBytePos));
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00093C0C File Offset: 0x00091E0C
		public void SetRangeHeader(int firstBytePos, int lastBytePos)
		{
			this.SetHeader("Range", string.Format("bytes={0}-{1}", firstBytePos, lastBytePos));
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00093C2F File Offset: 0x00091E2F
		public void EnumerateHeaders(OnHeaderEnumerationDelegate callback)
		{
			this.EnumerateHeaders(callback, false);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00093C3C File Offset: 0x00091E3C
		public void EnumerateHeaders(OnHeaderEnumerationDelegate callback, bool callBeforeSendCallback)
		{
			if (!this.HasHeader("Host"))
			{
				if (this.CurrentUri.Port == 80 || this.CurrentUri.Port == 443)
				{
					this.SetHeader("Host", this.CurrentUri.Host);
				}
				else
				{
					this.SetHeader("Host", this.CurrentUri.Authority);
				}
			}
			if (this.IsRedirected && !this.HasHeader("Referer"))
			{
				this.AddHeader("Referer", this.Uri.ToString());
			}
			if (!this.HasHeader("Accept-Encoding"))
			{
				this.AddHeader("Accept-Encoding", "gzip, identity");
			}
			if (this.HasProxy && !this.HasHeader("Proxy-Connection"))
			{
				this.AddHeader("Proxy-Connection", this.IsKeepAlive ? "Keep-Alive" : "Close");
			}
			if (!this.HasHeader("Connection"))
			{
				this.AddHeader("Connection", this.IsKeepAlive ? "Keep-Alive, TE" : "Close, TE");
			}
			if (!this.HasHeader("TE"))
			{
				this.AddHeader("TE", "identity");
			}
			if (!string.IsNullOrEmpty(HTTPManager.UserAgent) && !this.HasHeader("User-Agent"))
			{
				this.AddHeader("User-Agent", HTTPManager.UserAgent);
			}
			long num;
			if (this.UploadStream == null)
			{
				byte[] entityBody = this.GetEntityBody();
				num = (long)((entityBody != null) ? entityBody.Length : 0);
				if (this.RawData == null && (this.FormImpl != null || (this.FieldCollector != null && !this.FieldCollector.IsEmpty)))
				{
					this.SelectFormImplementation();
					if (this.FormImpl != null)
					{
						this.FormImpl.PrepareRequest(this);
					}
				}
			}
			else
			{
				num = this.UploadStreamLength;
				if (num == -1L)
				{
					this.SetHeader("Transfer-Encoding", "Chunked");
				}
				if (!this.HasHeader("Content-Type"))
				{
					this.SetHeader("Content-Type", "application/octet-stream");
				}
			}
			if (num >= 0L && !this.HasHeader("Content-Length"))
			{
				this.SetHeader("Content-Length", num.ToString());
			}
			if (this.HasProxy && this.Proxy.Credentials != null)
			{
				switch (this.Proxy.Credentials.Type)
				{
				case AuthenticationTypes.Unknown:
				case AuthenticationTypes.Digest:
				{
					Digest digest = DigestStore.Get(this.Proxy.Address);
					if (digest != null)
					{
						string value = digest.GenerateResponseHeader(this, this.Proxy.Credentials, false);
						if (!string.IsNullOrEmpty(value))
						{
							this.SetHeader("Proxy-Authorization", value);
						}
					}
					break;
				}
				case AuthenticationTypes.Basic:
					this.SetHeader("Proxy-Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Proxy.Credentials.UserName + ":" + this.Proxy.Credentials.Password)));
					break;
				}
			}
			if (this.Credentials != null)
			{
				switch (this.Credentials.Type)
				{
				case AuthenticationTypes.Unknown:
				case AuthenticationTypes.Digest:
				{
					Digest digest2 = DigestStore.Get(this.CurrentUri);
					if (digest2 != null)
					{
						string value2 = digest2.GenerateResponseHeader(this, this.Credentials, false);
						if (!string.IsNullOrEmpty(value2))
						{
							this.SetHeader("Authorization", value2);
						}
					}
					break;
				}
				case AuthenticationTypes.Basic:
					this.SetHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Credentials.UserName + ":" + this.Credentials.Password)));
					break;
				}
			}
			List<Cookie> list = this.IsCookiesEnabled ? CookieJar.Get(this.CurrentUri) : null;
			if (list == null || list.Count == 0)
			{
				list = this.customCookies;
			}
			else if (this.customCookies != null)
			{
				for (int i = 0; i < this.customCookies.Count; i++)
				{
					Cookie customCookie = this.customCookies[i];
					int num2 = list.FindIndex((Cookie c) => c.Name.Equals(customCookie.Name));
					if (num2 >= 0)
					{
						list[num2] = customCookie;
					}
					else
					{
						list.Add(customCookie);
					}
				}
			}
			if (list != null && list.Count > 0)
			{
				bool flag = true;
				string text = string.Empty;
				bool flag2 = HTTPProtocolFactory.IsSecureProtocol(this.CurrentUri);
				foreach (Cookie cookie in list)
				{
					if (!cookie.IsSecure || (cookie.IsSecure && flag2))
					{
						if (!flag)
						{
							text += "; ";
						}
						else
						{
							flag = false;
						}
						text += cookie.ToString();
						cookie.LastAccess = DateTime.UtcNow;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.SetHeader("Cookie", text);
				}
			}
			if (callBeforeSendCallback && this._onBeforeHeaderSend != null)
			{
				try
				{
					this._onBeforeHeaderSend(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("HTTPRequest", "OnBeforeHeaderSend", ex);
				}
			}
			if (callback != null && this.Headers != null)
			{
				foreach (KeyValuePair<string, List<string>> keyValuePair in this.Headers)
				{
					callback(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x000941CC File Offset: 0x000923CC
		private void SendHeaders(Stream stream)
		{
			this.EnumerateHeaders(delegate(string header, List<string> values)
			{
				if (string.IsNullOrEmpty(header) || values == null)
				{
					return;
				}
				byte[] asciibytes = (header + ": ").GetASCIIBytes();
				for (int i = 0; i < values.Count; i++)
				{
					if (string.IsNullOrEmpty(values[i]))
					{
						HTTPManager.Logger.Warning("HTTPRequest", string.Format("Null/empty value for header: {0}", header));
					}
					else
					{
						if (HTTPManager.Logger.Level <= Loglevels.Information)
						{
							this.VerboseLogging(string.Concat(new string[]
							{
								"Header - '",
								header,
								"': '",
								values[i],
								"'"
							}));
						}
						byte[] asciibytes2 = values[i].GetASCIIBytes();
						stream.WriteArray(asciibytes);
						stream.WriteArray(asciibytes2);
						stream.WriteArray(HTTPRequest.EOL);
						VariableSizedBufferPool.Release(asciibytes2);
					}
				}
				VariableSizedBufferPool.Release(asciibytes);
			}, true);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00094200 File Offset: 0x00092400
		public string DumpHeaders()
		{
			string result;
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream(5120))
			{
				this.SendHeaders(bufferPoolMemoryStream);
				result = bufferPoolMemoryStream.ToArray().AsciiToString();
			}
			return result;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00094248 File Offset: 0x00092448
		public byte[] GetEntityBody()
		{
			if (this.RawData != null)
			{
				return this.RawData;
			}
			if (this.FormImpl != null || (this.FieldCollector != null && !this.FieldCollector.IsEmpty))
			{
				this.SelectFormImplementation();
				if (this.FormImpl != null)
				{
					return this.FormImpl.GetData();
				}
			}
			return null;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000942A0 File Offset: 0x000924A0
		internal void SendOutTo(Stream stream)
		{
			try
			{
				string arg = this.HasProxy ? this.Proxy.GetRequestPath(this.CurrentUri) : this.CurrentUri.GetRequestPathAndQueryURL();
				string text = string.Format("{0} {1} HTTP/1.1", HTTPRequest.MethodNames[(int)this.MethodType], arg);
				if (HTTPManager.Logger.Level <= Loglevels.Information)
				{
					HTTPManager.Logger.Information("HTTPRequest", string.Format("Sending request: '{0}'", text));
				}
				using (WriteOnlyBufferedStream writeOnlyBufferedStream = new WriteOnlyBufferedStream(stream, (int)((float)HTTPRequest.UploadChunkSize * 1.5f)))
				{
					byte[] asciibytes = text.GetASCIIBytes();
					writeOnlyBufferedStream.WriteArray(asciibytes);
					writeOnlyBufferedStream.WriteArray(HTTPRequest.EOL);
					VariableSizedBufferPool.Release(asciibytes);
					this.SendHeaders(writeOnlyBufferedStream);
					writeOnlyBufferedStream.WriteArray(HTTPRequest.EOL);
					writeOnlyBufferedStream.Flush();
					byte[] array = this.RawData;
					if (array == null && this.FormImpl != null)
					{
						array = this.FormImpl.GetData();
					}
					if (array != null || this.UploadStream != null)
					{
						Stream stream2 = this.UploadStream;
						if (stream2 == null)
						{
							stream2 = new MemoryStream(array, 0, array.Length);
							this.UploadLength = (long)array.Length;
						}
						else
						{
							this.UploadLength = (this.UseUploadStreamLength ? this.UploadStreamLength : -1L);
						}
						this.Uploaded = 0L;
						byte[] array2 = VariableSizedBufferPool.Get((long)HTTPRequest.UploadChunkSize, true);
						int num;
						while ((num = stream2.Read(array2, 0, array2.Length)) > 0)
						{
							if (!this.UseUploadStreamLength)
							{
								byte[] asciibytes2 = num.ToString("X").GetASCIIBytes();
								writeOnlyBufferedStream.WriteArray(asciibytes2);
								writeOnlyBufferedStream.WriteArray(HTTPRequest.EOL);
								VariableSizedBufferPool.Release(asciibytes2);
							}
							writeOnlyBufferedStream.Write(array2, 0, num);
							if (!this.UseUploadStreamLength)
							{
								writeOnlyBufferedStream.WriteArray(HTTPRequest.EOL);
							}
							this.Uploaded += (long)num;
							writeOnlyBufferedStream.Flush();
							this.UploadProgressChanged = true;
						}
						VariableSizedBufferPool.Release(array2);
						if (!this.UseUploadStreamLength)
						{
							byte[] array3 = VariableSizedBufferPool.Get(1L, true);
							array3[0] = 48;
							writeOnlyBufferedStream.Write(array3, 0, 1);
							writeOnlyBufferedStream.WriteArray(HTTPRequest.EOL);
							writeOnlyBufferedStream.WriteArray(HTTPRequest.EOL);
							VariableSizedBufferPool.Release(array3);
						}
						writeOnlyBufferedStream.Flush();
						if (this.UploadStream == null && stream2 != null)
						{
							stream2.Dispose();
						}
					}
					else
					{
						writeOnlyBufferedStream.Flush();
					}
				}
				HTTPManager.Logger.Information("HTTPRequest", "'" + text + "' sent out");
			}
			finally
			{
				if (this.UploadStream != null && this.DisposeUploadStream)
				{
					this.UploadStream.Dispose();
				}
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00094554 File Offset: 0x00092754
		internal void UpgradeCallback()
		{
			if (this.Response == null || !this.Response.IsUpgraded)
			{
				return;
			}
			try
			{
				if (this.OnUpgraded != null)
				{
					this.OnUpgraded(this, this.Response);
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HTTPRequest", "UpgradeCallback", ex);
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x000945BC File Offset: 0x000927BC
		internal void CallCallback()
		{
			try
			{
				if (this.Callback != null)
				{
					this.Callback(this, this.Response);
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HTTPRequest", "CallCallback", ex);
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00094610 File Offset: 0x00092810
		internal bool CallOnBeforeRedirection(Uri redirectUri)
		{
			return this.onBeforeRedirection == null || this.onBeforeRedirection(this, this.Response, redirectUri);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0009462F File Offset: 0x0009282F
		internal void FinishStreaming()
		{
			if (this.Response != null && this.UseStreaming)
			{
				this.Response.FinishStreaming();
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0000248C File Offset: 0x0000068C
		internal void Prepare()
		{
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0009464C File Offset: 0x0009284C
		internal bool CallCustomCertificationValidator(X509Certificate cert, X509Chain chain)
		{
			return this.CustomCertificationValidator == null || this.CustomCertificationValidator(this, cert, chain);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00094666 File Offset: 0x00092866
		public HTTPRequest Send()
		{
			return HTTPManager.SendRequest(this);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00094670 File Offset: 0x00092870
		public void Abort()
		{
			if (Monitor.TryEnter(HTTPManager.Locker, TimeSpan.FromMilliseconds(100.0)))
			{
				try
				{
					if (this.State >= HTTPRequestStates.Finished)
					{
						HTTPManager.Logger.Warning("HTTPRequest", string.Format("Abort - Already in a state({0}) where no Abort required!", this.State.ToString()));
						return;
					}
					ConnectionBase connectionWith = HTTPManager.GetConnectionWith(this);
					if (connectionWith == null)
					{
						if (!HTTPManager.RemoveFromQueue(this))
						{
							HTTPManager.Logger.Warning("HTTPRequest", "Abort - No active connection found with this request! (The request may already finished?)");
						}
						this.State = HTTPRequestStates.Aborted;
						this.CallCallback();
						return;
					}
					if (this.Response != null && this.Response.IsStreamed)
					{
						this.Response.Dispose();
					}
					connectionWith.Abort(HTTPConnectionStates.AbortRequested);
					return;
				}
				finally
				{
					Monitor.Exit(HTTPManager.Locker);
				}
			}
			throw new Exception("Wasn't able to acquire a thread lock. Abort failed!");
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00094758 File Offset: 0x00092958
		public void Clear()
		{
			this.ClearForm();
			this.RemoveHeaders();
			this.IsRedirected = false;
			this.RedirectCount = 0;
			this.Downloaded = (this.DownloadLength = 0L);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00094790 File Offset: 0x00092990
		private void VerboseLogging(string str)
		{
			HTTPManager.Logger.Verbose("HTTPRequest", "'" + this.CurrentUri.ToString() + "' - " + str);
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x0008D54A File Offset: 0x0008B74A
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x000947BC File Offset: 0x000929BC
		public bool MoveNext()
		{
			return this.State < HTTPRequestStates.Finished;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x000947C7 File Offset: 0x000929C7
		public void Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x000947CE File Offset: 0x000929CE
		HTTPRequest IEnumerator<HTTPRequest>.Current
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x000947D1 File Offset: 0x000929D1
		public void Dispose()
		{
			if (this.Response != null)
			{
				this.Response.Dispose();
			}
		}

		// Token: 0x040012EF RID: 4847
		public static readonly byte[] EOL = new byte[]
		{
			13,
			10
		};

		// Token: 0x040012F0 RID: 4848
		public static readonly string[] MethodNames = new string[]
		{
			HTTPMethods.Get.ToString().ToUpper(),
			HTTPMethods.Head.ToString().ToUpper(),
			HTTPMethods.Post.ToString().ToUpper(),
			HTTPMethods.Put.ToString().ToUpper(),
			HTTPMethods.Delete.ToString().ToUpper(),
			HTTPMethods.Patch.ToString().ToUpper(),
			HTTPMethods.Merge.ToString().ToUpper(),
			HTTPMethods.Options.ToString().ToUpper()
		};

		// Token: 0x040012F1 RID: 4849
		public static int UploadChunkSize = 2048;

		// Token: 0x040012F8 RID: 4856
		public OnUploadProgressDelegate OnUploadProgress;

		// Token: 0x040012FB RID: 4859
		public OnDownloadProgressDelegate OnProgress;

		// Token: 0x040012FC RID: 4860
		public OnRequestFinishedDelegate OnUpgraded;

		// Token: 0x04001309 RID: 4873
		private List<Cookie> customCookies;

		// Token: 0x04001317 RID: 4887
		private OnBeforeRedirectionDelegate onBeforeRedirection;

		// Token: 0x04001318 RID: 4888
		private OnBeforeHeaderSendDelegate _onBeforeHeaderSend;

		// Token: 0x04001320 RID: 4896
		private bool isKeepAlive;

		// Token: 0x04001321 RID: 4897
		private bool disableCache;

		// Token: 0x04001322 RID: 4898
		private bool cacheOnly;

		// Token: 0x04001323 RID: 4899
		private int streamFragmentSize;

		// Token: 0x04001324 RID: 4900
		private bool useStreaming;

		// Token: 0x04001326 RID: 4902
		private HTTPFormBase FieldCollector;

		// Token: 0x04001327 RID: 4903
		private HTTPFormBase FormImpl;
	}
}
