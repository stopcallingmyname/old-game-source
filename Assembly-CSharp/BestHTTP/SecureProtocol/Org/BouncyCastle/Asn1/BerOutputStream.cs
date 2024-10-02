using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200063D RID: 1597
	public class BerOutputStream : DerOutputStream
	{
		// Token: 0x06003BE3 RID: 15331 RVA: 0x0016EB5D File Offset: 0x0016CD5D
		public BerOutputStream(Stream os) : base(os)
		{
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x0016FD58 File Offset: 0x0016DF58
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
			throw new IOException("object not BerEncodable");
		}
	}
}
