using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x02000510 RID: 1296
	public class ZeroBytePadding : IBlockCipherPadding
	{
		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06003111 RID: 12561 RVA: 0x00128BAD File Offset: 0x00126DAD
		public string PaddingName
		{
			get
			{
				return "ZeroBytePadding";
			}
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x0000248C File Offset: 0x0000068C
		public void Init(SecureRandom random)
		{
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x00128BB4 File Offset: 0x00126DB4
		public int AddPadding(byte[] input, int inOff)
		{
			int result = input.Length - inOff;
			while (inOff < input.Length)
			{
				input[inOff] = 0;
				inOff++;
			}
			return result;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x00128BDC File Offset: 0x00126DDC
		public int PadCount(byte[] input)
		{
			int num = input.Length;
			while (num > 0 && input[num - 1] == 0)
			{
				num--;
			}
			return input.Length - num;
		}
	}
}
