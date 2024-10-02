using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000461 RID: 1121
	public class TlsDHKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x06002BB1 RID: 11185 RVA: 0x0011657D File Offset: 0x0011477D
		[Obsolete("Use constructor that takes a TlsDHVerifier")]
		public TlsDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, DHParameters dhParameters) : this(keyExchange, supportedSignatureAlgorithms, new DefaultTlsDHVerifier(), dhParameters)
		{
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x00116590 File Offset: 0x00114790
		public TlsDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, TlsDHVerifier dhVerifier, DHParameters dhParameters) : base(keyExchange, supportedSignatureAlgorithms)
		{
			switch (keyExchange)
			{
			case 3:
				this.mTlsSigner = new TlsDssSigner();
				goto IL_64;
			case 5:
				this.mTlsSigner = new TlsRsaSigner();
				goto IL_64;
			case 7:
			case 9:
			case 11:
				this.mTlsSigner = null;
				goto IL_64;
			}
			throw new InvalidOperationException("unsupported key exchange algorithm");
			IL_64:
			this.mDHVerifier = dhVerifier;
			this.mDHParameters = dhParameters;
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x00116610 File Offset: 0x00114810
		public override void Init(TlsContext context)
		{
			base.Init(context);
			if (this.mTlsSigner != null)
			{
				this.mTlsSigner.Init(context);
			}
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x0011662D File Offset: 0x0011482D
		public override void SkipServerCredentials()
		{
			if (this.mKeyExchange != 11)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x00116644 File Offset: 0x00114844
		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (this.mKeyExchange == 11)
			{
				throw new TlsFatalAlert(10);
			}
			if (serverCertificate.IsEmpty)
			{
				throw new TlsFatalAlert(42);
			}
			X509CertificateStructure certificateAt = serverCertificate.GetCertificateAt(0);
			SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
			try
			{
				this.mServerPublicKey = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			if (this.mTlsSigner == null)
			{
				try
				{
					this.mDHAgreePublicKey = (DHPublicKeyParameters)this.mServerPublicKey;
					this.mDHParameters = this.mDHAgreePublicKey.Parameters;
				}
				catch (InvalidCastException alertCause2)
				{
					throw new TlsFatalAlert(46, alertCause2);
				}
				TlsUtilities.ValidateKeyUsage(certificateAt, 8);
			}
			else
			{
				if (!this.mTlsSigner.IsValidPublicKey(this.mServerPublicKey))
				{
					throw new TlsFatalAlert(46);
				}
				TlsUtilities.ValidateKeyUsage(certificateAt, 128);
			}
			base.ProcessServerCertificate(serverCertificate);
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x00116724 File Offset: 0x00114924
		public override bool RequiresServerKeyExchange
		{
			get
			{
				int mKeyExchange = this.mKeyExchange;
				return mKeyExchange == 3 || mKeyExchange == 5 || mKeyExchange == 11;
			}
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x00116748 File Offset: 0x00114948
		public override byte[] GenerateServerKeyExchange()
		{
			if (!this.RequiresServerKeyExchange)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralServerKeyExchange(this.mContext.SecureRandom, this.mDHParameters, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x00116788 File Offset: 0x00114988
		public override void ProcessServerKeyExchange(Stream input)
		{
			if (!this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(10);
			}
			this.mDHParameters = TlsDHUtilities.ReceiveDHParameters(this.mDHVerifier, input);
			this.mDHAgreePublicKey = new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(input), this.mDHParameters);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x001167C4 File Offset: 0x001149C4
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			if (this.mKeyExchange == 11)
			{
				throw new TlsFatalAlert(40);
			}
			foreach (byte b in certificateRequest.CertificateTypes)
			{
				if (b - 1 > 3 && b != 64)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x0011680F File Offset: 0x00114A0F
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (this.mKeyExchange == 11)
			{
				throw new TlsFatalAlert(80);
			}
			if (clientCredentials is TlsAgreementCredentials)
			{
				this.mAgreementCredentials = (TlsAgreementCredentials)clientCredentials;
				return;
			}
			if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x00116848 File Offset: 0x00114A48
		public override void GenerateClientKeyExchange(Stream output)
		{
			if (this.mAgreementCredentials == null)
			{
				this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange(this.mContext.SecureRandom, this.mDHParameters, output);
			}
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x0011686F File Offset: 0x00114A6F
		public override void ProcessClientCertificate(Certificate clientCertificate)
		{
			if (this.mKeyExchange == 11)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x00116883 File Offset: 0x00114A83
		public override void ProcessClientKeyExchange(Stream input)
		{
			if (this.mDHAgreePublicKey != null)
			{
				return;
			}
			this.mDHAgreePublicKey = new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(input), this.mDHParameters);
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x001168A5 File Offset: 0x00114AA5
		public override byte[] GeneratePremasterSecret()
		{
			if (this.mAgreementCredentials != null)
			{
				return this.mAgreementCredentials.GenerateAgreement(this.mDHAgreePublicKey);
			}
			if (this.mDHAgreePrivateKey != null)
			{
				return TlsDHUtilities.CalculateDHBasicAgreement(this.mDHAgreePublicKey, this.mDHAgreePrivateKey);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x04001E27 RID: 7719
		protected TlsSigner mTlsSigner;

		// Token: 0x04001E28 RID: 7720
		protected TlsDHVerifier mDHVerifier;

		// Token: 0x04001E29 RID: 7721
		protected DHParameters mDHParameters;

		// Token: 0x04001E2A RID: 7722
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x04001E2B RID: 7723
		protected TlsAgreementCredentials mAgreementCredentials;

		// Token: 0x04001E2C RID: 7724
		protected DHPrivateKeyParameters mDHAgreePrivateKey;

		// Token: 0x04001E2D RID: 7725
		protected DHPublicKeyParameters mDHAgreePublicKey;
	}
}
