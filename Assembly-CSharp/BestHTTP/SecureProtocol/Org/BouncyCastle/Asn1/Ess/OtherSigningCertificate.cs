using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x0200073E RID: 1854
	[Obsolete("Use version in Asn1.Esf instead")]
	public class OtherSigningCertificate : Asn1Encodable
	{
		// Token: 0x06004312 RID: 17170 RVA: 0x00188D70 File Offset: 0x00186F70
		public static OtherSigningCertificate GetInstance(object o)
		{
			if (o == null || o is OtherSigningCertificate)
			{
				return (OtherSigningCertificate)o;
			}
			if (o is Asn1Sequence)
			{
				return new OtherSigningCertificate((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in 'OtherSigningCertificate' factory : " + Platform.GetTypeName(o) + ".");
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x00188DC0 File Offset: 0x00186FC0
		public OtherSigningCertificate(Asn1Sequence seq)
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

		// Token: 0x06004314 RID: 17172 RVA: 0x00188E2D File Offset: 0x0018702D
		public OtherSigningCertificate(OtherCertID otherCertID)
		{
			this.certs = new DerSequence(otherCertID);
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x00188E44 File Offset: 0x00187044
		public OtherCertID[] GetCerts()
		{
			OtherCertID[] array = new OtherCertID[this.certs.Count];
			for (int num = 0; num != this.certs.Count; num++)
			{
				array[num] = OtherCertID.GetInstance(this.certs[num]);
			}
			return array;
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x00188E90 File Offset: 0x00187090
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

		// Token: 0x06004317 RID: 17175 RVA: 0x00188EE4 File Offset: 0x001870E4
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

		// Token: 0x04002C15 RID: 11285
		private Asn1Sequence certs;

		// Token: 0x04002C16 RID: 11286
		private Asn1Sequence policies;
	}
}
