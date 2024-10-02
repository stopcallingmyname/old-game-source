using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x0200074A RID: 1866
	public class CrlValidatedID : Asn1Encodable
	{
		// Token: 0x0600435D RID: 17245 RVA: 0x0018A050 File Offset: 0x00188250
		public static CrlValidatedID GetInstance(object obj)
		{
			if (obj == null || obj is CrlValidatedID)
			{
				return (CrlValidatedID)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CrlValidatedID((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CrlValidatedID' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0018A0A0 File Offset: 0x001882A0
		private CrlValidatedID(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.crlHash = OtherHash.GetInstance(seq[0].ToAsn1Object());
			if (seq.Count > 1)
			{
				this.crlIdentifier = CrlIdentifier.GetInstance(seq[1].ToAsn1Object());
			}
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x0018A12A File Offset: 0x0018832A
		public CrlValidatedID(OtherHash crlHash) : this(crlHash, null)
		{
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x0018A134 File Offset: 0x00188334
		public CrlValidatedID(OtherHash crlHash, CrlIdentifier crlIdentifier)
		{
			if (crlHash == null)
			{
				throw new ArgumentNullException("crlHash");
			}
			this.crlHash = crlHash;
			this.crlIdentifier = crlIdentifier;
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06004361 RID: 17249 RVA: 0x0018A158 File Offset: 0x00188358
		public OtherHash CrlHash
		{
			get
			{
				return this.crlHash;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06004362 RID: 17250 RVA: 0x0018A160 File Offset: 0x00188360
		public CrlIdentifier CrlIdentifier
		{
			get
			{
				return this.crlIdentifier;
			}
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0018A168 File Offset: 0x00188368
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.crlHash.ToAsn1Object()
			});
			if (this.crlIdentifier != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.crlIdentifier.ToAsn1Object()
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C2F RID: 11311
		private readonly OtherHash crlHash;

		// Token: 0x04002C30 RID: 11312
		private readonly CrlIdentifier crlIdentifier;
	}
}
