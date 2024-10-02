using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005A2 RID: 1442
	public class Blake2bDigest : IDigest
	{
		// Token: 0x060036AC RID: 13996 RVA: 0x001526F2 File Offset: 0x001508F2
		public Blake2bDigest() : this(512)
		{
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x00152700 File Offset: 0x00150900
		public Blake2bDigest(Blake2bDigest digest)
		{
			this.digestLength = 64;
			this.internalState = new ulong[16];
			base..ctor();
			this.bufferPos = digest.bufferPos;
			this.buffer = Arrays.Clone(digest.buffer);
			this.keyLength = digest.keyLength;
			this.key = Arrays.Clone(digest.key);
			this.digestLength = digest.digestLength;
			this.chainValue = Arrays.Clone(digest.chainValue);
			this.personalization = Arrays.Clone(digest.personalization);
			this.salt = Arrays.Clone(digest.salt);
			this.t0 = digest.t0;
			this.t1 = digest.t1;
			this.f0 = digest.f0;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x001527C8 File Offset: 0x001509C8
		public Blake2bDigest(int digestSize)
		{
			this.digestLength = 64;
			this.internalState = new ulong[16];
			base..ctor();
			if (digestSize < 8 || digestSize > 512 || digestSize % 8 != 0)
			{
				throw new ArgumentException("BLAKE2b digest bit length must be a multiple of 8 and not greater than 512");
			}
			this.buffer = new byte[128];
			this.keyLength = 0;
			this.digestLength = digestSize / 8;
			this.Init();
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x00152834 File Offset: 0x00150A34
		public Blake2bDigest(byte[] key)
		{
			this.digestLength = 64;
			this.internalState = new ulong[16];
			base..ctor();
			this.buffer = new byte[128];
			if (key != null)
			{
				this.key = new byte[key.Length];
				Array.Copy(key, 0, this.key, 0, key.Length);
				if (key.Length > 64)
				{
					throw new ArgumentException("Keys > 64 are not supported");
				}
				this.keyLength = key.Length;
				Array.Copy(key, 0, this.buffer, 0, key.Length);
				this.bufferPos = 128;
			}
			this.digestLength = 64;
			this.Init();
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x001528D4 File Offset: 0x00150AD4
		public Blake2bDigest(byte[] key, int digestLength, byte[] salt, byte[] personalization)
		{
			this.digestLength = 64;
			this.internalState = new ulong[16];
			base..ctor();
			if (digestLength < 1 || digestLength > 64)
			{
				throw new ArgumentException("Invalid digest length (required: 1 - 64)");
			}
			this.digestLength = digestLength;
			this.buffer = new byte[128];
			if (salt != null)
			{
				if (salt.Length != 16)
				{
					throw new ArgumentException("salt length must be exactly 16 bytes");
				}
				this.salt = new byte[16];
				Array.Copy(salt, 0, this.salt, 0, salt.Length);
			}
			if (personalization != null)
			{
				if (personalization.Length != 16)
				{
					throw new ArgumentException("personalization length must be exactly 16 bytes");
				}
				this.personalization = new byte[16];
				Array.Copy(personalization, 0, this.personalization, 0, personalization.Length);
			}
			if (key != null)
			{
				if (key.Length > 64)
				{
					throw new ArgumentException("Keys > 64 are not supported");
				}
				this.key = new byte[key.Length];
				Array.Copy(key, 0, this.key, 0, key.Length);
				this.keyLength = key.Length;
				Array.Copy(key, 0, this.buffer, 0, key.Length);
				this.bufferPos = 128;
			}
			this.Init();
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x001529F0 File Offset: 0x00150BF0
		private void Init()
		{
			if (this.chainValue == null)
			{
				this.chainValue = new ulong[8];
				this.chainValue[0] = (Blake2bDigest.blake2b_IV[0] ^ (ulong)((long)(this.digestLength | this.keyLength << 8 | 16842752)));
				this.chainValue[1] = Blake2bDigest.blake2b_IV[1];
				this.chainValue[2] = Blake2bDigest.blake2b_IV[2];
				this.chainValue[3] = Blake2bDigest.blake2b_IV[3];
				this.chainValue[4] = Blake2bDigest.blake2b_IV[4];
				this.chainValue[5] = Blake2bDigest.blake2b_IV[5];
				if (this.salt != null)
				{
					this.chainValue[4] ^= Pack.LE_To_UInt64(this.salt, 0);
					this.chainValue[5] ^= Pack.LE_To_UInt64(this.salt, 8);
				}
				this.chainValue[6] = Blake2bDigest.blake2b_IV[6];
				this.chainValue[7] = Blake2bDigest.blake2b_IV[7];
				if (this.personalization != null)
				{
					this.chainValue[6] ^= Pack.LE_To_UInt64(this.personalization, 0);
					this.chainValue[7] ^= Pack.LE_To_UInt64(this.personalization, 8);
				}
			}
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x00152B24 File Offset: 0x00150D24
		private void InitializeInternalState()
		{
			Array.Copy(this.chainValue, 0, this.internalState, 0, this.chainValue.Length);
			Array.Copy(Blake2bDigest.blake2b_IV, 0, this.internalState, this.chainValue.Length, 4);
			this.internalState[12] = (this.t0 ^ Blake2bDigest.blake2b_IV[4]);
			this.internalState[13] = (this.t1 ^ Blake2bDigest.blake2b_IV[5]);
			this.internalState[14] = (this.f0 ^ Blake2bDigest.blake2b_IV[6]);
			this.internalState[15] = Blake2bDigest.blake2b_IV[7];
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x00152BBC File Offset: 0x00150DBC
		public virtual void Update(byte b)
		{
			if (128 - this.bufferPos == 0)
			{
				this.t0 += 128UL;
				if (this.t0 == 0UL)
				{
					this.t1 += 1UL;
				}
				this.Compress(this.buffer, 0);
				Array.Clear(this.buffer, 0, this.buffer.Length);
				this.buffer[0] = b;
				this.bufferPos = 1;
				return;
			}
			this.buffer[this.bufferPos] = b;
			this.bufferPos++;
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x00152C50 File Offset: 0x00150E50
		public virtual void BlockUpdate(byte[] message, int offset, int len)
		{
			if (message == null || len == 0)
			{
				return;
			}
			int num = 0;
			if (this.bufferPos != 0)
			{
				num = 128 - this.bufferPos;
				if (num >= len)
				{
					Array.Copy(message, offset, this.buffer, this.bufferPos, len);
					this.bufferPos += len;
					return;
				}
				Array.Copy(message, offset, this.buffer, this.bufferPos, num);
				this.t0 += 128UL;
				if (this.t0 == 0UL)
				{
					this.t1 += 1UL;
				}
				this.Compress(this.buffer, 0);
				this.bufferPos = 0;
				Array.Clear(this.buffer, 0, this.buffer.Length);
			}
			int num2 = offset + len - 128;
			int i;
			for (i = offset + num; i < num2; i += 128)
			{
				this.t0 += 128UL;
				if (this.t0 == 0UL)
				{
					this.t1 += 1UL;
				}
				this.Compress(message, i);
			}
			Array.Copy(message, i, this.buffer, 0, offset + len - i);
			this.bufferPos += offset + len - i;
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x00152D80 File Offset: 0x00150F80
		public virtual int DoFinal(byte[] output, int outOffset)
		{
			this.f0 = ulong.MaxValue;
			this.t0 += (ulong)((long)this.bufferPos);
			if (this.bufferPos > 0 && this.t0 == 0UL)
			{
				this.t1 += 1UL;
			}
			this.Compress(this.buffer, 0);
			Array.Clear(this.buffer, 0, this.buffer.Length);
			Array.Clear(this.internalState, 0, this.internalState.Length);
			int num = 0;
			while (num < this.chainValue.Length && num * 8 < this.digestLength)
			{
				byte[] sourceArray = Pack.UInt64_To_LE(this.chainValue[num]);
				if (num * 8 < this.digestLength - 8)
				{
					Array.Copy(sourceArray, 0, output, outOffset + num * 8, 8);
				}
				else
				{
					Array.Copy(sourceArray, 0, output, outOffset + num * 8, this.digestLength - num * 8);
				}
				num++;
			}
			Array.Clear(this.chainValue, 0, this.chainValue.Length);
			this.Reset();
			return this.digestLength;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x00152E80 File Offset: 0x00151080
		public virtual void Reset()
		{
			this.bufferPos = 0;
			this.f0 = 0UL;
			this.t0 = 0UL;
			this.t1 = 0UL;
			this.chainValue = null;
			Array.Clear(this.buffer, 0, this.buffer.Length);
			if (this.key != null)
			{
				Array.Copy(this.key, 0, this.buffer, 0, this.key.Length);
				this.bufferPos = 128;
			}
			this.Init();
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x00152EFC File Offset: 0x001510FC
		private void Compress(byte[] message, int messagePos)
		{
			this.InitializeInternalState();
			ulong[] array = new ulong[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = Pack.LE_To_UInt64(message, messagePos + i * 8);
			}
			for (int j = 0; j < 12; j++)
			{
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 0]], array[(int)Blake2bDigest.blake2b_sigma[j, 1]], 0, 4, 8, 12);
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 2]], array[(int)Blake2bDigest.blake2b_sigma[j, 3]], 1, 5, 9, 13);
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 4]], array[(int)Blake2bDigest.blake2b_sigma[j, 5]], 2, 6, 10, 14);
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 6]], array[(int)Blake2bDigest.blake2b_sigma[j, 7]], 3, 7, 11, 15);
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 8]], array[(int)Blake2bDigest.blake2b_sigma[j, 9]], 0, 5, 10, 15);
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 10]], array[(int)Blake2bDigest.blake2b_sigma[j, 11]], 1, 6, 11, 12);
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 12]], array[(int)Blake2bDigest.blake2b_sigma[j, 13]], 2, 7, 8, 13);
				this.G(array[(int)Blake2bDigest.blake2b_sigma[j, 14]], array[(int)Blake2bDigest.blake2b_sigma[j, 15]], 3, 4, 9, 14);
			}
			for (int k = 0; k < this.chainValue.Length; k++)
			{
				this.chainValue[k] = (this.chainValue[k] ^ this.internalState[k] ^ this.internalState[k + 8]);
			}
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x001530C4 File Offset: 0x001512C4
		private void G(ulong m1, ulong m2, int posA, int posB, int posC, int posD)
		{
			this.internalState[posA] = this.internalState[posA] + this.internalState[posB] + m1;
			this.internalState[posD] = Blake2bDigest.Rotr64(this.internalState[posD] ^ this.internalState[posA], 32);
			this.internalState[posC] = this.internalState[posC] + this.internalState[posD];
			this.internalState[posB] = Blake2bDigest.Rotr64(this.internalState[posB] ^ this.internalState[posC], 24);
			this.internalState[posA] = this.internalState[posA] + this.internalState[posB] + m2;
			this.internalState[posD] = Blake2bDigest.Rotr64(this.internalState[posD] ^ this.internalState[posA], 16);
			this.internalState[posC] = this.internalState[posC] + this.internalState[posD];
			this.internalState[posB] = Blake2bDigest.Rotr64(this.internalState[posB] ^ this.internalState[posC], 63);
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x001531CB File Offset: 0x001513CB
		private static ulong Rotr64(ulong x, int rot)
		{
			return x >> rot | x << -rot;
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060036BA RID: 14010 RVA: 0x001531DB File Offset: 0x001513DB
		public virtual string AlgorithmName
		{
			get
			{
				return "BLAKE2b";
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x001531E2 File Offset: 0x001513E2
		public virtual int GetDigestSize()
		{
			return this.digestLength;
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x001531EA File Offset: 0x001513EA
		public virtual int GetByteLength()
		{
			return 128;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x001531F1 File Offset: 0x001513F1
		public virtual void ClearKey()
		{
			if (this.key != null)
			{
				Array.Clear(this.key, 0, this.key.Length);
				Array.Clear(this.buffer, 0, this.buffer.Length);
			}
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x00153223 File Offset: 0x00151423
		public virtual void ClearSalt()
		{
			if (this.salt != null)
			{
				Array.Clear(this.salt, 0, this.salt.Length);
			}
		}

		// Token: 0x040023BC RID: 9148
		private static readonly ulong[] blake2b_IV = new ulong[]
		{
			7640891576956012808UL,
			13503953896175478587UL,
			4354685564936845355UL,
			11912009170470909681UL,
			5840696475078001361UL,
			11170449401992604703UL,
			2270897969802886507UL,
			6620516959819538809UL
		};

		// Token: 0x040023BD RID: 9149
		private static readonly byte[,] blake2b_sigma = new byte[,]
		{
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			},
			{
				14,
				10,
				4,
				8,
				9,
				15,
				13,
				6,
				1,
				12,
				0,
				2,
				11,
				7,
				5,
				3
			},
			{
				11,
				8,
				12,
				0,
				5,
				2,
				15,
				13,
				10,
				14,
				3,
				6,
				7,
				1,
				9,
				4
			},
			{
				7,
				9,
				3,
				1,
				13,
				12,
				11,
				14,
				2,
				6,
				5,
				10,
				4,
				0,
				15,
				8
			},
			{
				9,
				0,
				5,
				7,
				2,
				4,
				10,
				15,
				14,
				1,
				11,
				12,
				6,
				8,
				3,
				13
			},
			{
				2,
				12,
				6,
				10,
				0,
				11,
				8,
				3,
				4,
				13,
				7,
				5,
				15,
				14,
				1,
				9
			},
			{
				12,
				5,
				1,
				15,
				14,
				13,
				4,
				10,
				0,
				7,
				6,
				3,
				9,
				2,
				8,
				11
			},
			{
				13,
				11,
				7,
				14,
				12,
				1,
				3,
				9,
				5,
				0,
				15,
				4,
				8,
				6,
				2,
				10
			},
			{
				6,
				15,
				14,
				9,
				11,
				3,
				0,
				8,
				12,
				2,
				13,
				7,
				1,
				4,
				10,
				5
			},
			{
				10,
				2,
				8,
				4,
				7,
				6,
				1,
				5,
				15,
				11,
				9,
				14,
				3,
				12,
				13,
				0
			},
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			},
			{
				14,
				10,
				4,
				8,
				9,
				15,
				13,
				6,
				1,
				12,
				0,
				2,
				11,
				7,
				5,
				3
			}
		};

		// Token: 0x040023BE RID: 9150
		private const int ROUNDS = 12;

		// Token: 0x040023BF RID: 9151
		private const int BLOCK_LENGTH_BYTES = 128;

		// Token: 0x040023C0 RID: 9152
		private int digestLength;

		// Token: 0x040023C1 RID: 9153
		private int keyLength;

		// Token: 0x040023C2 RID: 9154
		private byte[] salt;

		// Token: 0x040023C3 RID: 9155
		private byte[] personalization;

		// Token: 0x040023C4 RID: 9156
		private byte[] key;

		// Token: 0x040023C5 RID: 9157
		private byte[] buffer;

		// Token: 0x040023C6 RID: 9158
		private int bufferPos;

		// Token: 0x040023C7 RID: 9159
		private ulong[] internalState;

		// Token: 0x040023C8 RID: 9160
		private ulong[] chainValue;

		// Token: 0x040023C9 RID: 9161
		private ulong t0;

		// Token: 0x040023CA RID: 9162
		private ulong t1;

		// Token: 0x040023CB RID: 9163
		private ulong f0;
	}
}
