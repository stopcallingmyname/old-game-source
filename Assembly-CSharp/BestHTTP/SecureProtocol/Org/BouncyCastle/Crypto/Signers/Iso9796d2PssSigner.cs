using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A2 RID: 1186
	public class Iso9796d2PssSigner : ISignerWithRecovery, ISigner
	{
		// Token: 0x06002E74 RID: 11892 RVA: 0x00120980 File Offset: 0x0011EB80
		public byte[] GetRecoveredMessage()
		{
			return this.recoveredMessage;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x00120988 File Offset: 0x0011EB88
		public Iso9796d2PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLength, bool isImplicit)
		{
			this.cipher = cipher;
			this.digest = digest;
			this.hLen = digest.GetDigestSize();
			this.saltLength = saltLength;
			if (isImplicit)
			{
				this.trailer = 188;
				return;
			}
			if (IsoTrailers.NoTrailerAvailable(digest))
			{
				throw new ArgumentException("no valid trailer", "digest");
			}
			this.trailer = IsoTrailers.GetTrailer(digest);
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x001209F0 File Offset: 0x0011EBF0
		public Iso9796d2PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLength) : this(cipher, digest, saltLength, false)
		{
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002E77 RID: 11895 RVA: 0x001209FC File Offset: 0x0011EBFC
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "withISO9796-2S2";
			}
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x00120A14 File Offset: 0x0011EC14
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			RsaKeyParameters rsaKeyParameters;
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				rsaKeyParameters = (RsaKeyParameters)parametersWithRandom.Parameters;
				if (forSigning)
				{
					this.random = parametersWithRandom.Random;
				}
			}
			else if (parameters is ParametersWithSalt)
			{
				if (!forSigning)
				{
					throw new ArgumentException("ParametersWithSalt only valid for signing", "parameters");
				}
				ParametersWithSalt parametersWithSalt = (ParametersWithSalt)parameters;
				rsaKeyParameters = (RsaKeyParameters)parametersWithSalt.Parameters;
				this.standardSalt = parametersWithSalt.GetSalt();
				if (this.standardSalt.Length != this.saltLength)
				{
					throw new ArgumentException("Fixed salt is of wrong length");
				}
			}
			else
			{
				rsaKeyParameters = (RsaKeyParameters)parameters;
				if (forSigning)
				{
					this.random = new SecureRandom();
				}
			}
			this.cipher.Init(forSigning, rsaKeyParameters);
			this.keyBits = rsaKeyParameters.Modulus.BitLength;
			this.block = new byte[(this.keyBits + 7) / 8];
			if (this.trailer == 188)
			{
				this.mBuf = new byte[this.block.Length - this.digest.GetDigestSize() - this.saltLength - 1 - 1];
			}
			else
			{
				this.mBuf = new byte[this.block.Length - this.digest.GetDigestSize() - this.saltLength - 1 - 2];
			}
			this.Reset();
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x00120B54 File Offset: 0x0011ED54
		private bool IsSameAs(byte[] a, byte[] b)
		{
			if (this.messageLength != b.Length)
			{
				return false;
			}
			bool result = true;
			for (int num = 0; num != b.Length; num++)
			{
				if (a[num] != b[num])
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x00120B89 File Offset: 0x0011ED89
		private void ClearBlock(byte[] block)
		{
			Array.Clear(block, 0, block.Length);
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x00120B98 File Offset: 0x0011ED98
		public virtual void UpdateWithRecoveredMessage(byte[] signature)
		{
			byte[] array = this.cipher.ProcessBlock(signature, 0, signature.Length);
			if (array.Length < (this.keyBits + 7) / 8)
			{
				byte[] array2 = new byte[(this.keyBits + 7) / 8];
				Array.Copy(array, 0, array2, array2.Length - array.Length, array.Length);
				this.ClearBlock(array);
				array = array2;
			}
			int num;
			if (((array[array.Length - 1] & 255) ^ 188) == 0)
			{
				num = 1;
			}
			else
			{
				int num2 = (int)(array[array.Length - 2] & byte.MaxValue) << 8 | (int)(array[array.Length - 1] & byte.MaxValue);
				if (IsoTrailers.NoTrailerAvailable(this.digest))
				{
					throw new ArgumentException("unrecognised hash in signature");
				}
				if (num2 != IsoTrailers.GetTrailer(this.digest))
				{
					throw new InvalidOperationException("signer initialised with wrong digest for trailer " + num2);
				}
				num = 2;
			}
			byte[] output = new byte[this.hLen];
			this.digest.DoFinal(output, 0);
			byte[] array3 = this.MaskGeneratorFunction1(array, array.Length - this.hLen - num, this.hLen, array.Length - this.hLen - num);
			for (int num3 = 0; num3 != array3.Length; num3++)
			{
				byte[] array4 = array;
				int num4 = num3;
				array4[num4] ^= array3[num3];
			}
			byte[] array5 = array;
			int num5 = 0;
			array5[num5] &= 127;
			int num6 = 0;
			while (num6 < array.Length && array[num6++] != 1)
			{
			}
			if (num6 >= array.Length)
			{
				this.ClearBlock(array);
			}
			this.fullMessage = (num6 > 1);
			this.recoveredMessage = new byte[array3.Length - num6 - this.saltLength];
			Array.Copy(array, num6, this.recoveredMessage, 0, this.recoveredMessage.Length);
			this.recoveredMessage.CopyTo(this.mBuf, 0);
			this.preSig = signature;
			this.preBlock = array;
			this.preMStart = num6;
			this.preTLength = num;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x00120D6C File Offset: 0x0011EF6C
		public virtual void Update(byte input)
		{
			if (this.preSig == null && this.messageLength < this.mBuf.Length)
			{
				byte[] array = this.mBuf;
				int num = this.messageLength;
				this.messageLength = num + 1;
				array[num] = input;
				return;
			}
			this.digest.Update(input);
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x00120DB8 File Offset: 0x0011EFB8
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			if (this.preSig == null)
			{
				while (length > 0 && this.messageLength < this.mBuf.Length)
				{
					this.Update(input[inOff]);
					inOff++;
					length--;
				}
			}
			if (length > 0)
			{
				this.digest.BlockUpdate(input, inOff, length);
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x00120E08 File Offset: 0x0011F008
		public virtual void Reset()
		{
			this.digest.Reset();
			this.messageLength = 0;
			if (this.mBuf != null)
			{
				this.ClearBlock(this.mBuf);
			}
			if (this.recoveredMessage != null)
			{
				this.ClearBlock(this.recoveredMessage);
				this.recoveredMessage = null;
			}
			this.fullMessage = false;
			if (this.preSig != null)
			{
				this.preSig = null;
				this.ClearBlock(this.preBlock);
				this.preBlock = null;
			}
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x00120E80 File Offset: 0x0011F080
		public virtual byte[] GenerateSignature()
		{
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			byte[] array2 = new byte[8];
			this.LtoOSP((long)(this.messageLength * 8), array2);
			this.digest.BlockUpdate(array2, 0, array2.Length);
			this.digest.BlockUpdate(this.mBuf, 0, this.messageLength);
			this.digest.BlockUpdate(array, 0, array.Length);
			byte[] array3;
			if (this.standardSalt != null)
			{
				array3 = this.standardSalt;
			}
			else
			{
				array3 = new byte[this.saltLength];
				this.random.NextBytes(array3);
			}
			this.digest.BlockUpdate(array3, 0, array3.Length);
			byte[] array4 = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array4, 0);
			int num = 2;
			if (this.trailer == 188)
			{
				num = 1;
			}
			int num2 = this.block.Length - this.messageLength - array3.Length - this.hLen - num - 1;
			this.block[num2] = 1;
			Array.Copy(this.mBuf, 0, this.block, num2 + 1, this.messageLength);
			Array.Copy(array3, 0, this.block, num2 + 1 + this.messageLength, array3.Length);
			byte[] array5 = this.MaskGeneratorFunction1(array4, 0, array4.Length, this.block.Length - this.hLen - num);
			for (int num3 = 0; num3 != array5.Length; num3++)
			{
				byte[] array6 = this.block;
				int num4 = num3;
				array6[num4] ^= array5[num3];
			}
			Array.Copy(array4, 0, this.block, this.block.Length - this.hLen - num, this.hLen);
			if (this.trailer == 188)
			{
				this.block[this.block.Length - 1] = 188;
			}
			else
			{
				this.block[this.block.Length - 2] = (byte)((uint)this.trailer >> 8);
				this.block[this.block.Length - 1] = (byte)this.trailer;
			}
			byte[] array7 = this.block;
			int num5 = 0;
			array7[num5] &= 127;
			byte[] result = this.cipher.ProcessBlock(this.block, 0, this.block.Length);
			this.ClearBlock(this.mBuf);
			this.ClearBlock(this.block);
			this.messageLength = 0;
			return result;
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x001210DC File Offset: 0x0011F2DC
		public virtual bool VerifySignature(byte[] signature)
		{
			byte[] array = new byte[this.hLen];
			this.digest.DoFinal(array, 0);
			if (this.preSig == null)
			{
				try
				{
					this.UpdateWithRecoveredMessage(signature);
					goto IL_4F;
				}
				catch (Exception)
				{
					return false;
				}
			}
			if (!Arrays.AreEqual(this.preSig, signature))
			{
				throw new InvalidOperationException("UpdateWithRecoveredMessage called on different signature");
			}
			IL_4F:
			byte[] array2 = this.preBlock;
			int num = this.preMStart;
			int num2 = this.preTLength;
			this.preSig = null;
			this.preBlock = null;
			byte[] array3 = new byte[8];
			this.LtoOSP((long)(this.recoveredMessage.Length * 8), array3);
			this.digest.BlockUpdate(array3, 0, array3.Length);
			if (this.recoveredMessage.Length != 0)
			{
				this.digest.BlockUpdate(this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			this.digest.BlockUpdate(array, 0, array.Length);
			if (this.standardSalt != null)
			{
				this.digest.BlockUpdate(this.standardSalt, 0, this.standardSalt.Length);
			}
			else
			{
				this.digest.BlockUpdate(array2, num + this.recoveredMessage.Length, this.saltLength);
			}
			byte[] array4 = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array4, 0);
			int num3 = array2.Length - num2 - array4.Length;
			bool flag = true;
			for (int num4 = 0; num4 != array4.Length; num4++)
			{
				if (array4[num4] != array2[num3 + num4])
				{
					flag = false;
				}
			}
			this.ClearBlock(array2);
			this.ClearBlock(array4);
			if (!flag)
			{
				this.fullMessage = false;
				this.messageLength = 0;
				this.ClearBlock(this.recoveredMessage);
				return false;
			}
			if (this.messageLength != 0 && !this.IsSameAs(this.mBuf, this.recoveredMessage))
			{
				this.messageLength = 0;
				this.ClearBlock(this.mBuf);
				return false;
			}
			this.messageLength = 0;
			this.ClearBlock(this.mBuf);
			return true;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x001212D8 File Offset: 0x0011F4D8
		public virtual bool HasFullMessage()
		{
			return this.fullMessage;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x001212E0 File Offset: 0x0011F4E0
		private void ItoOSP(int i, byte[] sp)
		{
			sp[0] = (byte)((uint)i >> 24);
			sp[1] = (byte)((uint)i >> 16);
			sp[2] = (byte)((uint)i >> 8);
			sp[3] = (byte)i;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x001212FE File Offset: 0x0011F4FE
		private void LtoOSP(long l, byte[] sp)
		{
			sp[0] = (byte)((ulong)l >> 56);
			sp[1] = (byte)((ulong)l >> 48);
			sp[2] = (byte)((ulong)l >> 40);
			sp[3] = (byte)((ulong)l >> 32);
			sp[4] = (byte)((ulong)l >> 24);
			sp[5] = (byte)((ulong)l >> 16);
			sp[6] = (byte)((ulong)l >> 8);
			sp[7] = (byte)l;
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x0012133C File Offset: 0x0011F53C
		private byte[] MaskGeneratorFunction1(byte[] Z, int zOff, int zLen, int length)
		{
			byte[] array = new byte[length];
			byte[] array2 = new byte[this.hLen];
			byte[] array3 = new byte[4];
			int num = 0;
			this.digest.Reset();
			do
			{
				this.ItoOSP(num, array3);
				this.digest.BlockUpdate(Z, zOff, zLen);
				this.digest.BlockUpdate(array3, 0, array3.Length);
				this.digest.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, num * this.hLen, this.hLen);
			}
			while (++num < length / this.hLen);
			if (num * this.hLen < length)
			{
				this.ItoOSP(num, array3);
				this.digest.BlockUpdate(Z, zOff, zLen);
				this.digest.BlockUpdate(array3, 0, array3.Length);
				this.digest.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, num * this.hLen, array.Length - num * this.hLen);
			}
			return array;
		}

		// Token: 0x04001EF4 RID: 7924
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerImplicit = 188;

		// Token: 0x04001EF5 RID: 7925
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerRipeMD160 = 12748;

		// Token: 0x04001EF6 RID: 7926
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerRipeMD128 = 13004;

		// Token: 0x04001EF7 RID: 7927
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha1 = 13260;

		// Token: 0x04001EF8 RID: 7928
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha256 = 13516;

		// Token: 0x04001EF9 RID: 7929
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha512 = 13772;

		// Token: 0x04001EFA RID: 7930
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha384 = 14028;

		// Token: 0x04001EFB RID: 7931
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerWhirlpool = 14284;

		// Token: 0x04001EFC RID: 7932
		private IDigest digest;

		// Token: 0x04001EFD RID: 7933
		private IAsymmetricBlockCipher cipher;

		// Token: 0x04001EFE RID: 7934
		private SecureRandom random;

		// Token: 0x04001EFF RID: 7935
		private byte[] standardSalt;

		// Token: 0x04001F00 RID: 7936
		private int hLen;

		// Token: 0x04001F01 RID: 7937
		private int trailer;

		// Token: 0x04001F02 RID: 7938
		private int keyBits;

		// Token: 0x04001F03 RID: 7939
		private byte[] block;

		// Token: 0x04001F04 RID: 7940
		private byte[] mBuf;

		// Token: 0x04001F05 RID: 7941
		private int messageLength;

		// Token: 0x04001F06 RID: 7942
		private readonly int saltLength;

		// Token: 0x04001F07 RID: 7943
		private bool fullMessage;

		// Token: 0x04001F08 RID: 7944
		private byte[] recoveredMessage;

		// Token: 0x04001F09 RID: 7945
		private byte[] preSig;

		// Token: 0x04001F0A RID: 7946
		private byte[] preBlock;

		// Token: 0x04001F0B RID: 7947
		private int preMStart;

		// Token: 0x04001F0C RID: 7948
		private int preTLength;
	}
}
