using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000443 RID: 1091
	public class ServerName
	{
		// Token: 0x06002B0D RID: 11021 RVA: 0x0011406C File Offset: 0x0011226C
		public ServerName(byte nameType, object name)
		{
			if (!ServerName.IsCorrectType(nameType, name))
			{
				throw new ArgumentException("not an instance of the correct type", "name");
			}
			this.mNameType = nameType;
			this.mName = name;
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x0011409B File Offset: 0x0011229B
		public virtual byte NameType
		{
			get
			{
				return this.mNameType;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x001140A3 File Offset: 0x001122A3
		public virtual object Name
		{
			get
			{
				return this.mName;
			}
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x001140AB File Offset: 0x001122AB
		public virtual string GetHostName()
		{
			if (!ServerName.IsCorrectType(0, this.mName))
			{
				throw new InvalidOperationException("'name' is not a HostName string");
			}
			return (string)this.mName;
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x001140D4 File Offset: 0x001122D4
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mNameType, output);
			if (this.mNameType != 0)
			{
				throw new TlsFatalAlert(80);
			}
			byte[] array = Strings.ToAsciiByteArray((string)this.mName);
			if (array.Length < 1)
			{
				throw new TlsFatalAlert(80);
			}
			TlsUtilities.WriteOpaque16(array, output);
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x00114124 File Offset: 0x00112324
		public static ServerName Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (b != 0)
			{
				throw new TlsFatalAlert(50);
			}
			byte[] array = TlsUtilities.ReadOpaque16(input);
			if (array.Length < 1)
			{
				throw new TlsFatalAlert(50);
			}
			object name = Strings.FromAsciiByteArray(array);
			return new ServerName(b, name);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x00114166 File Offset: 0x00112366
		protected static bool IsCorrectType(byte nameType, object name)
		{
			if (nameType == 0)
			{
				return name is string;
			}
			throw new ArgumentException("unsupported NameType", "nameType");
		}

		// Token: 0x04001DD6 RID: 7638
		protected readonly byte mNameType;

		// Token: 0x04001DD7 RID: 7639
		protected readonly object mName;
	}
}
