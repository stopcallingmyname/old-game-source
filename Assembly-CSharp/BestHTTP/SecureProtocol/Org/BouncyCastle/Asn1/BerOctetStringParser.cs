using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200063C RID: 1596
	public class BerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
	{
		// Token: 0x06003BE0 RID: 15328 RVA: 0x0016FCF0 File Offset: 0x0016DEF0
		internal BerOctetStringParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0016FCFF File Offset: 0x0016DEFF
		public Stream GetOctetStream()
		{
			return new ConstructedOctetStream(this._parser);
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x0016FD0C File Offset: 0x0016DF0C
		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = new BerOctetString(Streams.ReadAll(this.GetOctetStream()));
			}
			catch (IOException ex)
			{
				throw new Asn1ParsingException("IOException converting stream to byte array: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x040026C8 RID: 9928
		private readonly Asn1StreamParser _parser;
	}
}
