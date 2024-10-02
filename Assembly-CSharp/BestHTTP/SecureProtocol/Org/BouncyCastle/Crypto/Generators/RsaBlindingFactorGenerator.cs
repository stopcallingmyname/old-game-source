using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000561 RID: 1377
	public class RsaBlindingFactorGenerator
	{
		// Token: 0x060033BB RID: 13243 RVA: 0x00136BEC File Offset: 0x00134DEC
		public void Init(ICipherParameters param)
		{
			if (param is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
				this.key = (RsaKeyParameters)parametersWithRandom.Parameters;
				this.random = parametersWithRandom.Random;
			}
			else
			{
				this.key = (RsaKeyParameters)param;
				this.random = new SecureRandom();
			}
			if (this.key.IsPrivate)
			{
				throw new ArgumentException("generator requires RSA public key");
			}
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x00136C58 File Offset: 0x00134E58
		public BigInteger GenerateBlindingFactor()
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("generator not initialised");
			}
			BigInteger modulus = this.key.Modulus;
			int sizeInBits = modulus.BitLength - 1;
			BigInteger bigInteger;
			BigInteger bigInteger2;
			do
			{
				bigInteger = new BigInteger(sizeInBits, this.random);
				bigInteger2 = bigInteger.Gcd(modulus);
			}
			while (bigInteger.SignValue == 0 || bigInteger.Equals(BigInteger.One) || !bigInteger2.Equals(BigInteger.One));
			return bigInteger;
		}

		// Token: 0x040021CB RID: 8651
		private RsaKeyParameters key;

		// Token: 0x040021CC RID: 8652
		private SecureRandom random;
	}
}
