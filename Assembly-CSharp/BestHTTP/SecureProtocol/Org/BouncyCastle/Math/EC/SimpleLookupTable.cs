using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000332 RID: 818
	public class SimpleLookupTable : ECLookupTable
	{
		// Token: 0x06001F77 RID: 8055 RVA: 0x000E82F8 File Offset: 0x000E64F8
		private static ECPoint[] Copy(ECPoint[] points, int off, int len)
		{
			ECPoint[] array = new ECPoint[len];
			for (int i = 0; i < len; i++)
			{
				array[i] = points[off + i];
			}
			return array;
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x000E8321 File Offset: 0x000E6521
		public SimpleLookupTable(ECPoint[] points, int off, int len)
		{
			this.points = SimpleLookupTable.Copy(points, off, len);
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001F79 RID: 8057 RVA: 0x000E8337 File Offset: 0x000E6537
		public virtual int Size
		{
			get
			{
				return this.points.Length;
			}
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x000E8341 File Offset: 0x000E6541
		public virtual ECPoint Lookup(int index)
		{
			return this.points[index];
		}

		// Token: 0x04001983 RID: 6531
		private readonly ECPoint[] points;
	}
}
