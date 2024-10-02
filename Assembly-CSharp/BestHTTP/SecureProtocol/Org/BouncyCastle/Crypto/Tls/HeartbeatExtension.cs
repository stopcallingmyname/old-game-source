using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000432 RID: 1074
	public class HeartbeatExtension
	{
		// Token: 0x06002AA7 RID: 10919 RVA: 0x00113205 File Offset: 0x00111405
		public HeartbeatExtension(byte mode)
		{
			if (!HeartbeatMode.IsValid(mode))
			{
				throw new ArgumentException("not a valid HeartbeatMode value", "mode");
			}
			this.mMode = mode;
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x0011322C File Offset: 0x0011142C
		public virtual byte Mode
		{
			get
			{
				return this.mMode;
			}
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x00113234 File Offset: 0x00111434
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mMode, output);
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x00113242 File Offset: 0x00111442
		public static HeartbeatExtension Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (!HeartbeatMode.IsValid(b))
			{
				throw new TlsFatalAlert(47);
			}
			return new HeartbeatExtension(b);
		}

		// Token: 0x04001D50 RID: 7504
		protected readonly byte mMode;
	}
}
