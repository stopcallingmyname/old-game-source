using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000784 RID: 1924
	public class CompressedDataParser
	{
		// Token: 0x060044F4 RID: 17652 RVA: 0x0018F50C File Offset: 0x0018D70C
		public CompressedDataParser(Asn1SequenceParser seq)
		{
			this._version = (DerInteger)seq.ReadObject();
			this._compressionAlgorithm = AlgorithmIdentifier.GetInstance(seq.ReadObject().ToAsn1Object());
			this._encapContentInfo = new ContentInfoParser((Asn1SequenceParser)seq.ReadObject());
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060044F5 RID: 17653 RVA: 0x0018F55C File Offset: 0x0018D75C
		public DerInteger Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060044F6 RID: 17654 RVA: 0x0018F564 File Offset: 0x0018D764
		public AlgorithmIdentifier CompressionAlgorithmIdentifier
		{
			get
			{
				return this._compressionAlgorithm;
			}
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x0018F56C File Offset: 0x0018D76C
		public ContentInfoParser GetEncapContentInfo()
		{
			return this._encapContentInfo;
		}

		// Token: 0x04002D2C RID: 11564
		private DerInteger _version;

		// Token: 0x04002D2D RID: 11565
		private AlgorithmIdentifier _compressionAlgorithm;

		// Token: 0x04002D2E RID: 11566
		private ContentInfoParser _encapContentInfo;
	}
}
