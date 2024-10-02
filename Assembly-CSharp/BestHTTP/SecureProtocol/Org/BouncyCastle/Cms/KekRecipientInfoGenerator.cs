using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Kisa;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ntt;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200060A RID: 1546
	internal class KekRecipientInfoGenerator : RecipientInfoGenerator
	{
		// Token: 0x06003AB1 RID: 15025 RVA: 0x00022F1F File Offset: 0x0002111F
		internal KekRecipientInfoGenerator()
		{
		}

		// Token: 0x1700078E RID: 1934
		// (set) Token: 0x06003AB2 RID: 15026 RVA: 0x0016BC0F File Offset: 0x00169E0F
		internal KekIdentifier KekIdentifier
		{
			set
			{
				this.kekIdentifier = value;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (set) Token: 0x06003AB3 RID: 15027 RVA: 0x0016BC18 File Offset: 0x00169E18
		internal KeyParameter KeyEncryptionKey
		{
			set
			{
				this.keyEncryptionKey = value;
				this.keyEncryptionAlgorithm = KekRecipientInfoGenerator.DetermineKeyEncAlg(this.keyEncryptionKeyOID, this.keyEncryptionKey);
			}
		}

		// Token: 0x17000790 RID: 1936
		// (set) Token: 0x06003AB4 RID: 15028 RVA: 0x0016BC38 File Offset: 0x00169E38
		internal string KeyEncryptionKeyOID
		{
			set
			{
				this.keyEncryptionKeyOID = value;
			}
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x0016BC44 File Offset: 0x00169E44
		public RecipientInfo Generate(KeyParameter contentEncryptionKey, SecureRandom random)
		{
			byte[] key = contentEncryptionKey.GetKey();
			IWrapper wrapper = KekRecipientInfoGenerator.Helper.CreateWrapper(this.keyEncryptionAlgorithm.Algorithm.Id);
			wrapper.Init(true, new ParametersWithRandom(this.keyEncryptionKey, random));
			Asn1OctetString encryptedKey = new DerOctetString(wrapper.Wrap(key, 0, key.Length));
			return new RecipientInfo(new KekRecipientInfo(this.kekIdentifier, this.keyEncryptionAlgorithm, encryptedKey));
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x0016BCAC File Offset: 0x00169EAC
		private static AlgorithmIdentifier DetermineKeyEncAlg(string algorithm, KeyParameter key)
		{
			if (Platform.StartsWith(algorithm, "DES"))
			{
				return new AlgorithmIdentifier(PkcsObjectIdentifiers.IdAlgCms3DesWrap, DerNull.Instance);
			}
			if (Platform.StartsWith(algorithm, "RC2"))
			{
				return new AlgorithmIdentifier(PkcsObjectIdentifiers.IdAlgCmsRC2Wrap, new DerInteger(58));
			}
			if (Platform.StartsWith(algorithm, "AES"))
			{
				int num = key.GetKey().Length * 8;
				DerObjectIdentifier algorithm2;
				if (num == 128)
				{
					algorithm2 = NistObjectIdentifiers.IdAes128Wrap;
				}
				else if (num == 192)
				{
					algorithm2 = NistObjectIdentifiers.IdAes192Wrap;
				}
				else
				{
					if (num != 256)
					{
						throw new ArgumentException("illegal keysize in AES");
					}
					algorithm2 = NistObjectIdentifiers.IdAes256Wrap;
				}
				return new AlgorithmIdentifier(algorithm2);
			}
			if (Platform.StartsWith(algorithm, "SEED"))
			{
				return new AlgorithmIdentifier(KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap);
			}
			if (Platform.StartsWith(algorithm, "CAMELLIA"))
			{
				int num2 = key.GetKey().Length * 8;
				DerObjectIdentifier algorithm3;
				if (num2 == 128)
				{
					algorithm3 = NttObjectIdentifiers.IdCamellia128Wrap;
				}
				else if (num2 == 192)
				{
					algorithm3 = NttObjectIdentifiers.IdCamellia192Wrap;
				}
				else
				{
					if (num2 != 256)
					{
						throw new ArgumentException("illegal keysize in Camellia");
					}
					algorithm3 = NttObjectIdentifiers.IdCamellia256Wrap;
				}
				return new AlgorithmIdentifier(algorithm3);
			}
			throw new ArgumentException("unknown algorithm");
		}

		// Token: 0x04002658 RID: 9816
		private static readonly CmsEnvelopedHelper Helper = CmsEnvelopedHelper.Instance;

		// Token: 0x04002659 RID: 9817
		private KeyParameter keyEncryptionKey;

		// Token: 0x0400265A RID: 9818
		private string keyEncryptionKeyOID;

		// Token: 0x0400265B RID: 9819
		private KekIdentifier kekIdentifier;

		// Token: 0x0400265C RID: 9820
		private AlgorithmIdentifier keyEncryptionAlgorithm;
	}
}
