using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005FD RID: 1533
	public class CmsSignedDataGenerator : CmsSignedGenerator
	{
		// Token: 0x06003A23 RID: 14883 RVA: 0x00169178 File Offset: 0x00167378
		public CmsSignedDataGenerator()
		{
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x0016918B File Offset: 0x0016738B
		public CmsSignedDataGenerator(SecureRandom rand) : base(rand)
		{
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x0016919F File Offset: 0x0016739F
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string digestOID)
		{
			this.AddSigner(privateKey, cert, CmsSignedDataGenerator.Helper.GetEncOid(privateKey, digestOID), digestOID);
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x001691B6 File Offset: 0x001673B6
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string encryptionOID, string digestOID)
		{
			this.doAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(cert), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator(), null, null);
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x001691CF File Offset: 0x001673CF
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOID)
		{
			this.AddSigner(privateKey, subjectKeyID, CmsSignedDataGenerator.Helper.GetEncOid(privateKey, digestOID), digestOID);
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x001691E6 File Offset: 0x001673E6
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string encryptionOID, string digestOID)
		{
			this.doAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(subjectKeyID), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator(), null, null);
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x001691FF File Offset: 0x001673FF
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string digestOID, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.AddSigner(privateKey, cert, CmsSignedDataGenerator.Helper.GetEncOid(privateKey, digestOID), digestOID, signedAttr, unsignedAttr);
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x0016921A File Offset: 0x0016741A
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string encryptionOID, string digestOID, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.doAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(cert), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator(signedAttr), new SimpleAttributeTableGenerator(unsignedAttr), signedAttr);
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x0016923C File Offset: 0x0016743C
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOID, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.AddSigner(privateKey, subjectKeyID, CmsSignedDataGenerator.Helper.GetEncOid(privateKey, digestOID), digestOID, signedAttr, unsignedAttr);
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x00169257 File Offset: 0x00167457
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string encryptionOID, string digestOID, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.doAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(subjectKeyID), encryptionOID, digestOID, new DefaultSignedAttributeTableGenerator(signedAttr), new SimpleAttributeTableGenerator(unsignedAttr), signedAttr);
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x00169279 File Offset: 0x00167479
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string digestOID, CmsAttributeTableGenerator signedAttrGen, CmsAttributeTableGenerator unsignedAttrGen)
		{
			this.AddSigner(privateKey, cert, CmsSignedDataGenerator.Helper.GetEncOid(privateKey, digestOID), digestOID, signedAttrGen, unsignedAttrGen);
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x00169294 File Offset: 0x00167494
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string encryptionOID, string digestOID, CmsAttributeTableGenerator signedAttrGen, CmsAttributeTableGenerator unsignedAttrGen)
		{
			this.doAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(cert), encryptionOID, digestOID, signedAttrGen, unsignedAttrGen, null);
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x001692AB File Offset: 0x001674AB
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOID, CmsAttributeTableGenerator signedAttrGen, CmsAttributeTableGenerator unsignedAttrGen)
		{
			this.AddSigner(privateKey, subjectKeyID, CmsSignedDataGenerator.Helper.GetEncOid(privateKey, digestOID), digestOID, signedAttrGen, unsignedAttrGen);
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x001692C6 File Offset: 0x001674C6
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string encryptionOID, string digestOID, CmsAttributeTableGenerator signedAttrGen, CmsAttributeTableGenerator unsignedAttrGen)
		{
			this.doAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(subjectKeyID), encryptionOID, digestOID, signedAttrGen, unsignedAttrGen, null);
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x001692DD File Offset: 0x001674DD
		public void AddSignerInfoGenerator(SignerInfoGenerator signerInfoGenerator)
		{
			this.signerInfs.Add(new CmsSignedDataGenerator.SignerInf(this, signerInfoGenerator.contentSigner, signerInfoGenerator.sigId, signerInfoGenerator.signedGen, signerInfoGenerator.unsignedGen, null));
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x0016930C File Offset: 0x0016750C
		private void doAddSigner(AsymmetricKeyParameter privateKey, SignerIdentifier signerIdentifier, string encryptionOID, string digestOID, CmsAttributeTableGenerator signedAttrGen, CmsAttributeTableGenerator unsignedAttrGen, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable)
		{
			this.signerInfs.Add(new CmsSignedDataGenerator.SignerInf(this, privateKey, signerIdentifier, digestOID, encryptionOID, signedAttrGen, unsignedAttrGen, baseSignedTable));
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x00169336 File Offset: 0x00167536
		public CmsSignedData Generate(CmsProcessable content)
		{
			return this.Generate(content, false);
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x00169340 File Offset: 0x00167540
		public CmsSignedData Generate(string signedContentType, CmsProcessable content, bool encapsulate)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			this._digests.Clear();
			foreach (object obj in this._signers)
			{
				SignerInformation signerInformation = (SignerInformation)obj;
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					CmsSignedDataGenerator.Helper.FixAlgID(signerInformation.DigestAlgorithmID)
				});
				asn1EncodableVector2.Add(new Asn1Encodable[]
				{
					signerInformation.ToSignerInfo()
				});
			}
			DerObjectIdentifier contentType = (signedContentType == null) ? null : new DerObjectIdentifier(signedContentType);
			foreach (object obj2 in this.signerInfs)
			{
				CmsSignedDataGenerator.SignerInf signerInf = (CmsSignedDataGenerator.SignerInf)obj2;
				try
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						signerInf.DigestAlgorithmID
					});
					asn1EncodableVector2.Add(new Asn1Encodable[]
					{
						signerInf.ToSignerInfo(contentType, content, this.rand)
					});
				}
				catch (IOException e)
				{
					throw new CmsException("encoding error.", e);
				}
				catch (InvalidKeyException e2)
				{
					throw new CmsException("key inappropriate for signature.", e2);
				}
				catch (SignatureException e3)
				{
					throw new CmsException("error creating signature.", e3);
				}
				catch (CertificateEncodingException e4)
				{
					throw new CmsException("error creating sid.", e4);
				}
			}
			Asn1Set certificates = null;
			if (this._certs.Count != 0)
			{
				certificates = (base.UseDerForCerts ? CmsUtilities.CreateDerSetFromList(this._certs) : CmsUtilities.CreateBerSetFromList(this._certs));
			}
			Asn1Set crls = null;
			if (this._crls.Count != 0)
			{
				crls = (base.UseDerForCrls ? CmsUtilities.CreateDerSetFromList(this._crls) : CmsUtilities.CreateBerSetFromList(this._crls));
			}
			Asn1OctetString content2 = null;
			if (encapsulate)
			{
				MemoryStream memoryStream = new MemoryStream();
				if (content != null)
				{
					try
					{
						content.Write(memoryStream);
					}
					catch (IOException e5)
					{
						throw new CmsException("encapsulation error.", e5);
					}
				}
				content2 = new BerOctetString(memoryStream.ToArray());
			}
			ContentInfo contentInfo = new ContentInfo(contentType, content2);
			SignedData content3 = new SignedData(new DerSet(asn1EncodableVector), contentInfo, certificates, crls, new DerSet(asn1EncodableVector2));
			ContentInfo sigData = new ContentInfo(CmsObjectIdentifiers.SignedData, content3);
			return new CmsSignedData(content, sigData);
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x001695D0 File Offset: 0x001677D0
		public CmsSignedData Generate(CmsProcessable content, bool encapsulate)
		{
			return this.Generate(CmsSignedGenerator.Data, content, encapsulate);
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x001695DF File Offset: 0x001677DF
		public SignerInformationStore GenerateCounterSigners(SignerInformation signer)
		{
			return this.Generate(null, new CmsProcessableByteArray(signer.GetSignature()), false).GetSignerInfos();
		}

		// Token: 0x04002616 RID: 9750
		private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;

		// Token: 0x04002617 RID: 9751
		private readonly IList signerInfs = Platform.CreateArrayList();

		// Token: 0x02000984 RID: 2436
		private class SignerInf
		{
			// Token: 0x06004FB7 RID: 20407 RVA: 0x001B71C8 File Offset: 0x001B53C8
			internal SignerInf(CmsSignedGenerator outer, AsymmetricKeyParameter key, SignerIdentifier signerIdentifier, string digestOID, string encOID, CmsAttributeTableGenerator sAttr, CmsAttributeTableGenerator unsAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable)
			{
				string algorithm = CmsSignedDataGenerator.Helper.GetDigestAlgName(digestOID) + "with" + CmsSignedDataGenerator.Helper.GetEncryptionAlgName(encOID);
				this.outer = outer;
				this.sigCalc = new Asn1SignatureFactory(algorithm, key);
				this.signerIdentifier = signerIdentifier;
				this.digestOID = digestOID;
				this.encOID = encOID;
				this.sAttr = sAttr;
				this.unsAttr = unsAttr;
				this.baseSignedTable = baseSignedTable;
			}

			// Token: 0x06004FB8 RID: 20408 RVA: 0x001B7244 File Offset: 0x001B5444
			internal SignerInf(CmsSignedGenerator outer, ISignatureFactory sigCalc, SignerIdentifier signerIdentifier, CmsAttributeTableGenerator sAttr, CmsAttributeTableGenerator unsAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable)
			{
				this.outer = outer;
				this.sigCalc = sigCalc;
				this.signerIdentifier = signerIdentifier;
				this.digestOID = new DefaultDigestAlgorithmIdentifierFinder().find((AlgorithmIdentifier)sigCalc.AlgorithmDetails).Algorithm.Id;
				this.encOID = ((AlgorithmIdentifier)sigCalc.AlgorithmDetails).Algorithm.Id;
				this.sAttr = sAttr;
				this.unsAttr = unsAttr;
				this.baseSignedTable = baseSignedTable;
			}

			// Token: 0x17000C68 RID: 3176
			// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x001B72C4 File Offset: 0x001B54C4
			internal AlgorithmIdentifier DigestAlgorithmID
			{
				get
				{
					return new AlgorithmIdentifier(new DerObjectIdentifier(this.digestOID), DerNull.Instance);
				}
			}

			// Token: 0x17000C69 RID: 3177
			// (get) Token: 0x06004FBA RID: 20410 RVA: 0x001B72DB File Offset: 0x001B54DB
			internal CmsAttributeTableGenerator SignedAttributes
			{
				get
				{
					return this.sAttr;
				}
			}

			// Token: 0x17000C6A RID: 3178
			// (get) Token: 0x06004FBB RID: 20411 RVA: 0x001B72E3 File Offset: 0x001B54E3
			internal CmsAttributeTableGenerator UnsignedAttributes
			{
				get
				{
					return this.unsAttr;
				}
			}

			// Token: 0x06004FBC RID: 20412 RVA: 0x001B72EC File Offset: 0x001B54EC
			internal SignerInfo ToSignerInfo(DerObjectIdentifier contentType, CmsProcessable content, SecureRandom random)
			{
				AlgorithmIdentifier digestAlgorithmID = this.DigestAlgorithmID;
				string digestAlgName = CmsSignedDataGenerator.Helper.GetDigestAlgName(this.digestOID);
				string algorithm = digestAlgName + "with" + CmsSignedDataGenerator.Helper.GetEncryptionAlgName(this.encOID);
				byte[] array;
				if (this.outer._digests.Contains(this.digestOID))
				{
					array = (byte[])this.outer._digests[this.digestOID];
				}
				else
				{
					IDigest digestInstance = CmsSignedDataGenerator.Helper.GetDigestInstance(digestAlgName);
					if (content != null)
					{
						content.Write(new DigestSink(digestInstance));
					}
					array = DigestUtilities.DoFinal(digestInstance);
					this.outer._digests.Add(this.digestOID, array.Clone());
				}
				IStreamCalculator streamCalculator = this.sigCalc.CreateCalculator();
				Stream stream = new BufferedStream(streamCalculator.Stream);
				Asn1Set asn1Set = null;
				if (this.sAttr != null)
				{
					IDictionary baseParameters = this.outer.GetBaseParameters(contentType, digestAlgorithmID, array);
					BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable attributeTable = this.sAttr.GetAttributes(baseParameters);
					if (contentType == null && attributeTable != null && attributeTable[CmsAttributes.ContentType] != null)
					{
						IDictionary dictionary = attributeTable.ToDictionary();
						dictionary.Remove(CmsAttributes.ContentType);
						attributeTable = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(dictionary);
					}
					asn1Set = this.outer.GetAttributeSet(attributeTable);
					new DerOutputStream(stream).WriteObject(asn1Set);
				}
				else if (content != null)
				{
					content.Write(stream);
				}
				Platform.Dispose(stream);
				byte[] array2 = ((IBlockResult)streamCalculator.GetResult()).Collect();
				Asn1Set unauthenticatedAttributes = null;
				if (this.unsAttr != null)
				{
					IDictionary baseParameters2 = this.outer.GetBaseParameters(contentType, digestAlgorithmID, array);
					baseParameters2[CmsAttributeTableParameter.Signature] = array2.Clone();
					BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable attributes = this.unsAttr.GetAttributes(baseParameters2);
					unauthenticatedAttributes = this.outer.GetAttributeSet(attributes);
				}
				Asn1Encodable defaultX509Parameters = SignerUtilities.GetDefaultX509Parameters(algorithm);
				AlgorithmIdentifier encAlgorithmIdentifier = CmsSignedDataGenerator.Helper.GetEncAlgorithmIdentifier(new DerObjectIdentifier(this.encOID), defaultX509Parameters);
				return new SignerInfo(this.signerIdentifier, digestAlgorithmID, asn1Set, encAlgorithmIdentifier, new DerOctetString(array2), unauthenticatedAttributes);
			}

			// Token: 0x040036EF RID: 14063
			private readonly CmsSignedGenerator outer;

			// Token: 0x040036F0 RID: 14064
			private readonly ISignatureFactory sigCalc;

			// Token: 0x040036F1 RID: 14065
			private readonly SignerIdentifier signerIdentifier;

			// Token: 0x040036F2 RID: 14066
			private readonly string digestOID;

			// Token: 0x040036F3 RID: 14067
			private readonly string encOID;

			// Token: 0x040036F4 RID: 14068
			private readonly CmsAttributeTableGenerator sAttr;

			// Token: 0x040036F5 RID: 14069
			private readonly CmsAttributeTableGenerator unsAttr;

			// Token: 0x040036F6 RID: 14070
			private readonly BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable baseSignedTable;
		}
	}
}
