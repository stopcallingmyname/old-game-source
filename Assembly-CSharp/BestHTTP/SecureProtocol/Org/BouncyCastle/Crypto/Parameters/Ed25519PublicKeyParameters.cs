using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D8 RID: 1240
	public sealed class Ed25519PublicKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06003007 RID: 12295 RVA: 0x00126C24 File Offset: 0x00124E24
		public Ed25519PublicKeyParameters(byte[] buf, int off) : base(false)
		{
			Array.Copy(buf, off, this.data, 0, Ed25519PublicKeyParameters.KeySize);
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x00126C50 File Offset: 0x00124E50
		public Ed25519PublicKeyParameters(Stream input) : base(false)
		{
			if (Ed25519PublicKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of Ed25519 public key");
			}
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x00126C87 File Offset: 0x00124E87
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, Ed25519PublicKeyParameters.KeySize);
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x00126C9C File Offset: 0x00124E9C
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x04001FE2 RID: 8162
		public static readonly int KeySize = Ed25519.PublicKeySize;

		// Token: 0x04001FE3 RID: 8163
		private readonly byte[] data = new byte[Ed25519PublicKeyParameters.KeySize];
	}
}
