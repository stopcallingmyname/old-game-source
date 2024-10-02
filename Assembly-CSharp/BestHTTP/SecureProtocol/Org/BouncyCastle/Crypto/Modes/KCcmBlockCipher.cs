using System;
using System.IO;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000521 RID: 1313
	public class KCcmBlockCipher : IAeadBlockCipher
	{
		// Token: 0x060031A7 RID: 12711 RVA: 0x0012BCC9 File Offset: 0x00129EC9
		private void setNb(int Nb)
		{
			if (Nb == 4 || Nb == 6 || Nb == 8)
			{
				this.Nb_ = Nb;
				return;
			}
			throw new ArgumentException("Nb = 4 is recommended by DSTU7624 but can be changed to only 6 or 8 in this implementation");
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x0012BCE9 File Offset: 0x00129EE9
		public KCcmBlockCipher(IBlockCipher engine) : this(engine, 4)
		{
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x0012BCF4 File Offset: 0x00129EF4
		public KCcmBlockCipher(IBlockCipher engine, int Nb)
		{
			this.engine = engine;
			this.macSize = engine.GetBlockSize();
			this.nonce = new byte[engine.GetBlockSize()];
			this.initialAssociatedText = new byte[engine.GetBlockSize()];
			this.mac = new byte[engine.GetBlockSize()];
			this.macBlock = new byte[engine.GetBlockSize()];
			this.G1 = new byte[engine.GetBlockSize()];
			this.buffer = new byte[engine.GetBlockSize()];
			this.s = new byte[engine.GetBlockSize()];
			this.counter = new byte[engine.GetBlockSize()];
			this.setNb(Nb);
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x0012BDC8 File Offset: 0x00129FC8
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			ICipherParameters parameters2;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				if (aeadParameters.MacSize > KCcmBlockCipher.MAX_MAC_BIT_LENGTH || aeadParameters.MacSize < KCcmBlockCipher.MIN_MAC_BIT_LENGTH || aeadParameters.MacSize % 8 != 0)
				{
					throw new ArgumentException("Invalid mac size specified");
				}
				this.nonce = aeadParameters.GetNonce();
				this.macSize = aeadParameters.MacSize / KCcmBlockCipher.BITS_IN_BYTE;
				this.initialAssociatedText = aeadParameters.GetAssociatedText();
				parameters2 = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("Invalid parameters specified");
				}
				this.nonce = ((ParametersWithIV)parameters).GetIV();
				this.macSize = this.engine.GetBlockSize();
				this.initialAssociatedText = null;
				parameters2 = ((ParametersWithIV)parameters).Parameters;
			}
			this.mac = new byte[this.macSize];
			this.forEncryption = forEncryption;
			this.engine.Init(true, parameters2);
			this.counter[0] = 1;
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x0012BEDB File Offset: 0x0012A0DB
		public virtual string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName + "/KCCM";
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x0012BEF2 File Offset: 0x0012A0F2
		public virtual int GetBlockSize()
		{
			return this.engine.GetBlockSize();
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x0012BEFF File Offset: 0x0012A0FF
		public virtual IBlockCipher GetUnderlyingCipher()
		{
			return this.engine;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x0012BF07 File Offset: 0x0012A107
		public virtual void ProcessAadByte(byte input)
		{
			this.associatedText.WriteByte(input);
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x0012BF15 File Offset: 0x0012A115
		public virtual void ProcessAadBytes(byte[] input, int inOff, int len)
		{
			this.associatedText.Write(input, inOff, len);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x0012BF28 File Offset: 0x0012A128
		private void ProcessAAD(byte[] assocText, int assocOff, int assocLen, int dataLen)
		{
			if (assocLen - assocOff < this.engine.GetBlockSize())
			{
				throw new ArgumentException("authText buffer too short");
			}
			if (assocLen % this.engine.GetBlockSize() != 0)
			{
				throw new ArgumentException("padding not supported");
			}
			Array.Copy(this.nonce, 0, this.G1, 0, this.nonce.Length - this.Nb_ - 1);
			this.intToBytes(dataLen, this.buffer, 0);
			Array.Copy(this.buffer, 0, this.G1, this.nonce.Length - this.Nb_ - 1, KCcmBlockCipher.BYTES_IN_INT);
			this.G1[this.G1.Length - 1] = this.getFlag(true, this.macSize);
			this.engine.ProcessBlock(this.G1, 0, this.macBlock, 0);
			this.intToBytes(assocLen, this.buffer, 0);
			if (assocLen <= this.engine.GetBlockSize() - this.Nb_)
			{
				for (int i = 0; i < assocLen; i++)
				{
					byte[] array = this.buffer;
					int num = i + this.Nb_;
					array[num] ^= assocText[assocOff + i];
				}
				for (int j = 0; j < this.engine.GetBlockSize(); j++)
				{
					byte[] array2 = this.macBlock;
					int num2 = j;
					array2[num2] ^= this.buffer[j];
				}
				this.engine.ProcessBlock(this.macBlock, 0, this.macBlock, 0);
				return;
			}
			for (int k = 0; k < this.engine.GetBlockSize(); k++)
			{
				byte[] array3 = this.macBlock;
				int num3 = k;
				array3[num3] ^= this.buffer[k];
			}
			this.engine.ProcessBlock(this.macBlock, 0, this.macBlock, 0);
			for (int num4 = assocLen; num4 != 0; num4 -= this.engine.GetBlockSize())
			{
				for (int l = 0; l < this.engine.GetBlockSize(); l++)
				{
					byte[] array4 = this.macBlock;
					int num5 = l;
					array4[num5] ^= assocText[l + assocOff];
				}
				this.engine.ProcessBlock(this.macBlock, 0, this.macBlock, 0);
				assocOff += this.engine.GetBlockSize();
			}
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x0012C14C File Offset: 0x0012A34C
		public virtual int ProcessByte(byte input, byte[] output, int outOff)
		{
			this.data.WriteByte(input);
			return 0;
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x0012C15B File Offset: 0x0012A35B
		public virtual int ProcessBytes(byte[] input, int inOff, int inLen, byte[] output, int outOff)
		{
			Check.DataLength(input, inOff, inLen, "input buffer too short");
			this.data.Write(input, inOff, inLen);
			return 0;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x0012C17C File Offset: 0x0012A37C
		public int ProcessPacket(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			Check.DataLength(input, inOff, len, "input buffer too short");
			Check.OutputLength(output, outOff, len, "output buffer too short");
			if (this.associatedText.Length > 0L)
			{
				byte[] assocText = this.associatedText.GetBuffer();
				int assocLen = (int)this.associatedText.Length;
				int dataLen = this.forEncryption ? ((int)this.data.Length) : ((int)this.data.Length - this.macSize);
				this.ProcessAAD(assocText, 0, assocLen, dataLen);
			}
			if (this.forEncryption)
			{
				Check.DataLength(len % this.engine.GetBlockSize() != 0, "partial blocks not supported");
				this.CalculateMac(input, inOff, len);
				this.engine.ProcessBlock(this.nonce, 0, this.s, 0);
				int i = len;
				while (i > 0)
				{
					this.ProcessBlock(input, inOff, len, output, outOff);
					i -= this.engine.GetBlockSize();
					inOff += this.engine.GetBlockSize();
					outOff += this.engine.GetBlockSize();
				}
				for (int j = 0; j < this.counter.Length; j++)
				{
					byte[] array = this.s;
					int num = j;
					array[num] += this.counter[j];
				}
				this.engine.ProcessBlock(this.s, 0, this.buffer, 0);
				for (int k = 0; k < this.macSize; k++)
				{
					output[outOff + k] = (this.buffer[k] ^ this.macBlock[k]);
				}
				Array.Copy(this.macBlock, 0, this.mac, 0, this.macSize);
				this.Reset();
				return len + this.macSize;
			}
			Check.DataLength((len - this.macSize) % this.engine.GetBlockSize() != 0, "partial blocks not supported");
			this.engine.ProcessBlock(this.nonce, 0, this.s, 0);
			int num2 = len / this.engine.GetBlockSize();
			for (int l = 0; l < num2; l++)
			{
				this.ProcessBlock(input, inOff, len, output, outOff);
				inOff += this.engine.GetBlockSize();
				outOff += this.engine.GetBlockSize();
			}
			if (len > inOff)
			{
				for (int m = 0; m < this.counter.Length; m++)
				{
					byte[] array2 = this.s;
					int num3 = m;
					array2[num3] += this.counter[m];
				}
				this.engine.ProcessBlock(this.s, 0, this.buffer, 0);
				for (int n = 0; n < this.macSize; n++)
				{
					output[outOff + n] = (this.buffer[n] ^ input[inOff + n]);
				}
				outOff += this.macSize;
			}
			for (int num4 = 0; num4 < this.counter.Length; num4++)
			{
				byte[] array3 = this.s;
				int num5 = num4;
				array3[num5] += this.counter[num4];
			}
			this.engine.ProcessBlock(this.s, 0, this.buffer, 0);
			Array.Copy(output, outOff - this.macSize, this.buffer, 0, this.macSize);
			this.CalculateMac(output, 0, outOff - this.macSize);
			Array.Copy(this.macBlock, 0, this.mac, 0, this.macSize);
			byte[] array4 = new byte[this.macSize];
			Array.Copy(this.buffer, 0, array4, 0, this.macSize);
			if (!Arrays.ConstantTimeAreEqual(this.mac, array4))
			{
				throw new InvalidCipherTextException("mac check failed");
			}
			this.Reset();
			return len - this.macSize;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x0012C524 File Offset: 0x0012A724
		private void ProcessBlock(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			for (int i = 0; i < this.counter.Length; i++)
			{
				byte[] array = this.s;
				int num = i;
				array[num] += this.counter[i];
			}
			this.engine.ProcessBlock(this.s, 0, this.buffer, 0);
			for (int j = 0; j < this.engine.GetBlockSize(); j++)
			{
				output[outOff + j] = (this.buffer[j] ^ input[inOff + j]);
			}
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x0012C5A4 File Offset: 0x0012A7A4
		private void CalculateMac(byte[] authText, int authOff, int len)
		{
			int i = len;
			while (i > 0)
			{
				for (int j = 0; j < this.engine.GetBlockSize(); j++)
				{
					byte[] array = this.macBlock;
					int num = j;
					array[num] ^= authText[authOff + j];
				}
				this.engine.ProcessBlock(this.macBlock, 0, this.macBlock, 0);
				i -= this.engine.GetBlockSize();
				authOff += this.engine.GetBlockSize();
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x0012C61C File Offset: 0x0012A81C
		public virtual int DoFinal(byte[] output, int outOff)
		{
			byte[] input = this.data.GetBuffer();
			int len = (int)this.data.Length;
			int result = this.ProcessPacket(input, 0, len, output, outOff);
			this.Reset();
			return result;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x0012C653 File Offset: 0x0012A853
		public virtual byte[] GetMac()
		{
			return Arrays.Clone(this.mac);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000A54F9 File Offset: 0x000A36F9
		public virtual int GetUpdateOutputSize(int len)
		{
			return len;
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x0012C660 File Offset: 0x0012A860
		public virtual int GetOutputSize(int len)
		{
			return len + this.macSize;
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x0012C66C File Offset: 0x0012A86C
		public virtual void Reset()
		{
			Arrays.Fill(this.G1, 0);
			Arrays.Fill(this.buffer, 0);
			Arrays.Fill(this.counter, 0);
			Arrays.Fill(this.macBlock, 0);
			this.counter[0] = 1;
			this.data.SetLength(0L);
			this.associatedText.SetLength(0L);
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x0012BCA5 File Offset: 0x00129EA5
		private void intToBytes(int num, byte[] outBytes, int outOff)
		{
			outBytes[outOff + 3] = (byte)(num >> 24);
			outBytes[outOff + 2] = (byte)(num >> 16);
			outBytes[outOff + 1] = (byte)(num >> 8);
			outBytes[outOff] = (byte)num;
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x0012C6EC File Offset: 0x0012A8EC
		private byte getFlag(bool authTextPresents, int macSize)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (authTextPresents)
			{
				stringBuilder.Append("1");
			}
			else
			{
				stringBuilder.Append("0");
			}
			if (macSize <= 16)
			{
				if (macSize != 8)
				{
					if (macSize == 16)
					{
						stringBuilder.Append("011");
					}
				}
				else
				{
					stringBuilder.Append("010");
				}
			}
			else if (macSize != 32)
			{
				if (macSize != 48)
				{
					if (macSize == 64)
					{
						stringBuilder.Append("110");
					}
				}
				else
				{
					stringBuilder.Append("101");
				}
			}
			else
			{
				stringBuilder.Append("100");
			}
			string text = Convert.ToString(this.Nb_ - 1, 2);
			while (text.Length < 4)
			{
				text = new StringBuilder(text).Insert(0, "0").ToString();
			}
			stringBuilder.Append(text);
			return (byte)Convert.ToInt32(stringBuilder.ToString(), 2);
		}

		// Token: 0x040020A7 RID: 8359
		private static readonly int BYTES_IN_INT = 4;

		// Token: 0x040020A8 RID: 8360
		private static readonly int BITS_IN_BYTE = 8;

		// Token: 0x040020A9 RID: 8361
		private static readonly int MAX_MAC_BIT_LENGTH = 512;

		// Token: 0x040020AA RID: 8362
		private static readonly int MIN_MAC_BIT_LENGTH = 64;

		// Token: 0x040020AB RID: 8363
		private IBlockCipher engine;

		// Token: 0x040020AC RID: 8364
		private int macSize;

		// Token: 0x040020AD RID: 8365
		private bool forEncryption;

		// Token: 0x040020AE RID: 8366
		private byte[] initialAssociatedText;

		// Token: 0x040020AF RID: 8367
		private byte[] mac;

		// Token: 0x040020B0 RID: 8368
		private byte[] macBlock;

		// Token: 0x040020B1 RID: 8369
		private byte[] nonce;

		// Token: 0x040020B2 RID: 8370
		private byte[] G1;

		// Token: 0x040020B3 RID: 8371
		private byte[] buffer;

		// Token: 0x040020B4 RID: 8372
		private byte[] s;

		// Token: 0x040020B5 RID: 8373
		private byte[] counter;

		// Token: 0x040020B6 RID: 8374
		private readonly MemoryStream associatedText = new MemoryStream();

		// Token: 0x040020B7 RID: 8375
		private readonly MemoryStream data = new MemoryStream();

		// Token: 0x040020B8 RID: 8376
		private int Nb_ = 4;
	}
}
