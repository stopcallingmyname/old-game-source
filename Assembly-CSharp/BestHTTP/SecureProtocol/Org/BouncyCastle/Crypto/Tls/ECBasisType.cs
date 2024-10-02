using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000429 RID: 1065
	public abstract class ECBasisType
	{
		// Token: 0x06002A96 RID: 10902 RVA: 0x00113119 File Offset: 0x00111319
		public static bool IsValid(byte ecBasisType)
		{
			return ecBasisType >= 1 && ecBasisType <= 2;
		}

		// Token: 0x04001CEE RID: 7406
		public const byte ec_basis_trinomial = 1;

		// Token: 0x04001CEF RID: 7407
		public const byte ec_basis_pentanomial = 2;
	}
}
