using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x0200026F RID: 623
	public sealed class JZlib
	{
		// Token: 0x060016D9 RID: 5849 RVA: 0x000B6572 File Offset: 0x000B4772
		public static string version()
		{
			return "1.0.7";
		}

		// Token: 0x04001788 RID: 6024
		private const string _version = "1.0.7";

		// Token: 0x04001789 RID: 6025
		public const int Z_NO_COMPRESSION = 0;

		// Token: 0x0400178A RID: 6026
		public const int Z_BEST_SPEED = 1;

		// Token: 0x0400178B RID: 6027
		public const int Z_BEST_COMPRESSION = 9;

		// Token: 0x0400178C RID: 6028
		public const int Z_DEFAULT_COMPRESSION = -1;

		// Token: 0x0400178D RID: 6029
		public const int Z_FILTERED = 1;

		// Token: 0x0400178E RID: 6030
		public const int Z_HUFFMAN_ONLY = 2;

		// Token: 0x0400178F RID: 6031
		public const int Z_DEFAULT_STRATEGY = 0;

		// Token: 0x04001790 RID: 6032
		public const int Z_NO_FLUSH = 0;

		// Token: 0x04001791 RID: 6033
		public const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x04001792 RID: 6034
		public const int Z_SYNC_FLUSH = 2;

		// Token: 0x04001793 RID: 6035
		public const int Z_FULL_FLUSH = 3;

		// Token: 0x04001794 RID: 6036
		public const int Z_FINISH = 4;

		// Token: 0x04001795 RID: 6037
		public const int Z_OK = 0;

		// Token: 0x04001796 RID: 6038
		public const int Z_STREAM_END = 1;

		// Token: 0x04001797 RID: 6039
		public const int Z_NEED_DICT = 2;

		// Token: 0x04001798 RID: 6040
		public const int Z_ERRNO = -1;

		// Token: 0x04001799 RID: 6041
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x0400179A RID: 6042
		public const int Z_DATA_ERROR = -3;

		// Token: 0x0400179B RID: 6043
		public const int Z_MEM_ERROR = -4;

		// Token: 0x0400179C RID: 6044
		public const int Z_BUF_ERROR = -5;

		// Token: 0x0400179D RID: 6045
		public const int Z_VERSION_ERROR = -6;
	}
}
