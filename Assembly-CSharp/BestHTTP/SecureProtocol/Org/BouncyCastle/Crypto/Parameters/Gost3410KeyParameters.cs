using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E2 RID: 1250
	public abstract class Gost3410KeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06003039 RID: 12345 RVA: 0x00127204 File Offset: 0x00125404
		protected Gost3410KeyParameters(bool isPrivate, Gost3410Parameters parameters) : base(isPrivate)
		{
			this.parameters = parameters;
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x00127214 File Offset: 0x00125414
		protected Gost3410KeyParameters(bool isPrivate, DerObjectIdentifier publicKeyParamSet) : base(isPrivate)
		{
			this.parameters = Gost3410KeyParameters.LookupParameters(publicKeyParamSet);
			this.publicKeyParamSet = publicKeyParamSet;
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x00127230 File Offset: 0x00125430
		public Gost3410Parameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x00127238 File Offset: 0x00125438
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x00127240 File Offset: 0x00125440
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

		// Token: 0x04001FF2 RID: 8178
		private readonly Gost3410Parameters parameters;

		// Token: 0x04001FF3 RID: 8179
		private readonly DerObjectIdentifier publicKeyParamSet;
	}
}
