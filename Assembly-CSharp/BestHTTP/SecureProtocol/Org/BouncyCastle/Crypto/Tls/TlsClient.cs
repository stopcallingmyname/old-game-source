using System;
using System.Collections;
using System.Collections.Generic;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000458 RID: 1112
	public interface TlsClient : TlsPeer
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002B75 RID: 11125
		// (set) Token: 0x06002B76 RID: 11126
		List<string> HostNames { get; set; }

		// Token: 0x06002B77 RID: 11127
		void Init(TlsClientContext context);

		// Token: 0x06002B78 RID: 11128
		TlsSession GetSessionToResume();

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002B79 RID: 11129
		ProtocolVersion ClientHelloRecordLayerVersion { get; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002B7A RID: 11130
		ProtocolVersion ClientVersion { get; }

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002B7B RID: 11131
		bool IsFallback { get; }

		// Token: 0x06002B7C RID: 11132
		int[] GetCipherSuites();

		// Token: 0x06002B7D RID: 11133
		byte[] GetCompressionMethods();

		// Token: 0x06002B7E RID: 11134
		IDictionary GetClientExtensions();

		// Token: 0x06002B7F RID: 11135
		void NotifyServerVersion(ProtocolVersion selectedVersion);

		// Token: 0x06002B80 RID: 11136
		void NotifySessionID(byte[] sessionID);

		// Token: 0x06002B81 RID: 11137
		void NotifySelectedCipherSuite(int selectedCipherSuite);

		// Token: 0x06002B82 RID: 11138
		void NotifySelectedCompressionMethod(byte selectedCompressionMethod);

		// Token: 0x06002B83 RID: 11139
		void ProcessServerExtensions(IDictionary serverExtensions);

		// Token: 0x06002B84 RID: 11140
		void ProcessServerSupplementalData(IList serverSupplementalData);

		// Token: 0x06002B85 RID: 11141
		TlsKeyExchange GetKeyExchange();

		// Token: 0x06002B86 RID: 11142
		TlsAuthentication GetAuthentication();

		// Token: 0x06002B87 RID: 11143
		IList GetClientSupplementalData();

		// Token: 0x06002B88 RID: 11144
		void NotifyNewSessionTicket(NewSessionTicket newSessionTicket);
	}
}
