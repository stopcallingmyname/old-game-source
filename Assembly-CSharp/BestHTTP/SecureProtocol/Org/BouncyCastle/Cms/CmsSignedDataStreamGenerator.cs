using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005FF RID: 1535
	public class CmsSignedDataStreamGenerator : CmsSignedGenerator
	{
		// Token: 0x06003A49 RID: 14921 RVA: 0x00169BC9 File Offset: 0x00167DC9
		public CmsSignedDataStreamGenerator()
		{
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00169BFD File Offset: 0x00167DFD
		public CmsSignedDataStreamGenerator(SecureRandom rand) : base(rand)
		{
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00169C32 File Offset: 0x00167E32
		public void SetBufferSize(int bufferSize)
		{
			this._bufferSize = bufferSize;
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x00169C3B File Offset: 0x00167E3B
		public void AddDigests(params string[] digestOids)
		{
			this.AddDigests(digestOids);
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x00169C44 File Offset: 0x00167E44
		public void AddDigests(IEnumerable digestOids)
		{
			foreach (object obj in digestOids)
			{
				string digestOid = (string)obj;
				this.ConfigureDigest(digestOid);
			}
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00169C98 File Offset: 0x00167E98
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string digestOid)
		{
			this.AddSigner(privateKey, cert, digestOid, new DefaultSignedAttributeTableGenerator(), null);
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00169CA9 File Offset: 0x00167EA9
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string encryptionOid, string digestOid)
		{
			this.AddSigner(privateKey, cert, encryptionOid, digestOid, new DefaultSignedAttributeTableGenerator(), null);
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00169CBC File Offset: 0x00167EBC
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string digestOid, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.AddSigner(privateKey, cert, digestOid, new DefaultSignedAttributeTableGenerator(signedAttr), new SimpleAttributeTableGenerator(unsignedAttr));
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x00169CD5 File Offset: 0x00167ED5
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string encryptionOid, string digestOid, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.AddSigner(privateKey, cert, encryptionOid, digestOid, new DefaultSignedAttributeTableGenerator(signedAttr), new SimpleAttributeTableGenerator(unsignedAttr));
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x00169CF0 File Offset: 0x00167EF0
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string digestOid, CmsAttributeTableGenerator signedAttrGenerator, CmsAttributeTableGenerator unsignedAttrGenerator)
		{
			this.AddSigner(privateKey, cert, CmsSignedDataStreamGenerator.Helper.GetEncOid(privateKey, digestOid), digestOid, signedAttrGenerator, unsignedAttrGenerator);
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x00169D0B File Offset: 0x00167F0B
		public void AddSigner(AsymmetricKeyParameter privateKey, X509Certificate cert, string encryptionOid, string digestOid, CmsAttributeTableGenerator signedAttrGenerator, CmsAttributeTableGenerator unsignedAttrGenerator)
		{
			this.DoAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(cert), encryptionOid, digestOid, signedAttrGenerator, unsignedAttrGenerator);
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x00169D21 File Offset: 0x00167F21
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOid)
		{
			this.AddSigner(privateKey, subjectKeyID, digestOid, new DefaultSignedAttributeTableGenerator(), null);
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00169D32 File Offset: 0x00167F32
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string encryptionOid, string digestOid)
		{
			this.AddSigner(privateKey, subjectKeyID, encryptionOid, digestOid, new DefaultSignedAttributeTableGenerator(), null);
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00169D45 File Offset: 0x00167F45
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOid, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.AddSigner(privateKey, subjectKeyID, digestOid, new DefaultSignedAttributeTableGenerator(signedAttr), new SimpleAttributeTableGenerator(unsignedAttr));
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x00169D5E File Offset: 0x00167F5E
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string digestOid, CmsAttributeTableGenerator signedAttrGenerator, CmsAttributeTableGenerator unsignedAttrGenerator)
		{
			this.AddSigner(privateKey, subjectKeyID, CmsSignedDataStreamGenerator.Helper.GetEncOid(privateKey, digestOid), digestOid, signedAttrGenerator, unsignedAttrGenerator);
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x00169D79 File Offset: 0x00167F79
		public void AddSigner(AsymmetricKeyParameter privateKey, byte[] subjectKeyID, string encryptionOid, string digestOid, CmsAttributeTableGenerator signedAttrGenerator, CmsAttributeTableGenerator unsignedAttrGenerator)
		{
			this.DoAddSigner(privateKey, CmsSignedGenerator.GetSignerIdentifier(subjectKeyID), encryptionOid, digestOid, signedAttrGenerator, unsignedAttrGenerator);
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x00169D90 File Offset: 0x00167F90
		private void DoAddSigner(AsymmetricKeyParameter privateKey, SignerIdentifier signerIdentifier, string encryptionOid, string digestOid, CmsAttributeTableGenerator signedAttrGenerator, CmsAttributeTableGenerator unsignedAttrGenerator)
		{
			this.ConfigureDigest(digestOid);
			CmsSignedDataStreamGenerator.SignerInfoGeneratorImpl signerInf = new CmsSignedDataStreamGenerator.SignerInfoGeneratorImpl(this, privateKey, signerIdentifier, digestOid, encryptionOid, signedAttrGenerator, unsignedAttrGenerator);
			this._signerInfs.Add(new CmsSignedDataStreamGenerator.DigestAndSignerInfoGeneratorHolder(signerInf, digestOid));
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x00169DC9 File Offset: 0x00167FC9
		internal override void AddSignerCallback(SignerInformation si)
		{
			this.RegisterDigestOid(si.DigestAlgorithmID.Algorithm.Id);
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x00169DE1 File Offset: 0x00167FE1
		public Stream Open(Stream outStream)
		{
			return this.Open(outStream, false);
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00169DEB File Offset: 0x00167FEB
		public Stream Open(Stream outStream, bool encapsulate)
		{
			return this.Open(outStream, CmsSignedGenerator.Data, encapsulate);
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00169DFA File Offset: 0x00167FFA
		public Stream Open(Stream outStream, bool encapsulate, Stream dataOutputStream)
		{
			return this.Open(outStream, CmsSignedGenerator.Data, encapsulate, dataOutputStream);
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x00169E0A File Offset: 0x0016800A
		public Stream Open(Stream outStream, string signedContentType, bool encapsulate)
		{
			return this.Open(outStream, signedContentType, encapsulate, null);
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00169E18 File Offset: 0x00168018
		public Stream Open(Stream outStream, string signedContentType, bool encapsulate, Stream dataOutputStream)
		{
			if (outStream == null)
			{
				throw new ArgumentNullException("outStream");
			}
			if (!outStream.CanWrite)
			{
				throw new ArgumentException("Expected writeable stream", "outStream");
			}
			if (dataOutputStream != null && !dataOutputStream.CanWrite)
			{
				throw new ArgumentException("Expected writeable stream", "dataOutputStream");
			}
			this._messageDigestsLocked = true;
			BerSequenceGenerator berSequenceGenerator = new BerSequenceGenerator(outStream);
			berSequenceGenerator.AddObject(CmsObjectIdentifiers.SignedData);
			BerSequenceGenerator berSequenceGenerator2 = new BerSequenceGenerator(berSequenceGenerator.GetRawOutputStream(), 0, true);
			DerObjectIdentifier derObjectIdentifier = (signedContentType == null) ? null : new DerObjectIdentifier(signedContentType);
			berSequenceGenerator2.AddObject(this.CalculateVersion(derObjectIdentifier));
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this._messageDigestOids)
			{
				string identifier = (string)obj;
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new AlgorithmIdentifier(new DerObjectIdentifier(identifier), DerNull.Instance)
				});
			}
			byte[] encoded = new DerSet(asn1EncodableVector).GetEncoded();
			berSequenceGenerator2.GetRawOutputStream().Write(encoded, 0, encoded.Length);
			BerSequenceGenerator berSequenceGenerator3 = new BerSequenceGenerator(berSequenceGenerator2.GetRawOutputStream());
			berSequenceGenerator3.AddObject(derObjectIdentifier);
			Stream s = encapsulate ? CmsUtilities.CreateBerOctetOutputStream(berSequenceGenerator3.GetRawOutputStream(), 0, true, this._bufferSize) : null;
			Stream safeTeeOutputStream = CmsSignedDataStreamGenerator.GetSafeTeeOutputStream(dataOutputStream, s);
			Stream outStream2 = CmsSignedDataStreamGenerator.AttachDigestsToOutputStream(this._messageDigests.Values, safeTeeOutputStream);
			return new CmsSignedDataStreamGenerator.CmsSignedDataOutputStream(this, outStream2, signedContentType, berSequenceGenerator, berSequenceGenerator2, berSequenceGenerator3);
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x00169FA0 File Offset: 0x001681A0
		private void RegisterDigestOid(string digestOid)
		{
			if (this._messageDigestsLocked)
			{
				if (!this._messageDigestOids.Contains(digestOid))
				{
					throw new InvalidOperationException("Cannot register new digest OIDs after the data stream is opened");
				}
			}
			else
			{
				this._messageDigestOids.Add(digestOid);
			}
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x00169FD0 File Offset: 0x001681D0
		private void ConfigureDigest(string digestOid)
		{
			this.RegisterDigestOid(digestOid);
			string digestAlgName = CmsSignedDataStreamGenerator.Helper.GetDigestAlgName(digestOid);
			if ((IDigest)this._messageDigests[digestAlgName] == null)
			{
				if (this._messageDigestsLocked)
				{
					throw new InvalidOperationException("Cannot configure new digests after the data stream is opened");
				}
				IDigest digestInstance = CmsSignedDataStreamGenerator.Helper.GetDigestInstance(digestAlgName);
				this._messageDigests[digestAlgName] = digestInstance;
			}
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x0016A034 File Offset: 0x00168234
		internal void Generate(Stream outStream, string eContentType, bool encapsulate, Stream dataOutputStream, CmsProcessable content)
		{
			Stream stream = this.Open(outStream, eContentType, encapsulate, dataOutputStream);
			if (content != null)
			{
				content.Write(stream);
			}
			Platform.Dispose(stream);
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x0016A060 File Offset: 0x00168260
		private DerInteger CalculateVersion(DerObjectIdentifier contentOid)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (this._certs != null)
			{
				foreach (object obj in this._certs)
				{
					if (obj is Asn1TaggedObject)
					{
						Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
						if (asn1TaggedObject.TagNo == 1)
						{
							flag3 = true;
						}
						else if (asn1TaggedObject.TagNo == 2)
						{
							flag4 = true;
						}
						else if (asn1TaggedObject.TagNo == 3)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				return new DerInteger(5);
			}
			if (this._crls != null)
			{
				using (IEnumerator enumerator = this._crls.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Asn1TaggedObject)
						{
							flag2 = true;
							break;
						}
					}
				}
			}
			if (flag2)
			{
				return new DerInteger(5);
			}
			if (flag4)
			{
				return new DerInteger(4);
			}
			if (flag3 || !CmsObjectIdentifiers.Data.Equals(contentOid) || this.CheckForVersion3(this._signers))
			{
				return new DerInteger(3);
			}
			return new DerInteger(1);
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x0016A1A0 File Offset: 0x001683A0
		private bool CheckForVersion3(IList signerInfos)
		{
			using (IEnumerator enumerator = signerInfos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (SignerInfo.GetInstance(((SignerInformation)enumerator.Current).ToSignerInfo()).Version.Value.IntValue == 3)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x0016A210 File Offset: 0x00168410
		private static Stream AttachDigestsToOutputStream(ICollection digests, Stream s)
		{
			Stream stream = s;
			foreach (object obj in digests)
			{
				IDigest digest = (IDigest)obj;
				stream = CmsSignedDataStreamGenerator.GetSafeTeeOutputStream(stream, new DigestSink(digest));
			}
			return stream;
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x0016A270 File Offset: 0x00168470
		private static Stream GetSafeOutputStream(Stream s)
		{
			if (s == null)
			{
				return new NullOutputStream();
			}
			return s;
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x0016A27C File Offset: 0x0016847C
		private static Stream GetSafeTeeOutputStream(Stream s1, Stream s2)
		{
			if (s1 == null)
			{
				return CmsSignedDataStreamGenerator.GetSafeOutputStream(s2);
			}
			if (s2 == null)
			{
				return CmsSignedDataStreamGenerator.GetSafeOutputStream(s1);
			}
			return new TeeOutputStream(s1, s2);
		}

		// Token: 0x04002625 RID: 9765
		private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;

		// Token: 0x04002626 RID: 9766
		private readonly IList _signerInfs = Platform.CreateArrayList();

		// Token: 0x04002627 RID: 9767
		private readonly ISet _messageDigestOids = new HashSet();

		// Token: 0x04002628 RID: 9768
		private readonly IDictionary _messageDigests = Platform.CreateHashtable();

		// Token: 0x04002629 RID: 9769
		private readonly IDictionary _messageHashes = Platform.CreateHashtable();

		// Token: 0x0400262A RID: 9770
		private bool _messageDigestsLocked;

		// Token: 0x0400262B RID: 9771
		private int _bufferSize;

		// Token: 0x02000985 RID: 2437
		private class DigestAndSignerInfoGeneratorHolder
		{
			// Token: 0x06004FBD RID: 20413 RVA: 0x001B74D7 File Offset: 0x001B56D7
			internal DigestAndSignerInfoGeneratorHolder(ISignerInfoGenerator signerInf, string digestOID)
			{
				this.signerInf = signerInf;
				this.digestOID = digestOID;
			}

			// Token: 0x17000C6B RID: 3179
			// (get) Token: 0x06004FBE RID: 20414 RVA: 0x001B74ED File Offset: 0x001B56ED
			internal AlgorithmIdentifier DigestAlgorithm
			{
				get
				{
					return new AlgorithmIdentifier(new DerObjectIdentifier(this.digestOID), DerNull.Instance);
				}
			}

			// Token: 0x040036F7 RID: 14071
			internal readonly ISignerInfoGenerator signerInf;

			// Token: 0x040036F8 RID: 14072
			internal readonly string digestOID;
		}

		// Token: 0x02000986 RID: 2438
		private class SignerInfoGeneratorImpl : ISignerInfoGenerator
		{
			// Token: 0x06004FBF RID: 20415 RVA: 0x001B7504 File Offset: 0x001B5704
			internal SignerInfoGeneratorImpl(CmsSignedDataStreamGenerator outer, AsymmetricKeyParameter key, SignerIdentifier signerIdentifier, string digestOID, string encOID, CmsAttributeTableGenerator sAttr, CmsAttributeTableGenerator unsAttr)
			{
				this.outer = outer;
				this._signerIdentifier = signerIdentifier;
				this._digestOID = digestOID;
				this._encOID = encOID;
				this._sAttr = sAttr;
				this._unsAttr = unsAttr;
				this._encName = CmsSignedDataStreamGenerator.Helper.GetEncryptionAlgName(this._encOID);
				string algorithm = CmsSignedDataStreamGenerator.Helper.GetDigestAlgName(this._digestOID) + "with" + this._encName;
				if (this._sAttr != null)
				{
					this._sig = CmsSignedDataStreamGenerator.Helper.GetSignatureInstance(algorithm);
				}
				else if (this._encName.Equals("RSA"))
				{
					this._sig = CmsSignedDataStreamGenerator.Helper.GetSignatureInstance("RSA");
				}
				else
				{
					if (!this._encName.Equals("DSA"))
					{
						throw new SignatureException("algorithm: " + this._encName + " not supported in base signatures.");
					}
					this._sig = CmsSignedDataStreamGenerator.Helper.GetSignatureInstance("NONEwithDSA");
				}
				this._sig.Init(true, new ParametersWithRandom(key, outer.rand));
			}

			// Token: 0x06004FC0 RID: 20416 RVA: 0x001B761C File Offset: 0x001B581C
			public SignerInfo Generate(DerObjectIdentifier contentType, AlgorithmIdentifier digestAlgorithm, byte[] calculatedDigest)
			{
				SignerInfo result;
				try
				{
					string algorithm = CmsSignedDataStreamGenerator.Helper.GetDigestAlgName(this._digestOID) + "with" + this._encName;
					byte[] array = calculatedDigest;
					Asn1Set asn1Set = null;
					if (this._sAttr != null)
					{
						IDictionary baseParameters = this.outer.GetBaseParameters(contentType, digestAlgorithm, calculatedDigest);
						BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable attributeTable = this._sAttr.GetAttributes(baseParameters);
						if (contentType == null && attributeTable != null && attributeTable[CmsAttributes.ContentType] != null)
						{
							IDictionary dictionary = attributeTable.ToDictionary();
							dictionary.Remove(CmsAttributes.ContentType);
							attributeTable = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(dictionary);
						}
						asn1Set = this.outer.GetAttributeSet(attributeTable);
						array = asn1Set.GetEncoded("DER");
					}
					else if (this._encName.Equals("RSA"))
					{
						array = new DigestInfo(digestAlgorithm, calculatedDigest).GetEncoded("DER");
					}
					this._sig.BlockUpdate(array, 0, array.Length);
					byte[] array2 = this._sig.GenerateSignature();
					Asn1Set unauthenticatedAttributes = null;
					if (this._unsAttr != null)
					{
						IDictionary baseParameters2 = this.outer.GetBaseParameters(contentType, digestAlgorithm, calculatedDigest);
						baseParameters2[CmsAttributeTableParameter.Signature] = array2.Clone();
						BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable attributes = this._unsAttr.GetAttributes(baseParameters2);
						unauthenticatedAttributes = this.outer.GetAttributeSet(attributes);
					}
					Asn1Encodable defaultX509Parameters = SignerUtilities.GetDefaultX509Parameters(algorithm);
					AlgorithmIdentifier encAlgorithmIdentifier = CmsSignedDataStreamGenerator.Helper.GetEncAlgorithmIdentifier(new DerObjectIdentifier(this._encOID), defaultX509Parameters);
					result = new SignerInfo(this._signerIdentifier, digestAlgorithm, asn1Set, encAlgorithmIdentifier, new DerOctetString(array2), unauthenticatedAttributes);
				}
				catch (IOException e)
				{
					throw new CmsStreamException("encoding error.", e);
				}
				catch (SignatureException e2)
				{
					throw new CmsStreamException("error creating signature.", e2);
				}
				return result;
			}

			// Token: 0x040036F9 RID: 14073
			private readonly CmsSignedDataStreamGenerator outer;

			// Token: 0x040036FA RID: 14074
			private readonly SignerIdentifier _signerIdentifier;

			// Token: 0x040036FB RID: 14075
			private readonly string _digestOID;

			// Token: 0x040036FC RID: 14076
			private readonly string _encOID;

			// Token: 0x040036FD RID: 14077
			private readonly CmsAttributeTableGenerator _sAttr;

			// Token: 0x040036FE RID: 14078
			private readonly CmsAttributeTableGenerator _unsAttr;

			// Token: 0x040036FF RID: 14079
			private readonly string _encName;

			// Token: 0x04003700 RID: 14080
			private readonly ISigner _sig;
		}

		// Token: 0x02000987 RID: 2439
		private class CmsSignedDataOutputStream : BaseOutputStream
		{
			// Token: 0x06004FC1 RID: 20417 RVA: 0x001B77D8 File Offset: 0x001B59D8
			public CmsSignedDataOutputStream(CmsSignedDataStreamGenerator outer, Stream outStream, string contentOID, BerSequenceGenerator sGen, BerSequenceGenerator sigGen, BerSequenceGenerator eiGen)
			{
				this.outer = outer;
				this._out = outStream;
				this._contentOID = new DerObjectIdentifier(contentOID);
				this._sGen = sGen;
				this._sigGen = sigGen;
				this._eiGen = eiGen;
			}

			// Token: 0x06004FC2 RID: 20418 RVA: 0x001B7812 File Offset: 0x001B5A12
			public override void WriteByte(byte b)
			{
				this._out.WriteByte(b);
			}

			// Token: 0x06004FC3 RID: 20419 RVA: 0x001B7820 File Offset: 0x001B5A20
			public override void Write(byte[] bytes, int off, int len)
			{
				this._out.Write(bytes, off, len);
			}

			// Token: 0x06004FC4 RID: 20420 RVA: 0x001B7830 File Offset: 0x001B5A30
			public override void Close()
			{
				this.DoClose();
				base.Close();
			}

			// Token: 0x06004FC5 RID: 20421 RVA: 0x001B7840 File Offset: 0x001B5A40
			private void DoClose()
			{
				Platform.Dispose(this._out);
				this._eiGen.Close();
				this.outer._digests.Clear();
				if (this.outer._certs.Count > 0)
				{
					Asn1Set obj = this.outer.UseDerForCerts ? CmsUtilities.CreateDerSetFromList(this.outer._certs) : CmsUtilities.CreateBerSetFromList(this.outer._certs);
					CmsSignedDataStreamGenerator.CmsSignedDataOutputStream.WriteToGenerator(this._sigGen, new BerTaggedObject(false, 0, obj));
				}
				if (this.outer._crls.Count > 0)
				{
					Asn1Set obj2 = this.outer.UseDerForCrls ? CmsUtilities.CreateDerSetFromList(this.outer._crls) : CmsUtilities.CreateBerSetFromList(this.outer._crls);
					CmsSignedDataStreamGenerator.CmsSignedDataOutputStream.WriteToGenerator(this._sigGen, new BerTaggedObject(false, 1, obj2));
				}
				foreach (object obj3 in this.outer._messageDigests)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					this.outer._messageHashes.Add(dictionaryEntry.Key, DigestUtilities.DoFinal((IDigest)dictionaryEntry.Value));
				}
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				foreach (object obj4 in this.outer._signerInfs)
				{
					CmsSignedDataStreamGenerator.DigestAndSignerInfoGeneratorHolder digestAndSignerInfoGeneratorHolder = (CmsSignedDataStreamGenerator.DigestAndSignerInfoGeneratorHolder)obj4;
					AlgorithmIdentifier digestAlgorithm = digestAndSignerInfoGeneratorHolder.DigestAlgorithm;
					byte[] array = (byte[])this.outer._messageHashes[CmsSignedDataStreamGenerator.Helper.GetDigestAlgName(digestAndSignerInfoGeneratorHolder.digestOID)];
					this.outer._digests[digestAndSignerInfoGeneratorHolder.digestOID] = array.Clone();
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						digestAndSignerInfoGeneratorHolder.signerInf.Generate(this._contentOID, digestAlgorithm, array)
					});
				}
				foreach (object obj5 in this.outer._signers)
				{
					SignerInformation signerInformation = (SignerInformation)obj5;
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						signerInformation.ToSignerInfo()
					});
				}
				CmsSignedDataStreamGenerator.CmsSignedDataOutputStream.WriteToGenerator(this._sigGen, new DerSet(asn1EncodableVector));
				this._sigGen.Close();
				this._sGen.Close();
			}

			// Token: 0x06004FC6 RID: 20422 RVA: 0x001B7AF4 File Offset: 0x001B5CF4
			private static void WriteToGenerator(Asn1Generator ag, Asn1Encodable ae)
			{
				byte[] encoded = ae.GetEncoded();
				ag.GetRawOutputStream().Write(encoded, 0, encoded.Length);
			}

			// Token: 0x04003701 RID: 14081
			private readonly CmsSignedDataStreamGenerator outer;

			// Token: 0x04003702 RID: 14082
			private Stream _out;

			// Token: 0x04003703 RID: 14083
			private DerObjectIdentifier _contentOID;

			// Token: 0x04003704 RID: 14084
			private BerSequenceGenerator _sGen;

			// Token: 0x04003705 RID: 14085
			private BerSequenceGenerator _sigGen;

			// Token: 0x04003706 RID: 14086
			private BerSequenceGenerator _eiGen;
		}
	}
}
