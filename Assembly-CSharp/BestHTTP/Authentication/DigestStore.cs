using System;
using System.Collections.Generic;
using System.Threading;

namespace BestHTTP.Authentication
{
	// Token: 0x0200081D RID: 2077
	public static class DigestStore
	{
		// Token: 0x06004A1C RID: 18972 RVA: 0x001A4C4C File Offset: 0x001A2E4C
		public static Digest Get(Uri uri)
		{
			DigestStore.rwLock.EnterReadLock();
			Digest result;
			try
			{
				Digest digest = null;
				if (DigestStore.Digests.TryGetValue(uri.Host, out digest) && !digest.IsUriProtected(uri))
				{
					result = null;
				}
				else
				{
					result = digest;
				}
			}
			finally
			{
				DigestStore.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x001A4CA8 File Offset: 0x001A2EA8
		public static Digest GetOrCreate(Uri uri)
		{
			DigestStore.rwLock.EnterUpgradeableReadLock();
			Digest result;
			try
			{
				Digest digest = null;
				if (!DigestStore.Digests.TryGetValue(uri.Host, out digest))
				{
					DigestStore.rwLock.EnterWriteLock();
					try
					{
						DigestStore.Digests.Add(uri.Host, digest = new Digest(uri));
					}
					finally
					{
						DigestStore.rwLock.ExitWriteLock();
					}
				}
				result = digest;
			}
			finally
			{
				DigestStore.rwLock.ExitUpgradeableReadLock();
			}
			return result;
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x001A4D30 File Offset: 0x001A2F30
		public static void Remove(Uri uri)
		{
			DigestStore.rwLock.EnterWriteLock();
			try
			{
				DigestStore.Digests.Remove(uri.Host);
			}
			finally
			{
				DigestStore.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x001A4D78 File Offset: 0x001A2F78
		public static string FindBest(List<string> authHeaders)
		{
			if (authHeaders == null || authHeaders.Count == 0)
			{
				return string.Empty;
			}
			List<string> list = new List<string>(authHeaders.Count);
			for (int j = 0; j < authHeaders.Count; j++)
			{
				list.Add(authHeaders[j].ToLower());
			}
			int i;
			int i2;
			for (i = 0; i < DigestStore.SupportedAlgorithms.Length; i = i2)
			{
				int num = list.FindIndex((string header) => header.StartsWith(DigestStore.SupportedAlgorithms[i]));
				if (num != -1)
				{
					return authHeaders[num];
				}
				i2 = i + 1;
			}
			return string.Empty;
		}

		// Token: 0x04003092 RID: 12434
		private static Dictionary<string, Digest> Digests = new Dictionary<string, Digest>();

		// Token: 0x04003093 RID: 12435
		private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		// Token: 0x04003094 RID: 12436
		private static string[] SupportedAlgorithms = new string[]
		{
			"digest",
			"basic"
		};
	}
}
