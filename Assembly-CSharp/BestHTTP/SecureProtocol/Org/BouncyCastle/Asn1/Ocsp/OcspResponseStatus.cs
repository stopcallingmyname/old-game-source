using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200070E RID: 1806
	public class OcspResponseStatus : DerEnumerated
	{
		// Token: 0x060041EF RID: 16879 RVA: 0x00176A79 File Offset: 0x00174C79
		public OcspResponseStatus(int value) : base(value)
		{
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x00176A82 File Offset: 0x00174C82
		public OcspResponseStatus(DerEnumerated value) : base(value.Value.IntValue)
		{
		}

		// Token: 0x04002ABA RID: 10938
		public const int Successful = 0;

		// Token: 0x04002ABB RID: 10939
		public const int MalformedRequest = 1;

		// Token: 0x04002ABC RID: 10940
		public const int InternalError = 2;

		// Token: 0x04002ABD RID: 10941
		public const int TryLater = 3;

		// Token: 0x04002ABE RID: 10942
		public const int SignatureRequired = 5;

		// Token: 0x04002ABF RID: 10943
		public const int Unauthorized = 6;
	}
}
