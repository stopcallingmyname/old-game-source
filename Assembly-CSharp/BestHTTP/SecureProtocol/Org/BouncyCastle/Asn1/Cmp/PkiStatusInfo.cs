using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C6 RID: 1990
	public class PkiStatusInfo : Asn1Encodable
	{
		// Token: 0x060046E0 RID: 18144 RVA: 0x001946B2 File Offset: 0x001928B2
		public static PkiStatusInfo GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return PkiStatusInfo.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x001946C0 File Offset: 0x001928C0
		public static PkiStatusInfo GetInstance(object obj)
		{
			if (obj is PkiStatusInfo)
			{
				return (PkiStatusInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PkiStatusInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x00194700 File Offset: 0x00192900
		public PkiStatusInfo(Asn1Sequence seq)
		{
			this.status = DerInteger.GetInstance(seq[0]);
			this.statusString = null;
			this.failInfo = null;
			if (seq.Count > 2)
			{
				this.statusString = PkiFreeText.GetInstance(seq[1]);
				this.failInfo = DerBitString.GetInstance(seq[2]);
				return;
			}
			if (seq.Count > 1)
			{
				object obj = seq[1];
				if (obj is DerBitString)
				{
					this.failInfo = DerBitString.GetInstance(obj);
					return;
				}
				this.statusString = PkiFreeText.GetInstance(obj);
			}
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x00194793 File Offset: 0x00192993
		public PkiStatusInfo(int status)
		{
			this.status = new DerInteger(status);
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x001947A7 File Offset: 0x001929A7
		public PkiStatusInfo(int status, PkiFreeText statusString)
		{
			this.status = new DerInteger(status);
			this.statusString = statusString;
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x001947C2 File Offset: 0x001929C2
		public PkiStatusInfo(int status, PkiFreeText statusString, PkiFailureInfo failInfo)
		{
			this.status = new DerInteger(status);
			this.statusString = statusString;
			this.failInfo = failInfo;
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060046E6 RID: 18150 RVA: 0x001947E4 File Offset: 0x001929E4
		public BigInteger Status
		{
			get
			{
				return this.status.Value;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060046E7 RID: 18151 RVA: 0x001947F1 File Offset: 0x001929F1
		public PkiFreeText StatusString
		{
			get
			{
				return this.statusString;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060046E8 RID: 18152 RVA: 0x001947F9 File Offset: 0x001929F9
		public DerBitString FailInfo
		{
			get
			{
				return this.failInfo;
			}
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x00194804 File Offset: 0x00192A04
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.status
			});
			if (this.statusString != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.statusString
				});
			}
			if (this.failInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.failInfo
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002E4D RID: 11853
		private DerInteger status;

		// Token: 0x04002E4E RID: 11854
		private PkiFreeText statusString;

		// Token: 0x04002E4F RID: 11855
		private DerBitString failInfo;
	}
}
