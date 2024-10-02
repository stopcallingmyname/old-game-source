using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000640 RID: 1600
	public class BerSequenceParser : Asn1SequenceParser, IAsn1Convertible
	{
		// Token: 0x06003BEE RID: 15342 RVA: 0x0016FEA4 File Offset: 0x0016E0A4
		internal BerSequenceParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x0016FEB3 File Offset: 0x0016E0B3
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x0016FEC0 File Offset: 0x0016E0C0
		public Asn1Object ToAsn1Object()
		{
			return new BerSequence(this._parser.ReadVector());
		}

		// Token: 0x040026CA RID: 9930
		private readonly Asn1StreamParser _parser;
	}
}
