using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003E1 RID: 993
	public interface ISigner
	{
		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06002863 RID: 10339
		string AlgorithmName { get; }

		// Token: 0x06002864 RID: 10340
		void Init(bool forSigning, ICipherParameters parameters);

		// Token: 0x06002865 RID: 10341
		void Update(byte input);

		// Token: 0x06002866 RID: 10342
		void BlockUpdate(byte[] input, int inOff, int length);

		// Token: 0x06002867 RID: 10343
		byte[] GenerateSignature();

		// Token: 0x06002868 RID: 10344
		bool VerifySignature(byte[] signature);

		// Token: 0x06002869 RID: 10345
		void Reset();
	}
}
