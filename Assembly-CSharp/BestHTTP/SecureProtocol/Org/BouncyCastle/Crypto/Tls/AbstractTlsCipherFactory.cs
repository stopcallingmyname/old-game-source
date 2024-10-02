using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F1 RID: 1009
	public class AbstractTlsCipherFactory : TlsCipherFactory
	{
		// Token: 0x060028CF RID: 10447 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		public virtual TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
		{
			throw new TlsFatalAlert(80);
		}
	}
}
