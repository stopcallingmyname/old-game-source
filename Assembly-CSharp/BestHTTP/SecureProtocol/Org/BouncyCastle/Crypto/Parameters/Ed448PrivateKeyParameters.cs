using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004DA RID: 1242
	public sealed class Ed448PrivateKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x0600300D RID: 12301 RVA: 0x00126CC3 File Offset: 0x00124EC3
		public Ed448PrivateKeyParameters(SecureRandom random) : base(true)
		{
			Ed448.GeneratePrivateKey(random, this.data);
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x00126CE8 File Offset: 0x00124EE8
		public Ed448PrivateKeyParameters(byte[] buf, int off) : base(true)
		{
			Array.Copy(buf, off, this.data, 0, Ed448PrivateKeyParameters.KeySize);
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00126D14 File Offset: 0x00124F14
		public Ed448PrivateKeyParameters(Stream input) : base(true)
		{
			if (Ed448PrivateKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of Ed448 private key");
			}
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x00126D4B File Offset: 0x00124F4B
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, Ed448PrivateKeyParameters.KeySize);
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x00126D60 File Offset: 0x00124F60
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x00126D70 File Offset: 0x00124F70
		public Ed448PublicKeyParameters GeneratePublicKey()
		{
			byte[] array = new byte[Ed448.PublicKeySize];
			Ed448.GeneratePublicKey(this.data, 0, array, 0);
			return new Ed448PublicKeyParameters(array, 0);
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x00126DA0 File Offset: 0x00124FA0
		public void Sign(Ed448.Algorithm algorithm, Ed448PublicKeyParameters publicKey, byte[] ctx, byte[] msg, int msgOff, int msgLen, byte[] sig, int sigOff)
		{
			byte[] array = new byte[Ed448.PublicKeySize];
			if (publicKey == null)
			{
				Ed448.GeneratePublicKey(this.data, 0, array, 0);
			}
			else
			{
				publicKey.Encode(array, 0);
			}
			if (algorithm == Ed448.Algorithm.Ed448)
			{
				Ed448.Sign(this.data, 0, array, 0, ctx, msg, msgOff, msgLen, sig, sigOff);
				return;
			}
			if (algorithm != Ed448.Algorithm.Ed448ph)
			{
				throw new ArgumentException("algorithm");
			}
			if (Ed448.PrehashSize != msgLen)
			{
				throw new ArgumentException("msgLen");
			}
			Ed448.SignPrehash(this.data, 0, array, 0, ctx, msg, msgOff, sig, sigOff);
		}

		// Token: 0x04001FE4 RID: 8164
		public static readonly int KeySize = Ed448.SecretKeySize;

		// Token: 0x04001FE5 RID: 8165
		public static readonly int SignatureSize = Ed448.SignatureSize;

		// Token: 0x04001FE6 RID: 8166
		private readonly byte[] data = new byte[Ed448PrivateKeyParameters.KeySize];
	}
}
