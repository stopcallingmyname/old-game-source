using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x02000288 RID: 648
	public sealed class Base64
	{
		// Token: 0x060017B9 RID: 6073 RVA: 0x00022F1F File Offset: 0x0002111F
		private Base64()
		{
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000B8763 File Offset: 0x000B6963
		public static string ToBase64String(byte[] data)
		{
			return Convert.ToBase64String(data, 0, data.Length);
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x000B876F File Offset: 0x000B696F
		public static string ToBase64String(byte[] data, int off, int length)
		{
			return Convert.ToBase64String(data, off, length);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x000B8779 File Offset: 0x000B6979
		public static byte[] Encode(byte[] data)
		{
			return Base64.Encode(data, 0, data.Length);
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x000B8785 File Offset: 0x000B6985
		public static byte[] Encode(byte[] data, int off, int length)
		{
			return Strings.ToAsciiByteArray(Convert.ToBase64String(data, off, length));
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x000B8794 File Offset: 0x000B6994
		public static int Encode(byte[] data, Stream outStream)
		{
			byte[] array = Base64.Encode(data);
			outStream.Write(array, 0, array.Length);
			return array.Length;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000B87B8 File Offset: 0x000B69B8
		public static int Encode(byte[] data, int off, int length, Stream outStream)
		{
			byte[] array = Base64.Encode(data, off, length);
			outStream.Write(array, 0, array.Length);
			return array.Length;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000B87DC File Offset: 0x000B69DC
		public static byte[] Decode(byte[] data)
		{
			return Convert.FromBase64String(Strings.FromAsciiByteArray(data));
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000B87E9 File Offset: 0x000B69E9
		public static byte[] Decode(string data)
		{
			return Convert.FromBase64String(data);
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x000B87F4 File Offset: 0x000B69F4
		public static int Decode(string data, Stream outStream)
		{
			byte[] array = Base64.Decode(data);
			outStream.Write(array, 0, array.Length);
			return array.Length;
		}
	}
}
