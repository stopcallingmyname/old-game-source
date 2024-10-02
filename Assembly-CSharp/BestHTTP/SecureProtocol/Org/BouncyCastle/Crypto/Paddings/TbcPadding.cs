using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200050E RID: 1294
	public class TbcPadding : IBlockCipherPadding
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x00128AC6 File Offset: 0x00126CC6
		public string PaddingName
		{
			get
			{
				return "TBC";
			}
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Init(SecureRandom random)
		{
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x00128AD0 File Offset: 0x00126CD0
		public virtual int AddPadding(byte[] input, int inOff)
		{
			int result = input.Length - inOff;
			byte b;
			if (inOff > 0)
			{
				b = (((input[inOff - 1] & 1) == 0) ? byte.MaxValue : 0);
			}
			else
			{
				b = (((input[input.Length - 1] & 1) == 0) ? byte.MaxValue : 0);
			}
			while (inOff < input.Length)
			{
				input[inOff] = b;
				inOff++;
			}
			return result;
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00128B24 File Offset: 0x00126D24
		public virtual int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1];
			int num = input.Length - 1;
			while (num > 0 && input[num - 1] == b)
			{
				num--;
			}
			return input.Length - num;
		}
	}
}
