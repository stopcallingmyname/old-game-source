using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000704 RID: 1796
	public class SignerInfo : Asn1Encodable
	{
		// Token: 0x060041A9 RID: 16809 RVA: 0x00183BD7 File Offset: 0x00181DD7
		public static SignerInfo GetInstance(object obj)
		{
			if (obj is SignerInfo)
			{
				return (SignerInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SignerInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x00183C16 File Offset: 0x00181E16
		public SignerInfo(DerInteger version, IssuerAndSerialNumber issuerAndSerialNumber, AlgorithmIdentifier digAlgorithm, Asn1Set authenticatedAttributes, AlgorithmIdentifier digEncryptionAlgorithm, Asn1OctetString encryptedDigest, Asn1Set unauthenticatedAttributes)
		{
			this.version = version;
			this.issuerAndSerialNumber = issuerAndSerialNumber;
			this.digAlgorithm = digAlgorithm;
			this.authenticatedAttributes = authenticatedAttributes;
			this.digEncryptionAlgorithm = digEncryptionAlgorithm;
			this.encryptedDigest = encryptedDigest;
			this.unauthenticatedAttributes = unauthenticatedAttributes;
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x00183C54 File Offset: 0x00181E54
		public SignerInfo(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.version = (DerInteger)enumerator.Current;
			enumerator.MoveNext();
			this.issuerAndSerialNumber = IssuerAndSerialNumber.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			this.digAlgorithm = AlgorithmIdentifier.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			object obj = enumerator.Current;
			if (obj is Asn1TaggedObject)
			{
				this.authenticatedAttributes = Asn1Set.GetInstance((Asn1TaggedObject)obj, false);
				enumerator.MoveNext();
				this.digEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(enumerator.Current);
			}
			else
			{
				this.authenticatedAttributes = null;
				this.digEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(obj);
			}
			enumerator.MoveNext();
			this.encryptedDigest = Asn1OctetString.GetInstance(enumerator.Current);
			if (enumerator.MoveNext())
			{
				this.unauthenticatedAttributes = Asn1Set.GetInstance((Asn1TaggedObject)enumerator.Current, false);
				return;
			}
			this.unauthenticatedAttributes = null;
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x00183D4A File Offset: 0x00181F4A
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x00183D52 File Offset: 0x00181F52
		public IssuerAndSerialNumber IssuerAndSerialNumber
		{
			get
			{
				return this.issuerAndSerialNumber;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x00183D5A File Offset: 0x00181F5A
		public Asn1Set AuthenticatedAttributes
		{
			get
			{
				return this.authenticatedAttributes;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x00183D62 File Offset: 0x00181F62
		public AlgorithmIdentifier DigestAlgorithm
		{
			get
			{
				return this.digAlgorithm;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x00183D6A File Offset: 0x00181F6A
		public Asn1OctetString EncryptedDigest
		{
			get
			{
				return this.encryptedDigest;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x060041B1 RID: 16817 RVA: 0x00183D72 File Offset: 0x00181F72
		public AlgorithmIdentifier DigestEncryptionAlgorithm
		{
			get
			{
				return this.digEncryptionAlgorithm;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x00183D7A File Offset: 0x00181F7A
		public Asn1Set UnauthenticatedAttributes
		{
			get
			{
				return this.unauthenticatedAttributes;
			}
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x00183D84 File Offset: 0x00181F84
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.issuerAndSerialNumber,
				this.digAlgorithm
			});
			if (this.authenticatedAttributes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.authenticatedAttributes)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.digEncryptionAlgorithm,
				this.encryptedDigest
			});
			if (this.unauthenticatedAttributes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.unauthenticatedAttributes)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002A8B RID: 10891
		private DerInteger version;

		// Token: 0x04002A8C RID: 10892
		private IssuerAndSerialNumber issuerAndSerialNumber;

		// Token: 0x04002A8D RID: 10893
		private AlgorithmIdentifier digAlgorithm;

		// Token: 0x04002A8E RID: 10894
		private Asn1Set authenticatedAttributes;

		// Token: 0x04002A8F RID: 10895
		private AlgorithmIdentifier digEncryptionAlgorithm;

		// Token: 0x04002A90 RID: 10896
		private Asn1OctetString encryptedDigest;

		// Token: 0x04002A91 RID: 10897
		private Asn1Set unauthenticatedAttributes;
	}
}
