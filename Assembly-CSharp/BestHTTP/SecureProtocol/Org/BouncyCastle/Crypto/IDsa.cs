using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D8 RID: 984
	public interface IDsa
	{
		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06002845 RID: 10309
		string AlgorithmName { get; }

		// Token: 0x06002846 RID: 10310
		void Init(bool forSigning, ICipherParameters parameters);

		// Token: 0x06002847 RID: 10311
		BigInteger[] GenerateSignature(byte[] message);

		// Token: 0x06002848 RID: 10312
		bool VerifySignature(byte[] message, BigInteger r, BigInteger s);
	}
}
