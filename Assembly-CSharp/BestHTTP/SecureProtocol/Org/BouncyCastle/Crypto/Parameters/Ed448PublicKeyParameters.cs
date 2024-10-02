using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004DB RID: 1243
	public sealed class Ed448PublicKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06003015 RID: 12309 RVA: 0x00126E42 File Offset: 0x00125042
		public Ed448PublicKeyParameters(byte[] buf, int off) : base(false)
		{
			Array.Copy(buf, off, this.data, 0, Ed448PublicKeyParameters.KeySize);
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x00126E6E File Offset: 0x0012506E
		public Ed448PublicKeyParameters(Stream input) : base(false)
		{
			if (Ed448PublicKeyParameters.KeySize != Streams.ReadFully(input, this.data))
			{
				throw new EndOfStreamException("EOF encountered in middle of Ed448 public key");
			}
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x00126EA5 File Offset: 0x001250A5
		public void Encode(byte[] buf, int off)
		{
			Array.Copy(this.data, 0, buf, off, Ed448PublicKeyParameters.KeySize);
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x00126EBA File Offset: 0x001250BA
		public byte[] GetEncoded()
		{
			return Arrays.Clone(this.data);
		}

		// Token: 0x04001FE7 RID: 8167
		public static readonly int KeySize = Ed448.PublicKeySize;

		// Token: 0x04001FE8 RID: 8168
		private readonly byte[] data = new byte[Ed448PublicKeyParameters.KeySize];
	}
}
