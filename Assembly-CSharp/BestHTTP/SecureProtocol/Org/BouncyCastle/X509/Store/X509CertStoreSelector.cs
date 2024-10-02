using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x02000257 RID: 599
	public class X509CertStoreSelector : IX509Selector, ICloneable
	{
		// Token: 0x060015C7 RID: 5575 RVA: 0x000AF0FA File Offset: 0x000AD2FA
		public X509CertStoreSelector()
		{
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x000AF10C File Offset: 0x000AD30C
		public X509CertStoreSelector(X509CertStoreSelector o)
		{
			this.authorityKeyIdentifier = o.AuthorityKeyIdentifier;
			this.basicConstraints = o.BasicConstraints;
			this.certificate = o.Certificate;
			this.certificateValid = o.CertificateValid;
			this.extendedKeyUsage = o.ExtendedKeyUsage;
			this.ignoreX509NameOrdering = o.IgnoreX509NameOrdering;
			this.issuer = o.Issuer;
			this.keyUsage = o.KeyUsage;
			this.policy = o.Policy;
			this.privateKeyValid = o.PrivateKeyValid;
			this.serialNumber = o.SerialNumber;
			this.subject = o.Subject;
			this.subjectKeyIdentifier = o.SubjectKeyIdentifier;
			this.subjectPublicKey = o.SubjectPublicKey;
			this.subjectPublicKeyAlgID = o.SubjectPublicKeyAlgID;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x000AF1DA File Offset: 0x000AD3DA
		public virtual object Clone()
		{
			return new X509CertStoreSelector(this);
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x000AF1E2 File Offset: 0x000AD3E2
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x000AF1EF File Offset: 0x000AD3EF
		public byte[] AuthorityKeyIdentifier
		{
			get
			{
				return Arrays.Clone(this.authorityKeyIdentifier);
			}
			set
			{
				this.authorityKeyIdentifier = Arrays.Clone(value);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x000AF1FD File Offset: 0x000AD3FD
		// (set) Token: 0x060015CD RID: 5581 RVA: 0x000AF205 File Offset: 0x000AD405
		public int BasicConstraints
		{
			get
			{
				return this.basicConstraints;
			}
			set
			{
				if (value < -2)
				{
					throw new ArgumentException("value can't be less than -2", "value");
				}
				this.basicConstraints = value;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x000AF223 File Offset: 0x000AD423
		// (set) Token: 0x060015CF RID: 5583 RVA: 0x000AF22B File Offset: 0x000AD42B
		public X509Certificate Certificate
		{
			get
			{
				return this.certificate;
			}
			set
			{
				this.certificate = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x000AF234 File Offset: 0x000AD434
		// (set) Token: 0x060015D1 RID: 5585 RVA: 0x000AF23C File Offset: 0x000AD43C
		public DateTimeObject CertificateValid
		{
			get
			{
				return this.certificateValid;
			}
			set
			{
				this.certificateValid = value;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x000AF245 File Offset: 0x000AD445
		// (set) Token: 0x060015D3 RID: 5587 RVA: 0x000AF252 File Offset: 0x000AD452
		public ISet ExtendedKeyUsage
		{
			get
			{
				return X509CertStoreSelector.CopySet(this.extendedKeyUsage);
			}
			set
			{
				this.extendedKeyUsage = X509CertStoreSelector.CopySet(value);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x000AF260 File Offset: 0x000AD460
		// (set) Token: 0x060015D5 RID: 5589 RVA: 0x000AF268 File Offset: 0x000AD468
		public bool IgnoreX509NameOrdering
		{
			get
			{
				return this.ignoreX509NameOrdering;
			}
			set
			{
				this.ignoreX509NameOrdering = value;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x000AF271 File Offset: 0x000AD471
		// (set) Token: 0x060015D7 RID: 5591 RVA: 0x000AF279 File Offset: 0x000AD479
		public X509Name Issuer
		{
			get
			{
				return this.issuer;
			}
			set
			{
				this.issuer = value;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x000AF282 File Offset: 0x000AD482
		[Obsolete("Avoid working with X509Name objects in string form")]
		public string IssuerAsString
		{
			get
			{
				if (this.issuer == null)
				{
					return null;
				}
				return this.issuer.ToString();
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x000AF299 File Offset: 0x000AD499
		// (set) Token: 0x060015DA RID: 5594 RVA: 0x000AF2A6 File Offset: 0x000AD4A6
		public bool[] KeyUsage
		{
			get
			{
				return X509CertStoreSelector.CopyBoolArray(this.keyUsage);
			}
			set
			{
				this.keyUsage = X509CertStoreSelector.CopyBoolArray(value);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x000AF2B4 File Offset: 0x000AD4B4
		// (set) Token: 0x060015DC RID: 5596 RVA: 0x000AF2C1 File Offset: 0x000AD4C1
		public ISet Policy
		{
			get
			{
				return X509CertStoreSelector.CopySet(this.policy);
			}
			set
			{
				this.policy = X509CertStoreSelector.CopySet(value);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x000AF2CF File Offset: 0x000AD4CF
		// (set) Token: 0x060015DE RID: 5598 RVA: 0x000AF2D7 File Offset: 0x000AD4D7
		public DateTimeObject PrivateKeyValid
		{
			get
			{
				return this.privateKeyValid;
			}
			set
			{
				this.privateKeyValid = value;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x000AF2E0 File Offset: 0x000AD4E0
		// (set) Token: 0x060015E0 RID: 5600 RVA: 0x000AF2E8 File Offset: 0x000AD4E8
		public BigInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
			set
			{
				this.serialNumber = value;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x000AF2F1 File Offset: 0x000AD4F1
		// (set) Token: 0x060015E2 RID: 5602 RVA: 0x000AF2F9 File Offset: 0x000AD4F9
		public X509Name Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				this.subject = value;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x000AF302 File Offset: 0x000AD502
		[Obsolete("Avoid working with X509Name objects in string form")]
		public string SubjectAsString
		{
			get
			{
				if (this.subject == null)
				{
					return null;
				}
				return this.subject.ToString();
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x000AF319 File Offset: 0x000AD519
		// (set) Token: 0x060015E5 RID: 5605 RVA: 0x000AF326 File Offset: 0x000AD526
		public byte[] SubjectKeyIdentifier
		{
			get
			{
				return Arrays.Clone(this.subjectKeyIdentifier);
			}
			set
			{
				this.subjectKeyIdentifier = Arrays.Clone(value);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x000AF334 File Offset: 0x000AD534
		// (set) Token: 0x060015E7 RID: 5607 RVA: 0x000AF33C File Offset: 0x000AD53C
		public SubjectPublicKeyInfo SubjectPublicKey
		{
			get
			{
				return this.subjectPublicKey;
			}
			set
			{
				this.subjectPublicKey = value;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x000AF345 File Offset: 0x000AD545
		// (set) Token: 0x060015E9 RID: 5609 RVA: 0x000AF34D File Offset: 0x000AD54D
		public DerObjectIdentifier SubjectPublicKeyAlgID
		{
			get
			{
				return this.subjectPublicKeyAlgID;
			}
			set
			{
				this.subjectPublicKeyAlgID = value;
			}
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x000AF358 File Offset: 0x000AD558
		public virtual bool Match(object obj)
		{
			X509Certificate x509Certificate = obj as X509Certificate;
			if (x509Certificate == null)
			{
				return false;
			}
			if (!X509CertStoreSelector.MatchExtension(this.authorityKeyIdentifier, x509Certificate, X509Extensions.AuthorityKeyIdentifier))
			{
				return false;
			}
			if (this.basicConstraints != -1)
			{
				int num = x509Certificate.GetBasicConstraints();
				if (this.basicConstraints == -2)
				{
					if (num != -1)
					{
						return false;
					}
				}
				else if (num < this.basicConstraints)
				{
					return false;
				}
			}
			if (this.certificate != null && !this.certificate.Equals(x509Certificate))
			{
				return false;
			}
			if (this.certificateValid != null && !x509Certificate.IsValid(this.certificateValid.Value))
			{
				return false;
			}
			if (this.extendedKeyUsage != null)
			{
				IList list = x509Certificate.GetExtendedKeyUsage();
				if (list != null)
				{
					foreach (object obj2 in this.extendedKeyUsage)
					{
						DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj2;
						if (!list.Contains(derObjectIdentifier.Id))
						{
							return false;
						}
					}
				}
			}
			if (this.issuer != null && !this.issuer.Equivalent(x509Certificate.IssuerDN, !this.ignoreX509NameOrdering))
			{
				return false;
			}
			if (this.keyUsage != null)
			{
				bool[] array = x509Certificate.GetKeyUsage();
				if (array != null)
				{
					for (int i = 0; i < 9; i++)
					{
						if (this.keyUsage[i] && !array[i])
						{
							return false;
						}
					}
				}
			}
			if (this.policy != null)
			{
				Asn1OctetString extensionValue = x509Certificate.GetExtensionValue(X509Extensions.CertificatePolicies);
				if (extensionValue == null)
				{
					return false;
				}
				Asn1Sequence instance = Asn1Sequence.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
				if (this.policy.Count < 1 && instance.Count < 1)
				{
					return false;
				}
				bool flag = false;
				foreach (object obj3 in instance)
				{
					PolicyInformation policyInformation = (PolicyInformation)obj3;
					if (this.policy.Contains(policyInformation.PolicyIdentifier))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (this.privateKeyValid != null)
			{
				Asn1OctetString extensionValue2 = x509Certificate.GetExtensionValue(X509Extensions.PrivateKeyUsagePeriod);
				if (extensionValue2 == null)
				{
					return false;
				}
				PrivateKeyUsagePeriod instance2 = PrivateKeyUsagePeriod.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue2));
				DateTime value = this.privateKeyValid.Value;
				DateTime value2 = instance2.NotAfter.ToDateTime();
				DateTime value3 = instance2.NotBefore.ToDateTime();
				if (value.CompareTo(value2) > 0 || value.CompareTo(value3) < 0)
				{
					return false;
				}
			}
			return (this.serialNumber == null || this.serialNumber.Equals(x509Certificate.SerialNumber)) && (this.subject == null || this.subject.Equivalent(x509Certificate.SubjectDN, !this.ignoreX509NameOrdering)) && X509CertStoreSelector.MatchExtension(this.subjectKeyIdentifier, x509Certificate, X509Extensions.SubjectKeyIdentifier) && (this.subjectPublicKey == null || this.subjectPublicKey.Equals(X509CertStoreSelector.GetSubjectPublicKey(x509Certificate))) && (this.subjectPublicKeyAlgID == null || this.subjectPublicKeyAlgID.Equals(X509CertStoreSelector.GetSubjectPublicKey(x509Certificate).AlgorithmID));
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x000AF664 File Offset: 0x000AD864
		internal static bool IssuersMatch(X509Name a, X509Name b)
		{
			if (a != null)
			{
				return a.Equivalent(b, true);
			}
			return b == null;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x000AF676 File Offset: 0x000AD876
		private static bool[] CopyBoolArray(bool[] b)
		{
			if (b != null)
			{
				return (bool[])b.Clone();
			}
			return null;
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x000AF688 File Offset: 0x000AD888
		private static ISet CopySet(ISet s)
		{
			if (s != null)
			{
				return new HashSet(s);
			}
			return null;
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x000AF695 File Offset: 0x000AD895
		private static SubjectPublicKeyInfo GetSubjectPublicKey(X509Certificate c)
		{
			return SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(c.GetPublicKey());
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x000AF6A4 File Offset: 0x000AD8A4
		private static bool MatchExtension(byte[] b, X509Certificate c, DerObjectIdentifier oid)
		{
			if (b == null)
			{
				return true;
			}
			Asn1OctetString extensionValue = c.GetExtensionValue(oid);
			return extensionValue != null && Arrays.AreEqual(b, extensionValue.GetOctets());
		}

		// Token: 0x04001667 RID: 5735
		private byte[] authorityKeyIdentifier;

		// Token: 0x04001668 RID: 5736
		private int basicConstraints = -1;

		// Token: 0x04001669 RID: 5737
		private X509Certificate certificate;

		// Token: 0x0400166A RID: 5738
		private DateTimeObject certificateValid;

		// Token: 0x0400166B RID: 5739
		private ISet extendedKeyUsage;

		// Token: 0x0400166C RID: 5740
		private bool ignoreX509NameOrdering;

		// Token: 0x0400166D RID: 5741
		private X509Name issuer;

		// Token: 0x0400166E RID: 5742
		private bool[] keyUsage;

		// Token: 0x0400166F RID: 5743
		private ISet policy;

		// Token: 0x04001670 RID: 5744
		private DateTimeObject privateKeyValid;

		// Token: 0x04001671 RID: 5745
		private BigInteger serialNumber;

		// Token: 0x04001672 RID: 5746
		private X509Name subject;

		// Token: 0x04001673 RID: 5747
		private byte[] subjectKeyIdentifier;

		// Token: 0x04001674 RID: 5748
		private SubjectPublicKeyInfo subjectPublicKey;

		// Token: 0x04001675 RID: 5749
		private DerObjectIdentifier subjectPublicKeyAlgID;
	}
}
