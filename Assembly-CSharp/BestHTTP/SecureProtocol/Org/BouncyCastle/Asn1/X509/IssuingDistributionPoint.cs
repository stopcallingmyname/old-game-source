using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A2 RID: 1698
	public class IssuingDistributionPoint : Asn1Encodable
	{
		// Token: 0x06003EC9 RID: 16073 RVA: 0x0017859A File Offset: 0x0017679A
		public static IssuingDistributionPoint GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return IssuingDistributionPoint.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x001785A8 File Offset: 0x001767A8
		public static IssuingDistributionPoint GetInstance(object obj)
		{
			if (obj == null || obj is IssuingDistributionPoint)
			{
				return (IssuingDistributionPoint)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new IssuingDistributionPoint((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x001785F8 File Offset: 0x001767F8
		public IssuingDistributionPoint(DistributionPointName distributionPoint, bool onlyContainsUserCerts, bool onlyContainsCACerts, ReasonFlags onlySomeReasons, bool indirectCRL, bool onlyContainsAttributeCerts)
		{
			this._distributionPoint = distributionPoint;
			this._indirectCRL = indirectCRL;
			this._onlyContainsAttributeCerts = onlyContainsAttributeCerts;
			this._onlyContainsCACerts = onlyContainsCACerts;
			this._onlyContainsUserCerts = onlyContainsUserCerts;
			this._onlySomeReasons = onlySomeReasons;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (distributionPoint != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, distributionPoint)
				});
			}
			if (onlyContainsUserCerts)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, DerBoolean.True)
				});
			}
			if (onlyContainsCACerts)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, DerBoolean.True)
				});
			}
			if (onlySomeReasons != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 3, onlySomeReasons)
				});
			}
			if (indirectCRL)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 4, DerBoolean.True)
				});
			}
			if (onlyContainsAttributeCerts)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 5, DerBoolean.True)
				});
			}
			this.seq = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x00178700 File Offset: 0x00176900
		private IssuingDistributionPoint(Asn1Sequence seq)
		{
			this.seq = seq;
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[num]);
				switch (instance.TagNo)
				{
				case 0:
					this._distributionPoint = DistributionPointName.GetInstance(instance, true);
					break;
				case 1:
					this._onlyContainsUserCerts = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				case 2:
					this._onlyContainsCACerts = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				case 3:
					this._onlySomeReasons = new ReasonFlags(DerBitString.GetInstance(instance, false));
					break;
				case 4:
					this._indirectCRL = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				case 5:
					this._onlyContainsAttributeCerts = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				default:
					throw new ArgumentException("unknown tag in IssuingDistributionPoint");
				}
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003ECD RID: 16077 RVA: 0x001787E3 File Offset: 0x001769E3
		public bool OnlyContainsUserCerts
		{
			get
			{
				return this._onlyContainsUserCerts;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003ECE RID: 16078 RVA: 0x001787EB File Offset: 0x001769EB
		public bool OnlyContainsCACerts
		{
			get
			{
				return this._onlyContainsCACerts;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06003ECF RID: 16079 RVA: 0x001787F3 File Offset: 0x001769F3
		public bool IsIndirectCrl
		{
			get
			{
				return this._indirectCRL;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x001787FB File Offset: 0x001769FB
		public bool OnlyContainsAttributeCerts
		{
			get
			{
				return this._onlyContainsAttributeCerts;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x00178803 File Offset: 0x00176A03
		public DistributionPointName DistributionPoint
		{
			get
			{
				return this._distributionPoint;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x0017880B File Offset: 0x00176A0B
		public ReasonFlags OnlySomeReasons
		{
			get
			{
				return this._onlySomeReasons;
			}
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x00178813 File Offset: 0x00176A13
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0017881C File Offset: 0x00176A1C
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("IssuingDistributionPoint: [");
			stringBuilder.Append(newLine);
			if (this._distributionPoint != null)
			{
				this.appendObject(stringBuilder, newLine, "distributionPoint", this._distributionPoint.ToString());
			}
			if (this._onlyContainsUserCerts)
			{
				this.appendObject(stringBuilder, newLine, "onlyContainsUserCerts", this._onlyContainsUserCerts.ToString());
			}
			if (this._onlyContainsCACerts)
			{
				this.appendObject(stringBuilder, newLine, "onlyContainsCACerts", this._onlyContainsCACerts.ToString());
			}
			if (this._onlySomeReasons != null)
			{
				this.appendObject(stringBuilder, newLine, "onlySomeReasons", this._onlySomeReasons.ToString());
			}
			if (this._onlyContainsAttributeCerts)
			{
				this.appendObject(stringBuilder, newLine, "onlyContainsAttributeCerts", this._onlyContainsAttributeCerts.ToString());
			}
			if (this._indirectCRL)
			{
				this.appendObject(stringBuilder, newLine, "indirectCRL", this._indirectCRL.ToString());
			}
			stringBuilder.Append("]");
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x00178930 File Offset: 0x00176B30
		private void appendObject(StringBuilder buf, string sep, string name, string val)
		{
			string value = "    ";
			buf.Append(value);
			buf.Append(name);
			buf.Append(":");
			buf.Append(sep);
			buf.Append(value);
			buf.Append(value);
			buf.Append(val);
			buf.Append(sep);
		}

		// Token: 0x040027D4 RID: 10196
		private readonly DistributionPointName _distributionPoint;

		// Token: 0x040027D5 RID: 10197
		private readonly bool _onlyContainsUserCerts;

		// Token: 0x040027D6 RID: 10198
		private readonly bool _onlyContainsCACerts;

		// Token: 0x040027D7 RID: 10199
		private readonly ReasonFlags _onlySomeReasons;

		// Token: 0x040027D8 RID: 10200
		private readonly bool _indirectCRL;

		// Token: 0x040027D9 RID: 10201
		private readonly bool _onlyContainsAttributeCerts;

		// Token: 0x040027DA RID: 10202
		private readonly Asn1Sequence seq;
	}
}
