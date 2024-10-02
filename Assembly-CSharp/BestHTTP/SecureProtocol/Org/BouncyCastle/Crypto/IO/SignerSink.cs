using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO
{
	// Token: 0x02000542 RID: 1346
	public class SignerSink : BaseOutputStream
	{
		// Token: 0x06003308 RID: 13064 RVA: 0x00132B1F File Offset: 0x00130D1F
		public SignerSink(ISigner signer)
		{
			this.mSigner = signer;
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x00132B2E File Offset: 0x00130D2E
		public virtual ISigner Signer
		{
			get
			{
				return this.mSigner;
			}
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x00132B36 File Offset: 0x00130D36
		public override void WriteByte(byte b)
		{
			this.mSigner.Update(b);
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00132B44 File Offset: 0x00130D44
		public override void Write(byte[] buf, int off, int len)
		{
			if (len > 0)
			{
				this.mSigner.BlockUpdate(buf, off, len);
			}
		}

		// Token: 0x04002176 RID: 8566
		private readonly ISigner mSigner;
	}
}
