using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002FB RID: 763
	public class OCSPRespGenerator
	{
		// Token: 0x06001BCE RID: 7118 RVA: 0x000D1260 File Offset: 0x000CF460
		public OcspResp Generate(int status, object response)
		{
			if (response == null)
			{
				return new OcspResp(new OcspResponse(new OcspResponseStatus(status), null));
			}
			if (response is BasicOcspResp)
			{
				BasicOcspResp basicOcspResp = (BasicOcspResp)response;
				Asn1OctetString response2;
				try
				{
					response2 = new DerOctetString(basicOcspResp.GetEncoded());
				}
				catch (Exception e)
				{
					throw new OcspException("can't encode object.", e);
				}
				ResponseBytes responseBytes = new ResponseBytes(OcspObjectIdentifiers.PkixOcspBasic, response2);
				return new OcspResp(new OcspResponse(new OcspResponseStatus(status), responseBytes));
			}
			throw new OcspException("unknown response object");
		}

		// Token: 0x04001902 RID: 6402
		public const int Successful = 0;

		// Token: 0x04001903 RID: 6403
		public const int MalformedRequest = 1;

		// Token: 0x04001904 RID: 6404
		public const int InternalError = 2;

		// Token: 0x04001905 RID: 6405
		public const int TryLater = 3;

		// Token: 0x04001906 RID: 6406
		public const int SigRequired = 5;

		// Token: 0x04001907 RID: 6407
		public const int Unauthorized = 6;
	}
}
