using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005E7 RID: 1511
	internal class CmsAuthEnvelopedData
	{
		// Token: 0x0600399D RID: 14749 RVA: 0x001676E0 File Offset: 0x001658E0
		public CmsAuthEnvelopedData(byte[] authEnvData) : this(CmsUtilities.ReadContentInfo(authEnvData))
		{
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x001676EE File Offset: 0x001658EE
		public CmsAuthEnvelopedData(Stream authEnvData) : this(CmsUtilities.ReadContentInfo(authEnvData))
		{
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x001676FC File Offset: 0x001658FC
		public CmsAuthEnvelopedData(ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;
			AuthEnvelopedData instance = AuthEnvelopedData.GetInstance(contentInfo.Content);
			this.originator = instance.OriginatorInfo;
			Asn1Set recipientInfos = instance.RecipientInfos;
			EncryptedContentInfo authEncryptedContentInfo = instance.AuthEncryptedContentInfo;
			this.authEncAlg = authEncryptedContentInfo.ContentEncryptionAlgorithm;
			CmsSecureReadable secureReadable = new CmsAuthEnvelopedData.AuthEnvelopedSecureReadable(this);
			this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(recipientInfos, secureReadable);
			this.authAttrs = instance.AuthAttrs;
			this.mac = instance.Mac.GetOctets();
			this.unauthAttrs = instance.UnauthAttrs;
		}

		// Token: 0x040025C8 RID: 9672
		internal RecipientInformationStore recipientInfoStore;

		// Token: 0x040025C9 RID: 9673
		internal ContentInfo contentInfo;

		// Token: 0x040025CA RID: 9674
		private OriginatorInfo originator;

		// Token: 0x040025CB RID: 9675
		private AlgorithmIdentifier authEncAlg;

		// Token: 0x040025CC RID: 9676
		private Asn1Set authAttrs;

		// Token: 0x040025CD RID: 9677
		private byte[] mac;

		// Token: 0x040025CE RID: 9678
		private Asn1Set unauthAttrs;

		// Token: 0x0200097F RID: 2431
		private class AuthEnvelopedSecureReadable : CmsSecureReadable
		{
			// Token: 0x06004FA3 RID: 20387 RVA: 0x001B6DF8 File Offset: 0x001B4FF8
			internal AuthEnvelopedSecureReadable(CmsAuthEnvelopedData parent)
			{
				this.parent = parent;
			}

			// Token: 0x17000C62 RID: 3170
			// (get) Token: 0x06004FA4 RID: 20388 RVA: 0x001B6E07 File Offset: 0x001B5007
			public AlgorithmIdentifier Algorithm
			{
				get
				{
					return this.parent.authEncAlg;
				}
			}

			// Token: 0x17000C63 RID: 3171
			// (get) Token: 0x06004FA5 RID: 20389 RVA: 0x0008D54A File Offset: 0x0008B74A
			public object CryptoObject
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06004FA6 RID: 20390 RVA: 0x001B6E14 File Offset: 0x001B5014
			public CmsReadable GetReadable(KeyParameter key)
			{
				throw new CmsException("AuthEnveloped data decryption not yet implemented");
			}

			// Token: 0x040036DF RID: 14047
			private readonly CmsAuthEnvelopedData parent;
		}
	}
}
