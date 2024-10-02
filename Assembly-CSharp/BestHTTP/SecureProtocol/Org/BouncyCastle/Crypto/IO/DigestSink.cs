using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO
{
	// Token: 0x0200053E RID: 1342
	public class DigestSink : BaseOutputStream
	{
		// Token: 0x060032DE RID: 13022 RVA: 0x001327CA File Offset: 0x001309CA
		public DigestSink(IDigest digest)
		{
			this.mDigest = digest;
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x001327D9 File Offset: 0x001309D9
		public virtual IDigest Digest
		{
			get
			{
				return this.mDigest;
			}
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x001327E1 File Offset: 0x001309E1
		public override void WriteByte(byte b)
		{
			this.mDigest.Update(b);
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x001327EF File Offset: 0x001309EF
		public override void Write(byte[] buf, int off, int len)
		{
			if (len > 0)
			{
				this.mDigest.BlockUpdate(buf, off, len);
			}
		}

		// Token: 0x0400216E RID: 8558
		private readonly IDigest mDigest;
	}
}
