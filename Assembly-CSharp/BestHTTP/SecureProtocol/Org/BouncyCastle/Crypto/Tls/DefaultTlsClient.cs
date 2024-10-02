using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000416 RID: 1046
	public abstract class DefaultTlsClient : AbstractTlsClient
	{
		// Token: 0x060029E0 RID: 10720 RVA: 0x0010F56D File Offset: 0x0010D76D
		public DefaultTlsClient() : this(new DefaultTlsCipherFactory())
		{
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x0010F57A File Offset: 0x0010D77A
		public DefaultTlsClient(TlsCipherFactory cipherFactory) : this(cipherFactory, new DefaultTlsDHVerifier())
		{
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x0010F588 File Offset: 0x0010D788
		public DefaultTlsClient(TlsCipherFactory cipherFactory, TlsDHVerifier dhVerifier) : base(cipherFactory)
		{
			this.mDHVerifier = dhVerifier;
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x0010F598 File Offset: 0x0010D798
		public override int[] GetCipherSuites()
		{
			return new int[]
			{
				49195,
				49187,
				49161,
				49199,
				49191,
				49171,
				156,
				60,
				47
			};
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x0010F5AC File Offset: 0x0010D7AC
		public override TlsKeyExchange GetKeyExchange()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			switch (keyExchangeAlgorithm)
			{
			case 1:
				return this.CreateRsaKeyExchange();
			case 3:
			case 5:
				return this.CreateDheKeyExchange(keyExchangeAlgorithm);
			case 7:
			case 9:
			case 11:
				return this.CreateDHKeyExchange(keyExchangeAlgorithm);
			case 16:
			case 18:
			case 20:
				return this.CreateECDHKeyExchange(keyExchangeAlgorithm);
			case 17:
			case 19:
				return this.CreateECDheKeyExchange(keyExchangeAlgorithm);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x0010F64D File Offset: 0x0010D84D
		protected virtual TlsKeyExchange CreateDHKeyExchange(int keyExchange)
		{
			return new TlsDHKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mDHVerifier, null);
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x0010F662 File Offset: 0x0010D862
		protected virtual TlsKeyExchange CreateDheKeyExchange(int keyExchange)
		{
			return new TlsDheKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mDHVerifier, null);
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x0010F677 File Offset: 0x0010D877
		protected virtual TlsKeyExchange CreateECDHKeyExchange(int keyExchange)
		{
			return new TlsECDHKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x0010F697 File Offset: 0x0010D897
		protected virtual TlsKeyExchange CreateECDheKeyExchange(int keyExchange)
		{
			return new TlsECDheKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x0010F6B7 File Offset: 0x0010D8B7
		protected virtual TlsKeyExchange CreateRsaKeyExchange()
		{
			return new TlsRsaKeyExchange(this.mSupportedSignatureAlgorithms);
		}

		// Token: 0x04001CAB RID: 7339
		protected TlsDHVerifier mDHVerifier;
	}
}
