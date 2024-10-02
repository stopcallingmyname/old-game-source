using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004FD RID: 1277
	public class RsaPrivateCrtKeyParameters : RsaKeyParameters
	{
		// Token: 0x060030AE RID: 12462 RVA: 0x00127E00 File Offset: 0x00126000
		public RsaPrivateCrtKeyParameters(BigInteger modulus, BigInteger publicExponent, BigInteger privateExponent, BigInteger p, BigInteger q, BigInteger dP, BigInteger dQ, BigInteger qInv) : base(true, modulus, privateExponent)
		{
			RsaPrivateCrtKeyParameters.ValidateValue(publicExponent, "publicExponent", "exponent");
			RsaPrivateCrtKeyParameters.ValidateValue(p, "p", "P value");
			RsaPrivateCrtKeyParameters.ValidateValue(q, "q", "Q value");
			RsaPrivateCrtKeyParameters.ValidateValue(dP, "dP", "DP value");
			RsaPrivateCrtKeyParameters.ValidateValue(dQ, "dQ", "DQ value");
			RsaPrivateCrtKeyParameters.ValidateValue(qInv, "qInv", "InverseQ value");
			this.e = publicExponent;
			this.p = p;
			this.q = q;
			this.dP = dP;
			this.dQ = dQ;
			this.qInv = qInv;
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x00127EAC File Offset: 0x001260AC
		public RsaPrivateCrtKeyParameters(RsaPrivateKeyStructure rsaPrivateKey) : this(rsaPrivateKey.Modulus, rsaPrivateKey.PublicExponent, rsaPrivateKey.PrivateExponent, rsaPrivateKey.Prime1, rsaPrivateKey.Prime2, rsaPrivateKey.Exponent1, rsaPrivateKey.Exponent2, rsaPrivateKey.Coefficient)
		{
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060030B0 RID: 12464 RVA: 0x00127EEF File Offset: 0x001260EF
		public BigInteger PublicExponent
		{
			get
			{
				return this.e;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x00127EF7 File Offset: 0x001260F7
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x00127EFF File Offset: 0x001260FF
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x00127F07 File Offset: 0x00126107
		public BigInteger DP
		{
			get
			{
				return this.dP;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x00127F0F File Offset: 0x0012610F
		public BigInteger DQ
		{
			get
			{
				return this.dQ;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x00127F17 File Offset: 0x00126117
		public BigInteger QInv
		{
			get
			{
				return this.qInv;
			}
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x00127F20 File Offset: 0x00126120
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = obj as RsaPrivateCrtKeyParameters;
			return rsaPrivateCrtKeyParameters != null && (rsaPrivateCrtKeyParameters.DP.Equals(this.dP) && rsaPrivateCrtKeyParameters.DQ.Equals(this.dQ) && rsaPrivateCrtKeyParameters.Exponent.Equals(base.Exponent) && rsaPrivateCrtKeyParameters.Modulus.Equals(base.Modulus) && rsaPrivateCrtKeyParameters.P.Equals(this.p) && rsaPrivateCrtKeyParameters.Q.Equals(this.q) && rsaPrivateCrtKeyParameters.PublicExponent.Equals(this.e)) && rsaPrivateCrtKeyParameters.QInv.Equals(this.qInv);
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x00127FDC File Offset: 0x001261DC
		public override int GetHashCode()
		{
			return this.DP.GetHashCode() ^ this.DQ.GetHashCode() ^ base.Exponent.GetHashCode() ^ base.Modulus.GetHashCode() ^ this.P.GetHashCode() ^ this.Q.GetHashCode() ^ this.PublicExponent.GetHashCode() ^ this.QInv.GetHashCode();
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x00128048 File Offset: 0x00126248
		private static void ValidateValue(BigInteger x, string name, string desc)
		{
			if (x == null)
			{
				throw new ArgumentNullException(name);
			}
			if (x.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA " + desc, name);
			}
		}

		// Token: 0x0400202A RID: 8234
		private readonly BigInteger e;

		// Token: 0x0400202B RID: 8235
		private readonly BigInteger p;

		// Token: 0x0400202C RID: 8236
		private readonly BigInteger q;

		// Token: 0x0400202D RID: 8237
		private readonly BigInteger dP;

		// Token: 0x0400202E RID: 8238
		private readonly BigInteger dQ;

		// Token: 0x0400202F RID: 8239
		private readonly BigInteger qInv;
	}
}
