using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200066F RID: 1647
	public class LazyAsn1InputStream : Asn1InputStream
	{
		// Token: 0x06003D4C RID: 15692 RVA: 0x001738FB File Offset: 0x00171AFB
		public LazyAsn1InputStream(byte[] input) : base(input)
		{
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00173904 File Offset: 0x00171B04
		public LazyAsn1InputStream(Stream inputStream) : base(inputStream)
		{
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x0017390D File Offset: 0x00171B0D
		internal override DerSequence CreateDerSequence(DefiniteLengthInputStream dIn)
		{
			return new LazyDerSequence(dIn.ToArray());
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x0017391A File Offset: 0x00171B1A
		internal override DerSet CreateDerSet(DefiniteLengthInputStream dIn)
		{
			return new LazyDerSet(dIn.ToArray());
		}
	}
}
