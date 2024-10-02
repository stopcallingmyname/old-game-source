using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A4 RID: 1700
	public class KeyUsage : DerBitString
	{
		// Token: 0x06003ED8 RID: 16088 RVA: 0x00178A58 File Offset: 0x00176C58
		public new static KeyUsage GetInstance(object obj)
		{
			if (obj is KeyUsage)
			{
				return (KeyUsage)obj;
			}
			if (obj is X509Extension)
			{
				return KeyUsage.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			return new KeyUsage(DerBitString.GetInstance(obj));
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0016F8CD File Offset: 0x0016DACD
		public KeyUsage(int usage) : base(usage)
		{
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x00178A8D File Offset: 0x00176C8D
		private KeyUsage(DerBitString usage) : base(usage.GetBytes(), usage.PadBits)
		{
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x00178AA4 File Offset: 0x00176CA4
		public override string ToString()
		{
			byte[] bytes = this.GetBytes();
			if (bytes.Length == 1)
			{
				return "KeyUsage: 0x" + ((int)(bytes[0] & byte.MaxValue)).ToString("X");
			}
			return "KeyUsage: 0x" + ((int)(bytes[1] & byte.MaxValue) << 8 | (int)(bytes[0] & byte.MaxValue)).ToString("X");
		}

		// Token: 0x040027E8 RID: 10216
		public const int DigitalSignature = 128;

		// Token: 0x040027E9 RID: 10217
		public const int NonRepudiation = 64;

		// Token: 0x040027EA RID: 10218
		public const int KeyEncipherment = 32;

		// Token: 0x040027EB RID: 10219
		public const int DataEncipherment = 16;

		// Token: 0x040027EC RID: 10220
		public const int KeyAgreement = 8;

		// Token: 0x040027ED RID: 10221
		public const int KeyCertSign = 4;

		// Token: 0x040027EE RID: 10222
		public const int CrlSign = 2;

		// Token: 0x040027EF RID: 10223
		public const int EncipherOnly = 1;

		// Token: 0x040027F0 RID: 10224
		public const int DecipherOnly = 32768;
	}
}
