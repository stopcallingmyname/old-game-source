using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl
{
	// Token: 0x020002D6 RID: 726
	public class Pkcs8Generator : PemObjectGenerator
	{
		// Token: 0x06001A98 RID: 6808 RVA: 0x000C7B86 File Offset: 0x000C5D86
		public Pkcs8Generator(AsymmetricKeyParameter privKey)
		{
			this.privKey = privKey;
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000C7B95 File Offset: 0x000C5D95
		public Pkcs8Generator(AsymmetricKeyParameter privKey, string algorithm)
		{
			this.privKey = privKey;
			this.algorithm = algorithm;
			this.iterationCount = 2048;
		}

		// Token: 0x1700035F RID: 863
		// (set) Token: 0x06001A9A RID: 6810 RVA: 0x000C7BB6 File Offset: 0x000C5DB6
		public SecureRandom SecureRandom
		{
			set
			{
				this.random = value;
			}
		}

		// Token: 0x17000360 RID: 864
		// (set) Token: 0x06001A9B RID: 6811 RVA: 0x000C7BBF File Offset: 0x000C5DBF
		public char[] Password
		{
			set
			{
				this.password = value;
			}
		}

		// Token: 0x17000361 RID: 865
		// (set) Token: 0x06001A9C RID: 6812 RVA: 0x000C7BC8 File Offset: 0x000C5DC8
		public int IterationCount
		{
			set
			{
				this.iterationCount = value;
			}
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000C7BD4 File Offset: 0x000C5DD4
		public PemObject Generate()
		{
			if (this.algorithm == null)
			{
				PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(this.privKey);
				return new PemObject("PRIVATE KEY", privateKeyInfo.GetEncoded());
			}
			byte[] array = new byte[20];
			if (this.random == null)
			{
				this.random = new SecureRandom();
			}
			this.random.NextBytes(array);
			PemObject result;
			try
			{
				EncryptedPrivateKeyInfo encryptedPrivateKeyInfo = EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(this.algorithm, this.password, array, this.iterationCount, this.privKey);
				result = new PemObject("ENCRYPTED PRIVATE KEY", encryptedPrivateKeyInfo.GetEncoded());
			}
			catch (Exception exception)
			{
				throw new PemGenerationException("Couldn't encrypt private key", exception);
			}
			return result;
		}

		// Token: 0x040018D1 RID: 6353
		public static readonly string PbeSha1_RC4_128 = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4.Id;

		// Token: 0x040018D2 RID: 6354
		public static readonly string PbeSha1_RC4_40 = PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4.Id;

		// Token: 0x040018D3 RID: 6355
		public static readonly string PbeSha1_3DES = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id;

		// Token: 0x040018D4 RID: 6356
		public static readonly string PbeSha1_2DES = PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc.Id;

		// Token: 0x040018D5 RID: 6357
		public static readonly string PbeSha1_RC2_128 = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc.Id;

		// Token: 0x040018D6 RID: 6358
		public static readonly string PbeSha1_RC2_40 = PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc.Id;

		// Token: 0x040018D7 RID: 6359
		private char[] password;

		// Token: 0x040018D8 RID: 6360
		private string algorithm;

		// Token: 0x040018D9 RID: 6361
		private int iterationCount;

		// Token: 0x040018DA RID: 6362
		private AsymmetricKeyParameter privKey;

		// Token: 0x040018DB RID: 6363
		private SecureRandom random;
	}
}
