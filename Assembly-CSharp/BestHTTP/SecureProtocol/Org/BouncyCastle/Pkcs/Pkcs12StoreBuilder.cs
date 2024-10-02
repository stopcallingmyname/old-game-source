using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002CC RID: 716
	public class Pkcs12StoreBuilder
	{
		// Token: 0x06001A68 RID: 6760 RVA: 0x000C656A File Offset: 0x000C476A
		public Pkcs12Store Build()
		{
			return new Pkcs12Store(this.keyAlgorithm, this.certAlgorithm, this.useDerEncoding);
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x000C6583 File Offset: 0x000C4783
		public Pkcs12StoreBuilder SetCertAlgorithm(DerObjectIdentifier certAlgorithm)
		{
			this.certAlgorithm = certAlgorithm;
			return this;
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x000C658D File Offset: 0x000C478D
		public Pkcs12StoreBuilder SetKeyAlgorithm(DerObjectIdentifier keyAlgorithm)
		{
			this.keyAlgorithm = keyAlgorithm;
			return this;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x000C6597 File Offset: 0x000C4797
		public Pkcs12StoreBuilder SetUseDerEncoding(bool useDerEncoding)
		{
			this.useDerEncoding = useDerEncoding;
			return this;
		}

		// Token: 0x040018C8 RID: 6344
		private DerObjectIdentifier keyAlgorithm = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc;

		// Token: 0x040018C9 RID: 6345
		private DerObjectIdentifier certAlgorithm = PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc;

		// Token: 0x040018CA RID: 6346
		private bool useDerEncoding;
	}
}
