using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Smime
{
	// Token: 0x020006E4 RID: 1764
	public class SmimeCapabilityVector
	{
		// Token: 0x060040C9 RID: 16585 RVA: 0x001807C2 File Offset: 0x0017E9C2
		public void AddCapability(DerObjectIdentifier capability)
		{
			this.capabilities.Add(new Asn1Encodable[]
			{
				new DerSequence(capability)
			});
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x001807DE File Offset: 0x0017E9DE
		public void AddCapability(DerObjectIdentifier capability, int value)
		{
			this.capabilities.Add(new Asn1Encodable[]
			{
				new DerSequence(new Asn1Encodable[]
				{
					capability,
					new DerInteger(value)
				})
			});
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x0018080C File Offset: 0x0017EA0C
		public void AddCapability(DerObjectIdentifier capability, Asn1Encodable parameters)
		{
			this.capabilities.Add(new Asn1Encodable[]
			{
				new DerSequence(new Asn1Encodable[]
				{
					capability,
					parameters
				})
			});
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00180835 File Offset: 0x0017EA35
		public Asn1EncodableVector ToAsn1EncodableVector()
		{
			return this.capabilities;
		}

		// Token: 0x04002974 RID: 10612
		private readonly Asn1EncodableVector capabilities = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
	}
}
