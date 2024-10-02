using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200055E RID: 1374
	public class Pkcs5S1ParametersGenerator : PbeParametersGenerator
	{
		// Token: 0x060033A5 RID: 13221 RVA: 0x00136690 File Offset: 0x00134890
		public Pkcs5S1ParametersGenerator(IDigest digest)
		{
			this.digest = digest;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x001366A0 File Offset: 0x001348A0
		private byte[] GenerateDerivedKey()
		{
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.BlockUpdate(this.mPassword, 0, this.mPassword.Length);
			this.digest.BlockUpdate(this.mSalt, 0, this.mSalt.Length);
			this.digest.DoFinal(array, 0);
			for (int i = 1; i < this.mIterationCount; i++)
			{
				this.digest.BlockUpdate(array, 0, array.Length);
				this.digest.DoFinal(array, 0);
			}
			return array;
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x0013621D File Offset: 0x0013441D
		public override ICipherParameters GenerateDerivedParameters(int keySize)
		{
			return this.GenerateDerivedMacParameters(keySize);
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x00136730 File Offset: 0x00134930
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize)
		{
			keySize /= 8;
			if (keySize > this.digest.GetDigestSize())
			{
				throw new ArgumentException("Can't Generate a derived key " + keySize + " bytes long.");
			}
			byte[] keyBytes = this.GenerateDerivedKey();
			return ParameterUtilities.CreateKeyParameter(algorithm, keyBytes, 0, keySize);
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x0013677C File Offset: 0x0013497C
		public override ICipherParameters GenerateDerivedParameters(int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			if (keySize + ivSize > this.digest.GetDigestSize())
			{
				throw new ArgumentException("Can't Generate a derived key " + (keySize + ivSize) + " bytes long.");
			}
			byte[] array = this.GenerateDerivedKey();
			return new ParametersWithIV(new KeyParameter(array, 0, keySize), array, keySize, ivSize);
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x001367D8 File Offset: 0x001349D8
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			if (keySize + ivSize > this.digest.GetDigestSize())
			{
				throw new ArgumentException("Can't Generate a derived key " + (keySize + ivSize) + " bytes long.");
			}
			byte[] array = this.GenerateDerivedKey();
			return new ParametersWithIV(ParameterUtilities.CreateKeyParameter(algorithm, array, 0, keySize), array, keySize, ivSize);
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x00136834 File Offset: 0x00134A34
		public override ICipherParameters GenerateDerivedMacParameters(int keySize)
		{
			keySize /= 8;
			if (keySize > this.digest.GetDigestSize())
			{
				throw new ArgumentException("Can't Generate a derived key " + keySize + " bytes long.");
			}
			return new KeyParameter(this.GenerateDerivedKey(), 0, keySize);
		}

		// Token: 0x040021C6 RID: 8646
		private readonly IDigest digest;
	}
}
