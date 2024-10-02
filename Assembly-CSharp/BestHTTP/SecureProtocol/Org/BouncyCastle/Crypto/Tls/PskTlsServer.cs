using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000440 RID: 1088
	public class PskTlsServer : AbstractTlsServer
	{
		// Token: 0x06002ADE RID: 10974 RVA: 0x00113921 File Offset: 0x00111B21
		public PskTlsServer(TlsPskIdentityManager pskIdentityManager) : this(new DefaultTlsCipherFactory(), pskIdentityManager)
		{
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x0011392F File Offset: 0x00111B2F
		public PskTlsServer(TlsCipherFactory cipherFactory, TlsPskIdentityManager pskIdentityManager) : base(cipherFactory)
		{
			this.mPskIdentityManager = pskIdentityManager;
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		protected virtual TlsEncryptionCredentials GetRsaEncryptionCredentials()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x0010F935 File Offset: 0x0010DB35
		protected virtual DHParameters GetDHParameters()
		{
			return DHStandardGroups.rfc7919_ffdhe2048;
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x0011393F File Offset: 0x00111B3F
		protected override int[] GetCipherSuites()
		{
			return new int[]
			{
				49207,
				49205,
				178,
				144
			};
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x00113954 File Offset: 0x00111B54
		public override TlsCredentials GetCredentials()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			if (keyExchangeAlgorithm - 13 > 1)
			{
				if (keyExchangeAlgorithm == 15)
				{
					return this.GetRsaEncryptionCredentials();
				}
				if (keyExchangeAlgorithm != 24)
				{
					throw new TlsFatalAlert(80);
				}
			}
			return null;
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x00113990 File Offset: 0x00111B90
		public override TlsKeyExchange GetKeyExchange()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			if (keyExchangeAlgorithm - 13 <= 2 || keyExchangeAlgorithm == 24)
			{
				return this.CreatePskKeyExchange(keyExchangeAlgorithm);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x001139C4 File Offset: 0x00111BC4
		protected virtual TlsKeyExchange CreatePskKeyExchange(int keyExchange)
		{
			return new TlsPskKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, null, this.mPskIdentityManager, null, this.GetDHParameters(), this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x04001DAD RID: 7597
		protected TlsPskIdentityManager mPskIdentityManager;
	}
}
