using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006FA RID: 1786
	public class Pfx : Asn1Encodable
	{
		// Token: 0x06004156 RID: 16726 RVA: 0x001823F0 File Offset: 0x001805F0
		public Pfx(Asn1Sequence seq)
		{
			if (((DerInteger)seq[0]).Value.IntValue != 3)
			{
				throw new ArgumentException("wrong version for PFX PDU");
			}
			this.contentInfo = ContentInfo.GetInstance(seq[1]);
			if (seq.Count == 3)
			{
				this.macData = MacData.GetInstance(seq[2]);
			}
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x00182454 File Offset: 0x00180654
		public Pfx(ContentInfo contentInfo, MacData macData)
		{
			this.contentInfo = contentInfo;
			this.macData = macData;
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x0018246A File Offset: 0x0018066A
		public ContentInfo AuthSafe
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06004159 RID: 16729 RVA: 0x00182472 File Offset: 0x00180672
		public MacData MacData
		{
			get
			{
				return this.macData;
			}
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x0018247C File Offset: 0x0018067C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger(3),
				this.contentInfo
			});
			if (this.macData != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.macData
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x040029D1 RID: 10705
		private ContentInfo contentInfo;

		// Token: 0x040029D2 RID: 10706
		private MacData macData;
	}
}
