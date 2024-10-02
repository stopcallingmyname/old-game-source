using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x0200028F RID: 655
	public interface IEncoder
	{
		// Token: 0x060017E9 RID: 6121
		int Encode(byte[] data, int off, int length, Stream outStream);

		// Token: 0x060017EA RID: 6122
		int Decode(byte[] data, int off, int length, Stream outStream);

		// Token: 0x060017EB RID: 6123
		int DecodeString(string data, Stream outStream);
	}
}
