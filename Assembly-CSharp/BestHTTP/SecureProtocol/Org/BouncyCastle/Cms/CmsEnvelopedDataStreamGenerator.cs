using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F1 RID: 1521
	public class CmsEnvelopedDataStreamGenerator : CmsEnvelopedGenerator
	{
		// Token: 0x060039C9 RID: 14793 RVA: 0x001676CF File Offset: 0x001658CF
		public CmsEnvelopedDataStreamGenerator()
		{
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x001676D7 File Offset: 0x001658D7
		public CmsEnvelopedDataStreamGenerator(SecureRandom rand) : base(rand)
		{
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x00168025 File Offset: 0x00166225
		public void SetBufferSize(int bufferSize)
		{
			this._bufferSize = bufferSize;
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x0016802E File Offset: 0x0016622E
		public void SetBerEncodeRecipients(bool berEncodeRecipientSet)
		{
			this._berEncodeRecipientSet = berEncodeRecipientSet;
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x00168037 File Offset: 0x00166237
		private DerInteger Version
		{
			get
			{
				return new DerInteger((this._originatorInfo != null || this._unprotectedAttributes != null) ? 2 : 0);
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x00168054 File Offset: 0x00166254
		private Stream Open(Stream outStream, string encryptionOid, CipherKeyGenerator keyGen)
		{
			byte[] array = keyGen.GenerateKey();
			KeyParameter keyParameter = ParameterUtilities.CreateKeyParameter(encryptionOid, array);
			Asn1Encodable asn1Params = this.GenerateAsn1Parameters(encryptionOid, array);
			ICipherParameters cipherParameters;
			AlgorithmIdentifier algorithmIdentifier = this.GetAlgorithmIdentifier(encryptionOid, keyParameter, asn1Params, out cipherParameters);
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.recipientInfoGenerators)
			{
				RecipientInfoGenerator recipientInfoGenerator = (RecipientInfoGenerator)obj;
				try
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						recipientInfoGenerator.Generate(keyParameter, this.rand)
					});
				}
				catch (InvalidKeyException e)
				{
					throw new CmsException("key inappropriate for algorithm.", e);
				}
				catch (GeneralSecurityException e2)
				{
					throw new CmsException("error making encrypted content.", e2);
				}
			}
			return this.Open(outStream, algorithmIdentifier, cipherParameters, asn1EncodableVector);
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x00168144 File Offset: 0x00166344
		private Stream Open(Stream outStream, AlgorithmIdentifier encAlgID, ICipherParameters cipherParameters, Asn1EncodableVector recipientInfos)
		{
			Stream result;
			try
			{
				BerSequenceGenerator berSequenceGenerator = new BerSequenceGenerator(outStream);
				berSequenceGenerator.AddObject(CmsObjectIdentifiers.EnvelopedData);
				BerSequenceGenerator berSequenceGenerator2 = new BerSequenceGenerator(berSequenceGenerator.GetRawOutputStream(), 0, true);
				berSequenceGenerator2.AddObject(this.Version);
				Stream rawOutputStream = berSequenceGenerator2.GetRawOutputStream();
				Asn1Generator asn1Generator = this._berEncodeRecipientSet ? new BerSetGenerator(rawOutputStream) : new DerSetGenerator(rawOutputStream);
				foreach (object obj in recipientInfos)
				{
					Asn1Encodable obj2 = (Asn1Encodable)obj;
					asn1Generator.AddObject(obj2);
				}
				asn1Generator.Close();
				BerSequenceGenerator berSequenceGenerator3 = new BerSequenceGenerator(rawOutputStream);
				berSequenceGenerator3.AddObject(CmsObjectIdentifiers.Data);
				berSequenceGenerator3.AddObject(encAlgID);
				Stream stream = CmsUtilities.CreateBerOctetOutputStream(berSequenceGenerator3.GetRawOutputStream(), 0, false, this._bufferSize);
				IBufferedCipher cipher = CipherUtilities.GetCipher(encAlgID.Algorithm);
				cipher.Init(true, new ParametersWithRandom(cipherParameters, this.rand));
				CipherStream outStream2 = new CipherStream(stream, null, cipher);
				result = new CmsEnvelopedDataStreamGenerator.CmsEnvelopedDataOutputStream(this, outStream2, berSequenceGenerator, berSequenceGenerator2, berSequenceGenerator3);
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e2)
			{
				throw new CmsException("key invalid in message.", e2);
			}
			catch (IOException e3)
			{
				throw new CmsException("exception decoding algorithm parameters.", e3);
			}
			return result;
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001682B0 File Offset: 0x001664B0
		public Stream Open(Stream outStream, string encryptionOid)
		{
			CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator(encryptionOid);
			keyGenerator.Init(new KeyGenerationParameters(this.rand, keyGenerator.DefaultStrength));
			return this.Open(outStream, encryptionOid, keyGenerator);
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x001682E4 File Offset: 0x001664E4
		public Stream Open(Stream outStream, string encryptionOid, int keySize)
		{
			CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator(encryptionOid);
			keyGenerator.Init(new KeyGenerationParameters(this.rand, keySize));
			return this.Open(outStream, encryptionOid, keyGenerator);
		}

		// Token: 0x040025E4 RID: 9700
		private object _originatorInfo;

		// Token: 0x040025E5 RID: 9701
		private object _unprotectedAttributes;

		// Token: 0x040025E6 RID: 9702
		private int _bufferSize;

		// Token: 0x040025E7 RID: 9703
		private bool _berEncodeRecipientSet;

		// Token: 0x02000981 RID: 2433
		private class CmsEnvelopedDataOutputStream : BaseOutputStream
		{
			// Token: 0x06004FAB RID: 20395 RVA: 0x001B6E97 File Offset: 0x001B5097
			public CmsEnvelopedDataOutputStream(CmsEnvelopedGenerator outer, CipherStream outStream, BerSequenceGenerator cGen, BerSequenceGenerator envGen, BerSequenceGenerator eiGen)
			{
				this._outer = outer;
				this._out = outStream;
				this._cGen = cGen;
				this._envGen = envGen;
				this._eiGen = eiGen;
			}

			// Token: 0x06004FAC RID: 20396 RVA: 0x001B6EC4 File Offset: 0x001B50C4
			public override void WriteByte(byte b)
			{
				this._out.WriteByte(b);
			}

			// Token: 0x06004FAD RID: 20397 RVA: 0x001B6ED2 File Offset: 0x001B50D2
			public override void Write(byte[] bytes, int off, int len)
			{
				this._out.Write(bytes, off, len);
			}

			// Token: 0x06004FAE RID: 20398 RVA: 0x001B6EE4 File Offset: 0x001B50E4
			public override void Close()
			{
				Platform.Dispose(this._out);
				this._eiGen.Close();
				if (this._outer.unprotectedAttributeGenerator != null)
				{
					Asn1Set obj = new BerSet(this._outer.unprotectedAttributeGenerator.GetAttributes(Platform.CreateHashtable()).ToAsn1EncodableVector());
					this._envGen.AddObject(new DerTaggedObject(false, 1, obj));
				}
				this._envGen.Close();
				this._cGen.Close();
				base.Close();
			}

			// Token: 0x040036E4 RID: 14052
			private readonly CmsEnvelopedGenerator _outer;

			// Token: 0x040036E5 RID: 14053
			private readonly CipherStream _out;

			// Token: 0x040036E6 RID: 14054
			private readonly BerSequenceGenerator _cGen;

			// Token: 0x040036E7 RID: 14055
			private readonly BerSequenceGenerator _envGen;

			// Token: 0x040036E8 RID: 14056
			private readonly BerSequenceGenerator _eiGen;
		}
	}
}
