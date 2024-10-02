using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F5 RID: 1269
	public class ParametersWithRandom : ICipherParameters
	{
		// Token: 0x0600308C RID: 12428 RVA: 0x00127A60 File Offset: 0x00125C60
		public ParametersWithRandom(ICipherParameters parameters, SecureRandom random)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			this.parameters = parameters;
			this.random = random;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x00127A92 File Offset: 0x00125C92
		public ParametersWithRandom(ICipherParameters parameters) : this(parameters, new SecureRandom())
		{
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x00127AA0 File Offset: 0x00125CA0
		[Obsolete("Use Random property instead")]
		public SecureRandom GetRandom()
		{
			return this.Random;
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x00127AA8 File Offset: 0x00125CA8
		public SecureRandom Random
		{
			get
			{
				return this.random;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x00127AB0 File Offset: 0x00125CB0
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0400201B RID: 8219
		private readonly ICipherParameters parameters;

		// Token: 0x0400201C RID: 8220
		private readonly SecureRandom random;
	}
}
