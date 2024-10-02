using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C4 RID: 1732
	public class X509Extensions : Asn1Encodable
	{
		// Token: 0x06003FD8 RID: 16344 RVA: 0x0017B4E4 File Offset: 0x001796E4
		public static X509Extensions GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return X509Extensions.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0017B4F4 File Offset: 0x001796F4
		public static X509Extensions GetInstance(object obj)
		{
			if (obj == null || obj is X509Extensions)
			{
				return (X509Extensions)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new X509Extensions((Asn1Sequence)obj);
			}
			if (obj is Asn1TaggedObject)
			{
				return X509Extensions.GetInstance(((Asn1TaggedObject)obj).GetObject());
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0017B55C File Offset: 0x0017975C
		private X509Extensions(Asn1Sequence seq)
		{
			this.ordering = Platform.CreateArrayList();
			foreach (object obj in seq)
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
				if (instance.Count < 2 || instance.Count > 3)
				{
					throw new ArgumentException("Bad sequence size: " + instance.Count);
				}
				DerObjectIdentifier instance2 = DerObjectIdentifier.GetInstance(instance[0].ToAsn1Object());
				bool critical = instance.Count == 3 && DerBoolean.GetInstance(instance[1].ToAsn1Object()).IsTrue;
				Asn1OctetString instance3 = Asn1OctetString.GetInstance(instance[instance.Count - 1].ToAsn1Object());
				if (this.extensions.Contains(instance2))
				{
					throw new ArgumentException("repeated extension found: " + instance2);
				}
				this.extensions.Add(instance2, new X509Extension(critical, instance3));
				this.ordering.Add(instance2);
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0017B694 File Offset: 0x00179894
		public X509Extensions(IDictionary extensions) : this(null, extensions)
		{
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x0017B6A0 File Offset: 0x001798A0
		public X509Extensions(IList ordering, IDictionary extensions)
		{
			if (ordering == null)
			{
				this.ordering = Platform.CreateArrayList(extensions.Keys);
			}
			else
			{
				this.ordering = Platform.CreateArrayList(ordering);
			}
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)extensions[key]);
			}
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x0017B740 File Offset: 0x00179940
		public X509Extensions(IList oids, IList values)
		{
			this.ordering = Platform.CreateArrayList(oids);
			int num = 0;
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)values[num++]);
			}
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0017B7D0 File Offset: 0x001799D0
		[Obsolete]
		public X509Extensions(Hashtable extensions) : this(null, extensions)
		{
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0017B7DC File Offset: 0x001799DC
		[Obsolete]
		public X509Extensions(ArrayList ordering, Hashtable extensions)
		{
			if (ordering == null)
			{
				this.ordering = Platform.CreateArrayList(extensions.Keys);
			}
			else
			{
				this.ordering = Platform.CreateArrayList(ordering);
			}
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)extensions[key]);
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0017B87C File Offset: 0x00179A7C
		[Obsolete]
		public X509Extensions(ArrayList oids, ArrayList values)
		{
			this.ordering = Platform.CreateArrayList(oids);
			int num = 0;
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)values[num++]);
			}
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0017B90C File Offset: 0x00179B0C
		[Obsolete("Use ExtensionOids IEnumerable property")]
		public IEnumerator Oids()
		{
			return this.ExtensionOids.GetEnumerator();
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x0017B919 File Offset: 0x00179B19
		public IEnumerable ExtensionOids
		{
			get
			{
				return new EnumerableProxy(this.ordering);
			}
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0017B926 File Offset: 0x00179B26
		public X509Extension GetExtension(DerObjectIdentifier oid)
		{
			return (X509Extension)this.extensions[oid];
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0017B93C File Offset: 0x00179B3C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
				X509Extension x509Extension = (X509Extension)this.extensions[derObjectIdentifier];
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(new Asn1Encodable[]
				{
					derObjectIdentifier
				});
				if (x509Extension.IsCritical)
				{
					asn1EncodableVector2.Add(new Asn1Encodable[]
					{
						DerBoolean.True
					});
				}
				asn1EncodableVector2.Add(new Asn1Encodable[]
				{
					x509Extension.Value
				});
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerSequence(asn1EncodableVector2)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0017BA10 File Offset: 0x00179C10
		public bool Equivalent(X509Extensions other)
		{
			if (this.extensions.Count != other.extensions.Count)
			{
				return false;
			}
			foreach (object obj in this.extensions.Keys)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				if (!this.extensions[key].Equals(other.extensions[key]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0017BAA8 File Offset: 0x00179CA8
		public DerObjectIdentifier[] GetExtensionOids()
		{
			return X509Extensions.ToOidArray(this.ordering);
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0017BAB5 File Offset: 0x00179CB5
		public DerObjectIdentifier[] GetNonCriticalExtensionOids()
		{
			return this.GetExtensionOids(false);
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0017BABE File Offset: 0x00179CBE
		public DerObjectIdentifier[] GetCriticalExtensionOids()
		{
			return this.GetExtensionOids(true);
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0017BAC8 File Offset: 0x00179CC8
		private DerObjectIdentifier[] GetExtensionOids(bool isCritical)
		{
			IList list = Platform.CreateArrayList();
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
				if (((X509Extension)this.extensions[derObjectIdentifier]).IsCritical == isCritical)
				{
					list.Add(derObjectIdentifier);
				}
			}
			return X509Extensions.ToOidArray(list);
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0017BB48 File Offset: 0x00179D48
		private static DerObjectIdentifier[] ToOidArray(IList oids)
		{
			DerObjectIdentifier[] array = new DerObjectIdentifier[oids.Count];
			oids.CopyTo(array, 0);
			return array;
		}

		// Token: 0x04002864 RID: 10340
		public static readonly DerObjectIdentifier SubjectDirectoryAttributes = new DerObjectIdentifier("2.5.29.9");

		// Token: 0x04002865 RID: 10341
		public static readonly DerObjectIdentifier SubjectKeyIdentifier = new DerObjectIdentifier("2.5.29.14");

		// Token: 0x04002866 RID: 10342
		public static readonly DerObjectIdentifier KeyUsage = new DerObjectIdentifier("2.5.29.15");

		// Token: 0x04002867 RID: 10343
		public static readonly DerObjectIdentifier PrivateKeyUsagePeriod = new DerObjectIdentifier("2.5.29.16");

		// Token: 0x04002868 RID: 10344
		public static readonly DerObjectIdentifier SubjectAlternativeName = new DerObjectIdentifier("2.5.29.17");

		// Token: 0x04002869 RID: 10345
		public static readonly DerObjectIdentifier IssuerAlternativeName = new DerObjectIdentifier("2.5.29.18");

		// Token: 0x0400286A RID: 10346
		public static readonly DerObjectIdentifier BasicConstraints = new DerObjectIdentifier("2.5.29.19");

		// Token: 0x0400286B RID: 10347
		public static readonly DerObjectIdentifier CrlNumber = new DerObjectIdentifier("2.5.29.20");

		// Token: 0x0400286C RID: 10348
		public static readonly DerObjectIdentifier ReasonCode = new DerObjectIdentifier("2.5.29.21");

		// Token: 0x0400286D RID: 10349
		public static readonly DerObjectIdentifier InstructionCode = new DerObjectIdentifier("2.5.29.23");

		// Token: 0x0400286E RID: 10350
		public static readonly DerObjectIdentifier InvalidityDate = new DerObjectIdentifier("2.5.29.24");

		// Token: 0x0400286F RID: 10351
		public static readonly DerObjectIdentifier DeltaCrlIndicator = new DerObjectIdentifier("2.5.29.27");

		// Token: 0x04002870 RID: 10352
		public static readonly DerObjectIdentifier IssuingDistributionPoint = new DerObjectIdentifier("2.5.29.28");

		// Token: 0x04002871 RID: 10353
		public static readonly DerObjectIdentifier CertificateIssuer = new DerObjectIdentifier("2.5.29.29");

		// Token: 0x04002872 RID: 10354
		public static readonly DerObjectIdentifier NameConstraints = new DerObjectIdentifier("2.5.29.30");

		// Token: 0x04002873 RID: 10355
		public static readonly DerObjectIdentifier CrlDistributionPoints = new DerObjectIdentifier("2.5.29.31");

		// Token: 0x04002874 RID: 10356
		public static readonly DerObjectIdentifier CertificatePolicies = new DerObjectIdentifier("2.5.29.32");

		// Token: 0x04002875 RID: 10357
		public static readonly DerObjectIdentifier PolicyMappings = new DerObjectIdentifier("2.5.29.33");

		// Token: 0x04002876 RID: 10358
		public static readonly DerObjectIdentifier AuthorityKeyIdentifier = new DerObjectIdentifier("2.5.29.35");

		// Token: 0x04002877 RID: 10359
		public static readonly DerObjectIdentifier PolicyConstraints = new DerObjectIdentifier("2.5.29.36");

		// Token: 0x04002878 RID: 10360
		public static readonly DerObjectIdentifier ExtendedKeyUsage = new DerObjectIdentifier("2.5.29.37");

		// Token: 0x04002879 RID: 10361
		public static readonly DerObjectIdentifier FreshestCrl = new DerObjectIdentifier("2.5.29.46");

		// Token: 0x0400287A RID: 10362
		public static readonly DerObjectIdentifier InhibitAnyPolicy = new DerObjectIdentifier("2.5.29.54");

		// Token: 0x0400287B RID: 10363
		public static readonly DerObjectIdentifier AuthorityInfoAccess = new DerObjectIdentifier("1.3.6.1.5.5.7.1.1");

		// Token: 0x0400287C RID: 10364
		public static readonly DerObjectIdentifier SubjectInfoAccess = new DerObjectIdentifier("1.3.6.1.5.5.7.1.11");

		// Token: 0x0400287D RID: 10365
		public static readonly DerObjectIdentifier LogoType = new DerObjectIdentifier("1.3.6.1.5.5.7.1.12");

		// Token: 0x0400287E RID: 10366
		public static readonly DerObjectIdentifier BiometricInfo = new DerObjectIdentifier("1.3.6.1.5.5.7.1.2");

		// Token: 0x0400287F RID: 10367
		public static readonly DerObjectIdentifier QCStatements = new DerObjectIdentifier("1.3.6.1.5.5.7.1.3");

		// Token: 0x04002880 RID: 10368
		public static readonly DerObjectIdentifier AuditIdentity = new DerObjectIdentifier("1.3.6.1.5.5.7.1.4");

		// Token: 0x04002881 RID: 10369
		public static readonly DerObjectIdentifier NoRevAvail = new DerObjectIdentifier("2.5.29.56");

		// Token: 0x04002882 RID: 10370
		public static readonly DerObjectIdentifier TargetInformation = new DerObjectIdentifier("2.5.29.55");

		// Token: 0x04002883 RID: 10371
		public static readonly DerObjectIdentifier ExpiredCertsOnCrl = new DerObjectIdentifier("2.5.29.60");

		// Token: 0x04002884 RID: 10372
		private readonly IDictionary extensions = Platform.CreateHashtable();

		// Token: 0x04002885 RID: 10373
		private readonly IList ordering;
	}
}
