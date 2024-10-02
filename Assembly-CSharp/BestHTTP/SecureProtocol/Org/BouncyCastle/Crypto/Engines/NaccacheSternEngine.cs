using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200057E RID: 1406
	public class NaccacheSternEngine : IAsymmetricBlockCipher
	{
		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06003504 RID: 13572 RVA: 0x0014442B File Offset: 0x0014262B
		public string AlgorithmName
		{
			get
			{
				return "NaccacheStern";
			}
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x00144434 File Offset: 0x00142634
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			this.key = (NaccacheSternKeyParameters)parameters;
			if (!this.forEncryption)
			{
				NaccacheSternPrivateKeyParameters naccacheSternPrivateKeyParameters = (NaccacheSternPrivateKeyParameters)this.key;
				IList smallPrimesList = naccacheSternPrivateKeyParameters.SmallPrimesList;
				this.lookup = new IList[smallPrimesList.Count];
				for (int i = 0; i < smallPrimesList.Count; i++)
				{
					BigInteger bigInteger = (BigInteger)smallPrimesList[i];
					int intValue = bigInteger.IntValue;
					this.lookup[i] = Platform.CreateArrayList(intValue);
					this.lookup[i].Add(BigInteger.One);
					BigInteger bigInteger2 = BigInteger.Zero;
					for (int j = 1; j < intValue; j++)
					{
						bigInteger2 = bigInteger2.Add(naccacheSternPrivateKeyParameters.PhiN);
						BigInteger e = bigInteger2.Divide(bigInteger);
						this.lookup[i].Add(naccacheSternPrivateKeyParameters.G.ModPow(e, naccacheSternPrivateKeyParameters.Modulus));
					}
				}
			}
		}

		// Token: 0x17000702 RID: 1794
		// (set) Token: 0x06003506 RID: 13574 RVA: 0x0000248C File Offset: 0x0000068C
		[Obsolete("Remove: no longer used")]
		public virtual bool Debug
		{
			set
			{
			}
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x00144538 File Offset: 0x00142738
		public virtual int GetInputBlockSize()
		{
			if (this.forEncryption)
			{
				return (this.key.LowerSigmaBound + 7) / 8 - 1;
			}
			return this.key.Modulus.BitLength / 8 + 1;
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x00144568 File Offset: 0x00142768
		public virtual int GetOutputBlockSize()
		{
			if (this.forEncryption)
			{
				return this.key.Modulus.BitLength / 8 + 1;
			}
			return (this.key.LowerSigmaBound + 7) / 8 - 1;
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x00144598 File Offset: 0x00142798
		public virtual byte[] ProcessBlock(byte[] inBytes, int inOff, int length)
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("NaccacheStern engine not initialised");
			}
			if (length > this.GetInputBlockSize() + 1)
			{
				throw new DataLengthException("input too large for Naccache-Stern cipher.\n");
			}
			if (!this.forEncryption && length < this.GetInputBlockSize())
			{
				throw new InvalidCipherTextException("BlockLength does not match modulus for Naccache-Stern cipher.\n");
			}
			BigInteger bigInteger = new BigInteger(1, inBytes, inOff, length);
			byte[] result;
			if (this.forEncryption)
			{
				result = this.Encrypt(bigInteger);
			}
			else
			{
				IList list = Platform.CreateArrayList();
				NaccacheSternPrivateKeyParameters naccacheSternPrivateKeyParameters = (NaccacheSternPrivateKeyParameters)this.key;
				IList smallPrimesList = naccacheSternPrivateKeyParameters.SmallPrimesList;
				for (int i = 0; i < smallPrimesList.Count; i++)
				{
					BigInteger value = bigInteger.ModPow(naccacheSternPrivateKeyParameters.PhiN.Divide((BigInteger)smallPrimesList[i]), naccacheSternPrivateKeyParameters.Modulus);
					IList list2 = this.lookup[i];
					if (this.lookup[i].Count != ((BigInteger)smallPrimesList[i]).IntValue)
					{
						throw new InvalidCipherTextException(string.Concat(new object[]
						{
							"Error in lookup Array for ",
							((BigInteger)smallPrimesList[i]).IntValue,
							": Size mismatch. Expected ArrayList with length ",
							((BigInteger)smallPrimesList[i]).IntValue,
							" but found ArrayList of length ",
							this.lookup[i].Count
						}));
					}
					int num = list2.IndexOf(value);
					if (num == -1)
					{
						throw new InvalidCipherTextException("Lookup failed");
					}
					list.Add(BigInteger.ValueOf((long)num));
				}
				result = NaccacheSternEngine.chineseRemainder(list, smallPrimesList).ToByteArray();
			}
			return result;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x00144744 File Offset: 0x00142944
		public virtual byte[] Encrypt(BigInteger plain)
		{
			byte[] array = new byte[this.key.Modulus.BitLength / 8 + 1];
			byte[] array2 = this.key.G.ModPow(plain, this.key.Modulus).ToByteArray();
			Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
			return array;
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x001447A0 File Offset: 0x001429A0
		public virtual byte[] AddCryptedBlocks(byte[] block1, byte[] block2)
		{
			if (this.forEncryption)
			{
				if (block1.Length > this.GetOutputBlockSize() || block2.Length > this.GetOutputBlockSize())
				{
					throw new InvalidCipherTextException("BlockLength too large for simple addition.\n");
				}
			}
			else if (block1.Length > this.GetInputBlockSize() || block2.Length > this.GetInputBlockSize())
			{
				throw new InvalidCipherTextException("BlockLength too large for simple addition.\n");
			}
			BigInteger bigInteger = new BigInteger(1, block1);
			BigInteger val = new BigInteger(1, block2);
			BigInteger bigInteger2 = bigInteger.Multiply(val).Mod(this.key.Modulus);
			byte[] array = new byte[this.key.Modulus.BitLength / 8 + 1];
			byte[] array2 = bigInteger2.ToByteArray();
			Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
			return array;
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x00144850 File Offset: 0x00142A50
		public virtual byte[] ProcessData(byte[] data)
		{
			if (data.Length > this.GetInputBlockSize())
			{
				int inputBlockSize = this.GetInputBlockSize();
				int outputBlockSize = this.GetOutputBlockSize();
				int i = 0;
				int num = 0;
				byte[] array = new byte[(data.Length / inputBlockSize + 1) * outputBlockSize];
				while (i < data.Length)
				{
					byte[] array2;
					if (i + inputBlockSize < data.Length)
					{
						array2 = this.ProcessBlock(data, i, inputBlockSize);
						i += inputBlockSize;
					}
					else
					{
						array2 = this.ProcessBlock(data, i, data.Length - i);
						i += data.Length - i;
					}
					if (array2 == null)
					{
						throw new InvalidCipherTextException("cipher returned null");
					}
					array2.CopyTo(array, num);
					num += array2.Length;
				}
				byte[] array3 = new byte[num];
				Array.Copy(array, 0, array3, 0, num);
				return array3;
			}
			return this.ProcessBlock(data, 0, data.Length);
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x0014490C File Offset: 0x00142B0C
		private static BigInteger chineseRemainder(IList congruences, IList primes)
		{
			BigInteger bigInteger = BigInteger.Zero;
			BigInteger bigInteger2 = BigInteger.One;
			for (int i = 0; i < primes.Count; i++)
			{
				bigInteger2 = bigInteger2.Multiply((BigInteger)primes[i]);
			}
			for (int j = 0; j < primes.Count; j++)
			{
				BigInteger bigInteger3 = (BigInteger)primes[j];
				BigInteger bigInteger4 = bigInteger2.Divide(bigInteger3);
				BigInteger val = bigInteger4.ModInverse(bigInteger3);
				BigInteger bigInteger5 = bigInteger4.Multiply(val);
				bigInteger5 = bigInteger5.Multiply((BigInteger)congruences[j]);
				bigInteger = bigInteger.Add(bigInteger5);
			}
			return bigInteger.Mod(bigInteger2);
		}

		// Token: 0x040022B9 RID: 8889
		private bool forEncryption;

		// Token: 0x040022BA RID: 8890
		private NaccacheSternKeyParameters key;

		// Token: 0x040022BB RID: 8891
		private IList[] lookup;
	}
}
