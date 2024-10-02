using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000444 RID: 1092
	public class ServerNameList
	{
		// Token: 0x06002B14 RID: 11028 RVA: 0x00114184 File Offset: 0x00112384
		public ServerNameList(IList serverNameList)
		{
			if (serverNameList == null)
			{
				throw new ArgumentNullException("serverNameList");
			}
			this.mServerNameList = serverNameList;
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x001141A1 File Offset: 0x001123A1
		public virtual IList ServerNames
		{
			get
			{
				return this.mServerNameList;
			}
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x001141AC File Offset: 0x001123AC
		public virtual void Encode(Stream output)
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = TlsUtilities.EmptyBytes;
			foreach (object obj in this.ServerNames)
			{
				ServerName serverName = (ServerName)obj;
				array = ServerNameList.CheckNameType(array, serverName.NameType);
				if (array == null)
				{
					throw new TlsFatalAlert(80);
				}
				serverName.Encode(memoryStream);
			}
			TlsUtilities.CheckUint16(memoryStream.Length);
			TlsUtilities.WriteUint16((int)memoryStream.Length, output);
			Streams.WriteBufTo(memoryStream, output);
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x0011424C File Offset: 0x0011244C
		public static ServerNameList Parse(Stream input)
		{
			int num = TlsUtilities.ReadUint16(input);
			if (num < 1)
			{
				throw new TlsFatalAlert(50);
			}
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
			byte[] array = TlsUtilities.EmptyBytes;
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				ServerName serverName = ServerName.Parse(memoryStream);
				array = ServerNameList.CheckNameType(array, serverName.NameType);
				if (array == null)
				{
					throw new TlsFatalAlert(47);
				}
				list.Add(serverName);
			}
			return new ServerNameList(list);
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x001142C1 File Offset: 0x001124C1
		private static byte[] CheckNameType(byte[] nameTypesSeen, byte nameType)
		{
			if (!NameType.IsValid(nameType) || Arrays.Contains(nameTypesSeen, nameType))
			{
				return null;
			}
			return Arrays.Append(nameTypesSeen, nameType);
		}

		// Token: 0x04001DD8 RID: 7640
		protected readonly IList mServerNameList;
	}
}
