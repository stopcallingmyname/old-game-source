using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000768 RID: 1896
	public class CertRequest : Asn1Encodable
	{
		// Token: 0x0600441D RID: 17437 RVA: 0x0018CF48 File Offset: 0x0018B148
		private CertRequest(Asn1Sequence seq)
		{
			this.certReqId = DerInteger.GetInstance(seq[0]);
			this.certTemplate = CertTemplate.GetInstance(seq[1]);
			if (seq.Count > 2)
			{
				this.controls = Controls.GetInstance(seq[2]);
			}
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0018CF9A File Offset: 0x0018B19A
		public static CertRequest GetInstance(object obj)
		{
			if (obj is CertRequest)
			{
				return (CertRequest)obj;
			}
			if (obj != null)
			{
				return new CertRequest(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0018CFBB File Offset: 0x0018B1BB
		public CertRequest(int certReqId, CertTemplate certTemplate, Controls controls) : this(new DerInteger(certReqId), certTemplate, controls)
		{
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x0018CFCB File Offset: 0x0018B1CB
		public CertRequest(DerInteger certReqId, CertTemplate certTemplate, Controls controls)
		{
			this.certReqId = certReqId;
			this.certTemplate = certTemplate;
			this.controls = controls;
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06004421 RID: 17441 RVA: 0x0018CFE8 File Offset: 0x0018B1E8
		public virtual DerInteger CertReqID
		{
			get
			{
				return this.certReqId;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06004422 RID: 17442 RVA: 0x0018CFF0 File Offset: 0x0018B1F0
		public virtual CertTemplate CertTemplate
		{
			get
			{
				return this.certTemplate;
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06004423 RID: 17443 RVA: 0x0018CFF8 File Offset: 0x0018B1F8
		public virtual Controls Controls
		{
			get
			{
				return this.controls;
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0018D000 File Offset: 0x0018B200
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certReqId,
				this.certTemplate
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.controls
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CAF RID: 11439
		private readonly DerInteger certReqId;

		// Token: 0x04002CB0 RID: 11440
		private readonly CertTemplate certTemplate;

		// Token: 0x04002CB1 RID: 11441
		private readonly Controls controls;
	}
}
