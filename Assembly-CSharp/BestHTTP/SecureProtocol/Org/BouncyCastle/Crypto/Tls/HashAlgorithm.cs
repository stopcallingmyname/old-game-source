using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000431 RID: 1073
	public abstract class HashAlgorithm
	{
		// Token: 0x06002AA2 RID: 10914 RVA: 0x00113154 File Offset: 0x00111354
		public static string GetName(byte hashAlgorithm)
		{
			switch (hashAlgorithm)
			{
			case 0:
				return "none";
			case 1:
				return "md5";
			case 2:
				return "sha1";
			case 3:
				return "sha224";
			case 4:
				return "sha256";
			case 5:
				return "sha384";
			case 6:
				return "sha512";
			default:
				return "UNKNOWN";
			}
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x001131B4 File Offset: 0x001113B4
		public static string GetText(byte hashAlgorithm)
		{
			return string.Concat(new object[]
			{
				HashAlgorithm.GetName(hashAlgorithm),
				"(",
				hashAlgorithm,
				")"
			});
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x001131E3 File Offset: 0x001113E3
		public static bool IsPrivate(byte hashAlgorithm)
		{
			return 224 <= hashAlgorithm && hashAlgorithm <= byte.MaxValue;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x001131FA File Offset: 0x001113FA
		public static bool IsRecognized(byte hashAlgorithm)
		{
			return hashAlgorithm - 1 <= 5;
		}

		// Token: 0x04001D49 RID: 7497
		public const byte none = 0;

		// Token: 0x04001D4A RID: 7498
		public const byte md5 = 1;

		// Token: 0x04001D4B RID: 7499
		public const byte sha1 = 2;

		// Token: 0x04001D4C RID: 7500
		public const byte sha224 = 3;

		// Token: 0x04001D4D RID: 7501
		public const byte sha256 = 4;

		// Token: 0x04001D4E RID: 7502
		public const byte sha384 = 5;

		// Token: 0x04001D4F RID: 7503
		public const byte sha512 = 6;
	}
}
