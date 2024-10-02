using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000469 RID: 1129
	public class TlsECDsaSigner : TlsDsaSigner
	{
		// Token: 0x06002C23 RID: 11299 RVA: 0x00117F16 File Offset: 0x00116116
		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is ECPublicKeyParameters;
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x00117F21 File Offset: 0x00116121
		protected override IDsa CreateDsaImpl(byte hashAlgorithm)
		{
			return new ECDsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06002C25 RID: 11301 RVA: 0x000A89A4 File Offset: 0x000A6BA4
		protected override byte SignatureAlgorithm
		{
			get
			{
				return 3;
			}
		}
	}
}
