using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200024C RID: 588
	public class X509V1CertificateGenerator
	{
		// Token: 0x0600153B RID: 5435 RVA: 0x000ADCEA File Offset: 0x000ABEEA
		public X509V1CertificateGenerator()
		{
			this.tbsGen = new V1TbsCertificateGenerator();
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x000ADCFD File Offset: 0x000ABEFD
		public void Reset()
		{
			this.tbsGen = new V1TbsCertificateGenerator();
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x000ADD0A File Offset: 0x000ABF0A
		public void SetSerialNumber(BigInteger serialNumber)
		{
			if (serialNumber.SignValue <= 0)
			{
				throw new ArgumentException("serial number must be a positive integer", "serialNumber");
			}
			this.tbsGen.SetSerialNumber(new DerInteger(serialNumber));
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x000ADD36 File Offset: 0x000ABF36
		public void SetIssuerDN(X509Name issuer)
		{
			this.tbsGen.SetIssuer(issuer);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x000ADD44 File Offset: 0x000ABF44
		public void SetNotBefore(DateTime date)
		{
			this.tbsGen.SetStartDate(new Time(date));
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x000ADD57 File Offset: 0x000ABF57
		public void SetNotAfter(DateTime date)
		{
			this.tbsGen.SetEndDate(new Time(date));
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x000ADD6A File Offset: 0x000ABF6A
		public void SetSubjectDN(X509Name subject)
		{
			this.tbsGen.SetSubject(subject);
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x000ADD78 File Offset: 0x000ABF78
		public void SetPublicKey(AsymmetricKeyParameter publicKey)
		{
			try
			{
				this.tbsGen.SetSubjectPublicKeyInfo(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey));
			}
			catch (Exception ex)
			{
				throw new ArgumentException("unable to process key - " + ex.ToString());
			}
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x000ADDC0 File Offset: 0x000ABFC0
		[Obsolete("Not needed if Generate used with an ISignatureFactory")]
		public void SetSignatureAlgorithm(string signatureAlgorithm)
		{
			this.signatureAlgorithm = signatureAlgorithm;
			try
			{
				this.sigOID = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
			}
			catch (Exception)
			{
				throw new ArgumentException("Unknown signature type requested", "signatureAlgorithm");
			}
			this.sigAlgId = X509Utilities.GetSigAlgID(this.sigOID, signatureAlgorithm);
			this.tbsGen.SetSignature(this.sigAlgId);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x000ADE28 File Offset: 0x000AC028
		[Obsolete("Use Generate with an ISignatureFactory")]
		public X509Certificate Generate(AsymmetricKeyParameter privateKey)
		{
			return this.Generate(privateKey, null);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x000ADE32 File Offset: 0x000AC032
		[Obsolete("Use Generate with an ISignatureFactory")]
		public X509Certificate Generate(AsymmetricKeyParameter privateKey, SecureRandom random)
		{
			return this.Generate(new Asn1SignatureFactory(this.signatureAlgorithm, privateKey, random));
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x000ADE48 File Offset: 0x000AC048
		public X509Certificate Generate(ISignatureFactory signatureCalculatorFactory)
		{
			this.tbsGen.SetSignature((AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails);
			TbsCertificateStructure tbsCertificateStructure = this.tbsGen.GenerateTbsCertificate();
			IStreamCalculator streamCalculator = signatureCalculatorFactory.CreateCalculator();
			byte[] derEncoded = tbsCertificateStructure.GetDerEncoded();
			streamCalculator.Stream.Write(derEncoded, 0, derEncoded.Length);
			Platform.Dispose(streamCalculator.Stream);
			return this.GenerateJcaObject(tbsCertificateStructure, (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails, ((IBlockResult)streamCalculator.GetResult()).Collect());
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x000ADEC2 File Offset: 0x000AC0C2
		private X509Certificate GenerateJcaObject(TbsCertificateStructure tbsCert, AlgorithmIdentifier sigAlg, byte[] signature)
		{
			return new X509Certificate(new X509CertificateStructure(tbsCert, sigAlg, new DerBitString(signature)));
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x000ADED6 File Offset: 0x000AC0D6
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x04001647 RID: 5703
		private V1TbsCertificateGenerator tbsGen;

		// Token: 0x04001648 RID: 5704
		private DerObjectIdentifier sigOID;

		// Token: 0x04001649 RID: 5705
		private AlgorithmIdentifier sigAlgId;

		// Token: 0x0400164A RID: 5706
		private string signatureAlgorithm;
	}
}
