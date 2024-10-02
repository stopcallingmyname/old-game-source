using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003EA RID: 1002
	public class KeyGenerationParameters
	{
		// Token: 0x0600287F RID: 10367 RVA: 0x0010CAEA File Offset: 0x0010ACEA
		public KeyGenerationParameters(SecureRandom random, int strength)
		{
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			if (strength < 1)
			{
				throw new ArgumentException("strength must be a positive value", "strength");
			}
			this.random = random;
			this.strength = strength;
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002880 RID: 10368 RVA: 0x0010CB22 File Offset: 0x0010AD22
		public SecureRandom Random
		{
			get
			{
				return this.random;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002881 RID: 10369 RVA: 0x0010CB2A File Offset: 0x0010AD2A
		public int Strength
		{
			get
			{
				return this.strength;
			}
		}

		// Token: 0x04001B06 RID: 6918
		private SecureRandom random;

		// Token: 0x04001B07 RID: 6919
		private int strength;
	}
}
