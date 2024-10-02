using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000693 RID: 1683
	public class CrlDistPoint : Asn1Encodable
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x0017690E File Offset: 0x00174B0E
		public static CrlDistPoint GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return CrlDistPoint.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0017691C File Offset: 0x00174B1C
		public static CrlDistPoint GetInstance(object obj)
		{
			if (obj is CrlDistPoint || obj == null)
			{
				return (CrlDistPoint)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CrlDistPoint((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x00176969 File Offset: 0x00174B69
		private CrlDistPoint(Asn1Sequence seq)
		{
			this.seq = seq;
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x00176978 File Offset: 0x00174B78
		public CrlDistPoint(DistributionPoint[] points)
		{
			this.seq = new DerSequence(points);
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x0017699C File Offset: 0x00174B9C
		public DistributionPoint[] GetDistributionPoints()
		{
			DistributionPoint[] array = new DistributionPoint[this.seq.Count];
			for (int num = 0; num != this.seq.Count; num++)
			{
				array[num] = DistributionPoint.GetInstance(this.seq[num]);
			}
			return array;
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x001769E5 File Offset: 0x00174BE5
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x001769F0 File Offset: 0x00174BF0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("CRLDistPoint:");
			stringBuilder.Append(newLine);
			DistributionPoint[] distributionPoints = this.GetDistributionPoints();
			for (int num = 0; num != distributionPoints.Length; num++)
			{
				stringBuilder.Append("    ");
				stringBuilder.Append(distributionPoints[num]);
				stringBuilder.Append(newLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002797 RID: 10135
		internal readonly Asn1Sequence seq;
	}
}
