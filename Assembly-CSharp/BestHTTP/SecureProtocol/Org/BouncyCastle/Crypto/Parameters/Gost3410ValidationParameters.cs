using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E6 RID: 1254
	public class Gost3410ValidationParameters
	{
		// Token: 0x0600304D RID: 12365 RVA: 0x001274D6 File Offset: 0x001256D6
		public Gost3410ValidationParameters(int x0, int c)
		{
			this.x0 = x0;
			this.c = c;
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x001274EC File Offset: 0x001256EC
		public Gost3410ValidationParameters(long x0L, long cL)
		{
			this.x0L = x0L;
			this.cL = cL;
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x00127502 File Offset: 0x00125702
		public int C
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x0012750A File Offset: 0x0012570A
		public int X0
		{
			get
			{
				return this.x0;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x00127512 File Offset: 0x00125712
		public long CL
		{
			get
			{
				return this.cL;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x0012751A File Offset: 0x0012571A
		public long X0L
		{
			get
			{
				return this.x0L;
			}
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x00127524 File Offset: 0x00125724
		public override bool Equals(object obj)
		{
			Gost3410ValidationParameters gost3410ValidationParameters = obj as Gost3410ValidationParameters;
			return gost3410ValidationParameters != null && gost3410ValidationParameters.c == this.c && gost3410ValidationParameters.x0 == this.x0 && gost3410ValidationParameters.cL == this.cL && gost3410ValidationParameters.x0L == this.x0L;
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x00127575 File Offset: 0x00125775
		public override int GetHashCode()
		{
			return this.c.GetHashCode() ^ this.x0.GetHashCode() ^ this.cL.GetHashCode() ^ this.x0L.GetHashCode();
		}

		// Token: 0x04001FFA RID: 8186
		private int x0;

		// Token: 0x04001FFB RID: 8187
		private int c;

		// Token: 0x04001FFC RID: 8188
		private long x0L;

		// Token: 0x04001FFD RID: 8189
		private long cL;
	}
}
