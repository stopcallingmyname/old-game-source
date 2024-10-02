using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000434 RID: 1076
	public abstract class HeartbeatMessageType
	{
		// Token: 0x06002AAE RID: 10926 RVA: 0x00113119 File Offset: 0x00111319
		public static bool IsValid(byte heartbeatMessageType)
		{
			return heartbeatMessageType >= 1 && heartbeatMessageType <= 2;
		}

		// Token: 0x04001D54 RID: 7508
		public const byte heartbeat_request = 1;

		// Token: 0x04001D55 RID: 7509
		public const byte heartbeat_response = 2;
	}
}
