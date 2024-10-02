using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002C6 RID: 710
	public class AsymmetricKeyEntry : Pkcs12Entry
	{
		// Token: 0x06001A22 RID: 6690 RVA: 0x000C3FF9 File Offset: 0x000C21F9
		public AsymmetricKeyEntry(AsymmetricKeyParameter key) : base(Platform.CreateHashtable())
		{
			this.key = key;
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000C400D File Offset: 0x000C220D
		[Obsolete]
		public AsymmetricKeyEntry(AsymmetricKeyParameter key, Hashtable attributes) : base(attributes)
		{
			this.key = key;
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x000C400D File Offset: 0x000C220D
		public AsymmetricKeyEntry(AsymmetricKeyParameter key, IDictionary attributes) : base(attributes)
		{
			this.key = key;
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x000C401D File Offset: 0x000C221D
		public AsymmetricKeyParameter Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000C4028 File Offset: 0x000C2228
		public override bool Equals(object obj)
		{
			AsymmetricKeyEntry asymmetricKeyEntry = obj as AsymmetricKeyEntry;
			return asymmetricKeyEntry != null && this.key.Equals(asymmetricKeyEntry.key);
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x000C4052 File Offset: 0x000C2252
		public override int GetHashCode()
		{
			return ~this.key.GetHashCode();
		}

		// Token: 0x040018B6 RID: 6326
		private readonly AsymmetricKeyParameter key;
	}
}
