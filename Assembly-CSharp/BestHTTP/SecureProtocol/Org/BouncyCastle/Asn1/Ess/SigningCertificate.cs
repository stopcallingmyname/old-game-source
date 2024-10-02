using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x0200073F RID: 1855
	public class SigningCertificate : Asn1Encodable
	{
		// Token: 0x06004318 RID: 17176 RVA: 0x00188F2C File Offset: 0x0018712C
		public static SigningCertificate GetInstance(object o)
		{
			if (o == null || o is SigningCertificate)
			{
				return (SigningCertificate)o;
			}
			if (o is Asn1Sequence)
			{
				return new SigningCertificate((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in 'SigningCertificate' factory : " + Platform.GetTypeName(o) + ".");
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x00188F7C File Offset: 0x0018717C
		public SigningCertificate(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.certs = Asn1Sequence.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.policies = Asn1Sequence.GetInstance(seq[1]);
			}
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x00188FE9 File Offset: 0x001871E9
		public SigningCertificate(EssCertID essCertID)
		{
			this.certs = new DerSequence(essCertID);
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00189000 File Offset: 0x00187200
		public EssCertID[] GetCerts()
		{
			EssCertID[] array = new EssCertID[this.certs.Count];
			for (int num = 0; num != this.certs.Count; num++)
			{
				array[num] = EssCertID.GetInstance(this.certs[num]);
			}
			return array;
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x0018904C File Offset: 0x0018724C
		public PolicyInformation[] GetPolicies()
		{
			if (this.policies == null)
			{
				return null;
			}
			PolicyInformation[] array = new PolicyInformation[this.policies.Count];
			for (int num = 0; num != this.policies.Count; num++)
			{
				array[num] = PolicyInformation.GetInstance(this.policies[num]);
			}
			return array;
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x001890A0 File Offset: 0x001872A0
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

		// Token: 0x04002C17 RID: 11287
		private Asn1Sequence certs;

		// Token: 0x04002C18 RID: 11288
		private Asn1Sequence policies;
	}
}
