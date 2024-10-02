using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x0200028E RID: 654
	public class HexTranslator : ITranslator
	{
		// Token: 0x060017E3 RID: 6115 RVA: 0x000A7398 File Offset: 0x000A5598
		public int GetEncodedBlockSize()
		{
			return 2;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000B94C8 File Offset: 0x000B76C8
		public int Encode(byte[] input, int inOff, int length, byte[] outBytes, int outOff)
		{
			int i = 0;
			int num = 0;
			while (i < length)
			{
				outBytes[outOff + num] = HexTranslator.hexTable[input[inOff] >> 4 & 15];
				outBytes[outOff + num + 1] = HexTranslator.hexTable[(int)(input[inOff] & 15)];
				inOff++;
				i++;
				num += 2;
			}
			return length * 2;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0006AE98 File Offset: 0x00069098
		public int GetDecodedBlockSize()
		{
			return 1;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000B951C File Offset: 0x000B771C
		public int Decode(byte[] input, int inOff, int length, byte[] outBytes, int outOff)
		{
			int num = length / 2;
			for (int i = 0; i < num; i++)
			{
				byte b = input[inOff + i * 2];
				byte b2 = input[inOff + i * 2 + 1];
				if (b < 97)
				{
					outBytes[outOff] = (byte)(b - 48 << 4);
				}
				else
				{
					outBytes[outOff] = (byte)(b - 97 + 10 << 4);
				}
				if (b2 < 97)
				{
					int num2 = outOff;
					outBytes[num2] += b2 - 48;
				}
				else
				{
					int num3 = outOff;
					outBytes[num3] += b2 - 97 + 10;
				}
				outOff++;
			}
			return num;
		}

		// Token: 0x04001826 RID: 6182
		private static readonly byte[] hexTable = new byte[]
		{
			48,
			49,
			50,
			51,
			52,
			53,
			54,
			55,
			56,
			57,
			97,
			98,
			99,
			100,
			101,
			102
		};
	}
}
