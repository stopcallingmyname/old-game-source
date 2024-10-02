using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000438 RID: 1080
	public abstract class MaxFragmentLength
	{
		// Token: 0x06002AB4 RID: 10932 RVA: 0x001133A6 File Offset: 0x001115A6
		public static bool IsValid(byte maxFragmentLength)
		{
			return maxFragmentLength >= 1 && maxFragmentLength <= 4;
		}

		// Token: 0x04001D79 RID: 7545
		public const byte pow2_9 = 1;

		// Token: 0x04001D7A RID: 7546
		public const byte pow2_10 = 2;

		// Token: 0x04001D7B RID: 7547
		public const byte pow2_11 = 3;

		// Token: 0x04001D7C RID: 7548
		public const byte pow2_12 = 4;
	}
}
