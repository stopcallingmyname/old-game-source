using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200046C RID: 1132
	public abstract class TlsExtensionsUtilities
	{
		// Token: 0x06002C29 RID: 11305 RVA: 0x00117F33 File Offset: 0x00116133
		public static IDictionary EnsureExtensionsInitialised(IDictionary extensions)
		{
			if (extensions != null)
			{
				return extensions;
			}
			return Platform.CreateHashtable();
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x00117F3F File Offset: 0x0011613F
		public static void AddClientCertificateTypeExtensionClient(IDictionary extensions, byte[] certificateTypes)
		{
			extensions[19] = TlsExtensionsUtilities.CreateCertificateTypeExtensionClient(certificateTypes);
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x00117F54 File Offset: 0x00116154
		public static void AddClientCertificateTypeExtensionServer(IDictionary extensions, byte certificateType)
		{
			extensions[19] = TlsExtensionsUtilities.CreateCertificateTypeExtensionServer(certificateType);
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x00117F69 File Offset: 0x00116169
		public static void AddEncryptThenMacExtension(IDictionary extensions)
		{
			extensions[22] = TlsExtensionsUtilities.CreateEncryptThenMacExtension();
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x00117F7D File Offset: 0x0011617D
		public static void AddExtendedMasterSecretExtension(IDictionary extensions)
		{
			extensions[23] = TlsExtensionsUtilities.CreateExtendedMasterSecretExtension();
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x00117F91 File Offset: 0x00116191
		public static void AddHeartbeatExtension(IDictionary extensions, HeartbeatExtension heartbeatExtension)
		{
			extensions[15] = TlsExtensionsUtilities.CreateHeartbeatExtension(heartbeatExtension);
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x00117FA6 File Offset: 0x001161A6
		public static void AddMaxFragmentLengthExtension(IDictionary extensions, byte maxFragmentLength)
		{
			extensions[1] = TlsExtensionsUtilities.CreateMaxFragmentLengthExtension(maxFragmentLength);
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x00117FBA File Offset: 0x001161BA
		public static void AddPaddingExtension(IDictionary extensions, int dataLength)
		{
			extensions[21] = TlsExtensionsUtilities.CreatePaddingExtension(dataLength);
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x00117FCF File Offset: 0x001161CF
		public static void AddServerCertificateTypeExtensionClient(IDictionary extensions, byte[] certificateTypes)
		{
			extensions[20] = TlsExtensionsUtilities.CreateCertificateTypeExtensionClient(certificateTypes);
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x00117FE4 File Offset: 0x001161E4
		public static void AddServerCertificateTypeExtensionServer(IDictionary extensions, byte certificateType)
		{
			extensions[20] = TlsExtensionsUtilities.CreateCertificateTypeExtensionServer(certificateType);
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x00117FF9 File Offset: 0x001161F9
		public static void AddServerNameExtension(IDictionary extensions, ServerNameList serverNameList)
		{
			extensions[0] = TlsExtensionsUtilities.CreateServerNameExtension(serverNameList);
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x0011800D File Offset: 0x0011620D
		public static void AddStatusRequestExtension(IDictionary extensions, CertificateStatusRequest statusRequest)
		{
			extensions[5] = TlsExtensionsUtilities.CreateStatusRequestExtension(statusRequest);
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x00118021 File Offset: 0x00116221
		public static void AddTruncatedHMacExtension(IDictionary extensions)
		{
			extensions[4] = TlsExtensionsUtilities.CreateTruncatedHMacExtension();
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x00118034 File Offset: 0x00116234
		public static byte[] GetClientCertificateTypeExtensionClient(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 19);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadCertificateTypeExtensionClient(extensionData);
			}
			return null;
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x00118058 File Offset: 0x00116258
		public static short GetClientCertificateTypeExtensionServer(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 19);
			if (extensionData != null)
			{
				return (short)TlsExtensionsUtilities.ReadCertificateTypeExtensionServer(extensionData);
			}
			return -1;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x0011807C File Offset: 0x0011627C
		public static HeartbeatExtension GetHeartbeatExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 15);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadHeartbeatExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x001180A0 File Offset: 0x001162A0
		public static short GetMaxFragmentLengthExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 1);
			if (extensionData != null)
			{
				return (short)TlsExtensionsUtilities.ReadMaxFragmentLengthExtension(extensionData);
			}
			return -1;
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x001180C0 File Offset: 0x001162C0
		public static int GetPaddingExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 21);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadPaddingExtension(extensionData);
			}
			return -1;
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x001180E4 File Offset: 0x001162E4
		public static byte[] GetServerCertificateTypeExtensionClient(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 20);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadCertificateTypeExtensionClient(extensionData);
			}
			return null;
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00118108 File Offset: 0x00116308
		public static short GetServerCertificateTypeExtensionServer(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 20);
			if (extensionData != null)
			{
				return (short)TlsExtensionsUtilities.ReadCertificateTypeExtensionServer(extensionData);
			}
			return -1;
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x0011812C File Offset: 0x0011632C
		public static ServerNameList GetServerNameExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 0);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadServerNameExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x0011814C File Offset: 0x0011634C
		public static CertificateStatusRequest GetStatusRequestExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 5);
			if (extensionData != null)
			{
				return TlsExtensionsUtilities.ReadStatusRequestExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x0011816C File Offset: 0x0011636C
		public static bool HasEncryptThenMacExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 22);
			return extensionData != null && TlsExtensionsUtilities.ReadEncryptThenMacExtension(extensionData);
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x00118190 File Offset: 0x00116390
		public static bool HasExtendedMasterSecretExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 23);
			return extensionData != null && TlsExtensionsUtilities.ReadExtendedMasterSecretExtension(extensionData);
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x001181B4 File Offset: 0x001163B4
		public static bool HasTruncatedHMacExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 4);
			return extensionData != null && TlsExtensionsUtilities.ReadTruncatedHMacExtension(extensionData);
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x00116990 File Offset: 0x00114B90
		public static byte[] CreateCertificateTypeExtensionClient(byte[] certificateTypes)
		{
			if (certificateTypes == null || certificateTypes.Length < 1 || certificateTypes.Length > 255)
			{
				throw new TlsFatalAlert(80);
			}
			return TlsUtilities.EncodeUint8ArrayWithUint8Length(certificateTypes);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x001169B3 File Offset: 0x00114BB3
		public static byte[] CreateCertificateTypeExtensionServer(byte certificateType)
		{
			return TlsUtilities.EncodeUint8(certificateType);
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x001181D4 File Offset: 0x001163D4
		public static byte[] CreateEmptyExtensionData()
		{
			return TlsUtilities.EmptyBytes;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x001181DB File Offset: 0x001163DB
		public static byte[] CreateEncryptThenMacExtension()
		{
			return TlsExtensionsUtilities.CreateEmptyExtensionData();
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x001181DB File Offset: 0x001163DB
		public static byte[] CreateExtendedMasterSecretExtension()
		{
			return TlsExtensionsUtilities.CreateEmptyExtensionData();
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x001181E4 File Offset: 0x001163E4
		public static byte[] CreateHeartbeatExtension(HeartbeatExtension heartbeatExtension)
		{
			if (heartbeatExtension == null)
			{
				throw new TlsFatalAlert(80);
			}
			MemoryStream memoryStream = new MemoryStream();
			heartbeatExtension.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x001169B3 File Offset: 0x00114BB3
		public static byte[] CreateMaxFragmentLengthExtension(byte maxFragmentLength)
		{
			return TlsUtilities.EncodeUint8(maxFragmentLength);
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x0011820F File Offset: 0x0011640F
		public static byte[] CreatePaddingExtension(int dataLength)
		{
			TlsUtilities.CheckUint16(dataLength);
			return new byte[dataLength];
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00118220 File Offset: 0x00116420
		public static byte[] CreateServerNameExtension(ServerNameList serverNameList)
		{
			if (serverNameList == null)
			{
				throw new TlsFatalAlert(80);
			}
			MemoryStream memoryStream = new MemoryStream();
			serverNameList.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x0011824C File Offset: 0x0011644C
		public static byte[] CreateStatusRequestExtension(CertificateStatusRequest statusRequest)
		{
			if (statusRequest == null)
			{
				throw new TlsFatalAlert(80);
			}
			MemoryStream memoryStream = new MemoryStream();
			statusRequest.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x001181DB File Offset: 0x001163DB
		public static byte[] CreateTruncatedHMacExtension()
		{
			return TlsExtensionsUtilities.CreateEmptyExtensionData();
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x00118277 File Offset: 0x00116477
		private static bool ReadEmptyExtensionData(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			if (extensionData.Length != 0)
			{
				throw new TlsFatalAlert(47);
			}
			return true;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x001169BB File Offset: 0x00114BBB
		public static byte[] ReadCertificateTypeExtensionClient(byte[] extensionData)
		{
			byte[] array = TlsUtilities.DecodeUint8ArrayWithUint8Length(extensionData);
			if (array.Length < 1)
			{
				throw new TlsFatalAlert(50);
			}
			return array;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x001169D1 File Offset: 0x00114BD1
		public static byte ReadCertificateTypeExtensionServer(byte[] extensionData)
		{
			return TlsUtilities.DecodeUint8(extensionData);
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x00118294 File Offset: 0x00116494
		public static bool ReadEncryptThenMacExtension(byte[] extensionData)
		{
			return TlsExtensionsUtilities.ReadEmptyExtensionData(extensionData);
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x00118294 File Offset: 0x00116494
		public static bool ReadExtendedMasterSecretExtension(byte[] extensionData)
		{
			return TlsExtensionsUtilities.ReadEmptyExtensionData(extensionData);
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x0011829C File Offset: 0x0011649C
		public static HeartbeatExtension ReadHeartbeatExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			HeartbeatExtension result = HeartbeatExtension.Parse(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x001169D1 File Offset: 0x00114BD1
		public static byte ReadMaxFragmentLengthExtension(byte[] extensionData)
		{
			return TlsUtilities.DecodeUint8(extensionData);
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x001182CC File Offset: 0x001164CC
		public static int ReadPaddingExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			for (int i = 0; i < extensionData.Length; i++)
			{
				if (extensionData[i] != 0)
				{
					throw new TlsFatalAlert(47);
				}
			}
			return extensionData.Length;
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x00118308 File Offset: 0x00116508
		public static ServerNameList ReadServerNameExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			ServerNameList result = ServerNameList.Parse(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x00118338 File Offset: 0x00116538
		public static CertificateStatusRequest ReadStatusRequestExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			CertificateStatusRequest result = CertificateStatusRequest.Parse(memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x00118294 File Offset: 0x00116494
		public static bool ReadTruncatedHMacExtension(byte[] extensionData)
		{
			return TlsExtensionsUtilities.ReadEmptyExtensionData(extensionData);
		}
	}
}
