using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000701 RID: 1793
	public class RsassaPssParameters : Asn1Encodable
	{
		// Token: 0x0600418E RID: 16782 RVA: 0x00183624 File Offset: 0x00181824
		public static RsassaPssParameters GetInstance(object obj)
		{
			if (obj == null || obj is RsassaPssParameters)
			{
				return (RsassaPssParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RsassaPssParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x00183671 File Offset: 0x00181871
		public RsassaPssParameters()
		{
			this.hashAlgorithm = RsassaPssParameters.DefaultHashAlgorithm;
			this.maskGenAlgorithm = RsassaPssParameters.DefaultMaskGenFunction;
			this.saltLength = RsassaPssParameters.DefaultSaltLength;
			this.trailerField = RsassaPssParameters.DefaultTrailerField;
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x001836A5 File Offset: 0x001818A5
		public RsassaPssParameters(AlgorithmIdentifier hashAlgorithm, AlgorithmIdentifier maskGenAlgorithm, DerInteger saltLength, DerInteger trailerField)
		{
			this.hashAlgorithm = hashAlgorithm;
			this.maskGenAlgorithm = maskGenAlgorithm;
			this.saltLength = saltLength;
			this.trailerField = trailerField;
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x001836CC File Offset: 0x001818CC
		public RsassaPssParameters(Asn1Sequence seq)
		{
			this.hashAlgorithm = RsassaPssParameters.DefaultHashAlgorithm;
			this.maskGenAlgorithm = RsassaPssParameters.DefaultMaskGenFunction;
			this.saltLength = RsassaPssParameters.DefaultSaltLength;
			this.trailerField = RsassaPssParameters.DefaultTrailerField;
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[num];
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.hashAlgorithm = AlgorithmIdentifier.GetInstance(asn1TaggedObject, true);
					break;
				case 1:
					this.maskGenAlgorithm = AlgorithmIdentifier.GetInstance(asn1TaggedObject, true);
					break;
				case 2:
					this.saltLength = DerInteger.GetInstance(asn1TaggedObject, true);
					break;
				case 3:
					this.trailerField = DerInteger.GetInstance(asn1TaggedObject, true);
					break;
				default:
					throw new ArgumentException("unknown tag");
				}
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x0018378F File Offset: 0x0018198F
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x00183797 File Offset: 0x00181997
		public AlgorithmIdentifier MaskGenAlgorithm
		{
			get
			{
				return this.maskGenAlgorithm;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06004194 RID: 16788 RVA: 0x0018379F File Offset: 0x0018199F
		public DerInteger SaltLength
		{
			get
			{
				return this.saltLength;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x001837A7 File Offset: 0x001819A7
		public DerInteger TrailerField
		{
			get
			{
				return this.trailerField;
			}
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x001837B0 File Offset: 0x001819B0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (!this.hashAlgorithm.Equals(RsassaPssParameters.DefaultHashAlgorithm))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.hashAlgorithm)
				});
			}
			if (!this.maskGenAlgorithm.Equals(RsassaPssParameters.DefaultMaskGenFunction))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.maskGenAlgorithm)
				});
			}
			if (!this.saltLength.Equals(RsassaPssParameters.DefaultSaltLength))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.saltLength)
				});
			}
			if (!this.trailerField.Equals(RsassaPssParameters.DefaultTrailerField))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 3, this.trailerField)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002A7A RID: 10874
		private AlgorithmIdentifier hashAlgorithm;

		// Token: 0x04002A7B RID: 10875
		private AlgorithmIdentifier maskGenAlgorithm;

		// Token: 0x04002A7C RID: 10876
		private DerInteger saltLength;

		// Token: 0x04002A7D RID: 10877
		private DerInteger trailerField;

		// Token: 0x04002A7E RID: 10878
		public static readonly AlgorithmIdentifier DefaultHashAlgorithm = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance);

		// Token: 0x04002A7F RID: 10879
		public static readonly AlgorithmIdentifier DefaultMaskGenFunction = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdMgf1, RsassaPssParameters.DefaultHashAlgorithm);

		// Token: 0x04002A80 RID: 10880
		public static readonly DerInteger DefaultSaltLength = new DerInteger(20);

		// Token: 0x04002A81 RID: 10881
		public static readonly DerInteger DefaultTrailerField = new DerInteger(1);
	}
}
