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
	// Token: 0x020005E5 RID: 1509
	public class CmsAuthenticatedDataStreamGenerator : CmsAuthenticatedGenerator
	{
		// Token: 0x06003993 RID: 14739 RVA: 0x00166FC1 File Offset: 0x001651C1
		public CmsAuthenticatedDataStreamGenerator()
		{
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x00166FC9 File Offset: 0x001651C9
		public CmsAuthenticatedDataStreamGenerator(SecureRandom rand) : base(rand)
		{
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x00167401 File Offset: 0x00165601
		public void SetBufferSize(int bufferSize)
		{
			this._bufferSize = bufferSize;
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0016740A File Offset: 0x0016560A
		public void SetBerEncodeRecipients(bool berEncodeRecipientSet)
		{
			this._berEncodeRecipientSet = berEncodeRecipientSet;
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x00167414 File Offset: 0x00165614
		private Stream Open(Stream outStr, string macOid, CipherKeyGenerator keyGen)
		{
			byte[] array = keyGen.GenerateKey();
			KeyParameter keyParameter = ParameterUtilities.CreateKeyParameter(macOid, array);
			Asn1Encodable asn1Params = this.GenerateAsn1Parameters(macOid, array);
			ICipherParameters cipherParameters;
			AlgorithmIdentifier algorithmIdentifier = this.GetAlgorithmIdentifier(macOid, keyParameter, asn1Params, out cipherParameters);
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
			return this.Open(outStr, algorithmIdentifier, keyParameter, asn1EncodableVector);
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x00167504 File Offset: 0x00165704
		protected Stream Open(Stream outStr, AlgorithmIdentifier macAlgId, ICipherParameters cipherParameters, Asn1EncodableVector recipientInfos)
		{
			Stream result;
			try
			{
				BerSequenceGenerator berSequenceGenerator = new BerSequenceGenerator(outStr);
				berSequenceGenerator.AddObject(CmsObjectIdentifiers.AuthenticatedData);
				BerSequenceGenerator berSequenceGenerator2 = new BerSequenceGenerator(berSequenceGenerator.GetRawOutputStream(), 0, true);
				berSequenceGenerator2.AddObject(new DerInteger(AuthenticatedData.CalculateVersion(null)));
				Stream rawOutputStream = berSequenceGenerator2.GetRawOutputStream();
				Asn1Generator asn1Generator = this._berEncodeRecipientSet ? new BerSetGenerator(rawOutputStream) : new DerSetGenerator(rawOutputStream);
				foreach (object obj in recipientInfos)
				{
					Asn1Encodable obj2 = (Asn1Encodable)obj;
					asn1Generator.AddObject(obj2);
				}
				asn1Generator.Close();
				berSequenceGenerator2.AddObject(macAlgId);
				BerSequenceGenerator berSequenceGenerator3 = new BerSequenceGenerator(rawOutputStream);
				berSequenceGenerator3.AddObject(CmsObjectIdentifiers.Data);
				Stream output = CmsUtilities.CreateBerOctetOutputStream(berSequenceGenerator3.GetRawOutputStream(), 0, false, this._bufferSize);
				IMac mac = MacUtilities.GetMac(macAlgId.Algorithm);
				mac.Init(cipherParameters);
				result = new CmsAuthenticatedDataStreamGenerator.CmsAuthenticatedDataOutputStream(new TeeOutputStream(output, new MacSink(mac)), mac, berSequenceGenerator, berSequenceGenerator2, berSequenceGenerator3);
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

		// Token: 0x06003999 RID: 14745 RVA: 0x0016766C File Offset: 0x0016586C
		public Stream Open(Stream outStr, string encryptionOid)
		{
			CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator(encryptionOid);
			keyGenerator.Init(new KeyGenerationParameters(this.rand, keyGenerator.DefaultStrength));
			return this.Open(outStr, encryptionOid, keyGenerator);
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001676A0 File Offset: 0x001658A0
		public Stream Open(Stream outStr, string encryptionOid, int keySize)
		{
			CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator(encryptionOid);
			keyGenerator.Init(new KeyGenerationParameters(this.rand, keySize));
			return this.Open(outStr, encryptionOid, keyGenerator);
		}

		// Token: 0x040025C6 RID: 9670
		private int _bufferSize;

		// Token: 0x040025C7 RID: 9671
		private bool _berEncodeRecipientSet;

		// Token: 0x0200097E RID: 2430
		private class CmsAuthenticatedDataOutputStream : BaseOutputStream
		{
			// Token: 0x06004F9F RID: 20383 RVA: 0x001B6D51 File Offset: 0x001B4F51
			public CmsAuthenticatedDataOutputStream(Stream macStream, IMac mac, BerSequenceGenerator cGen, BerSequenceGenerator authGen, BerSequenceGenerator eiGen)
			{
				this.macStream = macStream;
				this.mac = mac;
				this.cGen = cGen;
				this.authGen = authGen;
				this.eiGen = eiGen;
			}

			// Token: 0x06004FA0 RID: 20384 RVA: 0x001B6D7E File Offset: 0x001B4F7E
			public override void WriteByte(byte b)
			{
				this.macStream.WriteByte(b);
			}

			// Token: 0x06004FA1 RID: 20385 RVA: 0x001B6D8C File Offset: 0x001B4F8C
			public override void Write(byte[] bytes, int off, int len)
			{
				this.macStream.Write(bytes, off, len);
			}

			// Token: 0x06004FA2 RID: 20386 RVA: 0x001B6D9C File Offset: 0x001B4F9C
			public override void Close()
			{
				Platform.Dispose(this.macStream);
				this.eiGen.Close();
				byte[] str = MacUtilities.DoFinal(this.mac);
				this.authGen.AddObject(new DerOctetString(str));
				this.authGen.Close();
				this.cGen.Close();
				base.Close();
			}

			// Token: 0x040036DA RID: 14042
			private readonly Stream macStream;

			// Token: 0x040036DB RID: 14043
			private readonly IMac mac;

			// Token: 0x040036DC RID: 14044
			private readonly BerSequenceGenerator cGen;

			// Token: 0x040036DD RID: 14045
			private readonly BerSequenceGenerator authGen;

			// Token: 0x040036DE RID: 14046
			private readonly BerSequenceGenerator eiGen;
		}
	}
}
