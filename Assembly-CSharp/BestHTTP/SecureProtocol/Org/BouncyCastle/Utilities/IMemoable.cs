using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000263 RID: 611
	public interface IMemoable
	{
		// Token: 0x06001665 RID: 5733
		IMemoable Copy();

		// Token: 0x06001666 RID: 5734
		void Reset(IMemoable other);
	}
}
