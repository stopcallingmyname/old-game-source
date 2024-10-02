using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000654 RID: 1620
	public class DerInteger : Asn1Object
	{
		// Token: 0x06003C96 RID: 15510 RVA: 0x00171E2C File Offset: 0x0017002C
		internal static bool AllowUnsafe()
		{
			string environmentVariable = Platform.GetEnvironmentVariable("BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.AllowUnsafeInteger");
			return environmentVariable != null && Platform.EqualsIgnoreCase("true", environmentVariable);
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x00171E54 File Offset: 0x00170054
		public static DerInteger GetInstance(object obj)
		{
			if (obj == null || obj is DerInteger)
			{
				return (DerInteger)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x00171E80 File Offset: 0x00170080
		public static DerInteger GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerInteger)
			{
				return DerInteger.GetInstance(@object);
			}
			return new DerInteger(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x00171EC4 File Offset: 0x001700C4
		public DerInteger(int value)
		{
			this.bytes = BigInteger.ValueOf((long)value).ToByteArray();
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x00171EDE File Offset: 0x001700DE
		public DerInteger(BigInteger value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.bytes = value.ToByteArray();
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00171F00 File Offset: 0x00170100
		public DerInteger(byte[] bytes)
		{
			if (bytes.Length > 1 && ((bytes[0] == 0 && (bytes[1] & 128) == 0) || (bytes[0] == 255 && (bytes[1] & 128) != 0)) && !DerInteger.AllowUnsafe())
			{
				throw new ArgumentException("malformed integer");
			}
			this.bytes = Arrays.Clone(bytes);
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06003C9C RID: 15516 RVA: 0x00171F5C File Offset: 0x0017015C
		public BigInteger Value
		{
			get
			{
				return new BigInteger(this.bytes);
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06003C9D RID: 15517 RVA: 0x00171F69 File Offset: 0x00170169
		public BigInteger PositiveValue
		{
			get
			{
				return new BigInteger(1, this.bytes);
			}
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x00171F77 File Offset: 0x00170177
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(2, this.bytes);
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x00171F86 File Offset: 0x00170186
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.bytes);
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x00171F94 File Offset: 0x00170194
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerInteger derInteger = asn1Object as DerInteger;
			return derInteger != null && Arrays.AreEqual(this.bytes, derInteger.bytes);
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x00171FBE File Offset: 0x001701BE
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x040026EF RID: 9967
		public const string AllowUnsafeProperty = "BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.AllowUnsafeInteger";

		// Token: 0x040026F0 RID: 9968
		private readonly byte[] bytes;
	}
}
