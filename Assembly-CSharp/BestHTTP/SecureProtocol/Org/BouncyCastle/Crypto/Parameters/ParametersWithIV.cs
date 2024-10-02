using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F4 RID: 1268
	public class ParametersWithIV : ICipherParameters
	{
		// Token: 0x06003088 RID: 12424 RVA: 0x00127A0A File Offset: 0x00125C0A
		public ParametersWithIV(ICipherParameters parameters, byte[] iv) : this(parameters, iv, 0, iv.Length)
		{
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x00127A18 File Offset: 0x00125C18
		public ParametersWithIV(ICipherParameters parameters, byte[] iv, int ivOff, int ivLen)
		{
			if (iv == null)
			{
				throw new ArgumentNullException("iv");
			}
			this.parameters = parameters;
			this.iv = Arrays.CopyOfRange(iv, ivOff, ivOff + ivLen);
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x00127A46 File Offset: 0x00125C46
		public byte[] GetIV()
		{
			return (byte[])this.iv.Clone();
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600308B RID: 12427 RVA: 0x00127A58 File Offset: 0x00125C58
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04002019 RID: 8217
		private readonly ICipherParameters parameters;

		// Token: 0x0400201A RID: 8218
		private readonly byte[] iv;
	}
}
