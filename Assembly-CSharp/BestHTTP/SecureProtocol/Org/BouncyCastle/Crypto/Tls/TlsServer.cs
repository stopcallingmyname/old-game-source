using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200047E RID: 1150
	public interface TlsServer : TlsPeer
	{
		// Token: 0x06002CF8 RID: 11512
		void Init(TlsServerContext context);

		// Token: 0x06002CF9 RID: 11513
		void NotifyClientVersion(ProtocolVersion clientVersion);

		// Token: 0x06002CFA RID: 11514
		void NotifyFallback(bool isFallback);

		// Token: 0x06002CFB RID: 11515
		void NotifyOfferedCipherSuites(int[] offeredCipherSuites);

		// Token: 0x06002CFC RID: 11516
		void NotifyOfferedCompressionMethods(byte[] offeredCompressionMethods);

		// Token: 0x06002CFD RID: 11517
		void ProcessClientExtensions(IDictionary clientExtensions);

		// Token: 0x06002CFE RID: 11518
		ProtocolVersion GetServerVersion();

		// Token: 0x06002CFF RID: 11519
		int GetSelectedCipherSuite();

		// Token: 0x06002D00 RID: 11520
		byte GetSelectedCompressionMethod();

		// Token: 0x06002D01 RID: 11521
		IDictionary GetServerExtensions();

		// Token: 0x06002D02 RID: 11522
		IList GetServerSupplementalData();

		// Token: 0x06002D03 RID: 11523
		TlsCredentials GetCredentials();

		// Token: 0x06002D04 RID: 11524
		CertificateStatus GetCertificateStatus();

		// Token: 0x06002D05 RID: 11525
		TlsKeyExchange GetKeyExchange();

		// Token: 0x06002D06 RID: 11526
		CertificateRequest GetCertificateRequest();

		// Token: 0x06002D07 RID: 11527
		void ProcessClientSupplementalData(IList clientSupplementalData);

		// Token: 0x06002D08 RID: 11528
		void NotifyClientCertificate(Certificate clientCertificate);

		// Token: 0x06002D09 RID: 11529
		NewSessionTicket GetNewSessionTicket();
	}
}
