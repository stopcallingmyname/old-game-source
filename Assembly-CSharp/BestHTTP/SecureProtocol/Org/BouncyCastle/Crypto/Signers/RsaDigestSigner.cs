using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A8 RID: 1192
	public class RsaDigestSigner : ISigner
	{
		// Token: 0x06002EB9 RID: 11961 RVA: 0x00122748 File Offset: 0x00120948
		static RsaDigestSigner()
		{
			RsaDigestSigner.oidMap["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
			RsaDigestSigner.oidMap["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
			RsaDigestSigner.oidMap["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
			RsaDigestSigner.oidMap["SHA-1"] = X509ObjectIdentifiers.IdSha1;
			RsaDigestSigner.oidMap["SHA-224"] = NistObjectIdentifiers.IdSha224;
			RsaDigestSigner.oidMap["SHA-256"] = NistObjectIdentifiers.IdSha256;
			RsaDigestSigner.oidMap["SHA-384"] = NistObjectIdentifiers.IdSha384;
			RsaDigestSigner.oidMap["SHA-512"] = NistObjectIdentifiers.IdSha512;
			RsaDigestSigner.oidMap["MD2"] = PkcsObjectIdentifiers.MD2;
			RsaDigestSigner.oidMap["MD4"] = PkcsObjectIdentifiers.MD4;
			RsaDigestSigner.oidMap["MD5"] = PkcsObjectIdentifiers.MD5;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x0012283B File Offset: 0x00120A3B
		public RsaDigestSigner(IDigest digest) : this(digest, (DerObjectIdentifier)RsaDigestSigner.oidMap[digest.AlgorithmName])
		{
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x00122859 File Offset: 0x00120A59
		public RsaDigestSigner(IDigest digest, DerObjectIdentifier digestOid) : this(digest, new AlgorithmIdentifier(digestOid, DerNull.Instance))
		{
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x0012286D File Offset: 0x00120A6D
		public RsaDigestSigner(IDigest digest, AlgorithmIdentifier algId) : this(new RsaCoreEngine(), digest, algId)
		{
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x0012287C File Offset: 0x00120A7C
		public RsaDigestSigner(IRsa rsa, IDigest digest, DerObjectIdentifier digestOid) : this(rsa, digest, new AlgorithmIdentifier(digestOid, DerNull.Instance))
		{
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x00122891 File Offset: 0x00120A91
		public RsaDigestSigner(IRsa rsa, IDigest digest, AlgorithmIdentifier algId) : this(new RsaBlindedEngine(rsa), digest, algId)
		{
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x001228A1 File Offset: 0x00120AA1
		public RsaDigestSigner(IAsymmetricBlockCipher rsaEngine, IDigest digest, AlgorithmIdentifier algId)
		{
			this.rsaEngine = new Pkcs1Encoding(rsaEngine);
			this.digest = digest;
			this.algId = algId;
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x001228C3 File Offset: 0x00120AC3
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "withRSA";
			}
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x001228DC File Offset: 0x00120ADC
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			AsymmetricKeyParameter asymmetricKeyParameter;
			if (parameters is ParametersWithRandom)
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters;
			}
			else
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
			}
			if (forSigning && !asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Signing requires private key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification requires public key.");
			}
			this.Reset();
			this.rsaEngine.Init(forSigning, parameters);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x00122951 File Offset: 0x00120B51
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x0012295F File Offset: 0x00120B5F
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.digest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00122970 File Offset: 0x00120B70
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("RsaDigestSigner not initialised for signature generation.");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			byte[] array2 = this.DerEncode(array);
			return this.rsaEngine.ProcessBlock(array2, 0, array2.Length);
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x001229C8 File Offset: 0x00120BC8
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("RsaDigestSigner not initialised for verification");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			byte[] array2;
			byte[] array3;
			try
			{
				array2 = this.rsaEngine.ProcessBlock(signature, 0, signature.Length);
				array3 = this.DerEncode(array);
			}
			catch (Exception)
			{
				return false;
			}
			if (array2.Length == array3.Length)
			{
				return Arrays.ConstantTimeAreEqual(array2, array3);
			}
			if (array2.Length == array3.Length - 2)
			{
				int num = array2.Length - array.Length - 2;
				int num2 = array3.Length - array.Length - 2;
				byte[] array4 = array3;
				int num3 = 1;
				array4[num3] -= 2;
				byte[] array5 = array3;
				int num4 = 3;
				array5[num4] -= 2;
				int num5 = 0;
				for (int i = 0; i < array.Length; i++)
				{
					num5 |= (int)(array2[num + i] ^ array3[num2 + i]);
				}
				for (int j = 0; j < num; j++)
				{
					num5 |= (int)(array2[j] ^ array3[j]);
				}
				return num5 == 0;
			}
			return false;
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x00122AD8 File Offset: 0x00120CD8
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x00122AE5 File Offset: 0x00120CE5
		private byte[] DerEncode(byte[] hash)
		{
			if (this.algId == null)
			{
				return hash;
			}
			return new DigestInfo(this.algId, hash).GetDerEncoded();
		}

		// Token: 0x04001F3E RID: 7998
		private readonly IAsymmetricBlockCipher rsaEngine;

		// Token: 0x04001F3F RID: 7999
		private readonly AlgorithmIdentifier algId;

		// Token: 0x04001F40 RID: 8000
		private readonly IDigest digest;

		// Token: 0x04001F41 RID: 8001
		private bool forSigning;

		// Token: 0x04001F42 RID: 8002
		private static readonly IDictionary oidMap = Platform.CreateHashtable();
	}
}
