using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006EC RID: 1772
	public class CertBag : Asn1Encodable
	{
		// Token: 0x060040F7 RID: 16631 RVA: 0x001814F0 File Offset: 0x0017F6F0
		public CertBag(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.certID = DerObjectIdentifier.GetInstance(seq[0]);
			this.certValue = Asn1TaggedObject.GetInstance(seq[1]).GetObject();
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00181545 File Offset: 0x0017F745
		public CertBag(DerObjectIdentifier certID, Asn1Object certValue)
		{
			this.certID = certID;
			this.certValue = certValue;
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060040F9 RID: 16633 RVA: 0x0018155B File Offset: 0x0017F75B
		public DerObjectIdentifier CertID
		{
			get
			{
				return this.certID;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060040FA RID: 16634 RVA: 0x00181563 File Offset: 0x0017F763
		public Asn1Object CertValue
		{
			get
			{
				return this.certValue;
			}
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x0018156B File Offset: 0x0017F76B
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.certID,
				new DerTaggedObject(0, this.certValue)
			});
		}

		// Token: 0x040029B2 RID: 10674
		private readonly DerObjectIdentifier certID;

		// Token: 0x040029B3 RID: 10675
		private readonly Asn1Object certValue;
	}
}
