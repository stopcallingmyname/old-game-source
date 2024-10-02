using System;

namespace LitJson
{
	// Token: 0x0200015E RID: 350
	public enum JsonToken
	{
		// Token: 0x04001221 RID: 4641
		None,
		// Token: 0x04001222 RID: 4642
		ObjectStart,
		// Token: 0x04001223 RID: 4643
		PropertyName,
		// Token: 0x04001224 RID: 4644
		ObjectEnd,
		// Token: 0x04001225 RID: 4645
		ArrayStart,
		// Token: 0x04001226 RID: 4646
		ArrayEnd,
		// Token: 0x04001227 RID: 4647
		Int,
		// Token: 0x04001228 RID: 4648
		Long,
		// Token: 0x04001229 RID: 4649
		Double,
		// Token: 0x0400122A RID: 4650
		String,
		// Token: 0x0400122B RID: 4651
		Boolean,
		// Token: 0x0400122C RID: 4652
		Null
	}
}
