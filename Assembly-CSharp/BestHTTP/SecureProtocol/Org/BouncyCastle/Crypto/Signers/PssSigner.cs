using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A6 RID: 1190
	public class PssSigner : ISigner
	{
		// Token: 0x06002EA0 RID: 11936 RVA: 0x00121F6D File Offset: 0x0012016D
		public static PssSigner CreateRawSigner(IAsymmetricBlockCipher cipher, IDigest digest)
		{
			return new PssSigner(cipher, new NullDigest(), digest, digest, digest.GetDigestSize(), null, 188);
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x00121F88 File Offset: 0x00120188
		public static PssSigner CreateRawSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen, byte trailer)
		{
			return new PssSigner(cipher, new NullDigest(), contentDigest, mgfDigest, saltLen, null, trailer);
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x00121F9B File Offset: 0x0012019B
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, digest.GetDigestSize())
		{
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x00121FAB File Offset: 0x001201AB
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLen) : this(cipher, digest, saltLen, 188)
		{
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x00121FBB File Offset: 0x001201BB
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, byte[] salt) : this(cipher, digest, digest, digest, salt.Length, salt, 188)
		{
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x00121FD0 File Offset: 0x001201D0
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen) : this(cipher, contentDigest, mgfDigest, saltLen, 188)
		{
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x00121FE2 File Offset: 0x001201E2
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, byte[] salt) : this(cipher, contentDigest, contentDigest, mgfDigest, salt.Length, salt, 188)
		{
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x00121FF9 File Offset: 0x001201F9
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest digest, int saltLen, byte trailer) : this(cipher, digest, digest, saltLen, 188)
		{
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x0012200A File Offset: 0x0012020A
		public PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest, IDigest mgfDigest, int saltLen, byte trailer) : this(cipher, contentDigest, contentDigest, mgfDigest, saltLen, null, trailer)
		{
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x0012201C File Offset: 0x0012021C
		private PssSigner(IAsymmetricBlockCipher cipher, IDigest contentDigest1, IDigest contentDigest2, IDigest mgfDigest, int saltLen, byte[] salt, byte trailer)
		{
			this.cipher = cipher;
			this.contentDigest1 = contentDigest1;
			this.contentDigest2 = contentDigest2;
			this.mgfDigest = mgfDigest;
			this.hLen = contentDigest2.GetDigestSize();
			this.mgfhLen = mgfDigest.GetDigestSize();
			this.sLen = saltLen;
			this.sSet = (salt != null);
			if (this.sSet)
			{
				this.salt = salt;
			}
			else
			{
				this.salt = new byte[saltLen];
			}
			this.mDash = new byte[8 + saltLen + this.hLen];
			this.trailer = trailer;
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06002EAA RID: 11946 RVA: 0x001220B5 File Offset: 0x001202B5
		public virtual string AlgorithmName
		{
			get
			{
				return this.mgfDigest.AlgorithmName + "withRSAandMGF1";
			}
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x001220CC File Offset: 0x001202CC
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				parameters = parametersWithRandom.Parameters;
				this.random = parametersWithRandom.Random;
			}
			else if (forSigning)
			{
				this.random = new SecureRandom();
			}
			this.cipher.Init(forSigning, parameters);
			RsaKeyParameters rsaKeyParameters;
			if (parameters is RsaBlindingParameters)
			{
				rsaKeyParameters = ((RsaBlindingParameters)parameters).PublicKey;
			}
			else
			{
				rsaKeyParameters = (RsaKeyParameters)parameters;
			}
			this.emBits = rsaKeyParameters.Modulus.BitLength - 1;
			if (this.emBits < 8 * this.hLen + 8 * this.sLen + 9)
			{
				throw new ArgumentException("key too small for specified hash and salt lengths");
			}
			this.block = new byte[(this.emBits + 7) / 8];
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x00120B89 File Offset: 0x0011ED89
		private void ClearBlock(byte[] block)
		{
			Array.Clear(block, 0, block.Length);
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x00122185 File Offset: 0x00120385
		public virtual void Update(byte input)
		{
			this.contentDigest1.Update(input);
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x00122193 File Offset: 0x00120393
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.contentDigest1.BlockUpdate(input, inOff, length);
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x001221A3 File Offset: 0x001203A3
		public virtual void Reset()
		{
			this.contentDigest1.Reset();
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x001221B0 File Offset: 0x001203B0
		public virtual byte[] GenerateSignature()
		{
			this.contentDigest1.DoFinal(this.mDash, this.mDash.Length - this.hLen - this.sLen);
			if (this.sLen != 0)
			{
				if (!this.sSet)
				{
					this.random.NextBytes(this.salt);
				}
				this.salt.CopyTo(this.mDash, this.mDash.Length - this.sLen);
			}
			byte[] array = new byte[this.hLen];
			this.contentDigest2.BlockUpdate(this.mDash, 0, this.mDash.Length);
			this.contentDigest2.DoFinal(array, 0);
			this.block[this.block.Length - this.sLen - 1 - this.hLen - 1] = 1;
			this.salt.CopyTo(this.block, this.block.Length - this.sLen - this.hLen - 1);
			byte[] array2 = this.MaskGeneratorFunction1(array, 0, array.Length, this.block.Length - this.hLen - 1);
			for (int num = 0; num != array2.Length; num++)
			{
				byte[] array3 = this.block;
				int num2 = num;
				array3[num2] ^= array2[num];
			}
			byte[] array4 = this.block;
			int num3 = 0;
			array4[num3] &= (byte)(255 >> this.block.Length * 8 - this.emBits);
			array.CopyTo(this.block, this.block.Length - this.hLen - 1);
			this.block[this.block.Length - 1] = this.trailer;
			byte[] result = this.cipher.ProcessBlock(this.block, 0, this.block.Length);
			this.ClearBlock(this.block);
			return result;
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x0012236C File Offset: 0x0012056C
		public virtual bool VerifySignature(byte[] signature)
		{
			this.contentDigest1.DoFinal(this.mDash, this.mDash.Length - this.hLen - this.sLen);
			byte[] array = this.cipher.ProcessBlock(signature, 0, signature.Length);
			array.CopyTo(this.block, this.block.Length - array.Length);
			if (this.block[this.block.Length - 1] != this.trailer)
			{
				this.ClearBlock(this.block);
				return false;
			}
			byte[] array2 = this.MaskGeneratorFunction1(this.block, this.block.Length - this.hLen - 1, this.hLen, this.block.Length - this.hLen - 1);
			for (int num = 0; num != array2.Length; num++)
			{
				byte[] array3 = this.block;
				int num2 = num;
				array3[num2] ^= array2[num];
			}
			byte[] array4 = this.block;
			int num3 = 0;
			array4[num3] &= (byte)(255 >> this.block.Length * 8 - this.emBits);
			for (int num4 = 0; num4 != this.block.Length - this.hLen - this.sLen - 2; num4++)
			{
				if (this.block[num4] != 0)
				{
					this.ClearBlock(this.block);
					return false;
				}
			}
			if (this.block[this.block.Length - this.hLen - this.sLen - 2] != 1)
			{
				this.ClearBlock(this.block);
				return false;
			}
			if (this.sSet)
			{
				Array.Copy(this.salt, 0, this.mDash, this.mDash.Length - this.sLen, this.sLen);
			}
			else
			{
				Array.Copy(this.block, this.block.Length - this.sLen - this.hLen - 1, this.mDash, this.mDash.Length - this.sLen, this.sLen);
			}
			this.contentDigest2.BlockUpdate(this.mDash, 0, this.mDash.Length);
			this.contentDigest2.DoFinal(this.mDash, this.mDash.Length - this.hLen);
			int num5 = this.block.Length - this.hLen - 1;
			for (int num6 = this.mDash.Length - this.hLen; num6 != this.mDash.Length; num6++)
			{
				if ((this.block[num5] ^ this.mDash[num6]) != 0)
				{
					this.ClearBlock(this.mDash);
					this.ClearBlock(this.block);
					return false;
				}
				num5++;
			}
			this.ClearBlock(this.mDash);
			this.ClearBlock(this.block);
			return true;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x001212E0 File Offset: 0x0011F4E0
		private void ItoOSP(int i, byte[] sp)
		{
			sp[0] = (byte)((uint)i >> 24);
			sp[1] = (byte)((uint)i >> 16);
			sp[2] = (byte)((uint)i >> 8);
			sp[3] = (byte)i;
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x00122610 File Offset: 0x00120810
		private byte[] MaskGeneratorFunction1(byte[] Z, int zOff, int zLen, int length)
		{
			byte[] array = new byte[length];
			byte[] array2 = new byte[this.mgfhLen];
			byte[] array3 = new byte[4];
			int i = 0;
			this.mgfDigest.Reset();
			while (i < length / this.mgfhLen)
			{
				this.ItoOSP(i, array3);
				this.mgfDigest.BlockUpdate(Z, zOff, zLen);
				this.mgfDigest.BlockUpdate(array3, 0, array3.Length);
				this.mgfDigest.DoFinal(array2, 0);
				array2.CopyTo(array, i * this.mgfhLen);
				i++;
			}
			if (i * this.mgfhLen < length)
			{
				this.ItoOSP(i, array3);
				this.mgfDigest.BlockUpdate(Z, zOff, zLen);
				this.mgfDigest.BlockUpdate(array3, 0, array3.Length);
				this.mgfDigest.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, i * this.mgfhLen, array.Length - i * this.mgfhLen);
			}
			return array;
		}

		// Token: 0x04001F2D RID: 7981
		public const byte TrailerImplicit = 188;

		// Token: 0x04001F2E RID: 7982
		private readonly IDigest contentDigest1;

		// Token: 0x04001F2F RID: 7983
		private readonly IDigest contentDigest2;

		// Token: 0x04001F30 RID: 7984
		private readonly IDigest mgfDigest;

		// Token: 0x04001F31 RID: 7985
		private readonly IAsymmetricBlockCipher cipher;

		// Token: 0x04001F32 RID: 7986
		private SecureRandom random;

		// Token: 0x04001F33 RID: 7987
		private int hLen;

		// Token: 0x04001F34 RID: 7988
		private int mgfhLen;

		// Token: 0x04001F35 RID: 7989
		private int sLen;

		// Token: 0x04001F36 RID: 7990
		private bool sSet;

		// Token: 0x04001F37 RID: 7991
		private int emBits;

		// Token: 0x04001F38 RID: 7992
		private byte[] salt;

		// Token: 0x04001F39 RID: 7993
		private byte[] mDash;

		// Token: 0x04001F3A RID: 7994
		private byte[] block;

		// Token: 0x04001F3B RID: 7995
		private byte trailer;
	}
}
