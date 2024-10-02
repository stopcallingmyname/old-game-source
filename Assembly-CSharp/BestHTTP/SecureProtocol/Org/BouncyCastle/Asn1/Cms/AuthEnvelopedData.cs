using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200077F RID: 1919
	public class AuthEnvelopedData : Asn1Encodable
	{
		// Token: 0x060044D4 RID: 17620 RVA: 0x0018EEB0 File Offset: 0x0018D0B0
		public AuthEnvelopedData(OriginatorInfo originatorInfo, Asn1Set recipientInfos, EncryptedContentInfo authEncryptedContentInfo, Asn1Set authAttrs, Asn1OctetString mac, Asn1Set unauthAttrs)
		{
			this.version = new DerInteger(0);
			this.originatorInfo = originatorInfo;
			this.recipientInfos = recipientInfos;
			this.authEncryptedContentInfo = authEncryptedContentInfo;
			this.authAttrs = authAttrs;
			this.mac = mac;
			this.unauthAttrs = unauthAttrs;
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x0018EEFC File Offset: 0x0018D0FC
		private AuthEnvelopedData(Asn1Sequence seq)
		{
			int num = 0;
			Asn1Object asn1Object = seq[num++].ToAsn1Object();
			this.version = (DerInteger)asn1Object;
			asn1Object = seq[num++].ToAsn1Object();
			if (asn1Object is Asn1TaggedObject)
			{
				this.originatorInfo = OriginatorInfo.GetInstance((Asn1TaggedObject)asn1Object, false);
				asn1Object = seq[num++].ToAsn1Object();
			}
			this.recipientInfos = Asn1Set.GetInstance(asn1Object);
			asn1Object = seq[num++].ToAsn1Object();
			this.authEncryptedContentInfo = EncryptedContentInfo.GetInstance(asn1Object);
			asn1Object = seq[num++].ToAsn1Object();
			if (asn1Object is Asn1TaggedObject)
			{
				this.authAttrs = Asn1Set.GetInstance((Asn1TaggedObject)asn1Object, false);
				asn1Object = seq[num++].ToAsn1Object();
			}
			this.mac = Asn1OctetString.GetInstance(asn1Object);
			if (seq.Count > num)
			{
				asn1Object = seq[num++].ToAsn1Object();
				this.unauthAttrs = Asn1Set.GetInstance((Asn1TaggedObject)asn1Object, false);
			}
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x0018F007 File Offset: 0x0018D207
		public static AuthEnvelopedData GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return AuthEnvelopedData.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x0018F015 File Offset: 0x0018D215
		public static AuthEnvelopedData GetInstance(object obj)
		{
			if (obj == null || obj is AuthEnvelopedData)
			{
				return (AuthEnvelopedData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AuthEnvelopedData((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid AuthEnvelopedData: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x0018F052 File Offset: 0x0018D252
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x060044D9 RID: 17625 RVA: 0x0018F05A File Offset: 0x0018D25A
		public OriginatorInfo OriginatorInfo
		{
			get
			{
				return this.originatorInfo;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060044DA RID: 17626 RVA: 0x0018F062 File Offset: 0x0018D262
		public Asn1Set RecipientInfos
		{
			get
			{
				return this.recipientInfos;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060044DB RID: 17627 RVA: 0x0018F06A File Offset: 0x0018D26A
		public EncryptedContentInfo AuthEncryptedContentInfo
		{
			get
			{
				return this.authEncryptedContentInfo;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060044DC RID: 17628 RVA: 0x0018F072 File Offset: 0x0018D272
		public Asn1Set AuthAttrs
		{
			get
			{
				return this.authAttrs;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060044DD RID: 17629 RVA: 0x0018F07A File Offset: 0x0018D27A
		public Asn1OctetString Mac
		{
			get
			{
				return this.mac;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060044DE RID: 17630 RVA: 0x0018F082 File Offset: 0x0018D282
		public Asn1Set UnauthAttrs
		{
			get
			{
				return this.unauthAttrs;
			}
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x0018F08C File Offset: 0x0018D28C
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
				this.authEncryptedContentInfo
			});
			if (this.authAttrs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.authAttrs)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.mac
			});
			if (this.unauthAttrs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, this.unauthAttrs)
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D0C RID: 11532
		private DerInteger version;

		// Token: 0x04002D0D RID: 11533
		private OriginatorInfo originatorInfo;

		// Token: 0x04002D0E RID: 11534
		private Asn1Set recipientInfos;

		// Token: 0x04002D0F RID: 11535
		private EncryptedContentInfo authEncryptedContentInfo;

		// Token: 0x04002D10 RID: 11536
		private Asn1Set authAttrs;

		// Token: 0x04002D11 RID: 11537
		private Asn1OctetString mac;

		// Token: 0x04002D12 RID: 11538
		private Asn1Set unauthAttrs;
	}
}
