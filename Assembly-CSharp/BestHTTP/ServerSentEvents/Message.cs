using System;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x02000237 RID: 567
	public sealed class Message
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x000AA6D8 File Offset: 0x000A88D8
		// (set) Token: 0x0600146A RID: 5226 RVA: 0x000AA6E0 File Offset: 0x000A88E0
		public string Id { get; internal set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x000AA6E9 File Offset: 0x000A88E9
		// (set) Token: 0x0600146C RID: 5228 RVA: 0x000AA6F1 File Offset: 0x000A88F1
		public string Event { get; internal set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x000AA6FA File Offset: 0x000A88FA
		// (set) Token: 0x0600146E RID: 5230 RVA: 0x000AA702 File Offset: 0x000A8902
		public string Data { get; internal set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x000AA70B File Offset: 0x000A890B
		// (set) Token: 0x06001470 RID: 5232 RVA: 0x000AA713 File Offset: 0x000A8913
		public TimeSpan Retry { get; internal set; }

		// Token: 0x06001471 RID: 5233 RVA: 0x000AA71C File Offset: 0x000A891C
		public override string ToString()
		{
			return string.Format("\"{0}\": \"{1}\"", this.Event, this.Data);
		}
	}
}
