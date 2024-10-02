using System;

namespace BestHTTP.Examples
{
	// Token: 0x0200019B RID: 411
	internal sealed class DateTimeData
	{
		// Token: 0x06000EF4 RID: 3828 RVA: 0x000985EE File Offset: 0x000967EE
		public override string ToString()
		{
			return string.Format("[DateTimeData EventId: {0}, DateTime: {1}]", this.eventid, this.datetime);
		}

		// Token: 0x04001393 RID: 5011
		public int eventid;

		// Token: 0x04001394 RID: 5012
		public string datetime;
	}
}
