using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000667 RID: 1639
	public class DerUtf8String : DerStringBase
	{
		// Token: 0x06003D2B RID: 15659 RVA: 0x001734D7 File Offset: 0x001716D7
		public static DerUtf8String GetInstance(object obj)
		{
			if (obj == null || obj is DerUtf8String)
			{
				return (DerUtf8String)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00173500 File Offset: 0x00171700
		public static DerUtf8String GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerUtf8String)
			{
				return DerUtf8String.GetInstance(@object);
			}
			return new DerUtf8String(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x00173536 File Offset: 0x00171736
		public DerUtf8String(byte[] str) : this(Encoding.UTF8.GetString(str, 0, str.Length))
		{
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x0017354D File Offset: 0x0017174D
		public DerUtf8String(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x0017356A File Offset: 0x0017176A
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00173574 File Offset: 0x00171774
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerUtf8String derUtf8String = asn1Object as DerUtf8String;
			return derUtf8String != null && this.str.Equals(derUtf8String.str);
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x0017359E File Offset: 0x0017179E
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(12, Encoding.UTF8.GetBytes(this.str));
		}

		// Token: 0x04002704 RID: 9988
		private readonly string str;
	}
}
