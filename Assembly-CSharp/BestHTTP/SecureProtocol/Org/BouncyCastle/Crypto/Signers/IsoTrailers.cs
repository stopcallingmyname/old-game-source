using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A4 RID: 1188
	public class IsoTrailers
	{
		// Token: 0x06002E94 RID: 11924 RVA: 0x00121D34 File Offset: 0x0011FF34
		private static IDictionary CreateTrailerMap()
		{
			IDictionary dictionary = Platform.CreateHashtable();
			dictionary.Add("RIPEMD128", 13004);
			dictionary.Add("RIPEMD160", 12748);
			dictionary.Add("SHA-1", 13260);
			dictionary.Add("SHA-224", 14540);
			dictionary.Add("SHA-256", 13516);
			dictionary.Add("SHA-384", 14028);
			dictionary.Add("SHA-512", 13772);
			dictionary.Add("SHA-512/224", 14796);
			dictionary.Add("SHA-512/256", 16588);
			dictionary.Add("Whirlpool", 14284);
			return CollectionUtilities.ReadOnly(dictionary);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x00121E1D File Offset: 0x0012001D
		public static int GetTrailer(IDigest digest)
		{
			return (int)IsoTrailers.trailerMap[digest.AlgorithmName];
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x00121E34 File Offset: 0x00120034
		public static bool NoTrailerAvailable(IDigest digest)
		{
			return !IsoTrailers.trailerMap.Contains(digest.AlgorithmName);
		}

		// Token: 0x04001F20 RID: 7968
		public const int TRAILER_IMPLICIT = 188;

		// Token: 0x04001F21 RID: 7969
		public const int TRAILER_RIPEMD160 = 12748;

		// Token: 0x04001F22 RID: 7970
		public const int TRAILER_RIPEMD128 = 13004;

		// Token: 0x04001F23 RID: 7971
		public const int TRAILER_SHA1 = 13260;

		// Token: 0x04001F24 RID: 7972
		public const int TRAILER_SHA256 = 13516;

		// Token: 0x04001F25 RID: 7973
		public const int TRAILER_SHA512 = 13772;

		// Token: 0x04001F26 RID: 7974
		public const int TRAILER_SHA384 = 14028;

		// Token: 0x04001F27 RID: 7975
		public const int TRAILER_WHIRLPOOL = 14284;

		// Token: 0x04001F28 RID: 7976
		public const int TRAILER_SHA224 = 14540;

		// Token: 0x04001F29 RID: 7977
		public const int TRAILER_SHA512_224 = 14796;

		// Token: 0x04001F2A RID: 7978
		public const int TRAILER_SHA512_256 = 16588;

		// Token: 0x04001F2B RID: 7979
		private static readonly IDictionary trailerMap = IsoTrailers.CreateTrailerMap();
	}
}
