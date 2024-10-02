using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000639 RID: 1593
	public class BerNull : DerNull
	{
		// Token: 0x06003BCC RID: 15308 RVA: 0x0016FA31 File Offset: 0x0016DC31
		[Obsolete("Use static Instance object")]
		public BerNull()
		{
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x0016FA39 File Offset: 0x0016DC39
		private BerNull(int dummy) : base(dummy)
		{
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x0016FA42 File Offset: 0x0016DC42
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(5);
				return;
			}
			base.Encode(derOut);
		}

		// Token: 0x040026C5 RID: 9925
		public new static readonly BerNull Instance = new BerNull(0);
	}
}
