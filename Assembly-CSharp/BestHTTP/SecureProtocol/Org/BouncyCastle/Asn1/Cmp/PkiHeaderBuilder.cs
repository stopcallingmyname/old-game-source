using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C1 RID: 1985
	public class PkiHeaderBuilder
	{
		// Token: 0x060046B4 RID: 18100 RVA: 0x0019413F File Offset: 0x0019233F
		public PkiHeaderBuilder(int pvno, GeneralName sender, GeneralName recipient) : this(new DerInteger(pvno), sender, recipient)
		{
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x0019414F File Offset: 0x0019234F
		private PkiHeaderBuilder(DerInteger pvno, GeneralName sender, GeneralName recipient)
		{
			this.pvno = pvno;
			this.sender = sender;
			this.recipient = recipient;
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0019416C File Offset: 0x0019236C
		public virtual PkiHeaderBuilder SetMessageTime(DerGeneralizedTime time)
		{
			this.messageTime = time;
			return this;
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x00194176 File Offset: 0x00192376
		public virtual PkiHeaderBuilder SetProtectionAlg(AlgorithmIdentifier aid)
		{
			this.protectionAlg = aid;
			return this;
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x00194180 File Offset: 0x00192380
		public virtual PkiHeaderBuilder SetSenderKID(byte[] kid)
		{
			return this.SetSenderKID((kid == null) ? null : new DerOctetString(kid));
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x00194194 File Offset: 0x00192394
		public virtual PkiHeaderBuilder SetSenderKID(Asn1OctetString kid)
		{
			this.senderKID = kid;
			return this;
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x0019419E File Offset: 0x0019239E
		public virtual PkiHeaderBuilder SetRecipKID(byte[] kid)
		{
			return this.SetRecipKID((kid == null) ? null : new DerOctetString(kid));
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x001941B2 File Offset: 0x001923B2
		public virtual PkiHeaderBuilder SetRecipKID(DerOctetString kid)
		{
			this.recipKID = kid;
			return this;
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x001941BC File Offset: 0x001923BC
		public virtual PkiHeaderBuilder SetTransactionID(byte[] tid)
		{
			return this.SetTransactionID((tid == null) ? null : new DerOctetString(tid));
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x001941D0 File Offset: 0x001923D0
		public virtual PkiHeaderBuilder SetTransactionID(Asn1OctetString tid)
		{
			this.transactionID = tid;
			return this;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x001941DA File Offset: 0x001923DA
		public virtual PkiHeaderBuilder SetSenderNonce(byte[] nonce)
		{
			return this.SetSenderNonce((nonce == null) ? null : new DerOctetString(nonce));
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x001941EE File Offset: 0x001923EE
		public virtual PkiHeaderBuilder SetSenderNonce(Asn1OctetString nonce)
		{
			this.senderNonce = nonce;
			return this;
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x001941F8 File Offset: 0x001923F8
		public virtual PkiHeaderBuilder SetRecipNonce(byte[] nonce)
		{
			return this.SetRecipNonce((nonce == null) ? null : new DerOctetString(nonce));
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x0019420C File Offset: 0x0019240C
		public virtual PkiHeaderBuilder SetRecipNonce(Asn1OctetString nonce)
		{
			this.recipNonce = nonce;
			return this;
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x00194216 File Offset: 0x00192416
		public virtual PkiHeaderBuilder SetFreeText(PkiFreeText text)
		{
			this.freeText = text;
			return this;
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x00194220 File Offset: 0x00192420
		public virtual PkiHeaderBuilder SetGeneralInfo(InfoTypeAndValue genInfo)
		{
			return this.SetGeneralInfo(PkiHeaderBuilder.MakeGeneralInfoSeq(genInfo));
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x0019422E File Offset: 0x0019242E
		public virtual PkiHeaderBuilder SetGeneralInfo(InfoTypeAndValue[] genInfos)
		{
			return this.SetGeneralInfo(PkiHeaderBuilder.MakeGeneralInfoSeq(genInfos));
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x0019423C File Offset: 0x0019243C
		public virtual PkiHeaderBuilder SetGeneralInfo(Asn1Sequence seqOfInfoTypeAndValue)
		{
			this.generalInfo = seqOfInfoTypeAndValue;
			return this;
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x00194246 File Offset: 0x00192446
		private static Asn1Sequence MakeGeneralInfoSeq(InfoTypeAndValue generalInfo)
		{
			return new DerSequence(generalInfo);
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x00194250 File Offset: 0x00192450
		private static Asn1Sequence MakeGeneralInfoSeq(InfoTypeAndValue[] generalInfos)
		{
			Asn1Sequence result = null;
			if (generalInfos != null)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				for (int i = 0; i < generalInfos.Length; i++)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						generalInfos[i]
					});
				}
				result = new DerSequence(asn1EncodableVector);
			}
			return result;
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x00194298 File Offset: 0x00192498
		public virtual PkiHeader Build()
		{
			Asn1EncodableVector v = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.pvno,
				this.sender,
				this.recipient
			});
			this.AddOptional(v, 0, this.messageTime);
			this.AddOptional(v, 1, this.protectionAlg);
			this.AddOptional(v, 2, this.senderKID);
			this.AddOptional(v, 3, this.recipKID);
			this.AddOptional(v, 4, this.transactionID);
			this.AddOptional(v, 5, this.senderNonce);
			this.AddOptional(v, 6, this.recipNonce);
			this.AddOptional(v, 7, this.freeText);
			this.AddOptional(v, 8, this.generalInfo);
			this.messageTime = null;
			this.protectionAlg = null;
			this.senderKID = null;
			this.recipKID = null;
			this.transactionID = null;
			this.senderNonce = null;
			this.recipNonce = null;
			this.freeText = null;
			this.generalInfo = null;
			return PkiHeader.GetInstance(new DerSequence(v));
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x0019382E File Offset: 0x00191A2E
		private void AddOptional(Asn1EncodableVector v, int tagNo, Asn1Encodable obj)
		{
			if (obj != null)
			{
				v.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, tagNo, obj)
				});
			}
		}

		// Token: 0x04002E2C RID: 11820
		private DerInteger pvno;

		// Token: 0x04002E2D RID: 11821
		private GeneralName sender;

		// Token: 0x04002E2E RID: 11822
		private GeneralName recipient;

		// Token: 0x04002E2F RID: 11823
		private DerGeneralizedTime messageTime;

		// Token: 0x04002E30 RID: 11824
		private AlgorithmIdentifier protectionAlg;

		// Token: 0x04002E31 RID: 11825
		private Asn1OctetString senderKID;

		// Token: 0x04002E32 RID: 11826
		private Asn1OctetString recipKID;

		// Token: 0x04002E33 RID: 11827
		private Asn1OctetString transactionID;

		// Token: 0x04002E34 RID: 11828
		private Asn1OctetString senderNonce;

		// Token: 0x04002E35 RID: 11829
		private Asn1OctetString recipNonce;

		// Token: 0x04002E36 RID: 11830
		private PkiFreeText freeText;

		// Token: 0x04002E37 RID: 11831
		private Asn1Sequence generalInfo;
	}
}
