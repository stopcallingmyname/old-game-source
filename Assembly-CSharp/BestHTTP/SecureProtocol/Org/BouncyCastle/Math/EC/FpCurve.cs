using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x0200031F RID: 799
	public class FpCurve : AbstractFpCurve
	{
		// Token: 0x06001E44 RID: 7748 RVA: 0x000E23FF File Offset: 0x000E05FF
		[Obsolete("Use constructor taking order/cofactor")]
		public FpCurve(BigInteger q, BigInteger a, BigInteger b) : this(q, a, b, null, null)
		{
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000E240C File Offset: 0x000E060C
		public FpCurve(BigInteger q, BigInteger a, BigInteger b, BigInteger order, BigInteger cofactor) : base(q)
		{
			this.m_q = q;
			this.m_r = FpFieldElement.CalculateResidue(q);
			this.m_infinity = new FpPoint(this, null, null, false);
			this.m_a = this.FromBigInteger(a);
			this.m_b = this.FromBigInteger(b);
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_coord = 4;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000E2473 File Offset: 0x000E0673
		[Obsolete("Use constructor taking order/cofactor")]
		protected FpCurve(BigInteger q, BigInteger r, ECFieldElement a, ECFieldElement b) : this(q, r, a, b, null, null)
		{
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000E2484 File Offset: 0x000E0684
		protected FpCurve(BigInteger q, BigInteger r, ECFieldElement a, ECFieldElement b, BigInteger order, BigInteger cofactor) : base(q)
		{
			this.m_q = q;
			this.m_r = r;
			this.m_infinity = new FpPoint(this, null, null, false);
			this.m_a = a;
			this.m_b = b;
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_coord = 4;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000E24DB File Offset: 0x000E06DB
		protected override ECCurve CloneCurve()
		{
			return new FpCurve(this.m_q, this.m_r, this.m_a, this.m_b, this.m_order, this.m_cofactor);
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000E2506 File Offset: 0x000E0706
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord <= 2 || coord == 4;
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x000E2513 File Offset: 0x000E0713
		public virtual BigInteger Q
		{
			get
			{
				return this.m_q;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x000E251B File Offset: 0x000E071B
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x000E2523 File Offset: 0x000E0723
		public override int FieldSize
		{
			get
			{
				return this.m_q.BitLength;
			}
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000E2530 File Offset: 0x000E0730
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new FpFieldElement(this.m_q, this.m_r, x);
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000E2544 File Offset: 0x000E0744
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new FpPoint(this, x, y, withCompression);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000E254F File Offset: 0x000E074F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new FpPoint(this, x, y, zs, withCompression);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000E255C File Offset: 0x000E075C
		public override ECPoint ImportPoint(ECPoint p)
		{
			if (this != p.Curve && this.CoordinateSystem == 2 && !p.IsInfinity)
			{
				int coordinateSystem = p.Curve.CoordinateSystem;
				if (coordinateSystem - 2 <= 2)
				{
					return new FpPoint(this, this.FromBigInteger(p.RawXCoord.ToBigInteger()), this.FromBigInteger(p.RawYCoord.ToBigInteger()), new ECFieldElement[]
					{
						this.FromBigInteger(p.GetZCoord(0).ToBigInteger())
					}, p.IsCompressed);
				}
			}
			return base.ImportPoint(p);
		}

		// Token: 0x0400195D RID: 6493
		private const int FP_DEFAULT_COORDS = 4;

		// Token: 0x0400195E RID: 6494
		protected readonly BigInteger m_q;

		// Token: 0x0400195F RID: 6495
		protected readonly BigInteger m_r;

		// Token: 0x04001960 RID: 6496
		protected readonly FpPoint m_infinity;
	}
}
