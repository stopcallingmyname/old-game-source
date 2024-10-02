using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000620 RID: 1568
	public class SignerInformationStore
	{
		// Token: 0x06003B36 RID: 15158 RVA: 0x0016E048 File Offset: 0x0016C248
		public SignerInformationStore(SignerInformation signerInfo)
		{
			this.all = Platform.CreateArrayList(1);
			this.all.Add(signerInfo);
			SignerID signerID = signerInfo.SignerID;
			this.table[signerID] = this.all;
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0016E098 File Offset: 0x0016C298
		public SignerInformationStore(ICollection signerInfos)
		{
			foreach (object obj in signerInfos)
			{
				SignerInformation signerInformation = (SignerInformation)obj;
				SignerID signerID = signerInformation.SignerID;
				IList list = (IList)this.table[signerID];
				if (list == null)
				{
					list = (this.table[signerID] = Platform.CreateArrayList(1));
				}
				list.Add(signerInformation);
			}
			this.all = Platform.CreateArrayList(signerInfos);
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0016E140 File Offset: 0x0016C340
		public SignerInformation GetFirstSigner(SignerID selector)
		{
			IList list = (IList)this.table[selector];
			if (list != null)
			{
				return (SignerInformation)list[0];
			}
			return null;
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x0016E170 File Offset: 0x0016C370
		public int Count
		{
			get
			{
				return this.all.Count;
			}
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x0016E17D File Offset: 0x0016C37D
		public ICollection GetSigners()
		{
			return Platform.CreateArrayList(this.all);
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x0016E18C File Offset: 0x0016C38C
		public ICollection GetSigners(SignerID selector)
		{
			IList list = (IList)this.table[selector];
			if (list != null)
			{
				return Platform.CreateArrayList(list);
			}
			return Platform.CreateArrayList();
		}

		// Token: 0x04002692 RID: 9874
		private readonly IList all;

		// Token: 0x04002693 RID: 9875
		private readonly IDictionary table = Platform.CreateHashtable();
	}
}
