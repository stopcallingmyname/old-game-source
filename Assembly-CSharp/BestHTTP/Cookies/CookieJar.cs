using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BestHTTP.PlatformSupport.FileSystem;

namespace BestHTTP.Cookies
{
	// Token: 0x02000815 RID: 2069
	public static class CookieJar
	{
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06004997 RID: 18839 RVA: 0x001A2310 File Offset: 0x001A0510
		public static bool IsSavingSupported
		{
			get
			{
				if (CookieJar.IsSupportCheckDone)
				{
					return CookieJar._isSavingSupported;
				}
				try
				{
					HTTPManager.IOService.DirectoryExists(HTTPManager.GetRootCacheFolder());
					CookieJar._isSavingSupported = true;
				}
				catch
				{
					CookieJar._isSavingSupported = false;
					HTTPManager.Logger.Warning("CookieJar", "Cookie saving and loading disabled!");
				}
				finally
				{
					CookieJar.IsSupportCheckDone = true;
				}
				return CookieJar._isSavingSupported;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06004998 RID: 18840 RVA: 0x001A2388 File Offset: 0x001A0588
		// (set) Token: 0x06004999 RID: 18841 RVA: 0x001A238F File Offset: 0x001A058F
		private static string CookieFolder { get; set; }

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x0600499A RID: 18842 RVA: 0x001A2397 File Offset: 0x001A0597
		// (set) Token: 0x0600499B RID: 18843 RVA: 0x001A239E File Offset: 0x001A059E
		private static string LibraryPath { get; set; }

		// Token: 0x0600499C RID: 18844 RVA: 0x001A23A8 File Offset: 0x001A05A8
		internal static void SetupFolder()
		{
			if (!CookieJar.IsSavingSupported)
			{
				return;
			}
			try
			{
				if (string.IsNullOrEmpty(CookieJar.CookieFolder) || string.IsNullOrEmpty(CookieJar.LibraryPath))
				{
					CookieJar.CookieFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "Cookies");
					CookieJar.LibraryPath = Path.Combine(CookieJar.CookieFolder, "Library");
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x001A2414 File Offset: 0x001A0614
		internal static void Set(HTTPResponse response)
		{
			if (response == null)
			{
				return;
			}
			List<Cookie> list = new List<Cookie>();
			List<string> headerValues = response.GetHeaderValues("set-cookie");
			if (headerValues == null)
			{
				return;
			}
			foreach (string header in headerValues)
			{
				Cookie cookie = Cookie.Parse(header, response.baseRequest.CurrentUri);
				if (cookie != null)
				{
					CookieJar.rwLock.EnterWriteLock();
					try
					{
						int num;
						Cookie cookie2 = CookieJar.Find(cookie, out num);
						if (!string.IsNullOrEmpty(cookie.Value) && cookie.WillExpireInTheFuture())
						{
							if (cookie2 == null)
							{
								CookieJar.Cookies.Add(cookie);
								list.Add(cookie);
							}
							else
							{
								cookie.Date = cookie2.Date;
								CookieJar.Cookies[num] = cookie;
								list.Add(cookie);
							}
						}
						else if (num != -1)
						{
							CookieJar.Cookies.RemoveAt(num);
						}
					}
					catch
					{
					}
					finally
					{
						CookieJar.rwLock.ExitWriteLock();
					}
				}
			}
			response.Cookies = list;
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x001A253C File Offset: 0x001A073C
		internal static void Maintain()
		{
			CookieJar.rwLock.EnterWriteLock();
			try
			{
				uint num = 0U;
				int i = 0;
				while (i < CookieJar.Cookies.Count)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (!cookie.WillExpireInTheFuture() || cookie.LastAccess + CookieJar.AccessThreshold < DateTime.UtcNow)
					{
						CookieJar.Cookies.RemoveAt(i);
					}
					else
					{
						if (!cookie.IsSession)
						{
							num += cookie.GuessSize();
						}
						i++;
					}
				}
				if (num > HTTPManager.CookieJarSize)
				{
					CookieJar.Cookies.Sort();
					while (num > HTTPManager.CookieJarSize && CookieJar.Cookies.Count > 0)
					{
						Cookie cookie2 = CookieJar.Cookies[0];
						CookieJar.Cookies.RemoveAt(0);
						num -= cookie2.GuessSize();
					}
				}
			}
			catch
			{
			}
			finally
			{
				CookieJar.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x001A2630 File Offset: 0x001A0830
		internal static void Persist()
		{
			if (!CookieJar.IsSavingSupported)
			{
				return;
			}
			if (!CookieJar.Loaded)
			{
				return;
			}
			CookieJar.Maintain();
			CookieJar.rwLock.EnterReadLock();
			try
			{
				if (!HTTPManager.IOService.DirectoryExists(CookieJar.CookieFolder))
				{
					HTTPManager.IOService.DirectoryCreate(CookieJar.CookieFolder);
				}
				using (Stream stream = HTTPManager.IOService.CreateFileStream(CookieJar.LibraryPath, FileStreamModes.Create))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(stream))
					{
						binaryWriter.Write(1);
						int num = 0;
						using (List<Cookie>.Enumerator enumerator = CookieJar.Cookies.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (!enumerator.Current.IsSession)
								{
									num++;
								}
							}
						}
						binaryWriter.Write(num);
						foreach (Cookie cookie in CookieJar.Cookies)
						{
							if (!cookie.IsSession)
							{
								cookie.SaveTo(binaryWriter);
							}
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				CookieJar.rwLock.ExitReadLock();
			}
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x001A2790 File Offset: 0x001A0990
		internal static void Load()
		{
			if (!CookieJar.IsSavingSupported)
			{
				return;
			}
			if (CookieJar.Loaded)
			{
				return;
			}
			CookieJar.SetupFolder();
			CookieJar.rwLock.EnterWriteLock();
			try
			{
				CookieJar.Cookies.Clear();
				if (!HTTPManager.IOService.DirectoryExists(CookieJar.CookieFolder))
				{
					HTTPManager.IOService.DirectoryCreate(CookieJar.CookieFolder);
				}
				if (HTTPManager.IOService.FileExists(CookieJar.LibraryPath))
				{
					using (Stream stream = HTTPManager.IOService.CreateFileStream(CookieJar.LibraryPath, FileStreamModes.Open))
					{
						using (BinaryReader binaryReader = new BinaryReader(stream))
						{
							binaryReader.ReadInt32();
							int num = binaryReader.ReadInt32();
							for (int i = 0; i < num; i++)
							{
								Cookie cookie = new Cookie();
								cookie.LoadFrom(binaryReader);
								if (cookie.WillExpireInTheFuture())
								{
									CookieJar.Cookies.Add(cookie);
								}
							}
						}
					}
				}
			}
			catch
			{
				CookieJar.Cookies.Clear();
			}
			finally
			{
				CookieJar.Loaded = true;
				CookieJar.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x001A28C0 File Offset: 0x001A0AC0
		public static List<Cookie> Get(Uri uri)
		{
			CookieJar.rwLock.EnterReadLock();
			List<Cookie> result;
			try
			{
				CookieJar.Load();
				List<Cookie> list = null;
				for (int i = 0; i < CookieJar.Cookies.Count; i++)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (cookie.WillExpireInTheFuture() && uri.Host.IndexOf(cookie.Domain) != -1 && uri.AbsolutePath.StartsWith(cookie.Path))
					{
						if (list == null)
						{
							list = new List<Cookie>();
						}
						list.Add(cookie);
					}
				}
				result = list;
			}
			finally
			{
				CookieJar.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x001A2960 File Offset: 0x001A0B60
		public static void Set(Uri uri, Cookie cookie)
		{
			CookieJar.Set(cookie);
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x001A2968 File Offset: 0x001A0B68
		public static void Set(Cookie cookie)
		{
			CookieJar.Load();
			CookieJar.rwLock.EnterWriteLock();
			try
			{
				int num;
				CookieJar.Find(cookie, out num);
				if (num >= 0)
				{
					CookieJar.Cookies[num] = cookie;
				}
				else
				{
					CookieJar.Cookies.Add(cookie);
				}
			}
			finally
			{
				CookieJar.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x001A29C8 File Offset: 0x001A0BC8
		public static List<Cookie> GetAll()
		{
			CookieJar.Load();
			return CookieJar.Cookies;
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x001A29D4 File Offset: 0x001A0BD4
		public static void Clear()
		{
			CookieJar.Load();
			CookieJar.rwLock.EnterWriteLock();
			try
			{
				CookieJar.Cookies.Clear();
			}
			finally
			{
				CookieJar.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x001A2A18 File Offset: 0x001A0C18
		public static void Clear(TimeSpan olderThan)
		{
			CookieJar.Load();
			CookieJar.rwLock.EnterWriteLock();
			try
			{
				int i = 0;
				while (i < CookieJar.Cookies.Count)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (!cookie.WillExpireInTheFuture() || cookie.Date + olderThan < DateTime.UtcNow)
					{
						CookieJar.Cookies.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			finally
			{
				CookieJar.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x001A2AA0 File Offset: 0x001A0CA0
		public static void Clear(string domain)
		{
			CookieJar.Load();
			CookieJar.rwLock.EnterWriteLock();
			try
			{
				int i = 0;
				while (i < CookieJar.Cookies.Count)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (!cookie.WillExpireInTheFuture() || cookie.Domain.IndexOf(domain) != -1)
					{
						CookieJar.Cookies.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			finally
			{
				CookieJar.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x001A2B20 File Offset: 0x001A0D20
		public static void Remove(Uri uri, string name)
		{
			CookieJar.Load();
			CookieJar.rwLock.EnterWriteLock();
			try
			{
				int i = 0;
				while (i < CookieJar.Cookies.Count)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (cookie.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && uri.Host.IndexOf(cookie.Domain) != -1)
					{
						CookieJar.Cookies.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			finally
			{
				CookieJar.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x001A2BAC File Offset: 0x001A0DAC
		private static Cookie Find(Cookie cookie, out int idx)
		{
			for (int i = 0; i < CookieJar.Cookies.Count; i++)
			{
				Cookie cookie2 = CookieJar.Cookies[i];
				if (cookie2.Equals(cookie))
				{
					idx = i;
					return cookie2;
				}
			}
			idx = -1;
			return null;
		}

		// Token: 0x0400305D RID: 12381
		private const int Version = 1;

		// Token: 0x0400305E RID: 12382
		public static TimeSpan AccessThreshold = TimeSpan.FromDays(7.0);

		// Token: 0x0400305F RID: 12383
		private static List<Cookie> Cookies = new List<Cookie>();

		// Token: 0x04003062 RID: 12386
		private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		// Token: 0x04003063 RID: 12387
		private static bool _isSavingSupported;

		// Token: 0x04003064 RID: 12388
		private static bool IsSupportCheckDone;

		// Token: 0x04003065 RID: 12389
		private static bool Loaded;
	}
}
