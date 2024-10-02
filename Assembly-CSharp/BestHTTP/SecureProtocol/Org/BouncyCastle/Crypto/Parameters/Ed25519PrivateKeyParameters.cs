using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D7 RID: 1239
	public sealed class Ed25519PrivateKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06002FFF RID: 12287 RVA: 0x00126A76 File Offset: 0x00124C76
		public Ed25519PrivateKeyParameters(SecureRandom random) : base(true)
		{
			Ed25519.GeneratePrivateKey(random, this.data);
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x00126A9B File Offset: 0x00124C9B
		public Ed25519PrivateKeyParameters(byte[] buf, int off) : base(true)
		{
			Array.Copy(buf, off, this.data, 0, Ed25519PrivateKeyParameters.KeySize);
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x00126AC7 File Offset: 0x00124CC7
		public Ed25519PrivateKeyParameters(Stream input) : base(true)
		{
			if (Ed25519PrivateKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of Ed25519 private key");
			}
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x00126AFE File Offset: 0x00124CFE
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, Ed25519PrivateKeyParameters.KeySize);
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x00126B13 File Offset: 0x00124D13
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x00126B20 File Offset: 0x00124D20
		public Ed25519PublicKeyParameters GeneratePublicKey()
		{
			byte[] array = new byte[Ed25519.PublicKeySize];
			Ed25519.GeneratePublicKey(this.data, 0, array, 0);
			return new Ed25519PublicKeyParameters(array, 0);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x00126B50 File Offset: 0x00124D50
		public void Sign(Ed25519.Algorithm algorithm, Ed25519PublicKeyParameters publicKey, byte[] ctx, byte[] msg, int msgOff, int msgLen, byte[] sig, int sigOff)
		{
			byte[] array = new byte[Ed25519.PublicKeySize];
			if (publicKey == null)
			{
				Ed25519.GeneratePublicKey(this.data, 0, array, 0);
			}
			else
			{
				publicKey.Encode(array, 0);
			}
			switch (algorithm)
			{
			case Ed25519.Algorithm.Ed25519:
				if (ctx != null)
				{
					throw new ArgumentException("ctx");
				}
				Ed25519.Sign(this.data, 0, array, 0, msg, msgOff, msgLen, sig, sigOff);
				return;
			case Ed25519.Algorithm.Ed25519ctx:
				Ed25519.Sign(this.data, 0, array, 0, ctx, msg, msgOff, msgLen, sig, sigOff);
				return;
			case Ed25519.Algorithm.Ed25519ph:
				if (Ed25519.PrehashSize != msgLen)
				{
					throw new ArgumentException("msgLen");
				}
				Ed25519.SignPrehash(this.data, 0, array, 0, ctx, msg, msgOff, sig, sigOff);
				return;
			default:
				throw new ArgumentException("algorithm");
			}
		}

		// Token: 0x04001FDF RID: 8159
		public static readonly int KeySize = Ed25519.SecretKeySize;

		// Token: 0x04001FE0 RID: 8160
		public static readonly int SignatureSize = Ed25519.SignatureSize;

		// Token: 0x04001FE1 RID: 8161
		private readonly byte[] data = new byte[Ed25519PrivateKeyParameters.KeySize];
	}
}
