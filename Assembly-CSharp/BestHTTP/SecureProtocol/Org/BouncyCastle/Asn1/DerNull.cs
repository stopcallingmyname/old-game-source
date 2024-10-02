using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000655 RID: 1621
	public class DerNull : Asn1Null
	{
		// Token: 0x06003CA2 RID: 15522 RVA: 0x00171FCB File Offset: 0x001701CB
		[Obsolete("Use static Instance object")]
		public DerNull()
		{
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x00171FCB File Offset: 0x001701CB
		protected internal DerNull(int dummy)
		{
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x00171FDF File Offset: 0x001701DF
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(5, this.zeroBytes);
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x00171FEE File Offset: 0x001701EE
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			return asn1Object is DerNull;
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0008D54D File Offset: 0x0008B74D
		protected override int Asn1GetHashCode()
		{
			return -1;
		}

		// Token: 0x040026F1 RID: 9969
		public static readonly DerNull Instance = new DerNull(0);

		// Token: 0x040026F2 RID: 9970
		private byte[] zeroBytes = new byte[0];
	}
}
