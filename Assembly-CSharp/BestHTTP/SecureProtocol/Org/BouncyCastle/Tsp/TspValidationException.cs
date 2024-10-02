using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002AE RID: 686
	[Serializable]
	public class TspValidationException : TspException
	{
		// Token: 0x060018FE RID: 6398 RVA: 0x000BB960 File Offset: 0x000B9B60
		public TspValidationException(string message) : base(message)
		{
			this.failureCode = -1;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x000BB970 File Offset: 0x000B9B70
		public TspValidationException(string message, int failureCode) : base(message)
		{
			this.failureCode = failureCode;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x000BB980 File Offset: 0x000B9B80
		public int FailureCode
		{
			get
			{
				return this.failureCode;
			}
		}

		// Token: 0x0400186A RID: 6250
		private int failureCode;
	}
}
