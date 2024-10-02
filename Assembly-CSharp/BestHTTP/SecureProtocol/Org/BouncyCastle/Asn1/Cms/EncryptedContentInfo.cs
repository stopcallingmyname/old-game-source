using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000787 RID: 1927
	public class EncryptedContentInfo : Asn1Encodable
	{
		// Token: 0x06004502 RID: 17666 RVA: 0x0018F716 File Offset: 0x0018D916
		public EncryptedContentInfo(DerObjectIdentifier contentType, AlgorithmIdentifier contentEncryptionAlgorithm, Asn1OctetString encryptedContent)
		{
			this.contentType = contentType;
			this.contentEncryptionAlgorithm = contentEncryptionAlgorithm;
			this.encryptedContent = encryptedContent;
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x0018F734 File Offset: 0x0018D934
		public EncryptedContentInfo(Asn1Sequence seq)
		{
			this.contentType = (DerObjectIdentifier)seq[0];
			this.contentEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(seq[1]);
			if (seq.Count > 2)
			{
				this.encryptedContent = Asn1OctetString.GetInstance((Asn1TaggedObject)seq[2], false);
			}
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0018F78C File Offset: 0x0018D98C
		public static EncryptedContentInfo GetInstance(object obj)
		{
			if (obj == null || obj is EncryptedContentInfo)
			{
				return (EncryptedContentInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new EncryptedContentInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid EncryptedContentInfo: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06004505 RID: 17669 RVA: 0x0018F7C9 File Offset: 0x0018D9C9
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06004506 RID: 17670 RVA: 0x0018F7D1 File Offset: 0x0018D9D1
		public AlgorithmIdentifier ContentEncryptionAlgorithm
		{
			get
			{
				return this.contentEncryptionAlgorithm;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004507 RID: 17671 RVA: 0x0018F7D9 File Offset: 0x0018D9D9
		public Asn1OctetString EncryptedContent
		{
			get
			{
				return this.encryptedContent;
			}
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x0018F7E4 File Offset: 0x0018D9E4
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.contentType,
				this.contentEncryptionAlgorithm
			});
			if (this.encryptedContent != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new BerTaggedObject(false, 0, this.encryptedContent)
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D33 RID: 11571
		private DerObjectIdentifier contentType;

		// Token: 0x04002D34 RID: 11572
		private AlgorithmIdentifier contentEncryptionAlgorithm;

		// Token: 0x04002D35 RID: 11573
		private Asn1OctetString encryptedContent;
	}
}
