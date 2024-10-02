using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200077D RID: 1917
	public class AuthenticatedData : Asn1Encodable
	{
		// Token: 0x060044BB RID: 17595 RVA: 0x0018E814 File Offset: 0x0018CA14
		public AuthenticatedData(OriginatorInfo originatorInfo, Asn1Set recipientInfos, AlgorithmIdentifier macAlgorithm, AlgorithmIdentifier digestAlgorithm, ContentInfo encapsulatedContent, Asn1Set authAttrs, Asn1OctetString mac, Asn1Set unauthAttrs)
		{
			if ((digestAlgorithm != null || authAttrs != null) && (digestAlgorithm == null || authAttrs == null))
			{
				throw new ArgumentException("digestAlgorithm and authAttrs must be set together");
			}
			this.version = new DerInteger(AuthenticatedData.CalculateVersion(originatorInfo));
			this.originatorInfo = originatorInfo;
			this.macAlgorithm = macAlgorithm;
			this.digestAlgorithm = digestAlgorithm;
			this.recipientInfos = recipientInfos;
			this.encapsulatedContentInfo = encapsulatedContent;
			this.authAttrs = authAttrs;
			this.mac = mac;
			this.unauthAttrs = unauthAttrs;
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x0018E890 File Offset: 0x0018CA90
		private AuthenticatedData(Asn1Sequence seq)
		{
			int num = 0;
			this.version = (DerInteger)seq[num++];
			Asn1Encodable asn1Encodable = seq[num++];
			if (asn1Encodable is Asn1TaggedObject)
			{
				this.originatorInfo = OriginatorInfo.GetInstance((Asn1TaggedObject)asn1Encodable, false);
				asn1Encodable = seq[num++];
			}
			this.recipientInfos = Asn1Set.GetInstance(asn1Encodable);
			this.macAlgorithm = AlgorithmIdentifier.GetInstance(seq[num++]);
			asn1Encodable = seq[num++];
			if (asn1Encodable is Asn1TaggedObject)
			{
				this.digestAlgorithm = AlgorithmIdentifier.GetInstance((Asn1TaggedObject)asn1Encodable, false);
				asn1Encodable = seq[num++];
			}
			this.encapsulatedContentInfo = ContentInfo.GetInstance(asn1Encodable);
			asn1Encodable = seq[num++];
			if (asn1Encodable is Asn1TaggedObject)
			{
				this.authAttrs = Asn1Set.GetInstance((Asn1TaggedObject)asn1Encodable, false);
				asn1Encodable = seq[num++];
			}
			this.mac = Asn1OctetString.GetInstance(asn1Encodable);
			if (seq.Count > num)
			{
				this.unauthAttrs = Asn1Set.GetInstance((Asn1TaggedObject)seq[num], false);
			}
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0018E9AC File Offset: 0x0018CBAC
		public static AuthenticatedData GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return AuthenticatedData.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x0018E9BA File Offset: 0x0018CBBA
		public static AuthenticatedData GetInstance(object obj)
		{
			if (obj == null || obj is AuthenticatedData)
			{
				return (AuthenticatedData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AuthenticatedData((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid AuthenticatedData: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060044BF RID: 17599 RVA: 0x0018E9F7 File Offset: 0x0018CBF7
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060044C0 RID: 17600 RVA: 0x0018E9FF File Offset: 0x0018CBFF
		public OriginatorInfo OriginatorInfo
		{
			get
			{
				return this.originatorInfo;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060044C1 RID: 17601 RVA: 0x0018EA07 File Offset: 0x0018CC07
		public Asn1Set RecipientInfos
		{
			get
			{
				return this.recipientInfos;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060044C2 RID: 17602 RVA: 0x0018EA0F File Offset: 0x0018CC0F
		public AlgorithmIdentifier MacAlgorithm
		{
			get
			{
				return this.macAlgorithm;
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060044C3 RID: 17603 RVA: 0x0018EA17 File Offset: 0x0018CC17
		public AlgorithmIdentifier DigestAlgorithm
		{
			get
			{
				return this.digestAlgorithm;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060044C4 RID: 17604 RVA: 0x0018EA1F File Offset: 0x0018CC1F
		public ContentInfo EncapsulatedContentInfo
		{
			get
			{
				return this.encapsulatedContentInfo;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x0018EA27 File Offset: 0x0018CC27
		public Asn1Set AuthAttrs
		{
			get
			{
				return this.authAttrs;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x0018EA2F File Offset: 0x0018CC2F
		public Asn1OctetString Mac
		{
			get
			{
				return this.mac;
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060044C7 RID: 17607 RVA: 0x0018EA37 File Offset: 0x0018CC37
		public Asn1Set UnauthAttrs
		{
			get
			{
				return this.unauthAttrs;
			}
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x0018EA40 File Offset: 0x0018CC40
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
				this.macAlgorithm
			});
			if (this.digestAlgorithm != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.digestAlgorithm)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.encapsulatedContentInfo
			});
			if (this.authAttrs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, this.authAttrs)
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
					new DerTaggedObject(false, 3, this.unauthAttrs)
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0018EB40 File Offset: 0x0018CD40
		public static int CalculateVersion(OriginatorInfo origInfo)
		{
			if (origInfo == null)
			{
				return 0;
			}
			int result = 0;
			foreach (object obj in origInfo.Certificates)
			{
				if (obj is Asn1TaggedObject)
				{
					Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
					if (asn1TaggedObject.TagNo == 2)
					{
						result = 1;
					}
					else if (asn1TaggedObject.TagNo == 3)
					{
						result = 3;
						break;
					}
				}
			}
			foreach (object obj2 in origInfo.Crls)
			{
				if (obj2 is Asn1TaggedObject && ((Asn1TaggedObject)obj2).TagNo == 1)
				{
					result = 3;
					break;
				}
			}
			return result;
		}

		// Token: 0x04002CFF RID: 11519
		private DerInteger version;

		// Token: 0x04002D00 RID: 11520
		private OriginatorInfo originatorInfo;

		// Token: 0x04002D01 RID: 11521
		private Asn1Set recipientInfos;

		// Token: 0x04002D02 RID: 11522
		private AlgorithmIdentifier macAlgorithm;

		// Token: 0x04002D03 RID: 11523
		private AlgorithmIdentifier digestAlgorithm;

		// Token: 0x04002D04 RID: 11524
		private ContentInfo encapsulatedContentInfo;

		// Token: 0x04002D05 RID: 11525
		private Asn1Set authAttrs;

		// Token: 0x04002D06 RID: 11526
		private Asn1OctetString mac;

		// Token: 0x04002D07 RID: 11527
		private Asn1Set unauthAttrs;
	}
}
