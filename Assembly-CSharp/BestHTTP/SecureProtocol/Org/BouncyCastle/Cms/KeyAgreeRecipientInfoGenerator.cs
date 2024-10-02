using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Ecc;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200060C RID: 1548
	internal class KeyAgreeRecipientInfoGenerator : RecipientInfoGenerator
	{
		// Token: 0x06003ABA RID: 15034 RVA: 0x00022F1F File Offset: 0x0002111F
		internal KeyAgreeRecipientInfoGenerator()
		{
		}

		// Token: 0x17000791 RID: 1937
		// (set) Token: 0x06003ABB RID: 15035 RVA: 0x0016BEBC File Offset: 0x0016A0BC
		internal DerObjectIdentifier KeyAgreementOID
		{
			set
			{
				this.keyAgreementOID = value;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (set) Token: 0x06003ABC RID: 15036 RVA: 0x0016BEC5 File Offset: 0x0016A0C5
		internal DerObjectIdentifier KeyEncryptionOID
		{
			set
			{
				this.keyEncryptionOID = value;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (set) Token: 0x06003ABD RID: 15037 RVA: 0x0016BECE File Offset: 0x0016A0CE
		internal ICollection RecipientCerts
		{
			set
			{
				this.recipientCerts = Platform.CreateArrayList(value);
			}
		}

		// Token: 0x17000794 RID: 1940
		// (set) Token: 0x06003ABE RID: 15038 RVA: 0x0016BEDC File Offset: 0x0016A0DC
		internal AsymmetricCipherKeyPair SenderKeyPair
		{
			set
			{
				this.senderKeyPair = value;
			}
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x0016BEE8 File Offset: 0x0016A0E8
		public RecipientInfo Generate(KeyParameter contentEncryptionKey, SecureRandom random)
		{
			byte[] key = contentEncryptionKey.GetKey();
			AsymmetricKeyParameter @public = this.senderKeyPair.Public;
			ICipherParameters cipherParameters = this.senderKeyPair.Private;
			OriginatorIdentifierOrKey originator;
			try
			{
				originator = new OriginatorIdentifierOrKey(KeyAgreeRecipientInfoGenerator.CreateOriginatorPublicKey(@public));
			}
			catch (IOException arg)
			{
				throw new InvalidKeyException("cannot extract originator public key: " + arg);
			}
			Asn1OctetString ukm = null;
			if (this.keyAgreementOID.Id.Equals(CmsEnvelopedGenerator.ECMqvSha1Kdf))
			{
				try
				{
					IAsymmetricCipherKeyPairGenerator keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator(this.keyAgreementOID);
					keyPairGenerator.Init(((ECPublicKeyParameters)@public).CreateKeyGenerationParameters(random));
					AsymmetricCipherKeyPair asymmetricCipherKeyPair = keyPairGenerator.GenerateKeyPair();
					ukm = new DerOctetString(new MQVuserKeyingMaterial(KeyAgreeRecipientInfoGenerator.CreateOriginatorPublicKey(asymmetricCipherKeyPair.Public), null));
					cipherParameters = new MqvPrivateParameters((ECPrivateKeyParameters)cipherParameters, (ECPrivateKeyParameters)asymmetricCipherKeyPair.Private, (ECPublicKeyParameters)asymmetricCipherKeyPair.Public);
				}
				catch (IOException arg2)
				{
					throw new InvalidKeyException("cannot extract MQV ephemeral public key: " + arg2);
				}
				catch (SecurityUtilityException arg3)
				{
					throw new InvalidKeyException("cannot determine MQV ephemeral key pair parameters from public key: " + arg3);
				}
			}
			DerSequence parameters = new DerSequence(new Asn1Encodable[]
			{
				this.keyEncryptionOID,
				DerNull.Instance
			});
			AlgorithmIdentifier keyEncryptionAlgorithm = new AlgorithmIdentifier(this.keyAgreementOID, parameters);
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.recipientCerts)
			{
				X509Certificate x509Certificate = (X509Certificate)obj;
				TbsCertificateStructure instance;
				try
				{
					instance = TbsCertificateStructure.GetInstance(Asn1Object.FromByteArray(x509Certificate.GetTbsCertificate()));
				}
				catch (Exception)
				{
					throw new ArgumentException("can't extract TBS structure from certificate");
				}
				KeyAgreeRecipientIdentifier id = new KeyAgreeRecipientIdentifier(new IssuerAndSerialNumber(instance.Issuer, instance.SerialNumber.Value));
				ICipherParameters cipherParameters2 = x509Certificate.GetPublicKey();
				if (this.keyAgreementOID.Id.Equals(CmsEnvelopedGenerator.ECMqvSha1Kdf))
				{
					cipherParameters2 = new MqvPublicParameters((ECPublicKeyParameters)cipherParameters2, (ECPublicKeyParameters)cipherParameters2);
				}
				IBasicAgreement basicAgreementWithKdf = AgreementUtilities.GetBasicAgreementWithKdf(this.keyAgreementOID, this.keyEncryptionOID.Id);
				basicAgreementWithKdf.Init(new ParametersWithRandom(cipherParameters, random));
				BigInteger s = basicAgreementWithKdf.CalculateAgreement(cipherParameters2);
				int qLength = GeneratorUtilities.GetDefaultKeySize(this.keyEncryptionOID) / 8;
				byte[] keyBytes = X9IntegerConverter.IntegerToBytes(s, qLength);
				KeyParameter parameters2 = ParameterUtilities.CreateKeyParameter(this.keyEncryptionOID, keyBytes);
				IWrapper wrapper = KeyAgreeRecipientInfoGenerator.Helper.CreateWrapper(this.keyEncryptionOID.Id);
				wrapper.Init(true, new ParametersWithRandom(parameters2, random));
				Asn1OctetString encryptedKey = new DerOctetString(wrapper.Wrap(key, 0, key.Length));
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new RecipientEncryptedKey(id, encryptedKey)
				});
			}
			return new RecipientInfo(new KeyAgreeRecipientInfo(originator, ukm, keyEncryptionAlgorithm, new DerSequence(asn1EncodableVector)));
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x0016C200 File Offset: 0x0016A400
		private static OriginatorPublicKey CreateOriginatorPublicKey(AsymmetricKeyParameter publicKey)
		{
			SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
			return new OriginatorPublicKey(new AlgorithmIdentifier(subjectPublicKeyInfo.AlgorithmID.Algorithm, DerNull.Instance), subjectPublicKeyInfo.PublicKeyData.GetBytes());
		}

		// Token: 0x0400265E RID: 9822
		private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;

		// Token: 0x0400265F RID: 9823
		private DerObjectIdentifier keyAgreementOID;

		// Token: 0x04002660 RID: 9824
		private DerObjectIdentifier keyEncryptionOID;

		// Token: 0x04002661 RID: 9825
		private IList recipientCerts;

		// Token: 0x04002662 RID: 9826
		private AsymmetricCipherKeyPair senderKeyPair;
	}
}
