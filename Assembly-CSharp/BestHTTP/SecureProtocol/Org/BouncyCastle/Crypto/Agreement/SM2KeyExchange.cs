using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020005CB RID: 1483
	public class SM2KeyExchange
	{
		// Token: 0x060038E6 RID: 14566 RVA: 0x00164B67 File Offset: 0x00162D67
		public SM2KeyExchange() : this(new SM3Digest())
		{
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x00164B74 File Offset: 0x00162D74
		public SM2KeyExchange(IDigest digest)
		{
			this.mDigest = digest;
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x00164B84 File Offset: 0x00162D84
		public virtual void Init(ICipherParameters privParam)
		{
			SM2KeyExchangePrivateParameters sm2KeyExchangePrivateParameters;
			if (privParam is ParametersWithID)
			{
				sm2KeyExchangePrivateParameters = (SM2KeyExchangePrivateParameters)((ParametersWithID)privParam).Parameters;
				this.mUserID = ((ParametersWithID)privParam).GetID();
			}
			else
			{
				sm2KeyExchangePrivateParameters = (SM2KeyExchangePrivateParameters)privParam;
				this.mUserID = new byte[0];
			}
			this.mInitiator = sm2KeyExchangePrivateParameters.IsInitiator;
			this.mStaticKey = sm2KeyExchangePrivateParameters.StaticPrivateKey;
			this.mEphemeralKey = sm2KeyExchangePrivateParameters.EphemeralPrivateKey;
			this.mECParams = this.mStaticKey.Parameters;
			this.mStaticPubPoint = sm2KeyExchangePrivateParameters.StaticPublicPoint;
			this.mEphemeralPubPoint = sm2KeyExchangePrivateParameters.EphemeralPublicPoint;
			this.mW = this.mECParams.Curve.FieldSize / 2 - 1;
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x00164C38 File Offset: 0x00162E38
		public virtual byte[] CalculateKey(int kLen, ICipherParameters pubParam)
		{
			SM2KeyExchangePublicParameters sm2KeyExchangePublicParameters;
			byte[] userID;
			if (pubParam is ParametersWithID)
			{
				sm2KeyExchangePublicParameters = (SM2KeyExchangePublicParameters)((ParametersWithID)pubParam).Parameters;
				userID = ((ParametersWithID)pubParam).GetID();
			}
			else
			{
				sm2KeyExchangePublicParameters = (SM2KeyExchangePublicParameters)pubParam;
				userID = new byte[0];
			}
			byte[] z = this.GetZ(this.mDigest, this.mUserID, this.mStaticPubPoint);
			byte[] z2 = this.GetZ(this.mDigest, userID, sm2KeyExchangePublicParameters.StaticPublicKey.Q);
			ECPoint u = this.CalculateU(sm2KeyExchangePublicParameters);
			byte[] result;
			if (this.mInitiator)
			{
				result = this.Kdf(u, z, z2, kLen);
			}
			else
			{
				result = this.Kdf(u, z2, z, kLen);
			}
			return result;
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x00164CDC File Offset: 0x00162EDC
		public virtual byte[][] CalculateKeyWithConfirmation(int kLen, byte[] confirmationTag, ICipherParameters pubParam)
		{
			SM2KeyExchangePublicParameters sm2KeyExchangePublicParameters;
			byte[] userID;
			if (pubParam is ParametersWithID)
			{
				sm2KeyExchangePublicParameters = (SM2KeyExchangePublicParameters)((ParametersWithID)pubParam).Parameters;
				userID = ((ParametersWithID)pubParam).GetID();
			}
			else
			{
				sm2KeyExchangePublicParameters = (SM2KeyExchangePublicParameters)pubParam;
				userID = new byte[0];
			}
			if (this.mInitiator && confirmationTag == null)
			{
				throw new ArgumentException("if initiating, confirmationTag must be set");
			}
			byte[] z = this.GetZ(this.mDigest, this.mUserID, this.mStaticPubPoint);
			byte[] z2 = this.GetZ(this.mDigest, userID, sm2KeyExchangePublicParameters.StaticPublicKey.Q);
			ECPoint u = this.CalculateU(sm2KeyExchangePublicParameters);
			byte[] array;
			if (!this.mInitiator)
			{
				array = this.Kdf(u, z2, z, kLen);
				byte[] inner = this.CalculateInnerHash(this.mDigest, u, z2, z, sm2KeyExchangePublicParameters.EphemeralPublicKey.Q, this.mEphemeralPubPoint);
				return new byte[][]
				{
					array,
					this.S1(this.mDigest, u, inner),
					this.S2(this.mDigest, u, inner)
				};
			}
			array = this.Kdf(u, z, z2, kLen);
			byte[] inner2 = this.CalculateInnerHash(this.mDigest, u, z, z2, this.mEphemeralPubPoint, sm2KeyExchangePublicParameters.EphemeralPublicKey.Q);
			if (!Arrays.ConstantTimeAreEqual(this.S1(this.mDigest, u, inner2), confirmationTag))
			{
				throw new InvalidOperationException("confirmation tag mismatch");
			}
			return new byte[][]
			{
				array,
				this.S2(this.mDigest, u, inner2)
			};
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x00164E4C File Offset: 0x0016304C
		protected virtual ECPoint CalculateU(SM2KeyExchangePublicParameters otherPub)
		{
			ECDomainParameters parameters = this.mStaticKey.Parameters;
			ECPoint p = ECAlgorithms.CleanPoint(parameters.Curve, otherPub.StaticPublicKey.Q);
			ECPoint ecpoint = ECAlgorithms.CleanPoint(parameters.Curve, otherPub.EphemeralPublicKey.Q);
			BigInteger bigInteger = this.Reduce(this.mEphemeralPubPoint.AffineXCoord.ToBigInteger());
			BigInteger val = this.Reduce(ecpoint.AffineXCoord.ToBigInteger());
			BigInteger val2 = this.mStaticKey.D.Add(bigInteger.Multiply(this.mEphemeralKey.D));
			BigInteger bigInteger2 = this.mECParams.H.Multiply(val2).Mod(this.mECParams.N);
			BigInteger b = bigInteger2.Multiply(val).Mod(this.mECParams.N);
			return ECAlgorithms.SumOfTwoMultiplies(p, bigInteger2, ecpoint, b).Normalize();
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x00164F2C File Offset: 0x0016312C
		protected virtual byte[] Kdf(ECPoint u, byte[] za, byte[] zb, int klen)
		{
			int digestSize = this.mDigest.GetDigestSize();
			byte[] array = new byte[Math.Max(4, digestSize)];
			byte[] array2 = new byte[(klen + 7) / 8];
			int i = 0;
			IMemoable memoable = this.mDigest as IMemoable;
			IMemoable other = null;
			if (memoable != null)
			{
				this.AddFieldElement(this.mDigest, u.AffineXCoord);
				this.AddFieldElement(this.mDigest, u.AffineYCoord);
				this.mDigest.BlockUpdate(za, 0, za.Length);
				this.mDigest.BlockUpdate(zb, 0, zb.Length);
				other = memoable.Copy();
			}
			uint num = 0U;
			while (i < array2.Length)
			{
				if (memoable != null)
				{
					memoable.Reset(other);
				}
				else
				{
					this.AddFieldElement(this.mDigest, u.AffineXCoord);
					this.AddFieldElement(this.mDigest, u.AffineYCoord);
					this.mDigest.BlockUpdate(za, 0, za.Length);
					this.mDigest.BlockUpdate(zb, 0, zb.Length);
				}
				Pack.UInt32_To_BE(num += 1U, array, 0);
				this.mDigest.BlockUpdate(array, 0, 4);
				this.mDigest.DoFinal(array, 0);
				int num2 = Math.Min(digestSize, array2.Length - i);
				Array.Copy(array, 0, array2, i, num2);
				i += num2;
			}
			return array2;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x0016506D File Offset: 0x0016326D
		private BigInteger Reduce(BigInteger x)
		{
			return x.And(BigInteger.One.ShiftLeft(this.mW).Subtract(BigInteger.One)).SetBit(this.mW);
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x0016509A File Offset: 0x0016329A
		private byte[] S1(IDigest digest, ECPoint u, byte[] inner)
		{
			digest.Update(2);
			this.AddFieldElement(digest, u.AffineYCoord);
			digest.BlockUpdate(inner, 0, inner.Length);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x001650C4 File Offset: 0x001632C4
		private byte[] CalculateInnerHash(IDigest digest, ECPoint u, byte[] za, byte[] zb, ECPoint p1, ECPoint p2)
		{
			this.AddFieldElement(digest, u.AffineXCoord);
			digest.BlockUpdate(za, 0, za.Length);
			digest.BlockUpdate(zb, 0, zb.Length);
			this.AddFieldElement(digest, p1.AffineXCoord);
			this.AddFieldElement(digest, p1.AffineYCoord);
			this.AddFieldElement(digest, p2.AffineXCoord);
			this.AddFieldElement(digest, p2.AffineYCoord);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x00165134 File Offset: 0x00163334
		private byte[] S2(IDigest digest, ECPoint u, byte[] inner)
		{
			digest.Update(3);
			this.AddFieldElement(digest, u.AffineYCoord);
			digest.BlockUpdate(inner, 0, inner.Length);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x0016515C File Offset: 0x0016335C
		private byte[] GetZ(IDigest digest, byte[] userID, ECPoint pubPoint)
		{
			this.AddUserID(digest, userID);
			this.AddFieldElement(digest, this.mECParams.Curve.A);
			this.AddFieldElement(digest, this.mECParams.Curve.B);
			this.AddFieldElement(digest, this.mECParams.G.AffineXCoord);
			this.AddFieldElement(digest, this.mECParams.G.AffineYCoord);
			this.AddFieldElement(digest, pubPoint.AffineXCoord);
			this.AddFieldElement(digest, pubPoint.AffineYCoord);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x001651F0 File Offset: 0x001633F0
		private void AddUserID(IDigest digest, byte[] userID)
		{
			uint num = (uint)(userID.Length * 8);
			digest.Update((byte)(num >> 8));
			digest.Update((byte)num);
			digest.BlockUpdate(userID, 0, userID.Length);
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x00165220 File Offset: 0x00163420
		private void AddFieldElement(IDigest digest, ECFieldElement v)
		{
			byte[] encoded = v.GetEncoded();
			digest.BlockUpdate(encoded, 0, encoded.Length);
		}

		// Token: 0x0400253A RID: 9530
		private readonly IDigest mDigest;

		// Token: 0x0400253B RID: 9531
		private byte[] mUserID;

		// Token: 0x0400253C RID: 9532
		private ECPrivateKeyParameters mStaticKey;

		// Token: 0x0400253D RID: 9533
		private ECPoint mStaticPubPoint;

		// Token: 0x0400253E RID: 9534
		private ECPoint mEphemeralPubPoint;

		// Token: 0x0400253F RID: 9535
		private ECDomainParameters mECParams;

		// Token: 0x04002540 RID: 9536
		private int mW;

		// Token: 0x04002541 RID: 9537
		private ECPrivateKeyParameters mEphemeralKey;

		// Token: 0x04002542 RID: 9538
		private bool mInitiator;
	}
}
