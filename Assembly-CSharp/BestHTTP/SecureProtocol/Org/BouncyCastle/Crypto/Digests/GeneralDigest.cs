using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005A5 RID: 1445
	public abstract class GeneralDigest : IDigest, IMemoable
	{
		// Token: 0x060036E9 RID: 14057 RVA: 0x00154C25 File Offset: 0x00152E25
		internal GeneralDigest()
		{
			this.xBuf = new byte[4];
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x00154C39 File Offset: 0x00152E39
		internal GeneralDigest(GeneralDigest t)
		{
			this.xBuf = new byte[t.xBuf.Length];
			this.CopyIn(t);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x00154C5B File Offset: 0x00152E5B
		protected void CopyIn(GeneralDigest t)
		{
			Array.Copy(t.xBuf, 0, this.xBuf, 0, t.xBuf.Length);
			this.xBufOff = t.xBufOff;
			this.byteCount = t.byteCount;
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x00154C90 File Offset: 0x00152E90
		public void Update(byte input)
		{
			byte[] array = this.xBuf;
			int num = this.xBufOff;
			this.xBufOff = num + 1;
			array[num] = input;
			if (this.xBufOff == this.xBuf.Length)
			{
				this.ProcessWord(this.xBuf, 0);
				this.xBufOff = 0;
			}
			this.byteCount += 1L;
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x00154CEC File Offset: 0x00152EEC
		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			length = Math.Max(0, length);
			int i = 0;
			if (this.xBufOff != 0)
			{
				while (i < length)
				{
					byte[] array = this.xBuf;
					int num = this.xBufOff;
					this.xBufOff = num + 1;
					array[num] = input[inOff + i++];
					if (this.xBufOff == 4)
					{
						this.ProcessWord(this.xBuf, 0);
						this.xBufOff = 0;
						break;
					}
				}
			}
			int num2 = (length - i & -4) + i;
			while (i < num2)
			{
				this.ProcessWord(input, inOff + i);
				i += 4;
			}
			while (i < length)
			{
				byte[] array2 = this.xBuf;
				int num = this.xBufOff;
				this.xBufOff = num + 1;
				array2[num] = input[inOff + i++];
			}
			this.byteCount += (long)length;
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x00154DA8 File Offset: 0x00152FA8
		public void Finish()
		{
			long bitLength = this.byteCount << 3;
			this.Update(128);
			while (this.xBufOff != 0)
			{
				this.Update(0);
			}
			this.ProcessLength(bitLength);
			this.ProcessBlock();
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x00154DE7 File Offset: 0x00152FE7
		public virtual void Reset()
		{
			this.byteCount = 0L;
			this.xBufOff = 0;
			Array.Clear(this.xBuf, 0, this.xBuf.Length);
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x00153D0C File Offset: 0x00151F0C
		public int GetByteLength()
		{
			return 64;
		}

		// Token: 0x060036F1 RID: 14065
		internal abstract void ProcessWord(byte[] input, int inOff);

		// Token: 0x060036F2 RID: 14066
		internal abstract void ProcessLength(long bitLength);

		// Token: 0x060036F3 RID: 14067
		internal abstract void ProcessBlock();

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060036F4 RID: 14068
		public abstract string AlgorithmName { get; }

		// Token: 0x060036F5 RID: 14069
		public abstract int GetDigestSize();

		// Token: 0x060036F6 RID: 14070
		public abstract int DoFinal(byte[] output, int outOff);

		// Token: 0x060036F7 RID: 14071
		public abstract IMemoable Copy();

		// Token: 0x060036F8 RID: 14072
		public abstract void Reset(IMemoable t);

		// Token: 0x040023EE RID: 9198
		private const int BYTE_LENGTH = 64;

		// Token: 0x040023EF RID: 9199
		private byte[] xBuf;

		// Token: 0x040023F0 RID: 9200
		private int xBufOff;

		// Token: 0x040023F1 RID: 9201
		private long byteCount;
	}
}
