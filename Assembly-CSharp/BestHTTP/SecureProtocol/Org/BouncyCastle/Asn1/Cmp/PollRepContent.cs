using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C7 RID: 1991
	public class PollRepContent : Asn1Encodable
	{
		// Token: 0x060046EA RID: 18154 RVA: 0x00194868 File Offset: 0x00192A68
		private PollRepContent(Asn1Sequence seq)
		{
			this.certReqId = DerInteger.GetInstance(seq[0]);
			this.checkAfter = DerInteger.GetInstance(seq[1]);
			if (seq.Count > 2)
			{
				this.reason = PkiFreeText.GetInstance(seq[2]);
			}
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x001948BA File Offset: 0x00192ABA
		public static PollRepContent GetInstance(object obj)
		{
			if (obj is PollRepContent)
			{
				return (PollRepContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PollRepContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x001948F9 File Offset: 0x00192AF9
		public PollRepContent(DerInteger certReqId, DerInteger checkAfter)
		{
			this.certReqId = certReqId;
			this.checkAfter = checkAfter;
			this.reason = null;
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x00194916 File Offset: 0x00192B16
		public PollRepContent(DerInteger certReqId, DerInteger checkAfter, PkiFreeText reason)
		{
			this.certReqId = certReqId;
			this.checkAfter = checkAfter;
			this.reason = reason;
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060046EE RID: 18158 RVA: 0x00194933 File Offset: 0x00192B33
		public virtual DerInteger CertReqID
		{
			get
			{
				return this.certReqId;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060046EF RID: 18159 RVA: 0x0019493B File Offset: 0x00192B3B
		public virtual DerInteger CheckAfter
		{
			get
			{
				return this.checkAfter;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x00194943 File Offset: 0x00192B43
		public virtual PkiFreeText Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x0019494C File Offset: 0x00192B4C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certReqId,
				this.checkAfter
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.reason
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002E50 RID: 11856
		private readonly DerInteger certReqId;

		// Token: 0x04002E51 RID: 11857
		private readonly DerInteger checkAfter;

		// Token: 0x04002E52 RID: 11858
		private readonly PkiFreeText reason;
	}
}
