using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000594 RID: 1428
	public class SM2Engine
	{
		// Token: 0x06003609 RID: 13833 RVA: 0x0014C54C File Offset: 0x0014A74C
		public SM2Engine() : this(new SM3Digest())
		{
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x0014C559 File Offset: 0x0014A759
		public SM2Engine(IDigest digest)
		{
			this.mDigest = digest;
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x0014C568 File Offset: 0x0014A768
		public virtual void Init(bool forEncryption, ICipherParameters param)
		{
			this.mForEncryption = forEncryption;
			if (forEncryption)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
				this.mECKey = (ECKeyParameters)parametersWithRandom.Parameters;
				this.mECParams = this.mECKey.Parameters;
				if (((ECPublicKeyParameters)this.mECKey).Q.Multiply(this.mECParams.H).IsInfinity)
				{
					throw new ArgumentException("invalid key: [h]Q at infinity");
				}
				this.mRandom = parametersWithRandom.Random;
			}
			else
			{
				this.mECKey = (ECKeyParameters)param;
				this.mECParams = this.mECKey.Parameters;
			}
			this.mCurveLength = (this.mECParams.Curve.FieldSize + 7) / 8;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x0014C61F File Offset: 0x0014A81F
		public virtual byte[] ProcessBlock(byte[] input, int inOff, int inLen)
		{
			if (this.mForEncryption)
			{
				return this.Encrypt(input, inOff, inLen);
			}
			return this.Decrypt(input, inOff, inLen);
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x0011F41C File Offset: 0x0011D61C
		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x0014C63C File Offset: 0x0014A83C
		private byte[] Encrypt(byte[] input, int inOff, int inLen)
		{
			byte[] array = new byte[inLen];
			Array.Copy(input, inOff, array, 0, array.Length);
			ECMultiplier ecmultiplier = this.CreateBasePointMultiplier();
			byte[] encoded;
			ECPoint ecpoint;
			do
			{
				BigInteger bigInteger = this.NextK();
				encoded = ecmultiplier.Multiply(this.mECParams.G, bigInteger).Normalize().GetEncoded(false);
				ecpoint = ((ECPublicKeyParameters)this.mECKey).Q.Multiply(bigInteger).Normalize();
				this.Kdf(this.mDigest, ecpoint, array);
			}
			while (this.NotEncrypted(array, input, inOff));
			this.AddFieldElement(this.mDigest, ecpoint.AffineXCoord);
			this.mDigest.BlockUpdate(input, inOff, inLen);
			this.AddFieldElement(this.mDigest, ecpoint.AffineYCoord);
			byte[] array2 = DigestUtilities.DoFinal(this.mDigest);
			return Arrays.ConcatenateAll(new byte[][]
			{
				encoded,
				array,
				array2
			});
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x0014C718 File Offset: 0x0014A918
		private byte[] Decrypt(byte[] input, int inOff, int inLen)
		{
			byte[] array = new byte[this.mCurveLength * 2 + 1];
			Array.Copy(input, inOff, array, 0, array.Length);
			ECPoint ecpoint = this.mECParams.Curve.DecodePoint(array);
			if (ecpoint.Multiply(this.mECParams.H).IsInfinity)
			{
				throw new InvalidCipherTextException("[h]C1 at infinity");
			}
			ecpoint = ecpoint.Multiply(((ECPrivateKeyParameters)this.mECKey).D).Normalize();
			byte[] array2 = new byte[inLen - array.Length - this.mDigest.GetDigestSize()];
			Array.Copy(input, inOff + array.Length, array2, 0, array2.Length);
			this.Kdf(this.mDigest, ecpoint, array2);
			this.AddFieldElement(this.mDigest, ecpoint.AffineXCoord);
			this.mDigest.BlockUpdate(array2, 0, array2.Length);
			this.AddFieldElement(this.mDigest, ecpoint.AffineYCoord);
			byte[] array3 = DigestUtilities.DoFinal(this.mDigest);
			int num = 0;
			for (int num2 = 0; num2 != array3.Length; num2++)
			{
				num |= (int)(array3[num2] ^ input[inOff + array.Length + array2.Length + num2]);
			}
			Arrays.Fill(array, 0);
			Arrays.Fill(array3, 0);
			if (num != 0)
			{
				Arrays.Fill(array2, 0);
				throw new InvalidCipherTextException("invalid cipher text");
			}
			return array2;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x0014C85C File Offset: 0x0014AA5C
		private bool NotEncrypted(byte[] encData, byte[] input, int inOff)
		{
			for (int num = 0; num != encData.Length; num++)
			{
				if (encData[num] != input[inOff])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x0014C884 File Offset: 0x0014AA84
		private void Kdf(IDigest digest, ECPoint c1, byte[] encData)
		{
			int digestSize = digest.GetDigestSize();
			byte[] array = new byte[Math.Max(4, digestSize)];
			int i = 0;
			IMemoable memoable = digest as IMemoable;
			IMemoable other = null;
			if (memoable != null)
			{
				this.AddFieldElement(digest, c1.AffineXCoord);
				this.AddFieldElement(digest, c1.AffineYCoord);
				other = memoable.Copy();
			}
			uint num = 0U;
			while (i < encData.Length)
			{
				if (memoable != null)
				{
					memoable.Reset(other);
				}
				else
				{
					this.AddFieldElement(digest, c1.AffineXCoord);
					this.AddFieldElement(digest, c1.AffineYCoord);
				}
				Pack.UInt32_To_BE(num += 1U, array, 0);
				digest.BlockUpdate(array, 0, 4);
				digest.DoFinal(array, 0);
				int num2 = Math.Min(digestSize, encData.Length - i);
				this.Xor(encData, array, i, num2);
				i += num2;
			}
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x0014C948 File Offset: 0x0014AB48
		private void Xor(byte[] data, byte[] kdfOut, int dOff, int dRemaining)
		{
			for (int num = 0; num != dRemaining; num++)
			{
				int num2 = dOff + num;
				data[num2] ^= kdfOut[num];
			}
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x0014C974 File Offset: 0x0014AB74
		private BigInteger NextK()
		{
			int bitLength = this.mECParams.N.BitLength;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(bitLength, this.mRandom);
			}
			while (bigInteger.SignValue == 0 || bigInteger.CompareTo(this.mECParams.N) >= 0);
			return bigInteger;
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x0014C9BC File Offset: 0x0014ABBC
		private void AddFieldElement(IDigest digest, ECFieldElement v)
		{
			byte[] encoded = v.GetEncoded();
			digest.BlockUpdate(encoded, 0, encoded.Length);
		}

		// Token: 0x0400233A RID: 9018
		private readonly IDigest mDigest;

		// Token: 0x0400233B RID: 9019
		private bool mForEncryption;

		// Token: 0x0400233C RID: 9020
		private ECKeyParameters mECKey;

		// Token: 0x0400233D RID: 9021
		private ECDomainParameters mECParams;

		// Token: 0x0400233E RID: 9022
		private int mCurveLength;

		// Token: 0x0400233F RID: 9023
		private SecureRandom mRandom;
	}
}
