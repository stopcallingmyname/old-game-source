using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006AA RID: 1706
	public sealed class PolicyQualifierID : DerObjectIdentifier
	{
		// Token: 0x06003F01 RID: 16129 RVA: 0x00176905 File Offset: 0x00174B05
		private PolicyQualifierID(string id) : base(id)
		{
		}

		// Token: 0x040027FF RID: 10239
		private const string IdQt = "1.3.6.1.5.5.7.2";

		// Token: 0x04002800 RID: 10240
		public static readonly PolicyQualifierID IdQtCps = new PolicyQualifierID("1.3.6.1.5.5.7.2.1");

		// Token: 0x04002801 RID: 10241
		public static readonly PolicyQualifierID IdQtUnotice = new PolicyQualifierID("1.3.6.1.5.5.7.2.2");
	}
}
