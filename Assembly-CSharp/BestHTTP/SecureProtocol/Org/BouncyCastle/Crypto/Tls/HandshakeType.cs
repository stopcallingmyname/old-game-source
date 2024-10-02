using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000430 RID: 1072
	public abstract class HandshakeType
	{
		// Token: 0x04001D3A RID: 7482
		public const byte hello_request = 0;

		// Token: 0x04001D3B RID: 7483
		public const byte client_hello = 1;

		// Token: 0x04001D3C RID: 7484
		public const byte server_hello = 2;

		// Token: 0x04001D3D RID: 7485
		public const byte certificate = 11;

		// Token: 0x04001D3E RID: 7486
		public const byte server_key_exchange = 12;

		// Token: 0x04001D3F RID: 7487
		public const byte certificate_request = 13;

		// Token: 0x04001D40 RID: 7488
		public const byte server_hello_done = 14;

		// Token: 0x04001D41 RID: 7489
		public const byte certificate_verify = 15;

		// Token: 0x04001D42 RID: 7490
		public const byte client_key_exchange = 16;

		// Token: 0x04001D43 RID: 7491
		public const byte finished = 20;

		// Token: 0x04001D44 RID: 7492
		public const byte certificate_url = 21;

		// Token: 0x04001D45 RID: 7493
		public const byte certificate_status = 22;

		// Token: 0x04001D46 RID: 7494
		public const byte hello_verify_request = 3;

		// Token: 0x04001D47 RID: 7495
		public const byte supplemental_data = 23;

		// Token: 0x04001D48 RID: 7496
		public const byte session_ticket = 4;
	}
}
