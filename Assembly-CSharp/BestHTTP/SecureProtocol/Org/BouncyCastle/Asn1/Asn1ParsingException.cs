using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200062C RID: 1580
	[Serializable]
	public class Asn1ParsingException : InvalidOperationException
	{
		// Token: 0x06003B7C RID: 15228 RVA: 0x0016EBB8 File Offset: 0x0016CDB8
		public Asn1ParsingException()
		{
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0016EBC0 File Offset: 0x0016CDC0
		public Asn1ParsingException(string message) : base(message)
		{
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0016EBC9 File Offset: 0x0016CDC9
		public Asn1ParsingException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
