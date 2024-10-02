using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000250 RID: 592
	public class X509V3CertificateGenerator
	{
		// Token: 0x06001587 RID: 5511 RVA: 0x000AE8BD File Offset: 0x000ACABD
		public X509V3CertificateGenerator()
		{
			this.tbsGen = new V3TbsCertificateGenerator();
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x000AE8DB File Offset: 0x000ACADB
		public void Reset()
		{
			this.tbsGen = new V3TbsCertificateGenerator();
			this.extGenerator.Reset();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x000AE8F3 File Offset: 0x000ACAF3
		public void SetSerialNumber(BigInteger serialNumber)
		{
			if (serialNumber.SignValue <= 0)
			{
				throw new ArgumentException("serial number must be a positive integer", "serialNumber");
			}
			this.tbsGen.SetSerialNumber(new DerInteger(serialNumber));
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x000AE91F File Offset: 0x000ACB1F
		public void SetIssuerDN(X509Name issuer)
		{
			this.tbsGen.SetIssuer(issuer);
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x000AE92D File Offset: 0x000ACB2D
		public void SetNotBefore(DateTime date)
		{
			this.tbsGen.SetStartDate(new Time(date));
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x000AE940 File Offset: 0x000ACB40
		public void SetNotAfter(DateTime date)
		{
			this.tbsGen.SetEndDate(new Time(date));
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x000AE953 File Offset: 0x000ACB53
		public void SetSubjectDN(X509Name subject)
		{
			this.tbsGen.SetSubject(subject);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x000AE961 File Offset: 0x000ACB61
		public void SetPublicKey(AsymmetricKeyParameter publicKey)
		{
			this.tbsGen.SetSubjectPublicKeyInfo(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey));
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x000AE974 File Offset: 0x000ACB74
		[Obsolete("Not needed if Generate used with an ISignatureFactory")]
		public void SetSignatureAlgorithm(string signatureAlgorithm)
		{
			this.signatureAlgorithm = signatureAlgorithm;
			try
			{
				this.sigOid = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
			}
			catch (Exception)
			{
				throw new ArgumentException("Unknown signature type requested: " + signatureAlgorithm);
			}
			this.sigAlgId = X509Utilities.GetSigAlgID(this.sigOid, signatureAlgorithm);
			this.tbsGen.SetSignature(this.sigAlgId);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x000AE9DC File Offset: 0x000ACBDC
		public void SetSubjectUniqueID(bool[] uniqueID)
		{
			this.tbsGen.SetSubjectUniqueID(this.booleanToBitString(uniqueID));
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000AE9F0 File Offset: 0x000ACBF0
		public void SetIssuerUniqueID(bool[] uniqueID)
		{
			this.tbsGen.SetIssuerUniqueID(this.booleanToBitString(uniqueID));
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x000AEA04 File Offset: 0x000ACC04
		private DerBitString booleanToBitString(bool[] id)
		{
			byte[] array = new byte[(id.Length + 7) / 8];
			for (int num = 0; num != id.Length; num++)
			{
				if (id[num])
				{
					byte[] array2 = array;
					int num2 = num / 8;
					array2[num2] |= (byte)(1 << 7 - num % 8);
				}
			}
			int num3 = id.Length % 8;
			if (num3 == 0)
			{
				return new DerBitString(array);
			}
			return new DerBitString(array, 8 - num3);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x000AEA63 File Offset: 0x000ACC63
		public void AddExtension(string oid, bool critical, Asn1Encodable extensionValue)
		{
			this.extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000AEA78 File Offset: 0x000ACC78
		public void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue)
		{
			this.extGenerator.AddExtension(oid, critical, extensionValue);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x000AEA88 File Offset: 0x000ACC88
		public void AddExtension(string oid, bool critical, byte[] extensionValue)
		{
			this.extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, new DerOctetString(extensionValue));
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x000AEAA2 File Offset: 0x000ACCA2
		public void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extensionValue)
		{
			this.extGenerator.AddExtension(oid, critical, new DerOctetString(extensionValue));
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x000AEAB7 File Offset: 0x000ACCB7
		public void CopyAndAddExtension(string oid, bool critical, X509Certificate cert)
		{
			this.CopyAndAddExtension(new DerObjectIdentifier(oid), critical, cert);
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x000AEAC8 File Offset: 0x000ACCC8
		public void CopyAndAddExtension(DerObjectIdentifier oid, bool critical, X509Certificate cert)
		{
			Asn1OctetString extensionValue = cert.GetExtensionValue(oid);
			if (extensionValue == null)
			{
				throw new CertificateParsingException("extension " + oid + " not present");
			}
			try
			{
				Asn1Encodable extensionValue2 = X509ExtensionUtilities.FromExtensionValue(extensionValue);
				this.AddExtension(oid, critical, extensionValue2);
			}
			catch (Exception ex)
			{
				throw new CertificateParsingException(ex.Message, ex);
			}
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x000AEB28 File Offset: 0x000ACD28
		[Obsolete("Use Generate with an ISignatureFactory")]
		public X509Certificate Generate(AsymmetricKeyParameter privateKey)
		{
			return this.Generate(privateKey, null);
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000AEB32 File Offset: 0x000ACD32
		[Obsolete("Use Generate with an ISignatureFactory")]
		public X509Certificate Generate(AsymmetricKeyParameter privateKey, SecureRandom random)
		{
			return this.Generate(new Asn1SignatureFactory(this.signatureAlgorithm, privateKey, random));
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x000AEB48 File Offset: 0x000ACD48
		public X509Certificate Generate(ISignatureFactory signatureCalculatorFactory)
		{
			this.tbsGen.SetSignature((AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails);
			if (!this.extGenerator.IsEmpty)
			{
				this.tbsGen.SetExtensions(this.extGenerator.Generate());
			}
			TbsCertificateStructure tbsCertificateStructure = this.tbsGen.GenerateTbsCertificate();
			IStreamCalculator streamCalculator = signatureCalculatorFactory.CreateCalculator();
			byte[] derEncoded = tbsCertificateStructure.GetDerEncoded();
			streamCalculator.Stream.Write(derEncoded, 0, derEncoded.Length);
			Platform.Dispose(streamCalculator.Stream);
			return this.GenerateJcaObject(tbsCertificateStructure, (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails, ((IBlockResult)streamCalculator.GetResult()).Collect());
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x000ADEC2 File Offset: 0x000AC0C2
		private X509Certificate GenerateJcaObject(TbsCertificateStructure tbsCert, AlgorithmIdentifier sigAlg, byte[] signature)
		{
			return new X509Certificate(new X509CertificateStructure(tbsCert, sigAlg, new DerBitString(signature)));
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600159D RID: 5533 RVA: 0x000ADED6 File Offset: 0x000AC0D6
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x04001658 RID: 5720
		private readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();

		// Token: 0x04001659 RID: 5721
		private V3TbsCertificateGenerator tbsGen;

		// Token: 0x0400165A RID: 5722
		private DerObjectIdentifier sigOid;

		// Token: 0x0400165B RID: 5723
		private AlgorithmIdentifier sigAlgId;

		// Token: 0x0400165C RID: 5724
		private string signatureAlgorithm;
	}
}
