using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x0200050F RID: 1295
	public class X923Padding : IBlockCipherPadding
	{
		// Token: 0x0600310C RID: 12556 RVA: 0x00128B56 File Offset: 0x00126D56
		public void Init(SecureRandom random)
		{
			this.random = random;
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x00128B5F File Offset: 0x00126D5F
		public string PaddingName
		{
			get
			{
				return "X9.23";
			}
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x00128B68 File Offset: 0x00126D68
		public int AddPadding(byte[] input, int inOff)
		{
			byte b = (byte)(input.Length - inOff);
			while (inOff < input.Length - 1)
			{
				if (this.random == null)
				{
					input[inOff] = 0;
				}
				else
				{
					input[inOff] = (byte)this.random.NextInt();
				}
				inOff++;
			}
			input[inOff] = b;
			return (int)b;
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x0012864F File Offset: 0x0012684F
		public int PadCount(byte[] input)
		{
			byte b = input[input.Length - 1] & byte.MaxValue;
			if ((int)b > input.Length)
			{
				throw new InvalidCipherTextException("pad block corrupted");
			}
			return (int)b;
		}

		// Token: 0x04002050 RID: 8272
		private SecureRandom random;
	}
}
