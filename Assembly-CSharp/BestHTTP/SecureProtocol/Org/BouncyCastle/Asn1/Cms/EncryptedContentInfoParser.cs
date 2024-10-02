using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000788 RID: 1928
	public class EncryptedContentInfoParser
	{
		// Token: 0x06004509 RID: 17673 RVA: 0x0018F839 File Offset: 0x0018DA39
		public EncryptedContentInfoParser(Asn1SequenceParser seq)
		{
			this._contentType = (DerObjectIdentifier)seq.ReadObject();
			this._contentEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(seq.ReadObject().ToAsn1Object());
			this._encryptedContent = (Asn1TaggedObjectParser)seq.ReadObject();
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x0600450A RID: 17674 RVA: 0x0018F879 File Offset: 0x0018DA79
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this._contentType;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600450B RID: 17675 RVA: 0x0018F881 File Offset: 0x0018DA81
		public AlgorithmIdentifier ContentEncryptionAlgorithm
		{
			get
			{
				return this._contentEncryptionAlgorithm;
			}
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x0018F889 File Offset: 0x0018DA89
		public IAsn1Convertible GetEncryptedContent(int tag)
		{
			return this._encryptedContent.GetObjectParser(tag, false);
		}

		// Token: 0x04002D36 RID: 11574
		private DerObjectIdentifier _contentType;

		// Token: 0x04002D37 RID: 11575
		private AlgorithmIdentifier _contentEncryptionAlgorithm;

		// Token: 0x04002D38 RID: 11576
		private Asn1TaggedObjectParser _encryptedContent;
	}
}
