using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D0 RID: 976
	public interface IBasicAgreement
	{
		// Token: 0x0600281E RID: 10270
		void Init(ICipherParameters parameters);

		// Token: 0x0600281F RID: 10271
		int GetFieldSize();

		// Token: 0x06002820 RID: 10272
		BigInteger CalculateAgreement(ICipherParameters pubKey);
	}
}
