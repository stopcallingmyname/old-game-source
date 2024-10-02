using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000421 RID: 1057
	internal interface DtlsHandshakeRetransmit
	{
		// Token: 0x06002A43 RID: 10819
		void ReceivedHandshakeRecord(int epoch, byte[] buf, int off, int len);
	}
}
