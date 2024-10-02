using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002FA RID: 762
	public class OcspResp
	{
		// Token: 0x06001BC5 RID: 7109 RVA: 0x000D1103 File Offset: 0x000CF303
		public OcspResp(OcspResponse resp)
		{
			this.resp = resp;
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000D1112 File Offset: 0x000CF312
		public OcspResp(byte[] resp) : this(new Asn1InputStream(resp))
		{
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000D1120 File Offset: 0x000CF320
		public OcspResp(Stream inStr) : this(new Asn1InputStream(inStr))
		{
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000D1130 File Offset: 0x000CF330
		private OcspResp(Asn1InputStream aIn)
		{
			try
			{
				this.resp = OcspResponse.GetInstance(aIn.ReadObject());
			}
			catch (Exception ex)
			{
				throw new IOException("malformed response: " + ex.Message, ex);
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000D1180 File Offset: 0x000CF380
		public int Status
		{
			get
			{
				return this.resp.ResponseStatus.Value.IntValue;
			}
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000D1198 File Offset: 0x000CF398
		public object GetResponseObject()
		{
			ResponseBytes responseBytes = this.resp.ResponseBytes;
			if (responseBytes == null)
			{
				return null;
			}
			if (responseBytes.ResponseType.Equals(OcspObjectIdentifiers.PkixOcspBasic))
			{
				try
				{
					return new BasicOcspResp(BasicOcspResponse.GetInstance(Asn1Object.FromByteArray(responseBytes.Response.GetOctets())));
				}
				catch (Exception ex)
				{
					throw new OcspException("problem decoding object: " + ex, ex);
				}
			}
			return responseBytes.Response;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000D1210 File Offset: 0x000CF410
		public byte[] GetEncoded()
		{
			return this.resp.GetEncoded();
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000D1220 File Offset: 0x000CF420
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			OcspResp ocspResp = obj as OcspResp;
			return ocspResp != null && this.resp.Equals(ocspResp.resp);
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000D1250 File Offset: 0x000CF450
		public override int GetHashCode()
		{
			return this.resp.GetHashCode();
		}

		// Token: 0x04001901 RID: 6401
		private OcspResponse resp;
	}
}
