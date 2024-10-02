using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000656 RID: 1622
	public class DerNumericString : DerStringBase
	{
		// Token: 0x06003CA8 RID: 15528 RVA: 0x00172006 File Offset: 0x00170206
		public static DerNumericString GetInstance(object obj)
		{
			if (obj == null || obj is DerNumericString)
			{
				return (DerNumericString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00172030 File Offset: 0x00170230
		public static DerNumericString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerNumericString)
			{
				return DerNumericString.GetInstance(@object);
			}
			return new DerNumericString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00172066 File Offset: 0x00170266
		public DerNumericString(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
		{
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x00172075 File Offset: 0x00170275
		public DerNumericString(string str) : this(str, false)
		{
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x0017207F File Offset: 0x0017027F
		public DerNumericString(string str, bool validate)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (validate && !DerNumericString.IsNumericString(str))
			{
				throw new ArgumentException("string contains illegal characters", "str");
			}
			this.str = str;
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x001720B7 File Offset: 0x001702B7
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x001720BF File Offset: 0x001702BF
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x001720CC File Offset: 0x001702CC
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(18, this.GetOctets());
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x001720DC File Offset: 0x001702DC
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerNumericString derNumericString = asn1Object as DerNumericString;
			return derNumericString != null && this.str.Equals(derNumericString.str);
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x00172108 File Offset: 0x00170308
		public static bool IsNumericString(string str)
		{
			foreach (char c in str)
			{
				if (c > '\u007f' || (c != ' ' && !char.IsDigit(c)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040026F3 RID: 9971
		private readonly string str;
	}
}
