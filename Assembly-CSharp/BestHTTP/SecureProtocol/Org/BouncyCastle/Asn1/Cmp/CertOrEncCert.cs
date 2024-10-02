using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007AD RID: 1965
	public class CertOrEncCert : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600462D RID: 17965 RVA: 0x001928EC File Offset: 0x00190AEC
		private CertOrEncCert(Asn1TaggedObject tagged)
		{
			if (tagged.TagNo == 0)
			{
				this.certificate = CmpCertificate.GetInstance(tagged.GetObject());
				return;
			}
			if (tagged.TagNo == 1)
			{
				this.encryptedCert = EncryptedValue.GetInstance(tagged.GetObject());
				return;
			}
			throw new ArgumentException("unknown tag: " + tagged.TagNo, "tagged");
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00192953 File Offset: 0x00190B53
		public static CertOrEncCert GetInstance(object obj)
		{
			if (obj is CertOrEncCert)
			{
				return (CertOrEncCert)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new CertOrEncCert((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x00192992 File Offset: 0x00190B92
		public CertOrEncCert(CmpCertificate certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			this.certificate = certificate;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x001929AF File Offset: 0x00190BAF
		public CertOrEncCert(EncryptedValue encryptedCert)
		{
			if (encryptedCert == null)
			{
				throw new ArgumentNullException("encryptedCert");
			}
			this.encryptedCert = encryptedCert;
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x001929CC File Offset: 0x00190BCC
		public virtual CmpCertificate Certificate
		{
			get
			{
				return this.certificate;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x001929D4 File Offset: 0x00190BD4
		public virtual EncryptedValue EncryptedCert
		{
			get
			{
				return this.encryptedCert;
			}
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x001929DC File Offset: 0x00190BDC
		public override Asn1Object ToAsn1Object()
		{
			if (this.certificate != null)
			{
				return new DerTaggedObject(true, 0, this.certificate);
			}
			return new DerTaggedObject(true, 1, this.encryptedCert);
		}

		// Token: 0x04002DA7 RID: 11687
		private readonly CmpCertificate certificate;

		// Token: 0x04002DA8 RID: 11688
		private readonly EncryptedValue encryptedCert;
	}
}
