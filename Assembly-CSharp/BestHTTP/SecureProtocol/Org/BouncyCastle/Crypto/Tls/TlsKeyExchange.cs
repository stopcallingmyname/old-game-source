using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000470 RID: 1136
	public interface TlsKeyExchange
	{
		// Token: 0x06002C65 RID: 11365
		void Init(TlsContext context);

		// Token: 0x06002C66 RID: 11366
		void SkipServerCredentials();

		// Token: 0x06002C67 RID: 11367
		void ProcessServerCredentials(TlsCredentials serverCredentials);

		// Token: 0x06002C68 RID: 11368
		void ProcessServerCertificate(Certificate serverCertificate);

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002C69 RID: 11369
		bool RequiresServerKeyExchange { get; }

		// Token: 0x06002C6A RID: 11370
		byte[] GenerateServerKeyExchange();

		// Token: 0x06002C6B RID: 11371
		void SkipServerKeyExchange();

		// Token: 0x06002C6C RID: 11372
		void ProcessServerKeyExchange(Stream input);

		// Token: 0x06002C6D RID: 11373
		void ValidateCertificateRequest(CertificateRequest certificateRequest);

		// Token: 0x06002C6E RID: 11374
		void SkipClientCredentials();

		// Token: 0x06002C6F RID: 11375
		void ProcessClientCredentials(TlsCredentials clientCredentials);

		// Token: 0x06002C70 RID: 11376
		void ProcessClientCertificate(Certificate clientCertificate);

		// Token: 0x06002C71 RID: 11377
		void GenerateClientKeyExchange(Stream output);

		// Token: 0x06002C72 RID: 11378
		void ProcessClientKeyExchange(Stream input);

		// Token: 0x06002C73 RID: 11379
		byte[] GeneratePremasterSecret();
	}
}
