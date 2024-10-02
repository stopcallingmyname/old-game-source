using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200043F RID: 1087
	public class PskTlsClient : AbstractTlsClient
	{
		// Token: 0x06002AD7 RID: 10967 RVA: 0x00113865 File Offset: 0x00111A65
		public PskTlsClient(TlsPskIdentity pskIdentity) : this(new DefaultTlsCipherFactory(), pskIdentity)
		{
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x00113873 File Offset: 0x00111A73
		public PskTlsClient(TlsCipherFactory cipherFactory, TlsPskIdentity pskIdentity) : this(cipherFactory, new DefaultTlsDHVerifier(), pskIdentity)
		{
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x00113882 File Offset: 0x00111A82
		public PskTlsClient(TlsCipherFactory cipherFactory, TlsDHVerifier dhVerifier, TlsPskIdentity pskIdentity) : base(cipherFactory)
		{
			this.mDHVerifier = dhVerifier;
			this.mPskIdentity = pskIdentity;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x00113899 File Offset: 0x00111A99
		public override int[] GetCipherSuites()
		{
			return new int[]
			{
				49207,
				49205
			};
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x001138B4 File Offset: 0x00111AB4
		public override TlsKeyExchange GetKeyExchange()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			if (keyExchangeAlgorithm - 13 <= 2 || keyExchangeAlgorithm == 24)
			{
				return this.CreatePskKeyExchange(keyExchangeAlgorithm);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		public override TlsAuthentication GetAuthentication()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x001138E8 File Offset: 0x00111AE8
		protected virtual TlsKeyExchange CreatePskKeyExchange(int keyExchange)
		{
			return new TlsPskKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mPskIdentity, null, this.mDHVerifier, null, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x04001DAB RID: 7595
		protected TlsDHVerifier mDHVerifier;

		// Token: 0x04001DAC RID: 7596
		protected TlsPskIdentity mPskIdentity;
	}
}
