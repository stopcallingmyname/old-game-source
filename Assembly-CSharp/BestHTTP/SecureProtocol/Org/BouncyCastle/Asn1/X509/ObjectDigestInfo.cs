using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A7 RID: 1703
	public class ObjectDigestInfo : Asn1Encodable
	{
		// Token: 0x06003EED RID: 16109 RVA: 0x00178E98 File Offset: 0x00177098
		public static ObjectDigestInfo GetInstance(object obj)
		{
			if (obj == null || obj is ObjectDigestInfo)
			{
				return (ObjectDigestInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ObjectDigestInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x00178EE5 File Offset: 0x001770E5
		public static ObjectDigestInfo GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return ObjectDigestInfo.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x00178EF3 File Offset: 0x001770F3
		public ObjectDigestInfo(int digestedObjectType, string otherObjectTypeID, AlgorithmIdentifier digestAlgorithm, byte[] objectDigest)
		{
			this.digestedObjectType = new DerEnumerated(digestedObjectType);
			if (digestedObjectType == 2)
			{
				this.otherObjectTypeID = new DerObjectIdentifier(otherObjectTypeID);
			}
			this.digestAlgorithm = digestAlgorithm;
			this.objectDigest = new DerBitString(objectDigest);
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x00178F2C File Offset: 0x0017712C
		private ObjectDigestInfo(Asn1Sequence seq)
		{
			if (seq.Count > 4 || seq.Count < 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.digestedObjectType = DerEnumerated.GetInstance(seq[0]);
			int num = 0;
			if (seq.Count == 4)
			{
				this.otherObjectTypeID = DerObjectIdentifier.GetInstance(seq[1]);
				num++;
			}
			this.digestAlgorithm = AlgorithmIdentifier.GetInstance(seq[1 + num]);
			this.objectDigest = DerBitString.GetInstance(seq[2 + num]);
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06003EF1 RID: 16113 RVA: 0x00178FC7 File Offset: 0x001771C7
		public DerEnumerated DigestedObjectType
		{
			get
			{
				return this.digestedObjectType;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06003EF2 RID: 16114 RVA: 0x00178FCF File Offset: 0x001771CF
		public DerObjectIdentifier OtherObjectTypeID
		{
			get
			{
				return this.otherObjectTypeID;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06003EF3 RID: 16115 RVA: 0x00178FD7 File Offset: 0x001771D7
		public AlgorithmIdentifier DigestAlgorithm
		{
			get
			{
				return this.digestAlgorithm;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06003EF4 RID: 16116 RVA: 0x00178FDF File Offset: 0x001771DF
		public DerBitString ObjectDigest
		{
			get
			{
				return this.objectDigest;
			}
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x00178FE8 File Offset: 0x001771E8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.digestedObjectType
			});
			if (this.otherObjectTypeID != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.otherObjectTypeID
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.digestAlgorithm,
				this.objectDigest
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040027F5 RID: 10229
		public const int PublicKey = 0;

		// Token: 0x040027F6 RID: 10230
		public const int PublicKeyCert = 1;

		// Token: 0x040027F7 RID: 10231
		public const int OtherObjectDigest = 2;

		// Token: 0x040027F8 RID: 10232
		internal readonly DerEnumerated digestedObjectType;

		// Token: 0x040027F9 RID: 10233
		internal readonly DerObjectIdentifier otherObjectTypeID;

		// Token: 0x040027FA RID: 10234
		internal readonly AlgorithmIdentifier digestAlgorithm;

		// Token: 0x040027FB RID: 10235
		internal readonly DerBitString objectDigest;
	}
}
