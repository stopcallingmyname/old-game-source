using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000549 RID: 1353
	internal class DHKeyGeneratorHelper
	{
		// Token: 0x06003337 RID: 13111 RVA: 0x00022F1F File Offset: 0x0002111F
		private DHKeyGeneratorHelper()
		{
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x0013373C File Offset: 0x0013193C
		internal BigInteger CalculatePrivate(DHParameters dhParams, SecureRandom random)
		{
			int l = dhParams.L;
			if (l != 0)
			{
				int num = l >> 2;
				BigInteger bigInteger;
				do
				{
					bigInteger = new BigInteger(l, random).SetBit(l - 1);
				}
				while (WNafUtilities.GetNafWeight(bigInteger) < num);
				return bigInteger;
			}
			BigInteger min = BigInteger.Two;
			int m = dhParams.M;
			if (m != 0)
			{
				min = BigInteger.One.ShiftLeft(m - 1);
			}
			BigInteger bigInteger2 = dhParams.Q;
			if (bigInteger2 == null)
			{
				bigInteger2 = dhParams.P;
			}
			BigInteger bigInteger3 = bigInteger2.Subtract(BigInteger.Two);
			int num2 = bigInteger3.BitLength >> 2;
			BigInteger bigInteger4;
			do
			{
				bigInteger4 = BigIntegers.CreateRandomInRange(min, bigInteger3, random);
			}
			while (WNafUtilities.GetNafWeight(bigInteger4) < num2);
			return bigInteger4;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x001337D6 File Offset: 0x001319D6
		internal BigInteger CalculatePublic(DHParameters dhParams, BigInteger x)
		{
			return dhParams.G.ModPow(x, dhParams.P);
		}

		// Token: 0x04002191 RID: 8593
		internal static readonly DHKeyGeneratorHelper Instance = new DHKeyGeneratorHelper();
	}
}
