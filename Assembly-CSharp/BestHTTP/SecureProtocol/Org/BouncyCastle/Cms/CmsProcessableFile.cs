using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F8 RID: 1528
	public class CmsProcessableFile : CmsProcessable, CmsReadable
	{
		// Token: 0x060039FF RID: 14847 RVA: 0x00168B25 File Offset: 0x00166D25
		public CmsProcessableFile(FileInfo file) : this(file, 32768)
		{
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x00168B33 File Offset: 0x00166D33
		public CmsProcessableFile(FileInfo file, int bufSize)
		{
			this._file = file;
			this._bufSize = bufSize;
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00168B49 File Offset: 0x00166D49
		public virtual Stream GetInputStream()
		{
			return new FileStream(this._file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, this._bufSize);
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x00168B64 File Offset: 0x00166D64
		public virtual void Write(Stream zOut)
		{
			Stream inputStream = this.GetInputStream();
			Streams.PipeAll(inputStream, zOut);
			Platform.Dispose(inputStream);
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x00168B78 File Offset: 0x00166D78
		[Obsolete]
		public virtual object GetContent()
		{
			return this._file;
		}

		// Token: 0x04002608 RID: 9736
		private const int DefaultBufSize = 32768;

		// Token: 0x04002609 RID: 9737
		private readonly FileInfo _file;

		// Token: 0x0400260A RID: 9738
		private readonly int _bufSize;
	}
}
