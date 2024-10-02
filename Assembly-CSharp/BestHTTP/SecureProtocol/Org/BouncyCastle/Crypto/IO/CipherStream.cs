using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO
{
	// Token: 0x0200053D RID: 1341
	public class CipherStream : Stream
	{
		// Token: 0x060032CB RID: 13003 RVA: 0x001324C7 File Offset: 0x001306C7
		public CipherStream(Stream stream, IBufferedCipher readCipher, IBufferedCipher writeCipher)
		{
			this.stream = stream;
			if (readCipher != null)
			{
				this.inCipher = readCipher;
				this.mInBuf = null;
			}
			if (writeCipher != null)
			{
				this.outCipher = writeCipher;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x001324F1 File Offset: 0x001306F1
		public IBufferedCipher ReadCipher
		{
			get
			{
				return this.inCipher;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x001324F9 File Offset: 0x001306F9
		public IBufferedCipher WriteCipher
		{
			get
			{
				return this.outCipher;
			}
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x00132504 File Offset: 0x00130704
		public override int ReadByte()
		{
			if (this.inCipher == null)
			{
				return this.stream.ReadByte();
			}
			if ((this.mInBuf == null || this.mInPos >= this.mInBuf.Length) && !this.FillInBuf())
			{
				return -1;
			}
			byte[] array = this.mInBuf;
			int num = this.mInPos;
			this.mInPos = num + 1;
			return array[num];
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x00132560 File Offset: 0x00130760
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.inCipher == null)
			{
				return this.stream.Read(buffer, offset, count);
			}
			int num = 0;
			while (num < count && ((this.mInBuf != null && this.mInPos < this.mInBuf.Length) || this.FillInBuf()))
			{
				int num2 = Math.Min(count - num, this.mInBuf.Length - this.mInPos);
				Array.Copy(this.mInBuf, this.mInPos, buffer, offset + num, num2);
				this.mInPos += num2;
				num += num2;
			}
			return num;
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x001325ED File Offset: 0x001307ED
		private bool FillInBuf()
		{
			if (this.inStreamEnded)
			{
				return false;
			}
			this.mInPos = 0;
			do
			{
				this.mInBuf = this.ReadAndProcessBlock();
			}
			while (!this.inStreamEnded && this.mInBuf == null);
			return this.mInBuf != null;
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x00132628 File Offset: 0x00130828
		private byte[] ReadAndProcessBlock()
		{
			int blockSize = this.inCipher.GetBlockSize();
			byte[] array = new byte[(blockSize == 0) ? 256 : blockSize];
			int num = 0;
			for (;;)
			{
				int num2 = this.stream.Read(array, num, array.Length - num);
				if (num2 < 1)
				{
					break;
				}
				num += num2;
				if (num >= array.Length)
				{
					goto IL_4C;
				}
			}
			this.inStreamEnded = true;
			IL_4C:
			byte[] array2 = this.inStreamEnded ? this.inCipher.DoFinal(array, 0, num) : this.inCipher.ProcessBytes(array);
			if (array2 != null && array2.Length == 0)
			{
				array2 = null;
			}
			return array2;
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x001326B0 File Offset: 0x001308B0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.outCipher == null)
			{
				this.stream.Write(buffer, offset, count);
				return;
			}
			byte[] array = this.outCipher.ProcessBytes(buffer, offset, count);
			if (array != null)
			{
				this.stream.Write(array, 0, array.Length);
			}
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x001326F8 File Offset: 0x001308F8
		public override void WriteByte(byte b)
		{
			if (this.outCipher == null)
			{
				this.stream.WriteByte(b);
				return;
			}
			byte[] array = this.outCipher.ProcessByte(b);
			if (array != null)
			{
				this.stream.Write(array, 0, array.Length);
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x0013273A File Offset: 0x0013093A
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead && this.inCipher != null;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x00132754 File Offset: 0x00130954
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite && this.outCipher != null;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060032D7 RID: 13015 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x0008FF0D File Offset: 0x0008E10D
		// (set) Token: 0x060032D9 RID: 13017 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x00132770 File Offset: 0x00130970
		public override void Close()
		{
			if (this.outCipher != null)
			{
				byte[] array = this.outCipher.DoFinal();
				this.stream.Write(array, 0, array.Length);
				this.stream.Flush();
			}
			Platform.Dispose(this.stream);
			base.Close();
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x001327BD File Offset: 0x001309BD
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override void SetLength(long length)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002168 RID: 8552
		internal Stream stream;

		// Token: 0x04002169 RID: 8553
		internal IBufferedCipher inCipher;

		// Token: 0x0400216A RID: 8554
		internal IBufferedCipher outCipher;

		// Token: 0x0400216B RID: 8555
		private byte[] mInBuf;

		// Token: 0x0400216C RID: 8556
		private int mInPos;

		// Token: 0x0400216D RID: 8557
		private bool inStreamEnded;
	}
}
