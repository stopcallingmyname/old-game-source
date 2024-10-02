using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200046E RID: 1134
	public class TlsFatalAlertReceived : TlsException
	{
		// Token: 0x06002C5C RID: 11356 RVA: 0x0011838F File Offset: 0x0011658F
		public TlsFatalAlertReceived(byte alertDescription) : base(BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls.AlertDescription.GetText(alertDescription), null)
		{
			this.alertDescription = alertDescription;
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06002C5D RID: 11357 RVA: 0x001183A5 File Offset: 0x001165A5
		public virtual byte AlertDescription
		{
			get
			{
				return this.alertDescription;
			}
		}

		// Token: 0x04001E44 RID: 7748
		private readonly byte alertDescription;
	}
}
