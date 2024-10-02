using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000472 RID: 1138
	public class TlsNoCloseNotifyException : EndOfStreamException
	{
		// Token: 0x06002C7B RID: 11387 RVA: 0x001185C6 File Offset: 0x001167C6
		public TlsNoCloseNotifyException() : base("No close_notify alert received before connection closed")
		{
		}
	}
}
