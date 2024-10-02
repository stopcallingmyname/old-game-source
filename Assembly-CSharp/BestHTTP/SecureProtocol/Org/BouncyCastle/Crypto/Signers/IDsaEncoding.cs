using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A0 RID: 1184
	public interface IDsaEncoding
	{
		// Token: 0x06002E6E RID: 11886
		BigInteger[] Decode(BigInteger n, byte[] encoding);

		// Token: 0x06002E6F RID: 11887
		byte[] Encode(BigInteger n, BigInteger r, BigInteger s);
	}
}
