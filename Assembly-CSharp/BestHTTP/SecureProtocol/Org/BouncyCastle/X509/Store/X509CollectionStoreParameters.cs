using System;
using System.Collections;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x02000259 RID: 601
	public class X509CollectionStoreParameters : IX509StoreParameters
	{
		// Token: 0x060015F2 RID: 5618 RVA: 0x000AF758 File Offset: 0x000AD958
		public X509CollectionStoreParameters(ICollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.collection = Platform.CreateArrayList(collection);
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x000AF77A File Offset: 0x000AD97A
		public ICollection GetCollection()
		{
			return Platform.CreateArrayList(this.collection);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000AF787 File Offset: 0x000AD987
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("X509CollectionStoreParameters: [\n");
			stringBuilder.Append("  collection: " + this.collection + "\n");
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x04001677 RID: 5751
		private readonly IList collection;
	}
}
