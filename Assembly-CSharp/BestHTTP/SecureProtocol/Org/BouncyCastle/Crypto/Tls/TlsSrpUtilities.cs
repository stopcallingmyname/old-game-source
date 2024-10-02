using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200048A RID: 1162
	public abstract class TlsSrpUtilities
	{
		// Token: 0x06002D4E RID: 11598 RVA: 0x0011BA4E File Offset: 0x00119C4E
		public static void AddSrpExtension(IDictionary extensions, byte[] identity)
		{
			extensions[12] = TlsSrpUtilities.CreateSrpExtension(identity);
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x0011BA64 File Offset: 0x00119C64
		public static byte[] GetSrpExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 12);
			if (extensionData != null)
			{
				return TlsSrpUtilities.ReadSrpExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x0011BA85 File Offset: 0x00119C85
		public static byte[] CreateSrpExtension(byte[] identity)
		{
			if (identity == null)
			{
				throw new TlsFatalAlert(80);
			}
			return TlsUtilities.EncodeOpaque8(identity);
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x0011BA98 File Offset: 0x00119C98
		public static byte[] ReadSrpExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			byte[] result = TlsUtilities.ReadOpaque8(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x00116D63 File Offset: 0x00114F63
		public static BigInteger ReadSrpParameter(Stream input)
		{
			return new BigInteger(1, TlsUtilities.ReadOpaque16(input));
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x00116DBA File Offset: 0x00114FBA
		public static void WriteSrpParameter(BigInteger x, Stream output)
		{
			TlsUtilities.WriteOpaque16(BigIntegers.AsUnsignedByteArray(x), output);
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x0011BAC7 File Offset: 0x00119CC7
		public static bool IsSrpCipherSuite(int cipherSuite)
		{
			return cipherSuite - 49178 <= 8;
		}
	}
}
