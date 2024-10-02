using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200062B RID: 1579
	public class Asn1OutputStream : DerOutputStream
	{
		// Token: 0x06003B7A RID: 15226 RVA: 0x0016EB5D File Offset: 0x0016CD5D
		public Asn1OutputStream(Stream os) : base(os)
		{
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0016EB68 File Offset: 0x0016CD68
		[Obsolete("Use version taking an Asn1Encodable arg instead")]
		public override void WriteObject(object obj)
		{
			if (obj == null)
			{
				base.WriteNull();
				return;
			}
			if (obj is Asn1Object)
			{
				((Asn1Object)obj).Encode(this);
				return;
			}
			if (obj is Asn1Encodable)
			{
				((Asn1Encodable)obj).ToAsn1Object().Encode(this);
				return;
			}
			throw new IOException("object not Asn1Encodable");
		}
	}
}
