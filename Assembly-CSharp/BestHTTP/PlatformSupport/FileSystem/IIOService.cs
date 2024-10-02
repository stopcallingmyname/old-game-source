using System;
using System.IO;

namespace BestHTTP.PlatformSupport.FileSystem
{
	// Token: 0x020007D9 RID: 2009
	public interface IIOService
	{
		// Token: 0x0600476D RID: 18285
		void DirectoryCreate(string path);

		// Token: 0x0600476E RID: 18286
		bool DirectoryExists(string path);

		// Token: 0x0600476F RID: 18287
		string[] GetFiles(string path);

		// Token: 0x06004770 RID: 18288
		void FileDelete(string path);

		// Token: 0x06004771 RID: 18289
		bool FileExists(string path);

		// Token: 0x06004772 RID: 18290
		Stream CreateFileStream(string path, FileStreamModes mode);
	}
}
