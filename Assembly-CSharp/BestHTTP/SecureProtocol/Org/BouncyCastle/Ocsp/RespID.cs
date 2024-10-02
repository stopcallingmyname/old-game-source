using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x02000301 RID: 769
	public class RespID
	{
		// Token: 0x06001BE3 RID: 7139 RVA: 0x000D192D File Offset: 0x000CFB2D
		public RespID(ResponderID id)
		{
			this.id = id;
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000D193C File Offset: 0x000CFB3C
		public RespID(X509Name name)
		{
			this.id = new ResponderID(name);
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000D1950 File Offset: 0x000CFB50
		public RespID(AsymmetricKeyParameter publicKey)
		{
			try
			{
				SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
				byte[] str = DigestUtilities.CalculateDigest("SHA1", subjectPublicKeyInfo.PublicKeyData.GetBytes());
				this.id = new ResponderID(new DerOctetString(str));
			}
			catch (Exception ex)
			{
				throw new OcspException("problem creating ID: " + ex, ex);
			}
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000D19B8 File Offset: 0x000CFBB8
		public ResponderID ToAsn1Object()
		{
			return this.id;
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000D19C0 File Offset: 0x000CFBC0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RespID respID = obj as RespID;
			return respID != null && this.id.Equals(respID.id);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000D19F0 File Offset: 0x000CFBF0
		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}

		// Token: 0x04001913 RID: 6419
		internal readonly ResponderID id;
	}
}
