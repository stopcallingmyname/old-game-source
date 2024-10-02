using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004E7 RID: 1255
	public class HkdfParameters : IDerivationParameters
	{
		// Token: 0x06003055 RID: 12373 RVA: 0x001275A8 File Offset: 0x001257A8
		private HkdfParameters(byte[] ikm, bool skip, byte[] salt, byte[] info)
		{
			if (ikm == null)
			{
				throw new ArgumentNullException("ikm");
			}
			this.ikm = Arrays.Clone(ikm);
			this.skipExpand = skip;
			if (salt == null || salt.Length == 0)
			{
				this.salt = null;
			}
			else
			{
				this.salt = Arrays.Clone(salt);
			}
			if (info == null)
			{
				this.info = new byte[0];
				return;
			}
			this.info = Arrays.Clone(info);
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x00127616 File Offset: 0x00125816
		public HkdfParameters(byte[] ikm, byte[] salt, byte[] info) : this(ikm, false, salt, info)
		{
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x00127622 File Offset: 0x00125822
		public static HkdfParameters SkipExtractParameters(byte[] ikm, byte[] info)
		{
			return new HkdfParameters(ikm, true, null, info);
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x0012762D File Offset: 0x0012582D
		public static HkdfParameters DefaultParameters(byte[] ikm)
		{
			return new HkdfParameters(ikm, false, null, null);
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x00127638 File Offset: 0x00125838
		public virtual byte[] GetIkm()
		{
			return Arrays.Clone(this.ikm);
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x00127645 File Offset: 0x00125845
		public virtual bool SkipExtract
		{
			get
			{
				return this.skipExpand;
			}
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x0012764D File Offset: 0x0012584D
		public virtual byte[] GetSalt()
		{
			return Arrays.Clone(this.salt);
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x0012765A File Offset: 0x0012585A
		public virtual byte[] GetInfo()
		{
			return Arrays.Clone(this.info);
		}

		// Token: 0x04001FFE RID: 8190
		private readonly byte[] ikm;

		// Token: 0x04001FFF RID: 8191
		private readonly bool skipExpand;

		// Token: 0x04002000 RID: 8192
		private readonly byte[] salt;

		// Token: 0x04002001 RID: 8193
		private readonly byte[] info;
	}
}
