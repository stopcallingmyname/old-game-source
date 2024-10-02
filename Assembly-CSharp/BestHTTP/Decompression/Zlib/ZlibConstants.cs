using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x02000811 RID: 2065
	public static class ZlibConstants
	{
		// Token: 0x04003034 RID: 12340
		public const int WindowBitsMax = 15;

		// Token: 0x04003035 RID: 12341
		public const int WindowBitsDefault = 15;

		// Token: 0x04003036 RID: 12342
		public const int Z_OK = 0;

		// Token: 0x04003037 RID: 12343
		public const int Z_STREAM_END = 1;

		// Token: 0x04003038 RID: 12344
		public const int Z_NEED_DICT = 2;

		// Token: 0x04003039 RID: 12345
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x0400303A RID: 12346
		public const int Z_DATA_ERROR = -3;

		// Token: 0x0400303B RID: 12347
		public const int Z_BUF_ERROR = -5;

		// Token: 0x0400303C RID: 12348
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x0400303D RID: 12349
		public const int WorkingBufferSizeMin = 1024;
	}
}
