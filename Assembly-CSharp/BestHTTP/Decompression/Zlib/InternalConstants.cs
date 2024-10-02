using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x0200080B RID: 2059
	internal static class InternalConstants
	{
		// Token: 0x04002FF8 RID: 12280
		internal static readonly int MAX_BITS = 15;

		// Token: 0x04002FF9 RID: 12281
		internal static readonly int BL_CODES = 19;

		// Token: 0x04002FFA RID: 12282
		internal static readonly int D_CODES = 30;

		// Token: 0x04002FFB RID: 12283
		internal static readonly int LITERALS = 256;

		// Token: 0x04002FFC RID: 12284
		internal static readonly int LENGTH_CODES = 29;

		// Token: 0x04002FFD RID: 12285
		internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;

		// Token: 0x04002FFE RID: 12286
		internal static readonly int MAX_BL_BITS = 7;

		// Token: 0x04002FFF RID: 12287
		internal static readonly int REP_3_6 = 16;

		// Token: 0x04003000 RID: 12288
		internal static readonly int REPZ_3_10 = 17;

		// Token: 0x04003001 RID: 12289
		internal static readonly int REPZ_11_138 = 18;
	}
}
