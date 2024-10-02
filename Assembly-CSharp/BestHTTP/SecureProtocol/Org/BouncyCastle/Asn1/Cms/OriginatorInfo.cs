using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000795 RID: 1941
	public class OriginatorInfo : Asn1Encodable
	{
		// Token: 0x06004573 RID: 17779 RVA: 0x0019088E File Offset: 0x0018EA8E
		public OriginatorInfo(Asn1Set certs, Asn1Set crls)
		{
			this.certs = certs;
			this.crls = crls;
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x001908A4 File Offset: 0x0018EAA4
		public OriginatorInfo(Asn1Sequence seq)
		{
			switch (seq.Count)
			{
			case 0:
				return;
			case 1:
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[0];
				int tagNo = asn1TaggedObject.TagNo;
				if (tagNo == 0)
				{
					this.certs = Asn1Set.GetInstance(asn1TaggedObject, false);
					return;
				}
				if (tagNo != 1)
				{
					throw new ArgumentException("Bad tag in OriginatorInfo: " + asn1TaggedObject.TagNo);
				}
				this.crls = Asn1Set.GetInstance(asn1TaggedObject, false);
				return;
			}
			case 2:
				this.certs = Asn1Set.GetInstance((Asn1TaggedObject)seq[0], false);
				this.crls = Asn1Set.GetInstance((Asn1TaggedObject)seq[1], false);
				return;
			default:
				throw new ArgumentException("OriginatorInfo too big");
			}
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x00190965 File Offset: 0x0018EB65
		public static OriginatorInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return OriginatorInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x00190973 File Offset: 0x0018EB73
		public static OriginatorInfo GetInstance(object obj)
		{
			if (obj == null || obj is OriginatorInfo)
			{
				return (OriginatorInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OriginatorInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid OriginatorInfo: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06004577 RID: 17783 RVA: 0x001909B0 File Offset: 0x0018EBB0
		public Asn1Set Certificates
		{
			get
			{
				return this.certs;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06004578 RID: 17784 RVA: 0x001909B8 File Offset: 0x0018EBB8
		public Asn1Set Crls
		{
			get
			{
				return this.crls;
			}
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x001909C0 File Offset: 0x0018EBC0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.certs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.certs)
				});
			}
			if (this.crls != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.crls)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D5F RID: 11615
		private Asn1Set certs;

		// Token: 0x04002D60 RID: 11616
		private Asn1Set crls;
	}
}
