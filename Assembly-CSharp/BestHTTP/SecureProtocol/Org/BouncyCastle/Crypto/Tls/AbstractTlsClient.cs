using System;
using System.Collections;
using System.Collections.Generic;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F2 RID: 1010
	public abstract class AbstractTlsClient : AbstractTlsPeer, TlsClient, TlsPeer
	{
		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060028D1 RID: 10449 RVA: 0x0010D2D0 File Offset: 0x0010B4D0
		// (set) Token: 0x060028D2 RID: 10450 RVA: 0x0010D2D8 File Offset: 0x0010B4D8
		public List<string> HostNames { get; set; }

		// Token: 0x060028D3 RID: 10451 RVA: 0x0010D2E1 File Offset: 0x0010B4E1
		public AbstractTlsClient() : this(new DefaultTlsCipherFactory())
		{
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x0010D2EE File Offset: 0x0010B4EE
		public AbstractTlsClient(TlsCipherFactory cipherFactory)
		{
			this.mCipherFactory = cipherFactory;
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x0010D2FD File Offset: 0x0010B4FD
		protected virtual bool AllowUnexpectedServerExtension(int extensionType, byte[] extensionData)
		{
			if (extensionType == 10)
			{
				TlsEccUtilities.ReadSupportedEllipticCurvesExtension(extensionData);
				return true;
			}
			if (extensionType != 11)
			{
				return false;
			}
			TlsEccUtilities.ReadSupportedPointFormatsExtension(extensionData);
			return true;
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x0010D320 File Offset: 0x0010B520
		protected virtual void CheckForUnexpectedServerExtension(IDictionary serverExtensions, int extensionType)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(serverExtensions, extensionType);
			if (extensionData != null && !this.AllowUnexpectedServerExtension(extensionType, extensionData))
			{
				throw new TlsFatalAlert(47);
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x0010D34A File Offset: 0x0010B54A
		public virtual void Init(TlsClientContext context)
		{
			this.mContext = context;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x0008D54A File Offset: 0x0008B74A
		public virtual TlsSession GetSessionToResume()
		{
			return null;
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x0010D353 File Offset: 0x0010B553
		public virtual ProtocolVersion ClientHelloRecordLayerVersion
		{
			get
			{
				return this.ClientVersion;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x0010D35B File Offset: 0x0010B55B
		public virtual ProtocolVersion ClientVersion
		{
			get
			{
				return ProtocolVersion.TLSv12;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsFallback
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x0010D364 File Offset: 0x0010B564
		public virtual IDictionary GetClientExtensions()
		{
			IDictionary dictionary = null;
			if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(this.mContext.ClientVersion))
			{
				this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultSupportedSignatureAlgorithms();
				dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(dictionary);
				TlsUtilities.AddSignatureAlgorithmsExtension(dictionary, this.mSupportedSignatureAlgorithms);
			}
			if (TlsEccUtilities.ContainsEccCipherSuites(this.GetCipherSuites()))
			{
				this.mNamedCurves = new int[]
				{
					23,
					24
				};
				this.mClientECPointFormats = new byte[]
				{
					0,
					1,
					2
				};
				dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(dictionary);
				TlsEccUtilities.AddSupportedEllipticCurvesExtension(dictionary, this.mNamedCurves);
				TlsEccUtilities.AddSupportedPointFormatsExtension(dictionary, this.mClientECPointFormats);
			}
			if (this.HostNames != null && this.HostNames.Count > 0)
			{
				List<ServerName> list = new List<ServerName>(this.HostNames.Count);
				for (int i = 0; i < this.HostNames.Count; i++)
				{
					list.Add(new ServerName(0, this.HostNames[i]));
				}
				TlsExtensionsUtilities.AddServerNameExtension(dictionary, new ServerNameList(list));
			}
			return dictionary;
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060028DD RID: 10461 RVA: 0x0010D45B File Offset: 0x0010B65B
		public virtual ProtocolVersion MinimumVersion
		{
			get
			{
				return ProtocolVersion.TLSv10;
			}
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x0010D462 File Offset: 0x0010B662
		public virtual void NotifyServerVersion(ProtocolVersion serverVersion)
		{
			if (!this.MinimumVersion.IsEqualOrEarlierVersionOf(serverVersion))
			{
				throw new TlsFatalAlert(70);
			}
		}

		// Token: 0x060028DF RID: 10463
		public abstract int[] GetCipherSuites();

		// Token: 0x060028E0 RID: 10464 RVA: 0x0010D47A File Offset: 0x0010B67A
		public virtual byte[] GetCompressionMethods()
		{
			return new byte[1];
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void NotifySessionID(byte[] sessionID)
		{
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x0010D482 File Offset: 0x0010B682
		public virtual void NotifySelectedCipherSuite(int selectedCipherSuite)
		{
			this.mSelectedCipherSuite = selectedCipherSuite;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x0010D48B File Offset: 0x0010B68B
		public virtual void NotifySelectedCompressionMethod(byte selectedCompressionMethod)
		{
			this.mSelectedCompressionMethod = (short)selectedCompressionMethod;
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x0010D494 File Offset: 0x0010B694
		public virtual void ProcessServerExtensions(IDictionary serverExtensions)
		{
			if (serverExtensions != null)
			{
				this.CheckForUnexpectedServerExtension(serverExtensions, 13);
				this.CheckForUnexpectedServerExtension(serverExtensions, 10);
				if (TlsEccUtilities.IsEccCipherSuite(this.mSelectedCipherSuite))
				{
					this.mServerECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension(serverExtensions);
				}
				else
				{
					this.CheckForUnexpectedServerExtension(serverExtensions, 11);
				}
				this.CheckForUnexpectedServerExtension(serverExtensions, 21);
			}
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x0010D4E3 File Offset: 0x0010B6E3
		public virtual void ProcessServerSupplementalData(IList serverSupplementalData)
		{
			if (serverSupplementalData != null)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x060028E6 RID: 10470
		public abstract TlsKeyExchange GetKeyExchange();

		// Token: 0x060028E7 RID: 10471
		public abstract TlsAuthentication GetAuthentication();

		// Token: 0x060028E8 RID: 10472 RVA: 0x0008D54A File Offset: 0x0008B74A
		public virtual IList GetClientSupplementalData()
		{
			return null;
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x0010D4F0 File Offset: 0x0010B6F0
		public override TlsCompression GetCompression()
		{
			short num = this.mSelectedCompressionMethod;
			if (num == 0)
			{
				return new TlsNullCompression();
			}
			if (num != 1)
			{
				throw new TlsFatalAlert(80);
			}
			return new TlsDeflateCompression();
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x0010D520 File Offset: 0x0010B720
		public override TlsCipher GetCipher()
		{
			int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(this.mSelectedCipherSuite);
			int macAlgorithm = TlsUtilities.GetMacAlgorithm(this.mSelectedCipherSuite);
			return this.mCipherFactory.CreateCipher(this.mContext, encryptionAlgorithm, macAlgorithm);
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void NotifyNewSessionTicket(NewSessionTicket newSessionTicket)
		{
		}

		// Token: 0x04001B0D RID: 6925
		protected TlsCipherFactory mCipherFactory;

		// Token: 0x04001B0E RID: 6926
		protected TlsClientContext mContext;

		// Token: 0x04001B0F RID: 6927
		protected IList mSupportedSignatureAlgorithms;

		// Token: 0x04001B10 RID: 6928
		protected int[] mNamedCurves;

		// Token: 0x04001B11 RID: 6929
		protected byte[] mClientECPointFormats;

		// Token: 0x04001B12 RID: 6930
		protected byte[] mServerECPointFormats;

		// Token: 0x04001B13 RID: 6931
		protected int mSelectedCipherSuite;

		// Token: 0x04001B14 RID: 6932
		protected short mSelectedCompressionMethod;
	}
}
