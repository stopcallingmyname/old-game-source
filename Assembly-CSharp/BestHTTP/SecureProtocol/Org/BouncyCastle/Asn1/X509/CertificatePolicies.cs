using System;
using System.Text;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000691 RID: 1681
	public class CertificatePolicies : Asn1Encodable
	{
		// Token: 0x06003E43 RID: 15939 RVA: 0x001767C4 File Offset: 0x001749C4
		public static CertificatePolicies GetInstance(object obj)
		{
			if (obj == null || obj is CertificatePolicies)
			{
				return (CertificatePolicies)obj;
			}
			return new CertificatePolicies(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x001767E3 File Offset: 0x001749E3
		public static CertificatePolicies GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return CertificatePolicies.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x001767F1 File Offset: 0x001749F1
		public CertificatePolicies(PolicyInformation name)
		{
			this.policyInformation = new PolicyInformation[]
			{
				name
			};
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x00176809 File Offset: 0x00174A09
		public CertificatePolicies(PolicyInformation[] policyInformation)
		{
			this.policyInformation = policyInformation;
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00176818 File Offset: 0x00174A18
		private CertificatePolicies(Asn1Sequence seq)
		{
			this.policyInformation = new PolicyInformation[seq.Count];
			for (int i = 0; i < seq.Count; i++)
			{
				this.policyInformation[i] = PolicyInformation.GetInstance(seq[i]);
			}
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00176861 File Offset: 0x00174A61
		public virtual PolicyInformation[] GetPolicyInformation()
		{
			return (PolicyInformation[])this.policyInformation.Clone();
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00176874 File Offset: 0x00174A74
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] v = this.policyInformation;
			return new DerSequence(v);
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00176890 File Offset: 0x00174A90
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("CertificatePolicies:");
			if (this.policyInformation != null && this.policyInformation.Length != 0)
			{
				stringBuilder.Append(' ');
				stringBuilder.Append(this.policyInformation[0]);
				for (int i = 1; i < this.policyInformation.Length; i++)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(this.policyInformation[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002796 RID: 10134
		private readonly PolicyInformation[] policyInformation;
	}
}
