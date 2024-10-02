using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x0200029A RID: 666
	public interface ISet : ICollection, IEnumerable
	{
		// Token: 0x06001823 RID: 6179
		void Add(object o);

		// Token: 0x06001824 RID: 6180
		void AddAll(IEnumerable e);

		// Token: 0x06001825 RID: 6181
		void Clear();

		// Token: 0x06001826 RID: 6182
		bool Contains(object o);

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001827 RID: 6183
		bool IsEmpty { get; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001828 RID: 6184
		bool IsFixedSize { get; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001829 RID: 6185
		bool IsReadOnly { get; }

		// Token: 0x0600182A RID: 6186
		void Remove(object o);

		// Token: 0x0600182B RID: 6187
		void RemoveAll(IEnumerable e);
	}
}
