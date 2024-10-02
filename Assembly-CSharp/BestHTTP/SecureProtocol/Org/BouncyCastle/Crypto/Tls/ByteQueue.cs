using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FF RID: 1023
	public class ByteQueue
	{
		// Token: 0x06002953 RID: 10579 RVA: 0x0010DF82 File Offset: 0x0010C182
		public static int NextTwoPow(int i)
		{
			i |= i >> 1;
			i |= i >> 2;
			i |= i >> 4;
			i |= i >> 8;
			i |= i >> 16;
			return i + 1;
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x0010DFAB File Offset: 0x0010C1AB
		public ByteQueue() : this(1024)
		{
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0010DFB8 File Offset: 0x0010C1B8
		public ByteQueue(int capacity)
		{
			this.databuf = ((capacity == 0) ? TlsUtilities.EmptyBytes : new byte[capacity]);
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x0010DFD6 File Offset: 0x0010C1D6
		public ByteQueue(byte[] buf, int off, int len)
		{
			this.databuf = buf;
			this.skipped = off;
			this.available = len;
			this.readOnlyBuf = true;
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0010DFFC File Offset: 0x0010C1FC
		public void AddData(byte[] data, int offset, int len)
		{
			if (this.readOnlyBuf)
			{
				throw new InvalidOperationException("Cannot add data to read-only buffer");
			}
			if (this.skipped + this.available + len > this.databuf.Length)
			{
				int num = ByteQueue.NextTwoPow(this.available + len);
				if (num > this.databuf.Length)
				{
					byte[] destinationArray = new byte[num];
					Array.Copy(this.databuf, this.skipped, destinationArray, 0, this.available);
					this.databuf = destinationArray;
				}
				else
				{
					Array.Copy(this.databuf, this.skipped, this.databuf, 0, this.available);
				}
				this.skipped = 0;
			}
			Array.Copy(data, offset, this.databuf, this.skipped + this.available, len);
			this.available += len;
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002958 RID: 10584 RVA: 0x0010E0C5 File Offset: 0x0010C2C5
		public int Available
		{
			get
			{
				return this.available;
			}
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x0010E0D0 File Offset: 0x0010C2D0
		public void CopyTo(Stream output, int length)
		{
			if (length > this.available)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Cannot copy ",
					length,
					" bytes, only got ",
					this.available
				}));
			}
			output.Write(this.databuf, this.skipped, length);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x0010E134 File Offset: 0x0010C334
		public void Read(byte[] buf, int offset, int len, int skip)
		{
			if (buf.Length - offset < len)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Buffer size of ",
					buf.Length,
					" is too small for a read of ",
					len,
					" bytes"
				}));
			}
			if (this.available - skip < len)
			{
				throw new InvalidOperationException("Not enough data to read");
			}
			Array.Copy(this.databuf, this.skipped + skip, buf, offset, len);
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x0010E1B4 File Offset: 0x0010C3B4
		public MemoryStream ReadFrom(int length)
		{
			if (length > this.available)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Cannot read ",
					length,
					" bytes, only got ",
					this.available
				}));
			}
			int index = this.skipped;
			this.available -= length;
			this.skipped += length;
			return new MemoryStream(this.databuf, index, length, false);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0010E234 File Offset: 0x0010C434
		public void RemoveData(int i)
		{
			if (i > this.available)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Cannot remove ",
					i,
					" bytes, only got ",
					this.available
				}));
			}
			this.available -= i;
			this.skipped += i;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0010E29E File Offset: 0x0010C49E
		public void RemoveData(byte[] buf, int off, int len, int skip)
		{
			this.Read(buf, off, len, skip);
			this.RemoveData(skip + len);
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0010E2B8 File Offset: 0x0010C4B8
		public byte[] RemoveData(int len, int skip)
		{
			byte[] array = new byte[len];
			this.RemoveData(array, 0, len, skip);
			return array;
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x0010E2D8 File Offset: 0x0010C4D8
		public void Shrink()
		{
			if (this.available == 0)
			{
				this.databuf = TlsUtilities.EmptyBytes;
				this.skipped = 0;
				return;
			}
			int num = ByteQueue.NextTwoPow(this.available);
			if (num < this.databuf.Length)
			{
				byte[] destinationArray = new byte[num];
				Array.Copy(this.databuf, this.skipped, destinationArray, 0, this.available);
				this.databuf = destinationArray;
				this.skipped = 0;
			}
		}

		// Token: 0x04001B5F RID: 7007
		private const int DefaultCapacity = 1024;

		// Token: 0x04001B60 RID: 7008
		private byte[] databuf;

		// Token: 0x04001B61 RID: 7009
		private int skipped;

		// Token: 0x04001B62 RID: 7010
		private int available;

		// Token: 0x04001B63 RID: 7011
		private bool readOnlyBuf;
	}
}
