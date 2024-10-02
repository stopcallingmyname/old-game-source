using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000648 RID: 1608
	public class DerApplicationSpecific : Asn1Object
	{
		// Token: 0x06003C12 RID: 15378 RVA: 0x001705E3 File Offset: 0x0016E7E3
		internal DerApplicationSpecific(bool isConstructed, int tag, byte[] octets)
		{
			this.isConstructed = isConstructed;
			this.tag = tag;
			this.octets = octets;
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x00170600 File Offset: 0x0016E800
		public DerApplicationSpecific(int tag, byte[] octets) : this(false, tag, octets)
		{
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x0017060B File Offset: 0x0016E80B
		public DerApplicationSpecific(int tag, Asn1Encodable obj) : this(true, tag, obj)
		{
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x00170618 File Offset: 0x0016E818
		public DerApplicationSpecific(bool isExplicit, int tag, Asn1Encodable obj)
		{
			Asn1Object asn1Object = obj.ToAsn1Object();
			byte[] derEncoded = asn1Object.GetDerEncoded();
			this.isConstructed = Asn1TaggedObject.IsConstructed(isExplicit, asn1Object);
			this.tag = tag;
			if (isExplicit)
			{
				this.octets = derEncoded;
				return;
			}
			int lengthOfHeader = this.GetLengthOfHeader(derEncoded);
			byte[] array = new byte[derEncoded.Length - lengthOfHeader];
			Array.Copy(derEncoded, lengthOfHeader, array, 0, array.Length);
			this.octets = array;
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x00170680 File Offset: 0x0016E880
		public DerApplicationSpecific(int tagNo, Asn1EncodableVector vec)
		{
			this.tag = tagNo;
			this.isConstructed = true;
			MemoryStream memoryStream = new MemoryStream();
			for (int num = 0; num != vec.Count; num++)
			{
				try
				{
					byte[] derEncoded = vec[num].GetDerEncoded();
					memoryStream.Write(derEncoded, 0, derEncoded.Length);
				}
				catch (IOException innerException)
				{
					throw new InvalidOperationException("malformed object", innerException);
				}
			}
			this.octets = memoryStream.ToArray();
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x001706FC File Offset: 0x0016E8FC
		private int GetLengthOfHeader(byte[] data)
		{
			int num = (int)data[1];
			if (num == 128)
			{
				return 2;
			}
			if (num <= 127)
			{
				return 2;
			}
			int num2 = num & 127;
			if (num2 > 4)
			{
				throw new InvalidOperationException("DER length more than 4 bytes: " + num2);
			}
			return num2 + 2;
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00170740 File Offset: 0x0016E940
		public bool IsConstructed()
		{
			return this.isConstructed;
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x00170748 File Offset: 0x0016E948
		public byte[] GetContents()
		{
			return this.octets;
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x00170750 File Offset: 0x0016E950
		public int ApplicationTag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x00170758 File Offset: 0x0016E958
		public Asn1Object GetObject()
		{
			return Asn1Object.FromByteArray(this.GetContents());
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x00170768 File Offset: 0x0016E968
		public Asn1Object GetObject(int derTagNo)
		{
			if (derTagNo >= 31)
			{
				throw new IOException("unsupported tag number");
			}
			byte[] encoded = base.GetEncoded();
			byte[] array = this.ReplaceTagNumber(derTagNo, encoded);
			if ((encoded[0] & 32) != 0)
			{
				byte[] array2 = array;
				int num = 0;
				array2[num] |= 32;
			}
			return Asn1Object.FromByteArray(array);
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x001707B4 File Offset: 0x0016E9B4
		internal override void Encode(DerOutputStream derOut)
		{
			int num = 64;
			if (this.isConstructed)
			{
				num |= 32;
			}
			derOut.WriteEncoded(num, this.tag, this.octets);
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x001707E4 File Offset: 0x0016E9E4
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerApplicationSpecific derApplicationSpecific = asn1Object as DerApplicationSpecific;
			return derApplicationSpecific != null && (this.isConstructed == derApplicationSpecific.isConstructed && this.tag == derApplicationSpecific.tag) && Arrays.AreEqual(this.octets, derApplicationSpecific.octets);
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x0017082C File Offset: 0x0016EA2C
		protected override int Asn1GetHashCode()
		{
			return this.isConstructed.GetHashCode() ^ this.tag.GetHashCode() ^ Arrays.GetHashCode(this.octets);
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x00170864 File Offset: 0x0016EA64
		private byte[] ReplaceTagNumber(int newTag, byte[] input)
		{
			int num = (int)(input[0] & 31);
			int num2 = 1;
			if (num == 31)
			{
				int num3 = (int)input[num2++];
				if ((num3 & 127) == 0)
				{
					throw new IOException("corrupted stream - invalid high tag number found");
				}
				while ((num3 & 128) != 0)
				{
					num3 = (int)input[num2++];
				}
			}
			int num4 = input.Length - num2;
			byte[] array = new byte[1 + num4];
			array[0] = (byte)newTag;
			Array.Copy(input, num2, array, 1, num4);
			return array;
		}

		// Token: 0x040026D6 RID: 9942
		private readonly bool isConstructed;

		// Token: 0x040026D7 RID: 9943
		private readonly int tag;

		// Token: 0x040026D8 RID: 9944
		private readonly byte[] octets;
	}
}
