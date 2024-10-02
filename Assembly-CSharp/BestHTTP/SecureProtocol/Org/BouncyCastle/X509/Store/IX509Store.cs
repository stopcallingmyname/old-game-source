using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x02000252 RID: 594
	public interface IX509Store
	{
		// Token: 0x0600159F RID: 5535
		ICollection GetMatches(IX509Selector selector);
	}
}
