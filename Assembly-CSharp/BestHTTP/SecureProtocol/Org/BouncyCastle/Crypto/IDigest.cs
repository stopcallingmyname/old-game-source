using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D7 RID: 983
	public interface IDigest
	{
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x0600283E RID: 10302
		string AlgorithmName { get; }

		// Token: 0x0600283F RID: 10303
		int GetDigestSize();

		// Token: 0x06002840 RID: 10304
		int GetByteLength();

		// Token: 0x06002841 RID: 10305
		void Update(byte input);

		// Token: 0x06002842 RID: 10306
		void BlockUpdate(byte[] input, int inOff, int length);

		// Token: 0x06002843 RID: 10307
		int DoFinal(byte[] output, int outOff);

		// Token: 0x06002844 RID: 10308
		void Reset();
	}
}
