using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200054B RID: 1355
	public class DHParametersGenerator
	{
		// Token: 0x0600333E RID: 13118 RVA: 0x0013384E File Offset: 0x00131A4E
		public virtual void Init(int size, int certainty, SecureRandom random)
		{
			this.size = size;
			this.certainty = certainty;
			this.random = random;
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x00133868 File Offset: 0x00131A68
		public virtual DHParameters GenerateParameters()
		{
			BigInteger[] array = DHParametersHelper.GenerateSafePrimes(this.size, this.certainty, this.random);
			BigInteger p = array[0];
			BigInteger q = array[1];
			BigInteger g = DHParametersHelper.SelectGenerator(p, q, this.random);
			return new DHParameters(p, g, q, BigInteger.Two, null);
		}

		// Token: 0x04002193 RID: 8595
		private int size;

		// Token: 0x04002194 RID: 8596
		private int certainty;

		// Token: 0x04002195 RID: 8597
		private SecureRandom random;
	}
}
