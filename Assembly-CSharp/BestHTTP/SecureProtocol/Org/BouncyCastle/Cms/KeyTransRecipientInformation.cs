using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200060F RID: 1551
	public class KeyTransRecipientInformation : RecipientInformation
	{
		// Token: 0x06003AD1 RID: 15057 RVA: 0x0016C708 File Offset: 0x0016A908
		internal KeyTransRecipientInformation(KeyTransRecipientInfo info, CmsSecureReadable secureReadable) : base(info.KeyEncryptionAlgorithm, secureReadable)
		{
			this.info = info;
			this.rid = new RecipientID();
			RecipientIdentifier recipientIdentifier = info.RecipientIdentifier;
			try
			{
				if (recipientIdentifier.IsTagged)
				{
					Asn1OctetString instance = Asn1OctetString.GetInstance(recipientIdentifier.ID);
					this.rid.SubjectKeyIdentifier = instance.GetOctets();
				}
				else
				{
					BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.IssuerAndSerialNumber instance2 = BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.IssuerAndSerialNumber.GetInstance(recipientIdentifier.ID);
					this.rid.Issuer = instance2.Name;
					this.rid.SerialNumber = instance2.SerialNumber.Value;
				}
			}
			catch (IOException)
			{
				throw new ArgumentException("invalid rid in KeyTransRecipientInformation");
			}
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x0016C7B4 File Offset: 0x0016A9B4
		private string GetExchangeEncryptionAlgorithmName(DerObjectIdentifier oid)
		{
			if (PkcsObjectIdentifiers.RsaEncryption.Equals(oid))
			{
				return "RSA//PKCS1Padding";
			}
			return oid.Id;
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x0016C7D0 File Offset: 0x0016A9D0
		internal KeyParameter UnwrapKey(ICipherParameters key)
		{
			byte[] octets = this.info.EncryptedKey.GetOctets();
			string exchangeEncryptionAlgorithmName = this.GetExchangeEncryptionAlgorithmName(this.keyEncAlg.Algorithm);
			KeyParameter result;
			try
			{
				IWrapper wrapper = WrapperUtilities.GetWrapper(exchangeEncryptionAlgorithmName);
				wrapper.Init(false, key);
				result = ParameterUtilities.CreateKeyParameter(base.GetContentAlgorithmName(), wrapper.Unwrap(octets, 0, octets.Length));
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e2)
			{
				throw new CmsException("key invalid in message.", e2);
			}
			catch (DataLengthException e3)
			{
				throw new CmsException("illegal blocksize in message.", e3);
			}
			catch (InvalidCipherTextException e4)
			{
				throw new CmsException("bad padding in message.", e4);
			}
			return result;
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x0016C89C File Offset: 0x0016AA9C
		public override CmsTypedStream GetContentStream(ICipherParameters key)
		{
			KeyParameter sKey = this.UnwrapKey(key);
			return base.GetContentFromSessionKey(sKey);
		}

		// Token: 0x0400266A RID: 9834
		private KeyTransRecipientInfo info;
	}
}
