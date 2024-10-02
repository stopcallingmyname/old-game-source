using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000679 RID: 1657
	public class OtherInfo : Asn1Encodable
	{
		// Token: 0x06003D85 RID: 15749 RVA: 0x0017420D File Offset: 0x0017240D
		public OtherInfo(KeySpecificInfo keyInfo, Asn1OctetString partyAInfo, Asn1OctetString suppPubInfo)
		{
			this.keyInfo = keyInfo;
			this.partyAInfo = partyAInfo;
			this.suppPubInfo = suppPubInfo;
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x0017422C File Offset: 0x0017242C
		public OtherInfo(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.keyInfo = new KeySpecificInfo((Asn1Sequence)enumerator.Current);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				DerTaggedObject derTaggedObject = (DerTaggedObject)obj;
				if (derTaggedObject.TagNo == 0)
				{
					this.partyAInfo = (Asn1OctetString)derTaggedObject.GetObject();
				}
				else if (derTaggedObject.TagNo == 2)
				{
					this.suppPubInfo = (Asn1OctetString)derTaggedObject.GetObject();
				}
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06003D87 RID: 15751 RVA: 0x001742AE File Offset: 0x001724AE
		public KeySpecificInfo KeyInfo
		{
			get
			{
				return this.keyInfo;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06003D88 RID: 15752 RVA: 0x001742B6 File Offset: 0x001724B6
		public Asn1OctetString PartyAInfo
		{
			get
			{
				return this.partyAInfo;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06003D89 RID: 15753 RVA: 0x001742BE File Offset: 0x001724BE
		public Asn1OctetString SuppPubInfo
		{
			get
			{
				return this.suppPubInfo;
			}
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x001742C8 File Offset: 0x001724C8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.keyInfo
			});
			if (this.partyAInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(0, this.partyAInfo)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new DerTaggedObject(2, this.suppPubInfo)
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002719 RID: 10009
		private KeySpecificInfo keyInfo;

		// Token: 0x0400271A RID: 10010
		private Asn1OctetString partyAInfo;

		// Token: 0x0400271B RID: 10011
		private Asn1OctetString suppPubInfo;
	}
}
