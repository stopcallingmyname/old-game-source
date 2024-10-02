using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003CA RID: 970
	internal class Check
	{
		// Token: 0x06002806 RID: 10246 RVA: 0x0010C9C3 File Offset: 0x0010ABC3
		internal static void DataLength(bool condition, string msg)
		{
			if (condition)
			{
				throw new DataLengthException(msg);
			}
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x0010C9CF File Offset: 0x0010ABCF
		internal static void DataLength(byte[] buf, int off, int len, string msg)
		{
			if (off + len > buf.Length)
			{
				throw new DataLengthException(msg);
			}
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x0010C9E0 File Offset: 0x0010ABE0
		internal static void OutputLength(byte[] buf, int off, int len, string msg)
		{
			if (off + len > buf.Length)
			{
				throw new OutputLengthException(msg);
			}
		}
	}
}
