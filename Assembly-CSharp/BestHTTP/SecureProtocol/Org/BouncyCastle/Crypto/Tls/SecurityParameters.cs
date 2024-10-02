using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000442 RID: 1090
	public class SecurityParameters
	{
		// Token: 0x06002AFF RID: 11007 RVA: 0x00113FC4 File Offset: 0x001121C4
		internal virtual void Clear()
		{
			if (this.masterSecret != null)
			{
				Arrays.Fill(this.masterSecret, 0);
				this.masterSecret = null;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06002B00 RID: 11008 RVA: 0x00113FE1 File Offset: 0x001121E1
		public virtual int Entity
		{
			get
			{
				return this.entity;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x00113FE9 File Offset: 0x001121E9
		public virtual int CipherSuite
		{
			get
			{
				return this.cipherSuite;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06002B02 RID: 11010 RVA: 0x00113FF1 File Offset: 0x001121F1
		public virtual byte CompressionAlgorithm
		{
			get
			{
				return this.compressionAlgorithm;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x00113FF9 File Offset: 0x001121F9
		public virtual int PrfAlgorithm
		{
			get
			{
				return this.prfAlgorithm;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06002B04 RID: 11012 RVA: 0x00114001 File Offset: 0x00112201
		public virtual int VerifyDataLength
		{
			get
			{
				return this.verifyDataLength;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x00114009 File Offset: 0x00112209
		public virtual byte[] MasterSecret
		{
			get
			{
				return this.masterSecret;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x00114011 File Offset: 0x00112211
		public virtual byte[] ClientRandom
		{
			get
			{
				return this.clientRandom;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002B07 RID: 11015 RVA: 0x00114019 File Offset: 0x00112219
		public virtual byte[] ServerRandom
		{
			get
			{
				return this.serverRandom;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002B08 RID: 11016 RVA: 0x00114021 File Offset: 0x00112221
		public virtual byte[] SessionHash
		{
			get
			{
				return this.sessionHash;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x00114029 File Offset: 0x00112229
		public virtual byte[] PskIdentity
		{
			get
			{
				return this.pskIdentity;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002B0A RID: 11018 RVA: 0x00114031 File Offset: 0x00112231
		public virtual byte[] SrpIdentity
		{
			get
			{
				return this.srpIdentity;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002B0B RID: 11019 RVA: 0x00114039 File Offset: 0x00112239
		public virtual bool IsExtendedMasterSecret
		{
			get
			{
				return this.extendedMasterSecret;
			}
		}

		// Token: 0x04001DC7 RID: 7623
		internal int entity = -1;

		// Token: 0x04001DC8 RID: 7624
		internal int cipherSuite = -1;

		// Token: 0x04001DC9 RID: 7625
		internal byte compressionAlgorithm;

		// Token: 0x04001DCA RID: 7626
		internal int prfAlgorithm = -1;

		// Token: 0x04001DCB RID: 7627
		internal int verifyDataLength = -1;

		// Token: 0x04001DCC RID: 7628
		internal byte[] masterSecret;

		// Token: 0x04001DCD RID: 7629
		internal byte[] clientRandom;

		// Token: 0x04001DCE RID: 7630
		internal byte[] serverRandom;

		// Token: 0x04001DCF RID: 7631
		internal byte[] sessionHash;

		// Token: 0x04001DD0 RID: 7632
		internal byte[] pskIdentity;

		// Token: 0x04001DD1 RID: 7633
		internal byte[] srpIdentity;

		// Token: 0x04001DD2 RID: 7634
		internal short maxFragmentLength = -1;

		// Token: 0x04001DD3 RID: 7635
		internal bool truncatedHMac;

		// Token: 0x04001DD4 RID: 7636
		internal bool encryptThenMac;

		// Token: 0x04001DD5 RID: 7637
		internal bool extendedMasterSecret;
	}
}
