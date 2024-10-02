using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000592 RID: 1426
	public abstract class SerpentEngineBase : IBlockCipher
	{
		// Token: 0x060035DF RID: 13791 RVA: 0x0014B7C0 File Offset: 0x001499C0
		public virtual void Init(bool encrypting, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to " + this.AlgorithmName + " init - " + Platform.GetTypeName(parameters));
			}
			this.encrypting = encrypting;
			this.wKey = this.MakeWorkingKey(((KeyParameter)parameters).GetKey());
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060035E0 RID: 13792 RVA: 0x0014B814 File Offset: 0x00149A14
		public virtual string AlgorithmName
		{
			get
			{
				return "Serpent";
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060035E1 RID: 13793 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x0014B81B File Offset: 0x00149A1B
		public virtual int GetBlockSize()
		{
			return SerpentEngineBase.BlockSize;
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0014B824 File Offset: 0x00149A24
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.wKey == null)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(input, inOff, SerpentEngineBase.BlockSize, "input buffer too short");
			Check.OutputLength(output, outOff, SerpentEngineBase.BlockSize, "output buffer too short");
			if (this.encrypting)
			{
				this.EncryptBlock(input, inOff, output, outOff);
			}
			else
			{
				this.DecryptBlock(input, inOff, output, outOff);
			}
			return SerpentEngineBase.BlockSize;
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x00148EDA File Offset: 0x001470DA
		protected static int RotateLeft(int x, int bits)
		{
			return x << bits | (int)((uint)x >> 32 - bits);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x00137301 File Offset: 0x00135501
		private static int RotateRight(int x, int bits)
		{
			return (int)((uint)x >> bits | (uint)((uint)x << 32 - bits));
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x0014B898 File Offset: 0x00149A98
		protected void Sb0(int a, int b, int c, int d)
		{
			int num = a ^ d;
			int num2 = c ^ num;
			int num3 = b ^ num2;
			this.X3 = ((a & d) ^ num3);
			int num4 = a ^ (b & num);
			this.X2 = (num3 ^ (c | num4));
			int num5 = this.X3 & (num2 ^ num4);
			this.X1 = (~num2 ^ num5);
			this.X0 = (num5 ^ ~num4);
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x0014B8F4 File Offset: 0x00149AF4
		protected void Ib0(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ b;
			int num3 = d ^ (num | num2);
			int num4 = c ^ num3;
			this.X2 = (num2 ^ num4);
			int num5 = num ^ (d & num2);
			this.X1 = (num3 ^ (this.X2 & num5));
			this.X3 = ((a & num3) ^ (num4 | this.X1));
			this.X0 = (this.X3 ^ (num4 ^ num5));
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x0014B958 File Offset: 0x00149B58
		protected void Sb1(int a, int b, int c, int d)
		{
			int num = b ^ ~a;
			int num2 = c ^ (a | num);
			this.X2 = (d ^ num2);
			int num3 = b ^ (d | num);
			int num4 = num ^ this.X2;
			this.X3 = (num4 ^ (num2 & num3));
			int num5 = num2 ^ num3;
			this.X1 = (this.X3 ^ num5);
			this.X0 = (num2 ^ (num4 & num5));
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x0014B9B8 File Offset: 0x00149BB8
		protected void Ib1(int a, int b, int c, int d)
		{
			int num = b ^ d;
			int num2 = a ^ (b & num);
			int num3 = num ^ num2;
			this.X3 = (c ^ num3);
			int num4 = b ^ (num & num2);
			int num5 = this.X3 | num4;
			this.X1 = (num2 ^ num5);
			int num6 = ~this.X1;
			int num7 = this.X3 ^ num4;
			this.X0 = (num6 ^ num7);
			this.X2 = (num3 ^ (num6 | num7));
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x0014BA24 File Offset: 0x00149C24
		protected void Sb2(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = b ^ d;
			int num3 = c & num;
			this.X0 = (num2 ^ num3);
			int num4 = c ^ num;
			int num5 = c ^ this.X0;
			int num6 = b & num5;
			this.X3 = (num4 ^ num6);
			this.X2 = (a ^ ((d | num6) & (this.X0 | num4)));
			this.X1 = (num2 ^ this.X3 ^ (this.X2 ^ (d | num)));
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x0014BA94 File Offset: 0x00149C94
		protected void Ib2(int a, int b, int c, int d)
		{
			int num = b ^ d;
			int num2 = ~num;
			int num3 = a ^ c;
			int num4 = c ^ num;
			int num5 = b & num4;
			this.X0 = (num3 ^ num5);
			int num6 = a | num2;
			int num7 = d ^ num6;
			int num8 = num3 | num7;
			this.X3 = (num ^ num8);
			int num9 = ~num4;
			int num10 = this.X0 | this.X3;
			this.X1 = (num9 ^ num10);
			this.X2 = ((d & num9) ^ (num3 ^ num10));
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0014BB0C File Offset: 0x00149D0C
		protected void Sb3(int a, int b, int c, int d)
		{
			int num = a ^ b;
			int num2 = a & c;
			int num3 = a | d;
			int num4 = c ^ d;
			int num5 = num & num3;
			int num6 = num2 | num5;
			this.X2 = (num4 ^ num6);
			int num7 = b ^ num3;
			int num8 = num6 ^ num7;
			int num9 = num4 & num8;
			this.X0 = (num ^ num9);
			int num10 = this.X2 & this.X0;
			this.X1 = (num8 ^ num10);
			this.X3 = ((b | d) ^ (num4 ^ num10));
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x0014BB84 File Offset: 0x00149D84
		protected void Ib3(int a, int b, int c, int d)
		{
			int num = a | b;
			int num2 = b ^ c;
			int num3 = b & num2;
			int num4 = a ^ num3;
			int num5 = c ^ num4;
			int num6 = d | num4;
			this.X0 = (num2 ^ num6);
			int num7 = num2 | num6;
			int num8 = d ^ num7;
			this.X2 = (num5 ^ num8);
			int num9 = num ^ num8;
			int num10 = this.X0 & num9;
			this.X3 = (num4 ^ num10);
			this.X1 = (this.X3 ^ (this.X0 ^ num9));
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x0014BBFC File Offset: 0x00149DFC
		protected void Sb4(int a, int b, int c, int d)
		{
			int num = a ^ d;
			int num2 = d & num;
			int num3 = c ^ num2;
			int num4 = b | num3;
			this.X3 = (num ^ num4);
			int num5 = ~b;
			int num6 = num | num5;
			this.X0 = (num3 ^ num6);
			int num7 = a & this.X0;
			int num8 = num ^ num5;
			int num9 = num4 & num8;
			this.X2 = (num7 ^ num9);
			this.X1 = (a ^ num3 ^ (num8 & this.X2));
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x0014BC6C File Offset: 0x00149E6C
		protected void Ib4(int a, int b, int c, int d)
		{
			int num = c | d;
			int num2 = a & num;
			int num3 = b ^ num2;
			int num4 = a & num3;
			int num5 = c ^ num4;
			this.X1 = (d ^ num5);
			int num6 = ~a;
			int num7 = num5 & this.X1;
			this.X3 = (num3 ^ num7);
			int num8 = this.X1 | num6;
			int num9 = d ^ num8;
			this.X0 = (this.X3 ^ num9);
			this.X2 = ((num3 & num9) ^ (this.X1 ^ num6));
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x0014BCE8 File Offset: 0x00149EE8
		protected void Sb5(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ b;
			int num3 = a ^ d;
			int num4 = c ^ num;
			int num5 = num2 | num3;
			this.X0 = (num4 ^ num5);
			int num6 = d & this.X0;
			int num7 = num2 ^ this.X0;
			this.X1 = (num6 ^ num7);
			int num8 = num | this.X0;
			int num9 = num2 | num6;
			int num10 = num3 ^ num8;
			this.X2 = (num9 ^ num10);
			this.X3 = (b ^ num6 ^ (this.X1 & num10));
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x0014BD68 File Offset: 0x00149F68
		protected void Ib5(int a, int b, int c, int d)
		{
			int num = ~c;
			int num2 = b & num;
			int num3 = d ^ num2;
			int num4 = a & num3;
			int num5 = b ^ num;
			this.X3 = (num4 ^ num5);
			int num6 = b | this.X3;
			int num7 = a & num6;
			this.X1 = (num3 ^ num7);
			int num8 = a | d;
			int num9 = num ^ num6;
			this.X0 = (num8 ^ num9);
			this.X2 = ((b & num8) ^ (num4 | (a ^ c)));
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x0014BDD8 File Offset: 0x00149FD8
		protected void Sb6(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ d;
			int num3 = b ^ num2;
			int num4 = num | num2;
			int num5 = c ^ num4;
			this.X1 = (b ^ num5);
			int num6 = num2 | this.X1;
			int num7 = d ^ num6;
			int num8 = num5 & num7;
			this.X2 = (num3 ^ num8);
			int num9 = num5 ^ num7;
			this.X0 = (this.X2 ^ num9);
			this.X3 = (~num5 ^ (num3 & num9));
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0014BE44 File Offset: 0x0014A044
		protected void Ib6(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ b;
			int num3 = c ^ num2;
			int num4 = c | num;
			int num5 = d ^ num4;
			this.X1 = (num3 ^ num5);
			int num6 = num3 & num5;
			int num7 = num2 ^ num6;
			int num8 = b | num7;
			this.X3 = (num5 ^ num8);
			int num9 = b | this.X3;
			this.X0 = (num7 ^ num9);
			this.X2 = ((d & num) ^ (num3 ^ num9));
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x0014BEB4 File Offset: 0x0014A0B4
		protected void Sb7(int a, int b, int c, int d)
		{
			int num = b ^ c;
			int num2 = c & num;
			int num3 = d ^ num2;
			int num4 = a ^ num3;
			int num5 = d | num;
			int num6 = num4 & num5;
			this.X1 = (b ^ num6);
			int num7 = num3 | this.X1;
			int num8 = a & num4;
			this.X3 = (num ^ num8);
			int num9 = num4 ^ num7;
			int num10 = this.X3 & num9;
			this.X2 = (num3 ^ num10);
			this.X0 = (~num9 ^ (this.X3 & this.X2));
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x0014BF34 File Offset: 0x0014A134
		protected void Ib7(int a, int b, int c, int d)
		{
			int num = c | (a & b);
			int num2 = d & (a | b);
			this.X3 = (num ^ num2);
			int num3 = ~d;
			int num4 = b ^ num2;
			int num5 = num4 | (this.X3 ^ num3);
			this.X1 = (a ^ num5);
			this.X0 = (c ^ num4 ^ (d | this.X1));
			this.X2 = (num ^ this.X1 ^ (this.X0 ^ (a & this.X3)));
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x0014BFA8 File Offset: 0x0014A1A8
		protected void LT()
		{
			int num = SerpentEngineBase.RotateLeft(this.X0, 13);
			int num2 = SerpentEngineBase.RotateLeft(this.X2, 3);
			int x = this.X1 ^ num ^ num2;
			int x2 = this.X3 ^ num2 ^ num << 3;
			this.X1 = SerpentEngineBase.RotateLeft(x, 1);
			this.X3 = SerpentEngineBase.RotateLeft(x2, 7);
			this.X0 = SerpentEngineBase.RotateLeft(num ^ this.X1 ^ this.X3, 5);
			this.X2 = SerpentEngineBase.RotateLeft(num2 ^ this.X3 ^ this.X1 << 7, 22);
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x0014C03C File Offset: 0x0014A23C
		protected void InverseLT()
		{
			int num = SerpentEngineBase.RotateRight(this.X2, 22) ^ this.X3 ^ this.X1 << 7;
			int num2 = SerpentEngineBase.RotateRight(this.X0, 5) ^ this.X1 ^ this.X3;
			int num3 = SerpentEngineBase.RotateRight(this.X3, 7);
			int num4 = SerpentEngineBase.RotateRight(this.X1, 1);
			this.X3 = (num3 ^ num ^ num2 << 3);
			this.X1 = (num4 ^ num2 ^ num);
			this.X2 = SerpentEngineBase.RotateRight(num, 3);
			this.X0 = SerpentEngineBase.RotateRight(num2, 13);
		}

		// Token: 0x060035F9 RID: 13817
		protected abstract int[] MakeWorkingKey(byte[] key);

		// Token: 0x060035FA RID: 13818
		protected abstract void EncryptBlock(byte[] input, int inOff, byte[] output, int outOff);

		// Token: 0x060035FB RID: 13819
		protected abstract void DecryptBlock(byte[] input, int inOff, byte[] output, int outOff);

		// Token: 0x0400232A RID: 9002
		protected static readonly int BlockSize = 16;

		// Token: 0x0400232B RID: 9003
		internal const int ROUNDS = 32;

		// Token: 0x0400232C RID: 9004
		internal const int PHI = -1640531527;

		// Token: 0x0400232D RID: 9005
		protected bool encrypting;

		// Token: 0x0400232E RID: 9006
		protected int[] wKey;

		// Token: 0x0400232F RID: 9007
		protected int X0;

		// Token: 0x04002330 RID: 9008
		protected int X1;

		// Token: 0x04002331 RID: 9009
		protected int X2;

		// Token: 0x04002332 RID: 9010
		protected int X3;
	}
}
