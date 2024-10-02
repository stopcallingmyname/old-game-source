using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002F3 RID: 755
	public class BasicOcspResp : X509ExtensionBase
	{
		// Token: 0x06001B78 RID: 7032 RVA: 0x000D024E File Offset: 0x000CE44E
		public BasicOcspResp(BasicOcspResponse resp)
		{
			this.resp = resp;
			this.data = resp.TbsResponseData;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000D026C File Offset: 0x000CE46C
		public byte[] GetTbsResponseData()
		{
			byte[] derEncoded;
			try
			{
				derEncoded = this.data.GetDerEncoded();
			}
			catch (IOException e)
			{
				throw new OcspException("problem encoding tbsResponseData", e);
			}
			return derEncoded;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001B7A RID: 7034 RVA: 0x000D02A8 File Offset: 0x000CE4A8
		public int Version
		{
			get
			{
				return this.data.Version.Value.IntValue + 1;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001B7B RID: 7035 RVA: 0x000D02C1 File Offset: 0x000CE4C1
		public RespID ResponderId
		{
			get
			{
				return new RespID(this.data.ResponderID);
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001B7C RID: 7036 RVA: 0x000D02D3 File Offset: 0x000CE4D3
		public DateTime ProducedAt
		{
			get
			{
				return this.data.ProducedAt.ToDateTime();
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001B7D RID: 7037 RVA: 0x000D02E8 File Offset: 0x000CE4E8
		public SingleResp[] Responses
		{
			get
			{
				Asn1Sequence responses = this.data.Responses;
				SingleResp[] array = new SingleResp[responses.Count];
				for (int num = 0; num != array.Length; num++)
				{
					array[num] = new SingleResp(SingleResponse.GetInstance(responses[num]));
				}
				return array;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x000D0330 File Offset: 0x000CE530
		public X509Extensions ResponseExtensions
		{
			get
			{
				return this.data.ResponseExtensions;
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000D033D File Offset: 0x000CE53D
		protected override X509Extensions GetX509Extensions()
		{
			return this.ResponseExtensions;
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x000D0345 File Offset: 0x000CE545
		public string SignatureAlgName
		{
			get
			{
				return OcspUtilities.GetAlgorithmName(this.resp.SignatureAlgorithm.Algorithm);
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001B81 RID: 7041 RVA: 0x000D035C File Offset: 0x000CE55C
		public string SignatureAlgOid
		{
			get
			{
				return this.resp.SignatureAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000D0373 File Offset: 0x000CE573
		[Obsolete("RespData class is no longer required as all functionality is available on this class")]
		public RespData GetResponseData()
		{
			return new RespData(this.data);
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000D0380 File Offset: 0x000CE580
		public byte[] GetSignature()
		{
			return this.resp.GetSignatureOctets();
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000D0390 File Offset: 0x000CE590
		private IList GetCertList()
		{
			IList list = Platform.CreateArrayList();
			Asn1Sequence certs = this.resp.Certs;
			if (certs != null)
			{
				foreach (object obj in certs)
				{
					Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
					try
					{
						list.Add(new X509CertificateParser().ReadCertificate(asn1Encodable.GetEncoded()));
					}
					catch (IOException e)
					{
						throw new OcspException("can't re-encode certificate!", e);
					}
					catch (CertificateException e2)
					{
						throw new OcspException("can't re-encode certificate!", e2);
					}
				}
			}
			return list;
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000D0448 File Offset: 0x000CE648
		public X509Certificate[] GetCerts()
		{
			IList certList = this.GetCertList();
			X509Certificate[] array = new X509Certificate[certList.Count];
			for (int i = 0; i < certList.Count; i++)
			{
				array[i] = (X509Certificate)certList[i];
			}
			return array;
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000D048C File Offset: 0x000CE68C
		public IX509Store GetCertificates(string type)
		{
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

		// Token: 0x06001B87 RID: 7047 RVA: 0x000D04D8 File Offset: 0x000CE6D8
		public bool Verify(AsymmetricKeyParameter publicKey)
		{
			bool result;
			try
			{
				ISigner signer = SignerUtilities.GetSigner(this.SignatureAlgName);
				signer.Init(false, publicKey);
				byte[] derEncoded = this.data.GetDerEncoded();
				signer.BlockUpdate(derEncoded, 0, derEncoded.Length);
				result = signer.VerifySignature(this.GetSignature());
			}
			catch (Exception ex)
			{
				throw new OcspException("exception processing sig: " + ex, ex);
			}
			return result;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000D0544 File Offset: 0x000CE744
		public byte[] GetEncoded()
		{
			return this.resp.GetEncoded();
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000D0554 File Offset: 0x000CE754
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			BasicOcspResp basicOcspResp = obj as BasicOcspResp;
			return basicOcspResp != null && this.resp.Equals(basicOcspResp.resp);
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000D0584 File Offset: 0x000CE784
		public override int GetHashCode()
		{
			return this.resp.GetHashCode();
		}

		// Token: 0x040018F5 RID: 6389
		private readonly BasicOcspResponse resp;

		// Token: 0x040018F6 RID: 6390
		private readonly ResponseData data;
	}
}
