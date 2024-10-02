using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002AF RID: 687
	public class CertStatus
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x000BB988 File Offset: 0x000B9B88
		// (set) Token: 0x06001902 RID: 6402 RVA: 0x000BB990 File Offset: 0x000B9B90
		public DateTimeObject RevocationDate
		{
			get
			{
				return this.revocationDate;
			}
			set
			{
				this.revocationDate = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x000BB999 File Offset: 0x000B9B99
		// (set) Token: 0x06001904 RID: 6404 RVA: 0x000BB9A1 File Offset: 0x000B9BA1
		public int Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}

		// Token: 0x0400186B RID: 6251
		public const int Unrevoked = 11;

		// Token: 0x0400186C RID: 6252
		public const int Undetermined = 12;

		// Token: 0x0400186D RID: 6253
		private int status = 11;

		// Token: 0x0400186E RID: 6254
		private DateTimeObject revocationDate;
	}
}
