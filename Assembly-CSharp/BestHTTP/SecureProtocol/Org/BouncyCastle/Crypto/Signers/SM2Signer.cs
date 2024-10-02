using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A9 RID: 1193
	public class SM2Signer : ISigner
	{
		// Token: 0x06002EC8 RID: 11976 RVA: 0x00122B02 File Offset: 0x00120D02
		public SM2Signer()
		{
			this.encoding = StandardDsaEncoding.Instance;
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x00122B2B File Offset: 0x00120D2B
		public SM2Signer(IDsaEncoding encoding)
		{
			this.encoding = encoding;
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002ECA RID: 11978 RVA: 0x00122B50 File Offset: 0x00120D50
		public virtual string AlgorithmName
		{
			get
			{
				return "SM2Sign";
			}
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x00122B58 File Offset: 0x00120D58
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			ICipherParameters cipherParameters;
			byte[] userID;
			if (parameters is ParametersWithID)
			{
				cipherParameters = ((ParametersWithID)parameters).Parameters;
				userID = ((ParametersWithID)parameters).GetID();
			}
			else
			{
				cipherParameters = parameters;
				userID = Hex.Decode("31323334353637383132333435363738");
			}
			if (forSigning)
			{
				if (cipherParameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)cipherParameters;
					this.ecKey = (ECKeyParameters)parametersWithRandom.Parameters;
					this.ecParams = this.ecKey.Parameters;
					this.kCalculator.Init(this.ecParams.N, parametersWithRandom.Random);
				}
				else
				{
					this.ecKey = (ECKeyParameters)cipherParameters;
					this.ecParams = this.ecKey.Parameters;
					this.kCalculator.Init(this.ecParams.N, new SecureRandom());
				}
				this.pubPoint = this.CreateBasePointMultiplier().Multiply(this.ecParams.G, ((ECPrivateKeyParameters)this.ecKey).D).Normalize();
			}
			else
			{
				this.ecKey = (ECKeyParameters)cipherParameters;
				this.ecParams = this.ecKey.Parameters;
				this.pubPoint = ((ECPublicKeyParameters)this.ecKey).Q;
			}
			this.digest.Reset();
			this.z = this.GetZ(userID);
			this.digest.BlockUpdate(this.z, 0, this.z.Length);
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x00122CB9 File Offset: 0x00120EB9
		public virtual void Update(byte b)
		{
			this.digest.Update(b);
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x00122CC7 File Offset: 0x00120EC7
		public virtual void BlockUpdate(byte[] buf, int off, int len)
		{
			this.digest.BlockUpdate(buf, off, len);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x00122CD8 File Offset: 0x00120ED8
		public virtual bool VerifySignature(byte[] signature)
		{
			try
			{
				BigInteger[] array = this.encoding.Decode(this.ecParams.N, signature);
				return this.VerifySignature(array[0], array[1]);
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x00122D24 File Offset: 0x00120F24
		public virtual void Reset()
		{
			if (this.z != null)
			{
				this.digest.Reset();
				this.digest.BlockUpdate(this.z, 0, this.z.Length);
			}
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x00122D54 File Offset: 0x00120F54
		public virtual byte[] GenerateSignature()
		{
			byte[] message = DigestUtilities.DoFinal(this.digest);
			BigInteger n = this.ecParams.N;
			BigInteger bigInteger = this.CalculateE(message);
			BigInteger d = ((ECPrivateKeyParameters)this.ecKey).D;
			ECMultiplier ecmultiplier = this.CreateBasePointMultiplier();
			BigInteger bigInteger3;
			BigInteger bigInteger5;
			for (;;)
			{
				BigInteger bigInteger2 = this.kCalculator.NextK();
				ECPoint ecpoint = ecmultiplier.Multiply(this.ecParams.G, bigInteger2).Normalize();
				bigInteger3 = bigInteger.Add(ecpoint.AffineXCoord.ToBigInteger()).Mod(n);
				if (bigInteger3.SignValue != 0 && !bigInteger3.Add(bigInteger2).Equals(n))
				{
					BigInteger bigInteger4 = d.Add(BigInteger.One).ModInverse(n);
					bigInteger5 = bigInteger2.Subtract(bigInteger3.Multiply(d)).Mod(n);
					bigInteger5 = bigInteger4.Multiply(bigInteger5).Mod(n);
					if (bigInteger5.SignValue != 0)
					{
						break;
					}
				}
			}
			byte[] result;
			try
			{
				result = this.encoding.Encode(this.ecParams.N, bigInteger3, bigInteger5);
			}
			catch (Exception ex)
			{
				throw new CryptoException("unable to encode signature: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x00122E88 File Offset: 0x00121088
		private bool VerifySignature(BigInteger r, BigInteger s)
		{
			BigInteger n = this.ecParams.N;
			if (r.CompareTo(BigInteger.One) < 0 || r.CompareTo(n) >= 0)
			{
				return false;
			}
			if (s.CompareTo(BigInteger.One) < 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			byte[] message = DigestUtilities.DoFinal(this.digest);
			BigInteger bigInteger = this.CalculateE(message);
			BigInteger bigInteger2 = r.Add(s).Mod(n);
			if (bigInteger2.SignValue == 0)
			{
				return false;
			}
			ECPoint q = ((ECPublicKeyParameters)this.ecKey).Q;
			ECPoint ecpoint = ECAlgorithms.SumOfTwoMultiplies(this.ecParams.G, s, q, bigInteger2).Normalize();
			return !ecpoint.IsInfinity && r.Equals(bigInteger.Add(ecpoint.AffineXCoord.ToBigInteger()).Mod(n));
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x00122F58 File Offset: 0x00121158
		private byte[] GetZ(byte[] userID)
		{
			this.AddUserID(this.digest, userID);
			this.AddFieldElement(this.digest, this.ecParams.Curve.A);
			this.AddFieldElement(this.digest, this.ecParams.Curve.B);
			this.AddFieldElement(this.digest, this.ecParams.G.AffineXCoord);
			this.AddFieldElement(this.digest, this.ecParams.G.AffineYCoord);
			this.AddFieldElement(this.digest, this.pubPoint.AffineXCoord);
			this.AddFieldElement(this.digest, this.pubPoint.AffineYCoord);
			return DigestUtilities.DoFinal(this.digest);
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x0012301C File Offset: 0x0012121C
		private void AddUserID(IDigest digest, byte[] userID)
		{
			int num = userID.Length * 8;
			digest.Update((byte)(num >> 8));
			digest.Update((byte)num);
			digest.BlockUpdate(userID, 0, userID.Length);
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x0012304C File Offset: 0x0012124C
		private void AddFieldElement(IDigest digest, ECFieldElement v)
		{
			byte[] encoded = v.GetEncoded();
			digest.BlockUpdate(encoded, 0, encoded.Length);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x0012306B File Offset: 0x0012126B
		protected virtual BigInteger CalculateE(byte[] message)
		{
			return new BigInteger(1, message);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x0011F41C File Offset: 0x0011D61C
		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}

		// Token: 0x04001F43 RID: 8003
		private readonly IDsaKCalculator kCalculator = new RandomDsaKCalculator();

		// Token: 0x04001F44 RID: 8004
		private readonly SM3Digest digest = new SM3Digest();

		// Token: 0x04001F45 RID: 8005
		private readonly IDsaEncoding encoding;

		// Token: 0x04001F46 RID: 8006
		private ECDomainParameters ecParams;

		// Token: 0x04001F47 RID: 8007
		private ECPoint pubPoint;

		// Token: 0x04001F48 RID: 8008
		private ECKeyParameters ecKey;

		// Token: 0x04001F49 RID: 8009
		private byte[] z;
	}
}
