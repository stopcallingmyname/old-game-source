using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000637 RID: 1591
	public class BerBitString : DerBitString
	{
		// Token: 0x06003BBE RID: 15294 RVA: 0x0016F8BA File Offset: 0x0016DABA
		public BerBitString(byte[] data, int padBits) : base(data, padBits)
		{
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x0016F8C4 File Offset: 0x0016DAC4
		public BerBitString(byte[] data) : base(data)
		{
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x0016F8CD File Offset: 0x0016DACD
		public BerBitString(int namedBits) : base(namedBits)
		{
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x0016F8D6 File Offset: 0x0016DAD6
		public BerBitString(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x0016F8DF File Offset: 0x0016DADF
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteEncoded(3, (byte)this.mPadBits, this.mData);
				return;
			}
			base.Encode(derOut);
		}
	}
}
