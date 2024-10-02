using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x02000297 RID: 663
	public sealed class EmptyEnumerator : IEnumerator
	{
		// Token: 0x0600180C RID: 6156 RVA: 0x00022F1F File Offset: 0x0002111F
		private EmptyEnumerator()
		{
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0000248C File Offset: 0x0000068C
		public void Reset()
		{
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x000B9957 File Offset: 0x000B7B57
		public object Current
		{
			get
			{
				throw new InvalidOperationException("No elements");
			}
		}

		// Token: 0x0400182B RID: 6187
		public static readonly IEnumerator Instance = new EmptyEnumerator();
	}
}
