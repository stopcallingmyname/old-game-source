using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x0200025B RID: 603
	[Serializable]
	public class X509StoreException : Exception
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public X509StoreException()
		{
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0008BF89 File Offset: 0x0008A189
		public X509StoreException(string message) : base(message)
		{
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0008BF92 File Offset: 0x0008A192
		public X509StoreException(string message, Exception e) : base(message, e)
		{
		}
	}
}
