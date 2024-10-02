using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200064E RID: 1614
	public class DerExternalParser : Asn1Encodable
	{
		// Token: 0x06003C61 RID: 15457 RVA: 0x001714C2 File Offset: 0x0016F6C2
		public DerExternalParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x001714D1 File Offset: 0x0016F6D1
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x001714DE File Offset: 0x0016F6DE
		public override Asn1Object ToAsn1Object()
		{
			return new DerExternal(this._parser.ReadVector());
		}

		// Token: 0x040026E7 RID: 9959
		private readonly Asn1StreamParser _parser;
	}
}
