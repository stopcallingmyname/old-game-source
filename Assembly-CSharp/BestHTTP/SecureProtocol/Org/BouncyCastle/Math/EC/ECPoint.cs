using System;
using System.Collections;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000328 RID: 808
	public abstract class ECPoint
	{
		// Token: 0x06001ED0 RID: 7888 RVA: 0x000E3D94 File Offset: 0x000E1F94
		protected static ECFieldElement[] GetInitialZCoords(ECCurve curve)
		{
			int num = (curve == null) ? 0 : curve.CoordinateSystem;
			if (num == 0 || num == 5)
			{
				return ECPoint.EMPTY_ZS;
			}
			ECFieldElement ecfieldElement = curve.FromBigInteger(BigInteger.One);
			switch (num)
			{
			case 1:
			case 2:
			case 6:
				return new ECFieldElement[]
				{
					ecfieldElement
				};
			case 3:
				return new ECFieldElement[]
				{
					ecfieldElement,
					ecfieldElement,
					ecfieldElement
				};
			case 4:
				return new ECFieldElement[]
				{
					ecfieldElement,
					curve.A
				};
			}
			throw new ArgumentException("unknown coordinate system");
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000E3E25 File Offset: 0x000E2025
		protected ECPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : this(curve, x, y, ECPoint.GetInitialZCoords(curve), withCompression)
		{
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000E3E38 File Offset: 0x000E2038
		internal ECPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			this.m_curve = curve;
			this.m_x = x;
			this.m_y = y;
			this.m_zs = zs;
			this.m_withCompression = withCompression;
		}

		// Token: 0x06001ED3 RID: 7891
		protected abstract bool SatisfiesCurveEquation();

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000E3E68 File Offset: 0x000E2068
		protected virtual bool SatisfiesOrder()
		{
			if (BigInteger.One.Equals(this.Curve.Cofactor))
			{
				return true;
			}
			BigInteger order = this.Curve.Order;
			return order == null || ECAlgorithms.ReferenceMultiply(this, order).IsInfinity;
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000E3EAB File Offset: 0x000E20AB
		public ECPoint GetDetachedPoint()
		{
			return this.Normalize().Detach();
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x000E3EB8 File Offset: 0x000E20B8
		public virtual ECCurve Curve
		{
			get
			{
				return this.m_curve;
			}
		}

		// Token: 0x06001ED7 RID: 7895
		protected abstract ECPoint Detach();

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001ED8 RID: 7896 RVA: 0x000E3EC0 File Offset: 0x000E20C0
		protected virtual int CurveCoordinateSystem
		{
			get
			{
				if (this.m_curve != null)
				{
					return this.m_curve.CoordinateSystem;
				}
				return 0;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000E3ED7 File Offset: 0x000E20D7
		public virtual ECFieldElement AffineXCoord
		{
			get
			{
				this.CheckNormalized();
				return this.XCoord;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x000E3EE5 File Offset: 0x000E20E5
		public virtual ECFieldElement AffineYCoord
		{
			get
			{
				this.CheckNormalized();
				return this.YCoord;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001EDB RID: 7899 RVA: 0x000E3EF3 File Offset: 0x000E20F3
		public virtual ECFieldElement XCoord
		{
			get
			{
				return this.m_x;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x000E3EFB File Offset: 0x000E20FB
		public virtual ECFieldElement YCoord
		{
			get
			{
				return this.m_y;
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000E3F03 File Offset: 0x000E2103
		public virtual ECFieldElement GetZCoord(int index)
		{
			if (index >= 0 && index < this.m_zs.Length)
			{
				return this.m_zs[index];
			}
			return null;
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000E3F20 File Offset: 0x000E2120
		public virtual ECFieldElement[] GetZCoords()
		{
			int num = this.m_zs.Length;
			if (num == 0)
			{
				return this.m_zs;
			}
			ECFieldElement[] array = new ECFieldElement[num];
			Array.Copy(this.m_zs, 0, array, 0, num);
			return array;
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000E3EF3 File Offset: 0x000E20F3
		protected internal ECFieldElement RawXCoord
		{
			get
			{
				return this.m_x;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001EE0 RID: 7904 RVA: 0x000E3EFB File Offset: 0x000E20FB
		protected internal ECFieldElement RawYCoord
		{
			get
			{
				return this.m_y;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x000E3F57 File Offset: 0x000E2157
		protected internal ECFieldElement[] RawZCoords
		{
			get
			{
				return this.m_zs;
			}
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000E3F5F File Offset: 0x000E215F
		protected virtual void CheckNormalized()
		{
			if (!this.IsNormalized())
			{
				throw new InvalidOperationException("point not in normal form");
			}
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000E3F74 File Offset: 0x000E2174
		public virtual bool IsNormalized()
		{
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			return curveCoordinateSystem == 0 || curveCoordinateSystem == 5 || this.IsInfinity || this.RawZCoords[0].IsOne;
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000E3FA8 File Offset: 0x000E21A8
		public virtual ECPoint Normalize()
		{
			if (this.IsInfinity)
			{
				return this;
			}
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			if (curveCoordinateSystem == 0 || curveCoordinateSystem == 5)
			{
				return this;
			}
			ECFieldElement ecfieldElement = this.RawZCoords[0];
			if (ecfieldElement.IsOne)
			{
				return this;
			}
			return this.Normalize(ecfieldElement.Invert());
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000E3FF0 File Offset: 0x000E21F0
		internal virtual ECPoint Normalize(ECFieldElement zInv)
		{
			switch (this.CurveCoordinateSystem)
			{
			case 1:
			case 6:
				return this.CreateScaledPoint(zInv, zInv);
			case 2:
			case 3:
			case 4:
			{
				ECFieldElement ecfieldElement = zInv.Square();
				ECFieldElement sy = ecfieldElement.Multiply(zInv);
				return this.CreateScaledPoint(ecfieldElement, sy);
			}
			}
			throw new InvalidOperationException("not a projective coordinate system");
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000E4051 File Offset: 0x000E2251
		protected virtual ECPoint CreateScaledPoint(ECFieldElement sx, ECFieldElement sy)
		{
			return this.Curve.CreateRawPoint(this.RawXCoord.Multiply(sx), this.RawYCoord.Multiply(sy), this.IsCompressed);
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000E407C File Offset: 0x000E227C
		public bool IsInfinity
		{
			get
			{
				return this.m_x == null && this.m_y == null;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000E4091 File Offset: 0x000E2291
		public bool IsCompressed
		{
			get
			{
				return this.m_withCompression;
			}
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000E4099 File Offset: 0x000E2299
		public bool IsValid()
		{
			return this.ImplIsValid(false, true);
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000E40A3 File Offset: 0x000E22A3
		internal bool IsValidPartial()
		{
			return this.ImplIsValid(false, false);
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000E40B0 File Offset: 0x000E22B0
		internal bool ImplIsValid(bool decompressed, bool checkOrder)
		{
			if (this.IsInfinity)
			{
				return true;
			}
			ECPoint.ValidityCallback callback = new ECPoint.ValidityCallback(this, decompressed, checkOrder);
			return !((ValidityPreCompInfo)this.Curve.Precompute(this, ValidityPreCompInfo.PRECOMP_NAME, callback)).HasFailed();
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000E40EF File Offset: 0x000E22EF
		public virtual ECPoint ScaleX(ECFieldElement scale)
		{
			if (!this.IsInfinity)
			{
				return this.Curve.CreateRawPoint(this.RawXCoord.Multiply(scale), this.RawYCoord, this.RawZCoords, this.IsCompressed);
			}
			return this;
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000E4124 File Offset: 0x000E2324
		public virtual ECPoint ScaleY(ECFieldElement scale)
		{
			if (!this.IsInfinity)
			{
				return this.Curve.CreateRawPoint(this.RawXCoord, this.RawYCoord.Multiply(scale), this.RawZCoords, this.IsCompressed);
			}
			return this;
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000E4159 File Offset: 0x000E2359
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ECPoint);
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x000E4168 File Offset: 0x000E2368
		public virtual bool Equals(ECPoint other)
		{
			if (this == other)
			{
				return true;
			}
			if (other == null)
			{
				return false;
			}
			ECCurve curve = this.Curve;
			ECCurve curve2 = other.Curve;
			bool flag = curve == null;
			bool flag2 = curve2 == null;
			bool isInfinity = this.IsInfinity;
			bool isInfinity2 = other.IsInfinity;
			if (isInfinity || isInfinity2)
			{
				return isInfinity && isInfinity2 && (flag || flag2 || curve.Equals(curve2));
			}
			ECPoint ecpoint = this;
			ECPoint ecpoint2 = other;
			if (!flag || !flag2)
			{
				if (flag)
				{
					ecpoint2 = ecpoint2.Normalize();
				}
				else if (flag2)
				{
					ecpoint = ecpoint.Normalize();
				}
				else
				{
					if (!curve.Equals(curve2))
					{
						return false;
					}
					ECPoint[] array = new ECPoint[]
					{
						this,
						curve.ImportPoint(ecpoint2)
					};
					curve.NormalizeAll(array);
					ecpoint = array[0];
					ecpoint2 = array[1];
				}
			}
			return ecpoint.XCoord.Equals(ecpoint2.XCoord) && ecpoint.YCoord.Equals(ecpoint2.YCoord);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000E4250 File Offset: 0x000E2450
		public override int GetHashCode()
		{
			ECCurve curve = this.Curve;
			int num = (curve == null) ? 0 : (~curve.GetHashCode());
			if (!this.IsInfinity)
			{
				ECPoint ecpoint = this.Normalize();
				num ^= ecpoint.XCoord.GetHashCode() * 17;
				num ^= ecpoint.YCoord.GetHashCode() * 257;
			}
			return num;
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000E42A8 File Offset: 0x000E24A8
		public override string ToString()
		{
			if (this.IsInfinity)
			{
				return "INF";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			stringBuilder.Append(this.RawXCoord);
			stringBuilder.Append(',');
			stringBuilder.Append(this.RawYCoord);
			for (int i = 0; i < this.m_zs.Length; i++)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(this.m_zs[i]);
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000E432F File Offset: 0x000E252F
		public virtual byte[] GetEncoded()
		{
			return this.GetEncoded(this.m_withCompression);
		}

		// Token: 0x06001EF3 RID: 7923
		public abstract byte[] GetEncoded(bool compressed);

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001EF4 RID: 7924
		protected internal abstract bool CompressionYTilde { get; }

		// Token: 0x06001EF5 RID: 7925
		public abstract ECPoint Add(ECPoint b);

		// Token: 0x06001EF6 RID: 7926
		public abstract ECPoint Subtract(ECPoint b);

		// Token: 0x06001EF7 RID: 7927
		public abstract ECPoint Negate();

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000E4340 File Offset: 0x000E2540
		public virtual ECPoint TimesPow2(int e)
		{
			if (e < 0)
			{
				throw new ArgumentException("cannot be negative", "e");
			}
			ECPoint ecpoint = this;
			while (--e >= 0)
			{
				ecpoint = ecpoint.Twice();
			}
			return ecpoint;
		}

		// Token: 0x06001EF9 RID: 7929
		public abstract ECPoint Twice();

		// Token: 0x06001EFA RID: 7930
		public abstract ECPoint Multiply(BigInteger b);

		// Token: 0x06001EFB RID: 7931 RVA: 0x000E4376 File Offset: 0x000E2576
		public virtual ECPoint TwicePlus(ECPoint b)
		{
			return this.Twice().Add(b);
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x000E4384 File Offset: 0x000E2584
		public virtual ECPoint ThreeTimes()
		{
			return this.TwicePlus(this);
		}

		// Token: 0x04001972 RID: 6514
		protected static ECFieldElement[] EMPTY_ZS = new ECFieldElement[0];

		// Token: 0x04001973 RID: 6515
		protected internal readonly ECCurve m_curve;

		// Token: 0x04001974 RID: 6516
		protected internal readonly ECFieldElement m_x;

		// Token: 0x04001975 RID: 6517
		protected internal readonly ECFieldElement m_y;

		// Token: 0x04001976 RID: 6518
		protected internal readonly ECFieldElement[] m_zs;

		// Token: 0x04001977 RID: 6519
		protected internal readonly bool m_withCompression;

		// Token: 0x04001978 RID: 6520
		protected internal IDictionary m_preCompTable;

		// Token: 0x02000910 RID: 2320
		private class ValidityCallback : IPreCompCallback
		{
			// Token: 0x06004E52 RID: 20050 RVA: 0x001B1436 File Offset: 0x001AF636
			internal ValidityCallback(ECPoint outer, bool decompressed, bool checkOrder)
			{
				this.m_outer = outer;
				this.m_decompressed = decompressed;
				this.m_checkOrder = checkOrder;
			}

			// Token: 0x06004E53 RID: 20051 RVA: 0x001B1454 File Offset: 0x001AF654
			public PreCompInfo Precompute(PreCompInfo existing)
			{
				ValidityPreCompInfo validityPreCompInfo = existing as ValidityPreCompInfo;
				if (validityPreCompInfo == null)
				{
					validityPreCompInfo = new ValidityPreCompInfo();
				}
				if (validityPreCompInfo.HasFailed())
				{
					return validityPreCompInfo;
				}
				if (!validityPreCompInfo.HasCurveEquationPassed())
				{
					if (!this.m_decompressed && !this.m_outer.SatisfiesCurveEquation())
					{
						validityPreCompInfo.ReportFailed();
						return validityPreCompInfo;
					}
					validityPreCompInfo.ReportCurveEquationPassed();
				}
				if (this.m_checkOrder && !validityPreCompInfo.HasOrderPassed())
				{
					if (!this.m_outer.SatisfiesOrder())
					{
						validityPreCompInfo.ReportFailed();
						return validityPreCompInfo;
					}
					validityPreCompInfo.ReportOrderPassed();
				}
				return validityPreCompInfo;
			}

			// Token: 0x04003566 RID: 13670
			private readonly ECPoint m_outer;

			// Token: 0x04003567 RID: 13671
			private readonly bool m_decompressed;

			// Token: 0x04003568 RID: 13672
			private readonly bool m_checkOrder;
		}
	}
}
