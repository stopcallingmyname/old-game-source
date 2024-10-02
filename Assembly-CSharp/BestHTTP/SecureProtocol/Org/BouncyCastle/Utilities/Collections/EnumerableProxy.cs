using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x02000298 RID: 664
	public sealed class EnumerableProxy : IEnumerable
	{
		// Token: 0x06001811 RID: 6161 RVA: 0x000B996F File Offset: 0x000B7B6F
		public EnumerableProxy(IEnumerable inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			this.inner = inner;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x000B998C File Offset: 0x000B7B8C
		public IEnumerator GetEnumerator()
		{
			return this.inner.GetEnumerator();
		}

		// Token: 0x0400182C RID: 6188
		private readonly IEnumerable inner;
	}
}
