using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x0200052A RID: 1322
	public interface IGcmExponentiator
	{
		// Token: 0x06003229 RID: 12841
		void Init(byte[] x);

		// Token: 0x0600322A RID: 12842
		void ExponentiateX(long pow, byte[] output);
	}
}
