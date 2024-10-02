using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200060B RID: 1547
	public class KekRecipientInformation : RecipientInformation
	{
		// Token: 0x06003AB8 RID: 15032 RVA: 0x0016BDD8 File Offset: 0x00169FD8
		internal KekRecipientInformation(KekRecipientInfo info, CmsSecureReadable secureReadable) : base(info.KeyEncryptionAlgorithm, secureReadable)
		{
			this.info = info;
			this.rid = new RecipientID();
			KekIdentifier kekID = info.KekID;
			this.rid.KeyIdentifier = kekID.KeyIdentifier.GetOctets();
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x0016BE24 File Offset: 0x0016A024
		public override CmsTypedStream GetContentStream(ICipherParameters key)
		{
			CmsTypedStream contentFromSessionKey;
			try
			{
				byte[] octets = this.info.EncryptedKey.GetOctets();
				IWrapper wrapper = WrapperUtilities.GetWrapper(this.keyEncAlg.Algorithm.Id);
				wrapper.Init(false, key);
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

		// Token: 0x0400265D RID: 9821
		private KekRecipientInfo info;
	}
}
