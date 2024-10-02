using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000553 RID: 1363
	public class ElGamalParametersGenerator
	{
		// Token: 0x0600336A RID: 13162 RVA: 0x00134620 File Offset: 0x00132820
		public void Init(int size, int certainty, SecureRandom random)
		{
			this.size = size;
			this.certainty = certainty;
			this.random = random;
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x00134638 File Offset: 0x00132838
		public ElGamalParameters GenerateParameters()
		{
			BigInteger[] array = DHParametersHelper.GenerateSafePrimes(this.size, this.certainty, this.random);
			BigInteger p = array[0];
			BigInteger q = array[1];
			BigInteger g = DHParametersHelper.SelectGenerator(p, q, this.random);
			return new ElGamalParameters(p, g);
		}

		// Token: 0x040021AA RID: 8618
		private int size;

		// Token: 0x040021AB RID: 8619
		private int certainty;

		// Token: 0x040021AC RID: 8620
		private SecureRandom random;
	}
}
