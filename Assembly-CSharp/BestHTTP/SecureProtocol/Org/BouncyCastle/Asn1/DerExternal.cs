using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200064D RID: 1613
	public class DerExternal : Asn1Object
	{
		// Token: 0x06003C4F RID: 15439 RVA: 0x001711A4 File Offset: 0x0016F3A4
		public DerExternal(Asn1EncodableVector vector)
		{
			int num = 0;
			Asn1Object objFromVector = DerExternal.GetObjFromVector(vector, num);
			if (objFromVector is DerObjectIdentifier)
			{
				this.directReference = (DerObjectIdentifier)objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (objFromVector is DerInteger)
			{
				this.indirectReference = (DerInteger)objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (!(objFromVector is Asn1TaggedObject))
			{
				this.dataValueDescriptor = objFromVector;
				num++;
				objFromVector = DerExternal.GetObjFromVector(vector, num);
			}
			if (vector.Count != num + 1)
			{
				throw new ArgumentException("input vector too large", "vector");
			}
			if (!(objFromVector is Asn1TaggedObject))
			{
				throw new ArgumentException("No tagged object found in vector. Structure doesn't seem to be of type External", "vector");
			}
			Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)objFromVector;
			this.Encoding = asn1TaggedObject.TagNo;
			if (this.encoding < 0 || this.encoding > 2)
			{
				throw new InvalidOperationException("invalid encoding value");
			}
			this.externalContent = asn1TaggedObject.GetObject();
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0017128B File Offset: 0x0016F48B
		public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, DerTaggedObject externalData) : this(directReference, indirectReference, dataValueDescriptor, externalData.TagNo, externalData.ToAsn1Object())
		{
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x001712A4 File Offset: 0x0016F4A4
		public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, int encoding, Asn1Object externalData)
		{
			this.DirectReference = directReference;
			this.IndirectReference = indirectReference;
			this.DataValueDescriptor = dataValueDescriptor;
			this.Encoding = encoding;
			this.ExternalContent = externalData.ToAsn1Object();
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x001712D8 File Offset: 0x0016F4D8
		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerExternal.WriteEncodable(memoryStream, this.directReference);
			DerExternal.WriteEncodable(memoryStream, this.indirectReference);
			DerExternal.WriteEncodable(memoryStream, this.dataValueDescriptor);
			DerExternal.WriteEncodable(memoryStream, new DerTaggedObject(8, this.externalContent));
			derOut.WriteEncoded(32, 8, memoryStream.ToArray());
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x00171330 File Offset: 0x0016F530
		protected override int Asn1GetHashCode()
		{
			int num = this.externalContent.GetHashCode();
			if (this.directReference != null)
			{
				num ^= this.directReference.GetHashCode();
			}
			if (this.indirectReference != null)
			{
				num ^= this.indirectReference.GetHashCode();
			}
			if (this.dataValueDescriptor != null)
			{
				num ^= this.dataValueDescriptor.GetHashCode();
			}
			return num;
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x0017138C File Offset: 0x0016F58C
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (this == asn1Object)
			{
				return true;
			}
			DerExternal derExternal = asn1Object as DerExternal;
			return derExternal != null && (object.Equals(this.directReference, derExternal.directReference) && object.Equals(this.indirectReference, derExternal.indirectReference) && object.Equals(this.dataValueDescriptor, derExternal.dataValueDescriptor)) && this.externalContent.Equals(derExternal.externalContent);
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06003C55 RID: 15445 RVA: 0x001713F7 File Offset: 0x0016F5F7
		// (set) Token: 0x06003C56 RID: 15446 RVA: 0x001713FF File Offset: 0x0016F5FF
		public Asn1Object DataValueDescriptor
		{
			get
			{
				return this.dataValueDescriptor;
			}
			set
			{
				this.dataValueDescriptor = value;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06003C57 RID: 15447 RVA: 0x00171408 File Offset: 0x0016F608
		// (set) Token: 0x06003C58 RID: 15448 RVA: 0x00171410 File Offset: 0x0016F610
		public DerObjectIdentifier DirectReference
		{
			get
			{
				return this.directReference;
			}
			set
			{
				this.directReference = value;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06003C59 RID: 15449 RVA: 0x00171419 File Offset: 0x0016F619
		// (set) Token: 0x06003C5A RID: 15450 RVA: 0x00171421 File Offset: 0x0016F621
		public int Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				if (this.encoding < 0 || this.encoding > 2)
				{
					throw new InvalidOperationException("invalid encoding value: " + this.encoding);
				}
				this.encoding = value;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06003C5B RID: 15451 RVA: 0x00171457 File Offset: 0x0016F657
		// (set) Token: 0x06003C5C RID: 15452 RVA: 0x0017145F File Offset: 0x0016F65F
		public Asn1Object ExternalContent
		{
			get
			{
				return this.externalContent;
			}
			set
			{
				this.externalContent = value;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x00171468 File Offset: 0x0016F668
		// (set) Token: 0x06003C5E RID: 15454 RVA: 0x00171470 File Offset: 0x0016F670
		public DerInteger IndirectReference
		{
			get
			{
				return this.indirectReference;
			}
			set
			{
				this.indirectReference = value;
			}
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x00171479 File Offset: 0x0016F679
		private static Asn1Object GetObjFromVector(Asn1EncodableVector v, int index)
		{
			if (v.Count <= index)
			{
				throw new ArgumentException("too few objects in input vector", "v");
			}
			return v[index].ToAsn1Object();
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x001714A0 File Offset: 0x0016F6A0
		private static void WriteEncodable(MemoryStream ms, Asn1Encodable e)
		{
			if (e != null)
			{
				byte[] derEncoded = e.GetDerEncoded();
				ms.Write(derEncoded, 0, derEncoded.Length);
			}
		}

		// Token: 0x040026E2 RID: 9954
		private DerObjectIdentifier directReference;

		// Token: 0x040026E3 RID: 9955
		private DerInteger indirectReference;

		// Token: 0x040026E4 RID: 9956
		private Asn1Object dataValueDescriptor;

		// Token: 0x040026E5 RID: 9957
		private int encoding;

		// Token: 0x040026E6 RID: 9958
		private Asn1Object externalContent;
	}
}
