using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x02000528 RID: 1320
	public class BasicGcmMultiplier : IGcmMultiplier
	{
		// Token: 0x06003203 RID: 12803 RVA: 0x0012E10D File Offset: 0x0012C30D
		public void Init(byte[] H)
		{
			this.H = GcmUtilities.AsUints(H);
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x0012E11B File Offset: 0x0012C31B
		public void MultiplyH(byte[] x)
		{
			uint[] x2 = GcmUtilities.AsUints(x);
			GcmUtilities.Multiply(x2, this.H);
			GcmUtilities.AsBytes(x2, x);
		}

		// Token: 0x040020E9 RID: 8425
		private uint[] H;
	}
}
