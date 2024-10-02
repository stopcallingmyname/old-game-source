using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x02000351 RID: 849
	public class GlvTypeBEndomorphism : GlvEndomorphism, ECEndomorphism
	{
		// Token: 0x0600208F RID: 8335 RVA: 0x000F040E File Offset: 0x000EE60E
		public GlvTypeBEndomorphism(ECCurve curve, GlvTypeBParameters parameters)
		{
			this.m_curve = curve;
			this.m_parameters = parameters;
			this.m_pointMap = new ScaleXPointMap(curve.FromBigInteger(parameters.Beta));
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000F043C File Offset: 0x000EE63C
		public virtual BigInteger[] DecomposeScalar(BigInteger k)
		{
			int bits = this.m_parameters.Bits;
			BigInteger bigInteger = this.CalculateB(k, this.m_parameters.G1, bits);
			BigInteger bigInteger2 = this.CalculateB(k, this.m_parameters.G2, bits);
			BigInteger[] v = this.m_parameters.V1;
			BigInteger[] v2 = this.m_parameters.V2;
			BigInteger bigInteger3 = k.Subtract(bigInteger.Multiply(v[0]).Add(bigInteger2.Multiply(v2[0])));
			BigInteger bigInteger4 = bigInteger.Multiply(v[1]).Add(bigInteger2.Multiply(v2[1])).Negate();
			return new BigInteger[]
			{
				bigInteger3,
				bigInteger4
			};
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x000F04E5 File Offset: 0x000EE6E5
		public virtual ECPointMap PointMap
		{
			get
			{
				return this.m_pointMap;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool HasEfficientPointMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000F04F0 File Offset: 0x000EE6F0
		protected virtual BigInteger CalculateB(BigInteger k, BigInteger g, int t)
		{
			bool flag = g.SignValue < 0;
			BigInteger bigInteger = k.Multiply(g.Abs());
			bool flag2 = bigInteger.TestBit(t - 1);
			bigInteger = bigInteger.ShiftRight(t);
			if (flag2)
			{
				bigInteger = bigInteger.Add(BigInteger.One);
			}
			if (!flag)
			{
				return bigInteger;
			}
			return bigInteger.Negate();
		}

		// Token: 0x040019EF RID: 6639
		protected readonly ECCurve m_curve;

		// Token: 0x040019F0 RID: 6640
		protected readonly GlvTypeBParameters m_parameters;

		// Token: 0x040019F1 RID: 6641
		protected readonly ECPointMap m_pointMap;
	}
}
