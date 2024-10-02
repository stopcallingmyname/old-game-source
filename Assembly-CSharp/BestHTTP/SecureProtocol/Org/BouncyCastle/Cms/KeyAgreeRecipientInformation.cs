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
using BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200060D RID: 1549
	public class KeyAgreeRecipientInformation : RecipientInformation
	{
		// Token: 0x06003AC2 RID: 15042 RVA: 0x0016C248 File Offset: 0x0016A448
		internal static void ReadRecipientInfo(IList infos, KeyAgreeRecipientInfo info, CmsSecureReadable secureReadable)
		{
			try
			{
				foreach (object obj in info.RecipientEncryptedKeys)
				{
					RecipientEncryptedKey instance = RecipientEncryptedKey.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
					RecipientID recipientID = new RecipientID();
					KeyAgreeRecipientIdentifier identifier = instance.Identifier;
					IssuerAndSerialNumber issuerAndSerialNumber = identifier.IssuerAndSerialNumber;
					if (issuerAndSerialNumber != null)
					{
						recipientID.Issuer = issuerAndSerialNumber.Name;
						recipientID.SerialNumber = issuerAndSerialNumber.SerialNumber.Value;
					}
					else
					{
						RecipientKeyIdentifier rkeyID = identifier.RKeyID;
						recipientID.SubjectKeyIdentifier = rkeyID.SubjectKeyIdentifier.GetOctets();
					}
					infos.Add(new KeyAgreeRecipientInformation(info, recipientID, instance.EncryptedKey, secureReadable));
				}
			}
			catch (IOException innerException)
			{
				throw new ArgumentException("invalid rid in KeyAgreeRecipientInformation", innerException);
			}
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x0016C330 File Offset: 0x0016A530
		internal KeyAgreeRecipientInformation(KeyAgreeRecipientInfo info, RecipientID rid, Asn1OctetString encryptedKey, CmsSecureReadable secureReadable) : base(info.KeyEncryptionAlgorithm, secureReadable)
		{
			this.info = info;
			this.rid = rid;
			this.encryptedKey = encryptedKey;
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x0016C358 File Offset: 0x0016A558
		private AsymmetricKeyParameter GetSenderPublicKey(AsymmetricKeyParameter receiverPrivateKey, OriginatorIdentifierOrKey originator)
		{
			OriginatorPublicKey originatorPublicKey = originator.OriginatorPublicKey;
			if (originatorPublicKey != null)
			{
				return this.GetPublicKeyFromOriginatorPublicKey(receiverPrivateKey, originatorPublicKey);
			}
			OriginatorID originatorID = new OriginatorID();
			IssuerAndSerialNumber issuerAndSerialNumber = originator.IssuerAndSerialNumber;
			if (issuerAndSerialNumber != null)
			{
				originatorID.Issuer = issuerAndSerialNumber.Name;
				originatorID.SerialNumber = issuerAndSerialNumber.SerialNumber.Value;
			}
			else
			{
				SubjectKeyIdentifier subjectKeyIdentifier = originator.SubjectKeyIdentifier;
				originatorID.SubjectKeyIdentifier = subjectKeyIdentifier.GetKeyIdentifier();
			}
			return this.GetPublicKeyFromOriginatorID(originatorID);
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x0016C3C1 File Offset: 0x0016A5C1
		private AsymmetricKeyParameter GetPublicKeyFromOriginatorPublicKey(AsymmetricKeyParameter receiverPrivateKey, OriginatorPublicKey originatorPublicKey)
		{
			return PublicKeyFactory.CreateKey(new SubjectPublicKeyInfo(PrivateKeyInfoFactory.CreatePrivateKeyInfo(receiverPrivateKey).PrivateKeyAlgorithm, originatorPublicKey.PublicKey.GetBytes()));
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x0016C3E3 File Offset: 0x0016A5E3
		private AsymmetricKeyParameter GetPublicKeyFromOriginatorID(OriginatorID origID)
		{
			throw new CmsException("No support for 'originator' as IssuerAndSerialNumber or SubjectKeyIdentifier");
		}

		// Token: 0x06003AC7 RID: 15047 RVA: 0x0016C3F0 File Offset: 0x0016A5F0
		private KeyParameter CalculateAgreedWrapKey(string wrapAlg, AsymmetricKeyParameter senderPublicKey, AsymmetricKeyParameter receiverPrivateKey)
		{
			DerObjectIdentifier algorithm = this.keyEncAlg.Algorithm;
			ICipherParameters cipherParameters = senderPublicKey;
			ICipherParameters cipherParameters2 = receiverPrivateKey;
			if (algorithm.Id.Equals(CmsEnvelopedGenerator.ECMqvSha1Kdf))
			{
				MQVuserKeyingMaterial instance = MQVuserKeyingMaterial.GetInstance(Asn1Object.FromByteArray(this.info.UserKeyingMaterial.GetOctets()));
				AsymmetricKeyParameter publicKeyFromOriginatorPublicKey = this.GetPublicKeyFromOriginatorPublicKey(receiverPrivateKey, instance.EphemeralPublicKey);
				cipherParameters = new MqvPublicParameters((ECPublicKeyParameters)cipherParameters, (ECPublicKeyParameters)publicKeyFromOriginatorPublicKey);
				cipherParameters2 = new MqvPrivateParameters((ECPrivateKeyParameters)cipherParameters2, (ECPrivateKeyParameters)cipherParameters2);
			}
			IBasicAgreement basicAgreementWithKdf = AgreementUtilities.GetBasicAgreementWithKdf(algorithm, wrapAlg);
			basicAgreementWithKdf.Init(cipherParameters2);
			BigInteger s = basicAgreementWithKdf.CalculateAgreement(cipherParameters);
			int qLength = GeneratorUtilities.GetDefaultKeySize(wrapAlg) / 8;
			byte[] keyBytes = X9IntegerConverter.IntegerToBytes(s, qLength);
			return ParameterUtilities.CreateKeyParameter(wrapAlg, keyBytes);
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x0016C49C File Offset: 0x0016A69C
		private KeyParameter UnwrapSessionKey(string wrapAlg, KeyParameter agreedKey)
		{
			byte[] octets = this.encryptedKey.GetOctets();
			IWrapper wrapper = WrapperUtilities.GetWrapper(wrapAlg);
			wrapper.Init(false, agreedKey);
			byte[] keyBytes = wrapper.Unwrap(octets, 0, octets.Length);
			return ParameterUtilities.CreateKeyParameter(base.GetContentAlgorithmName(), keyBytes);
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x0016C4DC File Offset: 0x0016A6DC
		internal KeyParameter GetSessionKey(AsymmetricKeyParameter receiverPrivateKey)
		{
			KeyParameter result;
			try
			{
				string id = DerObjectIdentifier.GetInstance(Asn1Sequence.GetInstance(this.keyEncAlg.Parameters)[0]).Id;
				AsymmetricKeyParameter senderPublicKey = this.GetSenderPublicKey(receiverPrivateKey, this.info.Originator);
				KeyParameter agreedKey = this.CalculateAgreedWrapKey(id, senderPublicKey, receiverPrivateKey);
				result = this.UnwrapSessionKey(id, agreedKey);
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e2)
			{
				throw new CmsException("key invalid in message.", e2);
			}
			catch (Exception e3)
			{
				throw new CmsException("originator key invalid.", e3);
			}
			return result;
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x0016C588 File Offset: 0x0016A788
		public override CmsTypedStream GetContentStream(ICipherParameters key)
		{
			if (!(key is AsymmetricKeyParameter))
			{
				throw new ArgumentException("KeyAgreement requires asymmetric key", "key");
			}
			AsymmetricKeyParameter asymmetricKeyParameter = (AsymmetricKeyParameter)key;
			if (!asymmetricKeyParameter.IsPrivate)
			{
				throw new ArgumentException("Expected private key", "key");
			}
			KeyParameter sessionKey = this.GetSessionKey(asymmetricKeyParameter);
			return base.GetContentFromSessionKey(sessionKey);
		}

		// Token: 0x04002663 RID: 9827
		private KeyAgreeRecipientInfo info;

		// Token: 0x04002664 RID: 9828
		private Asn1OctetString encryptedKey;
	}
}
