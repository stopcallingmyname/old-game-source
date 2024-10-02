using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000650 RID: 1616
	public class DerGeneralString : DerStringBase
	{
		// Token: 0x06003C75 RID: 15477 RVA: 0x001719B7 File Offset: 0x0016FBB7
		public static DerGeneralString GetInstance(object obj)
		{
			if (obj == null || obj is DerGeneralString)
			{
				return (DerGeneralString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x001719E0 File Offset: 0x0016FBE0
		public static DerGeneralString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGeneralString)
			{
				return DerGeneralString.GetInstance(@object);
			}
			return new DerGeneralString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00171A16 File Offset: 0x0016FC16
		public DerGeneralString(byte[] str) : this(Strings.FromAsciiByteArray(str))
		{
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x00171A24 File Offset: 0x0016FC24
		public DerGeneralString(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00171A41 File Offset: 0x0016FC41
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x00171A49 File Offset: 0x0016FC49
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x00171A56 File Offset: 0x0016FC56
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(27, this.GetOctets());
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x00171A68 File Offset: 0x0016FC68
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGeneralString derGeneralString = asn1Object as DerGeneralString;
			return derGeneralString != null && this.str.Equals(derGeneralString.str);
		}

		// Token: 0x040026E9 RID: 9961
		private readonly string str;
	}
}
