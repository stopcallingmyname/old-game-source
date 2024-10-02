using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Srp
{
	// Token: 0x020005CF RID: 1487
	public class Srp6Server
	{
		// Token: 0x06003907 RID: 14599 RVA: 0x0016553F File Offset: 0x0016373F
		public virtual void Init(BigInteger N, BigInteger g, BigInteger v, IDigest digest, SecureRandom random)
		{
			this.N = N;
			this.g = g;
			this.v = v;
			this.random = random;
			this.digest = digest;
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x00165566 File Offset: 0x00163766
		public virtual void Init(Srp6GroupParameters group, BigInteger v, IDigest digest, SecureRandom random)
		{
			this.Init(group.N, group.G, v, digest, random);
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x00165580 File Offset: 0x00163780
		public virtual BigInteger GenerateServerCredentials()
		{
			BigInteger bigInteger = Srp6Utilities.CalculateK(this.digest, this.N, this.g);
			this.privB = this.SelectPrivateValue();
			this.pubB = bigInteger.Multiply(this.v).Mod(this.N).Add(this.g.ModPow(this.privB, this.N)).Mod(this.N);
			return this.pubB;
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x001655FC File Offset: 0x001637FC
		public virtual BigInteger CalculateSecret(BigInteger clientA)
		{
			this.A = Srp6Utilities.ValidatePublicValue(this.N, clientA);
			this.u = Srp6Utilities.CalculateU(this.digest, this.N, this.A, this.pubB);
			this.S = this.CalculateS();
			return this.S;
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x00165650 File Offset: 0x00163850
		protected virtual BigInteger SelectPrivateValue()
		{
			return Srp6Utilities.GeneratePrivateValue(this.digest, this.N, this.g, this.random);
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0016566F File Offset: 0x0016386F
		private BigInteger CalculateS()
		{
			return this.v.ModPow(this.u, this.N).Multiply(this.A).Mod(this.N).ModPow(this.privB, this.N);
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x001656B0 File Offset: 0x001638B0
		public virtual bool VerifyClientEvidenceMessage(BigInteger clientM1)
		{
			if (this.A == null || this.pubB == null || this.S == null)
			{
				throw new CryptoException("Impossible to compute and verify M1: some data are missing from the previous operations (A,B,S)");
			}
			if (Srp6Utilities.CalculateM1(this.digest, this.N, this.A, this.pubB, this.S).Equals(clientM1))
			{
				this.M1 = clientM1;
				return true;
			}
			return false;
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x00165718 File Offset: 0x00163918
		public virtual BigInteger CalculateServerEvidenceMessage()
		{
			if (this.A == null || this.M1 == null || this.S == null)
			{
				throw new CryptoException("Impossible to compute M2: some data are missing from the previous operations (A,M1,S)");
			}
			this.M2 = Srp6Utilities.CalculateM2(this.digest, this.N, this.A, this.M1, this.S);
			return this.M2;
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x00165778 File Offset: 0x00163978
		public virtual BigInteger CalculateSessionKey()
		{
			if (this.S == null || this.M1 == null || this.M2 == null)
			{
				throw new CryptoException("Impossible to compute Key: some data are missing from the previous operations (S,M1,M2)");
			}
			this.Key = Srp6Utilities.CalculateKey(this.digest, this.N, this.S);
			return this.Key;
		}

		// Token: 0x04002552 RID: 9554
		protected BigInteger N;

		// Token: 0x04002553 RID: 9555
		protected BigInteger g;

		// Token: 0x04002554 RID: 9556
		protected BigInteger v;

		// Token: 0x04002555 RID: 9557
		protected SecureRandom random;

		// Token: 0x04002556 RID: 9558
		protected IDigest digest;

		// Token: 0x04002557 RID: 9559
		protected BigInteger A;

		// Token: 0x04002558 RID: 9560
		protected BigInteger privB;

		// Token: 0x04002559 RID: 9561
		protected BigInteger pubB;

		// Token: 0x0400255A RID: 9562
		protected BigInteger u;

		// Token: 0x0400255B RID: 9563
		protected BigInteger S;

		// Token: 0x0400255C RID: 9564
		protected BigInteger M1;

		// Token: 0x0400255D RID: 9565
		protected BigInteger M2;

		// Token: 0x0400255E RID: 9566
		protected BigInteger Key;
	}
}
