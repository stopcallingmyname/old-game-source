﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x0200031C RID: 796
	public class ECAlgorithms
	{
		// Token: 0x06001E04 RID: 7684 RVA: 0x000E1211 File Offset: 0x000DF411
		public static bool IsF2mCurve(ECCurve c)
		{
			return ECAlgorithms.IsF2mField(c.Field);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000E121E File Offset: 0x000DF41E
		public static bool IsF2mField(IFiniteField field)
		{
			return field.Dimension > 1 && field.Characteristic.Equals(BigInteger.Two) && field is IPolynomialExtensionField;
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x000E1246 File Offset: 0x000DF446
		public static bool IsFpCurve(ECCurve c)
		{
			return ECAlgorithms.IsFpField(c.Field);
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x000E1253 File Offset: 0x000DF453
		public static bool IsFpField(IFiniteField field)
		{
			return field.Dimension == 1;
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000E1260 File Offset: 0x000DF460
		public static ECPoint SumOfMultiplies(ECPoint[] ps, BigInteger[] ks)
		{
			if (ps == null || ks == null || ps.Length != ks.Length || ps.Length < 1)
			{
				throw new ArgumentException("point and scalar arrays should be non-null, and of equal, non-zero, length");
			}
			int num = ps.Length;
			if (num == 1)
			{
				return ps[0].Multiply(ks[0]);
			}
			if (num == 2)
			{
				return ECAlgorithms.SumOfTwoMultiplies(ps[0], ks[0], ps[1], ks[1]);
			}
			ECPoint ecpoint = ps[0];
			ECCurve curve = ecpoint.Curve;
			ECPoint[] array = new ECPoint[num];
			array[0] = ecpoint;
			for (int i = 1; i < num; i++)
			{
				array[i] = ECAlgorithms.ImportPoint(curve, ps[i]);
			}
			GlvEndomorphism glvEndomorphism = curve.GetEndomorphism() as GlvEndomorphism;
			if (glvEndomorphism != null)
			{
				return ECAlgorithms.ImplCheckResult(ECAlgorithms.ImplSumOfMultipliesGlv(array, ks, glvEndomorphism));
			}
			return ECAlgorithms.ImplCheckResult(ECAlgorithms.ImplSumOfMultiplies(array, ks));
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x000E1318 File Offset: 0x000DF518
		public static ECPoint SumOfTwoMultiplies(ECPoint P, BigInteger a, ECPoint Q, BigInteger b)
		{
			ECCurve curve = P.Curve;
			Q = ECAlgorithms.ImportPoint(curve, Q);
			AbstractF2mCurve abstractF2mCurve = curve as AbstractF2mCurve;
			if (abstractF2mCurve != null && abstractF2mCurve.IsKoblitz)
			{
				return ECAlgorithms.ImplCheckResult(P.Multiply(a).Add(Q.Multiply(b)));
			}
			GlvEndomorphism glvEndomorphism = curve.GetEndomorphism() as GlvEndomorphism;
			if (glvEndomorphism != null)
			{
				return ECAlgorithms.ImplCheckResult(ECAlgorithms.ImplSumOfMultipliesGlv(new ECPoint[]
				{
					P,
					Q
				}, new BigInteger[]
				{
					a,
					b
				}, glvEndomorphism));
			}
			return ECAlgorithms.ImplCheckResult(ECAlgorithms.ImplShamirsTrickWNaf(P, a, Q, b));
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x000E13A5 File Offset: 0x000DF5A5
		public static ECPoint ShamirsTrick(ECPoint P, BigInteger k, ECPoint Q, BigInteger l)
		{
			Q = ECAlgorithms.ImportPoint(P.Curve, Q);
			return ECAlgorithms.ImplCheckResult(ECAlgorithms.ImplShamirsTrickJsf(P, k, Q, l));
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000E13C4 File Offset: 0x000DF5C4
		public static ECPoint ImportPoint(ECCurve c, ECPoint p)
		{
			ECCurve curve = p.Curve;
			if (!c.Equals(curve))
			{
				throw new ArgumentException("Point must be on the same curve");
			}
			return c.ImportPoint(p);
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000E13F3 File Offset: 0x000DF5F3
		public static void MontgomeryTrick(ECFieldElement[] zs, int off, int len)
		{
			ECAlgorithms.MontgomeryTrick(zs, off, len, null);
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000E1400 File Offset: 0x000DF600
		public static void MontgomeryTrick(ECFieldElement[] zs, int off, int len, ECFieldElement scale)
		{
			ECFieldElement[] array = new ECFieldElement[len];
			array[0] = zs[off];
			int i = 0;
			while (++i < len)
			{
				array[i] = array[i - 1].Multiply(zs[off + i]);
			}
			i--;
			if (scale != null)
			{
				array[i] = array[i].Multiply(scale);
			}
			ECFieldElement ecfieldElement = array[i].Invert();
			while (i > 0)
			{
				int num = off + i--;
				ECFieldElement b = zs[num];
				zs[num] = array[i].Multiply(ecfieldElement);
				ecfieldElement = ecfieldElement.Multiply(b);
			}
			zs[off] = ecfieldElement;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000E1480 File Offset: 0x000DF680
		public static ECPoint ReferenceMultiply(ECPoint p, BigInteger k)
		{
			BigInteger bigInteger = k.Abs();
			ECPoint ecpoint = p.Curve.Infinity;
			int bitLength = bigInteger.BitLength;
			if (bitLength > 0)
			{
				if (bigInteger.TestBit(0))
				{
					ecpoint = p;
				}
				for (int i = 1; i < bitLength; i++)
				{
					p = p.Twice();
					if (bigInteger.TestBit(i))
					{
						ecpoint = ecpoint.Add(p);
					}
				}
			}
			if (k.SignValue >= 0)
			{
				return ecpoint;
			}
			return ecpoint.Negate();
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000E14EC File Offset: 0x000DF6EC
		public static ECPoint ValidatePoint(ECPoint p)
		{
			if (!p.IsValid())
			{
				throw new InvalidOperationException("Invalid point");
			}
			return p;
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000E1504 File Offset: 0x000DF704
		public static ECPoint CleanPoint(ECCurve c, ECPoint p)
		{
			ECCurve curve = p.Curve;
			if (!c.Equals(curve))
			{
				throw new ArgumentException("Point must be on the same curve", "p");
			}
			return c.DecodePoint(p.GetEncoded(false));
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000E153E File Offset: 0x000DF73E
		internal static ECPoint ImplCheckResult(ECPoint p)
		{
			if (!p.IsValidPartial())
			{
				throw new InvalidOperationException("Invalid result");
			}
			return p;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000E1554 File Offset: 0x000DF754
		internal static ECPoint ImplShamirsTrickJsf(ECPoint P, BigInteger k, ECPoint Q, BigInteger l)
		{
			ECCurve curve = P.Curve;
			ECPoint infinity = curve.Infinity;
			ECPoint ecpoint = P.Add(Q);
			ECPoint ecpoint2 = P.Subtract(Q);
			ECPoint[] array = new ECPoint[]
			{
				Q,
				ecpoint2,
				P,
				ecpoint
			};
			curve.NormalizeAll(array);
			ECPoint[] array2 = new ECPoint[]
			{
				array[3].Negate(),
				array[2].Negate(),
				array[1].Negate(),
				array[0].Negate(),
				infinity,
				array[0],
				array[1],
				array[2],
				array[3]
			};
			byte[] array3 = WNafUtilities.GenerateJsf(k, l);
			ECPoint ecpoint3 = infinity;
			int num = array3.Length;
			while (--num >= 0)
			{
				byte b = array3[num];
				int num2 = b << 24 >> 28;
				int num3 = b << 28 >> 28;
				int num4 = 4 + num2 * 3 + num3;
				ecpoint3 = ecpoint3.TwicePlus(array2[num4]);
			}
			return ecpoint3;
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000E1648 File Offset: 0x000DF848
		internal static ECPoint ImplShamirsTrickWNaf(ECPoint P, BigInteger k, ECPoint Q, BigInteger l)
		{
			bool flag = k.SignValue < 0;
			bool flag2 = l.SignValue < 0;
			k = k.Abs();
			l = l.Abs();
			int width = Math.Max(2, Math.Min(16, WNafUtilities.GetWindowSize(k.BitLength)));
			int width2 = Math.Max(2, Math.Min(16, WNafUtilities.GetWindowSize(l.BitLength)));
			WNafPreCompInfo wnafPreCompInfo = WNafUtilities.Precompute(P, width, true);
			WNafPreCompInfo wnafPreCompInfo2 = WNafUtilities.Precompute(Q, width2, true);
			ECPoint[] preCompP = flag ? wnafPreCompInfo.PreCompNeg : wnafPreCompInfo.PreComp;
			ECPoint[] preCompQ = flag2 ? wnafPreCompInfo2.PreCompNeg : wnafPreCompInfo2.PreComp;
			ECPoint[] preCompNegP = flag ? wnafPreCompInfo.PreComp : wnafPreCompInfo.PreCompNeg;
			ECPoint[] preCompNegQ = flag2 ? wnafPreCompInfo2.PreComp : wnafPreCompInfo2.PreCompNeg;
			byte[] wnafP = WNafUtilities.GenerateWindowNaf(width, k);
			byte[] wnafQ = WNafUtilities.GenerateWindowNaf(width2, l);
			return ECAlgorithms.ImplShamirsTrickWNaf(preCompP, preCompNegP, wnafP, preCompQ, preCompNegQ, wnafQ);
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000E1730 File Offset: 0x000DF930
		internal static ECPoint ImplShamirsTrickWNaf(ECPoint P, BigInteger k, ECPointMap pointMapQ, BigInteger l)
		{
			bool flag = k.SignValue < 0;
			bool flag2 = l.SignValue < 0;
			k = k.Abs();
			l = l.Abs();
			int width = Math.Max(2, Math.Min(16, WNafUtilities.GetWindowSize(Math.Max(k.BitLength, l.BitLength))));
			ECPoint p = WNafUtilities.MapPointWithPrecomp(P, width, true, pointMapQ);
			WNafPreCompInfo wnafPreCompInfo = WNafUtilities.GetWNafPreCompInfo(P);
			WNafPreCompInfo wnafPreCompInfo2 = WNafUtilities.GetWNafPreCompInfo(p);
			ECPoint[] preCompP = flag ? wnafPreCompInfo.PreCompNeg : wnafPreCompInfo.PreComp;
			ECPoint[] preCompQ = flag2 ? wnafPreCompInfo2.PreCompNeg : wnafPreCompInfo2.PreComp;
			ECPoint[] preCompNegP = flag ? wnafPreCompInfo.PreComp : wnafPreCompInfo.PreCompNeg;
			ECPoint[] preCompNegQ = flag2 ? wnafPreCompInfo2.PreComp : wnafPreCompInfo2.PreCompNeg;
			byte[] wnafP = WNafUtilities.GenerateWindowNaf(width, k);
			byte[] wnafQ = WNafUtilities.GenerateWindowNaf(width, l);
			return ECAlgorithms.ImplShamirsTrickWNaf(preCompP, preCompNegP, wnafP, preCompQ, preCompNegQ, wnafQ);
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000E1808 File Offset: 0x000DFA08
		private static ECPoint ImplShamirsTrickWNaf(ECPoint[] preCompP, ECPoint[] preCompNegP, byte[] wnafP, ECPoint[] preCompQ, ECPoint[] preCompNegQ, byte[] wnafQ)
		{
			int num = Math.Max(wnafP.Length, wnafQ.Length);
			ECPoint infinity = preCompP[0].Curve.Infinity;
			ECPoint ecpoint = infinity;
			int num2 = 0;
			for (int i = num - 1; i >= 0; i--)
			{
				int num3 = (int)((i < wnafP.Length) ? ((sbyte)wnafP[i]) : 0);
				int num4 = (int)((i < wnafQ.Length) ? ((sbyte)wnafQ[i]) : 0);
				if ((num3 | num4) == 0)
				{
					num2++;
				}
				else
				{
					ECPoint ecpoint2 = infinity;
					if (num3 != 0)
					{
						int num5 = Math.Abs(num3);
						ECPoint[] array = (num3 < 0) ? preCompNegP : preCompP;
						ecpoint2 = ecpoint2.Add(array[num5 >> 1]);
					}
					if (num4 != 0)
					{
						int num6 = Math.Abs(num4);
						ECPoint[] array2 = (num4 < 0) ? preCompNegQ : preCompQ;
						ecpoint2 = ecpoint2.Add(array2[num6 >> 1]);
					}
					if (num2 > 0)
					{
						ecpoint = ecpoint.TimesPow2(num2);
						num2 = 0;
					}
					ecpoint = ecpoint.TwicePlus(ecpoint2);
				}
			}
			if (num2 > 0)
			{
				ecpoint = ecpoint.TimesPow2(num2);
			}
			return ecpoint;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000E18EC File Offset: 0x000DFAEC
		internal static ECPoint ImplSumOfMultiplies(ECPoint[] ps, BigInteger[] ks)
		{
			int num = ps.Length;
			bool[] array = new bool[num];
			WNafPreCompInfo[] array2 = new WNafPreCompInfo[num];
			byte[][] array3 = new byte[num][];
			for (int i = 0; i < num; i++)
			{
				BigInteger bigInteger = ks[i];
				array[i] = (bigInteger.SignValue < 0);
				bigInteger = bigInteger.Abs();
				int width = Math.Max(2, Math.Min(16, WNafUtilities.GetWindowSize(bigInteger.BitLength)));
				array2[i] = WNafUtilities.Precompute(ps[i], width, true);
				array3[i] = WNafUtilities.GenerateWindowNaf(width, bigInteger);
			}
			return ECAlgorithms.ImplSumOfMultiplies(array, array2, array3);
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000E1980 File Offset: 0x000DFB80
		internal static ECPoint ImplSumOfMultipliesGlv(ECPoint[] ps, BigInteger[] ks, GlvEndomorphism glvEndomorphism)
		{
			BigInteger order = ps[0].Curve.Order;
			int num = ps.Length;
			BigInteger[] array = new BigInteger[num << 1];
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				BigInteger[] array2 = glvEndomorphism.DecomposeScalar(ks[i].Mod(order));
				array[num2++] = array2[0];
				array[num2++] = array2[1];
				i++;
			}
			ECPointMap pointMap = glvEndomorphism.PointMap;
			if (glvEndomorphism.HasEfficientPointMap)
			{
				return ECAlgorithms.ImplSumOfMultiplies(ps, pointMap, array);
			}
			ECPoint[] array3 = new ECPoint[num << 1];
			int j = 0;
			int num3 = 0;
			while (j < num)
			{
				ECPoint ecpoint = ps[j];
				ECPoint ecpoint2 = pointMap.Map(ecpoint);
				array3[num3++] = ecpoint;
				array3[num3++] = ecpoint2;
				j++;
			}
			return ECAlgorithms.ImplSumOfMultiplies(array3, array);
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000E1A4C File Offset: 0x000DFC4C
		internal static ECPoint ImplSumOfMultiplies(ECPoint[] ps, ECPointMap pointMap, BigInteger[] ks)
		{
			int num = ps.Length;
			int num2 = num << 1;
			bool[] array = new bool[num2];
			WNafPreCompInfo[] array2 = new WNafPreCompInfo[num2];
			byte[][] array3 = new byte[num2][];
			for (int i = 0; i < num; i++)
			{
				int num3 = i << 1;
				int num4 = num3 + 1;
				BigInteger bigInteger = ks[num3];
				array[num3] = (bigInteger.SignValue < 0);
				bigInteger = bigInteger.Abs();
				BigInteger bigInteger2 = ks[num4];
				array[num4] = (bigInteger2.SignValue < 0);
				bigInteger2 = bigInteger2.Abs();
				int width = Math.Max(2, Math.Min(16, WNafUtilities.GetWindowSize(Math.Max(bigInteger.BitLength, bigInteger2.BitLength))));
				ECPoint p = ps[i];
				ECPoint p2 = WNafUtilities.MapPointWithPrecomp(p, width, true, pointMap);
				array2[num3] = WNafUtilities.GetWNafPreCompInfo(p);
				array2[num4] = WNafUtilities.GetWNafPreCompInfo(p2);
				array3[num3] = WNafUtilities.GenerateWindowNaf(width, bigInteger);
				array3[num4] = WNafUtilities.GenerateWindowNaf(width, bigInteger2);
			}
			return ECAlgorithms.ImplSumOfMultiplies(array, array2, array3);
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000E1B44 File Offset: 0x000DFD44
		private static ECPoint ImplSumOfMultiplies(bool[] negs, WNafPreCompInfo[] infos, byte[][] wnafs)
		{
			int num = 0;
			int num2 = wnafs.Length;
			for (int i = 0; i < num2; i++)
			{
				num = Math.Max(num, wnafs[i].Length);
			}
			ECPoint infinity = infos[0].PreComp[0].Curve.Infinity;
			ECPoint ecpoint = infinity;
			int num3 = 0;
			for (int j = num - 1; j >= 0; j--)
			{
				ECPoint ecpoint2 = infinity;
				for (int k = 0; k < num2; k++)
				{
					byte[] array = wnafs[k];
					int num4 = (int)((j < array.Length) ? ((sbyte)array[j]) : 0);
					if (num4 != 0)
					{
						int num5 = Math.Abs(num4);
						WNafPreCompInfo wnafPreCompInfo = infos[k];
						ECPoint[] array2 = (num4 < 0 == negs[k]) ? wnafPreCompInfo.PreComp : wnafPreCompInfo.PreCompNeg;
						ecpoint2 = ecpoint2.Add(array2[num5 >> 1]);
					}
				}
				if (ecpoint2 == infinity)
				{
					num3++;
				}
				else
				{
					if (num3 > 0)
					{
						ecpoint = ecpoint.TimesPow2(num3);
						num3 = 0;
					}
					ecpoint = ecpoint.TwicePlus(ecpoint2);
				}
			}
			if (num3 > 0)
			{
				ecpoint = ecpoint.TimesPow2(num3);
			}
			return ecpoint;
		}
	}
}
