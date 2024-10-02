using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200065B RID: 1627
	public class DerPrintableString : DerStringBase
	{
		// Token: 0x06003CD9 RID: 15577 RVA: 0x0017297B File Offset: 0x00170B7B
		public static DerPrintableString GetInstance(object obj)
		{
			if (obj == null || obj is DerPrintableString)
			{
				return (DerPrintableString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x001729A4 File Offset: 0x00170BA4
		public static DerPrintableString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerPrintableString)
			{
				return DerPrintableString.GetInstance(@object);
			}
			return new DerPrintableString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x001729DA File Offset: 0x00170BDA
		public DerPrintableString(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
		{
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x001729E9 File Offset: 0x00170BE9
		public DerPrintableString(string str) : this(str, false)
		{
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x001729F3 File Offset: 0x00170BF3
		public DerPrintableString(string str, bool validate)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (validate && !DerPrintableString.IsPrintableString(str))
			{
				throw new ArgumentException("string contains illegal characters", "str");
			}
			this.str = str;
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00172A2B File Offset: 0x00170C2B
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x00172A33 File Offset: 0x00170C33
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00172A40 File Offset: 0x00170C40
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(19, this.GetOctets());
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00172A50 File Offset: 0x00170C50
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerPrintableString derPrintableString = asn1Object as DerPrintableString;
			return derPrintableString != null && this.str.Equals(derPrintableString.str);
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00172A7C File Offset: 0x00170C7C
		public static bool IsPrintableString(string str)
		{
			foreach (char c in str)
			{
				if (c > '\u007f')
				{
					return false;
				}
				if (!char.IsLetterOrDigit(c))
				{
					if (c <= ':')
					{
						switch (c)
						{
						case ' ':
						case '\'':
						case '(':
						case ')':
						case '+':
						case ',':
						case '-':
						case '.':
						case '/':
							goto IL_7E;
						case '!':
						case '"':
						case '#':
						case '$':
						case '%':
						case '&':
						case '*':
							break;
						default:
							if (c == ':')
							{
								goto IL_7E;
							}
							break;
						}
					}
					else if (c == '=' || c == '?')
					{
						goto IL_7E;
					}
					return false;
				}
				IL_7E:;
			}
			return true;
		}

		// Token: 0x040026F9 RID: 9977
		private readonly string str;
	}
}
