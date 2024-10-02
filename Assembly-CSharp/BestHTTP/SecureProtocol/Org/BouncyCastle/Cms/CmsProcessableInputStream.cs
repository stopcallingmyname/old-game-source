using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F9 RID: 1529
	public class CmsProcessableInputStream : CmsProcessable, CmsReadable
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x00168B80 File Offset: 0x00166D80
		public CmsProcessableInputStream(Stream input)
		{
			this.input = input;
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x00168B8F File Offset: 0x00166D8F
		public virtual Stream GetInputStream()
		{
			this.CheckSingleUsage();
			return this.input;
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00168B9D File Offset: 0x00166D9D
		public virtual void Write(Stream output)
		{
			this.CheckSingleUsage();
			Streams.PipeAll(this.input, output);
			Platform.Dispose(this.input);
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00168BBC File Offset: 0x00166DBC
		[Obsolete]
		public virtual object GetContent()
		{
			return this.GetInputStream();
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x00168BC4 File Offset: 0x00166DC4
		protected virtual void CheckSingleUsage()
		{
			lock (this)
			{
				if (this.used)
				{
					throw new InvalidOperationException("CmsProcessableInputStream can only be used once");
				}
				this.used = true;
			}
		}

		// Token: 0x0400260B RID: 9739
		private readonly Stream input;

		// Token: 0x0400260C RID: 9740
		private bool used;
	}
}
