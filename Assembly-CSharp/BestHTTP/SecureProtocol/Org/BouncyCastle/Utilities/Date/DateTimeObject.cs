using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date
{
	// Token: 0x02000293 RID: 659
	public sealed class DateTimeObject
	{
		// Token: 0x060017F9 RID: 6137 RVA: 0x000B9736 File Offset: 0x000B7936
		public DateTimeObject(DateTime dt)
		{
			this.dt = dt;
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x000B9745 File Offset: 0x000B7945
		public DateTime Value
		{
			get
			{
				return this.dt;
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x000B9750 File Offset: 0x000B7950
		public override string ToString()
		{
			return this.dt.ToString();
		}

		// Token: 0x04001828 RID: 6184
		private readonly DateTime dt;
	}
}
