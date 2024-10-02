using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200050D RID: 1293
	public class Pkcs7Padding : IBlockCipherPadding
	{
		// Token: 0x06003102 RID: 12546 RVA: 0x0000248C File Offset: 0x0000068C
		public void Init(SecureRandom random)
		{
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x00128A48 File Offset: 0x00126C48
		public string PaddingName
		{
			get
			{
				return "PKCS7";
			}
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x00128A50 File Offset: 0x00126C50
		public int AddPadding(byte[] input, int inOff)
		{
			byte b = (byte)(input.Length - inOff);
			while (inOff < input.Length)
			{
				input[inOff] = b;
				inOff++;
			}
			return (int)b;
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x00128A78 File Offset: 0x00126C78
		public int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1];
			int num = (int)b;
			if (num < 1 || num > input.Length)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			for (int i = 2; i <= num; i++)
			{
				if (input[input.Length - i] != b)
				{
					throw new InvalidCipherTextException("pad block corrupted");
				}
			}
			return num;
		}
	}
}
