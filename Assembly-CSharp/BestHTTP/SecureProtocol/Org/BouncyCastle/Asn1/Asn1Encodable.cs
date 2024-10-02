using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000622 RID: 1570
	public abstract class Asn1Encodable : IAsn1Convertible
	{
		// Token: 0x06003B3E RID: 15166 RVA: 0x0016E1D1 File Offset: 0x0016C3D1
		public byte[] GetEncoded()
		{
			MemoryStream memoryStream = new MemoryStream();
			new Asn1OutputStream(memoryStream).WriteObject(this);
			return memoryStream.ToArray();
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0016E1E9 File Offset: 0x0016C3E9
		public byte[] GetEncoded(string encoding)
		{
			if (encoding.Equals("DER"))
			{
				MemoryStream memoryStream = new MemoryStream();
				new DerOutputStream(memoryStream).WriteObject(this);
				return memoryStream.ToArray();
			}
			return this.GetEncoded();
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x0016E218 File Offset: 0x0016C418
		public byte[] GetDerEncoded()
		{
			byte[] result;
			try
			{
				result = this.GetEncoded("DER");
			}
			catch (IOException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x0016E24C File Offset: 0x0016C44C
		public sealed override int GetHashCode()
		{
			return this.ToAsn1Object().CallAsn1GetHashCode();
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x0016E25C File Offset: 0x0016C45C
		public sealed override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			IAsn1Convertible asn1Convertible = obj as IAsn1Convertible;
			if (asn1Convertible == null)
			{
				return false;
			}
			Asn1Object asn1Object = this.ToAsn1Object();
			Asn1Object asn1Object2 = asn1Convertible.ToAsn1Object();
			return asn1Object == asn1Object2 || asn1Object.CallAsn1Equals(asn1Object2);
		}

		// Token: 0x06003B43 RID: 15171
		public abstract Asn1Object ToAsn1Object();

		// Token: 0x04002695 RID: 9877
		public const string Der = "DER";

		// Token: 0x04002696 RID: 9878
		public const string Ber = "BER";
	}
}
