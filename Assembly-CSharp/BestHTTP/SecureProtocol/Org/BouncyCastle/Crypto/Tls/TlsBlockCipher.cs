﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000455 RID: 1109
	public class TlsBlockCipher : TlsCipher
	{
		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x00114F12 File Offset: 0x00113112
		public virtual TlsMac WriteMac
		{
			get
			{
				return this.mWriteMac;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x00114F1A File Offset: 0x0011311A
		public virtual TlsMac ReadMac
		{
			get
			{
				return this.mReadMac;
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x00114F24 File Offset: 0x00113124
		public TlsBlockCipher(TlsContext context, IBlockCipher clientWriteCipher, IBlockCipher serverWriteCipher, IDigest clientWriteDigest, IDigest serverWriteDigest, int cipherKeySize)
		{
			this.context = context;
			this.randomData = new byte[256];
			context.NonceRandomGenerator.NextBytes(this.randomData);
			this.useExplicitIV = TlsUtilities.IsTlsV11(context);
			this.encryptThenMac = context.SecurityParameters.encryptThenMac;
			int num = 2 * cipherKeySize + clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
			if (!this.useExplicitIV)
			{
				num += clientWriteCipher.GetBlockSize() + serverWriteCipher.GetBlockSize();
			}
			byte[] array = TlsUtilities.CalculateKeyBlock(context, num);
			int num2 = 0;
			TlsMac tlsMac = new TlsMac(context, clientWriteDigest, array, num2, clientWriteDigest.GetDigestSize());
			num2 += clientWriteDigest.GetDigestSize();
			TlsMac tlsMac2 = new TlsMac(context, serverWriteDigest, array, num2, serverWriteDigest.GetDigestSize());
			num2 += serverWriteDigest.GetDigestSize();
			KeyParameter parameters = new KeyParameter(array, num2, cipherKeySize);
			num2 += cipherKeySize;
			KeyParameter parameters2 = new KeyParameter(array, num2, cipherKeySize);
			num2 += cipherKeySize;
			byte[] iv;
			byte[] iv2;
			if (this.useExplicitIV)
			{
				iv = new byte[clientWriteCipher.GetBlockSize()];
				iv2 = new byte[serverWriteCipher.GetBlockSize()];
			}
			else
			{
				iv = Arrays.CopyOfRange(array, num2, num2 + clientWriteCipher.GetBlockSize());
				num2 += clientWriteCipher.GetBlockSize();
				iv2 = Arrays.CopyOfRange(array, num2, num2 + serverWriteCipher.GetBlockSize());
				num2 += serverWriteCipher.GetBlockSize();
			}
			if (num2 != num)
			{
				throw new TlsFatalAlert(80);
			}
			ICipherParameters parameters3;
			ICipherParameters parameters4;
			if (context.IsServer)
			{
				this.mWriteMac = tlsMac2;
				this.mReadMac = tlsMac;
				this.encryptCipher = serverWriteCipher;
				this.decryptCipher = clientWriteCipher;
				parameters3 = new ParametersWithIV(parameters2, iv2);
				parameters4 = new ParametersWithIV(parameters, iv);
			}
			else
			{
				this.mWriteMac = tlsMac;
				this.mReadMac = tlsMac2;
				this.encryptCipher = clientWriteCipher;
				this.decryptCipher = serverWriteCipher;
				parameters3 = new ParametersWithIV(parameters, iv);
				parameters4 = new ParametersWithIV(parameters2, iv2);
			}
			this.encryptCipher.Init(true, parameters3);
			this.decryptCipher.Init(false, parameters4);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x001150FC File Offset: 0x001132FC
		public virtual int GetPlaintextLimit(int ciphertextLimit)
		{
			int blockSize = this.encryptCipher.GetBlockSize();
			int size = this.mWriteMac.Size;
			int num = ciphertextLimit;
			if (this.useExplicitIV)
			{
				num -= blockSize;
			}
			if (this.encryptThenMac)
			{
				num -= size;
				num -= num % blockSize;
			}
			else
			{
				num -= num % blockSize;
				num -= size;
			}
			return num - 1;
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x00115154 File Offset: 0x00113354
		public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
		{
			int blockSize = this.encryptCipher.GetBlockSize();
			int size = this.mWriteMac.Size;
			ProtocolVersion serverVersion = this.context.ServerVersion;
			int num = len;
			if (!this.encryptThenMac)
			{
				num += size;
			}
			int num2 = blockSize - 1 - num % blockSize;
			if ((this.encryptThenMac || !this.context.SecurityParameters.truncatedHMac) && !serverVersion.IsDtls && !serverVersion.IsSsl)
			{
				int max = (255 - num2) / blockSize;
				int num3 = this.ChooseExtraPadBlocks(this.context.SecureRandom, max);
				num2 += num3 * blockSize;
			}
			int num4 = len + size + num2 + 1;
			if (this.useExplicitIV)
			{
				num4 += blockSize;
			}
			byte[] array = new byte[num4];
			int num5 = 0;
			if (this.useExplicitIV)
			{
				if (this.explicitIV == null || this.explicitIV.Length != blockSize)
				{
					Array.Resize<byte>(ref this.explicitIV, blockSize);
				}
				this.context.NonceRandomGenerator.NextBytes(this.explicitIV);
				this.encryptCipher.Init(true, new ParametersWithIV(null, this.explicitIV));
				Array.Copy(this.explicitIV, 0, array, num5, blockSize);
				num5 += blockSize;
			}
			int num6 = num5;
			Array.Copy(plaintext, offset, array, num5, len);
			num5 += len;
			if (!this.encryptThenMac)
			{
				byte[] array2 = this.mWriteMac.CalculateMac(seqNo, type, plaintext, offset, len);
				Array.Copy(array2, 0, array, num5, array2.Length);
				num5 += array2.Length;
			}
			for (int i = 0; i <= num2; i++)
			{
				array[num5++] = (byte)num2;
			}
			for (int j = num6; j < num5; j += blockSize)
			{
				this.encryptCipher.ProcessBlock(array, j, array, j);
			}
			if (this.encryptThenMac)
			{
				byte[] array3 = this.mWriteMac.CalculateMac(seqNo, type, array, 0, num5);
				Array.Copy(array3, 0, array, num5, array3.Length);
				num5 += array3.Length;
			}
			return array;
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x0011534C File Offset: 0x0011354C
		public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
		{
			int blockSize = this.decryptCipher.GetBlockSize();
			int size = this.mReadMac.Size;
			int num = blockSize;
			if (this.encryptThenMac)
			{
				num += size;
			}
			else
			{
				num = Math.Max(num, size + 1);
			}
			if (this.useExplicitIV)
			{
				num += blockSize;
			}
			if (len < num)
			{
				throw new TlsFatalAlert(50);
			}
			int num2 = len;
			if (this.encryptThenMac)
			{
				num2 -= size;
			}
			if (num2 % blockSize != 0)
			{
				throw new TlsFatalAlert(21);
			}
			if (this.encryptThenMac)
			{
				int num3 = offset + len;
				byte[] b = Arrays.CopyOfRange(ciphertext, num3 - size, num3);
				if (!Arrays.ConstantTimeAreEqual(this.mReadMac.CalculateMac(seqNo, type, ciphertext, offset, len - size), b))
				{
					throw new TlsFatalAlert(20);
				}
			}
			if (this.useExplicitIV)
			{
				this.decryptCipher.Init(false, new ParametersWithIV(null, ciphertext, offset, blockSize));
				offset += blockSize;
				num2 -= blockSize;
			}
			for (int i = 0; i < num2; i += blockSize)
			{
				this.decryptCipher.ProcessBlock(ciphertext, offset + i, ciphertext, offset + i);
			}
			int num4 = this.CheckPaddingConstantTime(ciphertext, offset, num2, blockSize, this.encryptThenMac ? 0 : size);
			bool flag = num4 == 0;
			int num5 = num2 - num4;
			if (!this.encryptThenMac)
			{
				num5 -= size;
				int num6 = num5;
				int num7 = offset + num6;
				byte[] b2 = Arrays.CopyOfRange(ciphertext, num7, num7 + size);
				byte[] a = this.mReadMac.CalculateMacConstantTime(seqNo, type, ciphertext, offset, num6, num2 - size, this.randomData);
				flag |= !Arrays.ConstantTimeAreEqual(a, b2);
			}
			if (flag)
			{
				throw new TlsFatalAlert(20);
			}
			return Arrays.CopyOfRange(ciphertext, offset, offset + num5);
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x001154EC File Offset: 0x001136EC
		protected virtual int CheckPaddingConstantTime(byte[] buf, int off, int len, int blockSize, int macSize)
		{
			int num = off + len;
			byte b = buf[num - 1];
			int num2 = (int)((b & byte.MaxValue) + 1);
			int i = 0;
			byte b2 = 0;
			if ((TlsUtilities.IsSsl(this.context) && num2 > blockSize) || macSize + num2 > len)
			{
				num2 = 0;
			}
			else
			{
				int num3 = num - num2;
				do
				{
					b2 |= (buf[num3++] ^ b);
				}
				while (num3 < num);
				i = num2;
				if (b2 != 0)
				{
					num2 = 0;
				}
			}
			byte[] array = this.randomData;
			while (i < 256)
			{
				b2 |= (array[i++] ^ b);
			}
			byte[] array2 = array;
			int num4 = 0;
			array2[num4] ^= b2;
			return num2;
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x00115588 File Offset: 0x00113788
		protected virtual int ChooseExtraPadBlocks(SecureRandom r, int max)
		{
			int x = r.NextInt();
			return Math.Min(this.LowestBitSet(x), max);
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x001155AC File Offset: 0x001137AC
		protected virtual int LowestBitSet(int x)
		{
			if (x == 0)
			{
				return 32;
			}
			uint num = (uint)x;
			int num2 = 0;
			while ((num & 1U) == 0U)
			{
				num2++;
				num >>= 1;
			}
			return num2;
		}

		// Token: 0x04001E10 RID: 7696
		protected readonly TlsContext context;

		// Token: 0x04001E11 RID: 7697
		protected readonly byte[] randomData;

		// Token: 0x04001E12 RID: 7698
		protected readonly bool useExplicitIV;

		// Token: 0x04001E13 RID: 7699
		protected readonly bool encryptThenMac;

		// Token: 0x04001E14 RID: 7700
		protected readonly IBlockCipher encryptCipher;

		// Token: 0x04001E15 RID: 7701
		protected readonly IBlockCipher decryptCipher;

		// Token: 0x04001E16 RID: 7702
		protected readonly TlsMac mWriteMac;

		// Token: 0x04001E17 RID: 7703
		protected readonly TlsMac mReadMac;

		// Token: 0x04001E18 RID: 7704
		private byte[] explicitIV;
	}
}
