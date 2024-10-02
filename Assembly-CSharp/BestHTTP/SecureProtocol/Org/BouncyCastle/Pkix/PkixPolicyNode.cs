using System;
using System.Collections;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002C1 RID: 705
	public class PkixPolicyNode
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x000C0B10 File Offset: 0x000BED10
		public virtual int Depth
		{
			get
			{
				return this.mDepth;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x000C0B18 File Offset: 0x000BED18
		public virtual IEnumerable Children
		{
			get
			{
				return new EnumerableProxy(this.mChildren);
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x000C0B25 File Offset: 0x000BED25
		// (set) Token: 0x060019D1 RID: 6609 RVA: 0x000C0B2D File Offset: 0x000BED2D
		public virtual bool IsCritical
		{
			get
			{
				return this.mCritical;
			}
			set
			{
				this.mCritical = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x000C0B36 File Offset: 0x000BED36
		public virtual ISet PolicyQualifiers
		{
			get
			{
				return new HashSet(this.mPolicyQualifiers);
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x000C0B43 File Offset: 0x000BED43
		public virtual string ValidPolicy
		{
			get
			{
				return this.mValidPolicy;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x000C0B4B File Offset: 0x000BED4B
		public virtual bool HasChildren
		{
			get
			{
				return this.mChildren.Count != 0;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x000C0B5B File Offset: 0x000BED5B
		// (set) Token: 0x060019D6 RID: 6614 RVA: 0x000C0B68 File Offset: 0x000BED68
		public virtual ISet ExpectedPolicies
		{
			get
			{
				return new HashSet(this.mExpectedPolicies);
			}
			set
			{
				this.mExpectedPolicies = new HashSet(value);
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x000C0B76 File Offset: 0x000BED76
		// (set) Token: 0x060019D8 RID: 6616 RVA: 0x000C0B7E File Offset: 0x000BED7E
		public virtual PkixPolicyNode Parent
		{
			get
			{
				return this.mParent;
			}
			set
			{
				this.mParent = value;
			}
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x000C0B88 File Offset: 0x000BED88
		public PkixPolicyNode(IList children, int depth, ISet expectedPolicies, PkixPolicyNode parent, ISet policyQualifiers, string validPolicy, bool critical)
		{
			if (children == null)
			{
				this.mChildren = Platform.CreateArrayList();
			}
			else
			{
				this.mChildren = Platform.CreateArrayList(children);
			}
			this.mDepth = depth;
			this.mExpectedPolicies = expectedPolicies;
			this.mParent = parent;
			this.mPolicyQualifiers = policyQualifiers;
			this.mValidPolicy = validPolicy;
			this.mCritical = critical;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x000C0BE5 File Offset: 0x000BEDE5
		public virtual void AddChild(PkixPolicyNode child)
		{
			child.Parent = this;
			this.mChildren.Add(child);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000C0BFB File Offset: 0x000BEDFB
		public virtual void RemoveChild(PkixPolicyNode child)
		{
			this.mChildren.Remove(child);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x000C0C09 File Offset: 0x000BEE09
		public override string ToString()
		{
			return this.ToString("");
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x000C0C18 File Offset: 0x000BEE18
		public virtual string ToString(string indent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(indent);
			stringBuilder.Append(this.mValidPolicy);
			stringBuilder.Append(" {");
			stringBuilder.Append(Platform.NewLine);
			foreach (object obj in this.mChildren)
			{
				PkixPolicyNode pkixPolicyNode = (PkixPolicyNode)obj;
				stringBuilder.Append(pkixPolicyNode.ToString(indent + "    "));
			}
			stringBuilder.Append(indent);
			stringBuilder.Append("}");
			stringBuilder.Append(Platform.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000C0CDC File Offset: 0x000BEEDC
		public virtual object Clone()
		{
			return this.Copy();
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000C0CE4 File Offset: 0x000BEEE4
		public virtual PkixPolicyNode Copy()
		{
			PkixPolicyNode pkixPolicyNode = new PkixPolicyNode(Platform.CreateArrayList(), this.mDepth, new HashSet(this.mExpectedPolicies), null, new HashSet(this.mPolicyQualifiers), this.mValidPolicy, this.mCritical);
			foreach (object obj in this.mChildren)
			{
				PkixPolicyNode pkixPolicyNode2 = ((PkixPolicyNode)obj).Copy();
				pkixPolicyNode2.Parent = pkixPolicyNode;
				pkixPolicyNode.AddChild(pkixPolicyNode2);
			}
			return pkixPolicyNode;
		}

		// Token: 0x040018A2 RID: 6306
		protected IList mChildren;

		// Token: 0x040018A3 RID: 6307
		protected int mDepth;

		// Token: 0x040018A4 RID: 6308
		protected ISet mExpectedPolicies;

		// Token: 0x040018A5 RID: 6309
		protected PkixPolicyNode mParent;

		// Token: 0x040018A6 RID: 6310
		protected ISet mPolicyQualifiers;

		// Token: 0x040018A7 RID: 6311
		protected string mValidPolicy;

		// Token: 0x040018A8 RID: 6312
		protected bool mCritical;
	}
}
