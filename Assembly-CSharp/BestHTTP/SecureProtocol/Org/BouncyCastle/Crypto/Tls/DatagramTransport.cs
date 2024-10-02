using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000413 RID: 1043
	public interface DatagramTransport
	{
		// Token: 0x060029BF RID: 10687
		int GetReceiveLimit();

		// Token: 0x060029C0 RID: 10688
		int GetSendLimit();

		// Token: 0x060029C1 RID: 10689
		int Receive(byte[] buf, int off, int len, int waitMillis);

		// Token: 0x060029C2 RID: 10690
		void Send(byte[] buf, int off, int len);

		// Token: 0x060029C3 RID: 10691
		void Close();
	}
}
