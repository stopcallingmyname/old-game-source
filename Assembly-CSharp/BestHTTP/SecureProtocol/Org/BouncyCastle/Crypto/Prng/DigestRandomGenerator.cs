using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004AF RID: 1199
	public class DigestRandomGenerator : IRandomGenerator
	{
		// Token: 0x06002EF4 RID: 12020 RVA: 0x00123578 File Offset: 0x00121778
		public DigestRandomGenerator(IDigest digest)
		{
			this.digest = digest;
			this.seed = new byte[digest.GetDigestSize()];
			this.seedCounter = 1L;
			this.state = new byte[digest.GetDigestSize()];
			this.stateCounter = 1L;
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x001235C4 File Offset: 0x001217C4
		public void AddSeedMaterial(byte[] inSeed)
		{
			lock (this)
			{
				this.DigestUpdate(inSeed);
				this.DigestUpdate(this.seed);
				this.DigestDoFinal(this.seed);
			}
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x00123618 File Offset: 0x00121818
		public void AddSeedMaterial(long rSeed)
		{
			lock (this)
			{
				this.DigestAddCounter(rSeed);
				this.DigestUpdate(this.seed);
				this.DigestDoFinal(this.seed);
			}
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x0012366C File Offset: 0x0012186C
		public void NextBytes(byte[] bytes)
		{
			this.NextBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x0012367C File Offset: 0x0012187C
		public void NextBytes(byte[] bytes, int start, int len)
		{
			lock (this)
			{
				int num = 0;
				this.GenerateState();
				int num2 = start + len;
				for (int i = start; i < num2; i++)
				{
					if (num == this.state.Length)
					{
						this.GenerateState();
						num = 0;
					}
					bytes[i] = this.state[num++];
				}
			}
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x001236F0 File Offset: 0x001218F0
		private void CycleSeed()
		{
			this.DigestUpdate(this.seed);
			long num = this.seedCounter;
			this.seedCounter = num + 1L;
			this.DigestAddCounter(num);
			this.DigestDoFinal(this.seed);
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x00123730 File Offset: 0x00121930
		private void GenerateState()
		{
			long num = this.stateCounter;
			this.stateCounter = num + 1L;
			this.DigestAddCounter(num);
			this.DigestUpdate(this.state);
			this.DigestUpdate(this.seed);
			this.DigestDoFinal(this.state);
			if (this.stateCounter % 10L == 0L)
			{
				this.CycleSeed();
			}
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x0012378C File Offset: 0x0012198C
		private void DigestAddCounter(long seedVal)
		{
			byte[] array = new byte[8];
			Pack.UInt64_To_LE((ulong)seedVal, array);
			this.digest.BlockUpdate(array, 0, array.Length);
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x001237B7 File Offset: 0x001219B7
		private void DigestUpdate(byte[] inSeed)
		{
			this.digest.BlockUpdate(inSeed, 0, inSeed.Length);
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x001237C9 File Offset: 0x001219C9
		private void DigestDoFinal(byte[] result)
		{
			this.digest.DoFinal(result, 0);
		}

		// Token: 0x04001F5F RID: 8031
		private const long CYCLE_COUNT = 10L;

		// Token: 0x04001F60 RID: 8032
		private long stateCounter;

		// Token: 0x04001F61 RID: 8033
		private long seedCounter;

		// Token: 0x04001F62 RID: 8034
		private IDigest digest;

		// Token: 0x04001F63 RID: 8035
		private byte[] state;

		// Token: 0x04001F64 RID: 8036
		private byte[] seed;
	}
}
