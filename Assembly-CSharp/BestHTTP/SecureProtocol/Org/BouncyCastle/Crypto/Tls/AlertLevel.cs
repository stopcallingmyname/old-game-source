using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FC RID: 1020
	public abstract class AlertLevel
	{
		// Token: 0x06002949 RID: 10569 RVA: 0x0010DEE6 File Offset: 0x0010C0E6
		public static string GetName(byte alertDescription)
		{
			if (alertDescription == 1)
			{
				return "warning";
			}
			if (alertDescription != 2)
			{
				return "UNKNOWN";
			}
			return "fatal";
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x0010DF03 File Offset: 0x0010C103
		public static string GetText(byte alertDescription)
		{
			return string.Concat(new object[]
			{
				AlertLevel.GetName(alertDescription),
				"(",
				alertDescription,
				")"
			});
		}

		// Token: 0x04001B53 RID: 6995
		public const byte warning = 1;

		// Token: 0x04001B54 RID: 6996
		public const byte fatal = 2;
	}
}
