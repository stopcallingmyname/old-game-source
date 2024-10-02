using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007AB RID: 1963
	public class CertConfirmContent : Asn1Encodable
	{
		// Token: 0x06004621 RID: 17953 RVA: 0x001926B5 File Offset: 0x001908B5
		private CertConfirmContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x001926C4 File Offset: 0x001908C4
		public static CertConfirmContent GetInstance(object obj)
		{
			if (obj is CertConfirmContent)
			{
				return (CertConfirmContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertConfirmContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x00192704 File Offset: 0x00190904
		public virtual CertStatus[] ToCertStatusArray()
		{
			CertStatus[] array = new CertStatus[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CertStatus.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x00192745 File Offset: 0x00190945
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002DA3 RID: 11683
		private readonly Asn1Sequence content;
	}
}
