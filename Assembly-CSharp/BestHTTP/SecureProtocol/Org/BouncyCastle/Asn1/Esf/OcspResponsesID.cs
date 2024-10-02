using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x0200074E RID: 1870
	public class OcspResponsesID : Asn1Encodable
	{
		// Token: 0x06004372 RID: 17266 RVA: 0x0018A558 File Offset: 0x00188758
		public static OcspResponsesID GetInstance(object obj)
		{
			if (obj == null || obj is OcspResponsesID)
			{
				return (OcspResponsesID)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OcspResponsesID((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OcspResponsesID' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x0018A5A8 File Offset: 0x001887A8
		private OcspResponsesID(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.ocspIdentifier = OcspIdentifier.GetInstance(seq[0].ToAsn1Object());
			if (seq.Count > 1)
			{
				this.ocspRepHash = OtherHash.GetInstance(seq[1].ToAsn1Object());
			}
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x0018A632 File Offset: 0x00188832
		public OcspResponsesID(OcspIdentifier ocspIdentifier) : this(ocspIdentifier, null)
		{
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0018A63C File Offset: 0x0018883C
		public OcspResponsesID(OcspIdentifier ocspIdentifier, OtherHash ocspRepHash)
		{
			if (ocspIdentifier == null)
			{
				throw new ArgumentNullException("ocspIdentifier");
			}
			this.ocspIdentifier = ocspIdentifier;
			this.ocspRepHash = ocspRepHash;
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06004376 RID: 17270 RVA: 0x0018A660 File Offset: 0x00188860
		public OcspIdentifier OcspIdentifier
		{
			get
			{
				return this.ocspIdentifier;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06004377 RID: 17271 RVA: 0x0018A668 File Offset: 0x00188868
		public OtherHash OcspRepHash
		{
			get
			{
				return this.ocspRepHash;
			}
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0018A670 File Offset: 0x00188870
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.ocspIdentifier.ToAsn1Object()
			});
			if (this.ocspRepHash != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.ocspRepHash.ToAsn1Object()
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C42 RID: 11330
		private readonly OcspIdentifier ocspIdentifier;

		// Token: 0x04002C43 RID: 11331
		private readonly OtherHash ocspRepHash;
	}
}
