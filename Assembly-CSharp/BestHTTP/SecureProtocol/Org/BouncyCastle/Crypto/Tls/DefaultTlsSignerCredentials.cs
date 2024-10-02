using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200041A RID: 1050
	public class DefaultTlsSignerCredentials : AbstractTlsSignerCredentials
	{
		// Token: 0x06002A07 RID: 10759 RVA: 0x0010FAE4 File Offset: 0x0010DCE4
		public DefaultTlsSignerCredentials(TlsContext context, Certificate certificate, AsymmetricKeyParameter privateKey) : this(context, certificate, privateKey, null)
		{
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x0010FAF0 File Offset: 0x0010DCF0
		public DefaultTlsSignerCredentials(TlsContext context, Certificate certificate, AsymmetricKeyParameter privateKey, SignatureAndHashAlgorithm signatureAndHashAlgorithm)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (certificate.IsEmpty)
			{
				throw new ArgumentException("cannot be empty", "clientCertificate");
			}
			if (privateKey == null)
			{
				throw new ArgumentNullException("privateKey");
			}
			if (!privateKey.IsPrivate)
			{
				throw new ArgumentException("must be private", "privateKey");
			}
			if (TlsUtilities.IsTlsV12(context) && signatureAndHashAlgorithm == null)
			{
				throw new ArgumentException("cannot be null for (D)TLS 1.2+", "signatureAndHashAlgorithm");
			}
			if (privateKey is RsaKeyParameters)
			{
				this.mSigner = new TlsRsaSigner();
			}
			else if (privateKey is DsaPrivateKeyParameters)
			{
				this.mSigner = new TlsDssSigner();
			}
			else
			{
				if (!(privateKey is ECPrivateKeyParameters))
				{
					throw new ArgumentException("type not supported: " + Platform.GetTypeName(privateKey), "privateKey");
				}
				this.mSigner = new TlsECDsaSigner();
			}
			this.mSigner.Init(context);
			this.mContext = context;
			this.mCertificate = certificate;
			this.mPrivateKey = privateKey;
			this.mSignatureAndHashAlgorithm = signatureAndHashAlgorithm;
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002A09 RID: 10761 RVA: 0x0010FBEE File Offset: 0x0010DDEE
		public override Certificate Certificate
		{
			get
			{
				return this.mCertificate;
			}
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x0010FBF8 File Offset: 0x0010DDF8
		public override byte[] GenerateCertificateSignature(byte[] hash)
		{
			byte[] result;
			try
			{
				if (TlsUtilities.IsTlsV12(this.mContext))
				{
					result = this.mSigner.GenerateRawSignature(this.mSignatureAndHashAlgorithm, this.mPrivateKey, hash);
				}
				else
				{
					result = this.mSigner.GenerateRawSignature(this.mPrivateKey, hash);
				}
			}
			catch (CryptoException alertCause)
			{
				throw new TlsFatalAlert(80, alertCause);
			}
			return result;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002A0B RID: 10763 RVA: 0x0010FC60 File Offset: 0x0010DE60
		public override SignatureAndHashAlgorithm SignatureAndHashAlgorithm
		{
			get
			{
				return this.mSignatureAndHashAlgorithm;
			}
		}

		// Token: 0x04001CB3 RID: 7347
		protected readonly TlsContext mContext;

		// Token: 0x04001CB4 RID: 7348
		protected readonly Certificate mCertificate;

		// Token: 0x04001CB5 RID: 7349
		protected readonly AsymmetricKeyParameter mPrivateKey;

		// Token: 0x04001CB6 RID: 7350
		protected readonly SignatureAndHashAlgorithm mSignatureAndHashAlgorithm;

		// Token: 0x04001CB7 RID: 7351
		protected readonly TlsSigner mSigner;
	}
}
