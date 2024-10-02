using System;
using System.Collections.Generic;
using System.Threading;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F1 RID: 2033
	public sealed class HeartbeatManager
	{
		// Token: 0x06004855 RID: 18517 RVA: 0x00198C48 File Offset: 0x00196E48
		public void Subscribe(IHeartbeat heartbeat)
		{
			this.rwLock.EnterWriteLock();
			try
			{
				if (!this.Heartbeats.Contains(heartbeat))
				{
					this.Heartbeats.Add(heartbeat);
				}
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x00198C98 File Offset: 0x00196E98
		public void Unsubscribe(IHeartbeat heartbeat)
		{
			this.rwLock.EnterWriteLock();
			try
			{
				this.Heartbeats.Remove(heartbeat);
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x00198CDC File Offset: 0x00196EDC
		public void Update()
		{
			if (this.LastUpdate == DateTime.MinValue)
			{
				this.LastUpdate = DateTime.UtcNow;
				return;
			}
			TimeSpan dif = DateTime.UtcNow - this.LastUpdate;
			this.LastUpdate = DateTime.UtcNow;
			int num = 0;
			this.rwLock.EnterReadLock();
			try
			{
				if (this.UpdateArray == null || this.UpdateArray.Length < this.Heartbeats.Count)
				{
					Array.Resize<IHeartbeat>(ref this.UpdateArray, this.Heartbeats.Count);
				}
				this.Heartbeats.CopyTo(0, this.UpdateArray, 0, this.Heartbeats.Count);
				num = this.Heartbeats.Count;
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
			for (int i = 0; i < num; i++)
			{
				try
				{
					this.UpdateArray[i].OnHeartbeatUpdate(dif);
				}
				catch
				{
				}
			}
		}

		// Token: 0x04002F02 RID: 12034
		private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		// Token: 0x04002F03 RID: 12035
		private List<IHeartbeat> Heartbeats = new List<IHeartbeat>();

		// Token: 0x04002F04 RID: 12036
		private IHeartbeat[] UpdateArray;

		// Token: 0x04002F05 RID: 12037
		private DateTime LastUpdate = DateTime.MinValue;
	}
}
