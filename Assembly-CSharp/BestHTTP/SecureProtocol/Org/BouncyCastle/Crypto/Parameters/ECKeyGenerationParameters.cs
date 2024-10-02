using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D2 RID: 1234
	public class ECKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06002FDE RID: 12254 RVA: 0x0012665F File Offset: 0x0012485F
		public ECKeyGenerationParameters(ECDomainParameters domainParameters, SecureRandom random) : base(random, domainParameters.N.BitLength)
		{
			this.domainParams = domainParameters;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x0012667A File Offset: 0x0012487A
		public ECKeyGenerationParameters(DerObjectIdentifier publicKeyParamSet, SecureRandom random) : this(ECKeyParameters.LookupParameters(publicKeyParamSet), random)
		{
			this.publicKeyParamSet = publicKeyParamSet;
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x00126690 File Offset: 0x00124890
		public ECDomainParameters DomainParameters
		{
			get
			{
				return this.domainParams;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x00126698 File Offset: 0x00124898
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x04001FD7 RID: 8151
		private readonly ECDomainParameters domainParams;

		// Token: 0x04001FD8 RID: 8152
		private readonly DerObjectIdentifier publicKeyParamSet;
	}
}
