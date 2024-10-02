using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003ED RID: 1005
	public abstract class PbeParametersGenerator
	{
		// Token: 0x06002889 RID: 10377 RVA: 0x0010CB4D File Offset: 0x0010AD4D
		public virtual void Init(byte[] password, byte[] salt, int iterationCount)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			if (salt == null)
			{
				throw new ArgumentNullException("salt");
			}
			this.mPassword = Arrays.Clone(password);
			this.mSalt = Arrays.Clone(salt);
			this.mIterationCount = iterationCount;
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x0010CB8A File Offset: 0x0010AD8A
		public virtual byte[] Password
		{
			get
			{
				return Arrays.Clone(this.mPassword);
			}
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x0010CB97 File Offset: 0x0010AD97
		[Obsolete("Use 'Password' property")]
		public byte[] GetPassword()
		{
			return this.Password;
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x0010CB9F File Offset: 0x0010AD9F
		public virtual byte[] Salt
		{
			get
			{
				return Arrays.Clone(this.mSalt);
			}
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x0010CBAC File Offset: 0x0010ADAC
		[Obsolete("Use 'Salt' property")]
		public byte[] GetSalt()
		{
			return this.Salt;
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x0010CBB4 File Offset: 0x0010ADB4
		public virtual int IterationCount
		{
			get
			{
				return this.mIterationCount;
			}
		}

		// Token: 0x0600288F RID: 10383
		[Obsolete("Use version with 'algorithm' parameter")]
		public abstract ICipherParameters GenerateDerivedParameters(int keySize);

		// Token: 0x06002890 RID: 10384
		public abstract ICipherParameters GenerateDerivedParameters(string algorithm, int keySize);

		// Token: 0x06002891 RID: 10385
		[Obsolete("Use version with 'algorithm' parameter")]
		public abstract ICipherParameters GenerateDerivedParameters(int keySize, int ivSize);

		// Token: 0x06002892 RID: 10386
		public abstract ICipherParameters GenerateDerivedParameters(string algorithm, int keySize, int ivSize);

		// Token: 0x06002893 RID: 10387
		public abstract ICipherParameters GenerateDerivedMacParameters(int keySize);

		// Token: 0x06002894 RID: 10388 RVA: 0x0010CBBC File Offset: 0x0010ADBC
		public static byte[] Pkcs5PasswordToBytes(char[] password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Strings.ToByteArray(password);
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x0010CBCE File Offset: 0x0010ADCE
		[Obsolete("Use version taking 'char[]' instead")]
		public static byte[] Pkcs5PasswordToBytes(string password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Strings.ToByteArray(password);
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x0010CBE0 File Offset: 0x0010ADE0
		public static byte[] Pkcs5PasswordToUtf8Bytes(char[] password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Encoding.UTF8.GetBytes(password);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x0010CBF7 File Offset: 0x0010ADF7
		[Obsolete("Use version taking 'char[]' instead")]
		public static byte[] Pkcs5PasswordToUtf8Bytes(string password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Encoding.UTF8.GetBytes(password);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x0010CC0E File Offset: 0x0010AE0E
		public static byte[] Pkcs12PasswordToBytes(char[] password)
		{
			return PbeParametersGenerator.Pkcs12PasswordToBytes(password, false);
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x0010CC18 File Offset: 0x0010AE18
		public static byte[] Pkcs12PasswordToBytes(char[] password, bool wrongPkcs12Zero)
		{
			if (password == null || password.Length < 1)
			{
				return new byte[wrongPkcs12Zero ? 2 : 0];
			}
			byte[] array = new byte[(password.Length + 1) * 2];
			Encoding.BigEndianUnicode.GetBytes(password, 0, password.Length, array, 0);
			return array;
		}

		// Token: 0x04001B08 RID: 6920
		protected byte[] mPassword;

		// Token: 0x04001B09 RID: 6921
		protected byte[] mSalt;

		// Token: 0x04001B0A RID: 6922
		protected int mIterationCount;
	}
}
