using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006FF RID: 1791
	public class RsaesOaepParameters : Asn1Encodable
	{
		// Token: 0x06004178 RID: 16760 RVA: 0x001831A0 File Offset: 0x001813A0
		public static RsaesOaepParameters GetInstance(object obj)
		{
			if (obj is RsaesOaepParameters)
			{
				return (RsaesOaepParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RsaesOaepParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x001831DF File Offset: 0x001813DF
		public RsaesOaepParameters()
		{
			this.hashAlgorithm = RsaesOaepParameters.DefaultHashAlgorithm;
			this.maskGenAlgorithm = RsaesOaepParameters.DefaultMaskGenFunction;
			this.pSourceAlgorithm = RsaesOaepParameters.DefaultPSourceAlgorithm;
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x00183208 File Offset: 0x00181408
		public RsaesOaepParameters(AlgorithmIdentifier hashAlgorithm, AlgorithmIdentifier maskGenAlgorithm, AlgorithmIdentifier pSourceAlgorithm)
		{
			this.hashAlgorithm = hashAlgorithm;
			this.maskGenAlgorithm = maskGenAlgorithm;
			this.pSourceAlgorithm = pSourceAlgorithm;
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x00183228 File Offset: 0x00181428
		public RsaesOaepParameters(Asn1Sequence seq)
		{
			this.hashAlgorithm = RsaesOaepParameters.DefaultHashAlgorithm;
			this.maskGenAlgorithm = RsaesOaepParameters.DefaultMaskGenFunction;
			this.pSourceAlgorithm = RsaesOaepParameters.DefaultPSourceAlgorithm;
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
					this.pSourceAlgorithm = AlgorithmIdentifier.GetInstance(asn1TaggedObject, true);
					break;
				default:
					throw new ArgumentException("unknown tag");
				}
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600417C RID: 16764 RVA: 0x001832CD File Offset: 0x001814CD
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x001832D5 File Offset: 0x001814D5
		public AlgorithmIdentifier MaskGenAlgorithm
		{
			get
			{
				return this.maskGenAlgorithm;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600417E RID: 16766 RVA: 0x001832DD File Offset: 0x001814DD
		public AlgorithmIdentifier PSourceAlgorithm
		{
			get
			{
				return this.pSourceAlgorithm;
			}
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x001832E8 File Offset: 0x001814E8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (!this.hashAlgorithm.Equals(RsaesOaepParameters.DefaultHashAlgorithm))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.hashAlgorithm)
				});
			}
			if (!this.maskGenAlgorithm.Equals(RsaesOaepParameters.DefaultMaskGenFunction))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.maskGenAlgorithm)
				});
			}
			if (!this.pSourceAlgorithm.Equals(RsaesOaepParameters.DefaultPSourceAlgorithm))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.pSourceAlgorithm)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002A6C RID: 10860
		private AlgorithmIdentifier hashAlgorithm;

		// Token: 0x04002A6D RID: 10861
		private AlgorithmIdentifier maskGenAlgorithm;

		// Token: 0x04002A6E RID: 10862
		private AlgorithmIdentifier pSourceAlgorithm;

		// Token: 0x04002A6F RID: 10863
		public static readonly AlgorithmIdentifier DefaultHashAlgorithm = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance);

		// Token: 0x04002A70 RID: 10864
		public static readonly AlgorithmIdentifier DefaultMaskGenFunction = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdMgf1, RsaesOaepParameters.DefaultHashAlgorithm);

		// Token: 0x04002A71 RID: 10865
		public static readonly AlgorithmIdentifier DefaultPSourceAlgorithm = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdPSpecified, new DerOctetString(new byte[0]));
	}
}
