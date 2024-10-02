using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000483 RID: 1155
	internal class TlsSessionImpl : TlsSession
	{
		// Token: 0x06002D26 RID: 11558 RVA: 0x0011B384 File Offset: 0x00119584
		internal TlsSessionImpl(byte[] sessionID, SessionParameters sessionParameters)
		{
			if (sessionID == null)
			{
				throw new ArgumentNullException("sessionID");
			}
			if (sessionID.Length > 32)
			{
				throw new ArgumentException("cannot be longer than 32 bytes", "sessionID");
			}
			this.mSessionID = Arrays.Clone(sessionID);
			this.mSessionParameters = sessionParameters;
			this.mResumable = (sessionID.Length != 0 && sessionParameters != null && sessionParameters.IsExtendedMasterSecret);
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x0011B3E8 File Offset: 0x001195E8
		public virtual SessionParameters ExportSessionParameters()
		{
			SessionParameters result;
			lock (this)
			{
				result = ((this.mSessionParameters == null) ? null : this.mSessionParameters.Copy());
			}
			return result;
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002D28 RID: 11560 RVA: 0x0011B438 File Offset: 0x00119638
		public virtual byte[] SessionID
		{
			get
			{
				byte[] result;
				lock (this)
				{
					result = this.mSessionID;
				}
				return result;
			}
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x0011B478 File Offset: 0x00119678
		public virtual void Invalidate()
		{
			lock (this)
			{
				this.mResumable = false;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06002D2A RID: 11562 RVA: 0x0011B4B4 File Offset: 0x001196B4
		public virtual bool IsResumable
		{
			get
			{
				bool result;
				lock (this)
				{
					result = this.mResumable;
				}
				return result;
			}
		}

		// Token: 0x04001E9B RID: 7835
		internal readonly byte[] mSessionID;

		// Token: 0x04001E9C RID: 7836
		internal readonly SessionParameters mSessionParameters;

		// Token: 0x04001E9D RID: 7837
		internal bool mResumable;
	}
}
