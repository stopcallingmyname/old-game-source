using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200063F RID: 1599
	public class BerSequenceGenerator : BerGenerator
	{
		// Token: 0x06003BEC RID: 15340 RVA: 0x0016FE80 File Offset: 0x0016E080
		public BerSequenceGenerator(Stream outStream) : base(outStream)
		{
			base.WriteBerHeader(48);
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x0016FE91 File Offset: 0x0016E091
		public BerSequenceGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
			base.WriteBerHeader(48);
		}
	}
}
