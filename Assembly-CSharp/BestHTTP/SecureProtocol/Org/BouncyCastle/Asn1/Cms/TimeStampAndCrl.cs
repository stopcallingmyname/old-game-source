using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A5 RID: 1957
	public class TimeStampAndCrl : Asn1Encodable
	{
		// Token: 0x060045F9 RID: 17913 RVA: 0x001920B4 File Offset: 0x001902B4
		public TimeStampAndCrl(ContentInfo timeStamp)
		{
			this.timeStamp = timeStamp;
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x001920C3 File Offset: 0x001902C3
		private TimeStampAndCrl(Asn1Sequence seq)
		{
			this.timeStamp = ContentInfo.GetInstance(seq[0]);
			if (seq.Count == 2)
			{
				this.crl = CertificateList.GetInstance(seq[1]);
			}
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x001920F8 File Offset: 0x001902F8
		public static TimeStampAndCrl GetInstance(object obj)
		{
			if (obj is TimeStampAndCrl)
			{
				return (TimeStampAndCrl)obj;
			}
			if (obj != null)
			{
				return new TimeStampAndCrl(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x00192119 File Offset: 0x00190319
		public virtual ContentInfo TimeStampToken
		{
			get
			{
				return this.timeStamp;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x00192121 File Offset: 0x00190321
		public virtual CertificateList Crl
		{
			get
			{
				return this.crl;
			}
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x0019212C File Offset: 0x0019032C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.timeStamp
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.crl
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D90 RID: 11664
		private ContentInfo timeStamp;

		// Token: 0x04002D91 RID: 11665
		private CertificateList crl;
	}
}
