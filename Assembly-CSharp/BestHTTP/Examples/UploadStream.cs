using System;
using System.IO;
using System.Threading;

namespace BestHTTP.Examples
{
	// Token: 0x02000196 RID: 406
	public sealed class UploadStream : Stream
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x00097558 File Offset: 0x00095758
		// (set) Token: 0x06000EBF RID: 3775 RVA: 0x00097560 File Offset: 0x00095760
		public string Name { get; private set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0009756C File Offset: 0x0009576C
		private bool IsReadBufferEmpty
		{
			get
			{
				object obj = this.locker;
				bool result;
				lock (obj)
				{
					result = (this.ReadBuffer.Position == this.ReadBuffer.Length);
				}
				return result;
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x000975C0 File Offset: 0x000957C0
		public UploadStream(string name) : this()
		{
			this.Name = name;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x000975D0 File Offset: 0x000957D0
		public UploadStream()
		{
			this.ReadBuffer = new MemoryStream();
			this.WriteBuffer = new MemoryStream();
			this.Name = string.Empty;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00097634 File Offset: 0x00095834
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.noMoreData)
			{
				if (this.ReadBuffer.Position != this.ReadBuffer.Length)
				{
					return this.ReadBuffer.Read(buffer, offset, count);
				}
				if (this.WriteBuffer.Length <= 0L)
				{
					HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Read - End Of Stream", this.Name));
					return -1;
				}
				this.SwitchBuffers();
			}
			object obj;
			if (this.IsReadBufferEmpty)
			{
				this.ARE.WaitOne();
				obj = this.locker;
				lock (obj)
				{
					if (this.IsReadBufferEmpty && this.WriteBuffer.Length > 0L)
					{
						this.SwitchBuffers();
					}
				}
			}
			int result = -1;
			obj = this.locker;
			lock (obj)
			{
				result = this.ReadBuffer.Read(buffer, offset, count);
			}
			return result;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00097744 File Offset: 0x00095944
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.noMoreData)
			{
				throw new ArgumentException("noMoreData already set!");
			}
			object obj = this.locker;
			lock (obj)
			{
				this.WriteBuffer.Write(buffer, offset, count);
				this.SwitchBuffers();
			}
			this.ARE.Set();
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000977B4 File Offset: 0x000959B4
		public override void Flush()
		{
			this.Finish();
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x000977BC File Offset: 0x000959BC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Dispose", this.Name));
				this.ReadBuffer.Dispose();
				this.ReadBuffer = null;
				this.WriteBuffer.Dispose();
				this.WriteBuffer = null;
				this.ARE.Close();
				this.ARE = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00097828 File Offset: 0x00095A28
		public void Finish()
		{
			if (this.noMoreData)
			{
				throw new ArgumentException("noMoreData already set!");
			}
			HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Finish", this.Name));
			this.noMoreData = true;
			this.ARE.Set();
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0009787C File Offset: 0x00095A7C
		private bool SwitchBuffers()
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.ReadBuffer.Position == this.ReadBuffer.Length)
				{
					this.WriteBuffer.Seek(0L, SeekOrigin.Begin);
					this.ReadBuffer.SetLength(0L);
					MemoryStream writeBuffer = this.WriteBuffer;
					this.WriteBuffer = this.ReadBuffer;
					this.ReadBuffer = writeBuffer;
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x000947C7 File Offset: 0x000929C7
		public override bool CanRead
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x000947C7 File Offset: 0x000929C7
		public override bool CanSeek
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x000947C7 File Offset: 0x000929C7
		public override bool CanWrite
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x000947C7 File Offset: 0x000929C7
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000947C7 File Offset: 0x000929C7
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400137C RID: 4988
		private MemoryStream ReadBuffer = new MemoryStream();

		// Token: 0x0400137D RID: 4989
		private MemoryStream WriteBuffer = new MemoryStream();

		// Token: 0x0400137E RID: 4990
		private bool noMoreData;

		// Token: 0x0400137F RID: 4991
		private AutoResetEvent ARE = new AutoResetEvent(false);

		// Token: 0x04001380 RID: 4992
		private object locker = new object();
	}
}
