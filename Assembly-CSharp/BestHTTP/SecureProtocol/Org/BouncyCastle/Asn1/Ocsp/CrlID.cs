using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200070A RID: 1802
	public class CrlID : Asn1Encodable
	{
		// Token: 0x060041DA RID: 16858 RVA: 0x0018437C File Offset: 0x0018257C
		public CrlID(Asn1Sequence seq)
		{
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.crlUrl = DerIA5String.GetInstance(asn1TaggedObject, true);
					break;
				case 1:
					this.crlNum = DerInteger.GetInstance(asn1TaggedObject, true);
					break;
				case 2:
					this.crlTime = DerGeneralizedTime.GetInstance(asn1TaggedObject, true);
					break;
				default:
					throw new ArgumentException("unknown tag number: " + asn1TaggedObject.TagNo);
				}
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x060041DB RID: 16859 RVA: 0x00184434 File Offset: 0x00182634
		public DerIA5String CrlUrl
		{
			get
			{
				return this.crlUrl;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x060041DC RID: 16860 RVA: 0x0018443C File Offset: 0x0018263C
		public DerInteger CrlNum
		{
			get
			{
				return this.crlNum;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x00184444 File Offset: 0x00182644
		public DerGeneralizedTime CrlTime
		{
			get
			{
				return this.crlTime;
			}
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x0018444C File Offset: 0x0018264C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.crlUrl != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.crlUrl)
				});
			}
			if (this.crlNum != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.crlNum)
				});
			}
			if (this.crlTime != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.crlTime)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AAA RID: 10922
		private readonly DerIA5String crlUrl;

		// Token: 0x04002AAB RID: 10923
		private readonly DerInteger crlNum;

		// Token: 0x04002AAC RID: 10924
		private readonly DerGeneralizedTime crlTime;
	}
}
