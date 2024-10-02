using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200057F RID: 1407
	public class NoekeonEngine : IBlockCipher
	{
		// Token: 0x0600350F RID: 13583 RVA: 0x001449A8 File Offset: 0x00142BA8
		public NoekeonEngine()
		{
			this._initialised = false;
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x001449DB File Offset: 0x00142BDB
		public virtual string AlgorithmName
		{
			get
			{
				return "Noekeon";
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x0012AD29 File Offset: 0x00128F29
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x001449E4 File Offset: 0x00142BE4
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("Invalid parameters passed to Noekeon init - " + Platform.GetTypeName(parameters), "parameters");
			}
			this._forEncryption = forEncryption;
			this._initialised = true;
			KeyParameter keyParameter = (KeyParameter)parameters;
			this.setKey(keyParameter.GetKey());
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x00144A38 File Offset: 0x00142C38
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this._initialised)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(input, inOff, 16, "input buffer too short");
			Check.OutputLength(output, outOff, 16, "output buffer too short");
			if (!this._forEncryption)
			{
				return this.decryptBlock(input, inOff, output, outOff);
			}
			return this.encryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x00144A9F File Offset: 0x00142C9F
		private void setKey(byte[] key)
		{
			this.subKeys[0] = Pack.BE_To_UInt32(key, 0);
			this.subKeys[1] = Pack.BE_To_UInt32(key, 4);
			this.subKeys[2] = Pack.BE_To_UInt32(key, 8);
			this.subKeys[3] = Pack.BE_To_UInt32(key, 12);
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x00144AE0 File Offset: 0x00142CE0
		private int encryptBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			this.state[0] = Pack.BE_To_UInt32(input, inOff);
			this.state[1] = Pack.BE_To_UInt32(input, inOff + 4);
			this.state[2] = Pack.BE_To_UInt32(input, inOff + 8);
			this.state[3] = Pack.BE_To_UInt32(input, inOff + 12);
			int i;
			for (i = 0; i < 16; i++)
			{
				this.state[0] ^= NoekeonEngine.roundConstants[i];
				this.theta(this.state, this.subKeys);
				this.pi1(this.state);
				this.gamma(this.state);
				this.pi2(this.state);
			}
			this.state[0] ^= NoekeonEngine.roundConstants[i];
			this.theta(this.state, this.subKeys);
			Pack.UInt32_To_BE(this.state[0], output, outOff);
			Pack.UInt32_To_BE(this.state[1], output, outOff + 4);
			Pack.UInt32_To_BE(this.state[2], output, outOff + 8);
			Pack.UInt32_To_BE(this.state[3], output, outOff + 12);
			return 16;
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x00144BFC File Offset: 0x00142DFC
		private int decryptBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			this.state[0] = Pack.BE_To_UInt32(input, inOff);
			this.state[1] = Pack.BE_To_UInt32(input, inOff + 4);
			this.state[2] = Pack.BE_To_UInt32(input, inOff + 8);
			this.state[3] = Pack.BE_To_UInt32(input, inOff + 12);
			Array.Copy(this.subKeys, 0, this.decryptKeys, 0, this.subKeys.Length);
			this.theta(this.decryptKeys, NoekeonEngine.nullVector);
			int i;
			for (i = 16; i > 0; i--)
			{
				this.theta(this.state, this.decryptKeys);
				this.state[0] ^= NoekeonEngine.roundConstants[i];
				this.pi1(this.state);
				this.gamma(this.state);
				this.pi2(this.state);
			}
			this.theta(this.state, this.decryptKeys);
			this.state[0] ^= NoekeonEngine.roundConstants[i];
			Pack.UInt32_To_BE(this.state[0], output, outOff);
			Pack.UInt32_To_BE(this.state[1], output, outOff + 4);
			Pack.UInt32_To_BE(this.state[2], output, outOff + 8);
			Pack.UInt32_To_BE(this.state[3], output, outOff + 12);
			return 16;
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x00144D44 File Offset: 0x00142F44
		private void gamma(uint[] a)
		{
			a[1] ^= (~a[3] & ~a[2]);
			a[0] ^= (a[2] & a[1]);
			uint num = a[3];
			a[3] = a[0];
			a[0] = num;
			a[2] ^= (a[0] ^ a[1] ^ a[3]);
			a[1] ^= (~a[3] & ~a[2]);
			a[0] ^= (a[2] & a[1]);
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x00144DC4 File Offset: 0x00142FC4
		private void theta(uint[] a, uint[] k)
		{
			uint num = a[0] ^ a[2];
			num ^= (this.rotl(num, 8) ^ this.rotl(num, 24));
			a[1] ^= num;
			a[3] ^= num;
			for (int i = 0; i < 4; i++)
			{
				a[i] ^= k[i];
			}
			num = (a[1] ^ a[3]);
			num ^= (this.rotl(num, 8) ^ this.rotl(num, 24));
			a[0] ^= num;
			a[2] ^= num;
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x00144E55 File Offset: 0x00143055
		private void pi1(uint[] a)
		{
			a[1] = this.rotl(a[1], 1);
			a[2] = this.rotl(a[2], 5);
			a[3] = this.rotl(a[3], 2);
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x00144E7E File Offset: 0x0014307E
		private void pi2(uint[] a)
		{
			a[1] = this.rotl(a[1], 31);
			a[2] = this.rotl(a[2], 27);
			a[3] = this.rotl(a[3], 30);
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x00144EAA File Offset: 0x001430AA
		private uint rotl(uint x, int y)
		{
			return x << y | x >> 32 - y;
		}

		// Token: 0x040022BC RID: 8892
		private const int GenericSize = 16;

		// Token: 0x040022BD RID: 8893
		private static readonly uint[] nullVector = new uint[4];

		// Token: 0x040022BE RID: 8894
		private static readonly uint[] roundConstants = new uint[]
		{
			128U,
			27U,
			54U,
			108U,
			216U,
			171U,
			77U,
			154U,
			47U,
			94U,
			188U,
			99U,
			198U,
			151U,
			53U,
			106U,
			212U
		};

		// Token: 0x040022BF RID: 8895
		private uint[] state = new uint[4];

		// Token: 0x040022C0 RID: 8896
		private uint[] subKeys = new uint[4];

		// Token: 0x040022C1 RID: 8897
		private uint[] decryptKeys = new uint[4];

		// Token: 0x040022C2 RID: 8898
		private bool _initialised;

		// Token: 0x040022C3 RID: 8899
		private bool _forEncryption;
	}
}
