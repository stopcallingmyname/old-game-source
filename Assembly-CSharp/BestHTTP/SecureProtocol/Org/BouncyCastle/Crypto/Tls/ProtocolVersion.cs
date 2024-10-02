using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200043E RID: 1086
	public sealed class ProtocolVersion
	{
		// Token: 0x06002AC6 RID: 10950 RVA: 0x001135BA File Offset: 0x001117BA
		private ProtocolVersion(int v, string name)
		{
			this.version = (v & 65535);
			this.name = name;
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x001135D6 File Offset: 0x001117D6
		public int FullVersion
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002AC8 RID: 10952 RVA: 0x001135DE File Offset: 0x001117DE
		public int MajorVersion
		{
			get
			{
				return this.version >> 8;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x001135E8 File Offset: 0x001117E8
		public int MinorVersion
		{
			get
			{
				return this.version & 255;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002ACA RID: 10954 RVA: 0x001135F6 File Offset: 0x001117F6
		public bool IsDtls
		{
			get
			{
				return this.MajorVersion == 254;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x00113605 File Offset: 0x00111805
		public bool IsSsl
		{
			get
			{
				return this == ProtocolVersion.SSLv3;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x0011360F File Offset: 0x0011180F
		public bool IsTls
		{
			get
			{
				return this.MajorVersion == 3;
			}
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x0011361A File Offset: 0x0011181A
		public ProtocolVersion GetEquivalentTLSVersion()
		{
			if (!this.IsDtls)
			{
				return this;
			}
			if (this == ProtocolVersion.DTLSv10)
			{
				return ProtocolVersion.TLSv11;
			}
			return ProtocolVersion.TLSv12;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x0011363C File Offset: 0x0011183C
		public bool IsEqualOrEarlierVersionOf(ProtocolVersion version)
		{
			if (this.MajorVersion != version.MajorVersion)
			{
				return false;
			}
			int num = version.MinorVersion - this.MinorVersion;
			if (!this.IsDtls)
			{
				return num >= 0;
			}
			return num <= 0;
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x00113680 File Offset: 0x00111880
		public bool IsLaterVersionOf(ProtocolVersion version)
		{
			if (this.MajorVersion != version.MajorVersion)
			{
				return false;
			}
			int num = version.MinorVersion - this.MinorVersion;
			if (!this.IsDtls)
			{
				return num < 0;
			}
			return num > 0;
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x001136BC File Offset: 0x001118BC
		public override bool Equals(object other)
		{
			return this == other || (other is ProtocolVersion && this.Equals((ProtocolVersion)other));
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x001136DA File Offset: 0x001118DA
		public bool Equals(ProtocolVersion other)
		{
			return other != null && this.version == other.version;
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x001135D6 File Offset: 0x001117D6
		public override int GetHashCode()
		{
			return this.version;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x001136F0 File Offset: 0x001118F0
		public static ProtocolVersion Get(int major, int minor)
		{
			if (major != 3)
			{
				if (major != 254)
				{
					throw new TlsFatalAlert(47);
				}
				switch (minor)
				{
				case 253:
					return ProtocolVersion.DTLSv12;
				case 254:
					throw new TlsFatalAlert(47);
				case 255:
					return ProtocolVersion.DTLSv10;
				default:
					return ProtocolVersion.GetUnknownVersion(major, minor, "DTLS");
				}
			}
			else
			{
				switch (minor)
				{
				case 0:
					return ProtocolVersion.SSLv3;
				case 1:
					return ProtocolVersion.TLSv10;
				case 2:
					return ProtocolVersion.TLSv11;
				case 3:
					return ProtocolVersion.TLSv12;
				default:
					return ProtocolVersion.GetUnknownVersion(major, minor, "TLS");
				}
			}
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x0011378A File Offset: 0x0011198A
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x00113794 File Offset: 0x00111994
		private static ProtocolVersion GetUnknownVersion(int major, int minor, string prefix)
		{
			TlsUtilities.CheckUint8(major);
			TlsUtilities.CheckUint8(minor);
			int num = major << 8 | minor;
			string str = Platform.ToUpperInvariant(Convert.ToString(65536 | num, 16).Substring(1));
			return new ProtocolVersion(num, prefix + " 0x" + str);
		}

		// Token: 0x04001DA3 RID: 7587
		public static readonly ProtocolVersion SSLv3 = new ProtocolVersion(768, "SSL 3.0");

		// Token: 0x04001DA4 RID: 7588
		public static readonly ProtocolVersion TLSv10 = new ProtocolVersion(769, "TLS 1.0");

		// Token: 0x04001DA5 RID: 7589
		public static readonly ProtocolVersion TLSv11 = new ProtocolVersion(770, "TLS 1.1");

		// Token: 0x04001DA6 RID: 7590
		public static readonly ProtocolVersion TLSv12 = new ProtocolVersion(771, "TLS 1.2");

		// Token: 0x04001DA7 RID: 7591
		public static readonly ProtocolVersion DTLSv10 = new ProtocolVersion(65279, "DTLS 1.0");

		// Token: 0x04001DA8 RID: 7592
		public static readonly ProtocolVersion DTLSv12 = new ProtocolVersion(65277, "DTLS 1.2");

		// Token: 0x04001DA9 RID: 7593
		private readonly int version;

		// Token: 0x04001DAA RID: 7594
		private readonly string name;
	}
}
