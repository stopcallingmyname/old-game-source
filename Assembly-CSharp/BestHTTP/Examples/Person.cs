using System;

namespace BestHTTP.Examples
{
	// Token: 0x020001AA RID: 426
	internal sealed class Person
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0009B56B File Offset: 0x0009976B
		// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x0009B573 File Offset: 0x00099773
		public string Name { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0009B57C File Offset: 0x0009977C
		// (set) Token: 0x06000FB3 RID: 4019 RVA: 0x0009B584 File Offset: 0x00099784
		public long Age { get; set; }

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0009B590 File Offset: 0x00099790
		public override string ToString()
		{
			return string.Format("[Person Name: '{0}', Age: {1}]", this.Name, this.Age.ToString());
		}
	}
}
