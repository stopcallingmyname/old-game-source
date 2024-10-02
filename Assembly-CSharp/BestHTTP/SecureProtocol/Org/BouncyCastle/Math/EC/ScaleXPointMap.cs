using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000330 RID: 816
	public class ScaleXPointMap : ECPointMap
	{
		// Token: 0x06001F73 RID: 8051 RVA: 0x000E82BD File Offset: 0x000E64BD
		public ScaleXPointMap(ECFieldElement scale)
		{
			this.scale = scale;
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000E82CC File Offset: 0x000E64CC
		public virtual ECPoint Map(ECPoint p)
		{
			return p.ScaleX(this.scale);
		}

		// Token: 0x04001981 RID: 6529
		protected readonly ECFieldElement scale;
	}
}
