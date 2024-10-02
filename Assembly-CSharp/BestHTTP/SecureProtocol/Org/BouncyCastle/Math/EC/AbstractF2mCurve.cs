using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Abc;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000320 RID: 800
	public abstract class AbstractF2mCurve : ECCurve
	{
		// Token: 0x06001E51 RID: 7761 RVA: 0x000E25E6 File Offset: 0x000E07E6
		public static BigInteger Inverse(int m, int[] ks, BigInteger x)
		{
			return new LongArray(x).ModInverse(m, ks).ToBigInteger();
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000E25FC File Offset: 0x000E07FC
		private static IFiniteField BuildField(int m, int k1, int k2, int k3)
		{
			if (k1 == 0)
			{
				throw new ArgumentException("k1 must be > 0");
			}
			if (k2 == 0)
			{
				if (k3 != 0)
				{
					throw new ArgumentException("k3 must be 0 if k2 == 0");
				}
				return FiniteFields.GetBinaryExtensionField(new int[]
				{
					0,
					k1,
					m
				});
			}
			else
			{
				if (k2 <= k1)
				{
					throw new ArgumentException("k2 must be > k1");
				}
				if (k3 <= k2)
				{
					throw new ArgumentException("k3 must be > k2");
				}
				return FiniteFields.GetBinaryExtensionField(new int[]
				{
					0,
					k1,
					k2,
					k3,
					m
				});
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000E2675 File Offset: 0x000E0875
		protected AbstractF2mCurve(int m, int k1, int k2, int k3) : base(AbstractF2mCurve.BuildField(m, k1, k2, k3))
		{
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000E2687 File Offset: 0x000E0887
		public override bool IsValidFieldElement(BigInteger x)
		{
			return x != null && x.SignValue >= 0 && x.BitLength <= this.FieldSize;
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000E26A8 File Offset: 0x000E08A8
		[Obsolete("Per-point compression property will be removed")]
		public override ECPoint CreatePoint(BigInteger x, BigInteger y, bool withCompression)
		{
			ECFieldElement ecfieldElement = this.FromBigInteger(x);
			ECFieldElement ecfieldElement2 = this.FromBigInteger(y);
			int coordinateSystem = this.CoordinateSystem;
			if (coordinateSystem - 5 <= 1)
			{
				if (ecfieldElement.IsZero)
				{
					if (!ecfieldElement2.Square().Equals(this.B))
					{
						throw new ArgumentException();
					}
				}
				else
				{
					ecfieldElement2 = ecfieldElement2.Divide(ecfieldElement).Add(ecfieldElement);
				}
			}
			return this.CreateRawPoint(ecfieldElement, ecfieldElement2, withCompression);
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000E270C File Offset: 0x000E090C
		protected override ECPoint DecompressPoint(int yTilde, BigInteger X1)
		{
			ECFieldElement ecfieldElement = this.FromBigInteger(X1);
			ECFieldElement ecfieldElement2 = null;
			if (ecfieldElement.IsZero)
			{
				ecfieldElement2 = this.B.Sqrt();
			}
			else
			{
				ECFieldElement beta = ecfieldElement.Square().Invert().Multiply(this.B).Add(this.A).Add(ecfieldElement);
				ECFieldElement ecfieldElement3 = this.SolveQuadraticEquation(beta);
				if (ecfieldElement3 != null)
				{
					if (ecfieldElement3.TestBitZero() != (yTilde == 1))
					{
						ecfieldElement3 = ecfieldElement3.AddOne();
					}
					int coordinateSystem = this.CoordinateSystem;
					if (coordinateSystem - 5 <= 1)
					{
						ecfieldElement2 = ecfieldElement3.Add(ecfieldElement);
					}
					else
					{
						ecfieldElement2 = ecfieldElement3.Multiply(ecfieldElement);
					}
				}
			}
			if (ecfieldElement2 == null)
			{
				throw new ArgumentException("Invalid point compression");
			}
			return this.CreateRawPoint(ecfieldElement, ecfieldElement2, true);
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000E27B8 File Offset: 0x000E09B8
		internal ECFieldElement SolveQuadraticEquation(ECFieldElement beta)
		{
			if (beta.IsZero)
			{
				return beta;
			}
			ECFieldElement ecfieldElement = this.FromBigInteger(BigInteger.Zero);
			int fieldSize = this.FieldSize;
			for (;;)
			{
				ECFieldElement b = this.FromBigInteger(BigInteger.Arbitrary(fieldSize));
				ECFieldElement ecfieldElement2 = ecfieldElement;
				ECFieldElement ecfieldElement3 = beta;
				for (int i = 1; i < fieldSize; i++)
				{
					ECFieldElement ecfieldElement4 = ecfieldElement3.Square();
					ecfieldElement2 = ecfieldElement2.Square().Add(ecfieldElement4.Multiply(b));
					ecfieldElement3 = ecfieldElement4.Add(beta);
				}
				if (!ecfieldElement3.IsZero)
				{
					break;
				}
				ECFieldElement ecfieldElement5 = ecfieldElement2.Square().Add(ecfieldElement2);
				if (!ecfieldElement5.IsZero)
				{
					return ecfieldElement2;
				}
			}
			return null;
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000E2850 File Offset: 0x000E0A50
		internal virtual BigInteger[] GetSi()
		{
			if (this.si == null)
			{
				lock (this)
				{
					if (this.si == null)
					{
						this.si = Tnaf.GetSi(this);
					}
				}
			}
			return this.si;
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x000E28A8 File Offset: 0x000E0AA8
		public virtual bool IsKoblitz
		{
			get
			{
				return this.m_order != null && this.m_cofactor != null && this.m_b.IsOne && (this.m_a.IsZero || this.m_a.IsOne);
			}
		}

		// Token: 0x04001961 RID: 6497
		private BigInteger[] si;
	}
}
