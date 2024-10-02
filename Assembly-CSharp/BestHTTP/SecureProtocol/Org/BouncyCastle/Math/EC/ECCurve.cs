using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x0200031D RID: 797
	public abstract class ECCurve
	{
		// Token: 0x06001E1B RID: 7707 RVA: 0x000E1C48 File Offset: 0x000DFE48
		public static int[] GetAllCoordinateSystems()
		{
			return new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7
			};
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000E1C5B File Offset: 0x000DFE5B
		protected ECCurve(IFiniteField field)
		{
			this.m_field = field;
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001E1D RID: 7709
		public abstract int FieldSize { get; }

		// Token: 0x06001E1E RID: 7710
		public abstract ECFieldElement FromBigInteger(BigInteger x);

		// Token: 0x06001E1F RID: 7711
		public abstract bool IsValidFieldElement(BigInteger x);

		// Token: 0x06001E20 RID: 7712 RVA: 0x000E1C6A File Offset: 0x000DFE6A
		public virtual ECCurve.Config Configure()
		{
			return new ECCurve.Config(this, this.m_coord, this.m_endomorphism, this.m_multiplier);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000E1C84 File Offset: 0x000DFE84
		public virtual ECPoint ValidatePoint(BigInteger x, BigInteger y)
		{
			ECPoint ecpoint = this.CreatePoint(x, y);
			if (!ecpoint.IsValid())
			{
				throw new ArgumentException("Invalid point coordinates");
			}
			return ecpoint;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000E1CA1 File Offset: 0x000DFEA1
		[Obsolete("Per-point compression property will be removed")]
		public virtual ECPoint ValidatePoint(BigInteger x, BigInteger y, bool withCompression)
		{
			ECPoint ecpoint = this.CreatePoint(x, y, withCompression);
			if (!ecpoint.IsValid())
			{
				throw new ArgumentException("Invalid point coordinates");
			}
			return ecpoint;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000E1CBF File Offset: 0x000DFEBF
		public virtual ECPoint CreatePoint(BigInteger x, BigInteger y)
		{
			return this.CreatePoint(x, y, false);
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000E1CCA File Offset: 0x000DFECA
		[Obsolete("Per-point compression property will be removed")]
		public virtual ECPoint CreatePoint(BigInteger x, BigInteger y, bool withCompression)
		{
			return this.CreateRawPoint(this.FromBigInteger(x), this.FromBigInteger(y), withCompression);
		}

		// Token: 0x06001E25 RID: 7717
		protected abstract ECCurve CloneCurve();

		// Token: 0x06001E26 RID: 7718
		protected internal abstract ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression);

		// Token: 0x06001E27 RID: 7719
		protected internal abstract ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression);

		// Token: 0x06001E28 RID: 7720 RVA: 0x000E1CE4 File Offset: 0x000DFEE4
		protected virtual ECMultiplier CreateDefaultMultiplier()
		{
			GlvEndomorphism glvEndomorphism = this.m_endomorphism as GlvEndomorphism;
			if (glvEndomorphism != null)
			{
				return new GlvMultiplier(this, glvEndomorphism);
			}
			return new WNafL2RMultiplier();
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000E1D0D File Offset: 0x000DFF0D
		public virtual bool SupportsCoordinateSystem(int coord)
		{
			return coord == 0;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000E1D14 File Offset: 0x000DFF14
		public virtual PreCompInfo GetPreCompInfo(ECPoint point, string name)
		{
			this.CheckPoint(point);
			IDictionary preCompTable;
			lock (point)
			{
				preCompTable = point.m_preCompTable;
			}
			if (preCompTable == null)
			{
				return null;
			}
			IDictionary obj = preCompTable;
			PreCompInfo result;
			lock (obj)
			{
				result = (PreCompInfo)preCompTable[name];
			}
			return result;
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000E1D90 File Offset: 0x000DFF90
		public virtual PreCompInfo Precompute(ECPoint point, string name, IPreCompCallback callback)
		{
			this.CheckPoint(point);
			IDictionary dictionary;
			lock (point)
			{
				dictionary = point.m_preCompTable;
				if (dictionary == null)
				{
					dictionary = (point.m_preCompTable = Platform.CreateHashtable(4));
				}
			}
			IDictionary obj = dictionary;
			PreCompInfo result;
			lock (obj)
			{
				PreCompInfo preCompInfo = (PreCompInfo)dictionary[name];
				PreCompInfo preCompInfo2 = callback.Precompute(preCompInfo);
				if (preCompInfo2 != preCompInfo)
				{
					dictionary[name] = preCompInfo2;
				}
				result = preCompInfo2;
			}
			return result;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000E1E38 File Offset: 0x000E0038
		public virtual ECPoint ImportPoint(ECPoint p)
		{
			if (this == p.Curve)
			{
				return p;
			}
			if (p.IsInfinity)
			{
				return this.Infinity;
			}
			p = p.Normalize();
			return this.CreatePoint(p.XCoord.ToBigInteger(), p.YCoord.ToBigInteger(), p.IsCompressed);
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x000E1E89 File Offset: 0x000E0089
		public virtual void NormalizeAll(ECPoint[] points)
		{
			this.NormalizeAll(points, 0, points.Length, null);
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x000E1E98 File Offset: 0x000E0098
		public virtual void NormalizeAll(ECPoint[] points, int off, int len, ECFieldElement iso)
		{
			this.CheckPoints(points, off, len);
			int coordinateSystem = this.CoordinateSystem;
			if (coordinateSystem == 0 || coordinateSystem == 5)
			{
				if (iso != null)
				{
					throw new ArgumentException("not valid for affine coordinates", "iso");
				}
				return;
			}
			else
			{
				ECFieldElement[] array = new ECFieldElement[len];
				int[] array2 = new int[len];
				int num = 0;
				for (int i = 0; i < len; i++)
				{
					ECPoint ecpoint = points[off + i];
					if (ecpoint != null && (iso != null || !ecpoint.IsNormalized()))
					{
						array[num] = ecpoint.GetZCoord(0);
						array2[num++] = off + i;
					}
				}
				if (num == 0)
				{
					return;
				}
				ECAlgorithms.MontgomeryTrick(array, 0, num, iso);
				for (int j = 0; j < num; j++)
				{
					int num2 = array2[j];
					points[num2] = points[num2].Normalize(array[j]);
				}
				return;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001E2F RID: 7727
		public abstract ECPoint Infinity { get; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x000E1F55 File Offset: 0x000E0155
		public virtual IFiniteField Field
		{
			get
			{
				return this.m_field;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x000E1F5D File Offset: 0x000E015D
		public virtual ECFieldElement A
		{
			get
			{
				return this.m_a;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x000E1F65 File Offset: 0x000E0165
		public virtual ECFieldElement B
		{
			get
			{
				return this.m_b;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x000E1F6D File Offset: 0x000E016D
		public virtual BigInteger Order
		{
			get
			{
				return this.m_order;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x000E1F75 File Offset: 0x000E0175
		public virtual BigInteger Cofactor
		{
			get
			{
				return this.m_cofactor;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x000E1F7D File Offset: 0x000E017D
		public virtual int CoordinateSystem
		{
			get
			{
				return this.m_coord;
			}
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000E1F88 File Offset: 0x000E0188
		public virtual ECLookupTable CreateCacheSafeLookupTable(ECPoint[] points, int off, int len)
		{
			int num = (this.FieldSize + 7) / 8;
			byte[] array = new byte[len * num * 2];
			int num2 = 0;
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				byte[] array2 = ecpoint.RawXCoord.ToBigInteger().ToByteArray();
				byte[] array3 = ecpoint.RawYCoord.ToBigInteger().ToByteArray();
				int num3 = (array2.Length > num) ? 1 : 0;
				int num4 = array2.Length - num3;
				int num5 = (array3.Length > num) ? 1 : 0;
				int num6 = array3.Length - num5;
				Array.Copy(array2, num3, array, num2 + num - num4, num4);
				num2 += num;
				Array.Copy(array3, num5, array, num2 + num - num6, num6);
				num2 += num;
			}
			return new ECCurve.DefaultLookupTable(this, array, len);
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000E203E File Offset: 0x000E023E
		protected virtual void CheckPoint(ECPoint point)
		{
			if (point == null || this != point.Curve)
			{
				throw new ArgumentException("must be non-null and on this curve", "point");
			}
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x000E205C File Offset: 0x000E025C
		protected virtual void CheckPoints(ECPoint[] points)
		{
			this.CheckPoints(points, 0, points.Length);
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x000E206C File Offset: 0x000E026C
		protected virtual void CheckPoints(ECPoint[] points, int off, int len)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (off < 0 || len < 0 || off > points.Length - len)
			{
				throw new ArgumentException("invalid range specified", "points");
			}
			for (int i = 0; i < len; i++)
			{
				ECPoint ecpoint = points[off + i];
				if (ecpoint != null && this != ecpoint.Curve)
				{
					throw new ArgumentException("entries must be null or on this curve", "points");
				}
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000E20D8 File Offset: 0x000E02D8
		public virtual bool Equals(ECCurve other)
		{
			return this == other || (other != null && (this.Field.Equals(other.Field) && this.A.ToBigInteger().Equals(other.A.ToBigInteger())) && this.B.ToBigInteger().Equals(other.B.ToBigInteger()));
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000E213D File Offset: 0x000E033D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ECCurve);
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000E214B File Offset: 0x000E034B
		public override int GetHashCode()
		{
			return this.Field.GetHashCode() ^ Integers.RotateLeft(this.A.ToBigInteger().GetHashCode(), 8) ^ Integers.RotateLeft(this.B.ToBigInteger().GetHashCode(), 16);
		}

		// Token: 0x06001E3D RID: 7741
		protected abstract ECPoint DecompressPoint(int yTilde, BigInteger X1);

		// Token: 0x06001E3E RID: 7742 RVA: 0x000E2187 File Offset: 0x000E0387
		public virtual ECEndomorphism GetEndomorphism()
		{
			return this.m_endomorphism;
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000E2190 File Offset: 0x000E0390
		public virtual ECMultiplier GetMultiplier()
		{
			ECMultiplier multiplier;
			lock (this)
			{
				if (this.m_multiplier == null)
				{
					this.m_multiplier = this.CreateDefaultMultiplier();
				}
				multiplier = this.m_multiplier;
			}
			return multiplier;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000E21E4 File Offset: 0x000E03E4
		public virtual ECPoint DecodePoint(byte[] encoded)
		{
			int num = (this.FieldSize + 7) / 8;
			byte b = encoded[0];
			ECPoint ecpoint;
			switch (b)
			{
			case 0:
				if (encoded.Length != 1)
				{
					throw new ArgumentException("Incorrect length for infinity encoding", "encoded");
				}
				ecpoint = this.Infinity;
				goto IL_159;
			case 2:
			case 3:
			{
				if (encoded.Length != num + 1)
				{
					throw new ArgumentException("Incorrect length for compressed encoding", "encoded");
				}
				int yTilde = (int)(b & 1);
				BigInteger x = new BigInteger(1, encoded, 1, num);
				ecpoint = this.DecompressPoint(yTilde, x);
				if (!ecpoint.ImplIsValid(true, true))
				{
					throw new ArgumentException("Invalid point");
				}
				goto IL_159;
			}
			case 4:
			{
				if (encoded.Length != 2 * num + 1)
				{
					throw new ArgumentException("Incorrect length for uncompressed encoding", "encoded");
				}
				BigInteger x2 = new BigInteger(1, encoded, 1, num);
				BigInteger y = new BigInteger(1, encoded, 1 + num, num);
				ecpoint = this.ValidatePoint(x2, y);
				goto IL_159;
			}
			case 6:
			case 7:
			{
				if (encoded.Length != 2 * num + 1)
				{
					throw new ArgumentException("Incorrect length for hybrid encoding", "encoded");
				}
				BigInteger x3 = new BigInteger(1, encoded, 1, num);
				BigInteger bigInteger = new BigInteger(1, encoded, 1 + num, num);
				if (bigInteger.TestBit(0) != (b == 7))
				{
					throw new ArgumentException("Inconsistent Y coordinate in hybrid encoding", "encoded");
				}
				ecpoint = this.ValidatePoint(x3, bigInteger);
				goto IL_159;
			}
			}
			throw new FormatException("Invalid point encoding " + b);
			IL_159:
			if (b != 0 && ecpoint.IsInfinity)
			{
				throw new ArgumentException("Invalid infinity encoding", "encoded");
			}
			return ecpoint;
		}

		// Token: 0x0400194D RID: 6477
		public const int COORD_AFFINE = 0;

		// Token: 0x0400194E RID: 6478
		public const int COORD_HOMOGENEOUS = 1;

		// Token: 0x0400194F RID: 6479
		public const int COORD_JACOBIAN = 2;

		// Token: 0x04001950 RID: 6480
		public const int COORD_JACOBIAN_CHUDNOVSKY = 3;

		// Token: 0x04001951 RID: 6481
		public const int COORD_JACOBIAN_MODIFIED = 4;

		// Token: 0x04001952 RID: 6482
		public const int COORD_LAMBDA_AFFINE = 5;

		// Token: 0x04001953 RID: 6483
		public const int COORD_LAMBDA_PROJECTIVE = 6;

		// Token: 0x04001954 RID: 6484
		public const int COORD_SKEWED = 7;

		// Token: 0x04001955 RID: 6485
		protected readonly IFiniteField m_field;

		// Token: 0x04001956 RID: 6486
		protected ECFieldElement m_a;

		// Token: 0x04001957 RID: 6487
		protected ECFieldElement m_b;

		// Token: 0x04001958 RID: 6488
		protected BigInteger m_order;

		// Token: 0x04001959 RID: 6489
		protected BigInteger m_cofactor;

		// Token: 0x0400195A RID: 6490
		protected int m_coord;

		// Token: 0x0400195B RID: 6491
		protected ECEndomorphism m_endomorphism;

		// Token: 0x0400195C RID: 6492
		protected ECMultiplier m_multiplier;

		// Token: 0x0200090D RID: 2317
		public class Config
		{
			// Token: 0x06004E47 RID: 20039 RVA: 0x001B1131 File Offset: 0x001AF331
			internal Config(ECCurve outer, int coord, ECEndomorphism endomorphism, ECMultiplier multiplier)
			{
				this.outer = outer;
				this.coord = coord;
				this.endomorphism = endomorphism;
				this.multiplier = multiplier;
			}

			// Token: 0x06004E48 RID: 20040 RVA: 0x001B1156 File Offset: 0x001AF356
			public ECCurve.Config SetCoordinateSystem(int coord)
			{
				this.coord = coord;
				return this;
			}

			// Token: 0x06004E49 RID: 20041 RVA: 0x001B1160 File Offset: 0x001AF360
			public ECCurve.Config SetEndomorphism(ECEndomorphism endomorphism)
			{
				this.endomorphism = endomorphism;
				return this;
			}

			// Token: 0x06004E4A RID: 20042 RVA: 0x001B116A File Offset: 0x001AF36A
			public ECCurve.Config SetMultiplier(ECMultiplier multiplier)
			{
				this.multiplier = multiplier;
				return this;
			}

			// Token: 0x06004E4B RID: 20043 RVA: 0x001B1174 File Offset: 0x001AF374
			public ECCurve Create()
			{
				if (!this.outer.SupportsCoordinateSystem(this.coord))
				{
					throw new InvalidOperationException("unsupported coordinate system");
				}
				ECCurve eccurve = this.outer.CloneCurve();
				if (eccurve == this.outer)
				{
					throw new InvalidOperationException("implementation returned current curve");
				}
				eccurve.m_coord = this.coord;
				eccurve.m_endomorphism = this.endomorphism;
				eccurve.m_multiplier = this.multiplier;
				return eccurve;
			}

			// Token: 0x0400355C RID: 13660
			protected ECCurve outer;

			// Token: 0x0400355D RID: 13661
			protected int coord;

			// Token: 0x0400355E RID: 13662
			protected ECEndomorphism endomorphism;

			// Token: 0x0400355F RID: 13663
			protected ECMultiplier multiplier;
		}

		// Token: 0x0200090E RID: 2318
		private class DefaultLookupTable : ECLookupTable
		{
			// Token: 0x06004E4C RID: 20044 RVA: 0x001B11E2 File Offset: 0x001AF3E2
			internal DefaultLookupTable(ECCurve outer, byte[] table, int size)
			{
				this.m_outer = outer;
				this.m_table = table;
				this.m_size = size;
			}

			// Token: 0x17000C31 RID: 3121
			// (get) Token: 0x06004E4D RID: 20045 RVA: 0x001B11FF File Offset: 0x001AF3FF
			public virtual int Size
			{
				get
				{
					return this.m_size;
				}
			}

			// Token: 0x06004E4E RID: 20046 RVA: 0x001B1208 File Offset: 0x001AF408
			public virtual ECPoint Lookup(int index)
			{
				int num = (this.m_outer.FieldSize + 7) / 8;
				byte[] array = new byte[num];
				byte[] array2 = new byte[num];
				int num2 = 0;
				for (int i = 0; i < this.m_size; i++)
				{
					byte b = (byte)((i ^ index) - 1 >> 31);
					for (int j = 0; j < num; j++)
					{
						byte[] array3 = array;
						int num3 = j;
						array3[num3] ^= (this.m_table[num2 + j] & b);
						byte[] array4 = array2;
						int num4 = j;
						array4[num4] ^= (this.m_table[num2 + num + j] & b);
					}
					num2 += num * 2;
				}
				ECFieldElement x = this.m_outer.FromBigInteger(new BigInteger(1, array));
				ECFieldElement y = this.m_outer.FromBigInteger(new BigInteger(1, array2));
				return this.m_outer.CreateRawPoint(x, y, false);
			}

			// Token: 0x04003560 RID: 13664
			private readonly ECCurve m_outer;

			// Token: 0x04003561 RID: 13665
			private readonly byte[] m_table;

			// Token: 0x04003562 RID: 13666
			private readonly int m_size;
		}
	}
}
