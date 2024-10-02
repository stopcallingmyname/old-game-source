using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A8 RID: 1704
	public class PolicyInformation : Asn1Encodable
	{
		// Token: 0x06003EF6 RID: 16118 RVA: 0x0017904C File Offset: 0x0017724C
		private PolicyInformation(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.policyIdentifier = DerObjectIdentifier.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.policyQualifiers = Asn1Sequence.GetInstance(seq[1]);
			}
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x001790B9 File Offset: 0x001772B9
		public PolicyInformation(DerObjectIdentifier policyIdentifier)
		{
			this.policyIdentifier = policyIdentifier;
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x001790C8 File Offset: 0x001772C8
		public PolicyInformation(DerObjectIdentifier policyIdentifier, Asn1Sequence policyQualifiers)
		{
			this.policyIdentifier = policyIdentifier;
			this.policyQualifiers = policyQualifiers;
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x001790DE File Offset: 0x001772DE
		public static PolicyInformation GetInstance(object obj)
		{
			if (obj == null || obj is PolicyInformation)
			{
				return (PolicyInformation)obj;
			}
			return new PolicyInformation(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06003EFA RID: 16122 RVA: 0x001790FD File Offset: 0x001772FD
		public DerObjectIdentifier PolicyIdentifier
		{
			get
			{
				return this.policyIdentifier;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06003EFB RID: 16123 RVA: 0x00179105 File Offset: 0x00177305
		public Asn1Sequence PolicyQualifiers
		{
			get
			{
				return this.policyQualifiers;
			}
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x00179110 File Offset: 0x00177310
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.policyIdentifier
			});
			if (this.policyQualifiers != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.policyQualifiers
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040027FC RID: 10236
		private readonly DerObjectIdentifier policyIdentifier;

		// Token: 0x040027FD RID: 10237
		private readonly Asn1Sequence policyQualifiers;
	}
}
