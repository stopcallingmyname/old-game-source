using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006EB RID: 1771
	public class AuthenticatedSafe : Asn1Encodable
	{
		// Token: 0x060040F3 RID: 16627 RVA: 0x0018145C File Offset: 0x0017F65C
		public AuthenticatedSafe(Asn1Sequence seq)
		{
			this.info = new ContentInfo[seq.Count];
			for (int num = 0; num != this.info.Length; num++)
			{
				this.info[num] = ContentInfo.GetInstance(seq[num]);
			}
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x001814A7 File Offset: 0x0017F6A7
		public AuthenticatedSafe(ContentInfo[] info)
		{
			this.info = (ContentInfo[])info.Clone();
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x001814C0 File Offset: 0x0017F6C0
		public ContentInfo[] GetContentInfo()
		{
			return (ContentInfo[])this.info.Clone();
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x001814D4 File Offset: 0x0017F6D4
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] v = this.info;
			return new BerSequence(v);
		}

		// Token: 0x040029B1 RID: 10673
		private readonly ContentInfo[] info;
	}
}
