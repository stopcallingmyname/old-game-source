using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005A3 RID: 1443
	public class Blake2sDigest : IDigest
	{
		// Token: 0x060036C0 RID: 14016 RVA: 0x00153272 File Offset: 0x00151472
		public Blake2sDigest() : this(256)
		{
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x00153280 File Offset: 0x00151480
		public Blake2sDigest(Blake2sDigest digest)
		{
			this.digestLength = 32;
			this.internalState = new uint[16];
			base..ctor();
			this.bufferPos = digest.bufferPos;
			this.buffer = Arrays.Clone(digest.buffer);
			this.keyLength = digest.keyLength;
			this.key = Arrays.Clone(digest.key);
			this.digestLength = digest.digestLength;
			this.chainValue = Arrays.Clone(digest.chainValue);
			this.personalization = Arrays.Clone(digest.personalization);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00153310 File Offset: 0x00151510
		public Blake2sDigest(int digestBits)
		{
			this.digestLength = 32;
			this.internalState = new uint[16];
			base..ctor();
			if (digestBits < 8 || digestBits > 256 || digestBits % 8 != 0)
			{
				throw new ArgumentException("BLAKE2s digest bit length must be a multiple of 8 and not greater than 256");
			}
			this.buffer = new byte[64];
			this.keyLength = 0;
			this.digestLength = digestBits / 8;
			this.Init();
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x00153378 File Offset: 0x00151578
		public Blake2sDigest(byte[] key)
		{
			this.digestLength = 32;
			this.internalState = new uint[16];
			base..ctor();
			this.buffer = new byte[64];
			if (key != null)
			{
				if (key.Length > 32)
				{
					throw new ArgumentException("Keys > 32 are not supported");
				}
				this.key = new byte[key.Length];
				Array.Copy(key, 0, this.key, 0, key.Length);
				this.keyLength = key.Length;
				Array.Copy(key, 0, this.buffer, 0, key.Length);
				this.bufferPos = 64;
			}
			this.digestLength = 32;
			this.Init();
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x00153414 File Offset: 0x00151614
		public Blake2sDigest(byte[] key, int digestBytes, byte[] salt, byte[] personalization)
		{
			this.digestLength = 32;
			this.internalState = new uint[16];
			base..ctor();
			if (digestBytes < 1 || digestBytes > 32)
			{
				throw new ArgumentException("Invalid digest length (required: 1 - 32)");
			}
			this.digestLength = digestBytes;
			this.buffer = new byte[64];
			if (salt != null)
			{
				if (salt.Length != 8)
				{
					throw new ArgumentException("Salt length must be exactly 8 bytes");
				}
				this.salt = new byte[8];
				Array.Copy(salt, 0, this.salt, 0, salt.Length);
			}
			if (personalization != null)
			{
				if (personalization.Length != 8)
				{
					throw new ArgumentException("Personalization length must be exactly 8 bytes");
				}
				this.personalization = new byte[8];
				Array.Copy(personalization, 0, this.personalization, 0, personalization.Length);
			}
			if (key != null)
			{
				if (key.Length > 32)
				{
					throw new ArgumentException("Keys > 32 bytes are not supported");
				}
				this.key = new byte[key.Length];
				Array.Copy(key, 0, this.key, 0, key.Length);
				this.keyLength = key.Length;
				Array.Copy(key, 0, this.buffer, 0, key.Length);
				this.bufferPos = 64;
			}
			this.Init();
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x00153528 File Offset: 0x00151728
		private void Init()
		{
			if (this.chainValue == null)
			{
				this.chainValue = new uint[8];
				this.chainValue[0] = (Blake2sDigest.blake2s_IV[0] ^ (uint)(this.digestLength | this.keyLength << 8 | 16842752));
				this.chainValue[1] = Blake2sDigest.blake2s_IV[1];
				this.chainValue[2] = Blake2sDigest.blake2s_IV[2];
				this.chainValue[3] = Blake2sDigest.blake2s_IV[3];
				this.chainValue[4] = Blake2sDigest.blake2s_IV[4];
				this.chainValue[5] = Blake2sDigest.blake2s_IV[5];
				if (this.salt != null)
				{
					this.chainValue[4] ^= Pack.LE_To_UInt32(this.salt, 0);
					this.chainValue[5] ^= Pack.LE_To_UInt32(this.salt, 4);
				}
				this.chainValue[6] = Blake2sDigest.blake2s_IV[6];
				this.chainValue[7] = Blake2sDigest.blake2s_IV[7];
				if (this.personalization != null)
				{
					this.chainValue[6] ^= Pack.LE_To_UInt32(this.personalization, 0);
					this.chainValue[7] ^= Pack.LE_To_UInt32(this.personalization, 4);
				}
			}
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x0015365C File Offset: 0x0015185C
		private void InitializeInternalState()
		{
			Array.Copy(this.chainValue, 0, this.internalState, 0, this.chainValue.Length);
			Array.Copy(Blake2sDigest.blake2s_IV, 0, this.internalState, this.chainValue.Length, 4);
			this.internalState[12] = (this.t0 ^ Blake2sDigest.blake2s_IV[4]);
			this.internalState[13] = (this.t1 ^ Blake2sDigest.blake2s_IV[5]);
			this.internalState[14] = (this.f0 ^ Blake2sDigest.blake2s_IV[6]);
			this.internalState[15] = Blake2sDigest.blake2s_IV[7];
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x001536F4 File Offset: 0x001518F4
		public virtual void Update(byte b)
		{
			if (64 - this.bufferPos == 0)
			{
				this.t0 += 64U;
				if (this.t0 == 0U)
				{
					this.t1 += 1U;
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

		// Token: 0x060036C8 RID: 14024 RVA: 0x00153780 File Offset: 0x00151980
		public virtual void BlockUpdate(byte[] message, int offset, int len)
		{
			if (message == null || len == 0)
			{
				return;
			}
			int num = 0;
			if (this.bufferPos != 0)
			{
				num = 64 - this.bufferPos;
				if (num >= len)
				{
					Array.Copy(message, offset, this.buffer, this.bufferPos, len);
					this.bufferPos += len;
					return;
				}
				Array.Copy(message, offset, this.buffer, this.bufferPos, num);
				this.t0 += 64U;
				if (this.t0 == 0U)
				{
					this.t1 += 1U;
				}
				this.Compress(this.buffer, 0);
				this.bufferPos = 0;
				Array.Clear(this.buffer, 0, this.buffer.Length);
			}
			int num2 = offset + len - 64;
			int i;
			for (i = offset + num; i < num2; i += 64)
			{
				this.t0 += 64U;
				if (this.t0 == 0U)
				{
					this.t1 += 1U;
				}
				this.Compress(message, i);
			}
			Array.Copy(message, i, this.buffer, 0, offset + len - i);
			this.bufferPos += offset + len - i;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x001538A0 File Offset: 0x00151AA0
		public virtual int DoFinal(byte[] output, int outOffset)
		{
			this.f0 = uint.MaxValue;
			this.t0 += (uint)this.bufferPos;
			if (this.t0 < 0U && (long)this.bufferPos > (long)(-(long)((ulong)this.t0)))
			{
				this.t1 += 1U;
			}
			this.Compress(this.buffer, 0);
			Array.Clear(this.buffer, 0, this.buffer.Length);
			Array.Clear(this.internalState, 0, this.internalState.Length);
			int num = 0;
			while (num < this.chainValue.Length && num * 4 < this.digestLength)
			{
				byte[] sourceArray = Pack.UInt32_To_LE(this.chainValue[num]);
				if (num * 4 < this.digestLength - 4)
				{
					Array.Copy(sourceArray, 0, output, outOffset + num * 4, 4);
				}
				else
				{
					Array.Copy(sourceArray, 0, output, outOffset + num * 4, this.digestLength - num * 4);
				}
				num++;
			}
			Array.Clear(this.chainValue, 0, this.chainValue.Length);
			this.Reset();
			return this.digestLength;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x001539A4 File Offset: 0x00151BA4
		public virtual void Reset()
		{
			this.bufferPos = 0;
			this.f0 = 0U;
			this.t0 = 0U;
			this.t1 = 0U;
			this.chainValue = null;
			Array.Clear(this.buffer, 0, this.buffer.Length);
			if (this.key != null)
			{
				Array.Copy(this.key, 0, this.buffer, 0, this.key.Length);
				this.bufferPos = 64;
			}
			this.Init();
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x00153A1C File Offset: 0x00151C1C
		private void Compress(byte[] message, int messagePos)
		{
			this.InitializeInternalState();
			uint[] array = new uint[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = Pack.LE_To_UInt32(message, messagePos + i * 4);
			}
			for (int j = 0; j < 10; j++)
			{
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 0]], array[(int)Blake2sDigest.blake2s_sigma[j, 1]], 0, 4, 8, 12);
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 2]], array[(int)Blake2sDigest.blake2s_sigma[j, 3]], 1, 5, 9, 13);
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 4]], array[(int)Blake2sDigest.blake2s_sigma[j, 5]], 2, 6, 10, 14);
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 6]], array[(int)Blake2sDigest.blake2s_sigma[j, 7]], 3, 7, 11, 15);
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 8]], array[(int)Blake2sDigest.blake2s_sigma[j, 9]], 0, 5, 10, 15);
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 10]], array[(int)Blake2sDigest.blake2s_sigma[j, 11]], 1, 6, 11, 12);
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 12]], array[(int)Blake2sDigest.blake2s_sigma[j, 13]], 2, 7, 8, 13);
				this.G(array[(int)Blake2sDigest.blake2s_sigma[j, 14]], array[(int)Blake2sDigest.blake2s_sigma[j, 15]], 3, 4, 9, 14);
			}
			for (int k = 0; k < this.chainValue.Length; k++)
			{
				this.chainValue[k] = (this.chainValue[k] ^ this.internalState[k] ^ this.internalState[k + 8]);
			}
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x00153BE4 File Offset: 0x00151DE4
		private void G(uint m1, uint m2, int posA, int posB, int posC, int posD)
		{
			this.internalState[posA] = this.internalState[posA] + this.internalState[posB] + m1;
			this.internalState[posD] = this.rotr32(this.internalState[posD] ^ this.internalState[posA], 16);
			this.internalState[posC] = this.internalState[posC] + this.internalState[posD];
			this.internalState[posB] = this.rotr32(this.internalState[posB] ^ this.internalState[posC], 12);
			this.internalState[posA] = this.internalState[posA] + this.internalState[posB] + m2;
			this.internalState[posD] = this.rotr32(this.internalState[posD] ^ this.internalState[posA], 8);
			this.internalState[posC] = this.internalState[posC] + this.internalState[posD];
			this.internalState[posB] = this.rotr32(this.internalState[posB] ^ this.internalState[posC], 7);
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x00153CED File Offset: 0x00151EED
		private uint rotr32(uint x, int rot)
		{
			return x >> rot | x << -rot;
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x00153CFD File Offset: 0x00151EFD
		public virtual string AlgorithmName
		{
			get
			{
				return "BLAKE2s";
			}
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x00153D04 File Offset: 0x00151F04
		public virtual int GetDigestSize()
		{
			return this.digestLength;
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x00153D0C File Offset: 0x00151F0C
		public virtual int GetByteLength()
		{
			return 64;
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x00153D10 File Offset: 0x00151F10
		public virtual void ClearKey()
		{
			if (this.key != null)
			{
				Array.Clear(this.key, 0, this.key.Length);
				Array.Clear(this.buffer, 0, this.buffer.Length);
			}
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x00153D42 File Offset: 0x00151F42
		public virtual void ClearSalt()
		{
			if (this.salt != null)
			{
				Array.Clear(this.salt, 0, this.salt.Length);
			}
		}

		// Token: 0x040023CC RID: 9164
		private static readonly uint[] blake2s_IV = new uint[]
		{
			1779033703U,
			3144134277U,
			1013904242U,
			2773480762U,
			1359893119U,
			2600822924U,
			528734635U,
			1541459225U
		};

		// Token: 0x040023CD RID: 9165
		private static readonly byte[,] blake2s_sigma = new byte[,]
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
			}
		};

		// Token: 0x040023CE RID: 9166
		private const int ROUNDS = 10;

		// Token: 0x040023CF RID: 9167
		private const int BLOCK_LENGTH_BYTES = 64;

		// Token: 0x040023D0 RID: 9168
		private int digestLength;

		// Token: 0x040023D1 RID: 9169
		private int keyLength;

		// Token: 0x040023D2 RID: 9170
		private byte[] salt;

		// Token: 0x040023D3 RID: 9171
		private byte[] personalization;

		// Token: 0x040023D4 RID: 9172
		private byte[] key;

		// Token: 0x040023D5 RID: 9173
		private byte[] buffer;

		// Token: 0x040023D6 RID: 9174
		private int bufferPos;

		// Token: 0x040023D7 RID: 9175
		private uint[] internalState;

		// Token: 0x040023D8 RID: 9176
		private uint[] chainValue;

		// Token: 0x040023D9 RID: 9177
		private uint t0;

		// Token: 0x040023DA RID: 9178
		private uint t1;

		// Token: 0x040023DB RID: 9179
		private uint f0;
	}
}
