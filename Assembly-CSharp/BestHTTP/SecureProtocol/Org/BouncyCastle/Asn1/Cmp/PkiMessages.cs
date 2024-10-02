using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C3 RID: 1987
	public class PkiMessages : Asn1Encodable
	{
		// Token: 0x060046D5 RID: 18133 RVA: 0x00194529 File Offset: 0x00192729
		private PkiMessages(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x00194538 File Offset: 0x00192738
		public static PkiMessages GetInstance(object obj)
		{
			if (obj is PkiMessages)
			{
				return (PkiMessages)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PkiMessages((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x00194578 File Offset: 0x00192778
		public PkiMessages(params PkiMessage[] msgs)
		{
			this.content = new DerSequence(msgs);
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x0019459C File Offset: 0x0019279C
		public virtual PkiMessage[] ToPkiMessageArray()
		{
			PkiMessage[] array = new PkiMessage[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = PkiMessage.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x001945DD File Offset: 0x001927DD
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002E3C RID: 11836
		private Asn1Sequence content;
	}
}
