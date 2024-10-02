using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FA RID: 1018
	public abstract class AbstractTlsSignerCredentials : AbstractTlsCredentials, TlsSignerCredentials, TlsCredentials
	{
		// Token: 0x06002943 RID: 10563
		public abstract byte[] GenerateCertificateSignature(byte[] hash);

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002944 RID: 10564 RVA: 0x0010DCDA File Offset: 0x0010BEDA
		public virtual SignatureAndHashAlgorithm SignatureAndHashAlgorithm
		{
			get
			{
				throw new InvalidOperationException("TlsSignerCredentials implementation does not support (D)TLS 1.2+");
			}
		}
	}
}
