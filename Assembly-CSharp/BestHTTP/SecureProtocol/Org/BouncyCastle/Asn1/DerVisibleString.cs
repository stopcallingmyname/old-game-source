using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000669 RID: 1641
	public class DerVisibleString : DerStringBase
	{
		// Token: 0x06003D3A RID: 15674 RVA: 0x001736EC File Offset: 0x001718EC
		public static DerVisibleString GetInstance(object obj)
		{
			if (obj == null || obj is DerVisibleString)
			{
				return (DerVisibleString)obj;
			}
			if (obj is Asn1OctetString)
			{
				return new DerVisibleString(((Asn1OctetString)obj).GetOctets());
			}
			if (obj is Asn1TaggedObject)
			{
				return DerVisibleString.GetInstance(((Asn1TaggedObject)obj).GetObject());
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00173752 File Offset: 0x00171952
		public static DerVisibleString GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DerVisibleString.GetInstance(obj.GetObject());
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x0017375F File Offset: 0x0017195F
		public DerVisibleString(byte[] str) : this(Strings.FromAsciiByteArray(str))
		{
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x0017376D File Offset: 0x0017196D
		public DerVisibleString(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x0017378A File Offset: 0x0017198A
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00173792 File Offset: 0x00171992
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x0017379F File Offset: 0x0017199F
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(26, this.GetOctets());
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x001737B0 File Offset: 0x001719B0
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerVisibleString derVisibleString = asn1Object as DerVisibleString;
			return derVisibleString != null && this.str.Equals(derVisibleString.str);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x001737DA File Offset: 0x001719DA
		protected override int Asn1GetHashCode()
		{
			return this.str.GetHashCode();
		}

		// Token: 0x04002706 RID: 9990
		private readonly string str;
	}
}
