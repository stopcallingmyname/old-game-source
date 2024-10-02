using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200042D RID: 1069
	public abstract class ExporterLabel
	{
		// Token: 0x04001D0E RID: 7438
		public const string client_finished = "client finished";

		// Token: 0x04001D0F RID: 7439
		public const string server_finished = "server finished";

		// Token: 0x04001D10 RID: 7440
		public const string master_secret = "master secret";

		// Token: 0x04001D11 RID: 7441
		public const string key_expansion = "key expansion";

		// Token: 0x04001D12 RID: 7442
		public const string client_EAP_encryption = "client EAP encryption";

		// Token: 0x04001D13 RID: 7443
		public const string ttls_keying_material = "ttls keying material";

		// Token: 0x04001D14 RID: 7444
		public const string ttls_challenge = "ttls challenge";

		// Token: 0x04001D15 RID: 7445
		public const string dtls_srtp = "EXTRACTOR-dtls_srtp";

		// Token: 0x04001D16 RID: 7446
		public static readonly string extended_master_secret = "extended master secret";
	}
}
