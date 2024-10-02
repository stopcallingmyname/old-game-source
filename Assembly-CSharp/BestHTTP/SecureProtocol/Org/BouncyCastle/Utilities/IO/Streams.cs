using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200027E RID: 638
	public sealed class Streams
	{
		// Token: 0x0600178A RID: 6026 RVA: 0x00022F1F File Offset: 0x0002111F
		private Streams()
		{
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x000B7FFC File Offset: 0x000B61FC
		public static void Drain(Stream inStr)
		{
			byte[] array = new byte[512];
			while (inStr.Read(array, 0, array.Length) > 0)
			{
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x000B8024 File Offset: 0x000B6224
		public static byte[] ReadAll(Stream inStr)
		{
			MemoryStream memoryStream = new MemoryStream();
			Streams.PipeAll(inStr, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000B8044 File Offset: 0x000B6244
		public static byte[] ReadAllLimited(Stream inStr, int limit)
		{
			MemoryStream memoryStream = new MemoryStream();
			Streams.PipeAllLimited(inStr, (long)limit, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000B8067 File Offset: 0x000B6267
		public static int ReadFully(Stream inStr, byte[] buf)
		{
			return Streams.ReadFully(inStr, buf, 0, buf.Length);
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x000B8074 File Offset: 0x000B6274
		public static int ReadFully(Stream inStr, byte[] buf, int off, int len)
		{
			int i;
			int num;
			for (i = 0; i < len; i += num)
			{
				num = inStr.Read(buf, off + i, len - i);
				if (num < 1)
				{
					break;
				}
			}
			return i;
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x000B80A0 File Offset: 0x000B62A0
		public static void PipeAll(Stream inStr, Stream outStr)
		{
			byte[] array = new byte[512];
			int count;
			while ((count = inStr.Read(array, 0, array.Length)) > 0)
			{
				outStr.Write(array, 0, count);
			}
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000B80D4 File Offset: 0x000B62D4
		public static long PipeAllLimited(Stream inStr, long limit, Stream outStr)
		{
			byte[] array = new byte[512];
			long num = 0L;
			int num2;
			while ((num2 = inStr.Read(array, 0, array.Length)) > 0)
			{
				if (limit - num < (long)num2)
				{
					throw new StreamOverflowException("Data Overflow");
				}
				num += (long)num2;
				outStr.Write(array, 0, num2);
			}
			return num;
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000B8122 File Offset: 0x000B6322
		public static void WriteBufTo(MemoryStream buf, Stream output)
		{
			buf.WriteTo(output);
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x000B812C File Offset: 0x000B632C
		public static int WriteBufTo(MemoryStream buf, byte[] output, int offset)
		{
			int num = (int)buf.Length;
			buf.WriteTo(new MemoryStream(output, offset, num, true));
			return num;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000B8154 File Offset: 0x000B6354
		public static void WriteZeroes(Stream outStr, long count)
		{
			byte[] buffer = new byte[512];
			while (count > 512L)
			{
				outStr.Write(buffer, 0, 512);
				count -= 512L;
			}
			outStr.Write(buffer, 0, (int)count);
		}

		// Token: 0x04001809 RID: 6153
		private const int BufferSize = 512;
	}
}
