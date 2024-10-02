using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D3 RID: 1235
	public abstract class ECKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06002FE2 RID: 12258 RVA: 0x001266A0 File Offset: 0x001248A0
		protected ECKeyParameters(string algorithm, bool isPrivate, ECDomainParameters parameters) : base(isPrivate)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.algorithm = ECKeyParameters.VerifyAlgorithmName(algorithm);
			this.parameters = parameters;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x001266D8 File Offset: 0x001248D8
		protected ECKeyParameters(string algorithm, bool isPrivate, DerObjectIdentifier publicKeyParamSet) : base(isPrivate)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			this.algorithm = ECKeyParameters.VerifyAlgorithmName(algorithm);
			this.parameters = ECKeyParameters.LookupParameters(publicKeyParamSet);
			this.publicKeyParamSet = publicKeyParamSet;
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x00126727 File Offset: 0x00124927
		public string AlgorithmName
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x0012672F File Offset: 0x0012492F
		public ECDomainParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x00126737 File Offset: 0x00124937
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x00126740 File Offset: 0x00124940
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECDomainParameters ecdomainParameters = obj as ECDomainParameters;
			return ecdomainParameters != null && this.Equals(ecdomainParameters);
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x00126766 File Offset: 0x00124966
		protected bool Equals(ECKeyParameters other)
		{
			return this.parameters.Equals(other.parameters) && base.Equals(other);
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x00126784 File Offset: 0x00124984
		public override int GetHashCode()
		{
			return this.parameters.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x00126798 File Offset: 0x00124998
		internal ECKeyGenerationParameters CreateKeyGenerationParameters(SecureRandom random)
		{
			if (this.publicKeyParamSet != null)
			{
				return new ECKeyGenerationParameters(this.publicKeyParamSet, random);
			}
			return new ECKeyGenerationParameters(this.parameters, random);
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x001267BB File Offset: 0x001249BB
		internal static string VerifyAlgorithmName(string algorithm)
		{
			string result = Platform.ToUpperInvariant(algorithm);
			if (Array.IndexOf<string>(ECKeyParameters.algorithms, algorithm, 0, ECKeyParameters.algorithms.Length) < 0)
			{
				throw new ArgumentException("unrecognised algorithm: " + algorithm, "algorithm");
			}
			return result;
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x001267F0 File Offset: 0x001249F0
		internal static ECDomainParameters LookupParameters(DerObjectIdentifier publicKeyParamSet)
		{
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			ECDomainParameters ecdomainParameters = ECGost3410NamedCurves.GetByOid(publicKeyParamSet);
			if (ecdomainParameters == null)
			{
				X9ECParameters x9ECParameters = ECKeyPairGenerator.FindECCurveByOid(publicKeyParamSet);
				if (x9ECParameters == null)
				{
					throw new ArgumentException("OID is not a valid public key parameter set", "publicKeyParamSet");
				}
				ecdomainParameters = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N, x9ECParameters.H, x9ECParameters.GetSeed());
			}
			return ecdomainParameters;
		}

		// Token: 0x04001FD9 RID: 8153
		private static readonly string[] algorithms = new string[]
		{
			"EC",
			"ECDSA",
			"ECDH",
			"ECDHC",
			"ECGOST3410",
			"ECMQV"
		};

		// Token: 0x04001FDA RID: 8154
		private readonly string algorithm;

		// Token: 0x04001FDB RID: 8155
		private readonly ECDomainParameters parameters;

		// Token: 0x04001FDC RID: 8156
		private readonly DerObjectIdentifier publicKeyParamSet;
	}
}
