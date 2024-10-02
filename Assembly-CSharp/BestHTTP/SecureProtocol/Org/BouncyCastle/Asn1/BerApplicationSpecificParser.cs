using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000636 RID: 1590
	public class BerApplicationSpecificParser : IAsn1ApplicationSpecificParser, IAsn1Convertible
	{
		// Token: 0x06003BBB RID: 15291 RVA: 0x0016F87F File Offset: 0x0016DA7F
		internal BerApplicationSpecificParser(int tag, Asn1StreamParser parser)
		{
			this.tag = tag;
			this.parser = parser;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0016F895 File Offset: 0x0016DA95
		public IAsn1Convertible ReadObject()
		{
			return this.parser.ReadObject();
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0016F8A2 File Offset: 0x0016DAA2
		public Asn1Object ToAsn1Object()
		{
			return new BerApplicationSpecific(this.tag, this.parser.ReadVector());
		}

		// Token: 0x040026C0 RID: 9920
		private readonly int tag;

		// Token: 0x040026C1 RID: 9921
		private readonly Asn1StreamParser parser;
	}
}
