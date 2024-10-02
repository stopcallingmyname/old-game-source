using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200046D RID: 1133
	public class TlsFatalAlert : TlsException
	{
		// Token: 0x06002C59 RID: 11353 RVA: 0x00118367 File Offset: 0x00116567
		public TlsFatalAlert(byte alertDescription) : this(alertDescription, null)
		{
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x00118371 File Offset: 0x00116571
		public TlsFatalAlert(byte alertDescription, Exception alertCause) : base(BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls.AlertDescription.GetText(alertDescription), alertCause)
		{
			this.alertDescription = alertDescription;
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x00118387 File Offset: 0x00116587
		public virtual byte AlertDescription
		{
			get
			{
				return this.alertDescription;
			}
		}

		// Token: 0x04001E43 RID: 7747
		private readonly byte alertDescription;
	}
}
