using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200045D RID: 1117
	public interface TlsContext
	{
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002B9C RID: 11164
		IRandomGenerator NonceRandomGenerator { get; }

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002B9D RID: 11165
		SecureRandom SecureRandom { get; }

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002B9E RID: 11166
		SecurityParameters SecurityParameters { get; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002B9F RID: 11167
		bool IsServer { get; }

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002BA0 RID: 11168
		ProtocolVersion ClientVersion { get; }

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002BA1 RID: 11169
		ProtocolVersion ServerVersion { get; }

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06002BA2 RID: 11170
		TlsSession ResumableSession { get; }

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06002BA3 RID: 11171
		// (set) Token: 0x06002BA4 RID: 11172
		object UserObject { get; set; }

		// Token: 0x06002BA5 RID: 11173
		byte[] ExportKeyingMaterial(string asciiLabel, byte[] context_value, int length);
	}
}
