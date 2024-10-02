using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F3 RID: 1523
	internal class CmsEnvelopedHelper
	{
		// Token: 0x060039E0 RID: 14816 RVA: 0x001686F0 File Offset: 0x001668F0
		static CmsEnvelopedHelper()
		{
			CmsEnvelopedHelper.KeySizes.Add(CmsEnvelopedGenerator.DesEde3Cbc, 192);
			CmsEnvelopedHelper.KeySizes.Add(CmsEnvelopedGenerator.Aes128Cbc, 128);
			CmsEnvelopedHelper.KeySizes.Add(CmsEnvelopedGenerator.Aes192Cbc, 192);
			CmsEnvelopedHelper.KeySizes.Add(CmsEnvelopedGenerator.Aes256Cbc, 256);
			CmsEnvelopedHelper.BaseCipherNames.Add(CmsEnvelopedGenerator.DesEde3Cbc, "DESEDE");
			CmsEnvelopedHelper.BaseCipherNames.Add(CmsEnvelopedGenerator.Aes128Cbc, "AES");
			CmsEnvelopedHelper.BaseCipherNames.Add(CmsEnvelopedGenerator.Aes192Cbc, "AES");
			CmsEnvelopedHelper.BaseCipherNames.Add(CmsEnvelopedGenerator.Aes256Cbc, "AES");
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x001687CF File Offset: 0x001669CF
		private string GetAsymmetricEncryptionAlgName(string encryptionAlgOid)
		{
			if (PkcsObjectIdentifiers.RsaEncryption.Id.Equals(encryptionAlgOid))
			{
				return "RSA/ECB/PKCS1Padding";
			}
			return encryptionAlgOid;
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x001687EC File Offset: 0x001669EC
		internal IBufferedCipher CreateAsymmetricCipher(string encryptionOid)
		{
			string asymmetricEncryptionAlgName = this.GetAsymmetricEncryptionAlgName(encryptionOid);
			if (!asymmetricEncryptionAlgName.Equals(encryptionOid))
			{
				try
				{
					return CipherUtilities.GetCipher(asymmetricEncryptionAlgName);
				}
				catch (SecurityUtilityException)
				{
				}
			}
			return CipherUtilities.GetCipher(encryptionOid);
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x00168830 File Offset: 0x00166A30
		internal IWrapper CreateWrapper(string encryptionOid)
		{
			IWrapper wrapper;
			try
			{
				wrapper = WrapperUtilities.GetWrapper(encryptionOid);
			}
			catch (SecurityUtilityException)
			{
				wrapper = WrapperUtilities.GetWrapper(this.GetAsymmetricEncryptionAlgName(encryptionOid));
			}
			return wrapper;
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x00168868 File Offset: 0x00166A68
		internal string GetRfc3211WrapperName(string oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			string text = (string)CmsEnvelopedHelper.BaseCipherNames[oid];
			if (text == null)
			{
				throw new ArgumentException("no name for " + oid, "oid");
			}
			return text + "RFC3211Wrap";
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x001688B6 File Offset: 0x00166AB6
		internal int GetKeySize(string oid)
		{
			if (!CmsEnvelopedHelper.KeySizes.Contains(oid))
			{
				throw new ArgumentException("no keysize for " + oid, "oid");
			}
			return (int)CmsEnvelopedHelper.KeySizes[oid];
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x001688EC File Offset: 0x00166AEC
		internal static RecipientInformationStore BuildRecipientInformationStore(Asn1Set recipientInfos, CmsSecureReadable secureReadable)
		{
			IList list = Platform.CreateArrayList();
			for (int num = 0; num != recipientInfos.Count; num++)
			{
				RecipientInfo instance = RecipientInfo.GetInstance(recipientInfos[num]);
				CmsEnvelopedHelper.ReadRecipientInfo(list, instance, secureReadable);
			}
			return new RecipientInformationStore(list);
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x0016892C File Offset: 0x00166B2C
		private static void ReadRecipientInfo(IList infos, RecipientInfo info, CmsSecureReadable secureReadable)
		{
			Asn1Encodable info2 = info.Info;
			if (info2 is KeyTransRecipientInfo)
			{
				infos.Add(new KeyTransRecipientInformation((KeyTransRecipientInfo)info2, secureReadable));
				return;
			}
			if (info2 is KekRecipientInfo)
			{
				infos.Add(new KekRecipientInformation((KekRecipientInfo)info2, secureReadable));
				return;
			}
			if (info2 is KeyAgreeRecipientInfo)
			{
				KeyAgreeRecipientInformation.ReadRecipientInfo(infos, (KeyAgreeRecipientInfo)info2, secureReadable);
				return;
			}
			if (info2 is PasswordRecipientInfo)
			{
				infos.Add(new PasswordRecipientInformation((PasswordRecipientInfo)info2, secureReadable));
			}
		}

		// Token: 0x04002601 RID: 9729
		internal static readonly CmsEnvelopedHelper Instance = new CmsEnvelopedHelper();

		// Token: 0x04002602 RID: 9730
		private static readonly IDictionary KeySizes = Platform.CreateHashtable();

		// Token: 0x04002603 RID: 9731
		private static readonly IDictionary BaseCipherNames = Platform.CreateHashtable();

		// Token: 0x02000982 RID: 2434
		internal class CmsAuthenticatedSecureReadable : CmsSecureReadable
		{
			// Token: 0x06004FAF RID: 20399 RVA: 0x001B6F63 File Offset: 0x001B5163
			internal CmsAuthenticatedSecureReadable(AlgorithmIdentifier algorithm, CmsReadable readable)
			{
				this.algorithm = algorithm;
				this.readable = readable;
			}

			// Token: 0x17000C64 RID: 3172
			// (get) Token: 0x06004FB0 RID: 20400 RVA: 0x001B6F79 File Offset: 0x001B5179
			public AlgorithmIdentifier Algorithm
			{
				get
				{
					return this.algorithm;
				}
			}

			// Token: 0x17000C65 RID: 3173
			// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x001B6F81 File Offset: 0x001B5181
			public object CryptoObject
			{
				get
				{
					return this.mac;
				}
			}

			// Token: 0x06004FB2 RID: 20402 RVA: 0x001B6F8C File Offset: 0x001B518C
			public CmsReadable GetReadable(KeyParameter sKey)
			{
				string id = this.algorithm.Algorithm.Id;
				try
				{
					this.mac = MacUtilities.GetMac(id);
					this.mac.Init(sKey);
				}
				catch (SecurityUtilityException e)
				{
					throw new CmsException("couldn't create cipher.", e);
				}
				catch (InvalidKeyException e2)
				{
					throw new CmsException("key invalid in message.", e2);
				}
				catch (IOException e3)
				{
					throw new CmsException("error decoding algorithm parameters.", e3);
				}
				CmsReadable result;
				try
				{
					result = new CmsProcessableInputStream(new TeeInputStream(this.readable.GetInputStream(), new MacSink(this.mac)));
				}
				catch (IOException e4)
				{
					throw new CmsException("error reading content.", e4);
				}
				return result;
			}

			// Token: 0x040036E9 RID: 14057
			private AlgorithmIdentifier algorithm;

			// Token: 0x040036EA RID: 14058
			private IMac mac;

			// Token: 0x040036EB RID: 14059
			private CmsReadable readable;
		}

		// Token: 0x02000983 RID: 2435
		internal class CmsEnvelopedSecureReadable : CmsSecureReadable
		{
			// Token: 0x06004FB3 RID: 20403 RVA: 0x001B7058 File Offset: 0x001B5258
			internal CmsEnvelopedSecureReadable(AlgorithmIdentifier algorithm, CmsReadable readable)
			{
				this.algorithm = algorithm;
				this.readable = readable;
			}

			// Token: 0x17000C66 RID: 3174
			// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x001B706E File Offset: 0x001B526E
			public AlgorithmIdentifier Algorithm
			{
				get
				{
					return this.algorithm;
				}
			}

			// Token: 0x17000C67 RID: 3175
			// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x001B7076 File Offset: 0x001B5276
			public object CryptoObject
			{
				get
				{
					return this.cipher;
				}
			}

			// Token: 0x06004FB6 RID: 20406 RVA: 0x001B7080 File Offset: 0x001B5280
			public CmsReadable GetReadable(KeyParameter sKey)
			{
				try
				{
					this.cipher = CipherUtilities.GetCipher(this.algorithm.Algorithm);
					Asn1Encodable parameters = this.algorithm.Parameters;
					Asn1Object asn1Object = (parameters == null) ? null : parameters.ToAsn1Object();
					ICipherParameters cipherParameters = sKey;
					if (asn1Object != null && !(asn1Object is Asn1Null))
					{
						cipherParameters = ParameterUtilities.GetCipherParameters(this.algorithm.Algorithm, cipherParameters, asn1Object);
					}
					else
					{
						string id = this.algorithm.Algorithm.Id;
						if (id.Equals(CmsEnvelopedGenerator.DesEde3Cbc) || id.Equals("1.3.6.1.4.1.188.7.1.1.2") || id.Equals("1.2.840.113533.7.66.10"))
						{
							cipherParameters = new ParametersWithIV(cipherParameters, new byte[8]);
						}
					}
					this.cipher.Init(false, cipherParameters);
				}
				catch (SecurityUtilityException e)
				{
					throw new CmsException("couldn't create cipher.", e);
				}
				catch (InvalidKeyException e2)
				{
					throw new CmsException("key invalid in message.", e2);
				}
				catch (IOException e3)
				{
					throw new CmsException("error decoding algorithm parameters.", e3);
				}
				CmsReadable result;
				try
				{
					result = new CmsProcessableInputStream(new CipherStream(this.readable.GetInputStream(), this.cipher, null));
				}
				catch (IOException e4)
				{
					throw new CmsException("error reading content.", e4);
				}
				return result;
			}

			// Token: 0x040036EC RID: 14060
			private AlgorithmIdentifier algorithm;

			// Token: 0x040036ED RID: 14061
			private IBufferedCipher cipher;

			// Token: 0x040036EE RID: 14062
			private CmsReadable readable;
		}
	}
}
