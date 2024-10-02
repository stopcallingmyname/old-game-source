﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200051D RID: 1309
	public class EaxBlockCipher : IAeadBlockCipher
	{
		// Token: 0x06003164 RID: 12644 RVA: 0x0012A5CC File Offset: 0x001287CC
		public EaxBlockCipher(IBlockCipher cipher)
		{
			this.blockSize = cipher.GetBlockSize();
			this.mac = new CMac(cipher);
			this.macBlock = new byte[this.blockSize];
			this.associatedTextMac = new byte[this.mac.GetMacSize()];
			this.nonceMac = new byte[this.mac.GetMacSize()];
			this.cipher = new SicBlockCipher(cipher);
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x0012A640 File Offset: 0x00128840
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.GetUnderlyingCipher().AlgorithmName + "/EAX";
			}
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x0012A65C File Offset: 0x0012885C
		public virtual IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x0012A664 File Offset: 0x00128864
		public virtual int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x0012A674 File Offset: 0x00128874
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			byte[] array;
			ICipherParameters parameters2;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				array = aeadParameters.GetNonce();
				this.initialAssociatedText = aeadParameters.GetAssociatedText();
				this.macSize = aeadParameters.MacSize / 8;
				parameters2 = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("invalid parameters passed to EAX");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				array = parametersWithIV.GetIV();
				this.initialAssociatedText = null;
				this.macSize = this.mac.GetMacSize() / 2;
				parameters2 = parametersWithIV.Parameters;
			}
			this.bufBlock = new byte[forEncryption ? this.blockSize : (this.blockSize + this.macSize)];
			byte[] array2 = new byte[this.blockSize];
			this.mac.Init(parameters2);
			array2[this.blockSize - 1] = 0;
			this.mac.BlockUpdate(array2, 0, this.blockSize);
			this.mac.BlockUpdate(array, 0, array.Length);
			this.mac.DoFinal(this.nonceMac, 0);
			this.cipher.Init(true, new ParametersWithIV(null, this.nonceMac));
			this.Reset();
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x0012A7A0 File Offset: 0x001289A0
		private void InitCipher()
		{
			if (this.cipherInitialized)
			{
				return;
			}
			this.cipherInitialized = true;
			this.mac.DoFinal(this.associatedTextMac, 0);
			byte[] array = new byte[this.blockSize];
			array[this.blockSize - 1] = 2;
			this.mac.BlockUpdate(array, 0, this.blockSize);
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x0012A7FC File Offset: 0x001289FC
		private void CalculateMac()
		{
			byte[] array = new byte[this.blockSize];
			this.mac.DoFinal(array, 0);
			for (int i = 0; i < this.macBlock.Length; i++)
			{
				this.macBlock[i] = (this.nonceMac[i] ^ this.associatedTextMac[i] ^ array[i]);
			}
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x0012A854 File Offset: 0x00128A54
		public virtual void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x0012A860 File Offset: 0x00128A60
		private void Reset(bool clearMac)
		{
			this.cipher.Reset();
			this.mac.Reset();
			this.bufOff = 0;
			Array.Clear(this.bufBlock, 0, this.bufBlock.Length);
			if (clearMac)
			{
				Array.Clear(this.macBlock, 0, this.macBlock.Length);
			}
			byte[] array = new byte[this.blockSize];
			array[this.blockSize - 1] = 1;
			this.mac.BlockUpdate(array, 0, this.blockSize);
			this.cipherInitialized = false;
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x0012A903 File Offset: 0x00128B03
		public virtual void ProcessAadByte(byte input)
		{
			if (this.cipherInitialized)
			{
				throw new InvalidOperationException("AAD data cannot be added after encryption/decryption processing has begun.");
			}
			this.mac.Update(input);
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x0012A924 File Offset: 0x00128B24
		public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
		{
			if (this.cipherInitialized)
			{
				throw new InvalidOperationException("AAD data cannot be added after encryption/decryption processing has begun.");
			}
			this.mac.BlockUpdate(inBytes, inOff, len);
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x0012A947 File Offset: 0x00128B47
		public virtual int ProcessByte(byte input, byte[] outBytes, int outOff)
		{
			this.InitCipher();
			return this.Process(input, outBytes, outOff);
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x0012A958 File Offset: 0x00128B58
		public virtual int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff)
		{
			this.InitCipher();
			int num = 0;
			for (int num2 = 0; num2 != len; num2++)
			{
				num += this.Process(inBytes[inOff + num2], outBytes, outOff + num);
			}
			return num;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x0012A990 File Offset: 0x00128B90
		public virtual int DoFinal(byte[] outBytes, int outOff)
		{
			this.InitCipher();
			int num = this.bufOff;
			byte[] array = new byte[this.bufBlock.Length];
			this.bufOff = 0;
			if (this.forEncryption)
			{
				Check.OutputLength(outBytes, outOff, num + this.macSize, "Output buffer too short");
				this.cipher.ProcessBlock(this.bufBlock, 0, array, 0);
				Array.Copy(array, 0, outBytes, outOff, num);
				this.mac.BlockUpdate(array, 0, num);
				this.CalculateMac();
				Array.Copy(this.macBlock, 0, outBytes, outOff + num, this.macSize);
				this.Reset(false);
				return num + this.macSize;
			}
			if (num < this.macSize)
			{
				throw new InvalidCipherTextException("data too short");
			}
			Check.OutputLength(outBytes, outOff, num - this.macSize, "Output buffer too short");
			if (num > this.macSize)
			{
				this.mac.BlockUpdate(this.bufBlock, 0, num - this.macSize);
				this.cipher.ProcessBlock(this.bufBlock, 0, array, 0);
				Array.Copy(array, 0, outBytes, outOff, num - this.macSize);
			}
			this.CalculateMac();
			if (!this.VerifyMac(this.bufBlock, num - this.macSize))
			{
				throw new InvalidCipherTextException("mac check in EAX failed");
			}
			this.Reset(false);
			return num - this.macSize;
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x0012AADC File Offset: 0x00128CDC
		public virtual byte[] GetMac()
		{
			byte[] array = new byte[this.macSize];
			Array.Copy(this.macBlock, 0, array, 0, this.macSize);
			return array;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x0012AB0C File Offset: 0x00128D0C
		public virtual int GetUpdateOutputSize(int len)
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
			return num - num % this.blockSize;
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x0012AB48 File Offset: 0x00128D48
		public virtual int GetOutputSize(int len)
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

		// Token: 0x06003175 RID: 12661 RVA: 0x0012AB84 File Offset: 0x00128D84
		private int Process(byte b, byte[] outBytes, int outOff)
		{
			byte[] array = this.bufBlock;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = b;
			if (this.bufOff == this.bufBlock.Length)
			{
				Check.OutputLength(outBytes, outOff, this.blockSize, "Output buffer is too short");
				int result;
				if (this.forEncryption)
				{
					result = this.cipher.ProcessBlock(this.bufBlock, 0, outBytes, outOff);
					this.mac.BlockUpdate(outBytes, outOff, this.blockSize);
				}
				else
				{
					this.mac.BlockUpdate(this.bufBlock, 0, this.blockSize);
					result = this.cipher.ProcessBlock(this.bufBlock, 0, outBytes, outOff);
				}
				this.bufOff = 0;
				if (!this.forEncryption)
				{
					Array.Copy(this.bufBlock, this.blockSize, this.bufBlock, 0, this.macSize);
					this.bufOff = this.macSize;
				}
				return result;
			}
			return 0;
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x0012AC6C File Offset: 0x00128E6C
		private bool VerifyMac(byte[] mac, int off)
		{
			int num = 0;
			for (int i = 0; i < this.macSize; i++)
			{
				num |= (int)(this.macBlock[i] ^ mac[off + i]);
			}
			return num == 0;
		}

		// Token: 0x04002077 RID: 8311
		private SicBlockCipher cipher;

		// Token: 0x04002078 RID: 8312
		private bool forEncryption;

		// Token: 0x04002079 RID: 8313
		private int blockSize;

		// Token: 0x0400207A RID: 8314
		private IMac mac;

		// Token: 0x0400207B RID: 8315
		private byte[] nonceMac;

		// Token: 0x0400207C RID: 8316
		private byte[] associatedTextMac;

		// Token: 0x0400207D RID: 8317
		private byte[] macBlock;

		// Token: 0x0400207E RID: 8318
		private int macSize;

		// Token: 0x0400207F RID: 8319
		private byte[] bufBlock;

		// Token: 0x04002080 RID: 8320
		private int bufOff;

		// Token: 0x04002081 RID: 8321
		private bool cipherInitialized;

		// Token: 0x04002082 RID: 8322
		private byte[] initialAssociatedText;

		// Token: 0x02000955 RID: 2389
		private enum Tag : byte
		{
			// Token: 0x04003638 RID: 13880
			N,
			// Token: 0x04003639 RID: 13881
			H,
			// Token: 0x0400363A RID: 13882
			C
		}
	}
}
