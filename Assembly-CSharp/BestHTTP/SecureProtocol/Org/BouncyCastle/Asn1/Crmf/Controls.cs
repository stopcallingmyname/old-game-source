using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x0200076B RID: 1899
	public class Controls : Asn1Encodable
	{
		// Token: 0x0600443F RID: 17471 RVA: 0x0018D378 File Offset: 0x0018B578
		private Controls(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0018D387 File Offset: 0x0018B587
		public static Controls GetInstance(object obj)
		{
			if (obj is Controls)
			{
				return (Controls)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Controls((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0018D3C8 File Offset: 0x0018B5C8
		public Controls(params AttributeTypeAndValue[] atvs)
		{
			this.content = new DerSequence(atvs);
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0018D3EC File Offset: 0x0018B5EC
		public virtual AttributeTypeAndValue[] ToAttributeTypeAndValueArray()
		{
			AttributeTypeAndValue[] array = new AttributeTypeAndValue[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = AttributeTypeAndValue.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0018D42D File Offset: 0x0018B62D
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002CC7 RID: 11463
		private readonly Asn1Sequence content;
	}
}
