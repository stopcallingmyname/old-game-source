using System;
using System.Collections;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F6 RID: 1014
	public abstract class AbstractTlsKeyExchange : TlsKeyExchange
	{
		// Token: 0x06002900 RID: 10496 RVA: 0x0010D727 File Offset: 0x0010B927
		protected AbstractTlsKeyExchange(int keyExchange, IList supportedSignatureAlgorithms)
		{
			this.mKeyExchange = keyExchange;
			this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x0010D740 File Offset: 0x0010B940
		protected virtual DigitallySigned ParseSignature(Stream input)
		{
			DigitallySigned digitallySigned = DigitallySigned.Parse(this.mContext, input);
			SignatureAndHashAlgorithm algorithm = digitallySigned.Algorithm;
			if (algorithm != null)
			{
				TlsUtilities.VerifySupportedSignatureAlgorithm(this.mSupportedSignatureAlgorithms, algorithm);
			}
			return digitallySigned;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0010D770 File Offset: 0x0010B970
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
			ProtocolVersion clientVersion = context.ClientVersion;
			if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(clientVersion))
			{
				if (this.mSupportedSignatureAlgorithms == null)
				{
					switch (this.mKeyExchange)
					{
					case 1:
					case 5:
					case 9:
					case 15:
					case 18:
					case 19:
					case 23:
						this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultRsaSignatureAlgorithms();
						return;
					case 3:
					case 7:
					case 22:
						this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultDssSignatureAlgorithms();
						return;
					case 13:
					case 14:
					case 21:
					case 24:
						return;
					case 16:
					case 17:
						this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultECDsaSignatureAlgorithms();
						return;
					}
					throw new InvalidOperationException("unsupported key exchange algorithm");
				}
			}
			else if (this.mSupportedSignatureAlgorithms != null)
			{
				throw new InvalidOperationException("supported_signature_algorithms not allowed for " + clientVersion);
			}
		}

		// Token: 0x06002903 RID: 10499
		public abstract void SkipServerCredentials();

		// Token: 0x06002904 RID: 10500 RVA: 0x0010D85A File Offset: 0x0010BA5A
		public virtual void ProcessServerCertificate(Certificate serverCertificate)
		{
			IList list = this.mSupportedSignatureAlgorithms;
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x0010D863 File Offset: 0x0010BA63
		public virtual void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			this.ProcessServerCertificate(serverCredentials.Certificate);
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002906 RID: 10502 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool RequiresServerKeyExchange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x0010D871 File Offset: 0x0010BA71
		public virtual byte[] GenerateServerKeyExchange()
		{
			if (this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(80);
			}
			return null;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x0010D884 File Offset: 0x0010BA84
		public virtual void SkipServerKeyExchange()
		{
			if (this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x0010D896 File Offset: 0x0010BA96
		public virtual void ProcessServerKeyExchange(Stream input)
		{
			if (!this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x0600290A RID: 10506
		public abstract void ValidateCertificateRequest(CertificateRequest certificateRequest);

		// Token: 0x0600290B RID: 10507 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void SkipClientCredentials()
		{
		}

		// Token: 0x0600290C RID: 10508
		public abstract void ProcessClientCredentials(TlsCredentials clientCredentials);

		// Token: 0x0600290D RID: 10509 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void ProcessClientCertificate(Certificate clientCertificate)
		{
		}

		// Token: 0x0600290E RID: 10510
		public abstract void GenerateClientKeyExchange(Stream output);

		// Token: 0x0600290F RID: 10511 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		public virtual void ProcessClientKeyExchange(Stream input)
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002910 RID: 10512
		public abstract byte[] GeneratePremasterSecret();

		// Token: 0x04001B1E RID: 6942
		protected readonly int mKeyExchange;

		// Token: 0x04001B1F RID: 6943
		protected IList mSupportedSignatureAlgorithms;

		// Token: 0x04001B20 RID: 6944
		protected TlsContext mContext;
	}
}
