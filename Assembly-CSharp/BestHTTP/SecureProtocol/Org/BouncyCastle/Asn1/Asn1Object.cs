using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000628 RID: 1576
	public abstract class Asn1Object : Asn1Encodable
	{
		// Token: 0x06003B66 RID: 15206 RVA: 0x0016E958 File Offset: 0x0016CB58
		public static Asn1Object FromByteArray(byte[] data)
		{
			Asn1Object result;
			try
			{
				MemoryStream memoryStream = new MemoryStream(data, false);
				Asn1Object asn1Object = new Asn1InputStream(memoryStream, data.Length).ReadObject();
				if (memoryStream.Position != memoryStream.Length)
				{
					throw new IOException("extra data found after object");
				}
				result = asn1Object;
			}
			catch (InvalidCastException)
			{
				throw new IOException("cannot recognise object in byte array");
			}
			return result;
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0016E9B4 File Offset: 0x0016CBB4
		public static Asn1Object FromStream(Stream inStr)
		{
			Asn1Object result;
			try
			{
				result = new Asn1InputStream(inStr).ReadObject();
			}
			catch (InvalidCastException)
			{
				throw new IOException("cannot recognise object in stream");
			}
			return result;
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x000947CE File Offset: 0x000929CE
		public sealed override Asn1Object ToAsn1Object()
		{
			return this;
		}

		// Token: 0x06003B69 RID: 15209
		internal abstract void Encode(DerOutputStream derOut);

		// Token: 0x06003B6A RID: 15210
		protected abstract bool Asn1Equals(Asn1Object asn1Object);

		// Token: 0x06003B6B RID: 15211
		protected abstract int Asn1GetHashCode();

		// Token: 0x06003B6C RID: 15212 RVA: 0x0016E9EC File Offset: 0x0016CBEC
		internal bool CallAsn1Equals(Asn1Object obj)
		{
			return this.Asn1Equals(obj);
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x0016E9F5 File Offset: 0x0016CBF5
		internal int CallAsn1GetHashCode()
		{
			return this.Asn1GetHashCode();
		}
	}
}
