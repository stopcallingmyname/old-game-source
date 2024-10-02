using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000642 RID: 1602
	public class BerSetGenerator : BerGenerator
	{
		// Token: 0x06003BF9 RID: 15353 RVA: 0x0016FFC4 File Offset: 0x0016E1C4
		public BerSetGenerator(Stream outStream) : base(outStream)
		{
			base.WriteBerHeader(49);
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0016FFD5 File Offset: 0x0016E1D5
		public BerSetGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
			base.WriteBerHeader(49);
		}
	}
}
