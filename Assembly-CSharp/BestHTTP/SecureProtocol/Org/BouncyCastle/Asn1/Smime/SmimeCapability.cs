using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Smime
{
	// Token: 0x020006E3 RID: 1763
	public class SmimeCapability : Asn1Encodable
	{
		// Token: 0x060040C2 RID: 16578 RVA: 0x00180683 File Offset: 0x0017E883
		public SmimeCapability(Asn1Sequence seq)
		{
			this.capabilityID = (DerObjectIdentifier)seq[0].ToAsn1Object();
			if (seq.Count > 1)
			{
				this.parameters = seq[1].ToAsn1Object();
			}
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x001806BD File Offset: 0x0017E8BD
		public SmimeCapability(DerObjectIdentifier capabilityID, Asn1Encodable parameters)
		{
			if (capabilityID == null)
			{
				throw new ArgumentNullException("capabilityID");
			}
			this.capabilityID = capabilityID;
			if (parameters != null)
			{
				this.parameters = parameters.ToAsn1Object();
			}
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x001806E9 File Offset: 0x0017E8E9
		public static SmimeCapability GetInstance(object obj)
		{
			if (obj == null || obj is SmimeCapability)
			{
				return (SmimeCapability)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SmimeCapability((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid SmimeCapability");
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x0018071B File Offset: 0x0017E91B
		public DerObjectIdentifier CapabilityID
		{
			get
			{
				return this.capabilityID;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x00180723 File Offset: 0x0017E923
		public Asn1Object Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x0018072C File Offset: 0x0017E92C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.capabilityID
			});
			if (this.parameters != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.parameters
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x0400296C RID: 10604
		public static readonly DerObjectIdentifier PreferSignedData = PkcsObjectIdentifiers.PreferSignedData;

		// Token: 0x0400296D RID: 10605
		public static readonly DerObjectIdentifier CannotDecryptAny = PkcsObjectIdentifiers.CannotDecryptAny;

		// Token: 0x0400296E RID: 10606
		public static readonly DerObjectIdentifier SmimeCapabilitiesVersions = PkcsObjectIdentifiers.SmimeCapabilitiesVersions;

		// Token: 0x0400296F RID: 10607
		public static readonly DerObjectIdentifier DesCbc = new DerObjectIdentifier("1.3.14.3.2.7");

		// Token: 0x04002970 RID: 10608
		public static readonly DerObjectIdentifier DesEde3Cbc = PkcsObjectIdentifiers.DesEde3Cbc;

		// Token: 0x04002971 RID: 10609
		public static readonly DerObjectIdentifier RC2Cbc = PkcsObjectIdentifiers.RC2Cbc;

		// Token: 0x04002972 RID: 10610
		private DerObjectIdentifier capabilityID;

		// Token: 0x04002973 RID: 10611
		private Asn1Object parameters;
	}
}
