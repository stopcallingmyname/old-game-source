using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001C5 RID: 453
	public sealed class Error
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x0009EF84 File Offset: 0x0009D184
		// (set) Token: 0x0600109F RID: 4255 RVA: 0x0009EF8C File Offset: 0x0009D18C
		public SocketIOErrors Code { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0009EF95 File Offset: 0x0009D195
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x0009EF9D File Offset: 0x0009D19D
		public string Message { get; private set; }

		// Token: 0x060010A2 RID: 4258 RVA: 0x0009EFA6 File Offset: 0x0009D1A6
		public Error(SocketIOErrors code, string msg)
		{
			this.Code = code;
			this.Message = msg;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0009EFBC File Offset: 0x0009D1BC
		public override string ToString()
		{
			return string.Format("Code: {0} Message: \"{1}\"", this.Code.ToString(), this.Message);
		}
	}
}
