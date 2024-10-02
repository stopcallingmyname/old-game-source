using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007CA RID: 1994
	public class PopoDecKeyRespContent : Asn1Encodable
	{
		// Token: 0x060046FB RID: 18171 RVA: 0x00194B01 File Offset: 0x00192D01
		private PopoDecKeyRespContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x00194B10 File Offset: 0x00192D10
		public static PopoDecKeyRespContent GetInstance(object obj)
		{
			if (obj is PopoDecKeyRespContent)
			{
				return (PopoDecKeyRespContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PopoDecKeyRespContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x00194B50 File Offset: 0x00192D50
		public virtual DerInteger[] ToDerIntegerArray()
		{
			DerInteger[] array = new DerInteger[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = DerInteger.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x00194B91 File Offset: 0x00192D91
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002E55 RID: 11861
		private readonly Asn1Sequence content;
	}
}
