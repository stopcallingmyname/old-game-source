using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200057B RID: 1403
	public class IdeaEngine : IBlockCipher
	{
		// Token: 0x060034E3 RID: 13539 RVA: 0x001434AA File Offset: 0x001416AA
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to IDEA init - " + Platform.GetTypeName(parameters));
			}
			this.workingKey = this.GenerateWorkingKey(forEncryption, ((KeyParameter)parameters).GetKey());
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060034E4 RID: 13540 RVA: 0x001434E2 File Offset: 0x001416E2
		public virtual string AlgorithmName
		{
			get
			{
				return "IDEA";
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060034E5 RID: 13541 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x001434EC File Offset: 0x001416EC
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("IDEA engine not initialised");
			}
			Check.DataLength(input, inOff, 8, "input buffer too short");
			Check.OutputLength(output, outOff, 8, "output buffer too short");
			this.IdeaFunc(this.workingKey, input, inOff, output, outOff);
			return 8;
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x00143539 File Offset: 0x00141739
		private int BytesToWord(byte[] input, int inOff)
		{
			return ((int)input[inOff] << 8 & 65280) + (int)(input[inOff + 1] & byte.MaxValue);
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x00143552 File Offset: 0x00141752
		private void WordToBytes(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)((uint)word >> 8);
			outBytes[outOff + 1] = (byte)word;
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x00143564 File Offset: 0x00141764
		private int Mul(int x, int y)
		{
			if (x == 0)
			{
				x = IdeaEngine.BASE - y;
			}
			else if (y == 0)
			{
				x = IdeaEngine.BASE - x;
			}
			else
			{
				int num = x * y;
				y = (num & IdeaEngine.MASK);
				x = (int)((uint)num >> 16);
				x = y - x + ((y < x) ? 1 : 0);
			}
			return x & IdeaEngine.MASK;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x001435B4 File Offset: 0x001417B4
		private void IdeaFunc(int[] workingKey, byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = 0;
			int num2 = this.BytesToWord(input, inOff);
			int num3 = this.BytesToWord(input, inOff + 2);
			int num4 = this.BytesToWord(input, inOff + 4);
			int num5 = this.BytesToWord(input, inOff + 6);
			for (int i = 0; i < 8; i++)
			{
				num2 = this.Mul(num2, workingKey[num++]);
				num3 += workingKey[num++];
				num3 &= IdeaEngine.MASK;
				num4 += workingKey[num++];
				num4 &= IdeaEngine.MASK;
				num5 = this.Mul(num5, workingKey[num++]);
				int num6 = num3;
				int num7 = num4;
				num4 ^= num2;
				num3 ^= num5;
				num4 = this.Mul(num4, workingKey[num++]);
				num3 += num4;
				num3 &= IdeaEngine.MASK;
				num3 = this.Mul(num3, workingKey[num++]);
				num4 += num3;
				num4 &= IdeaEngine.MASK;
				num2 ^= num3;
				num5 ^= num4;
				num3 ^= num7;
				num4 ^= num6;
			}
			this.WordToBytes(this.Mul(num2, workingKey[num++]), outBytes, outOff);
			this.WordToBytes(num4 + workingKey[num++], outBytes, outOff + 2);
			this.WordToBytes(num3 + workingKey[num++], outBytes, outOff + 4);
			this.WordToBytes(this.Mul(num5, workingKey[num]), outBytes, outOff + 6);
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x00143708 File Offset: 0x00141908
		private int[] ExpandKey(byte[] uKey)
		{
			int[] array = new int[52];
			if (uKey.Length < 16)
			{
				byte[] array2 = new byte[16];
				Array.Copy(uKey, 0, array2, array2.Length - uKey.Length, uKey.Length);
				uKey = array2;
			}
			for (int i = 0; i < 8; i++)
			{
				array[i] = this.BytesToWord(uKey, i * 2);
			}
			for (int j = 8; j < 52; j++)
			{
				if ((j & 7) < 6)
				{
					array[j] = (((array[j - 7] & 127) << 9 | array[j - 6] >> 7) & IdeaEngine.MASK);
				}
				else if ((j & 7) == 6)
				{
					array[j] = (((array[j - 7] & 127) << 9 | array[j - 14] >> 7) & IdeaEngine.MASK);
				}
				else
				{
					array[j] = (((array[j - 15] & 127) << 9 | array[j - 14] >> 7) & IdeaEngine.MASK);
				}
			}
			return array;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x001437D0 File Offset: 0x001419D0
		private int MulInv(int x)
		{
			if (x < 2)
			{
				return x;
			}
			int num = 1;
			int num2 = IdeaEngine.BASE / x;
			int num3 = IdeaEngine.BASE % x;
			while (num3 != 1)
			{
				int num4 = x / num3;
				x %= num3;
				num = (num + num2 * num4 & IdeaEngine.MASK);
				if (x == 1)
				{
					return num;
				}
				num4 = num3 / x;
				num3 %= x;
				num2 = (num2 + num * num4 & IdeaEngine.MASK);
			}
			return 1 - num2 & IdeaEngine.MASK;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x00143833 File Offset: 0x00141A33
		private int AddInv(int x)
		{
			return 0 - x & IdeaEngine.MASK;
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x00143840 File Offset: 0x00141A40
		private int[] InvertKey(int[] inKey)
		{
			int num = 52;
			int[] array = new int[52];
			int num2 = 0;
			int num3 = this.MulInv(inKey[num2++]);
			int num4 = this.AddInv(inKey[num2++]);
			int num5 = this.AddInv(inKey[num2++]);
			int num6 = this.MulInv(inKey[num2++]);
			array[--num] = num6;
			array[--num] = num5;
			array[--num] = num4;
			array[--num] = num3;
			for (int i = 1; i < 8; i++)
			{
				num3 = inKey[num2++];
				num4 = inKey[num2++];
				array[--num] = num4;
				array[--num] = num3;
				num3 = this.MulInv(inKey[num2++]);
				num4 = this.AddInv(inKey[num2++]);
				num5 = this.AddInv(inKey[num2++]);
				num6 = this.MulInv(inKey[num2++]);
				array[--num] = num6;
				array[--num] = num4;
				array[--num] = num5;
				array[--num] = num3;
			}
			num3 = inKey[num2++];
			num4 = inKey[num2++];
			array[--num] = num4;
			array[--num] = num3;
			num3 = this.MulInv(inKey[num2++]);
			num4 = this.AddInv(inKey[num2++]);
			num5 = this.AddInv(inKey[num2++]);
			num6 = this.MulInv(inKey[num2]);
			array[--num] = num6;
			array[--num] = num5;
			array[--num] = num4;
			array[num - 1] = num3;
			return array;
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x00143A08 File Offset: 0x00141C08
		private int[] GenerateWorkingKey(bool forEncryption, byte[] userKey)
		{
			if (forEncryption)
			{
				return this.ExpandKey(userKey);
			}
			return this.InvertKey(this.ExpandKey(userKey));
		}

		// Token: 0x040022A1 RID: 8865
		private const int BLOCK_SIZE = 8;

		// Token: 0x040022A2 RID: 8866
		private int[] workingKey;

		// Token: 0x040022A3 RID: 8867
		private static readonly int MASK = 65535;

		// Token: 0x040022A4 RID: 8868
		private static readonly int BASE = 65537;
	}
}
