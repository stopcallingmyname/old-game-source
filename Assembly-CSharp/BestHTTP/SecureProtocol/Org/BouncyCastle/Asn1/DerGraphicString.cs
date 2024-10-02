using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000652 RID: 1618
	public class DerGraphicString : DerStringBase
	{
		// Token: 0x06003C83 RID: 15491 RVA: 0x00171BB8 File Offset: 0x0016FDB8
		public static DerGraphicString GetInstance(object obj)
		{
			if (obj == null || obj is DerGraphicString)
			{
				return (DerGraphicString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerGraphicString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString(), "obj");
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x00171C3C File Offset: 0x0016FE3C
		public static DerGraphicString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGraphicString)
			{
				return DerGraphicString.GetInstance(@object);
			}
			return new DerGraphicString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x00171C72 File Offset: 0x0016FE72
		public DerGraphicString(byte[] encoding)
		{
			this.mString = Arrays.Clone(encoding);
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00171C86 File Offset: 0x0016FE86
		public override string GetString()
		{
			return Strings.FromByteArray(this.mString);
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x00171C93 File Offset: 0x0016FE93
		public byte[] GetOctets()
		{
			return Arrays.Clone(this.mString);
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x00171CA0 File Offset: 0x0016FEA0
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(25, this.mString);
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x00171CB0 File Offset: 0x0016FEB0
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.mString);
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x00171CC0 File Offset: 0x0016FEC0
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGraphicString derGraphicString = asn1Object as DerGraphicString;
			return derGraphicString != null && Arrays.AreEqual(this.mString, derGraphicString.mString);
		}

		// Token: 0x040026ED RID: 9965
		private readonly byte[] mString;
	}
}
