using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x02000256 RID: 598
	public class X509CertPairStoreSelector : IX509Selector, ICloneable
	{
		// Token: 0x060015BC RID: 5564 RVA: 0x000AEFF0 File Offset: 0x000AD1F0
		private static X509CertStoreSelector CloneSelector(X509CertStoreSelector s)
		{
			if (s != null)
			{
				return (X509CertStoreSelector)s.Clone();
			}
			return null;
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00022F1F File Offset: 0x0002111F
		public X509CertPairStoreSelector()
		{
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x000AF002 File Offset: 0x000AD202
		private X509CertPairStoreSelector(X509CertPairStoreSelector o)
		{
			this.certPair = o.CertPair;
			this.forwardSelector = o.ForwardSelector;
			this.reverseSelector = o.ReverseSelector;
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x000AF02E File Offset: 0x000AD22E
		// (set) Token: 0x060015C0 RID: 5568 RVA: 0x000AF036 File Offset: 0x000AD236
		public X509CertificatePair CertPair
		{
			get
			{
				return this.certPair;
			}
			set
			{
				this.certPair = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x000AF03F File Offset: 0x000AD23F
		// (set) Token: 0x060015C2 RID: 5570 RVA: 0x000AF04C File Offset: 0x000AD24C
		public X509CertStoreSelector ForwardSelector
		{
			get
			{
				return X509CertPairStoreSelector.CloneSelector(this.forwardSelector);
			}
			set
			{
				this.forwardSelector = X509CertPairStoreSelector.CloneSelector(value);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060015C3 RID: 5571 RVA: 0x000AF05A File Offset: 0x000AD25A
		// (set) Token: 0x060015C4 RID: 5572 RVA: 0x000AF067 File Offset: 0x000AD267
		public X509CertStoreSelector ReverseSelector
		{
			get
			{
				return X509CertPairStoreSelector.CloneSelector(this.reverseSelector);
			}
			set
			{
				this.reverseSelector = X509CertPairStoreSelector.CloneSelector(value);
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x000AF078 File Offset: 0x000AD278
		public bool Match(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			X509CertificatePair x509CertificatePair = obj as X509CertificatePair;
			return x509CertificatePair != null && (this.certPair == null || this.certPair.Equals(x509CertificatePair)) && (this.forwardSelector == null || this.forwardSelector.Match(x509CertificatePair.Forward)) && (this.reverseSelector == null || this.reverseSelector.Match(x509CertificatePair.Reverse));
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x000AF0F2 File Offset: 0x000AD2F2
		public object Clone()
		{
			return new X509CertPairStoreSelector(this);
		}

		// Token: 0x04001664 RID: 5732
		private X509CertificatePair certPair;

		// Token: 0x04001665 RID: 5733
		private X509CertStoreSelector forwardSelector;

		// Token: 0x04001666 RID: 5734
		private X509CertStoreSelector reverseSelector;
	}
}
