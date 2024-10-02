using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using BestHTTP.Logger;
using BestHTTP.PlatformSupport.FileSystem;
using BestHTTP.PlatformSupport.Threading;

namespace BestHTTP.Caching
{
	// Token: 0x02000819 RID: 2073
	public static class HTTPCacheService
	{
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060049DD RID: 18909 RVA: 0x001A358C File Offset: 0x001A178C
		public static bool IsSupported
		{
			get
			{
				if (HTTPCacheService.IsSupportCheckDone)
				{
					return HTTPCacheService.isSupported;
				}
				try
				{
					HTTPManager.IOService.DirectoryExists(HTTPManager.GetRootCacheFolder());
					HTTPCacheService.isSupported = true;
				}
				catch
				{
					HTTPCacheService.isSupported = false;
					HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
				}
				finally
				{
					HTTPCacheService.IsSupportCheckDone = true;
				}
				return HTTPCacheService.isSupported;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x001A3604 File Offset: 0x001A1804
		// (set) Token: 0x060049DF RID: 18911 RVA: 0x001A360B File Offset: 0x001A180B
		internal static string CacheFolder { get; private set; }

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060049E0 RID: 18912 RVA: 0x001A3613 File Offset: 0x001A1813
		// (set) Token: 0x060049E1 RID: 18913 RVA: 0x001A361A File Offset: 0x001A181A
		private static string LibraryPath { get; set; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060049E2 RID: 18914 RVA: 0x001A3622 File Offset: 0x001A1822
		public static bool IsDoingMaintainence
		{
			get
			{
				return HTTPCacheService.InClearThread || HTTPCacheService.InMaintainenceThread;
			}
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x001A3654 File Offset: 0x001A1854
		internal static void CheckSetup()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			try
			{
				HTTPCacheService.SetupCacheFolder();
				HTTPCacheService.LoadLibrary();
			}
			catch
			{
			}
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x001A368C File Offset: 0x001A188C
		internal static void SetupCacheFolder()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			try
			{
				if (string.IsNullOrEmpty(HTTPCacheService.CacheFolder) || string.IsNullOrEmpty(HTTPCacheService.LibraryPath))
				{
					HTTPCacheService.CacheFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "HTTPCache");
					if (!HTTPManager.IOService.DirectoryExists(HTTPCacheService.CacheFolder))
					{
						HTTPManager.IOService.DirectoryCreate(HTTPCacheService.CacheFolder);
					}
					HTTPCacheService.LibraryPath = Path.Combine(HTTPManager.GetRootCacheFolder(), "Library");
				}
			}
			catch
			{
				HTTPCacheService.isSupported = false;
				HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
			}
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x001A3734 File Offset: 0x001A1934
		internal static ulong GetNameIdx()
		{
			ulong nextNameIDX = HTTPCacheService.NextNameIDX;
			do
			{
				HTTPCacheService.NextNameIDX = (HTTPCacheService.NextNameIDX += 1UL) % ulong.MaxValue;
			}
			while (HTTPCacheService.UsedIndexes.ContainsKey(HTTPCacheService.NextNameIDX));
			return nextNameIDX;
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x001A3770 File Offset: 0x001A1970
		internal static bool HasEntity(Uri uri)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterReadLock();
			bool result;
			try
			{
				result = HTTPCacheService.library.ContainsKey(uri);
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x001A37C0 File Offset: 0x001A19C0
		public static bool DeleteEntity(Uri uri, bool removeFromLibrary = true)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterUpgradeableReadLock();
			bool result;
			try
			{
				HTTPCacheFileInfo httpcacheFileInfo;
				bool flag = HTTPCacheService.library.TryGetValue(uri, out httpcacheFileInfo);
				if (flag)
				{
					httpcacheFileInfo.Delete();
				}
				if (flag && removeFromLibrary)
				{
					HTTPCacheService.rwLock.EnterWriteLock();
					HTTPCacheService.library.Remove(uri);
					HTTPCacheService.UsedIndexes.Remove(httpcacheFileInfo.MappedNameIDX);
					HTTPCacheService.rwLock.ExitWriteLock();
				}
				result = true;
			}
			finally
			{
				HTTPCacheService.rwLock.ExitUpgradeableReadLock();
			}
			return result;
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x001A3850 File Offset: 0x001A1A50
		internal static bool IsCachedEntityExpiresInTheFuture(HTTPRequest request)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheFileInfo httpcacheFileInfo = null;
			HTTPCacheService.rwLock.EnterReadLock();
			try
			{
				if (!HTTPCacheService.library.TryGetValue(request.CurrentUri, out httpcacheFileInfo))
				{
					return false;
				}
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
			return httpcacheFileInfo.WillExpireInTheFuture();
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x001A38B4 File Offset: 0x001A1AB4
		internal static void SetHeaders(HTTPRequest request)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			HTTPCacheService.CheckSetup();
			request.RemoveHeader("If-None-Match");
			request.RemoveHeader("If-Modified-Since");
			HTTPCacheFileInfo httpcacheFileInfo = null;
			HTTPCacheService.rwLock.EnterReadLock();
			try
			{
				if (!HTTPCacheService.library.TryGetValue(request.CurrentUri, out httpcacheFileInfo))
				{
					return;
				}
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
			httpcacheFileInfo.SetUpRevalidationHeaders(request);
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x001A392C File Offset: 0x001A1B2C
		internal static HTTPCacheFileInfo GetEntity(Uri uri)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheFileInfo result = null;
			HTTPCacheService.rwLock.EnterReadLock();
			try
			{
				HTTPCacheService.library.TryGetValue(uri, out result);
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x001A3980 File Offset: 0x001A1B80
		internal static HTTPResponse GetFullResponse(HTTPRequest request)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheFileInfo httpcacheFileInfo = null;
			HTTPCacheService.rwLock.EnterReadLock();
			try
			{
				if (!HTTPCacheService.library.TryGetValue(request.CurrentUri, out httpcacheFileInfo))
				{
					return null;
				}
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
			return httpcacheFileInfo.ReadResponseTo(request);
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x001A39E8 File Offset: 0x001A1BE8
		internal static bool IsCacheble(Uri uri, HTTPMethods method, HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			if (method != HTTPMethods.Get)
			{
				return false;
			}
			if (response == null)
			{
				return false;
			}
			if (response.StatusCode < 200 || response.StatusCode >= 400)
			{
				return false;
			}
			List<string> headerValues = response.GetHeaderValues("cache-control");
			if (headerValues != null)
			{
				if (headerValues.Exists(delegate(string headerValue)
				{
					string text = headerValue.ToLower();
					return text.Contains("no-store") || text.Contains("no-cache");
				}))
				{
					return false;
				}
			}
			List<string> headerValues2 = response.GetHeaderValues("pragma");
			if (headerValues2 != null)
			{
				if (headerValues2.Exists(delegate(string headerValue)
				{
					string text = headerValue.ToLower();
					return text.Contains("no-store") || text.Contains("no-cache");
				}))
				{
					return false;
				}
			}
			return response.GetHeaderValues("content-range") == null;
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x001A3AA4 File Offset: 0x001A1CA4
		internal static HTTPCacheFileInfo Store(Uri uri, HTTPMethods method, HTTPResponse response)
		{
			if (response == null || response.Data == null || response.Data.Length == 0)
			{
				return null;
			}
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheFileInfo httpcacheFileInfo = null;
			HTTPCacheService.rwLock.EnterWriteLock();
			try
			{
				if (!HTTPCacheService.library.TryGetValue(uri, out httpcacheFileInfo))
				{
					HTTPCacheService.library.Add(uri, httpcacheFileInfo = new HTTPCacheFileInfo(uri));
					HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
				}
			}
			finally
			{
				HTTPCacheService.rwLock.ExitWriteLock();
			}
			try
			{
				httpcacheFileInfo.Store(response);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPCacheService", string.Format("{0} - Saved to cache", uri.ToString()));
				}
			}
			catch
			{
				HTTPCacheService.DeleteEntity(uri, true);
				throw;
			}
			return httpcacheFileInfo;
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x001A3B80 File Offset: 0x001A1D80
		internal static Stream PrepareStreamed(Uri uri, HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterWriteLock();
			HTTPCacheFileInfo httpcacheFileInfo;
			try
			{
				if (!HTTPCacheService.library.TryGetValue(uri, out httpcacheFileInfo))
				{
					HTTPCacheService.library.Add(uri, httpcacheFileInfo = new HTTPCacheFileInfo(uri));
					HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
				}
			}
			finally
			{
				HTTPCacheService.rwLock.ExitWriteLock();
			}
			Stream saveStream;
			try
			{
				saveStream = httpcacheFileInfo.GetSaveStream(response);
			}
			catch
			{
				HTTPCacheService.DeleteEntity(uri, true);
				throw;
			}
			return saveStream;
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x001A3C18 File Offset: 0x001A1E18
		public static void BeginClear()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			if (HTTPCacheService.InClearThread)
			{
				return;
			}
			HTTPCacheService.InClearThread = true;
			HTTPCacheService.SetupCacheFolder();
			ThreadedRunner.RunShortLiving(new Action(HTTPCacheService.ClearImpl));
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x001A3C4C File Offset: 0x001A1E4C
		private static void ClearImpl()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterWriteLock();
			try
			{
				string[] files = HTTPManager.IOService.GetFiles(HTTPCacheService.CacheFolder);
				for (int i = 0; i < files.Length; i++)
				{
					try
					{
						HTTPManager.IOService.FileDelete(files[i]);
					}
					catch
					{
					}
				}
			}
			finally
			{
				HTTPCacheService.UsedIndexes.Clear();
				HTTPCacheService.library.Clear();
				HTTPCacheService.NextNameIDX = 1UL;
				HTTPCacheService.InClearThread = false;
				HTTPCacheService.rwLock.ExitWriteLock();
				HTTPCacheService.SaveLibrary();
			}
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x001A3CF4 File Offset: 0x001A1EF4
		public static void BeginMaintainence(HTTPCacheMaintananceParams maintananceParam)
		{
			if (maintananceParam == null)
			{
				throw new ArgumentNullException("maintananceParams == null");
			}
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			if (HTTPCacheService.InMaintainenceThread)
			{
				return;
			}
			HTTPCacheService.InMaintainenceThread = true;
			HTTPCacheService.SetupCacheFolder();
			ThreadedRunner.RunShortLiving<HTTPCacheMaintananceParams>(new Action<HTTPCacheMaintananceParams>(HTTPCacheService.MaintananceImpl), maintananceParam);
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x001A3D40 File Offset: 0x001A1F40
		private static void MaintananceImpl(HTTPCacheMaintananceParams maintananceParam)
		{
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterWriteLock();
			try
			{
				DateTime t = DateTime.UtcNow - maintananceParam.DeleteOlder;
				List<HTTPCacheFileInfo> list = new List<HTTPCacheFileInfo>();
				foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair in HTTPCacheService.library)
				{
					if (keyValuePair.Value.LastAccess < t && HTTPCacheService.DeleteEntity(keyValuePair.Key, false))
					{
						list.Add(keyValuePair.Value);
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					HTTPCacheService.library.Remove(list[i].Uri);
					HTTPCacheService.UsedIndexes.Remove(list[i].MappedNameIDX);
				}
				list.Clear();
				ulong num = HTTPCacheService.GetCacheSize();
				if (num > maintananceParam.MaxCacheSize)
				{
					List<HTTPCacheFileInfo> list2 = new List<HTTPCacheFileInfo>(HTTPCacheService.library.Count);
					foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair2 in HTTPCacheService.library)
					{
						list2.Add(keyValuePair2.Value);
					}
					list2.Sort();
					int num2 = 0;
					while (num >= maintananceParam.MaxCacheSize && num2 < list2.Count)
					{
						try
						{
							HTTPCacheFileInfo httpcacheFileInfo = list2[num2];
							ulong num3 = (ulong)((long)httpcacheFileInfo.BodyLength);
							HTTPCacheService.DeleteEntity(httpcacheFileInfo.Uri, true);
							num -= num3;
						}
						catch
						{
						}
						finally
						{
							num2++;
						}
					}
				}
			}
			finally
			{
				HTTPCacheService.InMaintainenceThread = false;
				HTTPCacheService.rwLock.ExitWriteLock();
				HTTPCacheService.SaveLibrary();
			}
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x001A3F64 File Offset: 0x001A2164
		public static int GetCacheEntityCount()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return 0;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterReadLock();
			int count;
			try
			{
				count = HTTPCacheService.library.Count;
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
			return count;
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x001A3FB4 File Offset: 0x001A21B4
		public static ulong GetCacheSize()
		{
			ulong num = 0UL;
			if (!HTTPCacheService.IsSupported)
			{
				return num;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterReadLock();
			ulong result;
			try
			{
				foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair in HTTPCacheService.library)
				{
					if (keyValuePair.Value.BodyLength > 0)
					{
						num += (ulong)((long)keyValuePair.Value.BodyLength);
					}
				}
				result = num;
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x001A4058 File Offset: 0x001A2258
		private static void LoadLibrary()
		{
			if (HTTPCacheService.library != null)
			{
				return;
			}
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			int num = 1;
			HTTPCacheService.rwLock.EnterWriteLock();
			HTTPCacheService.library = new Dictionary<Uri, HTTPCacheFileInfo>(new UriComparer());
			try
			{
				using (Stream stream = HTTPManager.IOService.CreateFileStream(HTTPCacheService.LibraryPath, FileStreamModes.Open))
				{
					using (BinaryReader binaryReader = new BinaryReader(stream))
					{
						num = binaryReader.ReadInt32();
						if (num > 1)
						{
							HTTPCacheService.NextNameIDX = binaryReader.ReadUInt64();
						}
						int num2 = binaryReader.ReadInt32();
						for (int i = 0; i < num2; i++)
						{
							Uri uri = new Uri(binaryReader.ReadString());
							HTTPCacheFileInfo httpcacheFileInfo = new HTTPCacheFileInfo(uri, binaryReader, num);
							if (httpcacheFileInfo.IsExists())
							{
								HTTPCacheService.library.Add(uri, httpcacheFileInfo);
								if (num > 1)
								{
									HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
								}
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
				HTTPCacheService.rwLock.ExitWriteLock();
			}
			if (num == 1)
			{
				HTTPCacheService.BeginClear();
				return;
			}
			HTTPCacheService.DeleteUnusedFiles();
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x001A4188 File Offset: 0x001A2388
		internal static void SaveLibrary()
		{
			if (HTTPCacheService.library == null)
			{
				return;
			}
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			HTTPCacheService.rwLock.EnterReadLock();
			try
			{
				using (Stream stream = HTTPManager.IOService.CreateFileStream(HTTPCacheService.LibraryPath, FileStreamModes.Create))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(stream))
					{
						binaryWriter.Write(2);
						binaryWriter.Write(HTTPCacheService.NextNameIDX);
						binaryWriter.Write(HTTPCacheService.library.Count);
						foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair in HTTPCacheService.library)
						{
							binaryWriter.Write(keyValuePair.Key.ToString());
							keyValuePair.Value.SaveTo(binaryWriter);
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				HTTPCacheService.rwLock.ExitReadLock();
			}
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x001A42A0 File Offset: 0x001A24A0
		internal static void SetBodyLength(Uri uri, int bodyLength)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			HTTPCacheService.CheckSetup();
			HTTPCacheService.rwLock.EnterUpgradeableReadLock();
			try
			{
				HTTPCacheFileInfo httpcacheFileInfo;
				if (HTTPCacheService.library.TryGetValue(uri, out httpcacheFileInfo))
				{
					httpcacheFileInfo.BodyLength = bodyLength;
				}
				else
				{
					HTTPCacheService.rwLock.EnterWriteLock();
					try
					{
						HTTPCacheService.library.Add(uri, httpcacheFileInfo = new HTTPCacheFileInfo(uri, DateTime.UtcNow, bodyLength));
						HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
					}
					finally
					{
						HTTPCacheService.rwLock.ExitWriteLock();
					}
				}
			}
			finally
			{
				HTTPCacheService.rwLock.ExitUpgradeableReadLock();
			}
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x001A4344 File Offset: 0x001A2544
		private static void DeleteUnusedFiles()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			HTTPCacheService.CheckSetup();
			string[] files = HTTPManager.IOService.GetFiles(HTTPCacheService.CacheFolder);
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					string fileName = Path.GetFileName(files[i]);
					ulong key = 0UL;
					bool flag = false;
					if (ulong.TryParse(fileName, NumberStyles.AllowHexSpecifier, null, out key))
					{
						HTTPCacheService.rwLock.EnterReadLock();
						try
						{
							flag = !HTTPCacheService.UsedIndexes.ContainsKey(key);
							goto IL_66;
						}
						finally
						{
							HTTPCacheService.rwLock.ExitReadLock();
						}
					}
					flag = true;
					IL_66:
					if (flag)
					{
						HTTPManager.IOService.FileDelete(files[i]);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x04003075 RID: 12405
		private const int LibraryVersion = 2;

		// Token: 0x04003076 RID: 12406
		private static bool isSupported;

		// Token: 0x04003077 RID: 12407
		private static bool IsSupportCheckDone;

		// Token: 0x04003078 RID: 12408
		private static Dictionary<Uri, HTTPCacheFileInfo> library;

		// Token: 0x04003079 RID: 12409
		private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		// Token: 0x0400307A RID: 12410
		private static Dictionary<ulong, HTTPCacheFileInfo> UsedIndexes = new Dictionary<ulong, HTTPCacheFileInfo>();

		// Token: 0x0400307D RID: 12413
		private static volatile bool InClearThread;

		// Token: 0x0400307E RID: 12414
		private static volatile bool InMaintainenceThread;

		// Token: 0x0400307F RID: 12415
		private static ulong NextNameIDX = 1UL;
	}
}
