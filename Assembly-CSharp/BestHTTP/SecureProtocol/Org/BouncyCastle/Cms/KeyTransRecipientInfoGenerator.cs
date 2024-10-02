using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200060E RID: 1550
	internal class KeyTransRecipientInfoGenerator : RecipientInfoGenerator
	{
		// Token: 0x06003ACB RID: 15051 RVA: 0x00022F1F File Offset: 0x0002111F
		internal KeyTransRecipientInfoGenerator()
		{
		}

		// Token: 0x17000795 RID: 1941
		// (set) Token: 0x06003ACC RID: 15052 RVA: 0x0016C5DB File Offset: 0x0016A7DB
		internal X509Certificate RecipientCert
		{
			set
			{
				this.recipientTbsCert = CmsUtilities.GetTbsCertificateStructure(value);
				this.recipientPublicKey = value.GetPublicKey();
				this.info = this.recipientTbsCert.SubjectPublicKeyInfo;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x0016C608 File Offset: 0x0016A808
		internal AsymmetricKeyParameter RecipientPublicKey
		{
			set
			{
				this.recipientPublicKey = value;
				try
				{
					this.info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(this.recipientPublicKey);
				}
				catch (IOException)
				{
					throw new ArgumentException("can't extract key algorithm from this key");
				}
			}
		}

		// Token: 0x17000797 RID: 1943
		// (set) Token: 0x06003ACE RID: 15054 RVA: 0x0016C64C File Offset: 0x0016A84C
		internal Asn1OctetString SubjectKeyIdentifier
		{
			set
			{
				this.subjectKeyIdentifier = value;
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x0016C658 File Offset: 0x0016A858
		public RecipientInfo Generate(KeyParameter contentEncryptionKey, SecureRandom random)
		{
			byte[] key = contentEncryptionKey.GetKey();
			AlgorithmIdentifier algorithmID = this.info.AlgorithmID;
			IWrapper wrapper = KeyTransRecipientInfoGenerator.Helper.CreateWrapper(algorithmID.Algorithm.Id);
			wrapper.Init(true, new ParametersWithRandom(this.recipientPublicKey, random));
			byte[] str = wrapper.Wrap(key, 0, key.Length);
			RecipientIdentifier rid;
			if (this.recipientTbsCert != null)
			{
				rid = new RecipientIdentifier(new IssuerAndSerialNumber(this.recipientTbsCert.Issuer, this.recipientTbsCert.SerialNumber.Value));
			}
			else
			{
				rid = new RecipientIdentifier(this.subjectKeyIdentifier);
			}
			return new RecipientInfo(new KeyTransRecipientInfo(rid, algorithmID, new DerOctetString(str)));
		}

		// Token: 0x04002665 RID: 9829
		private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;

		// Token: 0x04002666 RID: 9830
		private TbsCertificateStructure recipientTbsCert;

		// Token: 0x04002667 RID: 9831
		private AsymmetricKeyParameter recipientPublicKey;

		// Token: 0x04002668 RID: 9832
		private Asn1OctetString subjectKeyIdentifier;

		// Token: 0x04002669 RID: 9833
		private SubjectPublicKeyInfo info;
	}
}
