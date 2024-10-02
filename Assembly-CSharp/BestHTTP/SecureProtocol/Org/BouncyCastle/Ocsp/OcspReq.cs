using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002F8 RID: 760
	public class OcspReq : X509ExtensionBase
	{
		// Token: 0x06001BA8 RID: 7080 RVA: 0x000D0A48 File Offset: 0x000CEC48
		public OcspReq(OcspRequest req)
		{
			this.req = req;
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000D0A57 File Offset: 0x000CEC57
		public OcspReq(byte[] req) : this(new Asn1InputStream(req))
		{
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000D0A65 File Offset: 0x000CEC65
		public OcspReq(Stream inStr) : this(new Asn1InputStream(inStr))
		{
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000D0A74 File Offset: 0x000CEC74
		private OcspReq(Asn1InputStream aIn)
		{
			try
			{
				this.req = OcspRequest.GetInstance(aIn.ReadObject());
			}
			catch (ArgumentException ex)
			{
				throw new IOException("malformed request: " + ex.Message);
			}
			catch (InvalidCastException ex2)
			{
				throw new IOException("malformed request: " + ex2.Message);
			}
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x000D0AE4 File Offset: 0x000CECE4
		public byte[] GetTbsRequest()
		{
			byte[] encoded;
			try
			{
				encoded = this.req.TbsRequest.GetEncoded();
			}
			catch (IOException e)
			{
				throw new OcspException("problem encoding tbsRequest", e);
			}
			return encoded;
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x000D0B24 File Offset: 0x000CED24
		public int Version
		{
			get
			{
				return this.req.TbsRequest.Version.Value.IntValue + 1;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001BAE RID: 7086 RVA: 0x000D0B42 File Offset: 0x000CED42
		public GeneralName RequestorName
		{
			get
			{
				return GeneralName.GetInstance(this.req.TbsRequest.RequestorName);
			}
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000D0B5C File Offset: 0x000CED5C
		public Req[] GetRequestList()
		{
			Asn1Sequence requestList = this.req.TbsRequest.RequestList;
			Req[] array = new Req[requestList.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = new Req(Request.GetInstance(requestList[num]));
			}
			return array;
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x000D0BA9 File Offset: 0x000CEDA9
		public X509Extensions RequestExtensions
		{
			get
			{
				return X509Extensions.GetInstance(this.req.TbsRequest.RequestExtensions);
			}
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000D0BC0 File Offset: 0x000CEDC0
		protected override X509Extensions GetX509Extensions()
		{
			return this.RequestExtensions;
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x000D0BC8 File Offset: 0x000CEDC8
		public string SignatureAlgOid
		{
			get
			{
				if (!this.IsSigned)
				{
					return null;
				}
				return this.req.OptionalSignature.SignatureAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x000D0BEE File Offset: 0x000CEDEE
		public byte[] GetSignature()
		{
			if (!this.IsSigned)
			{
				return null;
			}
			return this.req.OptionalSignature.GetSignatureOctets();
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000D0C0C File Offset: 0x000CEE0C
		private IList GetCertList()
		{
			IList list = Platform.CreateArrayList();
			Asn1Sequence certs = this.req.OptionalSignature.Certs;
			if (certs != null)
			{
				foreach (object obj in certs)
				{
					Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
					try
					{
						list.Add(new X509CertificateParser().ReadCertificate(asn1Encodable.GetEncoded()));
					}
					catch (Exception e)
					{
						throw new OcspException("can't re-encode certificate!", e);
					}
				}
			}
			return list;
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x000D0CAC File Offset: 0x000CEEAC
		public X509Certificate[] GetCerts()
		{
			if (!this.IsSigned)
			{
				return null;
			}
			IList certList = this.GetCertList();
			X509Certificate[] array = new X509Certificate[certList.Count];
			for (int i = 0; i < certList.Count; i++)
			{
				array[i] = (X509Certificate)certList[i];
			}
			return array;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000D0CF8 File Offset: 0x000CEEF8
		public IX509Store GetCertificates(string type)
		{
			if (!this.IsSigned)
			{
				return null;
			}
			IX509Store result;
			try
			{
				result = X509StoreFactory.Create("Certificate/" + type, new X509CollectionStoreParameters(this.GetCertList()));
			}
			catch (Exception e)
			{
				throw new OcspException("can't setup the CertStore", e);
			}
			return result;
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x000D0D4C File Offset: 0x000CEF4C
		public bool IsSigned
		{
			get
			{
				return this.req.OptionalSignature != null;
			}
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000D0D5C File Offset: 0x000CEF5C
		public bool Verify(AsymmetricKeyParameter publicKey)
		{
			if (!this.IsSigned)
			{
				throw new OcspException("attempt to Verify signature on unsigned object");
			}
			bool result;
			try
			{
				ISigner signer = SignerUtilities.GetSigner(this.SignatureAlgOid);
				signer.Init(false, publicKey);
				byte[] encoded = this.req.TbsRequest.GetEncoded();
				signer.BlockUpdate(encoded, 0, encoded.Length);
				result = signer.VerifySignature(this.GetSignature());
			}
			catch (Exception ex)
			{
				throw new OcspException("exception processing sig: " + ex, ex);
			}
			return result;
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x000D0DE0 File Offset: 0x000CEFE0
		public byte[] GetEncoded()
		{
			return this.req.GetEncoded();
		}

		// Token: 0x040018FD RID: 6397
		private OcspRequest req;
	}
}
