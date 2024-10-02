using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x02000290 RID: 656
	public interface ITranslator
	{
		// Token: 0x060017EC RID: 6124
		int GetEncodedBlockSize();

		// Token: 0x060017ED RID: 6125
		int Encode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);

		// Token: 0x060017EE RID: 6126
		int GetDecodedBlockSize();

		// Token: 0x060017EF RID: 6127
		int Decode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);
	}
}
