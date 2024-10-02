using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B7 RID: 1975
	public class GenRepContent : Asn1Encodable
	{
		// Token: 0x06004668 RID: 18024 RVA: 0x00193489 File Offset: 0x00191689
		private GenRepContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x00193498 File Offset: 0x00191698
		public static GenRepContent GetInstance(object obj)
		{
			if (obj is GenRepContent)
			{
				return (GenRepContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new GenRepContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x001934D8 File Offset: 0x001916D8
		public GenRepContent(params InfoTypeAndValue[] itv)
		{
			this.content = new DerSequence(itv);
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x001934FC File Offset: 0x001916FC
		public virtual InfoTypeAndValue[] ToInfoTypeAndValueArray()
		{
			InfoTypeAndValue[] array = new InfoTypeAndValue[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = InfoTypeAndValue.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x0019353D File Offset: 0x0019173D
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002DD6 RID: 11734
		private readonly Asn1Sequence content;
	}
}
