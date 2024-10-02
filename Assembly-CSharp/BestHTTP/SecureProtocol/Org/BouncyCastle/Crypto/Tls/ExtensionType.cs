using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200042E RID: 1070
	public abstract class ExtensionType
	{
		// Token: 0x04001D17 RID: 7447
		public const int server_name = 0;

		// Token: 0x04001D18 RID: 7448
		public const int max_fragment_length = 1;

		// Token: 0x04001D19 RID: 7449
		public const int client_certificate_url = 2;

		// Token: 0x04001D1A RID: 7450
		public const int trusted_ca_keys = 3;

		// Token: 0x04001D1B RID: 7451
		public const int truncated_hmac = 4;

		// Token: 0x04001D1C RID: 7452
		public const int status_request = 5;

		// Token: 0x04001D1D RID: 7453
		public const int user_mapping = 6;

		// Token: 0x04001D1E RID: 7454
		public const int client_authz = 7;

		// Token: 0x04001D1F RID: 7455
		public const int server_authz = 8;

		// Token: 0x04001D20 RID: 7456
		public const int cert_type = 9;

		// Token: 0x04001D21 RID: 7457
		public const int supported_groups = 10;

		// Token: 0x04001D22 RID: 7458
		[Obsolete("Use 'supported_groups' instead")]
		public const int elliptic_curves = 10;

		// Token: 0x04001D23 RID: 7459
		public const int ec_point_formats = 11;

		// Token: 0x04001D24 RID: 7460
		public const int srp = 12;

		// Token: 0x04001D25 RID: 7461
		public const int signature_algorithms = 13;

		// Token: 0x04001D26 RID: 7462
		public const int use_srtp = 14;

		// Token: 0x04001D27 RID: 7463
		public const int heartbeat = 15;

		// Token: 0x04001D28 RID: 7464
		public const int application_layer_protocol_negotiation = 16;

		// Token: 0x04001D29 RID: 7465
		public const int status_request_v2 = 17;

		// Token: 0x04001D2A RID: 7466
		public const int signed_certificate_timestamp = 18;

		// Token: 0x04001D2B RID: 7467
		public const int client_certificate_type = 19;

		// Token: 0x04001D2C RID: 7468
		public const int server_certificate_type = 20;

		// Token: 0x04001D2D RID: 7469
		public const int padding = 21;

		// Token: 0x04001D2E RID: 7470
		public const int encrypt_then_mac = 22;

		// Token: 0x04001D2F RID: 7471
		public const int extended_master_secret = 23;

		// Token: 0x04001D30 RID: 7472
		public static readonly int DRAFT_token_binding = 24;

		// Token: 0x04001D31 RID: 7473
		public const int cached_info = 25;

		// Token: 0x04001D32 RID: 7474
		public const int session_ticket = 35;

		// Token: 0x04001D33 RID: 7475
		public static readonly int negotiated_ff_dhe_groups = 101;

		// Token: 0x04001D34 RID: 7476
		public const int renegotiation_info = 65281;
	}
}
