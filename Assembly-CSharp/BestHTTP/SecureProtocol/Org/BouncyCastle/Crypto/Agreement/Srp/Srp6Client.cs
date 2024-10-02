using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Srp
{
	// Token: 0x020005CE RID: 1486
	public class Srp6Client
	{
		// Token: 0x060038FD RID: 14589 RVA: 0x00165293 File Offset: 0x00163493
		public virtual void Init(BigInteger N, BigInteger g, IDigest digest, SecureRandom random)
		{
			this.N = N;
			this.g = g;
			this.digest = digest;
			this.random = random;
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x001652B2 File Offset: 0x001634B2
		public virtual void Init(Srp6GroupParameters group, IDigest digest, SecureRandom random)
		{
			this.Init(group.N, group.G, digest, random);
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x001652C8 File Offset: 0x001634C8
		public virtual BigInteger GenerateClientCredentials(byte[] salt, byte[] identity, byte[] password)
		{
			this.x = Srp6Utilities.CalculateX(this.digest, this.N, salt, identity, password);
			this.privA = this.SelectPrivateValue();
			this.pubA = this.g.ModPow(this.privA, this.N);
			return this.pubA;
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x00165320 File Offset: 0x00163520
		public virtual BigInteger CalculateSecret(BigInteger serverB)
		{
			this.B = Srp6Utilities.ValidatePublicValue(this.N, serverB);
			this.u = Srp6Utilities.CalculateU(this.digest, this.N, this.pubA, this.B);
			this.S = this.CalculateS();
			return this.S;
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x00165374 File Offset: 0x00163574
		protected virtual BigInteger SelectPrivateValue()
		{
			return Srp6Utilities.GeneratePrivateValue(this.digest, this.N, this.g, this.random);
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x00165394 File Offset: 0x00163594
		private BigInteger CalculateS()
		{
			BigInteger val = Srp6Utilities.CalculateK(this.digest, this.N, this.g);
			BigInteger e = this.u.Multiply(this.x).Add(this.privA);
			BigInteger n = this.g.ModPow(this.x, this.N).Multiply(val).Mod(this.N);
			return this.B.Subtract(n).Mod(this.N).ModPow(e, this.N);
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x00165424 File Offset: 0x00163624
		public virtual BigInteger CalculateClientEvidenceMessage()
		{
			if (this.pubA == null || this.B == null || this.S == null)
			{
				throw new CryptoException("Impossible to compute M1: some data are missing from the previous operations (A,B,S)");
			}
			this.M1 = Srp6Utilities.CalculateM1(this.digest, this.N, this.pubA, this.B, this.S);
			return this.M1;
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x00165484 File Offset: 0x00163684
		public virtual bool VerifyServerEvidenceMessage(BigInteger serverM2)
		{
			if (this.pubA == null || this.M1 == null || this.S == null)
			{
				throw new CryptoException("Impossible to compute and verify M2: some data are missing from the previous operations (A,M1,S)");
			}
			if (Srp6Utilities.CalculateM2(this.digest, this.N, this.pubA, this.M1, this.S).Equals(serverM2))
			{
				this.M2 = serverM2;
				return true;
			}
			return false;
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x001654EC File Offset: 0x001636EC
		public virtual BigInteger CalculateSessionKey()
		{
			if (this.S == null || this.M1 == null || this.M2 == null)
			{
				throw new CryptoException("Impossible to compute Key: some data are missing from the previous operations (S,M1,M2)");
			}
			this.Key = Srp6Utilities.CalculateKey(this.digest, this.N, this.S);
			return this.Key;
		}

		// Token: 0x04002545 RID: 9541
		protected BigInteger N;

		// Token: 0x04002546 RID: 9542
		protected BigInteger g;

		// Token: 0x04002547 RID: 9543
		protected BigInteger privA;

		// Token: 0x04002548 RID: 9544
		protected BigInteger pubA;

		// Token: 0x04002549 RID: 9545
		protected BigInteger B;

		// Token: 0x0400254A RID: 9546
		protected BigInteger x;

		// Token: 0x0400254B RID: 9547
		protected BigInteger u;

		// Token: 0x0400254C RID: 9548
		protected BigInteger S;

		// Token: 0x0400254D RID: 9549
		protected BigInteger M1;

		// Token: 0x0400254E RID: 9550
		protected BigInteger M2;

		// Token: 0x0400254F RID: 9551
		protected BigInteger Key;

		// Token: 0x04002550 RID: 9552
		protected IDigest digest;

		// Token: 0x04002551 RID: 9553
		protected SecureRandom random;
	}
}
