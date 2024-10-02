using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200059C RID: 1436
	public class XSalsa20Engine : Salsa20Engine
	{
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06003666 RID: 13926 RVA: 0x00150F82 File Offset: 0x0014F182
		public override string AlgorithmName
		{
			get
			{
				return "XSalsa20";
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06003667 RID: 13927 RVA: 0x00150F89 File Offset: 0x0014F189
		protected override int NonceSize
		{
			get
			{
				return 24;
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00150F90 File Offset: 0x0014F190
		protected override void SetKey(byte[] keyBytes, byte[] ivBytes)
		{
			if (keyBytes == null)
			{
				throw new ArgumentException(this.AlgorithmName + " doesn't support re-init with null key");
			}
			if (keyBytes.Length != 32)
			{
				throw new ArgumentException(this.AlgorithmName + " requires a 256 bit key");
			}
			base.SetKey(keyBytes, ivBytes);
			Pack.LE_To_UInt32(ivBytes, 8, this.engineState, 8, 2);
			uint[] array = new uint[this.engineState.Length];
			Salsa20Engine.SalsaCore(20, this.engineState, array);
			this.engineState[1] = array[0] - this.engineState[0];
			this.engineState[2] = array[5] - this.engineState[5];
			this.engineState[3] = array[10] - this.engineState[10];
			this.engineState[4] = array[15] - this.engineState[15];
			this.engineState[11] = array[6] - this.engineState[6];
			this.engineState[12] = array[7] - this.engineState[7];
			this.engineState[13] = array[8] - this.engineState[8];
			this.engineState[14] = array[9] - this.engineState[9];
			Pack.LE_To_UInt32(ivBytes, 16, this.engineState, 6, 2);
		}
	}
}
