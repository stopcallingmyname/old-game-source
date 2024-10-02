using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000491 RID: 1169
	public class UseSrtpData
	{
		// Token: 0x06002DFB RID: 11771 RVA: 0x0011EBA8 File Offset: 0x0011CDA8
		public UseSrtpData(int[] protectionProfiles, byte[] mki)
		{
			if (protectionProfiles == null || protectionProfiles.Length < 1 || protectionProfiles.Length >= 32768)
			{
				throw new ArgumentException("must have length from 1 to (2^15 - 1)", "protectionProfiles");
			}
			if (mki == null)
			{
				mki = TlsUtilities.EmptyBytes;
			}
			else if (mki.Length > 255)
			{
				throw new ArgumentException("cannot be longer than 255 bytes", "mki");
			}
			this.mProtectionProfiles = protectionProfiles;
			this.mMki = mki;
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002DFC RID: 11772 RVA: 0x0011EC12 File Offset: 0x0011CE12
		public virtual int[] ProtectionProfiles
		{
			get
			{
				return this.mProtectionProfiles;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002DFD RID: 11773 RVA: 0x0011EC1A File Offset: 0x0011CE1A
		public virtual byte[] Mki
		{
			get
			{
				return this.mMki;
			}
		}

		// Token: 0x04001EBE RID: 7870
		protected readonly int[] mProtectionProfiles;

		// Token: 0x04001EBF RID: 7871
		protected readonly byte[] mMki;
	}
}
