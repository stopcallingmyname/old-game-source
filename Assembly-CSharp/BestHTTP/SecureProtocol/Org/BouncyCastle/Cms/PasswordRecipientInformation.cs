using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000614 RID: 1556
	public class PasswordRecipientInformation : RecipientInformation
	{
		// Token: 0x06003AE6 RID: 15078 RVA: 0x0016CC4D File Offset: 0x0016AE4D
		internal PasswordRecipientInformation(PasswordRecipientInfo info, CmsSecureReadable secureReadable) : base(info.KeyEncryptionAlgorithm, secureReadable)
		{
			this.info = info;
			this.rid = new RecipientID();
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x0016CC6E File Offset: 0x0016AE6E
		public virtual AlgorithmIdentifier KeyDerivationAlgorithm
		{
			get
			{
				return this.info.KeyDerivationAlgorithm;
			}
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x0016CC7C File Offset: 0x0016AE7C
		public override CmsTypedStream GetContentStream(ICipherParameters key)
		{
			CmsTypedStream contentFromSessionKey;
			try
			{
				Asn1Sequence asn1Sequence = (Asn1Sequence)AlgorithmIdentifier.GetInstance(this.info.KeyEncryptionAlgorithm).Parameters;
				byte[] octets = this.info.EncryptedKey.GetOctets();
				string id = DerObjectIdentifier.GetInstance(asn1Sequence[0]).Id;
				IWrapper wrapper = WrapperUtilities.GetWrapper(CmsEnvelopedHelper.Instance.GetRfc3211WrapperName(id));
				byte[] octets2 = Asn1OctetString.GetInstance(asn1Sequence[1]).GetOctets();
				ICipherParameters parameters = ((CmsPbeKey)key).GetEncoded(id);
				parameters = new ParametersWithIV(parameters, octets2);
				wrapper.Init(false, parameters);
				KeyParameter sKey = ParameterUtilities.CreateKeyParameter(base.GetContentAlgorithmName(), wrapper.Unwrap(octets, 0, octets.Length));
				contentFromSessionKey = base.GetContentFromSessionKey(sKey);
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e2)
			{
				throw new CmsException("key invalid in message.", e2);
			}
			return contentFromSessionKey;
		}

		// Token: 0x04002672 RID: 9842
		private readonly PasswordRecipientInfo info;
	}
}
