using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200055D RID: 1373
	public class Pkcs12ParametersGenerator : PbeParametersGenerator
	{
		// Token: 0x0600339D RID: 13213 RVA: 0x001362C7 File Offset: 0x001344C7
		public Pkcs12ParametersGenerator(IDigest digest)
		{
			this.digest = digest;
			this.u = digest.GetDigestSize();
			this.v = digest.GetByteLength();
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x001362F0 File Offset: 0x001344F0
		private void Adjust(byte[] a, int aOff, byte[] b)
		{
			int num = (int)((b[b.Length - 1] & byte.MaxValue) + (a[aOff + b.Length - 1] & byte.MaxValue) + 1);
			a[aOff + b.Length - 1] = (byte)num;
			num = (int)((uint)num >> 8);
			for (int i = b.Length - 2; i >= 0; i--)
			{
				num += (int)((b[i] & byte.MaxValue) + (a[aOff + i] & byte.MaxValue));
				a[aOff + i] = (byte)num;
				num = (int)((uint)num >> 8);
			}
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x00136360 File Offset: 0x00134560
		private byte[] GenerateDerivedKey(int idByte, int n)
		{
			byte[] array = new byte[this.v];
			byte[] array2 = new byte[n];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = (byte)idByte;
			}
			byte[] array3;
			if (this.mSalt != null && this.mSalt.Length != 0)
			{
				array3 = new byte[this.v * ((this.mSalt.Length + this.v - 1) / this.v)];
				for (int num2 = 0; num2 != array3.Length; num2++)
				{
					array3[num2] = this.mSalt[num2 % this.mSalt.Length];
				}
			}
			else
			{
				array3 = new byte[0];
			}
			byte[] array4;
			if (this.mPassword != null && this.mPassword.Length != 0)
			{
				array4 = new byte[this.v * ((this.mPassword.Length + this.v - 1) / this.v)];
				for (int num3 = 0; num3 != array4.Length; num3++)
				{
					array4[num3] = this.mPassword[num3 % this.mPassword.Length];
				}
			}
			else
			{
				array4 = new byte[0];
			}
			byte[] array5 = new byte[array3.Length + array4.Length];
			Array.Copy(array3, 0, array5, 0, array3.Length);
			Array.Copy(array4, 0, array5, array3.Length, array4.Length);
			byte[] array6 = new byte[this.v];
			int num4 = (n + this.u - 1) / this.u;
			byte[] array7 = new byte[this.u];
			for (int i = 1; i <= num4; i++)
			{
				this.digest.BlockUpdate(array, 0, array.Length);
				this.digest.BlockUpdate(array5, 0, array5.Length);
				this.digest.DoFinal(array7, 0);
				for (int num5 = 1; num5 != this.mIterationCount; num5++)
				{
					this.digest.BlockUpdate(array7, 0, array7.Length);
					this.digest.DoFinal(array7, 0);
				}
				for (int num6 = 0; num6 != array6.Length; num6++)
				{
					array6[num6] = array7[num6 % array7.Length];
				}
				for (int num7 = 0; num7 != array5.Length / this.v; num7++)
				{
					this.Adjust(array5, num7 * this.v, array6);
				}
				if (i == num4)
				{
					Array.Copy(array7, 0, array2, (i - 1) * this.u, array2.Length - (i - 1) * this.u);
				}
				else
				{
					Array.Copy(array7, 0, array2, (i - 1) * this.u, array7.Length);
				}
			}
			return array2;
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x001365CE File Offset: 0x001347CE
		public override ICipherParameters GenerateDerivedParameters(int keySize)
		{
			keySize /= 8;
			return new KeyParameter(this.GenerateDerivedKey(1, keySize), 0, keySize);
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x001365E4 File Offset: 0x001347E4
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize)
		{
			keySize /= 8;
			byte[] keyBytes = this.GenerateDerivedKey(1, keySize);
			return ParameterUtilities.CreateKeyParameter(algorithm, keyBytes, 0, keySize);
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x00136608 File Offset: 0x00134808
		public override ICipherParameters GenerateDerivedParameters(int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			byte[] key = this.GenerateDerivedKey(1, keySize);
			byte[] iv = this.GenerateDerivedKey(2, ivSize);
			return new ParametersWithIV(new KeyParameter(key, 0, keySize), iv, 0, ivSize);
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x00136640 File Offset: 0x00134840
		public override ICipherParameters GenerateDerivedParameters(string algorithm, int keySize, int ivSize)
		{
			keySize /= 8;
			ivSize /= 8;
			byte[] keyBytes = this.GenerateDerivedKey(1, keySize);
			ICipherParameters parameters = ParameterUtilities.CreateKeyParameter(algorithm, keyBytes, 0, keySize);
			byte[] iv = this.GenerateDerivedKey(2, ivSize);
			return new ParametersWithIV(parameters, iv, 0, ivSize);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x0013667A File Offset: 0x0013487A
		public override ICipherParameters GenerateDerivedMacParameters(int keySize)
		{
			keySize /= 8;
			return new KeyParameter(this.GenerateDerivedKey(3, keySize), 0, keySize);
		}

		// Token: 0x040021C0 RID: 8640
		public const int KeyMaterial = 1;

		// Token: 0x040021C1 RID: 8641
		public const int IVMaterial = 2;

		// Token: 0x040021C2 RID: 8642
		public const int MacMaterial = 3;

		// Token: 0x040021C3 RID: 8643
		private readonly IDigest digest;

		// Token: 0x040021C4 RID: 8644
		private readonly int u;

		// Token: 0x040021C5 RID: 8645
		private readonly int v;
	}
}
