using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C1 RID: 1217
	[Obsolete("Use AeadParameters")]
	public class CcmParameters : AeadParameters
	{
		// Token: 0x06002F67 RID: 12135 RVA: 0x0012574B File Offset: 0x0012394B
		public CcmParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText) : base(key, macSize, nonce, associatedText)
		{
		}
	}
}
