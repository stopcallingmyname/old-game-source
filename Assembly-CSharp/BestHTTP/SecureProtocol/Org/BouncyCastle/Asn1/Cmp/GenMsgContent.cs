using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B6 RID: 1974
	public class GenMsgContent : Asn1Encodable
	{
		// Token: 0x06004663 RID: 18019 RVA: 0x001933CE File Offset: 0x001915CE
		private GenMsgContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x001933DD File Offset: 0x001915DD
		public static GenMsgContent GetInstance(object obj)
		{
			if (obj is GenMsgContent)
			{
				return (GenMsgContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new GenMsgContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x0019341C File Offset: 0x0019161C
		public GenMsgContent(params InfoTypeAndValue[] itv)
		{
			this.content = new DerSequence(itv);
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x00193440 File Offset: 0x00191640
		public virtual InfoTypeAndValue[] ToInfoTypeAndValueArray()
		{
			InfoTypeAndValue[] array = new InfoTypeAndValue[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = InfoTypeAndValue.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x00193481 File Offset: 0x00191681
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002DD5 RID: 11733
		private readonly Asn1Sequence content;
	}
}
