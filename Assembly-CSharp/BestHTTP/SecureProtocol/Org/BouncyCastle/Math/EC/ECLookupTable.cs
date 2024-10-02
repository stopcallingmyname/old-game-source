using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000327 RID: 807
	public interface ECLookupTable
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001ECE RID: 7886
		int Size { get; }

		// Token: 0x06001ECF RID: 7887
		ECPoint Lookup(int index);
	}
}
