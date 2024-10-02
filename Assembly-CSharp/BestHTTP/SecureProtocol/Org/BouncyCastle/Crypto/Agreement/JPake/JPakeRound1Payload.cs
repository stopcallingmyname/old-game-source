using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.JPake
{
	// Token: 0x020005DA RID: 1498
	public class JPakeRound1Payload
	{
		// Token: 0x06003949 RID: 14665 RVA: 0x0016688C File Offset: 0x00164A8C
		public JPakeRound1Payload(string participantId, BigInteger gx1, BigInteger gx2, BigInteger[] knowledgeProofForX1, BigInteger[] knowledgeProofForX2)
		{
			JPakeUtilities.ValidateNotNull(participantId, "participantId");
			JPakeUtilities.ValidateNotNull(gx1, "gx1");
			JPakeUtilities.ValidateNotNull(gx2, "gx2");
			JPakeUtilities.ValidateNotNull(knowledgeProofForX1, "knowledgeProofForX1");
			JPakeUtilities.ValidateNotNull(knowledgeProofForX2, "knowledgeProofForX2");
			this.participantId = participantId;
			this.gx1 = gx1;
			this.gx2 = gx2;
			this.knowledgeProofForX1 = new BigInteger[knowledgeProofForX1.Length];
			Array.Copy(knowledgeProofForX1, this.knowledgeProofForX1, knowledgeProofForX1.Length);
			this.knowledgeProofForX2 = new BigInteger[knowledgeProofForX2.Length];
			Array.Copy(knowledgeProofForX2, this.knowledgeProofForX2, knowledgeProofForX2.Length);
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x0016692D File Offset: 0x00164B2D
		public virtual string ParticipantId
		{
			get
			{
				return this.participantId;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x00166935 File Offset: 0x00164B35
		public virtual BigInteger Gx1
		{
			get
			{
				return this.gx1;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x0016693D File Offset: 0x00164B3D
		public virtual BigInteger Gx2
		{
			get
			{
				return this.gx2;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x00166948 File Offset: 0x00164B48
		public virtual BigInteger[] KnowledgeProofForX1
		{
			get
			{
				BigInteger[] array = new BigInteger[this.knowledgeProofForX1.Length];
				Array.Copy(this.knowledgeProofForX1, array, this.knowledgeProofForX1.Length);
				return array;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x00166978 File Offset: 0x00164B78
		public virtual BigInteger[] KnowledgeProofForX2
		{
			get
			{
				BigInteger[] array = new BigInteger[this.knowledgeProofForX2.Length];
				Array.Copy(this.knowledgeProofForX2, array, this.knowledgeProofForX2.Length);
				return array;
			}
		}

		// Token: 0x040025A6 RID: 9638
		private readonly string participantId;

		// Token: 0x040025A7 RID: 9639
		private readonly BigInteger gx1;

		// Token: 0x040025A8 RID: 9640
		private readonly BigInteger gx2;

		// Token: 0x040025A9 RID: 9641
		private readonly BigInteger[] knowledgeProofForX1;

		// Token: 0x040025AA RID: 9642
		private readonly BigInteger[] knowledgeProofForX2;
	}
}
