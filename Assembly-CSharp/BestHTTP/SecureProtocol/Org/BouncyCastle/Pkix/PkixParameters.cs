using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002C0 RID: 704
	public class PkixParameters
	{
		// Token: 0x060019A1 RID: 6561 RVA: 0x000C030C File Offset: 0x000BE50C
		public PkixParameters(ISet trustAnchors)
		{
			this.SetTrustAnchors(trustAnchors);
			this.initialPolicies = new HashSet();
			this.certPathCheckers = Platform.CreateArrayList();
			this.stores = Platform.CreateArrayList();
			this.additionalStores = Platform.CreateArrayList();
			this.trustedACIssuers = new HashSet();
			this.necessaryACAttributes = new HashSet();
			this.prohibitedACAttributes = new HashSet();
			this.attrCertCheckers = new HashSet();
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060019A2 RID: 6562 RVA: 0x000C038C File Offset: 0x000BE58C
		// (set) Token: 0x060019A3 RID: 6563 RVA: 0x000C0394 File Offset: 0x000BE594
		public virtual bool IsRevocationEnabled
		{
			get
			{
				return this.revocationEnabled;
			}
			set
			{
				this.revocationEnabled = value;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x000C039D File Offset: 0x000BE59D
		// (set) Token: 0x060019A5 RID: 6565 RVA: 0x000C03A5 File Offset: 0x000BE5A5
		public virtual bool IsExplicitPolicyRequired
		{
			get
			{
				return this.explicitPolicyRequired;
			}
			set
			{
				this.explicitPolicyRequired = value;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060019A6 RID: 6566 RVA: 0x000C03AE File Offset: 0x000BE5AE
		// (set) Token: 0x060019A7 RID: 6567 RVA: 0x000C03B6 File Offset: 0x000BE5B6
		public virtual bool IsAnyPolicyInhibited
		{
			get
			{
				return this.anyPolicyInhibited;
			}
			set
			{
				this.anyPolicyInhibited = value;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060019A8 RID: 6568 RVA: 0x000C03BF File Offset: 0x000BE5BF
		// (set) Token: 0x060019A9 RID: 6569 RVA: 0x000C03C7 File Offset: 0x000BE5C7
		public virtual bool IsPolicyMappingInhibited
		{
			get
			{
				return this.policyMappingInhibited;
			}
			set
			{
				this.policyMappingInhibited = value;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x000C03D0 File Offset: 0x000BE5D0
		// (set) Token: 0x060019AB RID: 6571 RVA: 0x000C03D8 File Offset: 0x000BE5D8
		public virtual bool IsPolicyQualifiersRejected
		{
			get
			{
				return this.policyQualifiersRejected;
			}
			set
			{
				this.policyQualifiersRejected = value;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060019AC RID: 6572 RVA: 0x000C03E1 File Offset: 0x000BE5E1
		// (set) Token: 0x060019AD RID: 6573 RVA: 0x000C03E9 File Offset: 0x000BE5E9
		public virtual DateTimeObject Date
		{
			get
			{
				return this.date;
			}
			set
			{
				this.date = value;
			}
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x000C03F2 File Offset: 0x000BE5F2
		public virtual ISet GetTrustAnchors()
		{
			return new HashSet(this.trustAnchors);
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x000C0400 File Offset: 0x000BE600
		public virtual void SetTrustAnchors(ISet tas)
		{
			if (tas == null)
			{
				throw new ArgumentNullException("value");
			}
			if (tas.IsEmpty)
			{
				throw new ArgumentException("non-empty set required", "value");
			}
			this.trustAnchors = new HashSet();
			foreach (object obj in tas)
			{
				TrustAnchor trustAnchor = (TrustAnchor)obj;
				if (trustAnchor != null)
				{
					this.trustAnchors.Add(trustAnchor);
				}
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x000C0490 File Offset: 0x000BE690
		public virtual X509CertStoreSelector GetTargetCertConstraints()
		{
			if (this.certSelector == null)
			{
				return null;
			}
			return (X509CertStoreSelector)this.certSelector.Clone();
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x000C04AC File Offset: 0x000BE6AC
		public virtual void SetTargetCertConstraints(IX509Selector selector)
		{
			if (selector == null)
			{
				this.certSelector = null;
				return;
			}
			this.certSelector = (IX509Selector)selector.Clone();
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x000C04CC File Offset: 0x000BE6CC
		public virtual ISet GetInitialPolicies()
		{
			ISet s = this.initialPolicies;
			if (this.initialPolicies == null)
			{
				s = new HashSet();
			}
			return new HashSet(s);
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x000C04F4 File Offset: 0x000BE6F4
		public virtual void SetInitialPolicies(ISet initialPolicies)
		{
			this.initialPolicies = new HashSet();
			if (initialPolicies != null)
			{
				foreach (object obj in initialPolicies)
				{
					string text = (string)obj;
					if (text != null)
					{
						this.initialPolicies.Add(text);
					}
				}
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000C0560 File Offset: 0x000BE760
		public virtual void SetCertPathCheckers(IList checkers)
		{
			this.certPathCheckers = Platform.CreateArrayList();
			if (checkers != null)
			{
				foreach (object obj in checkers)
				{
					PkixCertPathChecker pkixCertPathChecker = (PkixCertPathChecker)obj;
					this.certPathCheckers.Add(pkixCertPathChecker.Clone());
				}
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x000C05D0 File Offset: 0x000BE7D0
		public virtual IList GetCertPathCheckers()
		{
			IList list = Platform.CreateArrayList();
			foreach (object obj in this.certPathCheckers)
			{
				PkixCertPathChecker pkixCertPathChecker = (PkixCertPathChecker)obj;
				list.Add(pkixCertPathChecker.Clone());
			}
			return list;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000C0638 File Offset: 0x000BE838
		public virtual void AddCertPathChecker(PkixCertPathChecker checker)
		{
			if (checker != null)
			{
				this.certPathCheckers.Add(checker.Clone());
			}
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x000C064F File Offset: 0x000BE84F
		public virtual object Clone()
		{
			PkixParameters pkixParameters = new PkixParameters(this.GetTrustAnchors());
			pkixParameters.SetParams(this);
			return pkixParameters;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x000C0664 File Offset: 0x000BE864
		protected virtual void SetParams(PkixParameters parameters)
		{
			this.Date = parameters.Date;
			this.SetCertPathCheckers(parameters.GetCertPathCheckers());
			this.IsAnyPolicyInhibited = parameters.IsAnyPolicyInhibited;
			this.IsExplicitPolicyRequired = parameters.IsExplicitPolicyRequired;
			this.IsPolicyMappingInhibited = parameters.IsPolicyMappingInhibited;
			this.IsRevocationEnabled = parameters.IsRevocationEnabled;
			this.SetInitialPolicies(parameters.GetInitialPolicies());
			this.IsPolicyQualifiersRejected = parameters.IsPolicyQualifiersRejected;
			this.SetTargetCertConstraints(parameters.GetTargetCertConstraints());
			this.SetTrustAnchors(parameters.GetTrustAnchors());
			this.validityModel = parameters.validityModel;
			this.useDeltas = parameters.useDeltas;
			this.additionalLocationsEnabled = parameters.additionalLocationsEnabled;
			this.selector = ((parameters.selector == null) ? null : ((IX509Selector)parameters.selector.Clone()));
			this.stores = Platform.CreateArrayList(parameters.stores);
			this.additionalStores = Platform.CreateArrayList(parameters.additionalStores);
			this.trustedACIssuers = new HashSet(parameters.trustedACIssuers);
			this.prohibitedACAttributes = new HashSet(parameters.prohibitedACAttributes);
			this.necessaryACAttributes = new HashSet(parameters.necessaryACAttributes);
			this.attrCertCheckers = new HashSet(parameters.attrCertCheckers);
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x000C0794 File Offset: 0x000BE994
		// (set) Token: 0x060019BA RID: 6586 RVA: 0x000C079C File Offset: 0x000BE99C
		public virtual bool IsUseDeltasEnabled
		{
			get
			{
				return this.useDeltas;
			}
			set
			{
				this.useDeltas = value;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x000C07A5 File Offset: 0x000BE9A5
		// (set) Token: 0x060019BC RID: 6588 RVA: 0x000C07AD File Offset: 0x000BE9AD
		public virtual int ValidityModel
		{
			get
			{
				return this.validityModel;
			}
			set
			{
				this.validityModel = value;
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000C07B8 File Offset: 0x000BE9B8
		public virtual void SetStores(IList stores)
		{
			if (stores == null)
			{
				this.stores = Platform.CreateArrayList();
				return;
			}
			using (IEnumerator enumerator = stores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current is IX509Store))
					{
						throw new InvalidCastException("All elements of list must be of type " + typeof(IX509Store).FullName);
					}
				}
			}
			this.stores = Platform.CreateArrayList(stores);
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x000C0840 File Offset: 0x000BEA40
		public virtual void AddStore(IX509Store store)
		{
			if (store != null)
			{
				this.stores.Add(store);
			}
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x000C0852 File Offset: 0x000BEA52
		public virtual void AddAdditionalStore(IX509Store store)
		{
			if (store != null)
			{
				this.additionalStores.Add(store);
			}
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x000C0864 File Offset: 0x000BEA64
		public virtual IList GetAdditionalStores()
		{
			return Platform.CreateArrayList(this.additionalStores);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x000C0871 File Offset: 0x000BEA71
		public virtual IList GetStores()
		{
			return Platform.CreateArrayList(this.stores);
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x000C087E File Offset: 0x000BEA7E
		public virtual bool IsAdditionalLocationsEnabled
		{
			get
			{
				return this.additionalLocationsEnabled;
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x000C0886 File Offset: 0x000BEA86
		public virtual void SetAdditionalLocationsEnabled(bool enabled)
		{
			this.additionalLocationsEnabled = enabled;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x000C088F File Offset: 0x000BEA8F
		public virtual IX509Selector GetTargetConstraints()
		{
			if (this.selector != null)
			{
				return (IX509Selector)this.selector.Clone();
			}
			return null;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x000C08AB File Offset: 0x000BEAAB
		public virtual void SetTargetConstraints(IX509Selector selector)
		{
			if (selector != null)
			{
				this.selector = (IX509Selector)selector.Clone();
				return;
			}
			this.selector = null;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x000C08C9 File Offset: 0x000BEAC9
		public virtual ISet GetTrustedACIssuers()
		{
			return new HashSet(this.trustedACIssuers);
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000C08D8 File Offset: 0x000BEAD8
		public virtual void SetTrustedACIssuers(ISet trustedACIssuers)
		{
			if (trustedACIssuers == null)
			{
				this.trustedACIssuers = new HashSet();
				return;
			}
			using (IEnumerator enumerator = trustedACIssuers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current is TrustAnchor))
					{
						throw new InvalidCastException("All elements of set must be of type " + typeof(TrustAnchor).FullName + ".");
					}
				}
			}
			this.trustedACIssuers = new HashSet(trustedACIssuers);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000C0968 File Offset: 0x000BEB68
		public virtual ISet GetNecessaryACAttributes()
		{
			return new HashSet(this.necessaryACAttributes);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000C0978 File Offset: 0x000BEB78
		public virtual void SetNecessaryACAttributes(ISet necessaryACAttributes)
		{
			if (necessaryACAttributes == null)
			{
				this.necessaryACAttributes = new HashSet();
				return;
			}
			using (IEnumerator enumerator = necessaryACAttributes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current is string))
					{
						throw new InvalidCastException("All elements of set must be of type string.");
					}
				}
			}
			this.necessaryACAttributes = new HashSet(necessaryACAttributes);
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000C09EC File Offset: 0x000BEBEC
		public virtual ISet GetProhibitedACAttributes()
		{
			return new HashSet(this.prohibitedACAttributes);
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000C09FC File Offset: 0x000BEBFC
		public virtual void SetProhibitedACAttributes(ISet prohibitedACAttributes)
		{
			if (prohibitedACAttributes == null)
			{
				this.prohibitedACAttributes = new HashSet();
				return;
			}
			using (IEnumerator enumerator = prohibitedACAttributes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current is string))
					{
						throw new InvalidCastException("All elements of set must be of type string.");
					}
				}
			}
			this.prohibitedACAttributes = new HashSet(prohibitedACAttributes);
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000C0A70 File Offset: 0x000BEC70
		public virtual ISet GetAttrCertCheckers()
		{
			return new HashSet(this.attrCertCheckers);
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000C0A80 File Offset: 0x000BEC80
		public virtual void SetAttrCertCheckers(ISet attrCertCheckers)
		{
			if (attrCertCheckers == null)
			{
				this.attrCertCheckers = new HashSet();
				return;
			}
			using (IEnumerator enumerator = attrCertCheckers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current is PkixAttrCertChecker))
					{
						throw new InvalidCastException("All elements of set must be of type " + typeof(PkixAttrCertChecker).FullName + ".");
					}
				}
			}
			this.attrCertCheckers = new HashSet(attrCertCheckers);
		}

		// Token: 0x0400188C RID: 6284
		public const int PkixValidityModel = 0;

		// Token: 0x0400188D RID: 6285
		public const int ChainValidityModel = 1;

		// Token: 0x0400188E RID: 6286
		private ISet trustAnchors;

		// Token: 0x0400188F RID: 6287
		private DateTimeObject date;

		// Token: 0x04001890 RID: 6288
		private IList certPathCheckers;

		// Token: 0x04001891 RID: 6289
		private bool revocationEnabled = true;

		// Token: 0x04001892 RID: 6290
		private ISet initialPolicies;

		// Token: 0x04001893 RID: 6291
		private bool explicitPolicyRequired;

		// Token: 0x04001894 RID: 6292
		private bool anyPolicyInhibited;

		// Token: 0x04001895 RID: 6293
		private bool policyMappingInhibited;

		// Token: 0x04001896 RID: 6294
		private bool policyQualifiersRejected = true;

		// Token: 0x04001897 RID: 6295
		private IX509Selector certSelector;

		// Token: 0x04001898 RID: 6296
		private IList stores;

		// Token: 0x04001899 RID: 6297
		private IX509Selector selector;

		// Token: 0x0400189A RID: 6298
		private bool additionalLocationsEnabled;

		// Token: 0x0400189B RID: 6299
		private IList additionalStores;

		// Token: 0x0400189C RID: 6300
		private ISet trustedACIssuers;

		// Token: 0x0400189D RID: 6301
		private ISet necessaryACAttributes;

		// Token: 0x0400189E RID: 6302
		private ISet prohibitedACAttributes;

		// Token: 0x0400189F RID: 6303
		private ISet attrCertCheckers;

		// Token: 0x040018A0 RID: 6304
		private int validityModel;

		// Token: 0x040018A1 RID: 6305
		private bool useDeltas;
	}
}
