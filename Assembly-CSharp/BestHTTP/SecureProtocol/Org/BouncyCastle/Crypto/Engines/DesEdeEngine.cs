using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000572 RID: 1394
	public class DesEdeEngine : DesEngine
	{
		// Token: 0x06003471 RID: 13425 RVA: 0x0013F8C4 File Offset: 0x0013DAC4
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to DESede init - " + Platform.GetTypeName(parameters));
			}
			byte[] key = ((KeyParameter)parameters).GetKey();
			if (key.Length != 24 && key.Length != 16)
			{
				throw new ArgumentException("key size must be 16 or 24 bytes.");
			}
			this.forEncryption = forEncryption;
			byte[] array = new byte[8];
			Array.Copy(key, 0, array, 0, array.Length);
			this.workingKey1 = DesEngine.GenerateWorkingKey(forEncryption, array);
			byte[] array2 = new byte[8];
			Array.Copy(key, 8, array2, 0, array2.Length);
			this.workingKey2 = DesEngine.GenerateWorkingKey(!forEncryption, array2);
			if (key.Length == 24)
			{
				byte[] array3 = new byte[8];
				Array.Copy(key, 16, array3, 0, array3.Length);
				this.workingKey3 = DesEngine.GenerateWorkingKey(forEncryption, array3);
				return;
			}
			this.workingKey3 = this.workingKey1;
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06003472 RID: 13426 RVA: 0x0013F993 File Offset: 0x0013DB93
		public override string AlgorithmName
		{
			get
			{
				return "DESede";
			}
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public override int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x0013F99C File Offset: 0x0013DB9C
		public override int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey1 == null)
			{
				throw new InvalidOperationException("DESede engine not initialised");
			}
			Check.DataLength(input, inOff, 8, "input buffer too short");
			Check.OutputLength(output, outOff, 8, "output buffer too short");
			byte[] array = new byte[8];
			if (this.forEncryption)
			{
				DesEngine.DesFunc(this.workingKey1, input, inOff, array, 0);
				DesEngine.DesFunc(this.workingKey2, array, 0, array, 0);
				DesEngine.DesFunc(this.workingKey3, array, 0, output, outOff);
			}
			else
			{
				DesEngine.DesFunc(this.workingKey3, input, inOff, array, 0);
				DesEngine.DesFunc(this.workingKey2, array, 0, array, 0);
				DesEngine.DesFunc(this.workingKey1, array, 0, output, outOff);
			}
			return 8;
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x0000248C File Offset: 0x0000068C
		public override void Reset()
		{
		}

		// Token: 0x0400224D RID: 8781
		private int[] workingKey1;

		// Token: 0x0400224E RID: 8782
		private int[] workingKey2;

		// Token: 0x0400224F RID: 8783
		private int[] workingKey3;

		// Token: 0x04002250 RID: 8784
		private bool forEncryption;
	}
}
