using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000619 RID: 1561
	public abstract class RecipientInformation
	{
		// Token: 0x06003AF9 RID: 15097 RVA: 0x0016CF07 File Offset: 0x0016B107
		internal RecipientInformation(AlgorithmIdentifier keyEncAlg, CmsSecureReadable secureReadable)
		{
			this.keyEncAlg = keyEncAlg;
			this.secureReadable = secureReadable;
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0016CF28 File Offset: 0x0016B128
		internal string GetContentAlgorithmName()
		{
			return this.secureReadable.Algorithm.Algorithm.Id;
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x0016CF3F File Offset: 0x0016B13F
		public RecipientID RecipientID
		{
			get
			{
				return this.rid;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x0016CF47 File Offset: 0x0016B147
		public AlgorithmIdentifier KeyEncryptionAlgorithmID
		{
			get
			{
				return this.keyEncAlg;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x0016CF4F File Offset: 0x0016B14F
		public string KeyEncryptionAlgOid
		{
			get
			{
				return this.keyEncAlg.Algorithm.Id;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06003AFE RID: 15102 RVA: 0x0016CF64 File Offset: 0x0016B164
		public Asn1Object KeyEncryptionAlgParams
		{
			get
			{
				Asn1Encodable parameters = this.keyEncAlg.Parameters;
				if (parameters != null)
				{
					return parameters.ToAsn1Object();
				}
				return null;
			}
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x0016CF88 File Offset: 0x0016B188
		internal CmsTypedStream GetContentFromSessionKey(KeyParameter sKey)
		{
			CmsReadable readable = this.secureReadable.GetReadable(sKey);
			CmsTypedStream result;
			try
			{
				result = new CmsTypedStream(readable.GetInputStream());
			}
			catch (IOException e)
			{
				throw new CmsException("error getting .", e);
			}
			return result;
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0016CFD0 File Offset: 0x0016B1D0
		public byte[] GetContent(ICipherParameters key)
		{
			byte[] result;
			try
			{
				result = CmsUtilities.StreamToByteArray(this.GetContentStream(key).ContentStream);
			}
			catch (IOException arg)
			{
				throw new Exception("unable to parse internal stream: " + arg);
			}
			return result;
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0016D014 File Offset: 0x0016B214
		public byte[] GetMac()
		{
			if (this.resultMac == null)
			{
				object cryptoObject = this.secureReadable.CryptoObject;
				if (cryptoObject is IMac)
				{
					this.resultMac = MacUtilities.DoFinal((IMac)cryptoObject);
				}
			}
			return Arrays.Clone(this.resultMac);
		}

		// Token: 0x06003B02 RID: 15106
		public abstract CmsTypedStream GetContentStream(ICipherParameters key);

		// Token: 0x04002674 RID: 9844
		internal RecipientID rid = new RecipientID();

		// Token: 0x04002675 RID: 9845
		internal AlgorithmIdentifier keyEncAlg;

		// Token: 0x04002676 RID: 9846
		internal CmsSecureReadable secureReadable;

		// Token: 0x04002677 RID: 9847
		private byte[] resultMac;
	}
}
