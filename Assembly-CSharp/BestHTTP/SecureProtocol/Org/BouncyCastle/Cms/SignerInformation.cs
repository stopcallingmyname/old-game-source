using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200061F RID: 1567
	public class SignerInformation
	{
		// Token: 0x06003B18 RID: 15128 RVA: 0x0016D38C File Offset: 0x0016B58C
		internal SignerInformation(BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo info, DerObjectIdentifier contentType, CmsProcessable content, IDigestCalculator digestCalculator)
		{
			this.info = info;
			this.sid = new SignerID();
			this.contentType = contentType;
			this.isCounterSignature = (contentType == null);
			try
			{
				SignerIdentifier signerID = info.SignerID;
				if (signerID.IsTagged)
				{
					Asn1OctetString instance = Asn1OctetString.GetInstance(signerID.ID);
					this.sid.SubjectKeyIdentifier = instance.GetEncoded();
				}
				else
				{
					BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.IssuerAndSerialNumber instance2 = BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.IssuerAndSerialNumber.GetInstance(signerID.ID);
					this.sid.Issuer = instance2.Name;
					this.sid.SerialNumber = instance2.SerialNumber.Value;
				}
			}
			catch (IOException)
			{
				throw new ArgumentException("invalid sid in SignerInfo");
			}
			this.digestAlgorithm = info.DigestAlgorithm;
			this.signedAttributeSet = info.AuthenticatedAttributes;
			this.unsignedAttributeSet = info.UnauthenticatedAttributes;
			this.encryptionAlgorithm = info.DigestEncryptionAlgorithm;
			this.signature = info.EncryptedDigest.GetOctets();
			this.content = content;
			this.digestCalculator = digestCalculator;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0016D494 File Offset: 0x0016B694
		protected SignerInformation(SignerInformation baseInfo)
		{
			this.info = baseInfo.info;
			this.contentType = baseInfo.contentType;
			this.isCounterSignature = baseInfo.IsCounterSignature;
			this.sid = baseInfo.SignerID;
			this.digestAlgorithm = this.info.DigestAlgorithm;
			this.signedAttributeSet = this.info.AuthenticatedAttributes;
			this.unsignedAttributeSet = this.info.UnauthenticatedAttributes;
			this.encryptionAlgorithm = this.info.DigestEncryptionAlgorithm;
			this.signature = this.info.EncryptedDigest.GetOctets();
			this.content = baseInfo.content;
			this.resultDigest = baseInfo.resultDigest;
			this.signedAttributeTable = baseInfo.signedAttributeTable;
			this.unsignedAttributeTable = baseInfo.unsignedAttributeTable;
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06003B1A RID: 15130 RVA: 0x0016D561 File Offset: 0x0016B761
		public bool IsCounterSignature
		{
			get
			{
				return this.isCounterSignature;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06003B1B RID: 15131 RVA: 0x0016D569 File Offset: 0x0016B769
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06003B1C RID: 15132 RVA: 0x0016D571 File Offset: 0x0016B771
		public SignerID SignerID
		{
			get
			{
				return this.sid;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06003B1D RID: 15133 RVA: 0x0016D579 File Offset: 0x0016B779
		public int Version
		{
			get
			{
				return this.info.Version.Value.IntValue;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06003B1E RID: 15134 RVA: 0x0016D590 File Offset: 0x0016B790
		public AlgorithmIdentifier DigestAlgorithmID
		{
			get
			{
				return this.digestAlgorithm;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x0016D598 File Offset: 0x0016B798
		public string DigestAlgOid
		{
			get
			{
				return this.digestAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x0016D5AC File Offset: 0x0016B7AC
		public Asn1Object DigestAlgParams
		{
			get
			{
				Asn1Encodable parameters = this.digestAlgorithm.Parameters;
				if (parameters != null)
				{
					return parameters.ToAsn1Object();
				}
				return null;
			}
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x0016D5D0 File Offset: 0x0016B7D0
		public byte[] GetContentDigest()
		{
			if (this.resultDigest == null)
			{
				throw new InvalidOperationException("method can only be called after verify.");
			}
			return (byte[])this.resultDigest.Clone();
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x0016D5F5 File Offset: 0x0016B7F5
		public AlgorithmIdentifier EncryptionAlgorithmID
		{
			get
			{
				return this.encryptionAlgorithm;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x0016D5FD File Offset: 0x0016B7FD
		public string EncryptionAlgOid
		{
			get
			{
				return this.encryptionAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06003B24 RID: 15140 RVA: 0x0016D610 File Offset: 0x0016B810
		public Asn1Object EncryptionAlgParams
		{
			get
			{
				Asn1Encodable parameters = this.encryptionAlgorithm.Parameters;
				if (parameters != null)
				{
					return parameters.ToAsn1Object();
				}
				return null;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06003B25 RID: 15141 RVA: 0x0016D634 File Offset: 0x0016B834
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable SignedAttributes
		{
			get
			{
				if (this.signedAttributeSet != null && this.signedAttributeTable == null)
				{
					this.signedAttributeTable = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(this.signedAttributeSet);
				}
				return this.signedAttributeTable;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06003B26 RID: 15142 RVA: 0x0016D65D File Offset: 0x0016B85D
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable UnsignedAttributes
		{
			get
			{
				if (this.unsignedAttributeSet != null && this.unsignedAttributeTable == null)
				{
					this.unsignedAttributeTable = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(this.unsignedAttributeSet);
				}
				return this.unsignedAttributeTable;
			}
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x0016D686 File Offset: 0x0016B886
		public byte[] GetSignature()
		{
			return (byte[])this.signature.Clone();
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x0016D698 File Offset: 0x0016B898
		public SignerInformationStore GetCounterSignatures()
		{
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = this.UnsignedAttributes;
			if (unsignedAttributes == null)
			{
				return new SignerInformationStore(Platform.CreateArrayList(0));
			}
			IList list = Platform.CreateArrayList();
			foreach (object obj in unsignedAttributes.GetAll(CmsAttributes.CounterSignature))
			{
				Asn1Set attrValues = ((Attribute)obj).AttrValues;
				int count = attrValues.Count;
				foreach (object obj2 in attrValues)
				{
					BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo instance = BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo.GetInstance(((Asn1Encodable)obj2).ToAsn1Object());
					string digestAlgName = CmsSignedHelper.Instance.GetDigestAlgName(instance.DigestAlgorithm.Algorithm.Id);
					list.Add(new SignerInformation(instance, null, null, new CounterSignatureDigestCalculator(digestAlgName, this.GetSignature())));
				}
			}
			return new SignerInformationStore(list);
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0016D7AC File Offset: 0x0016B9AC
		public byte[] GetEncodedSignedAttributes()
		{
			if (this.signedAttributeSet != null)
			{
				return this.signedAttributeSet.GetEncoded("DER");
			}
			return null;
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x0016D7C8 File Offset: 0x0016B9C8
		private bool DoVerify(AsymmetricKeyParameter key)
		{
			string digestAlgName = SignerInformation.Helper.GetDigestAlgName(this.DigestAlgOid);
			IDigest digestInstance = SignerInformation.Helper.GetDigestInstance(digestAlgName);
			object algorithm = this.encryptionAlgorithm.Algorithm;
			Asn1Encodable parameters = this.encryptionAlgorithm.Parameters;
			ISigner signer;
			if (algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss))
			{
				if (parameters == null)
				{
					throw new CmsException("RSASSA-PSS signature must specify algorithm parameters");
				}
				try
				{
					RsassaPssParameters instance = RsassaPssParameters.GetInstance(parameters.ToAsn1Object());
					if (!instance.HashAlgorithm.Algorithm.Equals(this.digestAlgorithm.Algorithm))
					{
						throw new CmsException("RSASSA-PSS signature parameters specified incorrect hash algorithm");
					}
					if (!instance.MaskGenAlgorithm.Algorithm.Equals(PkcsObjectIdentifiers.IdMgf1))
					{
						throw new CmsException("RSASSA-PSS signature parameters specified unknown MGF");
					}
					IDigest digest = DigestUtilities.GetDigest(instance.HashAlgorithm.Algorithm);
					int intValue = instance.SaltLength.Value.IntValue;
					if ((byte)instance.TrailerField.Value.IntValue != 1)
					{
						throw new CmsException("RSASSA-PSS signature parameters must have trailerField of 1");
					}
					signer = new PssSigner(new RsaBlindedEngine(), digest, intValue);
					goto IL_133;
				}
				catch (Exception e)
				{
					throw new CmsException("failed to set RSASSA-PSS signature parameters", e);
				}
			}
			string algorithm2 = digestAlgName + "with" + SignerInformation.Helper.GetEncryptionAlgName(this.EncryptionAlgOid);
			signer = SignerInformation.Helper.GetSignatureInstance(algorithm2);
			IL_133:
			try
			{
				if (this.digestCalculator != null)
				{
					this.resultDigest = this.digestCalculator.GetDigest();
				}
				else
				{
					if (this.content != null)
					{
						this.content.Write(new DigestSink(digestInstance));
					}
					else if (this.signedAttributeSet == null)
					{
						throw new CmsException("data not encapsulated in signature - use detached constructor.");
					}
					this.resultDigest = DigestUtilities.DoFinal(digestInstance);
				}
			}
			catch (IOException e2)
			{
				throw new CmsException("can't process mime object to create signature.", e2);
			}
			Asn1Object singleValuedSignedAttribute = this.GetSingleValuedSignedAttribute(CmsAttributes.ContentType, "content-type");
			if (singleValuedSignedAttribute == null)
			{
				if (!this.isCounterSignature && this.signedAttributeSet != null)
				{
					throw new CmsException("The content-type attribute type MUST be present whenever signed attributes are present in signed-data");
				}
			}
			else
			{
				if (this.isCounterSignature)
				{
					throw new CmsException("[For counter signatures,] the signedAttributes field MUST NOT contain a content-type attribute");
				}
				if (!(singleValuedSignedAttribute is DerObjectIdentifier))
				{
					throw new CmsException("content-type attribute value not of ASN.1 type 'OBJECT IDENTIFIER'");
				}
				if (!((DerObjectIdentifier)singleValuedSignedAttribute).Equals(this.contentType))
				{
					throw new CmsException("content-type attribute value does not match eContentType");
				}
			}
			Asn1Object singleValuedSignedAttribute2 = this.GetSingleValuedSignedAttribute(CmsAttributes.MessageDigest, "message-digest");
			if (singleValuedSignedAttribute2 == null)
			{
				if (this.signedAttributeSet != null)
				{
					throw new CmsException("the message-digest signed attribute type MUST be present when there are any signed attributes present");
				}
			}
			else
			{
				if (!(singleValuedSignedAttribute2 is Asn1OctetString))
				{
					throw new CmsException("message-digest attribute value not of ASN.1 type 'OCTET STRING'");
				}
				Asn1OctetString asn1OctetString = (Asn1OctetString)singleValuedSignedAttribute2;
				if (!Arrays.AreEqual(this.resultDigest, asn1OctetString.GetOctets()))
				{
					throw new CmsException("message-digest attribute value does not match calculated value");
				}
			}
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributes = this.SignedAttributes;
			if (signedAttributes != null && signedAttributes.GetAll(CmsAttributes.CounterSignature).Count > 0)
			{
				throw new CmsException("A countersignature attribute MUST NOT be a signed attribute");
			}
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = this.UnsignedAttributes;
			if (unsignedAttributes != null)
			{
				using (IEnumerator enumerator = unsignedAttributes.GetAll(CmsAttributes.CounterSignature).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Attribute)enumerator.Current).AttrValues.Count < 1)
						{
							throw new CmsException("A countersignature attribute MUST contain at least one AttributeValue");
						}
					}
				}
			}
			bool result;
			try
			{
				signer.Init(false, key);
				if (this.signedAttributeSet == null)
				{
					if (this.digestCalculator != null)
					{
						return this.VerifyDigest(this.resultDigest, key, this.GetSignature());
					}
					if (this.content == null)
					{
						goto IL_37D;
					}
					try
					{
						this.content.Write(new SignerSink(signer));
						goto IL_37D;
					}
					catch (SignatureException arg)
					{
						throw new CmsStreamException("signature problem: " + arg);
					}
				}
				byte[] encodedSignedAttributes = this.GetEncodedSignedAttributes();
				signer.BlockUpdate(encodedSignedAttributes, 0, encodedSignedAttributes.Length);
				IL_37D:
				result = signer.VerifySignature(this.GetSignature());
			}
			catch (InvalidKeyException e3)
			{
				throw new CmsException("key not appropriate to signature in message.", e3);
			}
			catch (IOException e4)
			{
				throw new CmsException("can't process mime object to create signature.", e4);
			}
			catch (SignatureException ex)
			{
				throw new CmsException("invalid signature format in message: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0016DBF8 File Offset: 0x0016BDF8
		private bool IsNull(Asn1Encodable o)
		{
			return o is Asn1Null || o == null;
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0016DC08 File Offset: 0x0016BE08
		private DigestInfo DerDecode(byte[] encoding)
		{
			if (encoding[0] != 48)
			{
				throw new IOException("not a digest info object");
			}
			DigestInfo instance = DigestInfo.GetInstance(Asn1Object.FromByteArray(encoding));
			if (instance.GetEncoded().Length != encoding.Length)
			{
				throw new CmsException("malformed RSA signature");
			}
			return instance;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0016DC40 File Offset: 0x0016BE40
		private bool VerifyDigest(byte[] digest, AsymmetricKeyParameter key, byte[] signature)
		{
			string encryptionAlgName = SignerInformation.Helper.GetEncryptionAlgName(this.EncryptionAlgOid);
			bool result;
			try
			{
				if (encryptionAlgName.Equals("RSA"))
				{
					IBufferedCipher bufferedCipher = CmsEnvelopedHelper.Instance.CreateAsymmetricCipher("RSA/ECB/PKCS1Padding");
					bufferedCipher.Init(false, key);
					byte[] encoding = bufferedCipher.DoFinal(signature);
					DigestInfo digestInfo = this.DerDecode(encoding);
					if (!digestInfo.AlgorithmID.Algorithm.Equals(this.digestAlgorithm.Algorithm))
					{
						result = false;
					}
					else if (!this.IsNull(digestInfo.AlgorithmID.Parameters))
					{
						result = false;
					}
					else
					{
						byte[] digest2 = digestInfo.GetDigest();
						result = Arrays.ConstantTimeAreEqual(digest, digest2);
					}
				}
				else
				{
					if (!encryptionAlgName.Equals("DSA"))
					{
						throw new CmsException("algorithm: " + encryptionAlgName + " not supported in base signatures.");
					}
					ISigner signer = SignerUtilities.GetSigner("NONEwithDSA");
					signer.Init(false, key);
					signer.BlockUpdate(digest, 0, digest.Length);
					result = signer.VerifySignature(signature);
				}
			}
			catch (SecurityUtilityException ex)
			{
				throw ex;
			}
			catch (GeneralSecurityException ex2)
			{
				throw new CmsException("Exception processing signature: " + ex2, ex2);
			}
			catch (IOException ex3)
			{
				throw new CmsException("Exception decoding signature: " + ex3, ex3);
			}
			return result;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0016DD84 File Offset: 0x0016BF84
		public bool Verify(AsymmetricKeyParameter pubKey)
		{
			if (pubKey.IsPrivate)
			{
				throw new ArgumentException("Expected public key", "pubKey");
			}
			this.GetSigningTime();
			return this.DoVerify(pubKey);
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x0016DDAC File Offset: 0x0016BFAC
		public bool Verify(X509Certificate cert)
		{
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Time signingTime = this.GetSigningTime();
			if (signingTime != null)
			{
				cert.CheckValidity(signingTime.Date);
			}
			return this.DoVerify(cert.GetPublicKey());
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x0016DDDB File Offset: 0x0016BFDB
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo ToSignerInfo()
		{
			return this.info;
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x0016DDE4 File Offset: 0x0016BFE4
		private Asn1Object GetSingleValuedSignedAttribute(DerObjectIdentifier attrOID, string printableName)
		{
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = this.UnsignedAttributes;
			if (unsignedAttributes != null && unsignedAttributes.GetAll(attrOID).Count > 0)
			{
				throw new CmsException("The " + printableName + " attribute MUST NOT be an unsigned attribute");
			}
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributes = this.SignedAttributes;
			if (signedAttributes == null)
			{
				return null;
			}
			Asn1EncodableVector all = signedAttributes.GetAll(attrOID);
			int count = all.Count;
			if (count == 0)
			{
				return null;
			}
			if (count != 1)
			{
				throw new CmsException("The SignedAttributes in a signerInfo MUST NOT include multiple instances of the " + printableName + " attribute");
			}
			Asn1Set attrValues = ((Attribute)all[0]).AttrValues;
			if (attrValues.Count != 1)
			{
				throw new CmsException("A " + printableName + " attribute MUST have a single attribute value");
			}
			return attrValues[0].ToAsn1Object();
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x0016DE98 File Offset: 0x0016C098
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Time GetSigningTime()
		{
			Asn1Object singleValuedSignedAttribute = this.GetSingleValuedSignedAttribute(CmsAttributes.SigningTime, "signing-time");
			if (singleValuedSignedAttribute == null)
			{
				return null;
			}
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Time instance;
			try
			{
				instance = BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Time.GetInstance(singleValuedSignedAttribute);
			}
			catch (ArgumentException)
			{
				throw new CmsException("signing-time attribute value not a valid 'Time' structure");
			}
			return instance;
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x0016DEE4 File Offset: 0x0016C0E4
		public static SignerInformation ReplaceUnsignedAttributes(SignerInformation signerInformation, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes)
		{
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo signerInfo = signerInformation.info;
			Asn1Set unauthenticatedAttributes = null;
			if (unsignedAttributes != null)
			{
				unauthenticatedAttributes = new DerSet(unsignedAttributes.ToAsn1EncodableVector());
			}
			return new SignerInformation(new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo(signerInfo.SignerID, signerInfo.DigestAlgorithm, signerInfo.AuthenticatedAttributes, signerInfo.DigestEncryptionAlgorithm, signerInfo.EncryptedDigest, unauthenticatedAttributes), signerInformation.contentType, signerInformation.content, null);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x0016DF40 File Offset: 0x0016C140
		public static SignerInformation AddCounterSigners(SignerInformation signerInformation, SignerInformationStore counterSigners)
		{
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo signerInfo = signerInformation.info;
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = signerInformation.UnsignedAttributes;
			Asn1EncodableVector asn1EncodableVector;
			if (unsignedAttributes != null)
			{
				asn1EncodableVector = unsignedAttributes.ToAsn1EncodableVector();
			}
			else
			{
				asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			}
			Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in counterSigners.GetSigners())
			{
				SignerInformation signerInformation2 = (SignerInformation)obj;
				asn1EncodableVector2.Add(new Asn1Encodable[]
				{
					signerInformation2.ToSignerInfo()
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new Attribute(CmsAttributes.CounterSignature, new DerSet(asn1EncodableVector2))
			});
			return new SignerInformation(new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo(signerInfo.SignerID, signerInfo.DigestAlgorithm, signerInfo.AuthenticatedAttributes, signerInfo.DigestEncryptionAlgorithm, signerInfo.EncryptedDigest, new DerSet(asn1EncodableVector)), signerInformation.contentType, signerInformation.content, null);
		}

		// Token: 0x04002683 RID: 9859
		private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;

		// Token: 0x04002684 RID: 9860
		private SignerID sid;

		// Token: 0x04002685 RID: 9861
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.SignerInfo info;

		// Token: 0x04002686 RID: 9862
		private AlgorithmIdentifier digestAlgorithm;

		// Token: 0x04002687 RID: 9863
		private AlgorithmIdentifier encryptionAlgorithm;

		// Token: 0x04002688 RID: 9864
		private readonly Asn1Set signedAttributeSet;

		// Token: 0x04002689 RID: 9865
		private readonly Asn1Set unsignedAttributeSet;

		// Token: 0x0400268A RID: 9866
		private CmsProcessable content;

		// Token: 0x0400268B RID: 9867
		private byte[] signature;

		// Token: 0x0400268C RID: 9868
		private DerObjectIdentifier contentType;

		// Token: 0x0400268D RID: 9869
		private IDigestCalculator digestCalculator;

		// Token: 0x0400268E RID: 9870
		private byte[] resultDigest;

		// Token: 0x0400268F RID: 9871
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributeTable;

		// Token: 0x04002690 RID: 9872
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributeTable;

		// Token: 0x04002691 RID: 9873
		private readonly bool isCounterSignature;
	}
}
