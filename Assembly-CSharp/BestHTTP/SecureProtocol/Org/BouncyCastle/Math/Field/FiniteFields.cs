using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x02000314 RID: 788
	public abstract class FiniteFields
	{
		// Token: 0x06001DE7 RID: 7655 RVA: 0x000E0FB0 File Offset: 0x000DF1B0
		public static IPolynomialExtensionField GetBinaryExtensionField(int[] exponents)
		{
			if (exponents[0] != 0)
			{
				throw new ArgumentException("Irreducible polynomials in GF(2) must have constant term", "exponents");
			}
			for (int i = 1; i < exponents.Length; i++)
			{
				if (exponents[i] <= exponents[i - 1])
				{
					throw new ArgumentException("Polynomial exponents must be montonically increasing", "exponents");
				}
			}
			return new GenericPolynomialExtensionField(FiniteFields.GF_2, new GF2Polynomial(exponents));
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000E100C File Offset: 0x000DF20C
		public static IFiniteField GetPrimeField(BigInteger characteristic)
		{
			int bitLength = characteristic.BitLength;
			if (characteristic.SignValue <= 0 || bitLength < 2)
			{
				throw new ArgumentException("Must be >= 2", "characteristic");
			}
			if (bitLength < 3)
			{
				int intValue = characteristic.IntValue;
				if (intValue == 2)
				{
					return FiniteFields.GF_2;
				}
				if (intValue == 3)
				{
					return FiniteFields.GF_3;
				}
			}
			return new PrimeField(characteristic);
		}

		// Token: 0x04001947 RID: 6471
		internal static readonly IFiniteField GF_2 = new PrimeField(BigInteger.ValueOf(2L));

		// Token: 0x04001948 RID: 6472
		internal static readonly IFiniteField GF_3 = new PrimeField(BigInteger.ValueOf(3L));
	}
}
