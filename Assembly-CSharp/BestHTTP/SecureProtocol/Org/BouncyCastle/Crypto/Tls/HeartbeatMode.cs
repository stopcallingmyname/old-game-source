using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000435 RID: 1077
	public abstract class HeartbeatMode
	{
		// Token: 0x06002AB0 RID: 10928 RVA: 0x00113119 File Offset: 0x00111319
		public static bool IsValid(byte heartbeatMode)
		{
			return heartbeatMode >= 1 && heartbeatMode <= 2;
		}

		// Token: 0x04001D56 RID: 7510
		public const byte peer_allowed_to_send = 1;

		// Token: 0x04001D57 RID: 7511
		public const byte peer_not_allowed_to_send = 2;
	}
}
