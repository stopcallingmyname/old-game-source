using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C8 RID: 1992
	public class PollReqContent : Asn1Encodable
	{
		// Token: 0x060046F2 RID: 18162 RVA: 0x00194992 File Offset: 0x00192B92
		private PollReqContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x001949A1 File Offset: 0x00192BA1
		public static PollReqContent GetInstance(object obj)
		{
			if (obj is PollReqContent)
			{
				return (PollReqContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PollReqContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x001949E0 File Offset: 0x00192BE0
		public virtual DerInteger[][] GetCertReqIDs()
		{
			DerInteger[][] array = new DerInteger[this.content.Count][];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = PollReqContent.SequenceToDerIntegerArray((Asn1Sequence)this.content[num]);
			}
			return array;
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x00194A28 File Offset: 0x00192C28
		private static DerInteger[] SequenceToDerIntegerArray(Asn1Sequence seq)
		{
			DerInteger[] array = new DerInteger[seq.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = DerInteger.GetInstance(seq[num]);
			}
			return array;
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x00194A5F File Offset: 0x00192C5F
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002E53 RID: 11859
		private readonly Asn1Sequence content;
	}
}
