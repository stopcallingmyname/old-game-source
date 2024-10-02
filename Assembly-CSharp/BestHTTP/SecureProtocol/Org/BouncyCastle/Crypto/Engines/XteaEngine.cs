using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200059D RID: 1437
	public class XteaEngine : IBlockCipher
	{
		// Token: 0x0600366A RID: 13930 RVA: 0x001510C0 File Offset: 0x0014F2C0
		public XteaEngine()
		{
			this._initialised = false;
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600366B RID: 13931 RVA: 0x001510F5 File Offset: 0x0014F2F5
		public virtual string AlgorithmName
		{
			get
			{
				return "XTEA";
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x0600366C RID: 13932 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x001510FC File Offset: 0x0014F2FC
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to TEA init - " + Platform.GetTypeName(parameters));
			}
			this._forEncryption = forEncryption;
			this._initialised = true;
			KeyParameter keyParameter = (KeyParameter)parameters;
			this.setKey(keyParameter.GetKey());
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00151148 File Offset: 0x0014F348
		public virtual int ProcessBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			if (!this._initialised)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(inBytes, inOff, 8, "input buffer too short");
			Check.OutputLength(outBytes, outOff, 8, "output buffer too short");
			if (!this._forEncryption)
			{
				return this.decryptBlock(inBytes, inOff, outBytes, outOff);
			}
			return this.encryptBlock(inBytes, inOff, outBytes, outOff);
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x001511B0 File Offset: 0x0014F3B0
		private void setKey(byte[] key)
		{
			int i;
			int num = i = 0;
			while (i < 4)
			{
				this._S[i] = Pack.BE_To_UInt32(key, num);
				i++;
				num += 4;
			}
			num = (i = 0);
			while (i < 32)
			{
				this._sum0[i] = (uint)(num + (int)this._S[num & 3]);
				num += -1640531527;
				this._sum1[i] = (uint)(num + (int)this._S[num >> 11 & 3]);
				i++;
			}
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x00151220 File Offset: 0x0014F420
		private int encryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			for (int i = 0; i < 32; i++)
			{
				num += ((num2 << 4 ^ num2 >> 5) + num2 ^ this._sum0[i]);
				num2 += ((num << 4 ^ num >> 5) + num ^ this._sum1[i]);
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x0015128C File Offset: 0x0014F48C
		private int decryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			for (int i = 31; i >= 0; i--)
			{
				num2 -= ((num << 4 ^ num >> 5) + num ^ this._sum1[i]);
				num -= ((num2 << 4 ^ num2 >> 5) + num2 ^ this._sum0[i]);
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x04002396 RID: 9110
		private const int rounds = 32;

		// Token: 0x04002397 RID: 9111
		private const int block_size = 8;

		// Token: 0x04002398 RID: 9112
		private const int delta = -1640531527;

		// Token: 0x04002399 RID: 9113
		private uint[] _S = new uint[4];

		// Token: 0x0400239A RID: 9114
		private uint[] _sum0 = new uint[32];

		// Token: 0x0400239B RID: 9115
		private uint[] _sum1 = new uint[32];

		// Token: 0x0400239C RID: 9116
		private bool _initialised;

		// Token: 0x0400239D RID: 9117
		private bool _forEncryption;
	}
}
