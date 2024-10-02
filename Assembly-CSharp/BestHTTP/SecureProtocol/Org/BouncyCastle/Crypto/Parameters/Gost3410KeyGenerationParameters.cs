using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E1 RID: 1249
	public class Gost3410KeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06003034 RID: 12340 RVA: 0x00127174 File Offset: 0x00125374
		public Gost3410KeyGenerationParameters(SecureRandom random, Gost3410Parameters parameters) : base(random, parameters.P.BitLength - 1)
		{
			this.parameters = parameters;
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x00127191 File Offset: 0x00125391
		public Gost3410KeyGenerationParameters(SecureRandom random, DerObjectIdentifier publicKeyParamSet) : this(random, Gost3410KeyGenerationParameters.LookupParameters(publicKeyParamSet))
		{
			this.publicKeyParamSet = publicKeyParamSet;
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06003036 RID: 12342 RVA: 0x001271A7 File Offset: 0x001253A7
		public Gost3410Parameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x001271AF File Offset: 0x001253AF
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x001271B8 File Offset: 0x001253B8
		private static Gost3410Parameters LookupParameters(DerObjectIdentifier publicKeyParamSet)
		{
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			Gost3410ParamSetParameters byOid = Gost3410NamedParameters.GetByOid(publicKeyParamSet);
			if (byOid == null)
			{
				throw new ArgumentException("OID is not a valid CryptoPro public key parameter set", "publicKeyParamSet");
			}
			return new Gost3410Parameters(byOid.P, byOid.Q, byOid.A);
		}

		// Token: 0x04001FF0 RID: 8176
		private readonly Gost3410Parameters parameters;

		// Token: 0x04001FF1 RID: 8177
		private readonly DerObjectIdentifier publicKeyParamSet;
	}
}
