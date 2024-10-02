using System;
using BestHTTP.PlatformSupport.Threading;

namespace BestHTTP
{
	// Token: 0x02000172 RID: 370
	public abstract class ConnectionBase : IDisposable
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x00090419 File Offset: 0x0008E619
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00090421 File Offset: 0x0008E621
		public string ServerAddress { get; protected set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0009042A File Offset: 0x0008E62A
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x00090432 File Offset: 0x0008E632
		public HTTPConnectionStates State { get; protected set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0009043B File Offset: 0x0008E63B
		public bool IsFree
		{
			get
			{
				return this.State == HTTPConnectionStates.Initial || this.State == HTTPConnectionStates.Free;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00090450 File Offset: 0x0008E650
		public bool IsActive
		{
			get
			{
				return this.State > HTTPConnectionStates.Initial && this.State < HTTPConnectionStates.Free;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00090466 File Offset: 0x0008E666
		// (set) Token: 0x06000D07 RID: 3335 RVA: 0x0009046E File Offset: 0x0008E66E
		public HTTPRequest CurrentRequest { get; protected set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00090477 File Offset: 0x0008E677
		public virtual bool IsRemovable
		{
			get
			{
				return this.IsFree && DateTime.UtcNow - this.LastProcessTime > HTTPManager.MaxConnectionIdleTime;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0009049D File Offset: 0x0008E69D
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x000904A5 File Offset: 0x0008E6A5
		public DateTime StartTime { get; protected set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x000904AE File Offset: 0x0008E6AE
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x000904B6 File Offset: 0x0008E6B6
		public DateTime TimedOutStart { get; protected set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x000904BF File Offset: 0x0008E6BF
		public bool HasProxy
		{
			get
			{
				return this.CurrentRequest != null && this.CurrentRequest.Proxy != null;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x000904D9 File Offset: 0x0008E6D9
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x000904E1 File Offset: 0x0008E6E1
		public Uri LastProcessedUri { get; protected set; }

		// Token: 0x06000D10 RID: 3344 RVA: 0x000904EA File Offset: 0x0008E6EA
		public ConnectionBase(string serverAddress) : this(serverAddress, true)
		{
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000904F4 File Offset: 0x0008E6F4
		public ConnectionBase(string serverAddress, bool threaded)
		{
			this.ServerAddress = serverAddress;
			this.State = HTTPConnectionStates.Initial;
			this.LastProcessTime = DateTime.UtcNow;
			this.IsThreaded = threaded;
		}

		// Token: 0x06000D12 RID: 3346
		internal abstract void Abort(HTTPConnectionStates hTTPConnectionStates);

		// Token: 0x06000D13 RID: 3347 RVA: 0x0009051C File Offset: 0x0008E71C
		internal void Process(HTTPRequest request)
		{
			if (this.State == HTTPConnectionStates.Processing)
			{
				throw new Exception("Connection already processing a request!");
			}
			this.StartTime = DateTime.MaxValue;
			this.State = HTTPConnectionStates.Processing;
			this.CurrentRequest = request;
			if (this.IsThreaded)
			{
				ThreadedRunner.RunLongLiving(new Action(this.ThreadFunc));
				return;
			}
			this.ThreadFunc();
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0000248C File Offset: 0x0000068C
		protected virtual void ThreadFunc()
		{
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00090578 File Offset: 0x0008E778
		internal void HandleProgressCallback()
		{
			if (this.CurrentRequest.OnProgress != null && this.CurrentRequest.DownloadProgressChanged)
			{
				try
				{
					this.CurrentRequest.OnProgress(this.CurrentRequest, this.CurrentRequest.Downloaded, this.CurrentRequest.DownloadLength);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnProgress", ex);
				}
				this.CurrentRequest.DownloadProgressChanged = false;
			}
			if (this.CurrentRequest.OnUploadProgress != null && this.CurrentRequest.UploadProgressChanged)
			{
				try
				{
					this.CurrentRequest.OnUploadProgress(this.CurrentRequest, this.CurrentRequest.Uploaded, this.CurrentRequest.UploadLength);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("ConnectionBase", "HandleProgressCallback - OnUploadProgress", ex2);
				}
				this.CurrentRequest.UploadProgressChanged = false;
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0009067C File Offset: 0x0008E87C
		internal void HandleCallback()
		{
			try
			{
				this.HandleProgressCallback();
				if (this.State == HTTPConnectionStates.Upgraded)
				{
					if (this.CurrentRequest != null && this.CurrentRequest.Response != null && this.CurrentRequest.Response.IsUpgraded)
					{
						this.CurrentRequest.UpgradeCallback();
					}
					if (this.State == HTTPConnectionStates.Upgraded)
					{
						this.State = HTTPConnectionStates.WaitForProtocolShutdown;
					}
				}
				else
				{
					this.CurrentRequest.CallCallback();
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("ConnectionBase", "HandleCallback", ex);
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00090714 File Offset: 0x0008E914
		internal void Recycle(HTTPConnectionRecycledDelegate onConnectionRecycled)
		{
			this.OnConnectionRecycled = onConnectionRecycled;
			if (this.State <= HTTPConnectionStates.Initial || this.State >= HTTPConnectionStates.WaitForProtocolShutdown || this.State == HTTPConnectionStates.Redirected)
			{
				this.RecycleNow();
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00090740 File Offset: 0x0008E940
		protected void RecycleNow()
		{
			if (this.State == HTTPConnectionStates.TimedOut || this.State == HTTPConnectionStates.Closed)
			{
				this.LastProcessTime = DateTime.MinValue;
			}
			this.State = HTTPConnectionStates.Free;
			if (this.CurrentRequest != null)
			{
				this.CurrentRequest.Dispose();
			}
			this.CurrentRequest = null;
			if (this.OnConnectionRecycled != null)
			{
				this.OnConnectionRecycled(this);
				this.OnConnectionRecycled = null;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x000907A7 File Offset: 0x0008E9A7
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x000907AF File Offset: 0x0008E9AF
		private protected bool IsDisposed { protected get; private set; }

		// Token: 0x06000D1B RID: 3355 RVA: 0x000907B8 File Offset: 0x0008E9B8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000907C7 File Offset: 0x0008E9C7
		protected virtual void Dispose(bool disposing)
		{
			this.IsDisposed = true;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x000907D0 File Offset: 0x0008E9D0
		~ConnectionBase()
		{
			this.Dispose(false);
		}

		// Token: 0x04001297 RID: 4759
		protected DateTime LastProcessTime;

		// Token: 0x04001298 RID: 4760
		protected HTTPConnectionRecycledDelegate OnConnectionRecycled;

		// Token: 0x04001299 RID: 4761
		private bool IsThreaded;
	}
}
