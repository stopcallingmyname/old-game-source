using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x0200074C RID: 1868
	public class OcspIdentifier : Asn1Encodable
	{
		// Token: 0x06004366 RID: 17254 RVA: 0x0018A258 File Offset: 0x00188458
		public static OcspIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is OcspIdentifier)
			{
				return (OcspIdentifier)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OcspIdentifier((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OcspIdentifier' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x0018A2A8 File Offset: 0x001884A8
		private OcspIdentifier(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.ocspResponderID = ResponderID.GetInstance(seq[0].ToAsn1Object());
			this.producedAt = (DerGeneralizedTime)seq[1].ToAsn1Object();
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x0018A320 File Offset: 0x00188520
		public OcspIdentifier(ResponderID ocspResponderID, DateTime producedAt)
		{
			if (ocspResponderID == null)
			{
				throw new ArgumentNullException();
			}
			this.ocspResponderID = ocspResponderID;
			this.producedAt = new DerGeneralizedTime(producedAt);
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x0018A344 File Offset: 0x00188544
		public ResponderID OcspResponderID
		{
			get
			{
				return this.ocspResponderID;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x0600436A RID: 17258 RVA: 0x0018A34C File Offset: 0x0018854C
		public DateTime ProducedAt
		{
			get
			{
				return this.producedAt.ToDateTime();
			}
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0018A359 File Offset: 0x00188559
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.ocspResponderID,
				this.producedAt
			});
		}

		// Token: 0x04002C3F RID: 11327
		private readonly ResponderID ocspResponderID;

		// Token: 0x04002C40 RID: 11328
		private readonly DerGeneralizedTime producedAt;
	}
}
