using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200078A RID: 1930
	public class EnvelopedData : Asn1Encodable
	{
		// Token: 0x06004515 RID: 17685 RVA: 0x0018FA19 File Offset: 0x0018DC19
		public EnvelopedData(OriginatorInfo originatorInfo, Asn1Set recipientInfos, EncryptedContentInfo encryptedContentInfo, Asn1Set unprotectedAttrs)
		{
			this.version = new DerInteger(EnvelopedData.CalculateVersion(originatorInfo, recipientInfos, unprotectedAttrs));
			this.originatorInfo = originatorInfo;
			this.recipientInfos = recipientInfos;
			this.encryptedContentInfo = encryptedContentInfo;
			this.unprotectedAttrs = unprotectedAttrs;
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x0018FA54 File Offset: 0x0018DC54
		public EnvelopedData(OriginatorInfo originatorInfo, Asn1Set recipientInfos, EncryptedContentInfo encryptedContentInfo, Attributes unprotectedAttrs)
		{
			this.version = new DerInteger(EnvelopedData.CalculateVersion(originatorInfo, recipientInfos, Asn1Set.GetInstance(unprotectedAttrs)));
			this.originatorInfo = originatorInfo;
			this.recipientInfos = recipientInfos;
			this.encryptedContentInfo = encryptedContentInfo;
			this.unprotectedAttrs = Asn1Set.GetInstance(unprotectedAttrs);
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x0018FAA4 File Offset: 0x0018DCA4
		[Obsolete("Use 'GetInstance' instead")]
		public EnvelopedData(Asn1Sequence seq)
		{
			int num = 0;
			this.version = (DerInteger)seq[num++];
			object obj = seq[num++];
			if (obj is Asn1TaggedObject)
			{
				this.originatorInfo = OriginatorInfo.GetInstance((Asn1TaggedObject)obj, false);
				obj = seq[num++];
			}
			this.recipientInfos = Asn1Set.GetInstance(obj);
			this.encryptedContentInfo = EncryptedContentInfo.GetInstance(seq[num++]);
			if (seq.Count > num)
			{
				this.unprotectedAttrs = Asn1Set.GetInstance((Asn1TaggedObject)seq[num], false);
			}
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x0018FB44 File Offset: 0x0018DD44
		public static EnvelopedData GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return EnvelopedData.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x0018FB52 File Offset: 0x0018DD52
		public static EnvelopedData GetInstance(object obj)
		{
			if (obj is EnvelopedData)
			{
				return (EnvelopedData)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new EnvelopedData(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600451A RID: 17690 RVA: 0x0018FB73 File Offset: 0x0018DD73
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600451B RID: 17691 RVA: 0x0018FB7B File Offset: 0x0018DD7B
		public OriginatorInfo OriginatorInfo
		{
			get
			{
				return this.originatorInfo;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x0600451C RID: 17692 RVA: 0x0018FB83 File Offset: 0x0018DD83
		public Asn1Set RecipientInfos
		{
			get
			{
				return this.recipientInfos;
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x0600451D RID: 17693 RVA: 0x0018FB8B File Offset: 0x0018DD8B
		public EncryptedContentInfo EncryptedContentInfo
		{
			get
			{
				return this.encryptedContentInfo;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x0600451E RID: 17694 RVA: 0x0018FB93 File Offset: 0x0018DD93
		public Asn1Set UnprotectedAttrs
		{
			get
			{
				return this.unprotectedAttrs;
			}
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x0018FB9C File Offset: 0x0018DD9C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version
			});
			if (this.originatorInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.originatorInfo)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.recipientInfos,
				this.encryptedContentInfo
			});
			if (this.unprotectedAttrs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.unprotectedAttrs)
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x0018FC2C File Offset: 0x0018DE2C
		public static int CalculateVersion(OriginatorInfo originatorInfo, Asn1Set recipientInfos, Asn1Set unprotectedAttrs)
		{
			if (originatorInfo != null || unprotectedAttrs != null)
			{
				return 2;
			}
			using (IEnumerator enumerator = recipientInfos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (RecipientInfo.GetInstance(enumerator.Current).Version.Value.IntValue != 0)
					{
						return 2;
					}
				}
			}
			return 0;
		}

		// Token: 0x04002D3C RID: 11580
		private DerInteger version;

		// Token: 0x04002D3D RID: 11581
		private OriginatorInfo originatorInfo;

		// Token: 0x04002D3E RID: 11582
		private Asn1Set recipientInfos;

		// Token: 0x04002D3F RID: 11583
		private EncryptedContentInfo encryptedContentInfo;

		// Token: 0x04002D40 RID: 11584
		private Asn1Set unprotectedAttrs;
	}
}
