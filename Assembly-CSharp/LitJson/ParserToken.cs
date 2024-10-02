using System;

namespace LitJson
{
	// Token: 0x02000165 RID: 357
	internal enum ParserToken
	{
		// Token: 0x04001265 RID: 4709
		None = 65536,
		// Token: 0x04001266 RID: 4710
		Number,
		// Token: 0x04001267 RID: 4711
		True,
		// Token: 0x04001268 RID: 4712
		False,
		// Token: 0x04001269 RID: 4713
		Null,
		// Token: 0x0400126A RID: 4714
		CharSeq,
		// Token: 0x0400126B RID: 4715
		Char,
		// Token: 0x0400126C RID: 4716
		Text,
		// Token: 0x0400126D RID: 4717
		Object,
		// Token: 0x0400126E RID: 4718
		ObjectPrime,
		// Token: 0x0400126F RID: 4719
		Pair,
		// Token: 0x04001270 RID: 4720
		PairRest,
		// Token: 0x04001271 RID: 4721
		Array,
		// Token: 0x04001272 RID: 4722
		ArrayPrime,
		// Token: 0x04001273 RID: 4723
		Value,
		// Token: 0x04001274 RID: 4724
		ValueRest,
		// Token: 0x04001275 RID: 4725
		String,
		// Token: 0x04001276 RID: 4726
		End,
		// Token: 0x04001277 RID: 4727
		Epsilon
	}
}
