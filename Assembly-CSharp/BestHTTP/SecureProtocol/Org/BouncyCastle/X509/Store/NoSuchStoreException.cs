using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x02000254 RID: 596
	[Serializable]
	public class NoSuchStoreException : X509StoreException
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x000AEBE5 File Offset: 0x000ACDE5
		public NoSuchStoreException()
		{
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x000AEBED File Offset: 0x000ACDED
		public NoSuchStoreException(string message) : base(message)
		{
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x000AEBF6 File Offset: 0x000ACDF6
		public NoSuchStoreException(string message, Exception e) : base(message, e)
		{
		}
	}
}
