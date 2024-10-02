using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F0 RID: 1264
	public class NaccacheSternKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06003076 RID: 12406 RVA: 0x00127903 File Offset: 0x00125B03
		public NaccacheSternKeyGenerationParameters(SecureRandom random, int strength, int certainty, int countSmallPrimes) : base(random, strength)
		{
			if (countSmallPrimes % 2 == 1)
			{
				throw new ArgumentException("countSmallPrimes must be a multiple of 2");
			}
			if (countSmallPrimes < 30)
			{
				throw new ArgumentException("countSmallPrimes must be >= 30 for security reasons");
			}
			this.certainty = certainty;
			this.countSmallPrimes = countSmallPrimes;
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x0012793F File Offset: 0x00125B3F
		[Obsolete("Use version without 'debug' parameter")]
		public NaccacheSternKeyGenerationParameters(SecureRandom random, int strength, int certainty, int countSmallPrimes, bool debug) : this(random, strength, certainty, countSmallPrimes)
		{
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x0012794C File Offset: 0x00125B4C
		public int Certainty
		{
			get
			{
				return this.certainty;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x00127954 File Offset: 0x00125B54
		public int CountSmallPrimes
		{
			get
			{
				return this.countSmallPrimes;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x0007D96F File Offset: 0x0007BB6F
		[Obsolete("Remove: always false")]
		public bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04002010 RID: 8208
		private readonly int certainty;

		// Token: 0x04002011 RID: 8209
		private readonly int countSmallPrimes;
	}
}
