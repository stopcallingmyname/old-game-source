using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A5 RID: 1701
	public class NameConstraints : Asn1Encodable
	{
		// Token: 0x06003EDC RID: 16092 RVA: 0x00178B0C File Offset: 0x00176D0C
		public static NameConstraints GetInstance(object obj)
		{
			if (obj == null || obj is NameConstraints)
			{
				return (NameConstraints)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new NameConstraints((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x00178B5C File Offset: 0x00176D5C
		public NameConstraints(Asn1Sequence seq)
		{
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				int tagNo = asn1TaggedObject.TagNo;
				if (tagNo != 0)
				{
					if (tagNo == 1)
					{
						this.excluded = Asn1Sequence.GetInstance(asn1TaggedObject, false);
					}
				}
				else
				{
					this.permitted = Asn1Sequence.GetInstance(asn1TaggedObject, false);
				}
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x00178BDC File Offset: 0x00176DDC
		public NameConstraints(ArrayList permitted, ArrayList excluded) : this(permitted, excluded)
		{
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00178BE6 File Offset: 0x00176DE6
		public NameConstraints(IList permitted, IList excluded)
		{
			if (permitted != null)
			{
				this.permitted = this.CreateSequence(permitted);
			}
			if (excluded != null)
			{
				this.excluded = this.CreateSequence(excluded);
			}
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x00178C10 File Offset: 0x00176E10
		private DerSequence CreateSequence(IList subtrees)
		{
			GeneralSubtree[] array = new GeneralSubtree[subtrees.Count];
			for (int i = 0; i < subtrees.Count; i++)
			{
				array[i] = (GeneralSubtree)subtrees[i];
			}
			Asn1Encodable[] v = array;
			return new DerSequence(v);
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06003EE1 RID: 16097 RVA: 0x00178C51 File Offset: 0x00176E51
		public Asn1Sequence PermittedSubtrees
		{
			get
			{
				return this.permitted;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06003EE2 RID: 16098 RVA: 0x00178C59 File Offset: 0x00176E59
		public Asn1Sequence ExcludedSubtrees
		{
			get
			{
				return this.excluded;
			}
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00178C64 File Offset: 0x00176E64
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.permitted != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.permitted)
				});
			}
			if (this.excluded != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.excluded)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040027F1 RID: 10225
		private Asn1Sequence permitted;

		// Token: 0x040027F2 RID: 10226
		private Asn1Sequence excluded;
	}
}
