using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x02000291 RID: 657
	public class UrlBase64
	{
		// Token: 0x060017F0 RID: 6128 RVA: 0x000B95C0 File Offset: 0x000B77C0
		public static byte[] Encode(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				UrlBase64.encoder.Encode(data, 0, data.Length, memoryStream);
			}
			catch (IOException ex)
			{
				throw new Exception("exception encoding URL safe base64 string: " + ex.Message, ex);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x000B9614 File Offset: 0x000B7814
		public static int Encode(byte[] data, Stream outStr)
		{
			return UrlBase64.encoder.Encode(data, 0, data.Length, outStr);
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x000B9628 File Offset: 0x000B7828
		public static byte[] Decode(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				UrlBase64.encoder.Decode(data, 0, data.Length, memoryStream);
			}
			catch (IOException ex)
			{
				throw new Exception("exception decoding URL safe base64 string: " + ex.Message, ex);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x000B967C File Offset: 0x000B787C
		public static int Decode(byte[] data, Stream outStr)
		{
			return UrlBase64.encoder.Decode(data, 0, data.Length, outStr);
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000B9690 File Offset: 0x000B7890
		public static byte[] Decode(string data)
		{
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				UrlBase64.encoder.DecodeString(data, memoryStream);
			}
			catch (IOException ex)
			{
				throw new Exception("exception decoding URL safe base64 string: " + ex.Message, ex);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x000B96E0 File Offset: 0x000B78E0
		public static int Decode(string data, Stream outStr)
		{
			return UrlBase64.encoder.DecodeString(data, outStr);
		}

		// Token: 0x04001827 RID: 6183
		private static readonly IEncoder encoder = new UrlBase64Encoder();
	}
}
