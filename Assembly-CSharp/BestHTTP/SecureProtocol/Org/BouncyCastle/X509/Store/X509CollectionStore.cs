using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x02000258 RID: 600
	internal class X509CollectionStore : IX509Store
	{
		// Token: 0x060015F0 RID: 5616 RVA: 0x000AF6CF File Offset: 0x000AD8CF
		internal X509CollectionStore(ICollection collection)
		{
			this._local = Platform.CreateArrayList(collection);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000AF6E4 File Offset: 0x000AD8E4
		public ICollection GetMatches(IX509Selector selector)
		{
			if (selector == null)
			{
				return Platform.CreateArrayList(this._local);
			}
			IList list = Platform.CreateArrayList();
			foreach (object obj in this._local)
			{
				if (selector.Match(obj))
				{
					list.Add(obj);
				}
			}
			return list;
		}

		// Token: 0x04001676 RID: 5750
		private ICollection _local;
	}
}
