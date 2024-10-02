using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000658 RID: 1624
	public class DerOctetString : Asn1OctetString
	{
		// Token: 0x06003CC7 RID: 15559 RVA: 0x00172701 File Offset: 0x00170901
		public DerOctetString(byte[] str) : base(str)
		{
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x0017270A File Offset: 0x0017090A
		public DerOctetString(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00172713 File Offset: 0x00170913
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(4, this.str);
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x00172722 File Offset: 0x00170922
		internal static void Encode(DerOutputStream derOut, byte[] bytes, int offset, int length)
		{
			derOut.WriteEncoded(4, bytes, offset, length);
		}
	}
}
