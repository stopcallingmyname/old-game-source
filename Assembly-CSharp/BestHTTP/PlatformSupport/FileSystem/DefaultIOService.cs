using System;
using System.IO;
using BestHTTP.Logger;

namespace BestHTTP.PlatformSupport.FileSystem
{
	// Token: 0x020007D7 RID: 2007
	public sealed class DefaultIOService : IIOService
	{
		// Token: 0x06004766 RID: 18278 RVA: 0x001960DC File Offset: 0x001942DC
		public Stream CreateFileStream(string path, FileStreamModes mode)
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("DefaultIOService", string.Format("CreateFileStream path: '{0}' mode: {1}", path, mode));
			}
			switch (mode)
			{
			case FileStreamModes.Create:
				return new FileStream(path, FileMode.Create);
			case FileStreamModes.Open:
				return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			case FileStreamModes.Append:
				return new FileStream(path, FileMode.Append);
			default:
				throw new NotImplementedException("DefaultIOService.CreateFileStream - mode not implemented: " + mode.ToString());
			}
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0019615F File Offset: 0x0019435F
		public void DirectoryCreate(string path)
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("DefaultIOService", string.Format("DirectoryCreate path: '{0}'", path));
			}
			Directory.CreateDirectory(path);
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x00196190 File Offset: 0x00194390
		public bool DirectoryExists(string path)
		{
			bool flag = Directory.Exists(path);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("DefaultIOService", string.Format("DirectoryExists path: '{0}' exists: {1}", path, flag));
			}
			return flag;
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x001961D4 File Offset: 0x001943D4
		public string[] GetFiles(string path)
		{
			string[] files = Directory.GetFiles(path);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("DefaultIOService", string.Format("GetFiles path: '{0}' files count: {1}", path, files.Length));
			}
			return files;
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x00196217 File Offset: 0x00194417
		public void FileDelete(string path)
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("DefaultIOService", string.Format("FileDelete path: '{0}'", path));
			}
			File.Delete(path);
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00196248 File Offset: 0x00194448
		public bool FileExists(string path)
		{
			bool flag = File.Exists(path);
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Verbose("DefaultIOService", string.Format("FileExists path: '{0}' exists: {1}", path, flag));
			}
			return flag;
		}
	}
}
