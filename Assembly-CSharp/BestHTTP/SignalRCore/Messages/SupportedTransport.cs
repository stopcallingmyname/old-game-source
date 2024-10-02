using System;
using System.Collections.Generic;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001FC RID: 508
	public sealed class SupportedTransport
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x000A4F72 File Offset: 0x000A3172
		// (set) Token: 0x06001283 RID: 4739 RVA: 0x000A4F7A File Offset: 0x000A317A
		public string Name { get; private set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x000A4F83 File Offset: 0x000A3183
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x000A4F8B File Offset: 0x000A318B
		public List<string> SupportedFormats { get; private set; }

		// Token: 0x06001286 RID: 4742 RVA: 0x000A4F94 File Offset: 0x000A3194
		internal SupportedTransport(string transportName, List<string> transferFormats)
		{
			this.Name = transportName;
			this.SupportedFormats = transferFormats;
		}
	}
}
