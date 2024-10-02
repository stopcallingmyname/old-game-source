using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004FC RID: 1276
	public class RsaKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x060030A7 RID: 12455 RVA: 0x00127C83 File Offset: 0x00125E83
		private static BigInteger Validate(BigInteger modulus)
		{
			if ((modulus.IntValue & 1) == 0)
			{
				throw new ArgumentException("RSA modulus is even", "modulus");
			}
			if (!modulus.Gcd(RsaKeyParameters.SmallPrimesProduct).Equals(BigInteger.One))
			{
				throw new ArgumentException("RSA modulus has a small prime factor");
			}
			return modulus;
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x00127CC4 File Offset: 0x00125EC4
		public RsaKeyParameters(bool isPrivate, BigInteger modulus, BigInteger exponent) : base(isPrivate)
		{
			if (modulus == null)
			{
				throw new ArgumentNullException("modulus");
			}
			if (exponent == null)
			{
				throw new ArgumentNullException("exponent");
			}
			if (modulus.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA modulus", "modulus");
			}
			if (exponent.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA exponent", "exponent");
			}
			if (!isPrivate && (exponent.IntValue & 1) == 0)
			{
				throw new ArgumentException("RSA publicExponent is even", "exponent");
			}
			this.modulus = RsaKeyParameters.Validate(modulus);
			this.exponent = exponent;
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060030A9 RID: 12457 RVA: 0x00127D56 File Offset: 0x00125F56
		public BigInteger Modulus
		{
			get
			{
				return this.modulus;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060030AA RID: 12458 RVA: 0x00127D5E File Offset: 0x00125F5E
		public BigInteger Exponent
		{
			get
			{
				return this.exponent;
			}
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x00127D68 File Offset: 0x00125F68
		public override bool Equals(object obj)
		{
			RsaKeyParameters rsaKeyParameters = obj as RsaKeyParameters;
			return rsaKeyParameters != null && (rsaKeyParameters.IsPrivate == base.IsPrivate && rsaKeyParameters.Modulus.Equals(this.modulus)) && rsaKeyParameters.Exponent.Equals(this.exponent);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x00127DB8 File Offset: 0x00125FB8
		public override int GetHashCode()
		{
			return this.modulus.GetHashCode() ^ this.exponent.GetHashCode() ^ base.IsPrivate.GetHashCode();
		}

		// Token: 0x04002027 RID: 8231
		private static BigInteger SmallPrimesProduct = new BigInteger("8138E8A0FCF3A4E84A771D40FD305D7F4AA59306D7251DE54D98AF8FE95729A1F73D893FA424CD2EDC8636A6C3285E022B0E3866A565AE8108EED8591CD4FE8D2CE86165A978D719EBF647F362D33FCA29CD179FB42401CBAF3DF0C614056F9C8F3CFD51E474AFB6BC6974F78DB8ABA8E9E517FDED658591AB7502BD41849462F", 16);

		// Token: 0x04002028 RID: 8232
		private readonly BigInteger modulus;

		// Token: 0x04002029 RID: 8233
		private readonly BigInteger exponent;
	}
}
