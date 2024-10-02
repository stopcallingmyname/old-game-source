using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000649 RID: 1609
	public class DerBitString : DerStringBase
	{
		// Token: 0x06003C21 RID: 15393 RVA: 0x001708C8 File Offset: 0x0016EAC8
		public static DerBitString GetInstance(object obj)
		{
			if (obj == null || obj is DerBitString)
			{
				return (DerBitString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerBitString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString());
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x00170940 File Offset: 0x0016EB40
		public static DerBitString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBitString)
			{
				return DerBitString.GetInstance(@object);
			}
			return DerBitString.FromAsn1Octets(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x00170978 File Offset: 0x0016EB78
		public DerBitString(byte[] data, int padBits)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (padBits < 0 || padBits > 7)
			{
				throw new ArgumentException("must be in the range 0 to 7", "padBits");
			}
			if (data.Length == 0 && padBits != 0)
			{
				throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");
			}
			this.mData = Arrays.Clone(data);
			this.mPadBits = padBits;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x001709D6 File Offset: 0x0016EBD6
		public DerBitString(byte[] data) : this(data, 0)
		{
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x001709E0 File Offset: 0x0016EBE0
		public DerBitString(int namedBits)
		{
			if (namedBits == 0)
			{
				this.mData = new byte[0];
				this.mPadBits = 0;
				return;
			}
			int num = (BigInteger.BitLen(namedBits) + 7) / 8;
			byte[] array = new byte[num];
			num--;
			for (int i = 0; i < num; i++)
			{
				array[i] = (byte)namedBits;
				namedBits >>= 8;
			}
			array[num] = (byte)namedBits;
			int num2 = 0;
			while ((namedBits & 1 << num2) == 0)
			{
				num2++;
			}
			this.mData = array;
			this.mPadBits = num2;
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x00170A5B File Offset: 0x0016EC5B
		public DerBitString(Asn1Encodable obj) : this(obj.GetDerEncoded())
		{
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x00170A69 File Offset: 0x0016EC69
		public virtual byte[] GetOctets()
		{
			if (this.mPadBits != 0)
			{
				throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
			}
			return Arrays.Clone(this.mData);
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x00170A8C File Offset: 0x0016EC8C
		public virtual byte[] GetBytes()
		{
			byte[] array = Arrays.Clone(this.mData);
			if (this.mPadBits > 0)
			{
				byte[] array2 = array;
				int num = array.Length - 1;
				array2[num] &= (byte)(255 << this.mPadBits);
			}
			return array;
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x00170ACF File Offset: 0x0016ECCF
		public virtual int PadBits
		{
			get
			{
				return this.mPadBits;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x00170AD8 File Offset: 0x0016ECD8
		public virtual int IntValue
		{
			get
			{
				int num = 0;
				int num2 = Math.Min(4, this.mData.Length);
				for (int i = 0; i < num2; i++)
				{
					num |= (int)this.mData[i] << 8 * i;
				}
				if (this.mPadBits > 0 && num2 == this.mData.Length)
				{
					int num3 = (1 << this.mPadBits) - 1;
					num &= ~(num3 << 8 * (num2 - 1));
				}
				return num;
			}
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x00170B48 File Offset: 0x0016ED48
		internal override void Encode(DerOutputStream derOut)
		{
			if (this.mPadBits > 0)
			{
				int num = (int)this.mData[this.mData.Length - 1];
				int num2 = (1 << this.mPadBits) - 1;
				int num3 = num & num2;
				if (num3 != 0)
				{
					byte[] array = Arrays.Prepend(this.mData, (byte)this.mPadBits);
					array[array.Length - 1] = (byte)(num ^ num3);
					derOut.WriteEncoded(3, array);
					return;
				}
			}
			derOut.WriteEncoded(3, (byte)this.mPadBits, this.mData);
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x00170BC0 File Offset: 0x0016EDC0
		protected override int Asn1GetHashCode()
		{
			return this.mPadBits.GetHashCode() ^ Arrays.GetHashCode(this.mData);
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x00170BE8 File Offset: 0x0016EDE8
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBitString derBitString = asn1Object as DerBitString;
			return derBitString != null && this.mPadBits == derBitString.mPadBits && Arrays.AreEqual(this.mData, derBitString.mData);
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x00170C24 File Offset: 0x0016EE24
		public override string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder("#");
			byte[] derEncoded = base.GetDerEncoded();
			for (int num = 0; num != derEncoded.Length; num++)
			{
				uint num2 = (uint)derEncoded[num];
				stringBuilder.Append(DerBitString.table[(int)(num2 >> 4 & 15U)]);
				stringBuilder.Append(DerBitString.table[(int)(derEncoded[num] & 15)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x00170C84 File Offset: 0x0016EE84
		internal static DerBitString FromAsn1Octets(byte[] octets)
		{
			if (octets.Length < 1)
			{
				throw new ArgumentException("truncated BIT STRING detected", "octets");
			}
			int num = (int)octets[0];
			byte[] array = Arrays.CopyOfRange(octets, 1, octets.Length);
			if (num > 0 && num < 8 && array.Length != 0)
			{
				bool flag = array[array.Length - 1] != 0;
				int num2 = (1 << num) - 1;
				if (((flag ? 1 : 0) & num2) != 0)
				{
					return new BerBitString(array, num);
				}
			}
			return new DerBitString(array, num);
		}

		// Token: 0x040026D9 RID: 9945
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

		// Token: 0x040026DA RID: 9946
		protected readonly byte[] mData;

		// Token: 0x040026DB RID: 9947
		protected readonly int mPadBits;
	}
}
