using System;
using System.IO;
using UnityEngine;

namespace BestHTTP.Extensions
{
	// Token: 0x020007EA RID: 2026
	public sealed class BufferPoolMemoryStream : Stream
	{
		// Token: 0x060047FA RID: 18426 RVA: 0x00197882 File Offset: 0x00195A82
		public BufferPoolMemoryStream() : this(0)
		{
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0019788C File Offset: 0x00195A8C
		public BufferPoolMemoryStream(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this.canWrite = true;
			this.internalBuffer = ((capacity > 0) ? VariableSizedBufferPool.Get((long)capacity, true) : VariableSizedBufferPool.NoData);
			this.capacity = this.internalBuffer.Length;
			this.expandable = true;
			this.allowGetBuffer = true;
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x001978EA File Offset: 0x00195AEA
		public BufferPoolMemoryStream(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.InternalConstructor(buffer, 0, buffer.Length, true, false);
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0019790D File Offset: 0x00195B0D
		public BufferPoolMemoryStream(byte[] buffer, bool writable)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.InternalConstructor(buffer, 0, buffer.Length, writable, false);
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x00197930 File Offset: 0x00195B30
		public BufferPoolMemoryStream(byte[] buffer, int index, int count)
		{
			this.InternalConstructor(buffer, index, count, true, false);
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x00197943 File Offset: 0x00195B43
		public BufferPoolMemoryStream(byte[] buffer, int index, int count, bool writable)
		{
			this.InternalConstructor(buffer, index, count, writable, false);
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x00197957 File Offset: 0x00195B57
		public BufferPoolMemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
		{
			this.InternalConstructor(buffer, index, count, writable, publiclyVisible);
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0019796C File Offset: 0x00195B6C
		private void InternalConstructor(byte[] buffer, int index, int count, bool writable, bool publicallyVisible)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException("index or count is less than 0.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("index+count", "The size of the buffer is less than index + count.");
			}
			this.canWrite = writable;
			this.internalBuffer = buffer;
			this.capacity = count + index;
			this.length = this.capacity;
			this.position = index;
			this.initialIndex = index;
			this.allowGetBuffer = publicallyVisible;
			this.expandable = false;
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x001979F3 File Offset: 0x00195BF3
		private void CheckIfClosedThrowDisposed()
		{
			if (this.streamClosed)
			{
				throw new ObjectDisposedException("MemoryStream");
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06004803 RID: 18435 RVA: 0x00197A08 File Offset: 0x00195C08
		public override bool CanRead
		{
			get
			{
				return !this.streamClosed;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06004804 RID: 18436 RVA: 0x00197A08 File Offset: 0x00195C08
		public override bool CanSeek
		{
			get
			{
				return !this.streamClosed;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06004805 RID: 18437 RVA: 0x00197A13 File Offset: 0x00195C13
		public override bool CanWrite
		{
			get
			{
				return !this.streamClosed && this.canWrite;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06004806 RID: 18438 RVA: 0x00197A25 File Offset: 0x00195C25
		// (set) Token: 0x06004807 RID: 18439 RVA: 0x00197A3C File Offset: 0x00195C3C
		public int Capacity
		{
			get
			{
				this.CheckIfClosedThrowDisposed();
				return this.capacity - this.initialIndex;
			}
			set
			{
				this.CheckIfClosedThrowDisposed();
				if (value == this.capacity)
				{
					return;
				}
				if (!this.expandable)
				{
					throw new NotSupportedException("Cannot expand this MemoryStream");
				}
				if (value < 0 || value < this.length)
				{
					throw new ArgumentOutOfRangeException("value", string.Concat(new object[]
					{
						"New capacity cannot be negative or less than the current capacity ",
						value,
						" ",
						this.capacity
					}));
				}
				byte[] dst = null;
				if (value != 0)
				{
					dst = VariableSizedBufferPool.Get((long)value, true);
					Buffer.BlockCopy(this.internalBuffer, 0, dst, 0, this.length);
				}
				this.dirty_bytes = 0;
				VariableSizedBufferPool.Release(this.internalBuffer);
				this.internalBuffer = dst;
				this.capacity = ((this.internalBuffer != null) ? this.internalBuffer.Length : 0);
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06004808 RID: 18440 RVA: 0x00197B0A File Offset: 0x00195D0A
		public override long Length
		{
			get
			{
				this.CheckIfClosedThrowDisposed();
				return (long)(this.length - this.initialIndex);
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06004809 RID: 18441 RVA: 0x00197B20 File Offset: 0x00195D20
		// (set) Token: 0x0600480A RID: 18442 RVA: 0x00197B38 File Offset: 0x00195D38
		public override long Position
		{
			get
			{
				this.CheckIfClosedThrowDisposed();
				return (long)(this.position - this.initialIndex);
			}
			set
			{
				this.CheckIfClosedThrowDisposed();
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Position cannot be negative");
				}
				if (value > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("value", "Position must be non-negative and less than 2^31 - 1 - origin");
				}
				this.position = this.initialIndex + (int)value;
			}
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x00197B88 File Offset: 0x00195D88
		protected override void Dispose(bool disposing)
		{
			this.streamClosed = true;
			this.expandable = false;
			if (this.internalBuffer != null)
			{
				VariableSizedBufferPool.Release(this.internalBuffer);
			}
			this.internalBuffer = null;
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x0000248C File Offset: 0x0000068C
		public override void Flush()
		{
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x00197BB2 File Offset: 0x00195DB2
		public byte[] GetBuffer()
		{
			if (!this.allowGetBuffer)
			{
				throw new UnauthorizedAccessException();
			}
			return this.internalBuffer;
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x00197BC8 File Offset: 0x00195DC8
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckIfClosedThrowDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException("offset or count less than zero.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("offset+count", "The size of the buffer is less than offset + count.");
			}
			if (this.position >= this.length || count == 0)
			{
				return 0;
			}
			if (this.position > this.length - count)
			{
				count = this.length - this.position;
			}
			Buffer.BlockCopy(this.internalBuffer, this.position, buffer, offset, count);
			this.position += count;
			return count;
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x00197C6C File Offset: 0x00195E6C
		public override int ReadByte()
		{
			this.CheckIfClosedThrowDisposed();
			if (this.position >= this.length)
			{
				return -1;
			}
			byte[] array = this.internalBuffer;
			int num = this.position;
			this.position = num + 1;
			return array[num];
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x00197CA8 File Offset: 0x00195EA8
		public override long Seek(long offset, SeekOrigin loc)
		{
			this.CheckIfClosedThrowDisposed();
			if (offset > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("Offset out of range. " + offset);
			}
			int num;
			switch (loc)
			{
			case SeekOrigin.Begin:
				if (offset < 0L)
				{
					throw new IOException("Attempted to seek before start of MemoryStream.");
				}
				num = this.initialIndex;
				break;
			case SeekOrigin.Current:
				num = this.position;
				break;
			case SeekOrigin.End:
				num = this.length;
				break;
			default:
				throw new ArgumentException("loc", "Invalid SeekOrigin");
			}
			num += (int)offset;
			if (num < this.initialIndex)
			{
				throw new IOException("Attempted to seek before start of MemoryStream.");
			}
			this.position = num;
			return (long)this.position;
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x00197D50 File Offset: 0x00195F50
		private int CalculateNewCapacity(int minimum)
		{
			if (minimum < 256)
			{
				minimum = 256;
			}
			if (minimum < this.capacity * 2)
			{
				minimum = this.capacity * 2;
			}
			if (!Mathf.IsPowerOfTwo(minimum))
			{
				minimum = Mathf.NextPowerOfTwo(minimum);
			}
			return minimum;
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x00197D87 File Offset: 0x00195F87
		private void Expand(int newSize)
		{
			if (newSize > this.capacity)
			{
				this.Capacity = this.CalculateNewCapacity(newSize);
				return;
			}
			if (this.dirty_bytes > 0)
			{
				Array.Clear(this.internalBuffer, this.length, this.dirty_bytes);
				this.dirty_bytes = 0;
			}
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x00197DC8 File Offset: 0x00195FC8
		public override void SetLength(long value)
		{
			if (!this.expandable && value > (long)this.capacity)
			{
				throw new NotSupportedException("Expanding this MemoryStream is not supported");
			}
			this.CheckIfClosedThrowDisposed();
			if (!this.canWrite)
			{
				throw new NotSupportedException("Cannot write to this MemoryStream");
			}
			if (value < 0L || value + (long)this.initialIndex > 2147483647L)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num = (int)value + this.initialIndex;
			if (num > this.length)
			{
				this.Expand(num);
			}
			else if (num < this.length)
			{
				this.dirty_bytes += this.length - num;
			}
			this.length = num;
			if (this.position > this.length)
			{
				this.position = this.length;
			}
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x00197E82 File Offset: 0x00196082
		public byte[] ToArray()
		{
			return this.ToArray(false);
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x00197E8C File Offset: 0x0019608C
		public byte[] ToArray(bool canBeLarger)
		{
			int num = this.length - this.initialIndex;
			byte[] array = (num > 0) ? VariableSizedBufferPool.Get((long)num, canBeLarger) : VariableSizedBufferPool.NoData;
			if (this.internalBuffer != null)
			{
				Buffer.BlockCopy(this.internalBuffer, this.initialIndex, array, 0, num);
			}
			return array;
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x00197ED8 File Offset: 0x001960D8
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckIfClosedThrowDisposed();
			if (!this.canWrite)
			{
				throw new NotSupportedException("Cannot write to this stream.");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("offset+count", "The size of the buffer is less than offset + count.");
			}
			if (this.position > this.length - count)
			{
				this.Expand(this.position + count);
			}
			Buffer.BlockCopy(buffer, offset, this.internalBuffer, this.position, count);
			this.position += count;
			if (this.position >= this.length)
			{
				this.length = this.position;
			}
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x00197F8C File Offset: 0x0019618C
		public override void WriteByte(byte value)
		{
			this.CheckIfClosedThrowDisposed();
			if (!this.canWrite)
			{
				throw new NotSupportedException("Cannot write to this stream.");
			}
			if (this.position >= this.length)
			{
				this.Expand(this.position + 1);
				this.length = this.position + 1;
			}
			byte[] array = this.internalBuffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = value;
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x00197FF5 File Offset: 0x001961F5
		public void WriteTo(Stream stream)
		{
			this.CheckIfClosedThrowDisposed();
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			stream.Write(this.internalBuffer, this.initialIndex, this.length - this.initialIndex);
		}

		// Token: 0x04002EEF RID: 12015
		private bool canWrite;

		// Token: 0x04002EF0 RID: 12016
		private bool allowGetBuffer;

		// Token: 0x04002EF1 RID: 12017
		private int capacity;

		// Token: 0x04002EF2 RID: 12018
		private int length;

		// Token: 0x04002EF3 RID: 12019
		private byte[] internalBuffer;

		// Token: 0x04002EF4 RID: 12020
		private int initialIndex;

		// Token: 0x04002EF5 RID: 12021
		private bool expandable;

		// Token: 0x04002EF6 RID: 12022
		private bool streamClosed;

		// Token: 0x04002EF7 RID: 12023
		private int position;

		// Token: 0x04002EF8 RID: 12024
		private int dirty_bytes;
	}
}
