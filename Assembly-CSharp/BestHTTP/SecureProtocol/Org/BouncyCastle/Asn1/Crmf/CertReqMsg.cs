using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000767 RID: 1895
	public class CertReqMsg : Asn1Encodable
	{
		// Token: 0x06004415 RID: 17429 RVA: 0x0018CDCC File Offset: 0x0018AFCC
		private CertReqMsg(Asn1Sequence seq)
		{
			this.certReq = CertRequest.GetInstance(seq[0]);
			for (int i = 1; i < seq.Count; i++)
			{
				object obj = seq[i];
				if (obj is Asn1TaggedObject || obj is ProofOfPossession)
				{
					this.popo = ProofOfPossession.GetInstance(obj);
				}
				else
				{
					this.regInfo = Asn1Sequence.GetInstance(obj);
				}
			}
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0018CE34 File Offset: 0x0018B034
		public static CertReqMsg GetInstance(object obj)
		{
			if (obj is CertReqMsg)
			{
				return (CertReqMsg)obj;
			}
			if (obj != null)
			{
				return new CertReqMsg(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x0018CE55 File Offset: 0x0018B055
		public static CertReqMsg GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return CertReqMsg.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0018CE64 File Offset: 0x0018B064
		public CertReqMsg(CertRequest certReq, ProofOfPossession popo, AttributeTypeAndValue[] regInfo)
		{
			if (certReq == null)
			{
				throw new ArgumentNullException("certReq");
			}
			this.certReq = certReq;
			this.popo = popo;
			if (regInfo != null)
			{
				this.regInfo = new DerSequence(regInfo);
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06004419 RID: 17433 RVA: 0x0018CEA4 File Offset: 0x0018B0A4
		public virtual CertRequest CertReq
		{
			get
			{
				return this.certReq;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600441A RID: 17434 RVA: 0x0018CEAC File Offset: 0x0018B0AC
		public virtual ProofOfPossession Popo
		{
			get
			{
				return this.popo;
			}
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x0018CEB4 File Offset: 0x0018B0B4
		public virtual AttributeTypeAndValue[] GetRegInfo()
		{
			if (this.regInfo == null)
			{
				return null;
			}
			AttributeTypeAndValue[] array = new AttributeTypeAndValue[this.regInfo.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = AttributeTypeAndValue.GetInstance(this.regInfo[num]);
			}
			return array;
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x0018CF00 File Offset: 0x0018B100
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certReq
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.popo,
				this.regInfo
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CAC RID: 11436
		private readonly CertRequest certReq;

		// Token: 0x04002CAD RID: 11437
		private readonly ProofOfPossession popo;

		// Token: 0x04002CAE RID: 11438
		private readonly Asn1Sequence regInfo;
	}
}
