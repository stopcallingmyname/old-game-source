using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200061A RID: 1562
	public class RecipientInformationStore
	{
		// Token: 0x06003B03 RID: 15107 RVA: 0x0016D05C File Offset: 0x0016B25C
		public RecipientInformationStore(ICollection recipientInfos)
		{
			foreach (object obj in recipientInfos)
			{
				RecipientInformation recipientInformation = (RecipientInformation)obj;
				RecipientID recipientID = recipientInformation.RecipientID;
				IList list = (IList)this.table[recipientID];
				if (list == null)
				{
					list = (this.table[recipientID] = Platform.CreateArrayList(1));
				}
				list.Add(recipientInformation);
			}
			this.all = Platform.CreateArrayList(recipientInfos);
		}

		// Token: 0x170007A1 RID: 1953
		public RecipientInformation this[RecipientID selector]
		{
			get
			{
				return this.GetFirstRecipient(selector);
			}
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x0016D110 File Offset: 0x0016B310
		public RecipientInformation GetFirstRecipient(RecipientID selector)
		{
			IList list = (IList)this.table[selector];
			if (list != null)
			{
				return (RecipientInformation)list[0];
			}
			return null;
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06003B06 RID: 15110 RVA: 0x0016D140 File Offset: 0x0016B340
		public int Count
		{
			get
			{
				return this.all.Count;
			}
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x0016D14D File Offset: 0x0016B34D
		public ICollection GetRecipients()
		{
			return Platform.CreateArrayList(this.all);
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0016D15C File Offset: 0x0016B35C
		public ICollection GetRecipients(RecipientID selector)
		{
			IList list = (IList)this.table[selector];
			if (list != null)
			{
				return Platform.CreateArrayList(list);
			}
			return Platform.CreateArrayList();
		}

		// Token: 0x04002678 RID: 9848
		private readonly IList all;

		// Token: 0x04002679 RID: 9849
		private readonly IDictionary table = Platform.CreateHashtable();
	}
}
