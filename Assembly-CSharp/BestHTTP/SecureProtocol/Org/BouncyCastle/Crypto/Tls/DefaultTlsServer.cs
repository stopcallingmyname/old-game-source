using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000419 RID: 1049
	public abstract class DefaultTlsServer : AbstractTlsServer
	{
		// Token: 0x060029F8 RID: 10744 RVA: 0x0010F924 File Offset: 0x0010DB24
		public DefaultTlsServer()
		{
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x0010F92C File Offset: 0x0010DB2C
		public DefaultTlsServer(TlsCipherFactory cipherFactory) : base(cipherFactory)
		{
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		protected virtual TlsSignerCredentials GetDsaSignerCredentials()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		protected virtual TlsSignerCredentials GetECDsaSignerCredentials()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		protected virtual TlsEncryptionCredentials GetRsaEncryptionCredentials()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		protected virtual TlsSignerCredentials GetRsaSignerCredentials()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x0010F935 File Offset: 0x0010DB35
		protected virtual DHParameters GetDHParameters()
		{
			return DHStandardGroups.rfc7919_ffdhe2048;
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x0010F93C File Offset: 0x0010DB3C
		protected override int[] GetCipherSuites()
		{
			return new int[]
			{
				49200,
				49199,
				49192,
				49191,
				49172,
				49171,
				159,
				158,
				107,
				103,
				57,
				51,
				157,
				156,
				61,
				60,
				53,
				47
			};
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x0010F950 File Offset: 0x0010DB50
		public override TlsCredentials GetCredentials()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			switch (keyExchangeAlgorithm)
			{
			case 1:
				return this.GetRsaEncryptionCredentials();
			case 2:
			case 4:
				goto IL_66;
			case 3:
				return this.GetDsaSignerCredentials();
			case 5:
				break;
			default:
				if (keyExchangeAlgorithm != 11)
				{
					switch (keyExchangeAlgorithm)
					{
					case 17:
						return this.GetECDsaSignerCredentials();
					case 18:
						goto IL_66;
					case 19:
						goto IL_58;
					case 20:
						break;
					default:
						goto IL_66;
					}
				}
				return null;
			}
			IL_58:
			return this.GetRsaSignerCredentials();
			IL_66:
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x0010F9CC File Offset: 0x0010DBCC
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

		// Token: 0x06002A02 RID: 10754 RVA: 0x0010FA6D File Offset: 0x0010DC6D
		protected virtual TlsKeyExchange CreateDHKeyExchange(int keyExchange)
		{
			return new TlsDHKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, null, this.GetDHParameters());
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x0010FA82 File Offset: 0x0010DC82
		protected virtual TlsKeyExchange CreateDheKeyExchange(int keyExchange)
		{
			return new TlsDheKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, null, this.GetDHParameters());
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x0010FA97 File Offset: 0x0010DC97
		protected virtual TlsKeyExchange CreateECDHKeyExchange(int keyExchange)
		{
			return new TlsECDHKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x0010FAB7 File Offset: 0x0010DCB7
		protected virtual TlsKeyExchange CreateECDheKeyExchange(int keyExchange)
		{
			return new TlsECDheKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mNamedCurves, this.mClientECPointFormats, this.mServerECPointFormats);
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x0010FAD7 File Offset: 0x0010DCD7
		protected virtual TlsKeyExchange CreateRsaKeyExchange()
		{
			return new TlsRsaKeyExchange(this.mSupportedSignatureAlgorithms);
		}
	}
}
