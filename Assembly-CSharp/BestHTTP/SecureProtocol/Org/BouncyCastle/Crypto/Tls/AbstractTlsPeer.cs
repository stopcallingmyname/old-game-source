using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F7 RID: 1015
	public abstract class AbstractTlsPeer : TlsPeer
	{
		// Token: 0x06002911 RID: 10513 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool RequiresExtendedMasterSecret()
		{
			return false;
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool ShouldUseGmtUnixTime()
		{
			return false;
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x0010D8A8 File Offset: 0x0010BAA8
		public virtual void NotifySecureRenegotiation(bool secureRenegotiation)
		{
			if (!secureRenegotiation)
			{
				throw new TlsFatalAlert(40);
			}
		}

		// Token: 0x06002914 RID: 10516
		public abstract TlsCompression GetCompression();

		// Token: 0x06002915 RID: 10517
		public abstract TlsCipher GetCipher();

		// Token: 0x06002916 RID: 10518 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void NotifyAlertRaised(byte alertLevel, byte alertDescription, string message, Exception cause)
		{
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void NotifyAlertReceived(byte alertLevel, byte alertDescription)
		{
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void NotifyHandshakeComplete()
		{
		}
	}
}
