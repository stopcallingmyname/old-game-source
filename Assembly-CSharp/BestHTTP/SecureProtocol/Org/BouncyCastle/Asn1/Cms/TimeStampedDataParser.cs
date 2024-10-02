using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A7 RID: 1959
	public class TimeStampedDataParser
	{
		// Token: 0x06004607 RID: 17927 RVA: 0x001922F4 File Offset: 0x001904F4
		private TimeStampedDataParser(Asn1SequenceParser parser)
		{
			this.parser = parser;
			this.version = DerInteger.GetInstance(parser.ReadObject());
			Asn1Object asn1Object = parser.ReadObject().ToAsn1Object();
			if (asn1Object is DerIA5String)
			{
				this.dataUri = DerIA5String.GetInstance(asn1Object);
				asn1Object = parser.ReadObject().ToAsn1Object();
			}
			if (asn1Object is Asn1SequenceParser)
			{
				this.metaData = MetaData.GetInstance(asn1Object.ToAsn1Object());
				asn1Object = parser.ReadObject().ToAsn1Object();
			}
			if (asn1Object is Asn1OctetStringParser)
			{
				this.content = (Asn1OctetStringParser)asn1Object;
			}
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x00192384 File Offset: 0x00190584
		public static TimeStampedDataParser GetInstance(object obj)
		{
			if (obj is Asn1Sequence)
			{
				return new TimeStampedDataParser(((Asn1Sequence)obj).Parser);
			}
			if (obj is Asn1SequenceParser)
			{
				return new TimeStampedDataParser((Asn1SequenceParser)obj);
			}
			return null;
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x001923B4 File Offset: 0x001905B4
		public virtual DerIA5String DataUri
		{
			get
			{
				return this.dataUri;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x0600460A RID: 17930 RVA: 0x001923BC File Offset: 0x001905BC
		public virtual MetaData MetaData
		{
			get
			{
				return this.metaData;
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x0600460B RID: 17931 RVA: 0x001923C4 File Offset: 0x001905C4
		public virtual Asn1OctetStringParser Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x001923CC File Offset: 0x001905CC
		public virtual Evidence GetTemporalEvidence()
		{
			if (this.temporalEvidence == null)
			{
				this.temporalEvidence = Evidence.GetInstance(this.parser.ReadObject().ToAsn1Object());
			}
			return this.temporalEvidence;
		}

		// Token: 0x04002D97 RID: 11671
		private DerInteger version;

		// Token: 0x04002D98 RID: 11672
		private DerIA5String dataUri;

		// Token: 0x04002D99 RID: 11673
		private MetaData metaData;

		// Token: 0x04002D9A RID: 11674
		private Asn1OctetStringParser content;

		// Token: 0x04002D9B RID: 11675
		private Evidence temporalEvidence;

		// Token: 0x04002D9C RID: 11676
		private Asn1SequenceParser parser;
	}
}
