using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000783 RID: 1923
	public class CompressedData : Asn1Encodable
	{
		// Token: 0x060044EC RID: 17644 RVA: 0x0018F420 File Offset: 0x0018D620
		public CompressedData(AlgorithmIdentifier compressionAlgorithm, ContentInfo encapContentInfo)
		{
			this.version = new DerInteger(0);
			this.compressionAlgorithm = compressionAlgorithm;
			this.encapContentInfo = encapContentInfo;
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x0018F442 File Offset: 0x0018D642
		public CompressedData(Asn1Sequence seq)
		{
			this.version = (DerInteger)seq[0];
			this.compressionAlgorithm = AlgorithmIdentifier.GetInstance(seq[1]);
			this.encapContentInfo = ContentInfo.GetInstance(seq[2]);
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x0018F480 File Offset: 0x0018D680
		public static CompressedData GetInstance(Asn1TaggedObject ato, bool explicitly)
		{
			return CompressedData.GetInstance(Asn1Sequence.GetInstance(ato, explicitly));
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x0018F48E File Offset: 0x0018D68E
		public static CompressedData GetInstance(object obj)
		{
			if (obj == null || obj is CompressedData)
			{
				return (CompressedData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CompressedData((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid CompressedData: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060044F0 RID: 17648 RVA: 0x0018F4CB File Offset: 0x0018D6CB
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060044F1 RID: 17649 RVA: 0x0018F4D3 File Offset: 0x0018D6D3
		public AlgorithmIdentifier CompressionAlgorithmIdentifier
		{
			get
			{
				return this.compressionAlgorithm;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060044F2 RID: 17650 RVA: 0x0018F4DB File Offset: 0x0018D6DB
		public ContentInfo EncapContentInfo
		{
			get
			{
				return this.encapContentInfo;
			}
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x0018F4E3 File Offset: 0x0018D6E3
		public override Asn1Object ToAsn1Object()
		{
			return new BerSequence(new Asn1Encodable[]
			{
				this.version,
				this.compressionAlgorithm,
				this.encapContentInfo
			});
		}

		// Token: 0x04002D29 RID: 11561
		private DerInteger version;

		// Token: 0x04002D2A RID: 11562
		private AlgorithmIdentifier compressionAlgorithm;

		// Token: 0x04002D2B RID: 11563
		private ContentInfo encapContentInfo;
	}
}
