using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000749 RID: 1865
	public class CrlOcspRef : Asn1Encodable
	{
		// Token: 0x06004356 RID: 17238 RVA: 0x00189E70 File Offset: 0x00188070
		public static CrlOcspRef GetInstance(object obj)
		{
			if (obj == null || obj is CrlOcspRef)
			{
				return (CrlOcspRef)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CrlOcspRef((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CrlOcspRef' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x00189EC0 File Offset: 0x001880C0
		private CrlOcspRef(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				Asn1Object @object = asn1TaggedObject.GetObject();
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.crlids = CrlListID.GetInstance(@object);
					break;
				case 1:
					this.ocspids = OcspListID.GetInstance(@object);
					break;
				case 2:
					this.otherRev = OtherRevRefs.GetInstance(@object);
					break;
				default:
					throw new ArgumentException("Illegal tag in CrlOcspRef", "seq");
				}
			}
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x00189F7C File Offset: 0x0018817C
		public CrlOcspRef(CrlListID crlids, OcspListID ocspids, OtherRevRefs otherRev)
		{
			this.crlids = crlids;
			this.ocspids = ocspids;
			this.otherRev = otherRev;
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06004359 RID: 17241 RVA: 0x00189F99 File Offset: 0x00188199
		public CrlListID CrlIDs
		{
			get
			{
				return this.crlids;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600435A RID: 17242 RVA: 0x00189FA1 File Offset: 0x001881A1
		public OcspListID OcspIDs
		{
			get
			{
				return this.ocspids;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x0600435B RID: 17243 RVA: 0x00189FA9 File Offset: 0x001881A9
		public OtherRevRefs OtherRev
		{
			get
			{
				return this.otherRev;
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x00189FB4 File Offset: 0x001881B4
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.crlids != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.crlids.ToAsn1Object())
				});
			}
			if (this.ocspids != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.ocspids.ToAsn1Object())
				});
			}
			if (this.otherRev != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.otherRev.ToAsn1Object())
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C2C RID: 11308
		private readonly CrlListID crlids;

		// Token: 0x04002C2D RID: 11309
		private readonly OcspListID ocspids;

		// Token: 0x04002C2E RID: 11310
		private readonly OtherRevRefs otherRev;
	}
}
