using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200043A RID: 1082
	public abstract class NameType
	{
		// Token: 0x06002AB9 RID: 10937 RVA: 0x001133E6 File Offset: 0x001115E6
		public static bool IsValid(byte nameType)
		{
			return nameType == 0;
		}

		// Token: 0x04001D9B RID: 7579
		public const byte host_name = 0;
	}
}
