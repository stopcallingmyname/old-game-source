using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B4 RID: 1972
	public class CrlAnnContent : Asn1Encodable
	{
		// Token: 0x06004657 RID: 18007 RVA: 0x001931FB File Offset: 0x001913FB
		private CrlAnnContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x0019320A File Offset: 0x0019140A
		public static CrlAnnContent GetInstance(object obj)
		{
			if (obj is CrlAnnContent)
			{
				return (CrlAnnContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CrlAnnContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x0019324C File Offset: 0x0019144C
		public virtual CertificateList[] ToCertificateListArray()
		{
			CertificateList[] array = new CertificateList[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CertificateList.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0019328D File Offset: 0x0019148D
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002DD1 RID: 11729
		private readonly Asn1Sequence content;
	}
}
