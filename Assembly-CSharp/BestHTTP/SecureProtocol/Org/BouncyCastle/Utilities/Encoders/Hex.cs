using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x0200028C RID: 652
	public sealed class Hex
	{
		// Token: 0x060017D2 RID: 6098 RVA: 0x00022F1F File Offset: 0x0002111F
		private Hex()
		{
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000B9118 File Offset: 0x000B7318
		public static string ToHexString(byte[] data)
		{
			return Hex.ToHexString(data, 0, data.Length);
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000B9124 File Offset: 0x000B7324
		public static string ToHexString(byte[] data, int off, int length)
		{
			return Strings.FromAsciiByteArray(Hex.Encode(data, off, length));
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000B9133 File Offset: 0x000B7333
		public static byte[] Encode(byte[] data)
		{
			return Hex.Encode(data, 0, data.Length);
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x000B9140 File Offset: 0x000B7340
		public static byte[] Encode(byte[] data, int off, int length)
		{
			MemoryStream memoryStream = new MemoryStream(length * 2);
			Hex.encoder.Encode(data, off, length, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x000B916B File Offset: 0x000B736B
		public static int Encode(byte[] data, Stream outStream)
		{
			return Hex.encoder.Encode(data, 0, data.Length, outStream);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x000B917D File Offset: 0x000B737D
		public static int Encode(byte[] data, int off, int length, Stream outStream)
		{
			return Hex.encoder.Encode(data, off, length, outStream);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x000B9190 File Offset: 0x000B7390
		public static byte[] Decode(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream((data.Length + 1) / 2);
			Hex.encoder.Decode(data, 0, data.Length, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000B91C4 File Offset: 0x000B73C4
		public static byte[] Decode(string data)
		{
			MemoryStream memoryStream = new MemoryStream((data.Length + 1) / 2);
			Hex.encoder.DecodeString(data, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000B91F4 File Offset: 0x000B73F4
		public static int Decode(string data, Stream outStream)
		{
			return Hex.encoder.DecodeString(data, outStream);
		}

		// Token: 0x04001823 RID: 6179
		private static readonly IEncoder encoder = new HexEncoder();
	}
}
