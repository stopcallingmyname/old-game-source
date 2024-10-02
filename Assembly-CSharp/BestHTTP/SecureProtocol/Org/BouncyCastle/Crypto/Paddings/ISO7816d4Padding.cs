using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200050B RID: 1291
	public class ISO7816d4Padding : IBlockCipherPadding
	{
		// Token: 0x060030F5 RID: 12533 RVA: 0x0000248C File Offset: 0x0000068C
		public void Init(SecureRandom random)
		{
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x0012866F File Offset: 0x0012686F
		public string PaddingName
		{
			get
			{
				return "ISO7816-4";
			}
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x00128678 File Offset: 0x00126878
		public int AddPadding(byte[] input, int inOff)
		{
			int result = input.Length - inOff;
			input[inOff] = 128;
			for (inOff++; inOff < input.Length; inOff++)
			{
				input[inOff] = 0;
			}
			return result;
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x001286AC File Offset: 0x001268AC
		public int PadCount(byte[] input)
		{
			int num = input.Length - 1;
			while (num > 0 && input[num] == 0)
			{
				num--;
			}
			if (input[num] != 128)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			return input.Length - num;
		}
	}
}
