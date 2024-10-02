using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000423 RID: 1059
	internal class DtlsReassembler
	{
		// Token: 0x06002A4B RID: 10827 RVA: 0x00111305 File Offset: 0x0010F505
		internal DtlsReassembler(byte msg_type, int length)
		{
			this.mMsgType = msg_type;
			this.mBody = new byte[length];
			this.mMissing.Add(new DtlsReassembler.Range(0, length));
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x0011133E File Offset: 0x0010F53E
		internal byte MsgType
		{
			get
			{
				return this.mMsgType;
			}
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x00111346 File Offset: 0x0010F546
		internal byte[] GetBodyIfComplete()
		{
			if (this.mMissing.Count != 0)
			{
				return null;
			}
			return this.mBody;
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x00111360 File Offset: 0x0010F560
		internal void ContributeFragment(byte msg_type, int length, byte[] buf, int off, int fragment_offset, int fragment_length)
		{
			int num = fragment_offset + fragment_length;
			if (this.mMsgType != msg_type || this.mBody.Length != length || num > length)
			{
				return;
			}
			if (fragment_length == 0)
			{
				if (fragment_offset == 0 && this.mMissing.Count > 0 && ((DtlsReassembler.Range)this.mMissing[0]).End == 0)
				{
					this.mMissing.RemoveAt(0);
				}
				return;
			}
			for (int i = 0; i < this.mMissing.Count; i++)
			{
				DtlsReassembler.Range range = (DtlsReassembler.Range)this.mMissing[i];
				if (range.Start >= num)
				{
					break;
				}
				if (range.End > fragment_offset)
				{
					int num2 = Math.Max(range.Start, fragment_offset);
					int num3 = Math.Min(range.End, num);
					int length2 = num3 - num2;
					Array.Copy(buf, off + num2 - fragment_offset, this.mBody, num2, length2);
					if (num2 == range.Start)
					{
						if (num3 == range.End)
						{
							this.mMissing.RemoveAt(i--);
						}
						else
						{
							range.Start = num3;
						}
					}
					else
					{
						if (num3 != range.End)
						{
							this.mMissing.Insert(++i, new DtlsReassembler.Range(num3, range.End));
						}
						range.End = num2;
					}
				}
			}
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x001114A3 File Offset: 0x0010F6A3
		internal void Reset()
		{
			this.mMissing.Clear();
			this.mMissing.Add(new DtlsReassembler.Range(0, this.mBody.Length));
		}

		// Token: 0x04001CC6 RID: 7366
		private readonly byte mMsgType;

		// Token: 0x04001CC7 RID: 7367
		private readonly byte[] mBody;

		// Token: 0x04001CC8 RID: 7368
		private readonly IList mMissing = Platform.CreateArrayList();

		// Token: 0x0200093F RID: 2367
		private class Range
		{
			// Token: 0x06004ECB RID: 20171 RVA: 0x001B3312 File Offset: 0x001B1512
			internal Range(int start, int end)
			{
				this.mStart = start;
				this.mEnd = end;
			}

			// Token: 0x17000C53 RID: 3155
			// (get) Token: 0x06004ECC RID: 20172 RVA: 0x001B3328 File Offset: 0x001B1528
			// (set) Token: 0x06004ECD RID: 20173 RVA: 0x001B3330 File Offset: 0x001B1530
			public int Start
			{
				get
				{
					return this.mStart;
				}
				set
				{
					this.mStart = value;
				}
			}

			// Token: 0x17000C54 RID: 3156
			// (get) Token: 0x06004ECE RID: 20174 RVA: 0x001B3339 File Offset: 0x001B1539
			// (set) Token: 0x06004ECF RID: 20175 RVA: 0x001B3341 File Offset: 0x001B1541
			public int End
			{
				get
				{
					return this.mEnd;
				}
				set
				{
					this.mEnd = value;
				}
			}

			// Token: 0x040035FD RID: 13821
			private int mStart;

			// Token: 0x040035FE RID: 13822
			private int mEnd;
		}
	}
}
