using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000596 RID: 1430
	public class TeaEngine : IBlockCipher
	{
		// Token: 0x06003623 RID: 13859 RVA: 0x0014CEA4 File Offset: 0x0014B0A4
		public TeaEngine()
		{
			this._initialised = false;
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06003624 RID: 13860 RVA: 0x0014CEB3 File Offset: 0x0014B0B3
		public virtual string AlgorithmName
		{
			get
			{
				return "TEA";
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06003625 RID: 13861 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x0014CEBC File Offset: 0x0014B0BC
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

		// Token: 0x06003628 RID: 13864 RVA: 0x0014CF08 File Offset: 0x0014B108
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

		// Token: 0x06003629 RID: 13865 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x0014CF6D File Offset: 0x0014B16D
		private void setKey(byte[] key)
		{
			this._a = Pack.BE_To_UInt32(key, 0);
			this._b = Pack.BE_To_UInt32(key, 4);
			this._c = Pack.BE_To_UInt32(key, 8);
			this._d = Pack.BE_To_UInt32(key, 12);
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x0014CFA4 File Offset: 0x0014B1A4
		private int encryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			uint num3 = 0U;
			for (int num4 = 0; num4 != 32; num4++)
			{
				num3 += 2654435769U;
				num += ((num2 << 4) + this._a ^ num2 + num3 ^ (num2 >> 5) + this._b);
				num2 += ((num << 4) + this._c ^ num + num3 ^ (num >> 5) + this._d);
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x0014D028 File Offset: 0x0014B228
		private int decryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			uint num3 = 3337565984U;
			for (int num4 = 0; num4 != 32; num4++)
			{
				num2 -= ((num << 4) + this._c ^ num + num3 ^ (num >> 5) + this._d);
				num -= ((num2 << 4) + this._a ^ num2 + num3 ^ (num2 >> 5) + this._b);
				num3 -= 2654435769U;
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x04002345 RID: 9029
		private const int rounds = 32;

		// Token: 0x04002346 RID: 9030
		private const int block_size = 8;

		// Token: 0x04002347 RID: 9031
		private const uint delta = 2654435769U;

		// Token: 0x04002348 RID: 9032
		private const uint d_sum = 3337565984U;

		// Token: 0x04002349 RID: 9033
		private uint _a;

		// Token: 0x0400234A RID: 9034
		private uint _b;

		// Token: 0x0400234B RID: 9035
		private uint _c;

		// Token: 0x0400234C RID: 9036
		private uint _d;

		// Token: 0x0400234D RID: 9037
		private bool _initialised;

		// Token: 0x0400234E RID: 9038
		private bool _forEncryption;
	}
}
