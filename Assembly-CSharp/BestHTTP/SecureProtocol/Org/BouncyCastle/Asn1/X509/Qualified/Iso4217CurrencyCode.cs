using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006CF RID: 1743
	public class Iso4217CurrencyCode : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600403B RID: 16443 RVA: 0x0017DDC4 File Offset: 0x0017BFC4
		public static Iso4217CurrencyCode GetInstance(object obj)
		{
			if (obj == null || obj is Iso4217CurrencyCode)
			{
				return (Iso4217CurrencyCode)obj;
			}
			if (obj is DerInteger)
			{
				return new Iso4217CurrencyCode(DerInteger.GetInstance(obj).Value.IntValue);
			}
			if (obj is DerPrintableString)
			{
				return new Iso4217CurrencyCode(DerPrintableString.GetInstance(obj).GetString());
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0017DE34 File Offset: 0x0017C034
		public Iso4217CurrencyCode(int numeric)
		{
			if (numeric > 999 || numeric < 1)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"wrong size in numeric code : not in (",
					1,
					"..",
					999,
					")"
				}));
			}
			this.obj = new DerInteger(numeric);
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x0017DE9E File Offset: 0x0017C09E
		public Iso4217CurrencyCode(string alphabetic)
		{
			if (alphabetic.Length > 3)
			{
				throw new ArgumentException("wrong size in alphabetic code : max size is " + 3);
			}
			this.obj = new DerPrintableString(alphabetic);
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x0600403E RID: 16446 RVA: 0x0017DED1 File Offset: 0x0017C0D1
		public bool IsAlphabetic
		{
			get
			{
				return this.obj is DerPrintableString;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x0600403F RID: 16447 RVA: 0x0017DEE1 File Offset: 0x0017C0E1
		public string Alphabetic
		{
			get
			{
				return ((DerPrintableString)this.obj).GetString();
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x0017DEF3 File Offset: 0x0017C0F3
		public int Numeric
		{
			get
			{
				return ((DerInteger)this.obj).Value.IntValue;
			}
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x0017DF0A File Offset: 0x0017C10A
		public override Asn1Object ToAsn1Object()
		{
			return this.obj.ToAsn1Object();
		}

		// Token: 0x040028E7 RID: 10471
		internal const int AlphabeticMaxSize = 3;

		// Token: 0x040028E8 RID: 10472
		internal const int NumericMinSize = 1;

		// Token: 0x040028E9 RID: 10473
		internal const int NumericMaxSize = 999;

		// Token: 0x040028EA RID: 10474
		internal Asn1Encodable obj;
	}
}
