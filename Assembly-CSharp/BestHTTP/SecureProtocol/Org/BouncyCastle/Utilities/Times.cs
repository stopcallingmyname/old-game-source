using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000268 RID: 616
	public sealed class Times
	{
		// Token: 0x0600168E RID: 5774 RVA: 0x000B0E40 File Offset: 0x000AF040
		public static long NanoTime()
		{
			return DateTime.UtcNow.Ticks * Times.NanosecondsPerTick;
		}

		// Token: 0x04001688 RID: 5768
		private static long NanosecondsPerTick = 100L;
	}
}
