using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x02000296 RID: 662
	public sealed class EmptyEnumerable : IEnumerable
	{
		// Token: 0x06001809 RID: 6153 RVA: 0x00022F1F File Offset: 0x0002111F
		private EmptyEnumerable()
		{
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000B9944 File Offset: 0x000B7B44
		public IEnumerator GetEnumerator()
		{
			return EmptyEnumerator.Instance;
		}

		// Token: 0x0400182A RID: 6186
		public static readonly IEnumerable Instance = new EmptyEnumerable();
	}
}
