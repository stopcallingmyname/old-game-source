using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000573 RID: 1395
	public class DesEdeWrapEngine : IWrapper
	{
		// Token: 0x06003477 RID: 13431 RVA: 0x0013FA50 File Offset: 0x0013DC50
		public virtual void Init(bool forWrapping, ICipherParameters parameters)
		{
			this.forWrapping = forWrapping;
			this.engine = new CbcBlockCipher(new DesEdeEngine());
			SecureRandom secureRandom;
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				parameters = parametersWithRandom.Parameters;
				secureRandom = parametersWithRandom.Random;
			}
			else
			{
				secureRandom = new SecureRandom();
			}
			if (parameters is KeyParameter)
			{
				this.param = (KeyParameter)parameters;
				if (this.forWrapping)
				{
					this.iv = new byte[8];
					secureRandom.NextBytes(this.iv);
					this.paramPlusIV = new ParametersWithIV(this.param, this.iv);
					return;
				}
			}
			else if (parameters is ParametersWithIV)
			{
				if (!forWrapping)
				{
					throw new ArgumentException("You should not supply an IV for unwrapping");
				}
				this.paramPlusIV = (ParametersWithIV)parameters;
				this.iv = this.paramPlusIV.GetIV();
				this.param = (KeyParameter)this.paramPlusIV.Parameters;
				if (this.iv.Length != 8)
				{
					throw new ArgumentException("IV is not 8 octets", "parameters");
				}
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x0013F993 File Offset: 0x0013DB93
		public virtual string AlgorithmName
		{
			get
			{
				return "DESede";
			}
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x0013FB4C File Offset: 0x0013DD4C
		public virtual byte[] Wrap(byte[] input, int inOff, int length)
		{
			if (!this.forWrapping)
			{
				throw new InvalidOperationException("Not initialized for wrapping");
			}
			byte[] array = new byte[length];
			Array.Copy(input, inOff, array, 0, length);
			byte[] array2 = this.CalculateCmsKeyChecksum(array);
			byte[] array3 = new byte[array.Length + array2.Length];
			Array.Copy(array, 0, array3, 0, array.Length);
			Array.Copy(array2, 0, array3, array.Length, array2.Length);
			int blockSize = this.engine.GetBlockSize();
			if (array3.Length % blockSize != 0)
			{
				throw new InvalidOperationException("Not multiple of block length");
			}
			this.engine.Init(true, this.paramPlusIV);
			byte[] array4 = new byte[array3.Length];
			for (int num = 0; num != array3.Length; num += blockSize)
			{
				this.engine.ProcessBlock(array3, num, array4, num);
			}
			byte[] array5 = new byte[this.iv.Length + array4.Length];
			Array.Copy(this.iv, 0, array5, 0, this.iv.Length);
			Array.Copy(array4, 0, array5, this.iv.Length, array4.Length);
			byte[] array6 = DesEdeWrapEngine.reverse(array5);
			ParametersWithIV parameters = new ParametersWithIV(this.param, DesEdeWrapEngine.IV2);
			this.engine.Init(true, parameters);
			for (int num2 = 0; num2 != array6.Length; num2 += blockSize)
			{
				this.engine.ProcessBlock(array6, num2, array6, num2);
			}
			return array6;
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x0013FCA0 File Offset: 0x0013DEA0
		public virtual byte[] Unwrap(byte[] input, int inOff, int length)
		{
			if (this.forWrapping)
			{
				throw new InvalidOperationException("Not set for unwrapping");
			}
			if (input == null)
			{
				throw new InvalidCipherTextException("Null pointer as ciphertext");
			}
			int blockSize = this.engine.GetBlockSize();
			if (length % blockSize != 0)
			{
				throw new InvalidCipherTextException("Ciphertext not multiple of " + blockSize);
			}
			ParametersWithIV parameters = new ParametersWithIV(this.param, DesEdeWrapEngine.IV2);
			this.engine.Init(false, parameters);
			byte[] array = new byte[length];
			for (int num = 0; num != array.Length; num += blockSize)
			{
				this.engine.ProcessBlock(input, inOff + num, array, num);
			}
			byte[] array2 = DesEdeWrapEngine.reverse(array);
			this.iv = new byte[8];
			byte[] array3 = new byte[array2.Length - 8];
			Array.Copy(array2, 0, this.iv, 0, 8);
			Array.Copy(array2, 8, array3, 0, array2.Length - 8);
			this.paramPlusIV = new ParametersWithIV(this.param, this.iv);
			this.engine.Init(false, this.paramPlusIV);
			byte[] array4 = new byte[array3.Length];
			for (int num2 = 0; num2 != array4.Length; num2 += blockSize)
			{
				this.engine.ProcessBlock(array3, num2, array4, num2);
			}
			byte[] array5 = new byte[array4.Length - 8];
			byte[] array6 = new byte[8];
			Array.Copy(array4, 0, array5, 0, array4.Length - 8);
			Array.Copy(array4, array4.Length - 8, array6, 0, 8);
			if (!this.CheckCmsKeyChecksum(array5, array6))
			{
				throw new InvalidCipherTextException("Checksum inside ciphertext is corrupted");
			}
			return array5;
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x0013FE28 File Offset: 0x0013E028
		private byte[] CalculateCmsKeyChecksum(byte[] key)
		{
			this.sha1.BlockUpdate(key, 0, key.Length);
			this.sha1.DoFinal(this.digest, 0);
			byte[] array = new byte[8];
			Array.Copy(this.digest, 0, array, 0, 8);
			return array;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x0013FE6F File Offset: 0x0013E06F
		private bool CheckCmsKeyChecksum(byte[] key, byte[] checksum)
		{
			return Arrays.ConstantTimeAreEqual(this.CalculateCmsKeyChecksum(key), checksum);
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x0013FE80 File Offset: 0x0013E080
		private static byte[] reverse(byte[] bs)
		{
			byte[] array = new byte[bs.Length];
			for (int i = 0; i < bs.Length; i++)
			{
				array[i] = bs[bs.Length - (i + 1)];
			}
			return array;
		}

		// Token: 0x04002251 RID: 8785
		private CbcBlockCipher engine;

		// Token: 0x04002252 RID: 8786
		private KeyParameter param;

		// Token: 0x04002253 RID: 8787
		private ParametersWithIV paramPlusIV;

		// Token: 0x04002254 RID: 8788
		private byte[] iv;

		// Token: 0x04002255 RID: 8789
		private bool forWrapping;

		// Token: 0x04002256 RID: 8790
		private static readonly byte[] IV2 = new byte[]
		{
			74,
			221,
			162,
			44,
			121,
			232,
			33,
			5
		};

		// Token: 0x04002257 RID: 8791
		private readonly IDigest sha1 = new Sha1Digest();

		// Token: 0x04002258 RID: 8792
		private readonly byte[] digest = new byte[20];
	}
}
