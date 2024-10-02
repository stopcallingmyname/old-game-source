using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000613 RID: 1555
	internal class PasswordRecipientInfoGenerator : RecipientInfoGenerator
	{
		// Token: 0x06003AE0 RID: 15072 RVA: 0x00022F1F File Offset: 0x0002111F
		internal PasswordRecipientInfoGenerator()
		{
		}

		// Token: 0x17000798 RID: 1944
		// (set) Token: 0x06003AE1 RID: 15073 RVA: 0x0016CB64 File Offset: 0x0016AD64
		internal AlgorithmIdentifier KeyDerivationAlgorithm
		{
			set
			{
				this.keyDerivationAlgorithm = value;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (set) Token: 0x06003AE2 RID: 15074 RVA: 0x0016CB6D File Offset: 0x0016AD6D
		internal KeyParameter KeyEncryptionKey
		{
			set
			{
				this.keyEncryptionKey = value;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (set) Token: 0x06003AE3 RID: 15075 RVA: 0x0016CB76 File Offset: 0x0016AD76
		internal string KeyEncryptionKeyOID
		{
			set
			{
				this.keyEncryptionKeyOID = value;
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0016CB80 File Offset: 0x0016AD80
		public RecipientInfo Generate(KeyParameter contentEncryptionKey, SecureRandom random)
		{
			byte[] key = contentEncryptionKey.GetKey();
			string rfc3211WrapperName = PasswordRecipientInfoGenerator.Helper.GetRfc3211WrapperName(this.keyEncryptionKeyOID);
			IWrapper wrapper = PasswordRecipientInfoGenerator.Helper.CreateWrapper(rfc3211WrapperName);
			byte[] array = new byte[Platform.StartsWith(rfc3211WrapperName, "DESEDE") ? 8 : 16];
			random.NextBytes(array);
			ICipherParameters parameters = new ParametersWithIV(this.keyEncryptionKey, array);
			wrapper.Init(true, new ParametersWithRandom(parameters, random));
			Asn1OctetString encryptedKey = new DerOctetString(wrapper.Wrap(key, 0, key.Length));
			DerSequence parameters2 = new DerSequence(new Asn1Encodable[]
			{
				new DerObjectIdentifier(this.keyEncryptionKeyOID),
				new DerOctetString(array)
			});
			AlgorithmIdentifier keyEncryptionAlgorithm = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdAlgPwriKek, parameters2);
			return new RecipientInfo(new PasswordRecipientInfo(this.keyDerivationAlgorithm, keyEncryptionAlgorithm, encryptedKey));
		}

		// Token: 0x0400266E RID: 9838
		private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;

		// Token: 0x0400266F RID: 9839
		private AlgorithmIdentifier keyDerivationAlgorithm;

		// Token: 0x04002670 RID: 9840
		private KeyParameter keyEncryptionKey;

		// Token: 0x04002671 RID: 9841
		private string keyEncryptionKeyOID;
	}
}
