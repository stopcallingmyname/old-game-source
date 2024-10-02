using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO
{
	// Token: 0x02000540 RID: 1344
	public class MacSink : BaseOutputStream
	{
		// Token: 0x060032F3 RID: 13043 RVA: 0x00132973 File Offset: 0x00130B73
		public MacSink(IMac mac)
		{
			this.mMac = mac;
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060032F4 RID: 13044 RVA: 0x00132982 File Offset: 0x00130B82
		public virtual IMac Mac
		{
			get
			{
				return this.mMac;
			}
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0013298A File Offset: 0x00130B8A
		public override void WriteByte(byte b)
		{
			this.mMac.Update(b);
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x00132998 File Offset: 0x00130B98
		public override void Write(byte[] buf, int off, int len)
		{
			if (len > 0)
			{
				this.mMac.BlockUpdate(buf, off, len);
			}
		}

		// Token: 0x04002172 RID: 8562
		private readonly IMac mMac;
	}
}
