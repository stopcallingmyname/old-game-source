using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F7 RID: 1527
	public class CmsProcessableByteArray : CmsProcessable, CmsReadable
	{
		// Token: 0x060039FB RID: 14843 RVA: 0x00168AE4 File Offset: 0x00166CE4
		public CmsProcessableByteArray(byte[] bytes)
		{
			this.bytes = bytes;
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x00168AF3 File Offset: 0x00166CF3
		public virtual Stream GetInputStream()
		{
			return new MemoryStream(this.bytes, false);
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x00168B01 File Offset: 0x00166D01
		public virtual void Write(Stream zOut)
		{
			zOut.Write(this.bytes, 0, this.bytes.Length);
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x00168B18 File Offset: 0x00166D18
		[Obsolete]
		public virtual object GetContent()
		{
			return this.bytes.Clone();
		}

		// Token: 0x04002607 RID: 9735
		private readonly byte[] bytes;
	}
}
