using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200055F RID: 1375
	public class Pkcs5S2ParametersGenerator : PbeParametersGenerator
	{
		// Token: 0x060033AC RID: 13228 RVA: 0x00136871 File Offset: 0x00134A71
		public Pkcs5S2ParametersGenerator() : this(new Sha1Digest())
		{
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x0013687E File Offset: 0x00134A7E
		public Pkcs5S2ParametersGenerator(IDigest digest)
		{
			this.hMac = new HMac(digest);
			this.state = new byte[this.hMac.GetMacSize()];
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x001368A8 File Offset: 0x00134AA8
		private void F(byte[] S, int c, byte[] iBuf, byte[] outBytes, int outOff)
		{
			if (c == 0)
			{
				throw new ArgumentException("iteration count must be at least 1.");
			}
			if (S != null)
			{
				this.hMac.BlockUpdate(S, 0, S.Length);
			}
			this.hMac.BlockUpdate(iBuf, 0, iBuf.Length);
			this.hMac.DoFinal(this.state, 0);
			Array.Copy(this.state, 0, outBytes, outOff, this.state.Length);
			for (int i = 1; i < c; i++)
			{
				this.hMac.BlockUpdate(this.state, 0, this.state.Length);
				this.hMac.DoFinal(this.state, 0);
				for (int j = 0; j < this.state.Length; j++)
				{
					int num = outOff + j;
					outBytes[num] ^= this.state[j];
				}
			}
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x00136978 File Offset: 0x00134B78
		private byte[] GenerateDerivedKey(int dkLen)
		{
			int macSize = this.hMac.GetMacSize();
			int num = (dkLen + macSize - 1) / macSize;
			byte[] array = new byte[4];
			byte[] array2 = new byte[num * macSize];
			int num2 = 0;
			ICipherParameters parameters = new KeyParameter(this.mPassword);
			this.hMac.Init(parameters);
			for (int i = 1; i <= num; i++)
			{
				int num3 = 3;
				for (;;)
				{
					byte[] array3 = array;
					int num4 = num3;
					byte b = array3[num4] + 1;
					array3[num4] = b;
					if (b != 0)
					{
						break;
					}
					num3--;
				}
				this.F(this.mSalt, this.mIterationCount, array, array2, num2);
				num2 += macSize;
			}
			return array2;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x0013621D File Offset: 0x0013441D
		public override ICipherParameters GenerateDerivedParameters(int keySize)
		{
			return this.GenerateDerivedMacParameters(keySize);
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x00136A14 File Offset: 0x00134C14
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize)
		{
			keySize /= 8;
			byte[] keyBytes = this.GenerateDerivedKey(keySize);
			return ParameterUtilities.CreateKeyParameter(algorithm, keyBytes, 0, keySize);
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x00136A38 File Offset: 0x00134C38
		public override ICipherParameters GenerateDerivedParameters(int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			byte[] array = this.GenerateDerivedKey(keySize + ivSize);
			return new ParametersWithIV(new KeyParameter(array, 0, keySize), array, keySize, ivSize);
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x00136A6C File Offset: 0x00134C6C
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			byte[] array = this.GenerateDerivedKey(keySize + ivSize);
			return new ParametersWithIV(ParameterUtilities.CreateKeyParameter(algorithm, array, 0, keySize), array, keySize, ivSize);
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x00136A9E File Offset: 0x00134C9E
		public override ICipherParameters GenerateDerivedMacParameters(int keySize)
		{
			keySize /= 8;
			return new KeyParameter(this.GenerateDerivedKey(keySize), 0, keySize);
		}

		// Token: 0x040021C7 RID: 8647
		private readonly IMac hMac;

		// Token: 0x040021C8 RID: 8648
		private readonly byte[] state;
	}
}
