using System;
using System.IO;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002A6 RID: 678
	public class TimeStampResponse
	{
		// Token: 0x060018B8 RID: 6328 RVA: 0x000BA46C File Offset: 0x000B866C
		public TimeStampResponse(TimeStampResp resp)
		{
			this.resp = resp;
			if (resp.TimeStampToken != null)
			{
				this.timeStampToken = new TimeStampToken(resp.TimeStampToken);
			}
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x000BA494 File Offset: 0x000B8694
		public TimeStampResponse(byte[] resp) : this(TimeStampResponse.readTimeStampResp(new Asn1InputStream(resp)))
		{
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x000BA4A7 File Offset: 0x000B86A7
		public TimeStampResponse(Stream input) : this(TimeStampResponse.readTimeStampResp(new Asn1InputStream(input)))
		{
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x000BA4BC File Offset: 0x000B86BC
		private static TimeStampResp readTimeStampResp(Asn1InputStream input)
		{
			TimeStampResp instance;
			try
			{
				instance = TimeStampResp.GetInstance(input.ReadObject());
			}
			catch (ArgumentException ex)
			{
				throw new TspException("malformed timestamp response: " + ex, ex);
			}
			catch (InvalidCastException ex2)
			{
				throw new TspException("malformed timestamp response: " + ex2, ex2);
			}
			return instance;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060018BC RID: 6332 RVA: 0x000BA51C File Offset: 0x000B871C
		public int Status
		{
			get
			{
				return this.resp.Status.Status.IntValue;
			}
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000BA534 File Offset: 0x000B8734
		public string GetStatusString()
		{
			if (this.resp.Status.StatusString == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			PkiFreeText statusString = this.resp.Status.StatusString;
			for (int num = 0; num != statusString.Count; num++)
			{
				stringBuilder.Append(statusString[num].GetString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x000BA596 File Offset: 0x000B8796
		public PkiFailureInfo GetFailInfo()
		{
			if (this.resp.Status.FailInfo == null)
			{
				return null;
			}
			return new PkiFailureInfo(this.resp.Status.FailInfo);
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x000BA5C1 File Offset: 0x000B87C1
		public TimeStampToken TimeStampToken
		{
			get
			{
				return this.timeStampToken;
			}
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x000BA5CC File Offset: 0x000B87CC
		public void Validate(TimeStampRequest request)
		{
			TimeStampToken timeStampToken = this.TimeStampToken;
			if (timeStampToken != null)
			{
				TimeStampTokenInfo timeStampInfo = timeStampToken.TimeStampInfo;
				if (request.Nonce != null && !request.Nonce.Equals(timeStampInfo.Nonce))
				{
					throw new TspValidationException("response contains wrong nonce value.");
				}
				if (this.Status != 0 && this.Status != 1)
				{
					throw new TspValidationException("time stamp token found in failed request.");
				}
				if (!Arrays.ConstantTimeAreEqual(request.GetMessageImprintDigest(), timeStampInfo.GetMessageImprintDigest()))
				{
					throw new TspValidationException("response for different message imprint digest.");
				}
				if (!timeStampInfo.MessageImprintAlgOid.Equals(request.MessageImprintAlgOid))
				{
					throw new TspValidationException("response for different message imprint algorithm.");
				}
				Attribute attribute = timeStampToken.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificate];
				Attribute attribute2 = timeStampToken.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificateV2];
				if (attribute == null && attribute2 == null)
				{
					throw new TspValidationException("no signing certificate attribute present.");
				}
				if (attribute != null)
				{
				}
				if (request.ReqPolicy != null && !request.ReqPolicy.Equals(timeStampInfo.Policy))
				{
					throw new TspValidationException("TSA policy wrong for request.");
				}
			}
			else if (this.Status == 0 || this.Status == 1)
			{
				throw new TspValidationException("no time stamp token found and one expected.");
			}
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x000BA6E3 File Offset: 0x000B88E3
		public byte[] GetEncoded()
		{
			return this.resp.GetEncoded();
		}

		// Token: 0x0400183C RID: 6204
		private TimeStampResp resp;

		// Token: 0x0400183D RID: 6205
		private TimeStampToken timeStampToken;
	}
}
