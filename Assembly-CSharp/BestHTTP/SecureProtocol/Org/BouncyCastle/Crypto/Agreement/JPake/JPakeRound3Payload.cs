using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.JPake
{
	// Token: 0x020005DC RID: 1500
	public class JPakeRound3Payload
	{
		// Token: 0x06003953 RID: 14675 RVA: 0x00166A48 File Offset: 0x00164C48
		public JPakeRound3Payload(string participantId, BigInteger magTag)
		{
			this.participantId = participantId;
			this.macTag = magTag;
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06003954 RID: 14676 RVA: 0x00166A5E File Offset: 0x00164C5E
		public virtual string ParticipantId
		{
			get
			{
				return this.participantId;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x00166A66 File Offset: 0x00164C66
		public virtual BigInteger MacTag
		{
			get
			{
				return this.macTag;
			}
		}

		// Token: 0x040025AE RID: 9646
		private readonly string participantId;

		// Token: 0x040025AF RID: 9647
		private readonly BigInteger macTag;
	}
}
