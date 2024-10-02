using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000508 RID: 1288
	public sealed class X448PublicKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x060030E7 RID: 12519 RVA: 0x0012856E File Offset: 0x0012676E
		public X448PublicKeyParameters(byte[] buf, int off) : base(false)
		{
			Array.Copy(buf, off, this.data, 0, X448PublicKeyParameters.KeySize);
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x0012859A File Offset: 0x0012679A
		public X448PublicKeyParameters(Stream input) : base(false)
		{
			if (X448PublicKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of X448 public key");
			}
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x001285D1 File Offset: 0x001267D1
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, X448PublicKeyParameters.KeySize);
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x001285E6 File Offset: 0x001267E6
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x0400204C RID: 8268
		public static readonly int KeySize = 56;

		// Token: 0x0400204D RID: 8269
		private readonly byte[] data = new byte[X448PublicKeyParameters.KeySize];
	}
}
