using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003C7 RID: 967
	public abstract class BufferedCipherBase : IBufferedCipher
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060027DA RID: 10202
		public abstract string AlgorithmName { get; }

		// Token: 0x060027DB RID: 10203
		public abstract void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x060027DC RID: 10204
		public abstract int GetBlockSize();

		// Token: 0x060027DD RID: 10205
		public abstract int GetOutputSize(int inputLen);

		// Token: 0x060027DE RID: 10206
		public abstract int GetUpdateOutputSize(int inputLen);

		// Token: 0x060027DF RID: 10207
		public abstract byte[] ProcessByte(byte input);

		// Token: 0x060027E0 RID: 10208 RVA: 0x0010C668 File Offset: 0x0010A868
		public virtual int ProcessByte(byte input, byte[] output, int outOff)
		{
			byte[] array = this.ProcessByte(input);
			if (array == null)
			{
				return 0;
			}
			if (outOff + array.Length > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			array.CopyTo(output, outOff);
			return array.Length;
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x0010C6A2 File Offset: 0x0010A8A2
		public virtual byte[] ProcessBytes(byte[] input)
		{
			return this.ProcessBytes(input, 0, input.Length);
		}

		// Token: 0x060027E2 RID: 10210
		public abstract byte[] ProcessBytes(byte[] input, int inOff, int length);

		// Token: 0x060027E3 RID: 10211 RVA: 0x0010C6AF File Offset: 0x0010A8AF
		public virtual int ProcessBytes(byte[] input, byte[] output, int outOff)
		{
			return this.ProcessBytes(input, 0, input.Length, output, outOff);
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x0010C6C0 File Offset: 0x0010A8C0
		public virtual int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			byte[] array = this.ProcessBytes(input, inOff, length);
			if (array == null)
			{
				return 0;
			}
			if (outOff + array.Length > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			array.CopyTo(output, outOff);
			return array.Length;
		}

		// Token: 0x060027E5 RID: 10213
		public abstract byte[] DoFinal();

		// Token: 0x060027E6 RID: 10214 RVA: 0x0010C700 File Offset: 0x0010A900
		public virtual byte[] DoFinal(byte[] input)
		{
			return this.DoFinal(input, 0, input.Length);
		}

		// Token: 0x060027E7 RID: 10215
		public abstract byte[] DoFinal(byte[] input, int inOff, int length);

		// Token: 0x060027E8 RID: 10216 RVA: 0x0010C710 File Offset: 0x0010A910
		public virtual int DoFinal(byte[] output, int outOff)
		{
			byte[] array = this.DoFinal();
			if (outOff + array.Length > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			array.CopyTo(output, outOff);
			return array.Length;
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x0010C744 File Offset: 0x0010A944
		public virtual int DoFinal(byte[] input, byte[] output, int outOff)
		{
			return this.DoFinal(input, 0, input.Length, output, outOff);
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x0010C754 File Offset: 0x0010A954
		public virtual int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			int num = this.ProcessBytes(input, inOff, length, output, outOff);
			return num + this.DoFinal(output, outOff + num);
		}

		// Token: 0x060027EB RID: 10219
		public abstract void Reset();

		// Token: 0x04001AFD RID: 6909
		protected static readonly byte[] EmptyBuffer = new byte[0];
	}
}
