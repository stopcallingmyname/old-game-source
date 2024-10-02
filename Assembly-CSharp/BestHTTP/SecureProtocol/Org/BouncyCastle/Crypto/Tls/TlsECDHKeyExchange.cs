using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000468 RID: 1128
	public class TlsECDHKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x06002C16 RID: 11286 RVA: 0x00117B90 File Offset: 0x00115D90
		public TlsECDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats) : base(keyExchange, supportedSignatureAlgorithms)
		{
			switch (keyExchange)
			{
			case 16:
			case 18:
			case 20:
				this.mTlsSigner = null;
				break;
			case 17:
				this.mTlsSigner = new TlsECDsaSigner();
				break;
			case 19:
				this.mTlsSigner = new TlsRsaSigner();
				break;
			default:
				throw new InvalidOperationException("unsupported key exchange algorithm");
			}
			this.mNamedCurves = namedCurves;
			this.mClientECPointFormats = clientECPointFormats;
			this.mServerECPointFormats = serverECPointFormats;
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x00117C09 File Offset: 0x00115E09
		public override void Init(TlsContext context)
		{
			base.Init(context);
			if (this.mTlsSigner != null)
			{
				this.mTlsSigner.Init(context);
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x00117C26 File Offset: 0x00115E26
		public override void SkipServerCredentials()
		{
			if (this.mKeyExchange != 20)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x00117C3C File Offset: 0x00115E3C
		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (this.mKeyExchange == 20)
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
					this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey((ECPublicKeyParameters)this.mServerPublicKey);
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

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002C1A RID: 11290 RVA: 0x00117D10 File Offset: 0x00115F10
		public override bool RequiresServerKeyExchange
		{
			get
			{
				int mKeyExchange = this.mKeyExchange;
				return mKeyExchange == 17 || mKeyExchange - 19 <= 1;
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x00117D34 File Offset: 0x00115F34
		public override byte[] GenerateServerKeyExchange()
		{
			if (!this.RequiresServerKeyExchange)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange(this.mContext.SecureRandom, this.mNamedCurves, this.mClientECPointFormats, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x00117D7C File Offset: 0x00115F7C
		public override void ProcessServerKeyExchange(Stream input)
		{
			if (!this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(10);
			}
			ECDomainParameters curve_params = TlsEccUtilities.ReadECParameters(this.mNamedCurves, this.mClientECPointFormats, input);
			byte[] encoding = TlsUtilities.ReadOpaque8(input);
			this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mClientECPointFormats, curve_params, encoding));
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x00117DCC File Offset: 0x00115FCC
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			if (this.mKeyExchange == 20)
			{
				throw new TlsFatalAlert(40);
			}
			foreach (byte b in certificateRequest.CertificateTypes)
			{
				if (b - 1 > 1 && b - 64 > 2)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x00117E19 File Offset: 0x00116019
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (this.mKeyExchange == 20)
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

		// Token: 0x06002C1F RID: 11295 RVA: 0x00117E52 File Offset: 0x00116052
		public override void GenerateClientKeyExchange(Stream output)
		{
			if (this.mAgreementCredentials == null)
			{
				this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralClientKeyExchange(this.mContext.SecureRandom, this.mServerECPointFormats, this.mECAgreePublicKey.Parameters, output);
			}
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x00117E84 File Offset: 0x00116084
		public override void ProcessClientCertificate(Certificate clientCertificate)
		{
			if (this.mKeyExchange == 20)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x00117E98 File Offset: 0x00116098
		public override void ProcessClientKeyExchange(Stream input)
		{
			if (this.mECAgreePublicKey != null)
			{
				return;
			}
			byte[] encoding = TlsUtilities.ReadOpaque8(input);
			ECDomainParameters parameters = this.mECAgreePrivateKey.Parameters;
			this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mServerECPointFormats, parameters, encoding));
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x00117ED9 File Offset: 0x001160D9
		public override byte[] GeneratePremasterSecret()
		{
			if (this.mAgreementCredentials != null)
			{
				return this.mAgreementCredentials.GenerateAgreement(this.mECAgreePublicKey);
			}
			if (this.mECAgreePrivateKey != null)
			{
				return TlsEccUtilities.CalculateECDHBasicAgreement(this.mECAgreePublicKey, this.mECAgreePrivateKey);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x04001E3B RID: 7739
		protected TlsSigner mTlsSigner;

		// Token: 0x04001E3C RID: 7740
		protected int[] mNamedCurves;

		// Token: 0x04001E3D RID: 7741
		protected byte[] mClientECPointFormats;

		// Token: 0x04001E3E RID: 7742
		protected byte[] mServerECPointFormats;

		// Token: 0x04001E3F RID: 7743
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x04001E40 RID: 7744
		protected TlsAgreementCredentials mAgreementCredentials;

		// Token: 0x04001E41 RID: 7745
		protected ECPrivateKeyParameters mECAgreePrivateKey;

		// Token: 0x04001E42 RID: 7746
		protected ECPublicKeyParameters mECAgreePublicKey;
	}
}
