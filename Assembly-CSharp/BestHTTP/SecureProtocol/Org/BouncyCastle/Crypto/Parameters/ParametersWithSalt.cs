using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F6 RID: 1270
	public class ParametersWithSalt : ICipherParameters
	{
		// Token: 0x06003091 RID: 12433 RVA: 0x00127AB8 File Offset: 0x00125CB8
		public ParametersWithSalt(ICipherParameters parameters, byte[] salt) : this(parameters, salt, 0, salt.Length)
		{
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x00127AC6 File Offset: 0x00125CC6
		public ParametersWithSalt(ICipherParameters parameters, byte[] salt, int saltOff, int saltLen)
		{
			this.salt = new byte[saltLen];
			this.parameters = parameters;
			Array.Copy(salt, saltOff, this.salt, 0, saltLen);
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x00127AF2 File Offset: 0x00125CF2
		public byte[] GetSalt()
		{
			return this.salt;
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06003094 RID: 12436 RVA: 0x00127AFA File Offset: 0x00125CFA
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0400201D RID: 8221
		private byte[] salt;

		// Token: 0x0400201E RID: 8222
		private ICipherParameters parameters;
	}
}
