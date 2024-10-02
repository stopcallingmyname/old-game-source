using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A7 RID: 1191
	public class RandomDsaKCalculator : IDsaKCalculator
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsDeterministic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x001226F7 File Offset: 0x001208F7
		public virtual void Init(BigInteger n, SecureRandom random)
		{
			this.q = n;
			this.random = random;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x00120633 File Offset: 0x0011E833
		public virtual void Init(BigInteger n, BigInteger d, byte[] message)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x00122708 File Offset: 0x00120908
		public virtual BigInteger NextK()
		{
			int bitLength = this.q.BitLength;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(bitLength, this.random);
			}
			while (bigInteger.SignValue < 1 || bigInteger.CompareTo(this.q) >= 0);
			return bigInteger;
		}

		// Token: 0x04001F3C RID: 7996
		private BigInteger q;

		// Token: 0x04001F3D RID: 7997
		private SecureRandom random;
	}
}
