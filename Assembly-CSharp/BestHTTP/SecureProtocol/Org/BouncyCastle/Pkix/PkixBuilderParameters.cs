using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002B3 RID: 691
	public class PkixBuilderParameters : PkixParameters
	{
		// Token: 0x0600190F RID: 6415 RVA: 0x000BBE54 File Offset: 0x000BA054
		public static PkixBuilderParameters GetInstance(PkixParameters pkixParams)
		{
			PkixBuilderParameters pkixBuilderParameters = new PkixBuilderParameters(pkixParams.GetTrustAnchors(), new X509CertStoreSelector(pkixParams.GetTargetCertConstraints()));
			pkixBuilderParameters.SetParams(pkixParams);
			return pkixBuilderParameters;
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x000BBE73 File Offset: 0x000BA073
		public PkixBuilderParameters(ISet trustAnchors, IX509Selector targetConstraints) : base(trustAnchors)
		{
			this.SetTargetCertConstraints(targetConstraints);
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001911 RID: 6417 RVA: 0x000BBE95 File Offset: 0x000BA095
		// (set) Token: 0x06001912 RID: 6418 RVA: 0x000BBE9D File Offset: 0x000BA09D
		public virtual int MaxPathLength
		{
			get
			{
				return this.maxPathLength;
			}
			set
			{
				if (value < -1)
				{
					throw new InvalidParameterException("The maximum path length parameter can not be less than -1.");
				}
				this.maxPathLength = value;
			}
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x000BBEB5 File Offset: 0x000BA0B5
		public virtual ISet GetExcludedCerts()
		{
			return new HashSet(this.excludedCerts);
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x000BBEC2 File Offset: 0x000BA0C2
		public virtual void SetExcludedCerts(ISet excludedCerts)
		{
			if (excludedCerts == null)
			{
				excludedCerts = new HashSet();
				return;
			}
			this.excludedCerts = new HashSet(excludedCerts);
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x000BBEDC File Offset: 0x000BA0DC
		protected override void SetParams(PkixParameters parameters)
		{
			base.SetParams(parameters);
			if (parameters is PkixBuilderParameters)
			{
				PkixBuilderParameters pkixBuilderParameters = (PkixBuilderParameters)parameters;
				this.maxPathLength = pkixBuilderParameters.maxPathLength;
				this.excludedCerts = new HashSet(pkixBuilderParameters.excludedCerts);
			}
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x000BBF1C File Offset: 0x000BA11C
		public override object Clone()
		{
			PkixBuilderParameters pkixBuilderParameters = new PkixBuilderParameters(this.GetTrustAnchors(), this.GetTargetCertConstraints());
			pkixBuilderParameters.SetParams(this);
			return pkixBuilderParameters;
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x000BBF38 File Offset: 0x000BA138
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("PkixBuilderParameters [" + newLine);
			stringBuilder.Append(base.ToString());
			stringBuilder.Append("  Maximum Path Length: ");
			stringBuilder.Append(this.MaxPathLength);
			stringBuilder.Append(newLine + "]" + newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x04001870 RID: 6256
		private int maxPathLength = 5;

		// Token: 0x04001871 RID: 6257
		private ISet excludedCerts = new HashSet();
	}
}
