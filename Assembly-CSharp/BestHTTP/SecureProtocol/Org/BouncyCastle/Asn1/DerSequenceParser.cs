using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200065E RID: 1630
	public class DerSequenceParser : Asn1SequenceParser, IAsn1Convertible
	{
		// Token: 0x06003CEF RID: 15599 RVA: 0x00172CBE File Offset: 0x00170EBE
		internal DerSequenceParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00172CCD File Offset: 0x00170ECD
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00172CDA File Offset: 0x00170EDA
		public Asn1Object ToAsn1Object()
		{
			return new DerSequence(this._parser.ReadVector());
		}

		// Token: 0x040026FC RID: 9980
		private readonly Asn1StreamParser _parser;
	}
}
