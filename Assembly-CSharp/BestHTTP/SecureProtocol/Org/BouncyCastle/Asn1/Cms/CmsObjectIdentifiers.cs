using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000782 RID: 1922
	public abstract class CmsObjectIdentifiers
	{
		// Token: 0x04002D1C RID: 11548
		public static readonly DerObjectIdentifier Data = PkcsObjectIdentifiers.Data;

		// Token: 0x04002D1D RID: 11549
		public static readonly DerObjectIdentifier SignedData = PkcsObjectIdentifiers.SignedData;

		// Token: 0x04002D1E RID: 11550
		public static readonly DerObjectIdentifier EnvelopedData = PkcsObjectIdentifiers.EnvelopedData;

		// Token: 0x04002D1F RID: 11551
		public static readonly DerObjectIdentifier SignedAndEnvelopedData = PkcsObjectIdentifiers.SignedAndEnvelopedData;

		// Token: 0x04002D20 RID: 11552
		public static readonly DerObjectIdentifier DigestedData = PkcsObjectIdentifiers.DigestedData;

		// Token: 0x04002D21 RID: 11553
		public static readonly DerObjectIdentifier EncryptedData = PkcsObjectIdentifiers.EncryptedData;

		// Token: 0x04002D22 RID: 11554
		public static readonly DerObjectIdentifier AuthenticatedData = PkcsObjectIdentifiers.IdCTAuthData;

		// Token: 0x04002D23 RID: 11555
		public static readonly DerObjectIdentifier CompressedData = PkcsObjectIdentifiers.IdCTCompressedData;

		// Token: 0x04002D24 RID: 11556
		public static readonly DerObjectIdentifier AuthEnvelopedData = PkcsObjectIdentifiers.IdCTAuthEnvelopedData;

		// Token: 0x04002D25 RID: 11557
		public static readonly DerObjectIdentifier timestampedData = PkcsObjectIdentifiers.IdCTTimestampedData;

		// Token: 0x04002D26 RID: 11558
		public static readonly DerObjectIdentifier id_ri = new DerObjectIdentifier("1.3.6.1.5.5.7.16");

		// Token: 0x04002D27 RID: 11559
		public static readonly DerObjectIdentifier id_ri_ocsp_response = CmsObjectIdentifiers.id_ri.Branch("2");

		// Token: 0x04002D28 RID: 11560
		public static readonly DerObjectIdentifier id_ri_scvp = CmsObjectIdentifiers.id_ri.Branch("4");
	}
}
