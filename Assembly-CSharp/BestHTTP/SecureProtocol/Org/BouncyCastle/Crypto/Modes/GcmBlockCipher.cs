using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200051E RID: 1310
	public sealed class GcmBlockCipher : IAeadBlockCipher
	{
		// Token: 0x06003177 RID: 12663 RVA: 0x0012ACA1 File Offset: 0x00128EA1
		public GcmBlockCipher(IBlockCipher c) : this(c, null)
		{
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x0012ACAC File Offset: 0x00128EAC
		public GcmBlockCipher(IBlockCipher c, IGcmMultiplier m)
		{
			if (c.GetBlockSize() != 16)
			{
				throw new ArgumentException("cipher required with a block size of " + 16 + ".");
			}
			if (m == null)
			{
				m = new Tables8kGcmMultiplier();
			}
			this.cipher = c;
			this.multiplier = m;
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x0012AD0A File Offset: 0x00128F0A
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/GCM";
			}
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x0012AD21 File Offset: 0x00128F21
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x0012AD29 File Offset: 0x00128F29
		public int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x0012AD30 File Offset: 0x00128F30
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			this.macBlock = null;
			this.initialised = true;
			byte[] iv;
			KeyParameter keyParameter;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				iv = aeadParameters.GetNonce();
				this.initialAssociatedText = aeadParameters.GetAssociatedText();
				int num = aeadParameters.MacSize;
				if (num < 32 || num > 128 || num % 8 != 0)
				{
					throw new ArgumentException("Invalid value for MAC size: " + num);
				}
				this.macSize = num / 8;
				keyParameter = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("invalid parameters passed to GCM");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				iv = parametersWithIV.GetIV();
				this.initialAssociatedText = null;
				this.macSize = 16;
				keyParameter = (KeyParameter)parametersWithIV.Parameters;
			}
			int num2 = forEncryption ? 16 : (16 + this.macSize);
			this.bufBlock = new byte[num2];
			if (iv == null || iv.Length < 1)
			{
				throw new ArgumentException("IV must be at least 1 byte");
			}
			if (forEncryption && this.nonce != null && Arrays.AreEqual(this.nonce, iv))
			{
				if (keyParameter == null)
				{
					throw new ArgumentException("cannot reuse nonce for GCM encryption");
				}
				if (this.lastKey != null && Arrays.AreEqual(this.lastKey, keyParameter.GetKey()))
				{
					throw new ArgumentException("cannot reuse nonce for GCM encryption");
				}
			}
			this.nonce = iv;
			if (keyParameter != null)
			{
				this.lastKey = keyParameter.GetKey();
			}
			if (keyParameter != null)
			{
				this.cipher.Init(true, keyParameter);
				this.H = new byte[16];
				this.cipher.ProcessBlock(this.H, 0, this.H, 0);
				this.multiplier.Init(this.H);
				this.exp = null;
			}
			else if (this.H == null)
			{
				throw new ArgumentException("Key must be specified in initial init");
			}
			this.J0 = new byte[16];
			if (this.nonce.Length == 12)
			{
				Array.Copy(this.nonce, 0, this.J0, 0, this.nonce.Length);
				this.J0[15] = 1;
			}
			else
			{
				this.gHASH(this.J0, this.nonce, this.nonce.Length);
				byte[] array = new byte[16];
				Pack.UInt64_To_BE((ulong)((long)this.nonce.Length * 8L), array, 8);
				this.gHASHBlock(this.J0, array);
			}
			this.S = new byte[16];
			this.S_at = new byte[16];
			this.S_atPre = new byte[16];
			this.atBlock = new byte[16];
			this.atBlockPos = 0;
			this.atLength = 0UL;
			this.atLengthPre = 0UL;
			this.counter = Arrays.Clone(this.J0);
			this.blocksRemaining = 4294967294U;
			this.bufOff = 0;
			this.totalLength = 0UL;
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x0012B006 File Offset: 0x00129206
		public byte[] GetMac()
		{
			if (this.macBlock != null)
			{
				return Arrays.Clone(this.macBlock);
			}
			return new byte[this.macSize];
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x0012B028 File Offset: 0x00129228
		public int GetOutputSize(int len)
		{
			int num = len + this.bufOff;
			if (this.forEncryption)
			{
				return num + this.macSize;
			}
			if (num >= this.macSize)
			{
				return num - this.macSize;
			}
			return 0;
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x0012B064 File Offset: 0x00129264
		public int GetUpdateOutputSize(int len)
		{
			int num = len + this.bufOff;
			if (!this.forEncryption)
			{
				if (num < this.macSize)
				{
					return 0;
				}
				num -= this.macSize;
			}
			return num - num % 16;
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x0012B09C File Offset: 0x0012929C
		public void ProcessAadByte(byte input)
		{
			this.CheckStatus();
			this.atBlock[this.atBlockPos] = input;
			int num = this.atBlockPos + 1;
			this.atBlockPos = num;
			if (num == 16)
			{
				this.gHASHBlock(this.S_at, this.atBlock);
				this.atBlockPos = 0;
				this.atLength += 16UL;
			}
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x0012B0FC File Offset: 0x001292FC
		public void ProcessAadBytes(byte[] inBytes, int inOff, int len)
		{
			this.CheckStatus();
			for (int i = 0; i < len; i++)
			{
				this.atBlock[this.atBlockPos] = inBytes[inOff + i];
				int num = this.atBlockPos + 1;
				this.atBlockPos = num;
				if (num == 16)
				{
					this.gHASHBlock(this.S_at, this.atBlock);
					this.atBlockPos = 0;
					this.atLength += 16UL;
				}
			}
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x0012B16C File Offset: 0x0012936C
		private void InitCipher()
		{
			if (this.atLength > 0UL)
			{
				Array.Copy(this.S_at, 0, this.S_atPre, 0, 16);
				this.atLengthPre = this.atLength;
			}
			if (this.atBlockPos > 0)
			{
				this.gHASHPartial(this.S_atPre, this.atBlock, 0, this.atBlockPos);
				this.atLengthPre += (ulong)this.atBlockPos;
			}
			if (this.atLengthPre > 0UL)
			{
				Array.Copy(this.S_atPre, 0, this.S, 0, 16);
			}
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x0012B1FC File Offset: 0x001293FC
		public int ProcessByte(byte input, byte[] output, int outOff)
		{
			this.CheckStatus();
			this.bufBlock[this.bufOff] = input;
			int num = this.bufOff + 1;
			this.bufOff = num;
			if (num == this.bufBlock.Length)
			{
				this.ProcessBlock(this.bufBlock, 0, output, outOff);
				if (this.forEncryption)
				{
					this.bufOff = 0;
				}
				else
				{
					Array.Copy(this.bufBlock, 16, this.bufBlock, 0, this.macSize);
					this.bufOff = this.macSize;
				}
				return 16;
			}
			return 0;
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x0012B284 File Offset: 0x00129484
		public int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			this.CheckStatus();
			Check.DataLength(input, inOff, len, "input buffer too short");
			int num = 0;
			if (this.forEncryption)
			{
				if (this.bufOff != 0)
				{
					while (len > 0)
					{
						len--;
						this.bufBlock[this.bufOff] = input[inOff++];
						int num2 = this.bufOff + 1;
						this.bufOff = num2;
						if (num2 == 16)
						{
							this.ProcessBlock(this.bufBlock, 0, output, outOff);
							this.bufOff = 0;
							num += 16;
							break;
						}
					}
				}
				while (len >= 16)
				{
					this.ProcessBlock(input, inOff, output, outOff + num);
					inOff += 16;
					len -= 16;
					num += 16;
				}
				if (len > 0)
				{
					Array.Copy(input, inOff, this.bufBlock, 0, len);
					this.bufOff = len;
				}
			}
			else
			{
				for (int i = 0; i < len; i++)
				{
					this.bufBlock[this.bufOff] = input[inOff + i];
					int num2 = this.bufOff + 1;
					this.bufOff = num2;
					if (num2 == this.bufBlock.Length)
					{
						this.ProcessBlock(this.bufBlock, 0, output, outOff + num);
						Array.Copy(this.bufBlock, 16, this.bufBlock, 0, this.macSize);
						this.bufOff = this.macSize;
						num += 16;
					}
				}
			}
			return num;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x0012B3CC File Offset: 0x001295CC
		public int DoFinal(byte[] output, int outOff)
		{
			this.CheckStatus();
			if (this.totalLength == 0UL)
			{
				this.InitCipher();
			}
			int num = this.bufOff;
			if (this.forEncryption)
			{
				Check.OutputLength(output, outOff, num + this.macSize, "Output buffer too short");
			}
			else
			{
				if (num < this.macSize)
				{
					throw new InvalidCipherTextException("data too short");
				}
				num -= this.macSize;
				Check.OutputLength(output, outOff, num, "Output buffer too short");
			}
			if (num > 0)
			{
				this.ProcessPartial(this.bufBlock, 0, num, output, outOff);
			}
			this.atLength += (ulong)this.atBlockPos;
			if (this.atLength > this.atLengthPre)
			{
				if (this.atBlockPos > 0)
				{
					this.gHASHPartial(this.S_at, this.atBlock, 0, this.atBlockPos);
				}
				if (this.atLengthPre > 0UL)
				{
					GcmUtilities.Xor(this.S_at, this.S_atPre);
				}
				long pow = (long)(this.totalLength * 8UL + 127UL >> 7);
				byte[] array = new byte[16];
				if (this.exp == null)
				{
					this.exp = new Tables1kGcmExponentiator();
					this.exp.Init(this.H);
				}
				this.exp.ExponentiateX(pow, array);
				GcmUtilities.Multiply(this.S_at, array);
				GcmUtilities.Xor(this.S, this.S_at);
			}
			byte[] array2 = new byte[16];
			Pack.UInt64_To_BE(this.atLength * 8UL, array2, 0);
			Pack.UInt64_To_BE(this.totalLength * 8UL, array2, 8);
			this.gHASHBlock(this.S, array2);
			byte[] array3 = new byte[16];
			this.cipher.ProcessBlock(this.J0, 0, array3, 0);
			GcmUtilities.Xor(array3, this.S);
			int num2 = num;
			this.macBlock = new byte[this.macSize];
			Array.Copy(array3, 0, this.macBlock, 0, this.macSize);
			if (this.forEncryption)
			{
				Array.Copy(this.macBlock, 0, output, outOff + this.bufOff, this.macSize);
				num2 += this.macSize;
			}
			else
			{
				byte[] array4 = new byte[this.macSize];
				Array.Copy(this.bufBlock, num, array4, 0, this.macSize);
				if (!Arrays.ConstantTimeAreEqual(this.macBlock, array4))
				{
					throw new InvalidCipherTextException("mac check in GCM failed");
				}
			}
			this.Reset(false);
			return num2;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x0012B614 File Offset: 0x00129814
		public void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x0012B620 File Offset: 0x00129820
		private void Reset(bool clearMac)
		{
			this.cipher.Reset();
			this.S = new byte[16];
			this.S_at = new byte[16];
			this.S_atPre = new byte[16];
			this.atBlock = new byte[16];
			this.atBlockPos = 0;
			this.atLength = 0UL;
			this.atLengthPre = 0UL;
			this.counter = Arrays.Clone(this.J0);
			this.blocksRemaining = 4294967294U;
			this.bufOff = 0;
			this.totalLength = 0UL;
			if (this.bufBlock != null)
			{
				Arrays.Fill(this.bufBlock, 0);
			}
			if (clearMac)
			{
				this.macBlock = null;
			}
			if (this.forEncryption)
			{
				this.initialised = false;
				return;
			}
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x0012B6F8 File Offset: 0x001298F8
		private void ProcessBlock(byte[] buf, int bufOff, byte[] output, int outOff)
		{
			Check.OutputLength(output, outOff, 16, "Output buffer too short");
			if (this.totalLength == 0UL)
			{
				this.InitCipher();
			}
			this.GetNextCtrBlock(this.ctrBlock);
			if (this.forEncryption)
			{
				GcmUtilities.Xor(this.ctrBlock, buf, bufOff);
				this.gHASHBlock(this.S, this.ctrBlock);
				Array.Copy(this.ctrBlock, 0, output, outOff, 16);
			}
			else
			{
				this.gHASHBlock(this.S, buf, bufOff);
				GcmUtilities.Xor(this.ctrBlock, 0, buf, bufOff, output, outOff);
			}
			this.totalLength += 16UL;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x0012B798 File Offset: 0x00129998
		private void ProcessPartial(byte[] buf, int off, int len, byte[] output, int outOff)
		{
			this.GetNextCtrBlock(this.ctrBlock);
			if (this.forEncryption)
			{
				GcmUtilities.Xor(buf, off, this.ctrBlock, 0, len);
				this.gHASHPartial(this.S, buf, off, len);
			}
			else
			{
				this.gHASHPartial(this.S, buf, off, len);
				GcmUtilities.Xor(buf, off, this.ctrBlock, 0, len);
			}
			Array.Copy(buf, off, output, outOff, len);
			this.totalLength += (ulong)len;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x0012B814 File Offset: 0x00129A14
		private void gHASH(byte[] Y, byte[] b, int len)
		{
			for (int i = 0; i < len; i += 16)
			{
				int len2 = Math.Min(len - i, 16);
				this.gHASHPartial(Y, b, i, len2);
			}
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x0012B843 File Offset: 0x00129A43
		private void gHASHBlock(byte[] Y, byte[] b)
		{
			GcmUtilities.Xor(Y, b);
			this.multiplier.MultiplyH(Y);
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x0012B858 File Offset: 0x00129A58
		private void gHASHBlock(byte[] Y, byte[] b, int off)
		{
			GcmUtilities.Xor(Y, b, off);
			this.multiplier.MultiplyH(Y);
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x0012B86E File Offset: 0x00129A6E
		private void gHASHPartial(byte[] Y, byte[] b, int off, int len)
		{
			GcmUtilities.Xor(Y, b, off, len);
			this.multiplier.MultiplyH(Y);
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x0012B888 File Offset: 0x00129A88
		private void GetNextCtrBlock(byte[] block)
		{
			if (this.blocksRemaining == 0U)
			{
				throw new InvalidOperationException("Attempt to process too many blocks");
			}
			this.blocksRemaining -= 1U;
			uint num = 1U;
			num += (uint)this.counter[15];
			this.counter[15] = (byte)num;
			num >>= 8;
			num += (uint)this.counter[14];
			this.counter[14] = (byte)num;
			num >>= 8;
			num += (uint)this.counter[13];
			this.counter[13] = (byte)num;
			num >>= 8;
			num += (uint)this.counter[12];
			this.counter[12] = (byte)num;
			this.cipher.ProcessBlock(this.counter, 0, block, 0);
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x0012B935 File Offset: 0x00129B35
		private void CheckStatus()
		{
			if (this.initialised)
			{
				return;
			}
			if (this.forEncryption)
			{
				throw new InvalidOperationException("GCM cipher cannot be reused for encryption");
			}
			throw new InvalidOperationException("GCM cipher needs to be initialised");
		}

		// Token: 0x04002083 RID: 8323
		private const int BlockSize = 16;

		// Token: 0x04002084 RID: 8324
		private byte[] ctrBlock = new byte[16];

		// Token: 0x04002085 RID: 8325
		private readonly IBlockCipher cipher;

		// Token: 0x04002086 RID: 8326
		private readonly IGcmMultiplier multiplier;

		// Token: 0x04002087 RID: 8327
		private IGcmExponentiator exp;

		// Token: 0x04002088 RID: 8328
		private bool forEncryption;

		// Token: 0x04002089 RID: 8329
		private bool initialised;

		// Token: 0x0400208A RID: 8330
		private int macSize;

		// Token: 0x0400208B RID: 8331
		private byte[] lastKey;

		// Token: 0x0400208C RID: 8332
		private byte[] nonce;

		// Token: 0x0400208D RID: 8333
		private byte[] initialAssociatedText;

		// Token: 0x0400208E RID: 8334
		private byte[] H;

		// Token: 0x0400208F RID: 8335
		private byte[] J0;

		// Token: 0x04002090 RID: 8336
		private byte[] bufBlock;

		// Token: 0x04002091 RID: 8337
		private byte[] macBlock;

		// Token: 0x04002092 RID: 8338
		private byte[] S;

		// Token: 0x04002093 RID: 8339
		private byte[] S_at;

		// Token: 0x04002094 RID: 8340
		private byte[] S_atPre;

		// Token: 0x04002095 RID: 8341
		private byte[] counter;

		// Token: 0x04002096 RID: 8342
		private uint blocksRemaining;

		// Token: 0x04002097 RID: 8343
		private int bufOff;

		// Token: 0x04002098 RID: 8344
		private ulong totalLength;

		// Token: 0x04002099 RID: 8345
		private byte[] atBlock;

		// Token: 0x0400209A RID: 8346
		private int atBlockPos;

		// Token: 0x0400209B RID: 8347
		private ulong atLength;

		// Token: 0x0400209C RID: 8348
		private ulong atLengthPre;
	}
}
