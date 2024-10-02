using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000601 RID: 1537
	public class CmsSignedGenerator
	{
		// Token: 0x06003A6D RID: 14957 RVA: 0x0016A737 File Offset: 0x00168937
		protected CmsSignedGenerator() : this(new SecureRandom())
		{
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x0016A744 File Offset: 0x00168944
		protected CmsSignedGenerator(SecureRandom rand)
		{
			this.rand = rand;
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x0016A780 File Offset: 0x00168980
		protected internal virtual IDictionary GetBaseParameters(DerObjectIdentifier contentType, AlgorithmIdentifier digAlgId, byte[] hash)
		{
			IDictionary dictionary = Platform.CreateHashtable();
			if (contentType != null)
			{
				dictionary[CmsAttributeTableParameter.ContentType] = contentType;
			}
			dictionary[CmsAttributeTableParameter.DigestAlgorithmIdentifier] = digAlgId;
			dictionary[CmsAttributeTableParameter.Digest] = hash.Clone();
			return dictionary;
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x0016A7C3 File Offset: 0x001689C3
		protected internal virtual Asn1Set GetAttributeSet(BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable attr)
		{
			if (attr != null)
			{
				return new DerSet(attr.ToAsn1EncodableVector());
			}
			return null;
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x0016A7D5 File Offset: 0x001689D5
		public void AddCertificates(IX509Store certStore)
		{
			CollectionUtilities.AddRange(this._certs, CmsUtilities.GetCertificatesFromStore(certStore));
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x0016A7E8 File Offset: 0x001689E8
		public void AddCrls(IX509Store crlStore)
		{
			CollectionUtilities.AddRange(this._crls, CmsUtilities.GetCrlsFromStore(crlStore));
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x0016A7FC File Offset: 0x001689FC
		public void AddAttributeCertificates(IX509Store store)
		{
			try
			{
				foreach (object obj in store.GetMatches(null))
				{
					IX509AttributeCertificate ix509AttributeCertificate = (IX509AttributeCertificate)obj;
					this._certs.Add(new DerTaggedObject(false, 2, AttributeCertificate.GetInstance(Asn1Object.FromByteArray(ix509AttributeCertificate.GetEncoded()))));
				}
			}
			catch (Exception e)
			{
				throw new CmsException("error processing attribute certs", e);
			}
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x0016A890 File Offset: 0x00168A90
		public void AddSigners(SignerInformationStore signerStore)
		{
			foreach (object obj in signerStore.GetSigners())
			{
				SignerInformation signerInformation = (SignerInformation)obj;
				this._signers.Add(signerInformation);
				this.AddSignerCallback(signerInformation);
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x0016A8F8 File Offset: 0x00168AF8
		public IDictionary GetGeneratedDigests()
		{
			return Platform.CreateHashtable(this._digests);
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06003A76 RID: 14966 RVA: 0x0016A905 File Offset: 0x00168B05
		// (set) Token: 0x06003A77 RID: 14967 RVA: 0x0016A90D File Offset: 0x00168B0D
		public bool UseDerForCerts
		{
			get
			{
				return this._useDerForCerts;
			}
			set
			{
				this._useDerForCerts = value;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06003A78 RID: 14968 RVA: 0x0016A916 File Offset: 0x00168B16
		// (set) Token: 0x06003A79 RID: 14969 RVA: 0x0016A91E File Offset: 0x00168B1E
		public bool UseDerForCrls
		{
			get
			{
				return this._useDerForCrls;
			}
			set
			{
				this._useDerForCrls = value;
			}
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x0000248C File Offset: 0x0000068C
		internal virtual void AddSignerCallback(SignerInformation si)
		{
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x0016A927 File Offset: 0x00168B27
		internal static SignerIdentifier GetSignerIdentifier(X509Certificate cert)
		{
			return new SignerIdentifier(CmsUtilities.GetIssuerAndSerialNumber(cert));
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x0016A934 File Offset: 0x00168B34
		internal static SignerIdentifier GetSignerIdentifier(byte[] subjectKeyIdentifier)
		{
			return new SignerIdentifier(new DerOctetString(subjectKeyIdentifier));
		}

		// Token: 0x0400262E RID: 9774
		public static readonly string Data = CmsObjectIdentifiers.Data.Id;

		// Token: 0x0400262F RID: 9775
		public static readonly string DigestSha1 = OiwObjectIdentifiers.IdSha1.Id;

		// Token: 0x04002630 RID: 9776
		public static readonly string DigestSha224 = NistObjectIdentifiers.IdSha224.Id;

		// Token: 0x04002631 RID: 9777
		public static readonly string DigestSha256 = NistObjectIdentifiers.IdSha256.Id;

		// Token: 0x04002632 RID: 9778
		public static readonly string DigestSha384 = NistObjectIdentifiers.IdSha384.Id;

		// Token: 0x04002633 RID: 9779
		public static readonly string DigestSha512 = NistObjectIdentifiers.IdSha512.Id;

		// Token: 0x04002634 RID: 9780
		public static readonly string DigestMD5 = PkcsObjectIdentifiers.MD5.Id;

		// Token: 0x04002635 RID: 9781
		public static readonly string DigestGost3411 = CryptoProObjectIdentifiers.GostR3411.Id;

		// Token: 0x04002636 RID: 9782
		public static readonly string DigestRipeMD128 = TeleTrusTObjectIdentifiers.RipeMD128.Id;

		// Token: 0x04002637 RID: 9783
		public static readonly string DigestRipeMD160 = TeleTrusTObjectIdentifiers.RipeMD160.Id;

		// Token: 0x04002638 RID: 9784
		public static readonly string DigestRipeMD256 = TeleTrusTObjectIdentifiers.RipeMD256.Id;

		// Token: 0x04002639 RID: 9785
		public static readonly string EncryptionRsa = PkcsObjectIdentifiers.RsaEncryption.Id;

		// Token: 0x0400263A RID: 9786
		public static readonly string EncryptionDsa = X9ObjectIdentifiers.IdDsaWithSha1.Id;

		// Token: 0x0400263B RID: 9787
		public static readonly string EncryptionECDsa = X9ObjectIdentifiers.ECDsaWithSha1.Id;

		// Token: 0x0400263C RID: 9788
		public static readonly string EncryptionRsaPss = PkcsObjectIdentifiers.IdRsassaPss.Id;

		// Token: 0x0400263D RID: 9789
		public static readonly string EncryptionGost3410 = CryptoProObjectIdentifiers.GostR3410x94.Id;

		// Token: 0x0400263E RID: 9790
		public static readonly string EncryptionECGost3410 = CryptoProObjectIdentifiers.GostR3410x2001.Id;

		// Token: 0x0400263F RID: 9791
		internal IList _certs = Platform.CreateArrayList();

		// Token: 0x04002640 RID: 9792
		internal IList _crls = Platform.CreateArrayList();

		// Token: 0x04002641 RID: 9793
		internal IList _signers = Platform.CreateArrayList();

		// Token: 0x04002642 RID: 9794
		internal IDictionary _digests = Platform.CreateHashtable();

		// Token: 0x04002643 RID: 9795
		internal bool _useDerForCerts;

		// Token: 0x04002644 RID: 9796
		internal bool _useDerForCrls;

		// Token: 0x04002645 RID: 9797
		protected readonly SecureRandom rand;
	}
}
