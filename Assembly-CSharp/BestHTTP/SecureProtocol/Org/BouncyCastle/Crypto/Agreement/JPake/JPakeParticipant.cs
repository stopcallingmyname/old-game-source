using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.JPake
{
	// Token: 0x020005D7 RID: 1495
	public class JPakeParticipant
	{
		// Token: 0x06003936 RID: 14646 RVA: 0x00165FBC File Offset: 0x001641BC
		public JPakeParticipant(string participantId, char[] password) : this(participantId, password, JPakePrimeOrderGroups.NIST_3072)
		{
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x00165FCB File Offset: 0x001641CB
		public JPakeParticipant(string participantId, char[] password, JPakePrimeOrderGroup group) : this(participantId, password, group, new Sha256Digest(), new SecureRandom())
		{
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x00165FE0 File Offset: 0x001641E0
		public JPakeParticipant(string participantId, char[] password, JPakePrimeOrderGroup group, IDigest digest, SecureRandom random)
		{
			JPakeUtilities.ValidateNotNull(participantId, "participantId");
			JPakeUtilities.ValidateNotNull(password, "password");
			JPakeUtilities.ValidateNotNull(group, "p");
			JPakeUtilities.ValidateNotNull(digest, "digest");
			JPakeUtilities.ValidateNotNull(random, "random");
			if (password.Length == 0)
			{
				throw new ArgumentException("Password must not be empty.");
			}
			this.participantId = participantId;
			this.password = new char[password.Length];
			Array.Copy(password, this.password, password.Length);
			this.p = group.P;
			this.q = group.Q;
			this.g = group.G;
			this.digest = digest;
			this.random = random;
			this.state = JPakeParticipant.STATE_INITIALIZED;
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06003939 RID: 14649 RVA: 0x0016609E File Offset: 0x0016429E
		public virtual int State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x001660A8 File Offset: 0x001642A8
		public virtual JPakeRound1Payload CreateRound1PayloadToSend()
		{
			if (this.state >= JPakeParticipant.STATE_ROUND_1_CREATED)
			{
				throw new InvalidOperationException("Round 1 payload already created for " + this.participantId);
			}
			this.x1 = JPakeUtilities.GenerateX1(this.q, this.random);
			this.x2 = JPakeUtilities.GenerateX2(this.q, this.random);
			this.gx1 = JPakeUtilities.CalculateGx(this.p, this.g, this.x1);
			this.gx2 = JPakeUtilities.CalculateGx(this.p, this.g, this.x2);
			BigInteger[] knowledgeProofForX = JPakeUtilities.CalculateZeroKnowledgeProof(this.p, this.q, this.g, this.gx1, this.x1, this.participantId, this.digest, this.random);
			BigInteger[] knowledgeProofForX2 = JPakeUtilities.CalculateZeroKnowledgeProof(this.p, this.q, this.g, this.gx2, this.x2, this.participantId, this.digest, this.random);
			this.state = JPakeParticipant.STATE_ROUND_1_CREATED;
			return new JPakeRound1Payload(this.participantId, this.gx1, this.gx2, knowledgeProofForX, knowledgeProofForX2);
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x001661D0 File Offset: 0x001643D0
		public virtual void ValidateRound1PayloadReceived(JPakeRound1Payload round1PayloadReceived)
		{
			if (this.state >= JPakeParticipant.STATE_ROUND_1_VALIDATED)
			{
				throw new InvalidOperationException("Validation already attempted for round 1 payload for " + this.participantId);
			}
			this.partnerParticipantId = round1PayloadReceived.ParticipantId;
			this.gx3 = round1PayloadReceived.Gx1;
			this.gx4 = round1PayloadReceived.Gx2;
			BigInteger[] knowledgeProofForX = round1PayloadReceived.KnowledgeProofForX1;
			BigInteger[] knowledgeProofForX2 = round1PayloadReceived.KnowledgeProofForX2;
			JPakeUtilities.ValidateParticipantIdsDiffer(this.participantId, round1PayloadReceived.ParticipantId);
			JPakeUtilities.ValidateGx4(this.gx4);
			JPakeUtilities.ValidateZeroKnowledgeProof(this.p, this.q, this.g, this.gx3, knowledgeProofForX, round1PayloadReceived.ParticipantId, this.digest);
			JPakeUtilities.ValidateZeroKnowledgeProof(this.p, this.q, this.g, this.gx4, knowledgeProofForX2, round1PayloadReceived.ParticipantId, this.digest);
			this.state = JPakeParticipant.STATE_ROUND_1_VALIDATED;
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x001662B0 File Offset: 0x001644B0
		public virtual JPakeRound2Payload CreateRound2PayloadToSend()
		{
			if (this.state >= JPakeParticipant.STATE_ROUND_2_CREATED)
			{
				throw new InvalidOperationException("Round 2 payload already created for " + this.participantId);
			}
			if (this.state < JPakeParticipant.STATE_ROUND_1_VALIDATED)
			{
				throw new InvalidOperationException("Round 1 payload must be validated prior to creating round 2 payload for " + this.participantId);
			}
			BigInteger gA = JPakeUtilities.CalculateGA(this.p, this.gx1, this.gx3, this.gx4);
			BigInteger s = JPakeUtilities.CalculateS(this.password);
			BigInteger bigInteger = JPakeUtilities.CalculateX2s(this.q, this.x2, s);
			BigInteger bigInteger2 = JPakeUtilities.CalculateA(this.p, this.q, gA, bigInteger);
			BigInteger[] knowledgeProofForX2s = JPakeUtilities.CalculateZeroKnowledgeProof(this.p, this.q, gA, bigInteger2, bigInteger, this.participantId, this.digest, this.random);
			this.state = JPakeParticipant.STATE_ROUND_2_CREATED;
			return new JPakeRound2Payload(this.participantId, bigInteger2, knowledgeProofForX2s);
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x00166398 File Offset: 0x00164598
		public virtual void ValidateRound2PayloadReceived(JPakeRound2Payload round2PayloadReceived)
		{
			if (this.state >= JPakeParticipant.STATE_ROUND_2_VALIDATED)
			{
				throw new InvalidOperationException("Validation already attempted for round 2 payload for " + this.participantId);
			}
			if (this.state < JPakeParticipant.STATE_ROUND_1_VALIDATED)
			{
				throw new InvalidOperationException("Round 1 payload must be validated prior to validation round 2 payload for " + this.participantId);
			}
			BigInteger ga = JPakeUtilities.CalculateGA(this.p, this.gx3, this.gx1, this.gx2);
			this.b = round2PayloadReceived.A;
			BigInteger[] knowledgeProofForX2s = round2PayloadReceived.KnowledgeProofForX2s;
			JPakeUtilities.ValidateParticipantIdsDiffer(this.participantId, round2PayloadReceived.ParticipantId);
			JPakeUtilities.ValidateParticipantIdsEqual(this.partnerParticipantId, round2PayloadReceived.ParticipantId);
			JPakeUtilities.ValidateGa(ga);
			JPakeUtilities.ValidateZeroKnowledgeProof(this.p, this.q, ga, this.b, knowledgeProofForX2s, round2PayloadReceived.ParticipantId, this.digest);
			this.state = JPakeParticipant.STATE_ROUND_2_VALIDATED;
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x00166474 File Offset: 0x00164674
		public virtual BigInteger CalculateKeyingMaterial()
		{
			if (this.state >= JPakeParticipant.STATE_KEY_CALCULATED)
			{
				throw new InvalidOperationException("Key already calculated for " + this.participantId);
			}
			if (this.state < JPakeParticipant.STATE_ROUND_2_VALIDATED)
			{
				throw new InvalidOperationException("Round 2 payload must be validated prior to creating key for " + this.participantId);
			}
			BigInteger s = JPakeUtilities.CalculateS(this.password);
			Array.Clear(this.password, 0, this.password.Length);
			this.password = null;
			BigInteger result = JPakeUtilities.CalculateKeyingMaterial(this.p, this.q, this.gx4, this.x2, s, this.b);
			this.x1 = null;
			this.x2 = null;
			this.b = null;
			this.state = JPakeParticipant.STATE_KEY_CALCULATED;
			return result;
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x00166534 File Offset: 0x00164734
		public virtual JPakeRound3Payload CreateRound3PayloadToSend(BigInteger keyingMaterial)
		{
			if (this.state >= JPakeParticipant.STATE_ROUND_3_CREATED)
			{
				throw new InvalidOperationException("Round 3 payload already created for " + this.participantId);
			}
			if (this.state < JPakeParticipant.STATE_KEY_CALCULATED)
			{
				throw new InvalidOperationException("Keying material must be calculated prior to creating round 3 payload for " + this.participantId);
			}
			BigInteger magTag = JPakeUtilities.CalculateMacTag(this.participantId, this.partnerParticipantId, this.gx1, this.gx2, this.gx3, this.gx4, keyingMaterial, this.digest);
			this.state = JPakeParticipant.STATE_ROUND_3_CREATED;
			return new JPakeRound3Payload(this.participantId, magTag);
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x001665D0 File Offset: 0x001647D0
		public virtual void ValidateRound3PayloadReceived(JPakeRound3Payload round3PayloadReceived, BigInteger keyingMaterial)
		{
			if (this.state >= JPakeParticipant.STATE_ROUND_3_VALIDATED)
			{
				throw new InvalidOperationException("Validation already attempted for round 3 payload for " + this.participantId);
			}
			if (this.state < JPakeParticipant.STATE_KEY_CALCULATED)
			{
				throw new InvalidOperationException("Keying material must be calculated prior to validating round 3 payload for " + this.participantId);
			}
			JPakeUtilities.ValidateParticipantIdsDiffer(this.participantId, round3PayloadReceived.ParticipantId);
			JPakeUtilities.ValidateParticipantIdsEqual(this.partnerParticipantId, round3PayloadReceived.ParticipantId);
			JPakeUtilities.ValidateMacTag(this.participantId, this.partnerParticipantId, this.gx1, this.gx2, this.gx3, this.gx4, keyingMaterial, this.digest, round3PayloadReceived.MacTag);
			this.gx1 = null;
			this.gx2 = null;
			this.gx3 = null;
			this.gx4 = null;
			this.state = JPakeParticipant.STATE_ROUND_3_VALIDATED;
		}

		// Token: 0x04002588 RID: 9608
		public static readonly int STATE_INITIALIZED = 0;

		// Token: 0x04002589 RID: 9609
		public static readonly int STATE_ROUND_1_CREATED = 10;

		// Token: 0x0400258A RID: 9610
		public static readonly int STATE_ROUND_1_VALIDATED = 20;

		// Token: 0x0400258B RID: 9611
		public static readonly int STATE_ROUND_2_CREATED = 30;

		// Token: 0x0400258C RID: 9612
		public static readonly int STATE_ROUND_2_VALIDATED = 40;

		// Token: 0x0400258D RID: 9613
		public static readonly int STATE_KEY_CALCULATED = 50;

		// Token: 0x0400258E RID: 9614
		public static readonly int STATE_ROUND_3_CREATED = 60;

		// Token: 0x0400258F RID: 9615
		public static readonly int STATE_ROUND_3_VALIDATED = 70;

		// Token: 0x04002590 RID: 9616
		private string participantId;

		// Token: 0x04002591 RID: 9617
		private char[] password;

		// Token: 0x04002592 RID: 9618
		private IDigest digest;

		// Token: 0x04002593 RID: 9619
		private readonly SecureRandom random;

		// Token: 0x04002594 RID: 9620
		private readonly BigInteger p;

		// Token: 0x04002595 RID: 9621
		private readonly BigInteger q;

		// Token: 0x04002596 RID: 9622
		private readonly BigInteger g;

		// Token: 0x04002597 RID: 9623
		private string partnerParticipantId;

		// Token: 0x04002598 RID: 9624
		private BigInteger x1;

		// Token: 0x04002599 RID: 9625
		private BigInteger x2;

		// Token: 0x0400259A RID: 9626
		private BigInteger gx1;

		// Token: 0x0400259B RID: 9627
		private BigInteger gx2;

		// Token: 0x0400259C RID: 9628
		private BigInteger gx3;

		// Token: 0x0400259D RID: 9629
		private BigInteger gx4;

		// Token: 0x0400259E RID: 9630
		private BigInteger b;

		// Token: 0x0400259F RID: 9631
		private int state;
	}
}
