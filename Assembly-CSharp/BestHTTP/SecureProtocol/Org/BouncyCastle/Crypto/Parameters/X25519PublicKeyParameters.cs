using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000505 RID: 1285
	public sealed class X25519PublicKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x060030D9 RID: 12505 RVA: 0x001283BE File Offset: 0x001265BE
		public X25519PublicKeyParameters(byte[] buf, int off) : base(false)
		{
			Array.Copy(buf, off, this.data, 0, X25519PublicKeyParameters.KeySize);
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x001283EA File Offset: 0x001265EA
		public X25519PublicKeyParameters(Stream input) : base(false)
		{
			if (X25519PublicKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of X25519 public key");
			}
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x00128421 File Offset: 0x00126621
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, X25519PublicKeyParameters.KeySize);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x00128436 File Offset: 0x00126636
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x04002047 RID: 8263
		public static readonly int KeySize = 32;

		// Token: 0x04002048 RID: 8264
		private readonly byte[] data = new byte[X25519PublicKeyParameters.KeySize];
	}
}
