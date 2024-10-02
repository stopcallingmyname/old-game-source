using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x0200025A RID: 602
	public class X509CrlStoreSelector : IX509Selector, ICloneable
	{
		// Token: 0x060015F5 RID: 5621 RVA: 0x00022F1F File Offset: 0x0002111F
		public X509CrlStoreSelector()
		{
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x000AF7C8 File Offset: 0x000AD9C8
		public X509CrlStoreSelector(X509CrlStoreSelector o)
		{
			this.certificateChecking = o.CertificateChecking;
			this.dateAndTime = o.DateAndTime;
			this.issuers = o.Issuers;
			this.maxCrlNumber = o.MaxCrlNumber;
			this.minCrlNumber = o.MinCrlNumber;
			this.deltaCrlIndicatorEnabled = o.DeltaCrlIndicatorEnabled;
			this.completeCrlEnabled = o.CompleteCrlEnabled;
			this.maxBaseCrlNumber = o.MaxBaseCrlNumber;
			this.attrCertChecking = o.AttrCertChecking;
			this.issuingDistributionPointEnabled = o.IssuingDistributionPointEnabled;
			this.issuingDistributionPoint = o.IssuingDistributionPoint;
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x000AF85F File Offset: 0x000ADA5F
		public virtual object Clone()
		{
			return new X509CrlStoreSelector(this);
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060015F8 RID: 5624 RVA: 0x000AF867 File Offset: 0x000ADA67
		// (set) Token: 0x060015F9 RID: 5625 RVA: 0x000AF86F File Offset: 0x000ADA6F
		public X509Certificate CertificateChecking
		{
			get
			{
				return this.certificateChecking;
			}
			set
			{
				this.certificateChecking = value;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x000AF878 File Offset: 0x000ADA78
		// (set) Token: 0x060015FB RID: 5627 RVA: 0x000AF880 File Offset: 0x000ADA80
		public DateTimeObject DateAndTime
		{
			get
			{
				return this.dateAndTime;
			}
			set
			{
				this.dateAndTime = value;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060015FC RID: 5628 RVA: 0x000AF889 File Offset: 0x000ADA89
		// (set) Token: 0x060015FD RID: 5629 RVA: 0x000AF896 File Offset: 0x000ADA96
		public ICollection Issuers
		{
			get
			{
				return Platform.CreateArrayList(this.issuers);
			}
			set
			{
				this.issuers = Platform.CreateArrayList(value);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060015FE RID: 5630 RVA: 0x000AF8A4 File Offset: 0x000ADAA4
		// (set) Token: 0x060015FF RID: 5631 RVA: 0x000AF8AC File Offset: 0x000ADAAC
		public BigInteger MaxCrlNumber
		{
			get
			{
				return this.maxCrlNumber;
			}
			set
			{
				this.maxCrlNumber = value;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x000AF8B5 File Offset: 0x000ADAB5
		// (set) Token: 0x06001601 RID: 5633 RVA: 0x000AF8BD File Offset: 0x000ADABD
		public BigInteger MinCrlNumber
		{
			get
			{
				return this.minCrlNumber;
			}
			set
			{
				this.minCrlNumber = value;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x000AF8C6 File Offset: 0x000ADAC6
		// (set) Token: 0x06001603 RID: 5635 RVA: 0x000AF8CE File Offset: 0x000ADACE
		public IX509AttributeCertificate AttrCertChecking
		{
			get
			{
				return this.attrCertChecking;
			}
			set
			{
				this.attrCertChecking = value;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x000AF8D7 File Offset: 0x000ADAD7
		// (set) Token: 0x06001605 RID: 5637 RVA: 0x000AF8DF File Offset: 0x000ADADF
		public bool CompleteCrlEnabled
		{
			get
			{
				return this.completeCrlEnabled;
			}
			set
			{
				this.completeCrlEnabled = value;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x000AF8E8 File Offset: 0x000ADAE8
		// (set) Token: 0x06001607 RID: 5639 RVA: 0x000AF8F0 File Offset: 0x000ADAF0
		public bool DeltaCrlIndicatorEnabled
		{
			get
			{
				return this.deltaCrlIndicatorEnabled;
			}
			set
			{
				this.deltaCrlIndicatorEnabled = value;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x000AF8F9 File Offset: 0x000ADAF9
		// (set) Token: 0x06001609 RID: 5641 RVA: 0x000AF906 File Offset: 0x000ADB06
		public byte[] IssuingDistributionPoint
		{
			get
			{
				return Arrays.Clone(this.issuingDistributionPoint);
			}
			set
			{
				this.issuingDistributionPoint = Arrays.Clone(value);
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x000AF914 File Offset: 0x000ADB14
		// (set) Token: 0x0600160B RID: 5643 RVA: 0x000AF91C File Offset: 0x000ADB1C
		public bool IssuingDistributionPointEnabled
		{
			get
			{
				return this.issuingDistributionPointEnabled;
			}
			set
			{
				this.issuingDistributionPointEnabled = value;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x000AF925 File Offset: 0x000ADB25
		// (set) Token: 0x0600160D RID: 5645 RVA: 0x000AF92D File Offset: 0x000ADB2D
		public BigInteger MaxBaseCrlNumber
		{
			get
			{
				return this.maxBaseCrlNumber;
			}
			set
			{
				this.maxBaseCrlNumber = value;
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x000AF938 File Offset: 0x000ADB38
		public virtual bool Match(object obj)
		{
			X509Crl x509Crl = obj as X509Crl;
			if (x509Crl == null)
			{
				return false;
			}
			if (this.dateAndTime != null)
			{
				DateTime value = this.dateAndTime.Value;
				DateTime thisUpdate = x509Crl.ThisUpdate;
				DateTimeObject nextUpdate = x509Crl.NextUpdate;
				if (value.CompareTo(thisUpdate) < 0 || nextUpdate == null || value.CompareTo(nextUpdate.Value) >= 0)
				{
					return false;
				}
			}
			if (this.issuers != null)
			{
				X509Name issuerDN = x509Crl.IssuerDN;
				bool flag = false;
				using (IEnumerator enumerator = this.issuers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((X509Name)enumerator.Current).Equivalent(issuerDN, true))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (this.maxCrlNumber != null || this.minCrlNumber != null)
			{
				Asn1OctetString extensionValue = x509Crl.GetExtensionValue(X509Extensions.CrlNumber);
				if (extensionValue == null)
				{
					return false;
				}
				BigInteger positiveValue = DerInteger.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue)).PositiveValue;
				if (this.maxCrlNumber != null && positiveValue.CompareTo(this.maxCrlNumber) > 0)
				{
					return false;
				}
				if (this.minCrlNumber != null && positiveValue.CompareTo(this.minCrlNumber) < 0)
				{
					return false;
				}
			}
			DerInteger derInteger = null;
			try
			{
				Asn1OctetString extensionValue2 = x509Crl.GetExtensionValue(X509Extensions.DeltaCrlIndicator);
				if (extensionValue2 != null)
				{
					derInteger = DerInteger.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue2));
				}
			}
			catch (Exception)
			{
				return false;
			}
			if (derInteger == null)
			{
				if (this.DeltaCrlIndicatorEnabled)
				{
					return false;
				}
			}
			else
			{
				if (this.CompleteCrlEnabled)
				{
					return false;
				}
				if (this.maxBaseCrlNumber != null && derInteger.PositiveValue.CompareTo(this.maxBaseCrlNumber) > 0)
				{
					return false;
				}
			}
			if (this.issuingDistributionPointEnabled)
			{
				Asn1OctetString extensionValue3 = x509Crl.GetExtensionValue(X509Extensions.IssuingDistributionPoint);
				if (this.issuingDistributionPoint == null)
				{
					if (extensionValue3 != null)
					{
						return false;
					}
				}
				else if (!Arrays.AreEqual(extensionValue3.GetOctets(), this.issuingDistributionPoint))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001678 RID: 5752
		private X509Certificate certificateChecking;

		// Token: 0x04001679 RID: 5753
		private DateTimeObject dateAndTime;

		// Token: 0x0400167A RID: 5754
		private ICollection issuers;

		// Token: 0x0400167B RID: 5755
		private BigInteger maxCrlNumber;

		// Token: 0x0400167C RID: 5756
		private BigInteger minCrlNumber;

		// Token: 0x0400167D RID: 5757
		private IX509AttributeCertificate attrCertChecking;

		// Token: 0x0400167E RID: 5758
		private bool completeCrlEnabled;

		// Token: 0x0400167F RID: 5759
		private bool deltaCrlIndicatorEnabled;

		// Token: 0x04001680 RID: 5760
		private byte[] issuingDistributionPoint;

		// Token: 0x04001681 RID: 5761
		private bool issuingDistributionPointEnabled;

		// Token: 0x04001682 RID: 5762
		private BigInteger maxBaseCrlNumber;
	}
}
