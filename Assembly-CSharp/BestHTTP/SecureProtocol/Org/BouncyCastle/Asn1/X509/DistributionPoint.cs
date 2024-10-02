using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000698 RID: 1688
	public class DistributionPoint : Asn1Encodable
	{
		// Token: 0x06003E67 RID: 15975 RVA: 0x00176D63 File Offset: 0x00174F63
		public static DistributionPoint GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DistributionPoint.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00176D71 File Offset: 0x00174F71
		public static DistributionPoint GetInstance(object obj)
		{
			if (obj == null || obj is DistributionPoint)
			{
				return (DistributionPoint)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DistributionPoint((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DistributionPoint: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00176DB0 File Offset: 0x00174FB0
		private DistributionPoint(Asn1Sequence seq)
		{
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[num]);
				switch (instance.TagNo)
				{
				case 0:
					this.distributionPoint = DistributionPointName.GetInstance(instance, true);
					break;
				case 1:
					this.reasons = new ReasonFlags(DerBitString.GetInstance(instance, false));
					break;
				case 2:
					this.cRLIssuer = GeneralNames.GetInstance(instance, false);
					break;
				}
			}
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00176E2C File Offset: 0x0017502C
		public DistributionPoint(DistributionPointName distributionPointName, ReasonFlags reasons, GeneralNames crlIssuer)
		{
			this.distributionPoint = distributionPointName;
			this.reasons = reasons;
			this.cRLIssuer = crlIssuer;
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06003E6B RID: 15979 RVA: 0x00176E49 File Offset: 0x00175049
		public DistributionPointName DistributionPointName
		{
			get
			{
				return this.distributionPoint;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003E6C RID: 15980 RVA: 0x00176E51 File Offset: 0x00175051
		public ReasonFlags Reasons
		{
			get
			{
				return this.reasons;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x00176E59 File Offset: 0x00175059
		public GeneralNames CrlIssuer
		{
			get
			{
				return this.cRLIssuer;
			}
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x00176E64 File Offset: 0x00175064
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.distributionPoint != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(0, this.distributionPoint)
				});
			}
			if (this.reasons != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.reasons)
				});
			}
			if (this.cRLIssuer != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, this.cRLIssuer)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00176EF0 File Offset: 0x001750F0
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DistributionPoint: [");
			stringBuilder.Append(newLine);
			if (this.distributionPoint != null)
			{
				this.appendObject(stringBuilder, newLine, "distributionPoint", this.distributionPoint.ToString());
			}
			if (this.reasons != null)
			{
				this.appendObject(stringBuilder, newLine, "reasons", this.reasons.ToString());
			}
			if (this.cRLIssuer != null)
			{
				this.appendObject(stringBuilder, newLine, "cRLIssuer", this.cRLIssuer.ToString());
			}
			stringBuilder.Append("]");
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00176F98 File Offset: 0x00175198
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

		// Token: 0x040027AC RID: 10156
		internal readonly DistributionPointName distributionPoint;

		// Token: 0x040027AD RID: 10157
		internal readonly ReasonFlags reasons;

		// Token: 0x040027AE RID: 10158
		internal readonly GeneralNames cRLIssuer;
	}
}
