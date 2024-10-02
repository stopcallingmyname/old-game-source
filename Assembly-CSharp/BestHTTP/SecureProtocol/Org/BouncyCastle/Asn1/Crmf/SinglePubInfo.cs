using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000778 RID: 1912
	public class SinglePubInfo : Asn1Encodable
	{
		// Token: 0x06004499 RID: 17561 RVA: 0x0018E1D6 File Offset: 0x0018C3D6
		private SinglePubInfo(Asn1Sequence seq)
		{
			this.pubMethod = DerInteger.GetInstance(seq[0]);
			if (seq.Count == 2)
			{
				this.pubLocation = GeneralName.GetInstance(seq[1]);
			}
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0018E20B File Offset: 0x0018C40B
		public static SinglePubInfo GetInstance(object obj)
		{
			if (obj is SinglePubInfo)
			{
				return (SinglePubInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SinglePubInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x0600449B RID: 17563 RVA: 0x0018E24A File Offset: 0x0018C44A
		public virtual GeneralName PubLocation
		{
			get
			{
				return this.pubLocation;
			}
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x0018E254 File Offset: 0x0018C454
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.pubMethod
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.pubLocation
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CF7 RID: 11511
		private readonly DerInteger pubMethod;

		// Token: 0x04002CF8 RID: 11512
		private readonly GeneralName pubLocation;
	}
}
