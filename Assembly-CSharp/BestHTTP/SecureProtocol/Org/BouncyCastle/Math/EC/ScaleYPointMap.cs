using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000331 RID: 817
	public class ScaleYPointMap : ECPointMap
	{
		// Token: 0x06001F75 RID: 8053 RVA: 0x000E82DA File Offset: 0x000E64DA
		public ScaleYPointMap(ECFieldElement scale)
		{
			this.scale = scale;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x000E82E9 File Offset: 0x000E64E9
		public virtual ECPoint Map(ECPoint p)
		{
			return p.ScaleY(this.scale);
		}

		// Token: 0x04001982 RID: 6530
		protected readonly ECFieldElement scale;
	}
}
