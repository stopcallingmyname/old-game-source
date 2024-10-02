using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200064C RID: 1612
	public class DerEnumerated : Asn1Object
	{
		// Token: 0x06003C44 RID: 15428 RVA: 0x00170FEC File Offset: 0x0016F1EC
		public static DerEnumerated GetInstance(object obj)
		{
			if (obj == null || obj is DerEnumerated)
			{
				return (DerEnumerated)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x00171018 File Offset: 0x0016F218
		public static DerEnumerated GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerEnumerated)
			{
				return DerEnumerated.GetInstance(@object);
			}
			return DerEnumerated.FromOctetString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x0017104E File Offset: 0x0016F24E
		public DerEnumerated(int val)
		{
			this.bytes = BigInteger.ValueOf((long)val).ToByteArray();
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x00171068 File Offset: 0x0016F268
		public DerEnumerated(BigInteger val)
		{
			this.bytes = val.ToByteArray();
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x0017107C File Offset: 0x0016F27C
		public DerEnumerated(byte[] bytes)
		{
			if (bytes.Length > 1 && ((bytes[0] == 0 && (bytes[1] & 128) == 0) || (bytes[0] == 255 && (bytes[1] & 128) != 0)) && !DerInteger.AllowUnsafe())
			{
				throw new ArgumentException("malformed enumerated");
			}
			this.bytes = Arrays.Clone(bytes);
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x001710D8 File Offset: 0x0016F2D8
		public BigInteger Value
		{
			get
			{
				return new BigInteger(this.bytes);
			}
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x001710E5 File Offset: 0x0016F2E5
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(10, this.bytes);
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x001710F8 File Offset: 0x0016F2F8
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerEnumerated derEnumerated = asn1Object as DerEnumerated;
			return derEnumerated != null && Arrays.AreEqual(this.bytes, derEnumerated.bytes);
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x00171122 File Offset: 0x0016F322
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.bytes);
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x00171130 File Offset: 0x0016F330
		internal static DerEnumerated FromOctetString(byte[] enc)
		{
			if (enc.Length == 0)
			{
				throw new ArgumentException("ENUMERATED has zero length", "enc");
			}
			if (enc.Length == 1)
			{
				int num = (int)enc[0];
				if (num < DerEnumerated.cache.Length)
				{
					DerEnumerated derEnumerated = DerEnumerated.cache[num];
					if (derEnumerated != null)
					{
						return derEnumerated;
					}
					return DerEnumerated.cache[num] = new DerEnumerated(Arrays.Clone(enc));
				}
			}
			return new DerEnumerated(Arrays.Clone(enc));
		}

		// Token: 0x040026E0 RID: 9952
		private readonly byte[] bytes;

		// Token: 0x040026E1 RID: 9953
		private static readonly DerEnumerated[] cache = new DerEnumerated[12];
	}
}
