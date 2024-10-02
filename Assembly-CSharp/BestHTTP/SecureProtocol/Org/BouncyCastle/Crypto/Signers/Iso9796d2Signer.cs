using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A3 RID: 1187
	public class Iso9796d2Signer : ISignerWithRecovery, ISigner
	{
		// Token: 0x06002E85 RID: 11909 RVA: 0x00121428 File Offset: 0x0011F628
		public byte[] GetRecoveredMessage()
		{
			return this.recoveredMessage;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x00121430 File Offset: 0x0011F630
		public Iso9796d2Signer(IAsymmetricBlockCipher cipher, IDigest digest, bool isImplicit)
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

		// Token: 0x06002E87 RID: 11911 RVA: 0x00121484 File Offset: 0x0011F684
		public Iso9796d2Signer(IAsymmetricBlockCipher cipher, IDigest digest) : this(cipher, digest, false)
		{
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06002E88 RID: 11912 RVA: 0x0012148F File Offset: 0x0011F68F
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "withISO9796-2S1";
			}
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x001214A8 File Offset: 0x0011F6A8
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)parameters;
			this.cipher.Init(forSigning, rsaKeyParameters);
			this.keyBits = rsaKeyParameters.Modulus.BitLength;
			this.block = new byte[(this.keyBits + 7) / 8];
			if (this.trailer == 188)
			{
				this.mBuf = new byte[this.block.Length - this.digest.GetDigestSize() - 2];
			}
			else
			{
				this.mBuf = new byte[this.block.Length - this.digest.GetDigestSize() - 3];
			}
			this.Reset();
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x00121548 File Offset: 0x0011F748
		private bool IsSameAs(byte[] a, byte[] b)
		{
			int num;
			if (this.messageLength > this.mBuf.Length)
			{
				if (this.mBuf.Length > b.Length)
				{
					return false;
				}
				num = this.mBuf.Length;
			}
			else
			{
				if (this.messageLength != b.Length)
				{
					return false;
				}
				num = b.Length;
			}
			bool result = true;
			for (int num2 = 0; num2 != num; num2++)
			{
				if (a[num2] != b[num2])
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x00120B89 File Offset: 0x0011ED89
		private void ClearBlock(byte[] block)
		{
			Array.Clear(block, 0, block.Length);
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x001215AC File Offset: 0x0011F7AC
		public virtual void UpdateWithRecoveredMessage(byte[] signature)
		{
			byte[] array = this.cipher.ProcessBlock(signature, 0, signature.Length);
			if (((array[0] & 192) ^ 64) != 0)
			{
				throw new InvalidCipherTextException("malformed signature");
			}
			if (((array[array.Length - 1] & 15) ^ 12) != 0)
			{
				throw new InvalidCipherTextException("malformed signature");
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
			int num3 = 0;
			while (num3 != array.Length && ((array[num3] & 15) ^ 10) != 0)
			{
				num3++;
			}
			num3++;
			int num4 = array.Length - num - this.digest.GetDigestSize();
			if (num4 - num3 <= 0)
			{
				throw new InvalidCipherTextException("malformed block");
			}
			if ((array[0] & 32) == 0)
			{
				this.fullMessage = true;
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			else
			{
				this.fullMessage = false;
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			this.preSig = signature;
			this.preBlock = array;
			this.digest.BlockUpdate(this.recoveredMessage, 0, this.recoveredMessage.Length);
			this.messageLength = this.recoveredMessage.Length;
			this.recoveredMessage.CopyTo(this.mBuf, 0);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x00121761 File Offset: 0x0011F961
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
			if (this.messageLength < this.mBuf.Length)
			{
				this.mBuf[this.messageLength] = input;
			}
			this.messageLength++;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x0012179C File Offset: 0x0011F99C
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (length > 0 && this.messageLength < this.mBuf.Length)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
			this.digest.BlockUpdate(input, inOff, length);
			this.messageLength += length;
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x001217F0 File Offset: 0x0011F9F0
		public virtual void Reset()
		{
			this.digest.Reset();
			this.messageLength = 0;
			this.ClearBlock(this.mBuf);
			if (this.recoveredMessage != null)
			{
				this.ClearBlock(this.recoveredMessage);
			}
			this.recoveredMessage = null;
			this.fullMessage = false;
			if (this.preSig != null)
			{
				this.preSig = null;
				this.ClearBlock(this.preBlock);
				this.preBlock = null;
			}
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x00121860 File Offset: 0x0011FA60
		public virtual byte[] GenerateSignature()
		{
			int digestSize = this.digest.GetDigestSize();
			int num;
			int num2;
			if (this.trailer == 188)
			{
				num = 8;
				num2 = this.block.Length - digestSize - 1;
				this.digest.DoFinal(this.block, num2);
				this.block[this.block.Length - 1] = 188;
			}
			else
			{
				num = 16;
				num2 = this.block.Length - digestSize - 2;
				this.digest.DoFinal(this.block, num2);
				this.block[this.block.Length - 2] = (byte)((uint)this.trailer >> 8);
				this.block[this.block.Length - 1] = (byte)this.trailer;
			}
			int num3 = (digestSize + this.messageLength) * 8 + num + 4 - this.keyBits;
			byte b;
			if (num3 > 0)
			{
				int num4 = this.messageLength - (num3 + 7) / 8;
				b = 96;
				num2 -= num4;
				Array.Copy(this.mBuf, 0, this.block, num2, num4);
			}
			else
			{
				b = 64;
				num2 -= this.messageLength;
				Array.Copy(this.mBuf, 0, this.block, num2, this.messageLength);
			}
			if (num2 - 1 > 0)
			{
				for (int num5 = num2 - 1; num5 != 0; num5--)
				{
					this.block[num5] = 187;
				}
				byte[] array = this.block;
				int num6 = num2 - 1;
				array[num6] ^= 1;
				this.block[0] = 11;
				byte[] array2 = this.block;
				int num7 = 0;
				array2[num7] |= b;
			}
			else
			{
				this.block[0] = 10;
				byte[] array3 = this.block;
				int num8 = 0;
				array3[num8] |= b;
			}
			byte[] result = this.cipher.ProcessBlock(this.block, 0, this.block.Length);
			this.messageLength = 0;
			this.ClearBlock(this.mBuf);
			this.ClearBlock(this.block);
			return result;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x00121A38 File Offset: 0x0011FC38
		public virtual bool VerifySignature(byte[] signature)
		{
			byte[] array;
			if (this.preSig == null)
			{
				try
				{
					array = this.cipher.ProcessBlock(signature, 0, signature.Length);
					goto IL_52;
				}
				catch (Exception)
				{
					return false;
				}
			}
			if (!Arrays.AreEqual(this.preSig, signature))
			{
				throw new InvalidOperationException("updateWithRecoveredMessage called on different signature");
			}
			array = this.preBlock;
			this.preSig = null;
			this.preBlock = null;
			IL_52:
			if (((array[0] & 192) ^ 64) != 0)
			{
				return this.ReturnFalse(array);
			}
			if (((array[array.Length - 1] & 15) ^ 12) != 0)
			{
				return this.ReturnFalse(array);
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
			int num3 = 0;
			while (num3 != array.Length && ((array[num3] & 15) ^ 10) != 0)
			{
				num3++;
			}
			num3++;
			byte[] array2 = new byte[this.digest.GetDigestSize()];
			int num4 = array.Length - num - array2.Length;
			if (num4 - num3 <= 0)
			{
				return this.ReturnFalse(array);
			}
			if ((array[0] & 32) == 0)
			{
				this.fullMessage = true;
				if (this.messageLength > num4 - num3)
				{
					return this.ReturnFalse(array);
				}
				this.digest.Reset();
				this.digest.BlockUpdate(array, num3, num4 - num3);
				this.digest.DoFinal(array2, 0);
				bool flag = true;
				for (int num5 = 0; num5 != array2.Length; num5++)
				{
					byte[] array3 = array;
					int num6 = num4 + num5;
					array3[num6] ^= array2[num5];
					if (array[num4 + num5] != 0)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					return this.ReturnFalse(array);
				}
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			else
			{
				this.fullMessage = false;
				this.digest.DoFinal(array2, 0);
				bool flag2 = true;
				for (int num7 = 0; num7 != array2.Length; num7++)
				{
					byte[] array4 = array;
					int num8 = num4 + num7;
					array4[num8] ^= array2[num7];
					if (array[num4 + num7] != 0)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					return this.ReturnFalse(array);
				}
				this.recoveredMessage = new byte[num4 - num3];
				Array.Copy(array, num3, this.recoveredMessage, 0, this.recoveredMessage.Length);
			}
			if (this.messageLength != 0 && !this.IsSameAs(this.mBuf, this.recoveredMessage))
			{
				return this.ReturnFalse(array);
			}
			this.ClearBlock(this.mBuf);
			this.ClearBlock(array);
			this.messageLength = 0;
			return true;
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x00121D0C File Offset: 0x0011FF0C
		private bool ReturnFalse(byte[] block)
		{
			this.messageLength = 0;
			this.ClearBlock(this.mBuf);
			this.ClearBlock(block);
			return false;
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x00121D29 File Offset: 0x0011FF29
		public virtual bool HasFullMessage()
		{
			return this.fullMessage;
		}

		// Token: 0x04001F0D RID: 7949
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerImplicit = 188;

		// Token: 0x04001F0E RID: 7950
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerRipeMD160 = 12748;

		// Token: 0x04001F0F RID: 7951
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerRipeMD128 = 13004;

		// Token: 0x04001F10 RID: 7952
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha1 = 13260;

		// Token: 0x04001F11 RID: 7953
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha256 = 13516;

		// Token: 0x04001F12 RID: 7954
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha512 = 13772;

		// Token: 0x04001F13 RID: 7955
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerSha384 = 14028;

		// Token: 0x04001F14 RID: 7956
		[Obsolete("Use 'IsoTrailers' instead")]
		public const int TrailerWhirlpool = 14284;

		// Token: 0x04001F15 RID: 7957
		private IDigest digest;

		// Token: 0x04001F16 RID: 7958
		private IAsymmetricBlockCipher cipher;

		// Token: 0x04001F17 RID: 7959
		private int trailer;

		// Token: 0x04001F18 RID: 7960
		private int keyBits;

		// Token: 0x04001F19 RID: 7961
		private byte[] block;

		// Token: 0x04001F1A RID: 7962
		private byte[] mBuf;

		// Token: 0x04001F1B RID: 7963
		private int messageLength;

		// Token: 0x04001F1C RID: 7964
		private bool fullMessage;

		// Token: 0x04001F1D RID: 7965
		private byte[] recoveredMessage;

		// Token: 0x04001F1E RID: 7966
		private byte[] preSig;

		// Token: 0x04001F1F RID: 7967
		private byte[] preBlock;
	}
}
