using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000754 RID: 1876
	public class OtherSigningCertificate : Asn1Encodable
	{
		// Token: 0x0600439A RID: 17306 RVA: 0x0018ACD4 File Offset: 0x00188ED4
		public static OtherSigningCertificate GetInstance(object obj)
		{
			if (obj == null || obj is OtherSigningCertificate)
			{
				return (OtherSigningCertificate)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OtherSigningCertificate((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OtherSigningCertificate' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0018AD24 File Offset: 0x00188F24
		private OtherSigningCertificate(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.certs = Asn1Sequence.GetInstance(seq[0].ToAsn1Object());
			if (seq.Count > 1)
			{
				this.policies = Asn1Sequence.GetInstance(seq[1].ToAsn1Object());
			}
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0018ADAE File Offset: 0x00188FAE
		public OtherSigningCertificate(params OtherCertID[] certs) : this(certs, null)
		{
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x0018ADB8 File Offset: 0x00188FB8
		public OtherSigningCertificate(OtherCertID[] certs, params PolicyInformation[] policies)
		{
			if (certs == null)
			{
				throw new ArgumentNullException("certs");
			}
			this.certs = new DerSequence(certs);
			if (policies != null)
			{
				this.policies = new DerSequence(policies);
			}
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0018ADF8 File Offset: 0x00188FF8
		public OtherSigningCertificate(IEnumerable certs) : this(certs, null)
		{
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0018AE04 File Offset: 0x00189004
		public OtherSigningCertificate(IEnumerable certs, IEnumerable policies)
		{
			if (certs == null)
			{
				throw new ArgumentNullException("certs");
			}
			if (!CollectionUtilities.CheckElementsAreOfType(certs, typeof(OtherCertID)))
			{
				throw new ArgumentException("Must contain only 'OtherCertID' objects", "certs");
			}
			this.certs = new DerSequence(Asn1EncodableVector.FromEnumerable(certs));
			if (policies != null)
			{
				if (!CollectionUtilities.CheckElementsAreOfType(policies, typeof(PolicyInformation)))
				{
					throw new ArgumentException("Must contain only 'PolicyInformation' objects", "policies");
				}
				this.policies = new DerSequence(Asn1EncodableVector.FromEnumerable(policies));
			}
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0018AE90 File Offset: 0x00189090
		public OtherCertID[] GetCerts()
		{
			OtherCertID[] array = new OtherCertID[this.certs.Count];
			for (int i = 0; i < this.certs.Count; i++)
			{
				array[i] = OtherCertID.GetInstance(this.certs[i].ToAsn1Object());
			}
			return array;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0018AEE0 File Offset: 0x001890E0
		public PolicyInformation[] GetPolicies()
		{
			if (this.policies == null)
			{
				return null;
			}
			PolicyInformation[] array = new PolicyInformation[this.policies.Count];
			for (int i = 0; i < this.policies.Count; i++)
			{
				array[i] = PolicyInformation.GetInstance(this.policies[i].ToAsn1Object());
			}
			return array;
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x0018AF38 File Offset: 0x00189138
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certs
			});
			if (this.policies != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.policies
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C4E RID: 11342
		private readonly Asn1Sequence certs;

		// Token: 0x04002C4F RID: 11343
		private readonly Asn1Sequence policies;
	}
}
