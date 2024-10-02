using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200049F RID: 1183
	public class HMacDsaKCalculator : IDsaKCalculator
	{
		// Token: 0x06002E68 RID: 11880 RVA: 0x001205F3 File Offset: 0x0011E7F3
		public HMacDsaKCalculator(IDigest digest)
		{
			this.hMac = new HMac(digest);
			this.V = new byte[this.hMac.GetMacSize()];
			this.K = new byte[this.hMac.GetMacSize()];
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06002E69 RID: 11881 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsDeterministic
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x00120633 File Offset: 0x0011E833
		public virtual void Init(BigInteger n, SecureRandom random)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x00120640 File Offset: 0x0011E840
		public void Init(BigInteger n, BigInteger d, byte[] message)
		{
			this.n = n;
			Arrays.Fill(this.V, 1);
			Arrays.Fill(this.K, 0);
			int unsignedByteLength = BigIntegers.GetUnsignedByteLength(n);
			byte[] array = new byte[unsignedByteLength];
			byte[] array2 = BigIntegers.AsUnsignedByteArray(d);
			Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
			byte[] array3 = new byte[unsignedByteLength];
			BigInteger bigInteger = this.BitsToInt(message);
			if (bigInteger.CompareTo(n) >= 0)
			{
				bigInteger = bigInteger.Subtract(n);
			}
			byte[] array4 = BigIntegers.AsUnsignedByteArray(bigInteger);
			Array.Copy(array4, 0, array3, array3.Length - array4.Length, array4.Length);
			this.hMac.Init(new KeyParameter(this.K));
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.Update(0);
			this.hMac.BlockUpdate(array, 0, array.Length);
			this.hMac.BlockUpdate(array3, 0, array3.Length);
			this.hMac.DoFinal(this.K, 0);
			this.hMac.Init(new KeyParameter(this.K));
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.DoFinal(this.V, 0);
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.Update(1);
			this.hMac.BlockUpdate(array, 0, array.Length);
			this.hMac.BlockUpdate(array3, 0, array3.Length);
			this.hMac.DoFinal(this.K, 0);
			this.hMac.Init(new KeyParameter(this.K));
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.DoFinal(this.V, 0);
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x00120820 File Offset: 0x0011EA20
		public virtual BigInteger NextK()
		{
			byte[] array = new byte[BigIntegers.GetUnsignedByteLength(this.n)];
			BigInteger bigInteger;
			for (;;)
			{
				int num;
				for (int i = 0; i < array.Length; i += num)
				{
					this.hMac.BlockUpdate(this.V, 0, this.V.Length);
					this.hMac.DoFinal(this.V, 0);
					num = Math.Min(array.Length - i, this.V.Length);
					Array.Copy(this.V, 0, array, i, num);
				}
				bigInteger = this.BitsToInt(array);
				if (bigInteger.SignValue > 0 && bigInteger.CompareTo(this.n) < 0)
				{
					break;
				}
				this.hMac.BlockUpdate(this.V, 0, this.V.Length);
				this.hMac.Update(0);
				this.hMac.DoFinal(this.K, 0);
				this.hMac.Init(new KeyParameter(this.K));
				this.hMac.BlockUpdate(this.V, 0, this.V.Length);
				this.hMac.DoFinal(this.V, 0);
			}
			return bigInteger;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x00120940 File Offset: 0x0011EB40
		private BigInteger BitsToInt(byte[] t)
		{
			BigInteger bigInteger = new BigInteger(1, t);
			if (t.Length * 8 > this.n.BitLength)
			{
				bigInteger = bigInteger.ShiftRight(t.Length * 8 - this.n.BitLength);
			}
			return bigInteger;
		}

		// Token: 0x04001EF0 RID: 7920
		private readonly HMac hMac;

		// Token: 0x04001EF1 RID: 7921
		private readonly byte[] K;

		// Token: 0x04001EF2 RID: 7922
		private readonly byte[] V;

		// Token: 0x04001EF3 RID: 7923
		private BigInteger n;
	}
}
