using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A6 RID: 1958
	public class TimeStampedData : Asn1Encodable
	{
		// Token: 0x060045FF RID: 17919 RVA: 0x00192169 File Offset: 0x00190369
		public TimeStampedData(DerIA5String dataUri, MetaData metaData, Asn1OctetString content, Evidence temporalEvidence)
		{
			this.version = new DerInteger(1);
			this.dataUri = dataUri;
			this.metaData = metaData;
			this.content = content;
			this.temporalEvidence = temporalEvidence;
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x0019219C File Offset: 0x0019039C
		private TimeStampedData(Asn1Sequence seq)
		{
			this.version = DerInteger.GetInstance(seq[0]);
			int index = 1;
			if (seq[index] is DerIA5String)
			{
				this.dataUri = DerIA5String.GetInstance(seq[index++]);
			}
			if (seq[index] is MetaData || seq[index] is Asn1Sequence)
			{
				this.metaData = MetaData.GetInstance(seq[index++]);
			}
			if (seq[index] is Asn1OctetString)
			{
				this.content = Asn1OctetString.GetInstance(seq[index++]);
			}
			this.temporalEvidence = Evidence.GetInstance(seq[index]);
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x0019224F File Offset: 0x0019044F
		public static TimeStampedData GetInstance(object obj)
		{
			if (obj is TimeStampedData)
			{
				return (TimeStampedData)obj;
			}
			if (obj != null)
			{
				return new TimeStampedData(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x00192270 File Offset: 0x00190470
		public virtual DerIA5String DataUri
		{
			get
			{
				return this.dataUri;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06004603 RID: 17923 RVA: 0x00192278 File Offset: 0x00190478
		public MetaData MetaData
		{
			get
			{
				return this.metaData;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06004604 RID: 17924 RVA: 0x00192280 File Offset: 0x00190480
		public Asn1OctetString Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06004605 RID: 17925 RVA: 0x00192288 File Offset: 0x00190488
		public Evidence TemporalEvidence
		{
			get
			{
				return this.temporalEvidence;
			}
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x00192290 File Offset: 0x00190490
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.dataUri,
				this.metaData,
				this.content
			});
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.temporalEvidence
			});
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D92 RID: 11666
		private DerInteger version;

		// Token: 0x04002D93 RID: 11667
		private DerIA5String dataUri;

		// Token: 0x04002D94 RID: 11668
		private MetaData metaData;

		// Token: 0x04002D95 RID: 11669
		private Asn1OctetString content;

		// Token: 0x04002D96 RID: 11670
		private Evidence temporalEvidence;
	}
}
