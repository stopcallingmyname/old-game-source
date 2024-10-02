using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200033E RID: 830
	public class FixedPointUtilities
	{
		// Token: 0x06002049 RID: 8265 RVA: 0x000EF674 File Offset: 0x000ED874
		public static int GetCombSize(ECCurve c)
		{
			BigInteger order = c.Order;
			if (order != null)
			{
				return order.BitLength;
			}
			return c.FieldSize + 1;
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000EF69A File Offset: 0x000ED89A
		public static FixedPointPreCompInfo GetFixedPointPreCompInfo(PreCompInfo preCompInfo)
		{
			return preCompInfo as FixedPointPreCompInfo;
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000EF6A2 File Offset: 0x000ED8A2
		public static FixedPointPreCompInfo Precompute(ECPoint p)
		{
			return (FixedPointPreCompInfo)p.Curve.Precompute(p, FixedPointUtilities.PRECOMP_NAME, new FixedPointUtilities.FixedPointCallback(p));
		}

		// Token: 0x040019DE RID: 6622
		public static readonly string PRECOMP_NAME = "bc_fixed_point";

		// Token: 0x02000918 RID: 2328
		private class FixedPointCallback : IPreCompCallback
		{
			// Token: 0x06004E59 RID: 20057 RVA: 0x001B15B5 File Offset: 0x001AF7B5
			internal FixedPointCallback(ECPoint p)
			{
				this.m_p = p;
			}

			// Token: 0x06004E5A RID: 20058 RVA: 0x001B15C4 File Offset: 0x001AF7C4
			public PreCompInfo Precompute(PreCompInfo existing)
			{
				FixedPointPreCompInfo fixedPointPreCompInfo = (existing is FixedPointPreCompInfo) ? ((FixedPointPreCompInfo)existing) : null;
				ECCurve curve = this.m_p.Curve;
				int combSize = FixedPointUtilities.GetCombSize(curve);
				int num = (combSize > 250) ? 6 : 5;
				int num2 = 1 << num;
				if (this.CheckExisting(fixedPointPreCompInfo, num2))
				{
					return fixedPointPreCompInfo;
				}
				int e = (combSize + num - 1) / num;
				ECPoint[] array = new ECPoint[num + 1];
				array[0] = this.m_p;
				for (int i = 1; i < num; i++)
				{
					array[i] = array[i - 1].TimesPow2(e);
				}
				array[num] = array[0].Subtract(array[1]);
				curve.NormalizeAll(array);
				ECPoint[] array2 = new ECPoint[num2];
				array2[0] = array[0];
				for (int j = num - 1; j >= 0; j--)
				{
					ECPoint b = array[j];
					int num3 = 1 << j;
					for (int k = num3; k < num2; k += num3 << 1)
					{
						array2[k] = array2[k - num3].Add(b);
					}
				}
				curve.NormalizeAll(array2);
				return new FixedPointPreCompInfo
				{
					LookupTable = curve.CreateCacheSafeLookupTable(array2, 0, array2.Length),
					Offset = array[num],
					Width = num
				};
			}

			// Token: 0x06004E5B RID: 20059 RVA: 0x001B16FE File Offset: 0x001AF8FE
			private bool CheckExisting(FixedPointPreCompInfo existingFP, int n)
			{
				return existingFP != null && this.CheckTable(existingFP.LookupTable, n);
			}

			// Token: 0x06004E5C RID: 20060 RVA: 0x001B1712 File Offset: 0x001AF912
			private bool CheckTable(ECLookupTable table, int n)
			{
				return table != null && table.Size >= n;
			}

			// Token: 0x04003581 RID: 13697
			private readonly ECPoint m_p;
		}
	}
}
