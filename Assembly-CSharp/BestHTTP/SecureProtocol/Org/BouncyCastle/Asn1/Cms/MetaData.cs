using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000793 RID: 1939
	public class MetaData : Asn1Encodable
	{
		// Token: 0x0600455D RID: 17757 RVA: 0x001905A5 File Offset: 0x0018E7A5
		public MetaData(DerBoolean hashProtected, DerUtf8String fileName, DerIA5String mediaType, Attributes otherMetaData)
		{
			this.hashProtected = hashProtected;
			this.fileName = fileName;
			this.mediaType = mediaType;
			this.otherMetaData = otherMetaData;
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x001905CC File Offset: 0x0018E7CC
		private MetaData(Asn1Sequence seq)
		{
			this.hashProtected = DerBoolean.GetInstance(seq[0]);
			int num = 1;
			if (num < seq.Count && seq[num] is DerUtf8String)
			{
				this.fileName = DerUtf8String.GetInstance(seq[num++]);
			}
			if (num < seq.Count && seq[num] is DerIA5String)
			{
				this.mediaType = DerIA5String.GetInstance(seq[num++]);
			}
			if (num < seq.Count)
			{
				this.otherMetaData = Attributes.GetInstance(seq[num++]);
			}
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x0019066C File Offset: 0x0018E86C
		public static MetaData GetInstance(object obj)
		{
			if (obj is MetaData)
			{
				return (MetaData)obj;
			}
			if (obj != null)
			{
				return new MetaData(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00190690 File Offset: 0x0018E890
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.hashProtected
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.fileName,
				this.mediaType,
				this.otherMetaData
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06004561 RID: 17761 RVA: 0x001906DF File Offset: 0x0018E8DF
		public virtual bool IsHashProtected
		{
			get
			{
				return this.hashProtected.IsTrue;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06004562 RID: 17762 RVA: 0x001906EC File Offset: 0x0018E8EC
		public virtual DerUtf8String FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06004563 RID: 17763 RVA: 0x001906F4 File Offset: 0x0018E8F4
		public virtual DerIA5String MediaType
		{
			get
			{
				return this.mediaType;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06004564 RID: 17764 RVA: 0x001906FC File Offset: 0x0018E8FC
		public virtual Attributes OtherMetaData
		{
			get
			{
				return this.otherMetaData;
			}
		}

		// Token: 0x04002D5A RID: 11610
		private DerBoolean hashProtected;

		// Token: 0x04002D5B RID: 11611
		private DerUtf8String fileName;

		// Token: 0x04002D5C RID: 11612
		private DerIA5String mediaType;

		// Token: 0x04002D5D RID: 11613
		private Attributes otherMetaData;
	}
}
