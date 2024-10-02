using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C0 RID: 1216
	public class AeadParameters : ICipherParameters
	{
		// Token: 0x06002F61 RID: 12129 RVA: 0x001256FA File Offset: 0x001238FA
		public AeadParameters(KeyParameter key, int macSize, byte[] nonce) : this(key, macSize, nonce, null)
		{
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x00125706 File Offset: 0x00123906
		public AeadParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText)
		{
			this.key = key;
			this.nonce = nonce;
			this.macSize = macSize;
			this.associatedText = associatedText;
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x0012572B File Offset: 0x0012392B
		public virtual KeyParameter Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002F64 RID: 12132 RVA: 0x00125733 File Offset: 0x00123933
		public virtual int MacSize
		{
			get
			{
				return this.macSize;
			}
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x0012573B File Offset: 0x0012393B
		public virtual byte[] GetAssociatedText()
		{
			return this.associatedText;
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x00125743 File Offset: 0x00123943
		public virtual byte[] GetNonce()
		{
			return this.nonce;
		}

		// Token: 0x04001FA8 RID: 8104
		private readonly byte[] associatedText;

		// Token: 0x04001FA9 RID: 8105
		private readonly byte[] nonce;

		// Token: 0x04001FAA RID: 8106
		private readonly KeyParameter key;

		// Token: 0x04001FAB RID: 8107
		private readonly int macSize;
	}
}
