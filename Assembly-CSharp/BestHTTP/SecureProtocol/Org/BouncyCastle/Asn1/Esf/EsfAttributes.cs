using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x0200074B RID: 1867
	public abstract class EsfAttributes
	{
		// Token: 0x04002C31 RID: 11313
		public static readonly DerObjectIdentifier SigPolicyId = PkcsObjectIdentifiers.IdAAEtsSigPolicyID;

		// Token: 0x04002C32 RID: 11314
		public static readonly DerObjectIdentifier CommitmentType = PkcsObjectIdentifiers.IdAAEtsCommitmentType;

		// Token: 0x04002C33 RID: 11315
		public static readonly DerObjectIdentifier SignerLocation = PkcsObjectIdentifiers.IdAAEtsSignerLocation;

		// Token: 0x04002C34 RID: 11316
		public static readonly DerObjectIdentifier SignerAttr = PkcsObjectIdentifiers.IdAAEtsSignerAttr;

		// Token: 0x04002C35 RID: 11317
		public static readonly DerObjectIdentifier OtherSigCert = PkcsObjectIdentifiers.IdAAEtsOtherSigCert;

		// Token: 0x04002C36 RID: 11318
		public static readonly DerObjectIdentifier ContentTimestamp = PkcsObjectIdentifiers.IdAAEtsContentTimestamp;

		// Token: 0x04002C37 RID: 11319
		public static readonly DerObjectIdentifier CertificateRefs = PkcsObjectIdentifiers.IdAAEtsCertificateRefs;

		// Token: 0x04002C38 RID: 11320
		public static readonly DerObjectIdentifier RevocationRefs = PkcsObjectIdentifiers.IdAAEtsRevocationRefs;

		// Token: 0x04002C39 RID: 11321
		public static readonly DerObjectIdentifier CertValues = PkcsObjectIdentifiers.IdAAEtsCertValues;

		// Token: 0x04002C3A RID: 11322
		public static readonly DerObjectIdentifier RevocationValues = PkcsObjectIdentifiers.IdAAEtsRevocationValues;

		// Token: 0x04002C3B RID: 11323
		public static readonly DerObjectIdentifier EscTimeStamp = PkcsObjectIdentifiers.IdAAEtsEscTimeStamp;

		// Token: 0x04002C3C RID: 11324
		public static readonly DerObjectIdentifier CertCrlTimestamp = PkcsObjectIdentifiers.IdAAEtsCertCrlTimestamp;

		// Token: 0x04002C3D RID: 11325
		public static readonly DerObjectIdentifier ArchiveTimestamp = PkcsObjectIdentifiers.IdAAEtsArchiveTimestamp;

		// Token: 0x04002C3E RID: 11326
		public static readonly DerObjectIdentifier ArchiveTimestampV2 = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.48");
	}
}
