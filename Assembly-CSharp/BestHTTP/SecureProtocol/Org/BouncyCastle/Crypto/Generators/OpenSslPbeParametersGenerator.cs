using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200055C RID: 1372
	public class OpenSslPbeParametersGenerator : PbeParametersGenerator
	{
		// Token: 0x06003395 RID: 13205 RVA: 0x00136163 File Offset: 0x00134363
		public override void Init(byte[] password, byte[] salt, int iterationCount)
		{
			base.Init(password, salt, 1);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x00136163 File Offset: 0x00134363
		public virtual void Init(byte[] password, byte[] salt)
		{
			base.Init(password, salt, 1);
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x00136170 File Offset: 0x00134370
		private byte[] GenerateDerivedKey(int bytesNeeded)
		{
			byte[] array = new byte[this.digest.GetDigestSize()];
			byte[] array2 = new byte[bytesNeeded];
			int num = 0;
			for (;;)
			{
				this.digest.BlockUpdate(this.mPassword, 0, this.mPassword.Length);
				this.digest.BlockUpdate(this.mSalt, 0, this.mSalt.Length);
				this.digest.DoFinal(array, 0);
				int num2 = (bytesNeeded > array.Length) ? array.Length : bytesNeeded;
				Array.Copy(array, 0, array2, num, num2);
				num += num2;
				bytesNeeded -= num2;
				if (bytesNeeded == 0)
				{
					break;
				}
				this.digest.Reset();
				this.digest.BlockUpdate(array, 0, array.Length);
			}
			return array2;
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x0013621D File Offset: 0x0013441D
		[Obsolete("Use version with 'algorithm' parameter")]
		public override ICipherParameters GenerateDerivedParameters(int keySize)
		{
			return this.GenerateDerivedMacParameters(keySize);
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x00136228 File Offset: 0x00134428
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize)
		{
			keySize /= 8;
			byte[] keyBytes = this.GenerateDerivedKey(keySize);
			return ParameterUtilities.CreateKeyParameter(algorithm, keyBytes, 0, keySize);
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x0013624C File Offset: 0x0013444C
		[Obsolete("Use version with 'algorithm' parameter")]
		public override ICipherParameters GenerateDerivedParameters(int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			byte[] array = this.GenerateDerivedKey(keySize + ivSize);
			return new ParametersWithIV(new KeyParameter(array, 0, keySize), array, keySize, ivSize);
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x00136280 File Offset: 0x00134480
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			byte[] array = this.GenerateDerivedKey(keySize + ivSize);
			return new ParametersWithIV(ParameterUtilities.CreateKeyParameter(algorithm, array, 0, keySize), array, keySize, ivSize);
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x001362B2 File Offset: 0x001344B2
		public override ICipherParameters GenerateDerivedMacParameters(int keySize)
		{
			keySize /= 8;
			return new KeyParameter(this.GenerateDerivedKey(keySize), 0, keySize);
		}

		// Token: 0x040021BF RID: 8639
		private readonly IDigest digest = new MD5Digest();
	}
}
