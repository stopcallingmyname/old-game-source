using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000673 RID: 1651
	public class OidTokenizer
	{
		// Token: 0x06003D5F RID: 15711 RVA: 0x00173B4E File Offset: 0x00171D4E
		public OidTokenizer(string oid)
		{
			this.oid = oid;
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06003D60 RID: 15712 RVA: 0x00173B5D File Offset: 0x00171D5D
		public bool HasMoreTokens
		{
			get
			{
				return this.index != -1;
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00173B6C File Offset: 0x00171D6C
		public string NextToken()
		{
			if (this.index == -1)
			{
				return null;
			}
			int num = this.oid.IndexOf('.', this.index);
			if (num == -1)
			{
				string result = this.oid.Substring(this.index);
				this.index = -1;
				return result;
			}
			string result2 = this.oid.Substring(this.index, num - this.index);
			this.index = num + 1;
			return result2;
		}

		// Token: 0x0400270D RID: 9997
		private string oid;

		// Token: 0x0400270E RID: 9998
		private int index;
	}
}
