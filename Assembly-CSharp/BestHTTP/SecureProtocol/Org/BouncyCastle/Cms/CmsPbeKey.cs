using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F5 RID: 1525
	public abstract class CmsPbeKey : ICipherParameters
	{
		// Token: 0x060039EC RID: 14828 RVA: 0x001689A9 File Offset: 0x00166BA9
		[Obsolete("Use version taking 'char[]' instead")]
		public CmsPbeKey(string password, byte[] salt, int iterationCount) : this(password.ToCharArray(), salt, iterationCount)
		{
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x001689B9 File Offset: 0x00166BB9
		[Obsolete("Use version taking 'char[]' instead")]
		public CmsPbeKey(string password, AlgorithmIdentifier keyDerivationAlgorithm) : this(password.ToCharArray(), keyDerivationAlgorithm)
		{
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001689C8 File Offset: 0x00166BC8
		public CmsPbeKey(char[] password, byte[] salt, int iterationCount)
		{
			this.password = (char[])password.Clone();
			this.salt = Arrays.Clone(salt);
			this.iterationCount = iterationCount;
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x001689F4 File Offset: 0x00166BF4
		public CmsPbeKey(char[] password, AlgorithmIdentifier keyDerivationAlgorithm)
		{
			if (!keyDerivationAlgorithm.Algorithm.Equals(PkcsObjectIdentifiers.IdPbkdf2))
			{
				throw new ArgumentException("Unsupported key derivation algorithm: " + keyDerivationAlgorithm.Algorithm);
			}
			Pbkdf2Params instance = Pbkdf2Params.GetInstance(keyDerivationAlgorithm.Parameters.ToAsn1Object());
			this.password = (char[])password.Clone();
			this.salt = instance.GetSalt();
			this.iterationCount = instance.IterationCount.IntValue;
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x00168A70 File Offset: 0x00166C70
		~CmsPbeKey()
		{
			Array.Clear(this.password, 0, this.password.Length);
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060039F1 RID: 14833 RVA: 0x00168AAC File Offset: 0x00166CAC
		[Obsolete("Will be removed")]
		public string Password
		{
			get
			{
				return new string(this.password);
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x00168AB9 File Offset: 0x00166CB9
		public byte[] Salt
		{
			get
			{
				return Arrays.Clone(this.salt);
			}
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x00168AC6 File Offset: 0x00166CC6
		[Obsolete("Use 'Salt' property instead")]
		public byte[] GetSalt()
		{
			return this.Salt;
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x00168ACE File Offset: 0x00166CCE
		public int IterationCount
		{
			get
			{
				return this.iterationCount;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060039F5 RID: 14837 RVA: 0x00168AD6 File Offset: 0x00166CD6
		public string Algorithm
		{
			get
			{
				return "PKCS5S2";
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x00168ADD File Offset: 0x00166CDD
		public string Format
		{
			get
			{
				return "RAW";
			}
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x0008D54A File Offset: 0x0008B74A
		public byte[] GetEncoded()
		{
			return null;
		}

		// Token: 0x060039F8 RID: 14840
		internal abstract KeyParameter GetEncoded(string algorithmOid);

		// Token: 0x04002604 RID: 9732
		internal readonly char[] password;

		// Token: 0x04002605 RID: 9733
		internal readonly byte[] salt;

		// Token: 0x04002606 RID: 9734
		internal readonly int iterationCount;
	}
}
