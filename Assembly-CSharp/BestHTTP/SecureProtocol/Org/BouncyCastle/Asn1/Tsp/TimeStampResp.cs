using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp
{
	// Token: 0x020006DC RID: 1756
	public class TimeStampResp : Asn1Encodable
	{
		// Token: 0x06004098 RID: 16536 RVA: 0x0017FAF9 File Offset: 0x0017DCF9
		public static TimeStampResp GetInstance(object o)
		{
			if (o == null || o is TimeStampResp)
			{
				return (TimeStampResp)o;
			}
			if (o is Asn1Sequence)
			{
				return new TimeStampResp((Asn1Sequence)o);
			}
			throw new ArgumentException("Unknown object in 'TimeStampResp' factory: " + Platform.GetTypeName(o));
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0017FB36 File Offset: 0x0017DD36
		private TimeStampResp(Asn1Sequence seq)
		{
			this.pkiStatusInfo = PkiStatusInfo.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.timeStampToken = ContentInfo.GetInstance(seq[1]);
			}
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0017FB6B File Offset: 0x0017DD6B
		public TimeStampResp(PkiStatusInfo pkiStatusInfo, ContentInfo timeStampToken)
		{
			this.pkiStatusInfo = pkiStatusInfo;
			this.timeStampToken = timeStampToken;
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x0017FB81 File Offset: 0x0017DD81
		public PkiStatusInfo Status
		{
			get
			{
				return this.pkiStatusInfo;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x0017FB89 File Offset: 0x0017DD89
		public ContentInfo TimeStampToken
		{
			get
			{
				return this.timeStampToken;
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0017FB94 File Offset: 0x0017DD94
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.pkiStatusInfo
			});
			if (this.timeStampToken != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.timeStampToken
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002933 RID: 10547
		private readonly PkiStatusInfo pkiStatusInfo;

		// Token: 0x04002934 RID: 10548
		private readonly ContentInfo timeStampToken;
	}
}
