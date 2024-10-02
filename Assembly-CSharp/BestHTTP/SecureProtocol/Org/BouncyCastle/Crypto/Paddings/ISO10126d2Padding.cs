using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200050A RID: 1290
	public class ISO10126d2Padding : IBlockCipherPadding
	{
		// Token: 0x060030F0 RID: 12528 RVA: 0x001285FC File Offset: 0x001267FC
		public void Init(SecureRandom random)
		{
			this.random = ((random != null) ? random : new SecureRandom());
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060030F1 RID: 12529 RVA: 0x0012860F File Offset: 0x0012680F
		public string PaddingName
		{
			get
			{
				return "ISO10126-2";
			}
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x00128618 File Offset: 0x00126818
		public int AddPadding(byte[] input, int inOff)
		{
			byte b = (byte)(input.Length - inOff);
			while (inOff < input.Length - 1)
			{
				input[inOff] = (byte)this.random.NextInt();
				inOff++;
			}
			input[inOff] = b;
			return (int)b;
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x0012864F File Offset: 0x0012684F
		public int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1] & byte.MaxValue;
			if ((int)b > input.Length)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			return (int)b;
		}

		// Token: 0x0400204E RID: 8270
		private SecureRandom random;
	}
}
