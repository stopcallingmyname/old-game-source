using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc7748;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000507 RID: 1287
	public sealed class X448PrivateKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x060030DF RID: 12511 RVA: 0x0012844C File Offset: 0x0012664C
		public X448PrivateKeyParameters(SecureRandom random) : base(true)
		{
			X448.GeneratePrivateKey(random, this.data);
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x00128471 File Offset: 0x00126671
		public X448PrivateKeyParameters(byte[] buf, int off) : base(true)
		{
			Array.Copy(buf, off, this.data, 0, X448PrivateKeyParameters.KeySize);
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x0012849D File Offset: 0x0012669D
		public X448PrivateKeyParameters(Stream input) : base(true)
		{
			if (X448PrivateKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of X448 private key");
			}
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x001284D4 File Offset: 0x001266D4
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, X448PrivateKeyParameters.KeySize);
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x001284E9 File Offset: 0x001266E9
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x001284F8 File Offset: 0x001266F8
		public X448PublicKeyParameters GeneratePublicKey()
		{
			byte[] array = new byte[56];
			X448.GeneratePublicKey(this.data, 0, array, 0);
			return new X448PublicKeyParameters(array, 0);
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x00128524 File Offset: 0x00126724
		public void GenerateSecret(X448PublicKeyParameters publicKey, byte[] buf, int off)
		{
			byte[] array = new byte[56];
			publicKey.Encode(array, 0);
			if (!X448.CalculateAgreement(this.data, 0, array, 0, buf, off))
			{
				throw new InvalidOperationException("X448 agreement failed");
			}
		}

		// Token: 0x04002049 RID: 8265
		public static readonly int KeySize = 56;

		// Token: 0x0400204A RID: 8266
		public static readonly int SecretSize = 56;

		// Token: 0x0400204B RID: 8267
		private readonly byte[] data = new byte[X448PrivateKeyParameters.KeySize];
	}
}
