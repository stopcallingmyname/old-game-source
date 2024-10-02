using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002BB RID: 699
	public class PkixCertPathValidatorResult
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x000BD112 File Offset: 0x000BB312
		public PkixPolicyNode PolicyTree
		{
			get
			{
				return this.policyTree;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x000BD11A File Offset: 0x000BB31A
		public TrustAnchor TrustAnchor
		{
			get
			{
				return this.trustAnchor;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001940 RID: 6464 RVA: 0x000BD122 File Offset: 0x000BB322
		public AsymmetricKeyParameter SubjectPublicKey
		{
			get
			{
				return this.subjectPublicKey;
			}
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x000BD12A File Offset: 0x000BB32A
		public PkixCertPathValidatorResult(TrustAnchor trustAnchor, PkixPolicyNode policyTree, AsymmetricKeyParameter subjectPublicKey)
		{
			if (subjectPublicKey == null)
			{
				throw new NullReferenceException("subjectPublicKey must be non-null");
			}
			if (trustAnchor == null)
			{
				throw new NullReferenceException("trustAnchor must be non-null");
			}
			this.trustAnchor = trustAnchor;
			this.policyTree = policyTree;
			this.subjectPublicKey = subjectPublicKey;
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x000BD163 File Offset: 0x000BB363
		public object Clone()
		{
			return new PkixCertPathValidatorResult(this.TrustAnchor, this.PolicyTree, this.SubjectPublicKey);
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x000BD17C File Offset: 0x000BB37C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("PKIXCertPathValidatorResult: [ \n");
			stringBuilder.Append("  Trust Anchor: ").Append(this.TrustAnchor).Append('\n');
			stringBuilder.Append("  Policy Tree: ").Append(this.PolicyTree).Append('\n');
			stringBuilder.Append("  Subject Public Key: ").Append(this.SubjectPublicKey).Append("\n]");
			return stringBuilder.ToString();
		}

		// Token: 0x04001879 RID: 6265
		private TrustAnchor trustAnchor;

		// Token: 0x0400187A RID: 6266
		private PkixPolicyNode policyTree;

		// Token: 0x0400187B RID: 6267
		private AsymmetricKeyParameter subjectPublicKey;
	}
}
