using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C7 RID: 1735
	public abstract class X509NameEntryConverter
	{
		// Token: 0x06004011 RID: 16401 RVA: 0x0017D3A6 File Offset: 0x0017B5A6
		protected Asn1Object ConvertHexEncoded(string hexString, int offset)
		{
			return Asn1Object.FromByteArray(Hex.Decode(hexString.Substring(offset)));
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x0017D3B9 File Offset: 0x0017B5B9
		protected bool CanBePrintable(string str)
		{
			return DerPrintableString.IsPrintableString(str);
		}

		// Token: 0x06004013 RID: 16403
		public abstract Asn1Object GetConvertedValue(DerObjectIdentifier oid, string value);
	}
}
