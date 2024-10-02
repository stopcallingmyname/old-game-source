using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200069E RID: 1694
	public class GeneralSubtree : Asn1Encodable
	{
		// Token: 0x06003EA6 RID: 16038 RVA: 0x00177CC8 File Offset: 0x00175EC8
		private GeneralSubtree(Asn1Sequence seq)
		{
			this.baseName = GeneralName.GetInstance(seq[0]);
			switch (seq.Count)
			{
			case 1:
				return;
			case 2:
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[1]);
				int tagNo = instance.TagNo;
				if (tagNo == 0)
				{
					this.minimum = DerInteger.GetInstance(instance, false);
					return;
				}
				if (tagNo != 1)
				{
					throw new ArgumentException("Bad tag number: " + instance.TagNo);
				}
				this.maximum = DerInteger.GetInstance(instance, false);
				return;
			}
			case 3:
			{
				Asn1TaggedObject instance2 = Asn1TaggedObject.GetInstance(seq[1]);
				if (instance2.TagNo != 0)
				{
					throw new ArgumentException("Bad tag number for 'minimum': " + instance2.TagNo);
				}
				this.minimum = DerInteger.GetInstance(instance2, false);
				Asn1TaggedObject instance3 = Asn1TaggedObject.GetInstance(seq[2]);
				if (instance3.TagNo != 1)
				{
					throw new ArgumentException("Bad tag number for 'maximum': " + instance3.TagNo);
				}
				this.maximum = DerInteger.GetInstance(instance3, false);
				return;
			}
			default:
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x00177DF8 File Offset: 0x00175FF8
		public GeneralSubtree(GeneralName baseName, BigInteger minimum, BigInteger maximum)
		{
			this.baseName = baseName;
			if (minimum != null)
			{
				this.minimum = new DerInteger(minimum);
			}
			if (maximum != null)
			{
				this.maximum = new DerInteger(maximum);
			}
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x00177E25 File Offset: 0x00176025
		public GeneralSubtree(GeneralName baseName) : this(baseName, null, null)
		{
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00177E30 File Offset: 0x00176030
		public static GeneralSubtree GetInstance(Asn1TaggedObject o, bool isExplicit)
		{
			return new GeneralSubtree(Asn1Sequence.GetInstance(o, isExplicit));
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x00177E3E File Offset: 0x0017603E
		public static GeneralSubtree GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is GeneralSubtree)
			{
				return (GeneralSubtree)obj;
			}
			return new GeneralSubtree(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003EAB RID: 16043 RVA: 0x00177E5F File Offset: 0x0017605F
		public GeneralName Base
		{
			get
			{
				return this.baseName;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06003EAC RID: 16044 RVA: 0x00177E67 File Offset: 0x00176067
		public BigInteger Minimum
		{
			get
			{
				if (this.minimum != null)
				{
					return this.minimum.Value;
				}
				return BigInteger.Zero;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06003EAD RID: 16045 RVA: 0x00177E82 File Offset: 0x00176082
		public BigInteger Maximum
		{
			get
			{
				if (this.maximum != null)
				{
					return this.maximum.Value;
				}
				return null;
			}
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x00177E9C File Offset: 0x0017609C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.baseName
			});
			if (this.minimum != null && this.minimum.Value.SignValue != 0)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.minimum)
				});
			}
			if (this.maximum != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.maximum)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040027C4 RID: 10180
		private readonly GeneralName baseName;

		// Token: 0x040027C5 RID: 10181
		private readonly DerInteger minimum;

		// Token: 0x040027C6 RID: 10182
		private readonly DerInteger maximum;
	}
}
