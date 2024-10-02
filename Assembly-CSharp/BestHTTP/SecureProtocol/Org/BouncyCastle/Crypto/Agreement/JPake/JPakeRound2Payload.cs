using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.JPake
{
	// Token: 0x020005DB RID: 1499
	public class JPakeRound2Payload
	{
		// Token: 0x0600394F RID: 14671 RVA: 0x001669A8 File Offset: 0x00164BA8
		public JPakeRound2Payload(string participantId, BigInteger a, BigInteger[] knowledgeProofForX2s)
		{
			JPakeUtilities.ValidateNotNull(participantId, "participantId");
			JPakeUtilities.ValidateNotNull(a, "a");
			JPakeUtilities.ValidateNotNull(knowledgeProofForX2s, "knowledgeProofForX2s");
			this.participantId = participantId;
			this.a = a;
			this.knowledgeProofForX2s = new BigInteger[knowledgeProofForX2s.Length];
			knowledgeProofForX2s.CopyTo(this.knowledgeProofForX2s, 0);
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x00166A05 File Offset: 0x00164C05
		public virtual string ParticipantId
		{
			get
			{
				return this.participantId;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x00166A0D File Offset: 0x00164C0D
		public virtual BigInteger A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x00166A18 File Offset: 0x00164C18
		public virtual BigInteger[] KnowledgeProofForX2s
		{
			get
			{
				BigInteger[] array = new BigInteger[this.knowledgeProofForX2s.Length];
				Array.Copy(this.knowledgeProofForX2s, array, this.knowledgeProofForX2s.Length);
				return array;
			}
		}

		// Token: 0x040025AB RID: 9643
		private readonly string participantId;

		// Token: 0x040025AC RID: 9644
		private readonly BigInteger a;

		// Token: 0x040025AD RID: 9645
		private readonly BigInteger[] knowledgeProofForX2s;
	}
}
