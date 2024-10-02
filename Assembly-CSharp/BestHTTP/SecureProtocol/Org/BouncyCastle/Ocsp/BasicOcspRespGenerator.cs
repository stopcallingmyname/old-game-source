using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002F4 RID: 756
	public class BasicOcspRespGenerator
	{
		// Token: 0x06001B8B RID: 7051 RVA: 0x000D0591 File Offset: 0x000CE791
		public BasicOcspRespGenerator(RespID responderID)
		{
			this.responderID = responderID;
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000D05AB File Offset: 0x000CE7AB
		public BasicOcspRespGenerator(AsymmetricKeyParameter publicKey)
		{
			this.responderID = new RespID(publicKey);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000D05CA File Offset: 0x000CE7CA
		public void AddResponse(CertificateID certID, CertificateStatus certStatus)
		{
			this.list.Add(new BasicOcspRespGenerator.ResponseObject(certID, certStatus, DateTime.UtcNow, null));
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000D05E5 File Offset: 0x000CE7E5
		public void AddResponse(CertificateID certID, CertificateStatus certStatus, X509Extensions singleExtensions)
		{
			this.list.Add(new BasicOcspRespGenerator.ResponseObject(certID, certStatus, DateTime.UtcNow, singleExtensions));
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000D0600 File Offset: 0x000CE800
		public void AddResponse(CertificateID certID, CertificateStatus certStatus, DateTime nextUpdate, X509Extensions singleExtensions)
		{
			this.list.Add(new BasicOcspRespGenerator.ResponseObject(certID, certStatus, DateTime.UtcNow, nextUpdate, singleExtensions));
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000D061D File Offset: 0x000CE81D
		public void AddResponse(CertificateID certID, CertificateStatus certStatus, DateTime thisUpdate, DateTime nextUpdate, X509Extensions singleExtensions)
		{
			this.list.Add(new BasicOcspRespGenerator.ResponseObject(certID, certStatus, thisUpdate, nextUpdate, singleExtensions));
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000D0637 File Offset: 0x000CE837
		public void SetResponseExtensions(X509Extensions responseExtensions)
		{
			this.responseExtensions = responseExtensions;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000D0640 File Offset: 0x000CE840
		private BasicOcspResp GenerateResponse(ISignatureFactory signatureCalculator, X509Certificate[] chain, DateTime producedAt)
		{
			DerObjectIdentifier algorithm = ((AlgorithmIdentifier)signatureCalculator.AlgorithmDetails).Algorithm;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.list)
			{
				BasicOcspRespGenerator.ResponseObject responseObject = (BasicOcspRespGenerator.ResponseObject)obj;
				try
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						responseObject.ToResponse()
					});
				}
				catch (Exception e)
				{
					throw new OcspException("exception creating Request", e);
				}
			}
			ResponseData responseData = new ResponseData(this.responderID.ToAsn1Object(), new DerGeneralizedTime(producedAt), new DerSequence(asn1EncodableVector), this.responseExtensions);
			DerBitString signature = null;
			try
			{
				IStreamCalculator streamCalculator = signatureCalculator.CreateCalculator();
				byte[] derEncoded = responseData.GetDerEncoded();
				streamCalculator.Stream.Write(derEncoded, 0, derEncoded.Length);
				Platform.Dispose(streamCalculator.Stream);
				signature = new DerBitString(((IBlockResult)streamCalculator.GetResult()).Collect());
			}
			catch (Exception ex)
			{
				throw new OcspException("exception processing TBSRequest: " + ex, ex);
			}
			AlgorithmIdentifier sigAlgID = OcspUtilities.GetSigAlgID(algorithm);
			DerSequence certs = null;
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
				certs = new DerSequence(asn1EncodableVector2);
			}
			return new BasicOcspResp(new BasicOcspResponse(responseData, sigAlgID, signature, certs));
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000D0818 File Offset: 0x000CEA18
		public BasicOcspResp Generate(string signingAlgorithm, AsymmetricKeyParameter privateKey, X509Certificate[] chain, DateTime thisUpdate)
		{
			return this.Generate(signingAlgorithm, privateKey, chain, thisUpdate, null);
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000D0826 File Offset: 0x000CEA26
		public BasicOcspResp Generate(string signingAlgorithm, AsymmetricKeyParameter privateKey, X509Certificate[] chain, DateTime producedAt, SecureRandom random)
		{
			if (signingAlgorithm == null)
			{
				throw new ArgumentException("no signing algorithm specified");
			}
			return this.GenerateResponse(new Asn1SignatureFactory(signingAlgorithm, privateKey, random), chain, producedAt);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000D0848 File Offset: 0x000CEA48
		public BasicOcspResp Generate(ISignatureFactory signatureCalculatorFactory, X509Certificate[] chain, DateTime producedAt)
		{
			if (signatureCalculatorFactory == null)
			{
				throw new ArgumentException("no signature calculator specified");
			}
			return this.GenerateResponse(signatureCalculatorFactory, chain, producedAt);
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x000D0861 File Offset: 0x000CEA61
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return OcspUtilities.AlgNames;
			}
		}

		// Token: 0x040018F7 RID: 6391
		private readonly IList list = Platform.CreateArrayList();

		// Token: 0x040018F8 RID: 6392
		private X509Extensions responseExtensions;

		// Token: 0x040018F9 RID: 6393
		private RespID responderID;

		// Token: 0x02000909 RID: 2313
		private class ResponseObject
		{
			// Token: 0x06004E36 RID: 20022 RVA: 0x001B0F8A File Offset: 0x001AF18A
			public ResponseObject(CertificateID certId, CertificateStatus certStatus, DateTime thisUpdate, X509Extensions extensions) : this(certId, certStatus, new DerGeneralizedTime(thisUpdate), null, extensions)
			{
			}

			// Token: 0x06004E37 RID: 20023 RVA: 0x001B0F9D File Offset: 0x001AF19D
			public ResponseObject(CertificateID certId, CertificateStatus certStatus, DateTime thisUpdate, DateTime nextUpdate, X509Extensions extensions) : this(certId, certStatus, new DerGeneralizedTime(thisUpdate), new DerGeneralizedTime(nextUpdate), extensions)
			{
			}

			// Token: 0x06004E38 RID: 20024 RVA: 0x001B0FB8 File Offset: 0x001AF1B8
			private ResponseObject(CertificateID certId, CertificateStatus certStatus, DerGeneralizedTime thisUpdate, DerGeneralizedTime nextUpdate, X509Extensions extensions)
			{
				this.certId = certId;
				if (certStatus == null)
				{
					this.certStatus = new CertStatus();
				}
				else if (certStatus is UnknownStatus)
				{
					this.certStatus = new CertStatus(2, DerNull.Instance);
				}
				else
				{
					RevokedStatus revokedStatus = (RevokedStatus)certStatus;
					CrlReason revocationReason = revokedStatus.HasRevocationReason ? new CrlReason(revokedStatus.RevocationReason) : null;
					this.certStatus = new CertStatus(new RevokedInfo(new DerGeneralizedTime(revokedStatus.RevocationTime), revocationReason));
				}
				this.thisUpdate = thisUpdate;
				this.nextUpdate = nextUpdate;
				this.extensions = extensions;
			}

			// Token: 0x06004E39 RID: 20025 RVA: 0x001B104E File Offset: 0x001AF24E
			public SingleResponse ToResponse()
			{
				return new SingleResponse(this.certId.ToAsn1Object(), this.certStatus, this.thisUpdate, this.nextUpdate, this.extensions);
			}

			// Token: 0x04003550 RID: 13648
			internal CertificateID certId;

			// Token: 0x04003551 RID: 13649
			internal CertStatus certStatus;

			// Token: 0x04003552 RID: 13650
			internal DerGeneralizedTime thisUpdate;

			// Token: 0x04003553 RID: 13651
			internal DerGeneralizedTime nextUpdate;

			// Token: 0x04003554 RID: 13652
			internal X509Extensions extensions;
		}
	}
}
