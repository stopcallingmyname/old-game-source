using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000661 RID: 1633
	public class DerSetParser : Asn1SetParser, IAsn1Convertible
	{
		// Token: 0x06003D00 RID: 15616 RVA: 0x00172EC6 File Offset: 0x001710C6
		internal DerSetParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00172ED5 File Offset: 0x001710D5
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x00172EE2 File Offset: 0x001710E2
		public Asn1Object ToAsn1Object()
		{
			return new DerSet(this._parser.ReadVector(), false);
		}

		// Token: 0x040026FF RID: 9983
		private readonly Asn1StreamParser _parser;
	}
}
