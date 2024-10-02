using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000647 RID: 1607
	internal class DefiniteLengthInputStream : LimitedInputStream
	{
		// Token: 0x06003C0B RID: 15371 RVA: 0x00170374 File Offset: 0x0016E574
		internal DefiniteLengthInputStream(Stream inStream, int length) : base(inStream, length)
		{
			if (length < 0)
			{
				throw new ArgumentException("negative lengths not allowed", "length");
			}
			this._originalLength = length;
			this._remaining = length;
			if (length == 0)
			{
				this.SetParentEofDetect(true);
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06003C0C RID: 15372 RVA: 0x001703AA File Offset: 0x0016E5AA
		internal int Remaining
		{
			get
			{
				return this._remaining;
			}
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x001703B4 File Offset: 0x0016E5B4
		public override int ReadByte()
		{
			if (this._remaining == 0)
			{
				return -1;
			}
			int num = this._in.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			int num2 = this._remaining - 1;
			this._remaining = num2;
			if (num2 == 0)
			{
				this.SetParentEofDetect(true);
			}
			return num;
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00170434 File Offset: 0x0016E634
		public override int Read(byte[] buf, int off, int len)
		{
			if (this._remaining == 0)
			{
				return 0;
			}
			int count = Math.Min(len, this._remaining);
			int num = this._in.Read(buf, off, count);
			if (num < 1)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			if ((this._remaining -= num) == 0)
			{
				this.SetParentEofDetect(true);
			}
			return num;
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x001704C4 File Offset: 0x0016E6C4
		internal void ReadAllIntoByteArray(byte[] buf)
		{
			if (this._remaining != buf.Length)
			{
				throw new ArgumentException("buffer length not right for data");
			}
			if ((this._remaining -= Streams.ReadFully(this._in, buf)) != 0)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			this.SetParentEofDetect(true);
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x0017054C File Offset: 0x0016E74C
		internal byte[] ToArray()
		{
			if (this._remaining == 0)
			{
				return DefiniteLengthInputStream.EmptyBytes;
			}
			byte[] array = new byte[this._remaining];
			if ((this._remaining -= Streams.ReadFully(this._in, array)) != 0)
			{
				throw new EndOfStreamException(string.Concat(new object[]
				{
					"DEF length ",
					this._originalLength,
					" object truncated by ",
					this._remaining
				}));
			}
			this.SetParentEofDetect(true);
			return array;
		}

		// Token: 0x040026D3 RID: 9939
		private static readonly byte[] EmptyBytes = new byte[0];

		// Token: 0x040026D4 RID: 9940
		private readonly int _originalLength;

		// Token: 0x040026D5 RID: 9941
		private int _remaining;
	}
}
