using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200044D RID: 1101
	public class SrpTlsServer : AbstractTlsServer
	{
		// Token: 0x06002B46 RID: 11078 RVA: 0x001147BF File Offset: 0x001129BF
		public SrpTlsServer(TlsSrpIdentityManager srpIdentityManager) : this(new DefaultTlsCipherFactory(), srpIdentityManager)
		{
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x001147CD File Offset: 0x001129CD
		public SrpTlsServer(TlsCipherFactory cipherFactory, TlsSrpIdentityManager srpIdentityManager) : base(cipherFactory)
		{
			this.mSrpIdentityManager = srpIdentityManager;
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		protected virtual TlsSignerCredentials GetDsaSignerCredentials()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		protected virtual TlsSignerCredentials GetRsaSignerCredentials()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x001147DD File Offset: 0x001129DD
		protected override int[] GetCipherSuites()
		{
			return new int[]
			{
				49186,
				49183,
				49185,
				49182,
				49184,
				49181
			};
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x001147F0 File Offset: 0x001129F0
		public override void ProcessClientExtensions(IDictionary clientExtensions)
		{
			base.ProcessClientExtensions(clientExtensions);
			this.mSrpIdentity = TlsSrpUtilities.GetSrpExtension(clientExtensions);
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x00114805 File Offset: 0x00112A05
		public override int GetSelectedCipherSuite()
		{
			int selectedCipherSuite = base.GetSelectedCipherSuite();
			if (TlsSrpUtilities.IsSrpCipherSuite(selectedCipherSuite))
			{
				if (this.mSrpIdentity != null)
				{
					this.mLoginParameters = this.mSrpIdentityManager.GetLoginParameters(this.mSrpIdentity);
				}
				if (this.mLoginParameters == null)
				{
					throw new TlsFatalAlert(115);
				}
			}
			return selectedCipherSuite;
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x00114844 File Offset: 0x00112A44
		public override TlsCredentials GetCredentials()
		{
			switch (TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite))
			{
			case 21:
				return null;
			case 22:
				return this.GetDsaSignerCredentials();
			case 23:
				return this.GetRsaSignerCredentials();
			default:
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0011488C File Offset: 0x00112A8C
		public override TlsKeyExchange GetKeyExchange()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			if (keyExchangeAlgorithm - 21 <= 2)
			{
				return this.CreateSrpKeyExchange(keyExchangeAlgorithm);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x001148BB File Offset: 0x00112ABB
		protected virtual TlsKeyExchange CreateSrpKeyExchange(int keyExchange)
		{
			return new TlsSrpKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mSrpIdentity, this.mLoginParameters);
		}

		// Token: 0x04001DF3 RID: 7667
		protected TlsSrpIdentityManager mSrpIdentityManager;

		// Token: 0x04001DF4 RID: 7668
		protected byte[] mSrpIdentity;

		// Token: 0x04001DF5 RID: 7669
		protected TlsSrpLoginParameters mLoginParameters;
	}
}
