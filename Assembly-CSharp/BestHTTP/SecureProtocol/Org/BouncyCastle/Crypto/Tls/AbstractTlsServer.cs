using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F8 RID: 1016
	public abstract class AbstractTlsServer : AbstractTlsPeer, TlsServer, TlsPeer
	{
		// Token: 0x0600291A RID: 10522 RVA: 0x0010D8B5 File Offset: 0x0010BAB5
		public AbstractTlsServer() : this(new DefaultTlsCipherFactory())
		{
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x0010D8C2 File Offset: 0x0010BAC2
		public AbstractTlsServer(TlsCipherFactory cipherFactory)
		{
			this.mCipherFactory = cipherFactory;
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600291C RID: 10524 RVA: 0x0006AE98 File Offset: 0x00069098
		protected virtual bool AllowEncryptThenMac
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x0007D96F File Offset: 0x0007BB6F
		protected virtual bool AllowTruncatedHMac
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x0010D8D4 File Offset: 0x0010BAD4
		protected virtual IDictionary CheckServerExtensions()
		{
			return this.mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(this.mServerExtensions);
		}

		// Token: 0x0600291F RID: 10527
		protected abstract int[] GetCipherSuites();

		// Token: 0x06002920 RID: 10528 RVA: 0x0010D47A File Offset: 0x0010B67A
		protected byte[] GetCompressionMethods()
		{
			return new byte[1];
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x0010D8F5 File Offset: 0x0010BAF5
		protected virtual ProtocolVersion MaximumVersion
		{
			get
			{
				return ProtocolVersion.TLSv11;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x0010D45B File Offset: 0x0010B65B
		protected virtual ProtocolVersion MinimumVersion
		{
			get
			{
				return ProtocolVersion.TLSv10;
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x0010D8FC File Offset: 0x0010BAFC
		protected virtual bool SupportsClientEccCapabilities(int[] namedCurves, byte[] ecPointFormats)
		{
			if (namedCurves == null)
			{
				return TlsEccUtilities.HasAnySupportedNamedCurves();
			}
			foreach (int namedCurve in namedCurves)
			{
				if (NamedCurve.IsValid(namedCurve) && (!NamedCurve.RefersToASpecificNamedCurve(namedCurve) || TlsEccUtilities.IsSupportedNamedCurve(namedCurve)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x0010D93F File Offset: 0x0010BB3F
		public virtual void Init(TlsServerContext context)
		{
			this.mContext = context;
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x0010D948 File Offset: 0x0010BB48
		public virtual void NotifyClientVersion(ProtocolVersion clientVersion)
		{
			this.mClientVersion = clientVersion;
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x0010D951 File Offset: 0x0010BB51
		public virtual void NotifyFallback(bool isFallback)
		{
			if (isFallback && this.MaximumVersion.IsLaterVersionOf(this.mClientVersion))
			{
				throw new TlsFatalAlert(86);
			}
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x0010D971 File Offset: 0x0010BB71
		public virtual void NotifyOfferedCipherSuites(int[] offeredCipherSuites)
		{
			this.mOfferedCipherSuites = offeredCipherSuites;
			this.mEccCipherSuitesOffered = TlsEccUtilities.ContainsEccCipherSuites(this.mOfferedCipherSuites);
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x0010D98B File Offset: 0x0010BB8B
		public virtual void NotifyOfferedCompressionMethods(byte[] offeredCompressionMethods)
		{
			this.mOfferedCompressionMethods = offeredCompressionMethods;
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x0010D994 File Offset: 0x0010BB94
		public virtual void ProcessClientExtensions(IDictionary clientExtensions)
		{
			this.mClientExtensions = clientExtensions;
			if (clientExtensions != null)
			{
				this.mEncryptThenMacOffered = TlsExtensionsUtilities.HasEncryptThenMacExtension(clientExtensions);
				this.mMaxFragmentLengthOffered = TlsExtensionsUtilities.GetMaxFragmentLengthExtension(clientExtensions);
				if (this.mMaxFragmentLengthOffered >= 0 && !MaxFragmentLength.IsValid((byte)this.mMaxFragmentLengthOffered))
				{
					throw new TlsFatalAlert(47);
				}
				this.mTruncatedHMacOffered = TlsExtensionsUtilities.HasTruncatedHMacExtension(clientExtensions);
				this.mSupportedSignatureAlgorithms = TlsUtilities.GetSignatureAlgorithmsExtension(clientExtensions);
				if (this.mSupportedSignatureAlgorithms != null && !TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(this.mClientVersion))
				{
					throw new TlsFatalAlert(47);
				}
				this.mNamedCurves = TlsEccUtilities.GetSupportedEllipticCurvesExtension(clientExtensions);
				this.mClientECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension(clientExtensions);
			}
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x0010DA34 File Offset: 0x0010BC34
		public virtual ProtocolVersion GetServerVersion()
		{
			if (this.MinimumVersion.IsEqualOrEarlierVersionOf(this.mClientVersion))
			{
				ProtocolVersion maximumVersion = this.MaximumVersion;
				if (this.mClientVersion.IsEqualOrEarlierVersionOf(maximumVersion))
				{
					return this.mServerVersion = this.mClientVersion;
				}
				if (this.mClientVersion.IsLaterVersionOf(maximumVersion))
				{
					return this.mServerVersion = maximumVersion;
				}
			}
			throw new TlsFatalAlert(70);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0010DA9C File Offset: 0x0010BC9C
		public virtual int GetSelectedCipherSuite()
		{
			IList usableSignatureAlgorithms = TlsUtilities.GetUsableSignatureAlgorithms(this.mSupportedSignatureAlgorithms);
			bool flag = this.SupportsClientEccCapabilities(this.mNamedCurves, this.mClientECPointFormats);
			foreach (int num in this.GetCipherSuites())
			{
				if (Arrays.Contains(this.mOfferedCipherSuites, num) && (flag || !TlsEccUtilities.IsEccCipherSuite(num)) && TlsUtilities.IsValidCipherSuiteForVersion(num, this.mServerVersion) && TlsUtilities.IsValidCipherSuiteForSignatureAlgorithms(num, usableSignatureAlgorithms))
				{
					return this.mSelectedCipherSuite = num;
				}
			}
			throw new TlsFatalAlert(40);
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0010DB2C File Offset: 0x0010BD2C
		public virtual byte GetSelectedCompressionMethod()
		{
			byte[] compressionMethods = this.GetCompressionMethods();
			for (int i = 0; i < compressionMethods.Length; i++)
			{
				if (Arrays.Contains(this.mOfferedCompressionMethods, compressionMethods[i]))
				{
					return this.mSelectedCompressionMethod = compressionMethods[i];
				}
			}
			throw new TlsFatalAlert(40);
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x0010DB74 File Offset: 0x0010BD74
		public virtual IDictionary GetServerExtensions()
		{
			if (this.mEncryptThenMacOffered && this.AllowEncryptThenMac && TlsUtilities.IsBlockCipherSuite(this.mSelectedCipherSuite))
			{
				TlsExtensionsUtilities.AddEncryptThenMacExtension(this.CheckServerExtensions());
			}
			if (this.mMaxFragmentLengthOffered >= 0 && TlsUtilities.IsValidUint8((int)this.mMaxFragmentLengthOffered) && MaxFragmentLength.IsValid((byte)this.mMaxFragmentLengthOffered))
			{
				TlsExtensionsUtilities.AddMaxFragmentLengthExtension(this.CheckServerExtensions(), (byte)this.mMaxFragmentLengthOffered);
			}
			if (this.mTruncatedHMacOffered && this.AllowTruncatedHMac)
			{
				TlsExtensionsUtilities.AddTruncatedHMacExtension(this.CheckServerExtensions());
			}
			if (this.mClientECPointFormats != null && TlsEccUtilities.IsEccCipherSuite(this.mSelectedCipherSuite))
			{
				this.mServerECPointFormats = new byte[]
				{
					0,
					1,
					2
				};
				TlsEccUtilities.AddSupportedPointFormatsExtension(this.CheckServerExtensions(), this.mServerECPointFormats);
			}
			return this.mServerExtensions;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x0008D54A File Offset: 0x0008B74A
		public virtual IList GetServerSupplementalData()
		{
			return null;
		}

		// Token: 0x0600292F RID: 10543
		public abstract TlsCredentials GetCredentials();

		// Token: 0x06002930 RID: 10544 RVA: 0x0008D54A File Offset: 0x0008B74A
		public virtual CertificateStatus GetCertificateStatus()
		{
			return null;
		}

		// Token: 0x06002931 RID: 10545
		public abstract TlsKeyExchange GetKeyExchange();

		// Token: 0x06002932 RID: 10546 RVA: 0x0008D54A File Offset: 0x0008B74A
		public virtual CertificateRequest GetCertificateRequest()
		{
			return null;
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x0010D4E3 File Offset: 0x0010B6E3
		public virtual void ProcessClientSupplementalData(IList clientSupplementalData)
		{
			if (clientSupplementalData != null)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		public virtual void NotifyClientCertificate(Certificate clientCertificate)
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0010DC3C File Offset: 0x0010BE3C
		public override TlsCompression GetCompression()
		{
			if (this.mSelectedCompressionMethod == 0)
			{
				return new TlsNullCompression();
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x0010DC60 File Offset: 0x0010BE60
		public override TlsCipher GetCipher()
		{
			int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(this.mSelectedCipherSuite);
			int macAlgorithm = TlsUtilities.GetMacAlgorithm(this.mSelectedCipherSuite);
			return this.mCipherFactory.CreateCipher(this.mContext, encryptionAlgorithm, macAlgorithm);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x0010DC98 File Offset: 0x0010BE98
		public virtual NewSessionTicket GetNewSessionTicket()
		{
			return new NewSessionTicket(0L, TlsUtilities.EmptyBytes);
		}

		// Token: 0x04001B21 RID: 6945
		protected TlsCipherFactory mCipherFactory;

		// Token: 0x04001B22 RID: 6946
		protected TlsServerContext mContext;

		// Token: 0x04001B23 RID: 6947
		protected ProtocolVersion mClientVersion;

		// Token: 0x04001B24 RID: 6948
		protected int[] mOfferedCipherSuites;

		// Token: 0x04001B25 RID: 6949
		protected byte[] mOfferedCompressionMethods;

		// Token: 0x04001B26 RID: 6950
		protected IDictionary mClientExtensions;

		// Token: 0x04001B27 RID: 6951
		protected bool mEncryptThenMacOffered;

		// Token: 0x04001B28 RID: 6952
		protected short mMaxFragmentLengthOffered;

		// Token: 0x04001B29 RID: 6953
		protected bool mTruncatedHMacOffered;

		// Token: 0x04001B2A RID: 6954
		protected IList mSupportedSignatureAlgorithms;

		// Token: 0x04001B2B RID: 6955
		protected bool mEccCipherSuitesOffered;

		// Token: 0x04001B2C RID: 6956
		protected int[] mNamedCurves;

		// Token: 0x04001B2D RID: 6957
		protected byte[] mClientECPointFormats;

		// Token: 0x04001B2E RID: 6958
		protected byte[] mServerECPointFormats;

		// Token: 0x04001B2F RID: 6959
		protected ProtocolVersion mServerVersion;

		// Token: 0x04001B30 RID: 6960
		protected int mSelectedCipherSuite;

		// Token: 0x04001B31 RID: 6961
		protected byte mSelectedCompressionMethod;

		// Token: 0x04001B32 RID: 6962
		protected IDictionary mServerExtensions;
	}
}
