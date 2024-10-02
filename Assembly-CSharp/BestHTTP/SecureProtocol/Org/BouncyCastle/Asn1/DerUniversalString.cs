using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000665 RID: 1637
	public class DerUniversalString : DerStringBase
	{
		// Token: 0x06003D13 RID: 15635 RVA: 0x00173087 File Offset: 0x00171287
		public static DerUniversalString GetInstance(object obj)
		{
			if (obj == null || obj is DerUniversalString)
			{
				return (DerUniversalString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x001730B0 File Offset: 0x001712B0
		public static DerUniversalString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerUniversalString)
			{
				return DerUniversalString.GetInstance(@object);
			}
			return new DerUniversalString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x001730E6 File Offset: 0x001712E6
		public DerUniversalString(byte[] str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x00173104 File Offset: 0x00171304
		public override string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder("#");
			byte[] derEncoded = base.GetDerEncoded();
			for (int num = 0; num != derEncoded.Length; num++)
			{
				uint num2 = (uint)derEncoded[num];
				stringBuilder.Append(DerUniversalString.table[(int)(num2 >> 4 & 15U)]);
				stringBuilder.Append(DerUniversalString.table[(int)(derEncoded[num] & 15)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00173161 File Offset: 0x00171361
		public byte[] GetOctets()
		{
			return (byte[])this.str.Clone();
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x00173173 File Offset: 0x00171373
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(28, this.str);
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x00173184 File Offset: 0x00171384
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerUniversalString derUniversalString = asn1Object as DerUniversalString;
			return derUniversalString != null && Arrays.AreEqual(this.str, derUniversalString.str);
		}

		// Token: 0x04002701 RID: 9985
		private static readonly char[] table = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		// Token: 0x04002702 RID: 9986
		private readonly byte[] str;
	}
}
