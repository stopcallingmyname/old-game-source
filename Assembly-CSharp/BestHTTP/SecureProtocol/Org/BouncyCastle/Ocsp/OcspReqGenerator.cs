using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002F9 RID: 761
	public class OcspReqGenerator
	{
		// Token: 0x06001BBA RID: 7098 RVA: 0x000D0DED File Offset: 0x000CEFED
		public void AddRequest(CertificateID certId)
		{
			this.list.Add(new OcspReqGenerator.RequestObject(certId, null));
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000D0E02 File Offset: 0x000CF002
		public void AddRequest(CertificateID certId, X509Extensions singleRequestExtensions)
		{
			this.list.Add(new OcspReqGenerator.RequestObject(certId, singleRequestExtensions));
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000D0E18 File Offset: 0x000CF018
		public void SetRequestorName(X509Name requestorName)
		{
			try
			{
				this.requestorName = new GeneralName(4, requestorName);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("cannot encode principal", innerException);
			}
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000D0E54 File Offset: 0x000CF054
		public void SetRequestorName(GeneralName requestorName)
		{
			this.requestorName = requestorName;
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000D0E5D File Offset: 0x000CF05D
		public void SetRequestExtensions(X509Extensions requestExtensions)
		{
			this.requestExtensions = requestExtensions;
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000D0E68 File Offset: 0x000CF068
		private OcspReq GenerateRequest(DerObjectIdentifier signingAlgorithm, AsymmetricKeyParameter privateKey, X509Certificate[] chain, SecureRandom random)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.list)
			{
				OcspReqGenerator.RequestObject requestObject = (OcspReqGenerator.RequestObject)obj;
				try
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						requestObject.ToRequest()
					});
				}
				catch (Exception e)
				{
					throw new OcspException("exception creating Request", e);
				}
			}
			TbsRequest tbsRequest = new TbsRequest(this.requestorName, new DerSequence(asn1EncodableVector), this.requestExtensions);
			ISigner signer = null;
			Signature optionalSignature = null;
			if (signingAlgorithm != null)
			{
				if (this.requestorName == null)
				{
					throw new OcspException("requestorName must be specified if request is signed.");
				}
				try
				{
					signer = SignerUtilities.GetSigner(signingAlgorithm.Id);
					if (random != null)
					{
						signer.Init(true, new ParametersWithRandom(privateKey, random));
					}
					else
					{
						signer.Init(true, privateKey);
					}
				}
				catch (Exception ex)
				{
					throw new OcspException("exception creating signature: " + ex, ex);
				}
				DerBitString signatureValue = null;
				try
				{
					byte[] encoded = tbsRequest.GetEncoded();
					signer.BlockUpdate(encoded, 0, encoded.Length);
					signatureValue = new DerBitString(signer.GenerateSignature());
				}
				catch (Exception ex2)
				{
					throw new OcspException("exception processing TBSRequest: " + ex2, ex2);
				}
				AlgorithmIdentifier signatureAlgorithm = new AlgorithmIdentifier(signingAlgorithm, DerNull.Instance);
				if (chain != null && chain.Length != 0)
				{
					Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					try
					{
						for (int num = 0; num != chain.Length; num++)
						{
							asn1EncodableVector2.Add(new Asn1Encodable[]
							{
								X509CertificateStructure.GetInstance(Asn1Object.FromByteArray(chain[num].GetEncoded()))
							});
						}
					}
					catch (IOException e2)
					{
						throw new OcspException("error processing certs", e2);
					}
					catch (CertificateEncodingException e3)
					{
						throw new OcspException("error encoding certs", e3);
					}
					optionalSignature = new Signature(signatureAlgorithm, signatureValue, new DerSequence(asn1EncodableVector2));
				}
				else
				{
					optionalSignature = new Signature(signatureAlgorithm, signatureValue);
				}
			}
			return new OcspReq(new OcspRequest(tbsRequest, optionalSignature));
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000D1084 File Offset: 0x000CF284
		public OcspReq Generate()
		{
			return this.GenerateRequest(null, null, null, null);
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000D1090 File Offset: 0x000CF290
		public OcspReq Generate(string signingAlgorithm, AsymmetricKeyParameter privateKey, X509Certificate[] chain)
		{
			return this.Generate(signingAlgorithm, privateKey, chain, null);
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000D109C File Offset: 0x000CF29C
		public OcspReq Generate(string signingAlgorithm, AsymmetricKeyParameter privateKey, X509Certificate[] chain, SecureRandom random)
		{
			if (signingAlgorithm == null)
			{
				throw new ArgumentException("no signing algorithm specified");
			}
			OcspReq result;
			try
			{
				DerObjectIdentifier algorithmOid = OcspUtilities.GetAlgorithmOid(signingAlgorithm);
				result = this.GenerateRequest(algorithmOid, privateKey, chain, random);
			}
			catch (ArgumentException)
			{
				throw new ArgumentException("unknown signing algorithm specified: " + signingAlgorithm);
			}
			return result;
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x000D0861 File Offset: 0x000CEA61
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return OcspUtilities.AlgNames;
			}
		}

		// Token: 0x040018FE RID: 6398
		private IList list = Platform.CreateArrayList();

		// Token: 0x040018FF RID: 6399
		private GeneralName requestorName;

		// Token: 0x04001900 RID: 6400
		private X509Extensions requestExtensions;

		// Token: 0x0200090A RID: 2314
		private class RequestObject
		{
			// Token: 0x06004E3A RID: 20026 RVA: 0x001B1078 File Offset: 0x001AF278
			public RequestObject(CertificateID certId, X509Extensions extensions)
			{
				this.certId = certId;
				this.extensions = extensions;
			}

			// Token: 0x06004E3B RID: 20027 RVA: 0x001B108E File Offset: 0x001AF28E
			public Request ToRequest()
			{
				return new Request(this.certId.ToAsn1Object(), this.extensions);
			}

			// Token: 0x04003555 RID: 13653
			internal CertificateID certId;

			// Token: 0x04003556 RID: 13654
			internal X509Extensions extensions;
		}
	}
}
