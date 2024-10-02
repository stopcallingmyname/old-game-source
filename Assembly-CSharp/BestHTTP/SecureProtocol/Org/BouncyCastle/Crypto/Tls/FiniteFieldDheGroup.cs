using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200042F RID: 1071
	public abstract class FiniteFieldDheGroup
	{
		// Token: 0x06002A9F RID: 10911 RVA: 0x00113144 File Offset: 0x00111344
		public static bool IsValid(byte group)
		{
			return group >= 0 && group <= 4;
		}

		// Token: 0x04001D35 RID: 7477
		public const byte ffdhe2432 = 0;

		// Token: 0x04001D36 RID: 7478
		public const byte ffdhe3072 = 1;

		// Token: 0x04001D37 RID: 7479
		public const byte ffdhe4096 = 2;

		// Token: 0x04001D38 RID: 7480
		public const byte ffdhe6144 = 3;

		// Token: 0x04001D39 RID: 7481
		public const byte ffdhe8192 = 4;
	}
}
