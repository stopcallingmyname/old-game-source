using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000651 RID: 1617
	public abstract class DerGenerator : Asn1Generator
	{
		// Token: 0x06003C7D RID: 15485 RVA: 0x0016F90D File Offset: 0x0016DB0D
		protected DerGenerator(Stream outStream) : base(outStream)
		{
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x00171A92 File Offset: 0x0016FC92
		protected DerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
		{
			this._tagged = true;
			this._isExplicit = isExplicit;
			this._tagNo = tagNo;
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x00171AB0 File Offset: 0x0016FCB0
		private static void WriteLength(Stream outStr, int length)
		{
			if (length > 127)
			{
				int num = 1;
				int num2 = length;
				while ((num2 >>= 8) != 0)
				{
					num++;
				}
				outStr.WriteByte((byte)(num | 128));
				for (int i = (num - 1) * 8; i >= 0; i -= 8)
				{
					outStr.WriteByte((byte)(length >> i));
				}
				return;
			}
			outStr.WriteByte((byte)length);
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x00171B07 File Offset: 0x0016FD07
		internal static void WriteDerEncoded(Stream outStream, int tag, byte[] bytes)
		{
			outStream.WriteByte((byte)tag);
			DerGenerator.WriteLength(outStream, bytes.Length);
			outStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x00171B28 File Offset: 0x0016FD28
		internal void WriteDerEncoded(int tag, byte[] bytes)
		{
			if (!this._tagged)
			{
				DerGenerator.WriteDerEncoded(base.Out, tag, bytes);
				return;
			}
			int num = this._tagNo | 128;
			if (this._isExplicit)
			{
				int tag2 = this._tagNo | 32 | 128;
				MemoryStream memoryStream = new MemoryStream();
				DerGenerator.WriteDerEncoded(memoryStream, tag, bytes);
				DerGenerator.WriteDerEncoded(base.Out, tag2, memoryStream.ToArray());
				return;
			}
			if ((tag & 32) != 0)
			{
				num |= 32;
			}
			DerGenerator.WriteDerEncoded(base.Out, num, bytes);
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x00171BA9 File Offset: 0x0016FDA9
		internal static void WriteDerEncoded(Stream outStr, int tag, Stream inStr)
		{
			DerGenerator.WriteDerEncoded(outStr, tag, Streams.ReadAll(inStr));
		}

		// Token: 0x040026EA RID: 9962
		private bool _tagged;

		// Token: 0x040026EB RID: 9963
		private bool _isExplicit;

		// Token: 0x040026EC RID: 9964
		private int _tagNo;
	}
}
