using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A3 RID: 1955
	public class SignerInfo : Asn1Encodable
	{
		// Token: 0x060045E6 RID: 17894 RVA: 0x00191C20 File Offset: 0x0018FE20
		public static SignerInfo GetInstance(object obj)
		{
			if (obj == null || obj is SignerInfo)
			{
				return (SignerInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SignerInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x00191C70 File Offset: 0x0018FE70
		public SignerInfo(SignerIdentifier sid, AlgorithmIdentifier digAlgorithm, Asn1Set authenticatedAttributes, AlgorithmIdentifier digEncryptionAlgorithm, Asn1OctetString encryptedDigest, Asn1Set unauthenticatedAttributes)
		{
			this.version = new DerInteger(sid.IsTagged ? 3 : 1);
			this.sid = sid;
			this.digAlgorithm = digAlgorithm;
			this.authenticatedAttributes = authenticatedAttributes;
			this.digEncryptionAlgorithm = digEncryptionAlgorithm;
			this.encryptedDigest = encryptedDigest;
			this.unauthenticatedAttributes = unauthenticatedAttributes;
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x00191CC8 File Offset: 0x0018FEC8
		public SignerInfo(SignerIdentifier sid, AlgorithmIdentifier digAlgorithm, Attributes authenticatedAttributes, AlgorithmIdentifier digEncryptionAlgorithm, Asn1OctetString encryptedDigest, Attributes unauthenticatedAttributes)
		{
			this.version = new DerInteger(sid.IsTagged ? 3 : 1);
			this.sid = sid;
			this.digAlgorithm = digAlgorithm;
			this.authenticatedAttributes = Asn1Set.GetInstance(authenticatedAttributes);
			this.digEncryptionAlgorithm = digEncryptionAlgorithm;
			this.encryptedDigest = encryptedDigest;
			this.unauthenticatedAttributes = Asn1Set.GetInstance(unauthenticatedAttributes);
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x00191D2C File Offset: 0x0018FF2C
		[Obsolete("Use 'GetInstance' instead")]
		public SignerInfo(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.version = (DerInteger)enumerator.Current;
			enumerator.MoveNext();
			this.sid = SignerIdentifier.GetInstance(enumerator.Current);
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

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060045EA RID: 17898 RVA: 0x00191E22 File Offset: 0x00190022
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060045EB RID: 17899 RVA: 0x00191E2A File Offset: 0x0019002A
		public SignerIdentifier SignerID
		{
			get
			{
				return this.sid;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060045EC RID: 17900 RVA: 0x00191E32 File Offset: 0x00190032
		public Asn1Set AuthenticatedAttributes
		{
			get
			{
				return this.authenticatedAttributes;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060045ED RID: 17901 RVA: 0x00191E3A File Offset: 0x0019003A
		public AlgorithmIdentifier DigestAlgorithm
		{
			get
			{
				return this.digAlgorithm;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060045EE RID: 17902 RVA: 0x00191E42 File Offset: 0x00190042
		public Asn1OctetString EncryptedDigest
		{
			get
			{
				return this.encryptedDigest;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060045EF RID: 17903 RVA: 0x00191E4A File Offset: 0x0019004A
		public AlgorithmIdentifier DigestEncryptionAlgorithm
		{
			get
			{
				return this.digEncryptionAlgorithm;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060045F0 RID: 17904 RVA: 0x00191E52 File Offset: 0x00190052
		public Asn1Set UnauthenticatedAttributes
		{
			get
			{
				return this.unauthenticatedAttributes;
			}
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x00191E5C File Offset: 0x0019005C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.sid,
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

		// Token: 0x04002D88 RID: 11656
		private DerInteger version;

		// Token: 0x04002D89 RID: 11657
		private SignerIdentifier sid;

		// Token: 0x04002D8A RID: 11658
		private AlgorithmIdentifier digAlgorithm;

		// Token: 0x04002D8B RID: 11659
		private Asn1Set authenticatedAttributes;

		// Token: 0x04002D8C RID: 11660
		private AlgorithmIdentifier digEncryptionAlgorithm;

		// Token: 0x04002D8D RID: 11661
		private Asn1OctetString encryptedDigest;

		// Token: 0x04002D8E RID: 11662
		private Asn1Set unauthenticatedAttributes;
	}
}
