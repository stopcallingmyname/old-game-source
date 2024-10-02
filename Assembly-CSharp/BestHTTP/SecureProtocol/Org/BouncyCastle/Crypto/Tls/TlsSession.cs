using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000482 RID: 1154
	public interface TlsSession
	{
		// Token: 0x06002D22 RID: 11554
		SessionParameters ExportSessionParameters();

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002D23 RID: 11555
		byte[] SessionID { get; }

		// Token: 0x06002D24 RID: 11556
		void Invalidate();

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002D25 RID: 11557
		bool IsResumable { get; }
	}
}
