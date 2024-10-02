using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200064A RID: 1610
	public class DerBmpString : DerStringBase
	{
		// Token: 0x06003C31 RID: 15409 RVA: 0x00170CFE File Offset: 0x0016EEFE
		public static DerBmpString GetInstance(object obj)
		{
			if (obj == null || obj is DerBmpString)
			{
				return (DerBmpString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x00170D28 File Offset: 0x0016EF28
		public static DerBmpString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBmpString)
			{
				return DerBmpString.GetInstance(@object);
			}
			return new DerBmpString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x00170D60 File Offset: 0x0016EF60
		public DerBmpString(byte[] str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			char[] array = new char[str.Length / 2];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = (char)((int)str[2 * num] << 8 | (int)(str[2 * num + 1] & byte.MaxValue));
			}
			this.str = new string(array);
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x00170DBF File Offset: 0x0016EFBF
		public DerBmpString(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x00170DDC File Offset: 0x0016EFDC
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x00170DE4 File Offset: 0x0016EFE4
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBmpString derBmpString = asn1Object as DerBmpString;
			return derBmpString != null && this.str.Equals(derBmpString.str);
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x00170E10 File Offset: 0x0016F010
		internal override void Encode(DerOutputStream derOut)
		{
			char[] array = this.str.ToCharArray();
			byte[] array2 = new byte[array.Length * 2];
			for (int num = 0; num != array.Length; num++)
			{
				array2[2 * num] = (byte)(array[num] >> 8);
				array2[2 * num + 1] = (byte)array[num];
			}
			derOut.WriteEncoded(30, array2);
		}

		// Token: 0x040026DC RID: 9948
		private readonly string str;
	}
}
