using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc7748;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000504 RID: 1284
	public sealed class X25519PrivateKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x060030D1 RID: 12497 RVA: 0x0012829E File Offset: 0x0012649E
		public X25519PrivateKeyParameters(SecureRandom random) : base(true)
		{
			X25519.GeneratePrivateKey(random, this.data);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x001282C3 File Offset: 0x001264C3
		public X25519PrivateKeyParameters(byte[] buf, int off) : base(true)
		{
			Array.Copy(buf, off, this.data, 0, X25519PrivateKeyParameters.KeySize);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x001282EF File Offset: 0x001264EF
		public X25519PrivateKeyParameters(Stream input) : base(true)
		{
			if (X25519PrivateKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of X25519 private key");
			}
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x00128326 File Offset: 0x00126526
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, X25519PrivateKeyParameters.KeySize);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x0012833B File Offset: 0x0012653B
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x00128348 File Offset: 0x00126548
		public X25519PublicKeyParameters GeneratePublicKey()
		{
			byte[] array = new byte[32];
			X25519.GeneratePublicKey(this.data, 0, array, 0);
			return new X25519PublicKeyParameters(array, 0);
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x00128374 File Offset: 0x00126574
		public void GenerateSecret(X25519PublicKeyParameters publicKey, byte[] buf, int off)
		{
			byte[] array = new byte[32];
			publicKey.Encode(array, 0);
			if (!X25519.CalculateAgreement(this.data, 0, array, 0, buf, off))
			{
				throw new InvalidOperationException("X25519 agreement failed");
			}
		}

		// Token: 0x04002044 RID: 8260
		public static readonly int KeySize = 32;

		// Token: 0x04002045 RID: 8261
		public static readonly int SecretSize = 32;

		// Token: 0x04002046 RID: 8262
		private readonly byte[] data = new byte[X25519PrivateKeyParameters.KeySize];
	}
}
