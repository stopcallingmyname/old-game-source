using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000668 RID: 1640
	public class DerVideotexString : DerStringBase
	{
		// Token: 0x06003D32 RID: 15666 RVA: 0x001735B8 File Offset: 0x001717B8
		public static DerVideotexString GetInstance(object obj)
		{
			if (obj == null || obj is DerVideotexString)
			{
				return (DerVideotexString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerVideotexString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString(), "obj");
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x0017363C File Offset: 0x0017183C
		public static DerVideotexString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerVideotexString)
			{
				return DerVideotexString.GetInstance(@object);
			}
			return new DerVideotexString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00173672 File Offset: 0x00171872
		public DerVideotexString(byte[] encoding)
		{
			this.mString = Arrays.Clone(encoding);
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00173686 File Offset: 0x00171886
		public override string GetString()
		{
			return Strings.FromByteArray(this.mString);
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00173693 File Offset: 0x00171893
		public byte[] GetOctets()
		{
			return Arrays.Clone(this.mString);
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x001736A0 File Offset: 0x001718A0
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(21, this.mString);
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x001736B0 File Offset: 0x001718B0
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.mString);
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x001736C0 File Offset: 0x001718C0
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerVideotexString derVideotexString = asn1Object as DerVideotexString;
			return derVideotexString != null && Arrays.AreEqual(this.mString, derVideotexString.mString);
		}

		// Token: 0x04002705 RID: 9989
		private readonly byte[] mString;
	}
}
