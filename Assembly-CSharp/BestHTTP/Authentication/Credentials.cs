using System;

namespace BestHTTP.Authentication
{
	// Token: 0x0200081B RID: 2075
	public sealed class Credentials
	{
		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060049FA RID: 18938 RVA: 0x001A43F4 File Offset: 0x001A25F4
		// (set) Token: 0x060049FB RID: 18939 RVA: 0x001A43FC File Offset: 0x001A25FC
		public AuthenticationTypes Type { get; private set; }

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x001A4405 File Offset: 0x001A2605
		// (set) Token: 0x060049FD RID: 18941 RVA: 0x001A440D File Offset: 0x001A260D
		public string UserName { get; private set; }

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x001A4416 File Offset: 0x001A2616
		// (set) Token: 0x060049FF RID: 18943 RVA: 0x001A441E File Offset: 0x001A261E
		public string Password { get; private set; }

		// Token: 0x06004A00 RID: 18944 RVA: 0x001A4427 File Offset: 0x001A2627
		public Credentials(string userName, string password) : this(AuthenticationTypes.Unknown, userName, password)
		{
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x001A4432 File Offset: 0x001A2632
		public Credentials(AuthenticationTypes type, string userName, string password)
		{
			this.Type = type;
			this.UserName = userName;
			this.Password = password;
		}
	}
}
