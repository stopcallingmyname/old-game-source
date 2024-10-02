using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000643 RID: 1603
	public class BerSetParser : Asn1SetParser, IAsn1Convertible
	{
		// Token: 0x06003BFB RID: 15355 RVA: 0x0016FFE8 File Offset: 0x0016E1E8
		internal BerSetParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x0016FFF7 File Offset: 0x0016E1F7
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00170004 File Offset: 0x0016E204
		public Asn1Object ToAsn1Object()
		{
			return new BerSet(this._parser.ReadVector(), false);
		}

		// Token: 0x040026CC RID: 9932
		private readonly Asn1StreamParser _parser;
	}
}
