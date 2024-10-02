using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F8 RID: 2040
	public static class VariableSizedBufferPool
	{
		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x0600487F RID: 18559 RVA: 0x00199390 File Offset: 0x00197590
		// (set) Token: 0x06004880 RID: 18560 RVA: 0x00199399 File Offset: 0x00197599
		public static bool IsEnabled
		{
			get
			{
				return VariableSizedBufferPool._isEnabled;
			}
			set
			{
				VariableSizedBufferPool._isEnabled = value;
				if (!VariableSizedBufferPool._isEnabled)
				{
					VariableSizedBufferPool.Clear();
				}
			}
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x001993B4 File Offset: 0x001975B4
		static VariableSizedBufferPool()
		{
			VariableSizedBufferPool.IsDoubleReleaseCheckEnabled = false;
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x00199470 File Offset: 0x00197670
		public static byte[] Get(long size, bool canBeLarger)
		{
			if (!VariableSizedBufferPool._isEnabled)
			{
				return new byte[size];
			}
			if (size == 0L)
			{
				return VariableSizedBufferPool.NoData;
			}
			if (VariableSizedBufferPool.FreeBuffers.Count == 0)
			{
				return new byte[size];
			}
			BufferDesc bufferDesc = VariableSizedBufferPool.FindFreeBuffer(size, canBeLarger);
			if (bufferDesc.buffer == null)
			{
				if (canBeLarger)
				{
					if (size < VariableSizedBufferPool.MinBufferSize)
					{
						size = VariableSizedBufferPool.MinBufferSize;
					}
					else if (!VariableSizedBufferPool.IsPowerOfTwo(size))
					{
						size = VariableSizedBufferPool.NextPowerOf2(size);
					}
				}
				return new byte[size];
			}
			Interlocked.Increment(ref VariableSizedBufferPool.GetBuffers);
			Interlocked.Add(ref VariableSizedBufferPool.PoolSize, (long)(-(long)bufferDesc.buffer.Length));
			return bufferDesc.buffer;
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x00199510 File Offset: 0x00197710
		public static void Release(List<byte[]> buffers)
		{
			if (!VariableSizedBufferPool._isEnabled || buffers == null || buffers.Count == 0)
			{
				return;
			}
			for (int i = 0; i < buffers.Count; i++)
			{
				VariableSizedBufferPool.Release(buffers[i]);
			}
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x00199550 File Offset: 0x00197750
		public static void Release(byte[] buffer)
		{
			if (!VariableSizedBufferPool._isEnabled || buffer == null)
			{
				return;
			}
			int num = buffer.Length;
			if (num == 0 || (long)num > VariableSizedBufferPool.MaxBufferSize)
			{
				return;
			}
			VariableSizedBufferPool.rwLock.EnterWriteLock();
			try
			{
				if (VariableSizedBufferPool.PoolSize + (long)num <= VariableSizedBufferPool.MaxPoolSize)
				{
					VariableSizedBufferPool.PoolSize += (long)num;
					VariableSizedBufferPool.ReleaseBuffers += 1L;
					VariableSizedBufferPool.AddFreeBuffer(buffer);
				}
			}
			finally
			{
				VariableSizedBufferPool.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x001995D4 File Offset: 0x001977D4
		public static byte[] Resize(ref byte[] buffer, int newSize, bool canBeLarger)
		{
			if (!VariableSizedBufferPool._isEnabled)
			{
				Array.Resize<byte>(ref buffer, newSize);
				return buffer;
			}
			byte[] array = VariableSizedBufferPool.Get((long)newSize, canBeLarger);
			Array.Copy(buffer, 0, array, 0, Math.Min(array.Length, buffer.Length));
			VariableSizedBufferPool.Release(buffer);
			byte[] result;
			buffer = (result = array);
			return result;
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x00199620 File Offset: 0x00197820
		public static string GetStatistics(bool showEmptyBuffers = true)
		{
			VariableSizedBufferPool.rwLock.EnterReadLock();
			string result;
			try
			{
				VariableSizedBufferPool.statiscticsBuilder.Length = 0;
				VariableSizedBufferPool.statiscticsBuilder.AppendFormat("Pooled array reused count: {0:N0}\n", VariableSizedBufferPool.GetBuffers);
				VariableSizedBufferPool.statiscticsBuilder.AppendFormat("Release call count: {0:N0}\n", VariableSizedBufferPool.ReleaseBuffers);
				VariableSizedBufferPool.statiscticsBuilder.AppendFormat("PoolSize: {0:N0}\n", VariableSizedBufferPool.PoolSize);
				VariableSizedBufferPool.statiscticsBuilder.AppendFormat("Buffers: {0}\n", VariableSizedBufferPool.FreeBuffers.Count);
				for (int i = 0; i < VariableSizedBufferPool.FreeBuffers.Count; i++)
				{
					BufferStore bufferStore = VariableSizedBufferPool.FreeBuffers[i];
					List<BufferDesc> buffers = bufferStore.buffers;
					if (showEmptyBuffers || buffers.Count > 0)
					{
						VariableSizedBufferPool.statiscticsBuilder.AppendFormat("- Size: {0:N0} Count: {1:N0}\n", bufferStore.Size, buffers.Count);
					}
				}
				result = VariableSizedBufferPool.statiscticsBuilder.ToString();
			}
			finally
			{
				VariableSizedBufferPool.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x00199734 File Offset: 0x00197934
		public static void Clear()
		{
			VariableSizedBufferPool.rwLock.EnterWriteLock();
			try
			{
				VariableSizedBufferPool.FreeBuffers.Clear();
				VariableSizedBufferPool.PoolSize = 0L;
			}
			finally
			{
				VariableSizedBufferPool.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x0019977C File Offset: 0x0019797C
		internal static void Maintain()
		{
			DateTime utcNow = DateTime.UtcNow;
			if (!VariableSizedBufferPool._isEnabled || VariableSizedBufferPool.lastMaintenance + VariableSizedBufferPool.RunMaintenanceEvery > utcNow)
			{
				return;
			}
			VariableSizedBufferPool.lastMaintenance = utcNow;
			DateTime t = utcNow - VariableSizedBufferPool.RemoveOlderThan;
			VariableSizedBufferPool.rwLock.EnterWriteLock();
			try
			{
				for (int i = 0; i < VariableSizedBufferPool.FreeBuffers.Count; i++)
				{
					BufferStore bufferStore = VariableSizedBufferPool.FreeBuffers[i];
					List<BufferDesc> buffers = bufferStore.buffers;
					for (int j = buffers.Count - 1; j >= 0; j--)
					{
						if (buffers[j].released < t)
						{
							int num = j + 1;
							buffers.RemoveRange(0, num);
							VariableSizedBufferPool.PoolSize -= (long)((int)((long)num * bufferStore.Size));
							break;
						}
					}
					if (VariableSizedBufferPool.RemoveEmptyLists && buffers.Count == 0)
					{
						VariableSizedBufferPool.FreeBuffers.RemoveAt(i--);
					}
				}
			}
			finally
			{
				VariableSizedBufferPool.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x00199890 File Offset: 0x00197A90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsPowerOfTwo(long x)
		{
			return (x & x - 1L) == 0L;
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x0019989C File Offset: 0x00197A9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long NextPowerOf2(long x)
		{
			long num;
			for (num = 1L; num <= x; num *= 2L)
			{
			}
			return num;
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x001998B8 File Offset: 0x00197AB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static BufferDesc FindFreeBuffer(long size, bool canBeLarger)
		{
			VariableSizedBufferPool.rwLock.EnterUpgradeableReadLock();
			try
			{
				for (int i = 0; i < VariableSizedBufferPool.FreeBuffers.Count; i++)
				{
					BufferStore bufferStore = VariableSizedBufferPool.FreeBuffers[i];
					if (bufferStore.buffers.Count > 0 && (bufferStore.Size == size || (canBeLarger && bufferStore.Size > size)))
					{
						BufferDesc result = bufferStore.buffers[bufferStore.buffers.Count - 1];
						VariableSizedBufferPool.rwLock.EnterWriteLock();
						try
						{
							bufferStore.buffers.RemoveAt(bufferStore.buffers.Count - 1);
						}
						finally
						{
							VariableSizedBufferPool.rwLock.ExitWriteLock();
						}
						return result;
					}
				}
			}
			finally
			{
				VariableSizedBufferPool.rwLock.ExitUpgradeableReadLock();
			}
			return BufferDesc.Empty;
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x00199994 File Offset: 0x00197B94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void AddFreeBuffer(byte[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < VariableSizedBufferPool.FreeBuffers.Count; i++)
			{
				BufferStore bufferStore = VariableSizedBufferPool.FreeBuffers[i];
				if (bufferStore.Size == (long)num)
				{
					if (VariableSizedBufferPool.IsDoubleReleaseCheckEnabled)
					{
						for (int j = 0; j < bufferStore.buffers.Count; j++)
						{
							if (bufferStore.buffers[j].buffer == buffer)
							{
								HTTPManager.Logger.Error("VariableSizedBufferPool", "Buffer already added to the pool!");
								return;
							}
						}
					}
					bufferStore.buffers.Add(new BufferDesc(buffer));
					return;
				}
				if (bufferStore.Size > (long)num)
				{
					VariableSizedBufferPool.FreeBuffers.Insert(i, new BufferStore((long)num, buffer));
					return;
				}
				VariableSizedBufferPool.FreeBuffers.Add(new BufferStore((long)num, buffer));
			}
		}

		// Token: 0x04002F15 RID: 12053
		public static readonly byte[] NoData = new byte[0];

		// Token: 0x04002F16 RID: 12054
		public static volatile bool _isEnabled = true;

		// Token: 0x04002F17 RID: 12055
		public static TimeSpan RemoveOlderThan = TimeSpan.FromSeconds(30.0);

		// Token: 0x04002F18 RID: 12056
		public static TimeSpan RunMaintenanceEvery = TimeSpan.FromSeconds(10.0);

		// Token: 0x04002F19 RID: 12057
		public static long MinBufferSize = 256L;

		// Token: 0x04002F1A RID: 12058
		public static long MaxBufferSize = long.MaxValue;

		// Token: 0x04002F1B RID: 12059
		public static long MaxPoolSize = 10485760L;

		// Token: 0x04002F1C RID: 12060
		public static bool RemoveEmptyLists = true;

		// Token: 0x04002F1D RID: 12061
		public static bool IsDoubleReleaseCheckEnabled = false;

		// Token: 0x04002F1E RID: 12062
		private static List<BufferStore> FreeBuffers = new List<BufferStore>();

		// Token: 0x04002F1F RID: 12063
		private static DateTime lastMaintenance = DateTime.MinValue;

		// Token: 0x04002F20 RID: 12064
		private static long PoolSize = 0L;

		// Token: 0x04002F21 RID: 12065
		private static long GetBuffers = 0L;

		// Token: 0x04002F22 RID: 12066
		private static long ReleaseBuffers = 0L;

		// Token: 0x04002F23 RID: 12067
		private static StringBuilder statiscticsBuilder = new StringBuilder();

		// Token: 0x04002F24 RID: 12068
		private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
	}
}
