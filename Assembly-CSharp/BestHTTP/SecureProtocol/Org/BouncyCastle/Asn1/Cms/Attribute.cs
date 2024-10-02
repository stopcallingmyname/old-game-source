using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200077A RID: 1914
	public class Attribute : Asn1Encodable
	{
		// Token: 0x060044A0 RID: 17568 RVA: 0x0018E2E4 File Offset: 0x0018C4E4
		public static Attribute GetInstance(object obj)
		{
			if (obj == null || obj is Attribute)
			{
				return (Attribute)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Attribute((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0018E331 File Offset: 0x0018C531
		public Attribute(Asn1Sequence seq)
		{
			this.attrType = (DerObjectIdentifier)seq[0];
			this.attrValues = (Asn1Set)seq[1];
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0018E35D File Offset: 0x0018C55D
		public Attribute(DerObjectIdentifier attrType, Asn1Set attrValues)
		{
			this.attrType = attrType;
			this.attrValues = attrValues;
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060044A3 RID: 17571 RVA: 0x0018E373 File Offset: 0x0018C573
		public DerObjectIdentifier AttrType
		{
			get
			{
				return this.attrType;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060044A4 RID: 17572 RVA: 0x0018E37B File Offset: 0x0018C57B
		public Asn1Set AttrValues
		{
			get
			{
				return this.attrValues;
			}
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x0018E383 File Offset: 0x0018C583
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.attrType,
				this.attrValues
			});
		}

		// Token: 0x04002CFB RID: 11515
		private DerObjectIdentifier attrType;

		// Token: 0x04002CFC RID: 11516
		private Asn1Set attrValues;
	}
}
