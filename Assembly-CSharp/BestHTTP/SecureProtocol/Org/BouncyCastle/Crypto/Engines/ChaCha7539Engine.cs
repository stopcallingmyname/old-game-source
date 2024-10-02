using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000570 RID: 1392
	public class ChaCha7539Engine : Salsa20Engine
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x0013F35D File Offset: 0x0013D55D
		public override string AlgorithmName
		{
			get
			{
				return "ChaCha7539" + this.rounds;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x00104DE1 File Offset: 0x00102FE1
		protected override int NonceSize
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x0013F374 File Offset: 0x0013D574
		protected override void AdvanceCounter()
		{
			uint[] engineState = this.engineState;
			int num = 12;
			uint num2 = engineState[num] + 1U;
			engineState[num] = num2;
			if (num2 == 0U)
			{
				throw new InvalidOperationException("attempt to increase counter past 2^32.");
			}
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x0013F3A3 File Offset: 0x0013D5A3
		protected override void ResetCounter()
		{
			this.engineState[12] = 0U;
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x0013F3B0 File Offset: 0x0013D5B0
		protected override void SetKey(byte[] keyBytes, byte[] ivBytes)
		{
			if (keyBytes != null)
			{
				if (keyBytes.Length != 32)
				{
					throw new ArgumentException(this.AlgorithmName + " requires 256 bit key");
				}
				base.PackTauOrSigma(keyBytes.Length, this.engineState, 0);
				Pack.LE_To_UInt32(keyBytes, 0, this.engineState, 4, 8);
			}
			Pack.LE_To_UInt32(ivBytes, 0, this.engineState, 13, 3);
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x0013F40C File Offset: 0x0013D60C
		protected override void GenerateKeyStream(byte[] output)
		{
			ChaChaEngine.ChachaCore(this.rounds, this.engineState, this.x);
			Pack.UInt32_To_LE(this.x, output, 0);
		}
	}
}
