using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A8 RID: 1960
	public class TimeStampTokenEvidence : Asn1Encodable
	{
		// Token: 0x0600460D RID: 17933 RVA: 0x001923F7 File Offset: 0x001905F7
		public TimeStampTokenEvidence(TimeStampAndCrl[] timeStampAndCrls)
		{
			this.timeStampAndCrls = timeStampAndCrls;
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x00192406 File Offset: 0x00190606
		public TimeStampTokenEvidence(TimeStampAndCrl timeStampAndCrl)
		{
			this.timeStampAndCrls = new TimeStampAndCrl[]
			{
				timeStampAndCrl
			};
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x00192420 File Offset: 0x00190620
		private TimeStampTokenEvidence(Asn1Sequence seq)
		{
			this.timeStampAndCrls = new TimeStampAndCrl[seq.Count];
			int num = 0;
			foreach (object obj in seq)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				this.timeStampAndCrls[num++] = TimeStampAndCrl.GetInstance(asn1Encodable.ToAsn1Object());
			}
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x001924A0 File Offset: 0x001906A0
		public static TimeStampTokenEvidence GetInstance(Asn1TaggedObject tagged, bool isExplicit)
		{
			return TimeStampTokenEvidence.GetInstance(Asn1Sequence.GetInstance(tagged, isExplicit));
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x001924AE File Offset: 0x001906AE
		public static TimeStampTokenEvidence GetInstance(object obj)
		{
			if (obj is TimeStampTokenEvidence)
			{
				return (TimeStampTokenEvidence)obj;
			}
			if (obj != null)
			{
				return new TimeStampTokenEvidence(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x001924CF File Offset: 0x001906CF
		public virtual TimeStampAndCrl[] ToTimeStampAndCrlArray()
		{
			return (TimeStampAndCrl[])this.timeStampAndCrls.Clone();
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x001924E4 File Offset: 0x001906E4
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] v = this.timeStampAndCrls;
			return new DerSequence(v);
		}

		// Token: 0x04002D9D RID: 11677
		private TimeStampAndCrl[] timeStampAndCrls;
	}
}
