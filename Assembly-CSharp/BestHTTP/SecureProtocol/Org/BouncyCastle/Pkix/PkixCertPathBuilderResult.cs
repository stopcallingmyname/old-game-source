using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002B7 RID: 695
	public class PkixCertPathBuilderResult : PkixCertPathValidatorResult
	{
		// Token: 0x0600192B RID: 6443 RVA: 0x000BC923 File Offset: 0x000BAB23
		public PkixCertPathBuilderResult(PkixCertPath certPath, TrustAnchor trustAnchor, PkixPolicyNode policyTree, AsymmetricKeyParameter subjectPublicKey) : base(trustAnchor, policyTree, subjectPublicKey)
		{
			if (certPath == null)
			{
				throw new ArgumentNullException("certPath");
			}
			this.certPath = certPath;
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x000BC944 File Offset: 0x000BAB44
		public PkixCertPath CertPath
		{
			get
			{
				return this.certPath;
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x000BC94C File Offset: 0x000BAB4C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SimplePKIXCertPathBuilderResult: [\n");
			stringBuilder.Append("  Certification Path: ").Append(this.CertPath).Append('\n');
			stringBuilder.Append("  Trust Anchor: ").Append(base.TrustAnchor.TrustedCert.IssuerDN.ToString()).Append('\n');
			stringBuilder.Append("  Subject Public Key: ").Append(base.SubjectPublicKey).Append("\n]");
			return stringBuilder.ToString();
		}

		// Token: 0x04001875 RID: 6261
		private PkixCertPath certPath;
	}
}
