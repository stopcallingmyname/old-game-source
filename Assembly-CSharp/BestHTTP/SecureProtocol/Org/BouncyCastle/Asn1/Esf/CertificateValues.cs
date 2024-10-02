using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000741 RID: 1857
	public class CertificateValues : Asn1Encodable
	{
		// Token: 0x06004326 RID: 17190 RVA: 0x00189308 File Offset: 0x00187508
		public static CertificateValues GetInstance(object obj)
		{
			if (obj == null || obj is CertificateValues)
			{
				return (CertificateValues)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertificateValues((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CertificateValues' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x00189358 File Offset: 0x00187558
		private CertificateValues(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			foreach (object obj in seq)
			{
				X509CertificateStructure.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
			}
			this.certificates = seq;
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x001893CC File Offset: 0x001875CC
		public CertificateValues(params X509CertificateStructure[] certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			this.certificates = new DerSequence(certificates);
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x001893FC File Offset: 0x001875FC
		public CertificateValues(IEnumerable certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			if (!CollectionUtilities.CheckElementsAreOfType(certificates, typeof(X509CertificateStructure)))
			{
				throw new ArgumentException("Must contain only 'X509CertificateStructure' objects", "certificates");
			}
			this.certificates = new DerSequence(Asn1EncodableVector.FromEnumerable(certificates));
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x00189450 File Offset: 0x00187650
		public X509CertificateStructure[] GetCertificates()
		{
			X509CertificateStructure[] array = new X509CertificateStructure[this.certificates.Count];
			for (int i = 0; i < this.certificates.Count; i++)
			{
				array[i] = X509CertificateStructure.GetInstance(this.certificates[i]);
			}
			return array;
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x00189499 File Offset: 0x00187699
		public override Asn1Object ToAsn1Object()
		{
			return this.certificates;
		}

		// Token: 0x04002C1B RID: 11291
		private readonly Asn1Sequence certificates;
	}
}
