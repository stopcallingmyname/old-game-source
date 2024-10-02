using System;
using System.Collections;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200048B RID: 1163
	public abstract class TlsSRTPUtils
	{
		// Token: 0x06002D56 RID: 11606 RVA: 0x0011BAD6 File Offset: 0x00119CD6
		public static void AddUseSrtpExtension(IDictionary extensions, UseSrtpData useSRTPData)
		{
			extensions[14] = TlsSRTPUtils.CreateUseSrtpExtension(useSRTPData);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x0011BAEC File Offset: 0x00119CEC
		public static UseSrtpData GetUseSrtpExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 14);
			if (extensionData != null)
			{
				return TlsSRTPUtils.ReadUseSrtpExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x0011BB10 File Offset: 0x00119D10
		public static byte[] CreateUseSrtpExtension(UseSrtpData useSrtpData)
		{
			if (useSrtpData == null)
			{
				throw new ArgumentNullException("useSrtpData");
			}
			MemoryStream memoryStream = new MemoryStream();
			TlsUtilities.WriteUint16ArrayWithUint16Length(useSrtpData.ProtectionProfiles, memoryStream);
			TlsUtilities.WriteOpaque8(useSrtpData.Mki, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x0011BB50 File Offset: 0x00119D50
		public static UseSrtpData ReadUseSrtpExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, true);
			int num = TlsUtilities.ReadUint16(memoryStream);
			if (num < 2 || (num & 1) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			int[] protectionProfiles = TlsUtilities.ReadUint16Array(num / 2, memoryStream);
			byte[] mki = TlsUtilities.ReadOpaque8(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return new UseSrtpData(protectionProfiles, mki);
		}
	}
}
