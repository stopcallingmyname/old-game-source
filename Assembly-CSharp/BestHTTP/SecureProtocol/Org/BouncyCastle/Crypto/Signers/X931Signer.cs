using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004AB RID: 1195
	public class X931Signer : ISigner
	{
		// Token: 0x06002EDE RID: 11998 RVA: 0x00123164 File Offset: 0x00121364
		public X931Signer(IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit)
		{
			this.cipher = cipher;
			this.digest = digest;
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

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002EDF RID: 11999 RVA: 0x001231B8 File Offset: 0x001213B8
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "with" + this.cipher.AlgorithmName + "/X9.31";
			}
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x001231DF File Offset: 0x001213DF
		public X931Signer(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, false)
		{
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x001231EC File Offset: 0x001213EC
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.kParam = (RsaKeyParameters)parameters;
			this.cipher.Init(forSigning, this.kParam);
			this.keyBits = this.kParam.Modulus.BitLength;
			this.block = new byte[(this.keyBits + 7) / 8];
			this.Reset();
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x00120B89 File Offset: 0x0011ED89
		private void ClearBlock(byte[] block)
		{
			Array.Clear(block, 0, block.Length);
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x00123248 File Offset: 0x00121448
		public virtual void Update(byte b)
		{
			this.digest.Update(b);
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x00123256 File Offset: 0x00121456
		public virtual void BlockUpdate(byte[] input, int off, int len)
		{
			this.digest.BlockUpdate(input, off, len);
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x00123266 File Offset: 0x00121466
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x00123274 File Offset: 0x00121474
		public virtual byte[] GenerateSignature()
		{
			this.CreateSignatureBlock();
			BigInteger bigInteger = new BigInteger(1, this.cipher.ProcessBlock(this.block, 0, this.block.Length));
			this.ClearBlock(this.block);
			bigInteger = bigInteger.Min(this.kParam.Modulus.Subtract(bigInteger));
			return BigIntegers.AsUnsignedByteArray(BigIntegers.GetUnsignedByteLength(this.kParam.Modulus), bigInteger);
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x001232E4 File Offset: 0x001214E4
		private void CreateSignatureBlock()
		{
			int digestSize = this.digest.GetDigestSize();
			int num;
			if (this.trailer == 188)
			{
				num = this.block.Length - digestSize - 1;
				this.digest.DoFinal(this.block, num);
				this.block[this.block.Length - 1] = 188;
			}
			else
			{
				num = this.block.Length - digestSize - 2;
				this.digest.DoFinal(this.block, num);
				this.block[this.block.Length - 2] = (byte)(this.trailer >> 8);
				this.block[this.block.Length - 1] = (byte)this.trailer;
			}
			this.block[0] = 107;
			for (int num2 = num - 2; num2 != 0; num2--)
			{
				this.block[num2] = 187;
			}
			this.block[num - 1] = 186;
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x001233C8 File Offset: 0x001215C8
		public virtual bool VerifySignature(byte[] signature)
		{
			try
			{
				this.block = this.cipher.ProcessBlock(signature, 0, signature.Length);
			}
			catch (Exception)
			{
				return false;
			}
			BigInteger bigInteger = new BigInteger(1, this.block);
			BigInteger n;
			if ((bigInteger.IntValue & 15) == 12)
			{
				n = bigInteger;
			}
			else
			{
				bigInteger = this.kParam.Modulus.Subtract(bigInteger);
				if ((bigInteger.IntValue & 15) != 12)
				{
					return false;
				}
				n = bigInteger;
			}
			this.CreateSignatureBlock();
			byte[] b = BigIntegers.AsUnsignedByteArray(this.block.Length, n);
			bool result = Arrays.ConstantTimeAreEqual(this.block, b);
			this.ClearBlock(this.block);
			this.ClearBlock(b);
			return result;
		}

		// Token: 0x04001F4B RID: 8011
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_IMPLICIT = 188;

		// Token: 0x04001F4C RID: 8012
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_RIPEMD160 = 12748;

		// Token: 0x04001F4D RID: 8013
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_RIPEMD128 = 13004;

		// Token: 0x04001F4E RID: 8014
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA1 = 13260;

		// Token: 0x04001F4F RID: 8015
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA256 = 13516;

		// Token: 0x04001F50 RID: 8016
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA512 = 13772;

		// Token: 0x04001F51 RID: 8017
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA384 = 14028;

		// Token: 0x04001F52 RID: 8018
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_WHIRLPOOL = 14284;

		// Token: 0x04001F53 RID: 8019
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TRAILER_SHA224 = 14540;

		// Token: 0x04001F54 RID: 8020
		private IDigest digest;

		// Token: 0x04001F55 RID: 8021
		private IAsymmetricBlockCipher cipher;

		// Token: 0x04001F56 RID: 8022
		private RsaKeyParameters kParam;

		// Token: 0x04001F57 RID: 8023
		private int trailer;

		// Token: 0x04001F58 RID: 8024
		private int keyBits;

		// Token: 0x04001F59 RID: 8025
		private byte[] block;
	}
}
