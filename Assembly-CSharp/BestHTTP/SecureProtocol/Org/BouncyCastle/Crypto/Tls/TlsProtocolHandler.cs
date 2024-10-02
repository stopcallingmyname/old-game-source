using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000477 RID: 1143
	[Obsolete("Use 'TlsClientProtocol' instead")]
	public class TlsProtocolHandler : TlsClientProtocol
	{
		// Token: 0x06002CCC RID: 11468 RVA: 0x00119CBE File Offset: 0x00117EBE
		public TlsProtocolHandler(Stream stream, SecureRandom secureRandom) : base(stream, stream, secureRandom)
		{
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x00119CC9 File Offset: 0x00117EC9
		public TlsProtocolHandler(Stream input, Stream output, SecureRandom secureRandom) : base(input, output, secureRandom)
		{
		}
	}
}
