using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000659 RID: 1625
	public class DerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
	{
		// Token: 0x06003CCB RID: 15563 RVA: 0x0017272E File Offset: 0x0017092E
		internal DerOctetStringParser(DefiniteLengthInputStream stream)
		{
			this.stream = stream;
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x0017273D File Offset: 0x0017093D
		public Stream GetOctetStream()
		{
			return this.stream;
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00172748 File Offset: 0x00170948
		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = new DerOctetString(this.stream.ToArray());
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("IOException converting stream to byte array: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x040026F8 RID: 9976
		private readonly DefiniteLengthInputStream stream;
	}
}
