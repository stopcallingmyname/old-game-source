using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000494 RID: 1172
	public class ECDsaSigner : IDsaExt, IDsa
	{
		// Token: 0x06002E11 RID: 11793 RVA: 0x0011F0E4 File Offset: 0x0011D2E4
		public ECDsaSigner()
		{
			this.kCalculator = new RandomDsaKCalculator();
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0011F0F7 File Offset: 0x0011D2F7
		public ECDsaSigner(IDsaKCalculator kCalculator)
		{
			this.kCalculator = kCalculator;
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002E13 RID: 11795 RVA: 0x0011F106 File Offset: 0x0011D306
		public virtual string AlgorithmName
		{
			get
			{
				return "ECDSA";
			}
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x0011F110 File Offset: 0x0011D310
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			SecureRandom provided = null;
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
					provided = parametersWithRandom.Random;
					parameters = parametersWithRandom.Parameters;
				}
				if (!(parameters is ECPrivateKeyParameters))
				{
					throw new InvalidKeyException("EC private key required for signing");
				}
				this.key = (ECPrivateKeyParameters)parameters;
			}
			else
			{
				if (!(parameters is ECPublicKeyParameters))
				{
					throw new InvalidKeyException("EC public key required for verification");
				}
				this.key = (ECPublicKeyParameters)parameters;
			}
			this.random = this.InitSecureRandom(forSigning && !this.kCalculator.IsDeterministic, provided);
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002E15 RID: 11797 RVA: 0x0011F19F File Offset: 0x0011D39F
		public virtual BigInteger Order
		{
			get
			{
				return this.key.Parameters.N;
			}
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x0011F1B4 File Offset: 0x0011D3B4
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			ECDomainParameters parameters = this.key.Parameters;
			BigInteger n = parameters.N;
			BigInteger bigInteger = this.CalculateE(n, message);
			BigInteger d = ((ECPrivateKeyParameters)this.key).D;
			if (this.kCalculator.IsDeterministic)
			{
				this.kCalculator.Init(n, d, message);
			}
			else
			{
				this.kCalculator.Init(n, this.random);
			}
			ECMultiplier ecmultiplier = this.CreateBasePointMultiplier();
			BigInteger bigInteger3;
			BigInteger bigInteger4;
			for (;;)
			{
				BigInteger bigInteger2 = this.kCalculator.NextK();
				bigInteger3 = ecmultiplier.Multiply(parameters.G, bigInteger2).Normalize().AffineXCoord.ToBigInteger().Mod(n);
				if (bigInteger3.SignValue != 0)
				{
					bigInteger4 = bigInteger2.ModInverse(n).Multiply(bigInteger.Add(d.Multiply(bigInteger3))).Mod(n);
					if (bigInteger4.SignValue != 0)
					{
						break;
					}
				}
			}
			return new BigInteger[]
			{
				bigInteger3,
				bigInteger4
			};
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0011F2A0 File Offset: 0x0011D4A0
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			BigInteger n = this.key.Parameters.N;
			if (r.SignValue < 1 || s.SignValue < 1 || r.CompareTo(n) >= 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			BigInteger bigInteger = this.CalculateE(n, message);
			BigInteger val = s.ModInverse(n);
			BigInteger a = bigInteger.Multiply(val).Mod(n);
			BigInteger b = r.Multiply(val).Mod(n);
			ECPoint g = this.key.Parameters.G;
			ECPoint q = ((ECPublicKeyParameters)this.key).Q;
			ECPoint ecpoint = ECAlgorithms.SumOfTwoMultiplies(g, a, q, b);
			if (ecpoint.IsInfinity)
			{
				return false;
			}
			ECCurve curve = ecpoint.Curve;
			if (curve != null)
			{
				BigInteger cofactor = curve.Cofactor;
				if (cofactor != null && cofactor.CompareTo(ECDsaSigner.Eight) <= 0)
				{
					ECFieldElement denominator = this.GetDenominator(curve.CoordinateSystem, ecpoint);
					if (denominator != null && !denominator.IsZero)
					{
						ECFieldElement xcoord = ecpoint.XCoord;
						while (curve.IsValidFieldElement(r))
						{
							if (curve.FromBigInteger(r).Multiply(denominator).Equals(xcoord))
							{
								return true;
							}
							r = r.Add(n);
						}
						return false;
					}
				}
			}
			return ecpoint.Normalize().AffineXCoord.ToBigInteger().Mod(n).Equals(r);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x0011F3E8 File Offset: 0x0011D5E8
		protected virtual BigInteger CalculateE(BigInteger n, byte[] message)
		{
			int num = message.Length * 8;
			BigInteger bigInteger = new BigInteger(1, message);
			if (n.BitLength < num)
			{
				bigInteger = bigInteger.ShiftRight(num - n.BitLength);
			}
			return bigInteger;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0011F41C File Offset: 0x0011D61C
		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x0011F423 File Offset: 0x0011D623
		protected virtual ECFieldElement GetDenominator(int coordinateSystem, ECPoint p)
		{
			switch (coordinateSystem)
			{
			case 1:
			case 6:
			case 7:
				return p.GetZCoord(0);
			case 2:
			case 3:
			case 4:
				return p.GetZCoord(0).Square();
			}
			return null;
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x0011F0D3 File Offset: 0x0011D2D3
		protected virtual SecureRandom InitSecureRandom(bool needed, SecureRandom provided)
		{
			if (!needed)
			{
				return null;
			}
			if (provided == null)
			{
				return new SecureRandom();
			}
			return provided;
		}

		// Token: 0x04001EC7 RID: 7879
		private static readonly BigInteger Eight = BigInteger.ValueOf(8L);

		// Token: 0x04001EC8 RID: 7880
		protected readonly IDsaKCalculator kCalculator;

		// Token: 0x04001EC9 RID: 7881
		protected ECKeyParameters key;

		// Token: 0x04001ECA RID: 7882
		protected SecureRandom random;
	}
}
