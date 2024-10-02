using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F9 RID: 1273
	public class RC5Parameters : KeyParameter
	{
		// Token: 0x0600309D RID: 12445 RVA: 0x00127B8F File Offset: 0x00125D8F
		public RC5Parameters(byte[] key, int rounds) : base(key)
		{
			if (key.Length > 255)
			{
				throw new ArgumentException("RC5 key length can be no greater than 255");
			}
			this.rounds = rounds;
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x00127BB4 File Offset: 0x00125DB4
		public int Rounds
		{
			get
			{
				return this.rounds;
			}
		}

		// Token: 0x04002022 RID: 8226
		private readonly int rounds;
	}
}
