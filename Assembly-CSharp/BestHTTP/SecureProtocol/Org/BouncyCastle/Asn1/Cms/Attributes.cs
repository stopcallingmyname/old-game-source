using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200077B RID: 1915
	public class Attributes : Asn1Encodable
	{
		// Token: 0x060044A6 RID: 17574 RVA: 0x0018E3A2 File Offset: 0x0018C5A2
		private Attributes(Asn1Set attributes)
		{
			this.attributes = attributes;
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x0018E3B1 File Offset: 0x0018C5B1
		public Attributes(Asn1EncodableVector v)
		{
			this.attributes = new BerSet(v);
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x0018E3C5 File Offset: 0x0018C5C5
		public static Attributes GetInstance(object obj)
		{
			if (obj is Attributes)
			{
				return (Attributes)obj;
			}
			if (obj != null)
			{
				return new Attributes(Asn1Set.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x0018E3E8 File Offset: 0x0018C5E8
		public virtual Attribute[] GetAttributes()
		{
			Attribute[] array = new Attribute[this.attributes.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = Attribute.GetInstance(this.attributes[num]);
			}
			return array;
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x0018E429 File Offset: 0x0018C629
		public override Asn1Object ToAsn1Object()
		{
			return this.attributes;
		}

		// Token: 0x04002CFD RID: 11517
		private readonly Asn1Set attributes;
	}
}
