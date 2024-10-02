using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000653 RID: 1619
	public class DerIA5String : DerStringBase
	{
		// Token: 0x06003C8B RID: 15499 RVA: 0x00171CEA File Offset: 0x0016FEEA
		public static DerIA5String GetInstance(object obj)
		{
			if (obj == null || obj is DerIA5String)
			{
				return (DerIA5String)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x00171D14 File Offset: 0x0016FF14
		public static DerIA5String GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerIA5String)
			{
				return DerIA5String.GetInstance(@object);
			}
			return new DerIA5String(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x00171D4A File Offset: 0x0016FF4A
		public DerIA5String(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
		{
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x00171D59 File Offset: 0x0016FF59
		public DerIA5String(string str) : this(str, false)
		{
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x00171D63 File Offset: 0x0016FF63
		public DerIA5String(string str, bool validate)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (validate && !DerIA5String.IsIA5String(str))
			{
				throw new ArgumentException("string contains illegal characters", "str");
			}
			this.str = str;
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00171D9B File Offset: 0x0016FF9B
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x00171DA3 File Offset: 0x0016FFA3
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x00171DB0 File Offset: 0x0016FFB0
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(22, this.GetOctets());
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x00171DC0 File Offset: 0x0016FFC0
		protected override int Asn1GetHashCode()
		{
			return this.str.GetHashCode();
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x00171DD0 File Offset: 0x0016FFD0
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerIA5String derIA5String = asn1Object as DerIA5String;
			return derIA5String != null && this.str.Equals(derIA5String.str);
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x00171DFC File Offset: 0x0016FFFC
		public static bool IsIA5String(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] > '\u007f')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040026EE RID: 9966
		private readonly string str;
	}
}
