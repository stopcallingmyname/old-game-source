using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x0200052B RID: 1323
	public interface IGcmMultiplier
	{
		// Token: 0x0600322B RID: 12843
		void Init(byte[] H);

		// Token: 0x0600322C RID: 12844
		void MultiplyH(byte[] x);
	}
}
