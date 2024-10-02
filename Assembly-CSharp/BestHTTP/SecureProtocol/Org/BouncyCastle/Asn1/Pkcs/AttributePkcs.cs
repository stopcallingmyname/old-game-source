using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006EA RID: 1770
	public class AttributePkcs : Asn1Encodable
	{
		// Token: 0x060040ED RID: 16621 RVA: 0x0018137C File Offset: 0x0017F57C
		public static AttributePkcs GetInstance(object obj)
		{
			AttributePkcs attributePkcs = obj as AttributePkcs;
			if (obj == null || attributePkcs != null)
			{
				return attributePkcs;
			}
			Asn1Sequence asn1Sequence = obj as Asn1Sequence;
			if (asn1Sequence != null)
			{
				return new AttributePkcs(asn1Sequence);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x001813C4 File Offset: 0x0017F5C4
		private AttributePkcs(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.attrType = DerObjectIdentifier.GetInstance(seq[0]);
			this.attrValues = Asn1Set.GetInstance(seq[1]);
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x00181414 File Offset: 0x0017F614
		public AttributePkcs(DerObjectIdentifier attrType, Asn1Set attrValues)
		{
			this.attrType = attrType;
			this.attrValues = attrValues;
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060040F0 RID: 16624 RVA: 0x0018142A File Offset: 0x0017F62A
		public DerObjectIdentifier AttrType
		{
			get
			{
				return this.attrType;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060040F1 RID: 16625 RVA: 0x00181432 File Offset: 0x0017F632
		public Asn1Set AttrValues
		{
			get
			{
				return this.attrValues;
			}
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x0018143A File Offset: 0x0017F63A
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.attrType,
				this.attrValues
			});
		}

		// Token: 0x040029AF RID: 10671
		private readonly DerObjectIdentifier attrType;

		// Token: 0x040029B0 RID: 10672
		private readonly Asn1Set attrValues;
	}
}
