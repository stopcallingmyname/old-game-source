using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200034A RID: 842
	public abstract class WNafUtilities
	{
		// Token: 0x0600206F RID: 8303 RVA: 0x000EFBAC File Offset: 0x000EDDAC
		public static int[] GenerateCompactNaf(BigInteger k)
		{
			if (k.BitLength >> 16 != 0)
			{
				throw new ArgumentException("must have bitlength < 2^16", "k");
			}
			if (k.SignValue == 0)
			{
				return Arrays.EmptyInts;
			}
			BigInteger bigInteger = k.ShiftLeft(1).Add(k);
			int bitLength = bigInteger.BitLength;
			int[] array = new int[bitLength >> 1];
			BigInteger bigInteger2 = bigInteger.Xor(k);
			int num = bitLength - 1;
			int num2 = 0;
			int num3 = 0;
			for (int i = 1; i < num; i++)
			{
				if (!bigInteger2.TestBit(i))
				{
					num3++;
				}
				else
				{
					int num4 = k.TestBit(i) ? -1 : 1;
					array[num2++] = (num4 << 16 | num3);
					num3 = 1;
					i++;
				}
			}
			array[num2++] = (65536 | num3);
			if (array.Length > num2)
			{
				array = WNafUtilities.Trim(array, num2);
			}
			return array;
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x000EFC80 File Offset: 0x000EDE80
		public static int[] GenerateCompactWindowNaf(int width, BigInteger k)
		{
			if (width == 2)
			{
				return WNafUtilities.GenerateCompactNaf(k);
			}
			if (width < 2 || width > 16)
			{
				throw new ArgumentException("must be in the range [2, 16]", "width");
			}
			if (k.BitLength >> 16 != 0)
			{
				throw new ArgumentException("must have bitlength < 2^16", "k");
			}
			if (k.SignValue == 0)
			{
				return Arrays.EmptyInts;
			}
			int[] array = new int[k.BitLength / width + 1];
			int num = 1 << width;
			int num2 = num - 1;
			int num3 = num >> 1;
			bool flag = false;
			int num4 = 0;
			int i = 0;
			while (i <= k.BitLength)
			{
				if (k.TestBit(i) == flag)
				{
					i++;
				}
				else
				{
					k = k.ShiftRight(i);
					int num5 = k.IntValue & num2;
					if (flag)
					{
						num5++;
					}
					flag = ((num5 & num3) != 0);
					if (flag)
					{
						num5 -= num;
					}
					int num6 = (num4 > 0) ? (i - 1) : i;
					array[num4++] = (num5 << 16 | num6);
					i = width;
				}
			}
			if (array.Length > num4)
			{
				array = WNafUtilities.Trim(array, num4);
			}
			return array;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x000EFD88 File Offset: 0x000EDF88
		public static byte[] GenerateJsf(BigInteger g, BigInteger h)
		{
			byte[] array = new byte[Math.Max(g.BitLength, h.BitLength) + 1];
			BigInteger bigInteger = g;
			BigInteger bigInteger2 = h;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while ((num2 | num3) != 0 || bigInteger.BitLength > num4 || bigInteger2.BitLength > num4)
			{
				int num5 = (int)(((uint)bigInteger.IntValue >> num4) + (uint)num2 & 7U);
				int num6 = (int)(((uint)bigInteger2.IntValue >> num4) + (uint)num3 & 7U);
				int num7 = num5 & 1;
				if (num7 != 0)
				{
					num7 -= (num5 & 2);
					if (num5 + num7 == 4 && (num6 & 3) == 2)
					{
						num7 = -num7;
					}
				}
				int num8 = num6 & 1;
				if (num8 != 0)
				{
					num8 -= (num6 & 2);
					if (num6 + num8 == 4 && (num5 & 3) == 2)
					{
						num8 = -num8;
					}
				}
				if (num2 << 1 == 1 + num7)
				{
					num2 ^= 1;
				}
				if (num3 << 1 == 1 + num8)
				{
					num3 ^= 1;
				}
				if (++num4 == 30)
				{
					num4 = 0;
					bigInteger = bigInteger.ShiftRight(30);
					bigInteger2 = bigInteger2.ShiftRight(30);
				}
				array[num++] = (byte)(num7 << 4 | (num8 & 15));
			}
			if (array.Length > num)
			{
				array = WNafUtilities.Trim(array, num);
			}
			return array;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000EFEBC File Offset: 0x000EE0BC
		public static byte[] GenerateNaf(BigInteger k)
		{
			if (k.SignValue == 0)
			{
				return Arrays.EmptyBytes;
			}
			BigInteger bigInteger = k.ShiftLeft(1).Add(k);
			int num = bigInteger.BitLength - 1;
			byte[] array = new byte[num];
			BigInteger bigInteger2 = bigInteger.Xor(k);
			for (int i = 1; i < num; i++)
			{
				if (bigInteger2.TestBit(i))
				{
					array[i - 1] = (byte)(k.TestBit(i) ? -1 : 1);
					i++;
				}
			}
			array[num - 1] = 1;
			return array;
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000EFF30 File Offset: 0x000EE130
		public static byte[] GenerateWindowNaf(int width, BigInteger k)
		{
			if (width == 2)
			{
				return WNafUtilities.GenerateNaf(k);
			}
			if (width < 2 || width > 8)
			{
				throw new ArgumentException("must be in the range [2, 8]", "width");
			}
			if (k.SignValue == 0)
			{
				return Arrays.EmptyBytes;
			}
			byte[] array = new byte[k.BitLength + 1];
			int num = 1 << width;
			int num2 = num - 1;
			int num3 = num >> 1;
			bool flag = false;
			int num4 = 0;
			int i = 0;
			while (i <= k.BitLength)
			{
				if (k.TestBit(i) == flag)
				{
					i++;
				}
				else
				{
					k = k.ShiftRight(i);
					int num5 = k.IntValue & num2;
					if (flag)
					{
						num5++;
					}
					flag = ((num5 & num3) != 0);
					if (flag)
					{
						num5 -= num;
					}
					num4 += ((num4 > 0) ? (i - 1) : i);
					array[num4++] = (byte)num5;
					i = width;
				}
			}
			if (array.Length > num4)
			{
				array = WNafUtilities.Trim(array, num4);
			}
			return array;
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000F0017 File Offset: 0x000EE217
		public static int GetNafWeight(BigInteger k)
		{
			if (k.SignValue == 0)
			{
				return 0;
			}
			return k.ShiftLeft(1).Add(k).Xor(k).BitCount;
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000F003B File Offset: 0x000EE23B
		public static WNafPreCompInfo GetWNafPreCompInfo(ECPoint p)
		{
			return WNafUtilities.GetWNafPreCompInfo(p.Curve.GetPreCompInfo(p, WNafUtilities.PRECOMP_NAME));
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000F0053 File Offset: 0x000EE253
		public static WNafPreCompInfo GetWNafPreCompInfo(PreCompInfo preCompInfo)
		{
			return preCompInfo as WNafPreCompInfo;
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000F005B File Offset: 0x000EE25B
		public static int GetWindowSize(int bits)
		{
			return WNafUtilities.GetWindowSize(bits, WNafUtilities.DEFAULT_WINDOW_SIZE_CUTOFFS);
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000F0068 File Offset: 0x000EE268
		public static int GetWindowSize(int bits, int[] windowSizeCutoffs)
		{
			int num = 0;
			while (num < windowSizeCutoffs.Length && bits >= windowSizeCutoffs[num])
			{
				num++;
			}
			return num + 2;
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000F008C File Offset: 0x000EE28C
		public static ECPoint MapPointWithPrecomp(ECPoint p, int width, bool includeNegated, ECPointMap pointMap)
		{
			ECCurve curve = p.Curve;
			WNafPreCompInfo wnafPreCompP = WNafUtilities.Precompute(p, width, includeNegated);
			ECPoint ecpoint = pointMap.Map(p);
			curve.Precompute(ecpoint, WNafUtilities.PRECOMP_NAME, new WNafUtilities.MapPointCallback(wnafPreCompP, includeNegated, pointMap));
			return ecpoint;
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000F00C5 File Offset: 0x000EE2C5
		public static WNafPreCompInfo Precompute(ECPoint p, int width, bool includeNegated)
		{
			return (WNafPreCompInfo)p.Curve.Precompute(p, WNafUtilities.PRECOMP_NAME, new WNafUtilities.WNafCallback(p, width, includeNegated));
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000F00E8 File Offset: 0x000EE2E8
		private static byte[] Trim(byte[] a, int length)
		{
			byte[] array = new byte[length];
			Array.Copy(a, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000F010C File Offset: 0x000EE30C
		private static int[] Trim(int[] a, int length)
		{
			int[] array = new int[length];
			Array.Copy(a, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000F0130 File Offset: 0x000EE330
		private static ECPoint[] ResizeTable(ECPoint[] a, int length)
		{
			ECPoint[] array = new ECPoint[length];
			Array.Copy(a, 0, array, 0, a.Length);
			return array;
		}

		// Token: 0x040019EA RID: 6634
		public static readonly string PRECOMP_NAME = "bc_wnaf";

		// Token: 0x040019EB RID: 6635
		private static readonly int[] DEFAULT_WINDOW_SIZE_CUTOFFS = new int[]
		{
			13,
			41,
			121,
			337,
			897,
			2305
		};

		// Token: 0x040019EC RID: 6636
		private static readonly ECPoint[] EMPTY_POINTS = new ECPoint[0];

		// Token: 0x02000919 RID: 2329
		private class MapPointCallback : IPreCompCallback
		{
			// Token: 0x06004E5D RID: 20061 RVA: 0x001B1725 File Offset: 0x001AF925
			internal MapPointCallback(WNafPreCompInfo wnafPreCompP, bool includeNegated, ECPointMap pointMap)
			{
				this.m_wnafPreCompP = wnafPreCompP;
				this.m_includeNegated = includeNegated;
				this.m_pointMap = pointMap;
			}

			// Token: 0x06004E5E RID: 20062 RVA: 0x001B1744 File Offset: 0x001AF944
			public PreCompInfo Precompute(PreCompInfo existing)
			{
				WNafPreCompInfo wnafPreCompInfo = new WNafPreCompInfo();
				ECPoint twice = this.m_wnafPreCompP.Twice;
				if (twice != null)
				{
					ECPoint twice2 = this.m_pointMap.Map(twice);
					wnafPreCompInfo.Twice = twice2;
				}
				ECPoint[] preComp = this.m_wnafPreCompP.PreComp;
				ECPoint[] array = new ECPoint[preComp.Length];
				for (int i = 0; i < preComp.Length; i++)
				{
					array[i] = this.m_pointMap.Map(preComp[i]);
				}
				wnafPreCompInfo.PreComp = array;
				if (this.m_includeNegated)
				{
					ECPoint[] array2 = new ECPoint[array.Length];
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = array[j].Negate();
					}
					wnafPreCompInfo.PreCompNeg = array2;
				}
				return wnafPreCompInfo;
			}

			// Token: 0x04003582 RID: 13698
			private readonly WNafPreCompInfo m_wnafPreCompP;

			// Token: 0x04003583 RID: 13699
			private readonly bool m_includeNegated;

			// Token: 0x04003584 RID: 13700
			private readonly ECPointMap m_pointMap;
		}

		// Token: 0x0200091A RID: 2330
		private class WNafCallback : IPreCompCallback
		{
			// Token: 0x06004E5F RID: 20063 RVA: 0x001B17F9 File Offset: 0x001AF9F9
			internal WNafCallback(ECPoint p, int width, bool includeNegated)
			{
				this.m_p = p;
				this.m_width = width;
				this.m_includeNegated = includeNegated;
			}

			// Token: 0x06004E60 RID: 20064 RVA: 0x001B1818 File Offset: 0x001AFA18
			public PreCompInfo Precompute(PreCompInfo existing)
			{
				WNafPreCompInfo wnafPreCompInfo = existing as WNafPreCompInfo;
				int num = 1 << Math.Max(0, this.m_width - 2);
				if (this.CheckExisting(wnafPreCompInfo, num, this.m_includeNegated))
				{
					return wnafPreCompInfo;
				}
				ECCurve curve = this.m_p.Curve;
				ECPoint[] array = null;
				ECPoint[] array2 = null;
				ECPoint ecpoint = null;
				if (wnafPreCompInfo != null)
				{
					array = wnafPreCompInfo.PreComp;
					array2 = wnafPreCompInfo.PreCompNeg;
					ecpoint = wnafPreCompInfo.Twice;
				}
				int num2 = 0;
				if (array == null)
				{
					array = WNafUtilities.EMPTY_POINTS;
				}
				else
				{
					num2 = array.Length;
				}
				if (num2 < num)
				{
					array = WNafUtilities.ResizeTable(array, num);
					if (num == 1)
					{
						array[0] = this.m_p.Normalize();
					}
					else
					{
						int i = num2;
						if (i == 0)
						{
							array[0] = this.m_p;
							i = 1;
						}
						ECFieldElement ecfieldElement = null;
						if (num == 2)
						{
							array[1] = this.m_p.ThreeTimes();
						}
						else
						{
							ECPoint ecpoint2 = ecpoint;
							ECPoint ecpoint3 = array[i - 1];
							if (ecpoint2 == null)
							{
								ecpoint2 = array[0].Twice();
								ecpoint = ecpoint2;
								if (!ecpoint.IsInfinity && ECAlgorithms.IsFpCurve(curve) && curve.FieldSize >= 64)
								{
									int coordinateSystem = curve.CoordinateSystem;
									if (coordinateSystem - 2 <= 2)
									{
										ecfieldElement = ecpoint.GetZCoord(0);
										ecpoint2 = curve.CreatePoint(ecpoint.XCoord.ToBigInteger(), ecpoint.YCoord.ToBigInteger());
										ECFieldElement ecfieldElement2 = ecfieldElement.Square();
										ECFieldElement scale = ecfieldElement2.Multiply(ecfieldElement);
										ecpoint3 = ecpoint3.ScaleX(ecfieldElement2).ScaleY(scale);
										if (num2 == 0)
										{
											array[0] = ecpoint3;
										}
									}
								}
							}
							while (i < num)
							{
								ecpoint3 = (array[i++] = ecpoint3.Add(ecpoint2));
							}
						}
						curve.NormalizeAll(array, num2, num - num2, ecfieldElement);
					}
				}
				if (this.m_includeNegated)
				{
					int j;
					if (array2 == null)
					{
						j = 0;
						array2 = new ECPoint[num];
					}
					else
					{
						j = array2.Length;
						if (j < num)
						{
							array2 = WNafUtilities.ResizeTable(array2, num);
						}
					}
					while (j < num)
					{
						array2[j] = array[j].Negate();
						j++;
					}
				}
				return new WNafPreCompInfo
				{
					PreComp = array,
					PreCompNeg = array2,
					Twice = ecpoint
				};
			}

			// Token: 0x06004E61 RID: 20065 RVA: 0x001B1A22 File Offset: 0x001AFC22
			private bool CheckExisting(WNafPreCompInfo existingWNaf, int reqPreCompLen, bool includeNegated)
			{
				return existingWNaf != null && this.CheckTable(existingWNaf.PreComp, reqPreCompLen) && (!includeNegated || this.CheckTable(existingWNaf.PreCompNeg, reqPreCompLen));
			}

			// Token: 0x06004E62 RID: 20066 RVA: 0x001B1A4A File Offset: 0x001AFC4A
			private bool CheckTable(ECPoint[] table, int reqLen)
			{
				return table != null && table.Length >= reqLen;
			}

			// Token: 0x04003585 RID: 13701
			private readonly ECPoint m_p;

			// Token: 0x04003586 RID: 13702
			private readonly int m_width;

			// Token: 0x04003587 RID: 13703
			private readonly bool m_includeNegated;
		}
	}
}
