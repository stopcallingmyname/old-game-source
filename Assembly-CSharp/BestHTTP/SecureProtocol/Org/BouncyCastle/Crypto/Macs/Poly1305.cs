using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000539 RID: 1337
	public class Poly1305 : IMac
	{
		// Token: 0x060032A0 RID: 12960 RVA: 0x00130D9F File Offset: 0x0012EF9F
		public Poly1305()
		{
			this.cipher = null;
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x00130DC7 File Offset: 0x0012EFC7
		public Poly1305(IBlockCipher cipher)
		{
			if (cipher.GetBlockSize() != 16)
			{
				throw new ArgumentException("Poly1305 requires a 128 bit block cipher.");
			}
			this.cipher = cipher;
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x00130E04 File Offset: 0x0012F004
		public void Init(ICipherParameters parameters)
		{
			byte[] nonce = null;
			if (this.cipher != null)
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("Poly1305 requires an IV when used with a block cipher.", "parameters");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				nonce = parametersWithIV.GetIV();
				parameters = parametersWithIV.Parameters;
			}
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("Poly1305 requires a key.");
			}
			KeyParameter keyParameter = (KeyParameter)parameters;
			this.SetKey(keyParameter.GetKey(), nonce);
			this.Reset();
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x00130E74 File Offset: 0x0012F074
		private void SetKey(byte[] key, byte[] nonce)
		{
			if (key.Length != 32)
			{
				throw new ArgumentException("Poly1305 key must be 256 bits.");
			}
			if (this.cipher != null && (nonce == null || nonce.Length != 16))
			{
				throw new ArgumentException("Poly1305 requires a 128 bit IV.");
			}
			uint num = Pack.LE_To_UInt32(key, 0);
			uint num2 = Pack.LE_To_UInt32(key, 4);
			uint num3 = Pack.LE_To_UInt32(key, 8);
			uint num4 = Pack.LE_To_UInt32(key, 12);
			this.r0 = (num & 67108863U);
			this.r1 = ((num >> 26 | num2 << 6) & 67108611U);
			this.r2 = ((num2 >> 20 | num3 << 12) & 67092735U);
			this.r3 = ((num3 >> 14 | num4 << 18) & 66076671U);
			this.r4 = (num4 >> 8 & 1048575U);
			this.s1 = this.r1 * 5U;
			this.s2 = this.r2 * 5U;
			this.s3 = this.r3 * 5U;
			this.s4 = this.r4 * 5U;
			byte[] array;
			int num5;
			if (this.cipher == null)
			{
				array = key;
				num5 = 16;
			}
			else
			{
				array = new byte[16];
				num5 = 0;
				this.cipher.Init(true, new KeyParameter(key, 16, 16));
				this.cipher.ProcessBlock(nonce, 0, array, 0);
			}
			this.k0 = Pack.LE_To_UInt32(array, num5);
			this.k1 = Pack.LE_To_UInt32(array, num5 + 4);
			this.k2 = Pack.LE_To_UInt32(array, num5 + 8);
			this.k3 = Pack.LE_To_UInt32(array, num5 + 12);
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x00130FEA File Offset: 0x0012F1EA
		public string AlgorithmName
		{
			get
			{
				if (this.cipher != null)
				{
					return "Poly1305-" + this.cipher.AlgorithmName;
				}
				return "Poly1305";
			}
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x0012AD29 File Offset: 0x00128F29
		public int GetMacSize()
		{
			return 16;
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x0013100F File Offset: 0x0012F20F
		public void Update(byte input)
		{
			this.singleByte[0] = input;
			this.BlockUpdate(this.singleByte, 0, 1);
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x00131028 File Offset: 0x0012F228
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			int num = 0;
			while (len > num)
			{
				if (this.currentBlockOffset == 16)
				{
					this.ProcessBlock();
					this.currentBlockOffset = 0;
				}
				int num2 = Math.Min(len - num, 16 - this.currentBlockOffset);
				Array.Copy(input, num + inOff, this.currentBlock, this.currentBlockOffset, num2);
				num += num2;
				this.currentBlockOffset += num2;
			}
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x00131090 File Offset: 0x0012F290
		private void ProcessBlock()
		{
			if (this.currentBlockOffset < 16)
			{
				this.currentBlock[this.currentBlockOffset] = 1;
				for (int i = this.currentBlockOffset + 1; i < 16; i++)
				{
					this.currentBlock[i] = 0;
				}
			}
			ulong num = (ulong)Pack.LE_To_UInt32(this.currentBlock, 0);
			ulong num2 = (ulong)Pack.LE_To_UInt32(this.currentBlock, 4);
			ulong num3 = (ulong)Pack.LE_To_UInt32(this.currentBlock, 8);
			ulong num4 = (ulong)Pack.LE_To_UInt32(this.currentBlock, 12);
			this.h0 += (uint)(num & 67108863UL);
			this.h1 += (uint)((num2 << 32 | num) >> 26 & 67108863UL);
			this.h2 += (uint)((num3 << 32 | num2) >> 20 & 67108863UL);
			this.h3 += (uint)((num4 << 32 | num3) >> 14 & 67108863UL);
			this.h4 += (uint)(num4 >> 8);
			if (this.currentBlockOffset == 16)
			{
				this.h4 += 16777216U;
			}
			ulong num5 = Poly1305.mul32x32_64(this.h0, this.r0) + Poly1305.mul32x32_64(this.h1, this.s4) + Poly1305.mul32x32_64(this.h2, this.s3) + Poly1305.mul32x32_64(this.h3, this.s2) + Poly1305.mul32x32_64(this.h4, this.s1);
			ulong num6 = Poly1305.mul32x32_64(this.h0, this.r1) + Poly1305.mul32x32_64(this.h1, this.r0) + Poly1305.mul32x32_64(this.h2, this.s4) + Poly1305.mul32x32_64(this.h3, this.s3) + Poly1305.mul32x32_64(this.h4, this.s2);
			ulong num7 = Poly1305.mul32x32_64(this.h0, this.r2) + Poly1305.mul32x32_64(this.h1, this.r1) + Poly1305.mul32x32_64(this.h2, this.r0) + Poly1305.mul32x32_64(this.h3, this.s4) + Poly1305.mul32x32_64(this.h4, this.s3);
			ulong num8 = Poly1305.mul32x32_64(this.h0, this.r3) + Poly1305.mul32x32_64(this.h1, this.r2) + Poly1305.mul32x32_64(this.h2, this.r1) + Poly1305.mul32x32_64(this.h3, this.r0) + Poly1305.mul32x32_64(this.h4, this.s4);
			ulong num9 = Poly1305.mul32x32_64(this.h0, this.r4) + Poly1305.mul32x32_64(this.h1, this.r3) + Poly1305.mul32x32_64(this.h2, this.r2) + Poly1305.mul32x32_64(this.h3, this.r1) + Poly1305.mul32x32_64(this.h4, this.r0);
			this.h0 = ((uint)num5 & 67108863U);
			num6 += num5 >> 26;
			this.h1 = ((uint)num6 & 67108863U);
			num7 += num6 >> 26;
			this.h2 = ((uint)num7 & 67108863U);
			num8 += num7 >> 26;
			this.h3 = ((uint)num8 & 67108863U);
			num9 += num8 >> 26;
			this.h4 = ((uint)num9 & 67108863U);
			this.h0 += (uint)(num9 >> 26) * 5U;
			this.h1 += this.h0 >> 26;
			this.h0 &= 67108863U;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x00131424 File Offset: 0x0012F624
		public int DoFinal(byte[] output, int outOff)
		{
			Check.DataLength(output, outOff, 16, "Output buffer is too short.");
			if (this.currentBlockOffset > 0)
			{
				this.ProcessBlock();
			}
			this.h1 += this.h0 >> 26;
			this.h0 &= 67108863U;
			this.h2 += this.h1 >> 26;
			this.h1 &= 67108863U;
			this.h3 += this.h2 >> 26;
			this.h2 &= 67108863U;
			this.h4 += this.h3 >> 26;
			this.h3 &= 67108863U;
			this.h0 += (this.h4 >> 26) * 5U;
			this.h4 &= 67108863U;
			this.h1 += this.h0 >> 26;
			this.h0 &= 67108863U;
			uint num = this.h0 + 5U;
			uint num2 = num >> 26;
			num &= 67108863U;
			uint num3 = this.h1 + num2;
			num2 = num3 >> 26;
			num3 &= 67108863U;
			uint num4 = this.h2 + num2;
			num2 = num4 >> 26;
			num4 &= 67108863U;
			uint num5 = this.h3 + num2;
			num2 = num5 >> 26;
			num5 &= 67108863U;
			uint num6 = this.h4 + num2 - 67108864U;
			num2 = (num6 >> 31) - 1U;
			uint num7 = ~num2;
			this.h0 = ((this.h0 & num7) | (num & num2));
			this.h1 = ((this.h1 & num7) | (num3 & num2));
			this.h2 = ((this.h2 & num7) | (num4 & num2));
			this.h3 = ((this.h3 & num7) | (num5 & num2));
			this.h4 = ((this.h4 & num7) | (num6 & num2));
			ulong num8 = (ulong)(this.h0 | this.h1 << 26) + (ulong)this.k0;
			ulong num9 = (ulong)(this.h1 >> 6 | this.h2 << 20) + (ulong)this.k1;
			ulong num10 = (ulong)(this.h2 >> 12 | this.h3 << 14) + (ulong)this.k2;
			ulong num11 = (ulong)(this.h3 >> 18 | this.h4 << 8) + (ulong)this.k3;
			Pack.UInt32_To_LE((uint)num8, output, outOff);
			num9 += num8 >> 32;
			Pack.UInt32_To_LE((uint)num9, output, outOff + 4);
			num10 += num9 >> 32;
			Pack.UInt32_To_LE((uint)num10, output, outOff + 8);
			Pack.UInt32_To_LE((uint)(num11 + (num10 >> 32)), output, outOff + 12);
			this.Reset();
			return 16;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x001316E4 File Offset: 0x0012F8E4
		public void Reset()
		{
			this.currentBlockOffset = 0;
			this.h0 = (this.h1 = (this.h2 = (this.h3 = (this.h4 = 0U))));
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x00131723 File Offset: 0x0012F923
		private static ulong mul32x32_64(uint i1, uint i2)
		{
			return (ulong)i1 * (ulong)i2;
		}

		// Token: 0x04002137 RID: 8503
		private const int BlockSize = 16;

		// Token: 0x04002138 RID: 8504
		private readonly IBlockCipher cipher;

		// Token: 0x04002139 RID: 8505
		private readonly byte[] singleByte = new byte[1];

		// Token: 0x0400213A RID: 8506
		private uint r0;

		// Token: 0x0400213B RID: 8507
		private uint r1;

		// Token: 0x0400213C RID: 8508
		private uint r2;

		// Token: 0x0400213D RID: 8509
		private uint r3;

		// Token: 0x0400213E RID: 8510
		private uint r4;

		// Token: 0x0400213F RID: 8511
		private uint s1;

		// Token: 0x04002140 RID: 8512
		private uint s2;

		// Token: 0x04002141 RID: 8513
		private uint s3;

		// Token: 0x04002142 RID: 8514
		private uint s4;

		// Token: 0x04002143 RID: 8515
		private uint k0;

		// Token: 0x04002144 RID: 8516
		private uint k1;

		// Token: 0x04002145 RID: 8517
		private uint k2;

		// Token: 0x04002146 RID: 8518
		private uint k3;

		// Token: 0x04002147 RID: 8519
		private byte[] currentBlock = new byte[16];

		// Token: 0x04002148 RID: 8520
		private int currentBlockOffset;

		// Token: 0x04002149 RID: 8521
		private uint h0;

		// Token: 0x0400214A RID: 8522
		private uint h1;

		// Token: 0x0400214B RID: 8523
		private uint h2;

		// Token: 0x0400214C RID: 8524
		private uint h3;

		// Token: 0x0400214D RID: 8525
		private uint h4;
	}
}
