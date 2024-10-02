using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000644 RID: 1604
	public class BerTaggedObject : DerTaggedObject
	{
		// Token: 0x06003BFE RID: 15358 RVA: 0x00170017 File Offset: 0x0016E217
		public BerTaggedObject(int tagNo, Asn1Encodable obj) : base(tagNo, obj)
		{
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x00170021 File Offset: 0x0016E221
		public BerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj) : base(explicitly, tagNo, obj)
		{
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x0017002C File Offset: 0x0016E22C
		public BerTaggedObject(int tagNo) : base(false, tagNo, BerSequence.Empty)
		{
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x0017003C File Offset: 0x0016E23C
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteTag(160, this.tagNo);
				derOut.WriteByte(128);
				if (!base.IsEmpty())
				{
					if (!this.explicitly)
					{
						IEnumerable enumerable;
						if (this.obj is Asn1OctetString)
						{
							if (this.obj is BerOctetString)
							{
								enumerable = (BerOctetString)this.obj;
							}
							else
							{
								enumerable = new BerOctetString(((Asn1OctetString)this.obj).GetOctets());
							}
						}
						else if (this.obj is Asn1Sequence)
						{
							enumerable = (Asn1Sequence)this.obj;
						}
						else
						{
							if (!(this.obj is Asn1Set))
							{
								throw Platform.CreateNotImplementedException(Platform.GetTypeName(this.obj));
							}
							enumerable = (Asn1Set)this.obj;
						}
						using (IEnumerator enumerator = enumerable.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Asn1Encodable obj2 = (Asn1Encodable)obj;
								derOut.WriteObject(obj2);
							}
							goto IL_10F;
						}
					}
					derOut.WriteObject(this.obj);
				}
				IL_10F:
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}
	}
}
