using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003DC RID: 988
	public interface IMac
	{
		// Token: 0x0600284E RID: 10318
		void Init(ICipherParameters parameters);

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x0600284F RID: 10319
		string AlgorithmName { get; }

		// Token: 0x06002850 RID: 10320
		int GetMacSize();

		// Token: 0x06002851 RID: 10321
		void Update(byte input);

		// Token: 0x06002852 RID: 10322
		void BlockUpdate(byte[] input, int inOff, int len);

		// Token: 0x06002853 RID: 10323
		int DoFinal(byte[] output, int outOff);

		// Token: 0x06002854 RID: 10324
		void Reset();
	}
}
