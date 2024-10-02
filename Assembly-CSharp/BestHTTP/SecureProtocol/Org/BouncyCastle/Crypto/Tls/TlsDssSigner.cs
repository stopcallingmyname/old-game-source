using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000465 RID: 1125
	public class TlsDssSigner : TlsDsaSigner
	{
		// Token: 0x06002BE2 RID: 11234 RVA: 0x00116F9C File Offset: 0x0011519C
		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is DsaPublicKeyParameters;
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00116FA7 File Offset: 0x001151A7
		protected override IDsa CreateDsaImpl(byte hashAlgorithm)
		{
			return new DsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002BE4 RID: 11236 RVA: 0x000A7398 File Offset: 0x000A5598
		protected override byte SignatureAlgorithm
		{
			get
			{
				return 2;
			}
		}
	}
}
