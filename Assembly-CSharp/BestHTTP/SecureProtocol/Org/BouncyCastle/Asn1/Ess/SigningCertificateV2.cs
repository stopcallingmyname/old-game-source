using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x02000740 RID: 1856
	public class SigningCertificateV2 : Asn1Encodable
	{
		// Token: 0x0600431E RID: 17182 RVA: 0x001890E8 File Offset: 0x001872E8
		public static SigningCertificateV2 GetInstance(object o)
		{
			if (o == null || o is SigningCertificateV2)
			{
				return (SigningCertificateV2)o;
			}
			if (o is Asn1Sequence)
			{
				return new SigningCertificateV2((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in 'SigningCertificateV2' factory : " + Platform.GetTypeName(o) + ".");
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x00189138 File Offset: 0x00187338
		private SigningCertificateV2(Asn1Sequence seq)
		{
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

		// Token: 0x06004320 RID: 17184 RVA: 0x001891B4 File Offset: 0x001873B4
		public SigningCertificateV2(EssCertIDv2 cert)
		{
			this.certs = new DerSequence(cert);
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x001891C8 File Offset: 0x001873C8
		public SigningCertificateV2(EssCertIDv2[] certs)
		{
			this.certs = new DerSequence(certs);
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x001891EC File Offset: 0x001873EC
		public SigningCertificateV2(EssCertIDv2[] certs, PolicyInformation[] policies)
		{
			this.certs = new DerSequence(certs);
			if (policies != null)
			{
				this.policies = new DerSequence(policies);
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x00189220 File Offset: 0x00187420
		public EssCertIDv2[] GetCerts()
		{
			EssCertIDv2[] array = new EssCertIDv2[this.certs.Count];
			for (int num = 0; num != this.certs.Count; num++)
			{
				array[num] = EssCertIDv2.GetInstance(this.certs[num]);
			}
			return array;
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x0018926C File Offset: 0x0018746C
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

		// Token: 0x06004325 RID: 17189 RVA: 0x001892C0 File Offset: 0x001874C0
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

		// Token: 0x04002C19 RID: 11289
		private readonly Asn1Sequence certs;

		// Token: 0x04002C1A RID: 11290
		private readonly Asn1Sequence policies;
	}
}
