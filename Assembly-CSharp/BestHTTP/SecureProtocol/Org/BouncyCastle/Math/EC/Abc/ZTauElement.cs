using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Abc
{
	// Token: 0x020003C1 RID: 961
	internal class ZTauElement
	{
		// Token: 0x060027A8 RID: 10152 RVA: 0x0010BD8C File Offset: 0x00109F8C
		public ZTauElement(BigInteger u, BigInteger v)
		{
			this.u = u;
			this.v = v;
		}

		// Token: 0x04001AF0 RID: 6896
		public readonly BigInteger u;

		// Token: 0x04001AF1 RID: 6897
		public readonly BigInteger v;
	}
}
