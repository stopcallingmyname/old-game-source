using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002A7 RID: 679
	public class TimeStampResponseGenerator
	{
		// Token: 0x060018C2 RID: 6338 RVA: 0x000BA6F0 File Offset: 0x000B88F0
		public TimeStampResponseGenerator(TimeStampTokenGenerator tokenGenerator, IList acceptedAlgorithms) : this(tokenGenerator, acceptedAlgorithms, null, null)
		{
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x000BA6FC File Offset: 0x000B88FC
		public TimeStampResponseGenerator(TimeStampTokenGenerator tokenGenerator, IList acceptedAlgorithms, IList acceptedPolicy) : this(tokenGenerator, acceptedAlgorithms, acceptedPolicy, null)
		{
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x000BA708 File Offset: 0x000B8908
		public TimeStampResponseGenerator(TimeStampTokenGenerator tokenGenerator, IList acceptedAlgorithms, IList acceptedPolicies, IList acceptedExtensions)
		{
			this.tokenGenerator = tokenGenerator;
			this.acceptedAlgorithms = acceptedAlgorithms;
			this.acceptedPolicies = acceptedPolicies;
			this.acceptedExtensions = acceptedExtensions;
			this.statusStrings = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x000BA73D File Offset: 0x000B893D
		private void AddStatusString(string statusString)
		{
			this.statusStrings.Add(new Asn1Encodable[]
			{
				new DerUtf8String(statusString)
			});
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x000BA759 File Offset: 0x000B8959
		private void SetFailInfoField(int field)
		{
			this.failInfo |= field;
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x000BA76C File Offset: 0x000B896C
		private PkiStatusInfo GetPkiStatusInfo()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger((int)this.status)
			});
			if (this.statusStrings.Count > 0)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new PkiFreeText(new DerSequence(this.statusStrings))
				});
			}
			if (this.failInfo != 0)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new TimeStampResponseGenerator.FailInfo(this.failInfo)
				});
			}
			return new PkiStatusInfo(new DerSequence(asn1EncodableVector));
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x000BA7ED File Offset: 0x000B89ED
		public TimeStampResponse Generate(TimeStampRequest request, BigInteger serialNumber, DateTime genTime)
		{
			return this.Generate(request, serialNumber, new DateTimeObject(genTime));
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x000BA800 File Offset: 0x000B8A00
		public TimeStampResponse Generate(TimeStampRequest request, BigInteger serialNumber, DateTimeObject genTime)
		{
			TimeStampResp resp;
			try
			{
				if (genTime == null)
				{
					throw new TspValidationException("The time source is not available.", 512);
				}
				request.Validate(this.acceptedAlgorithms, this.acceptedPolicies, this.acceptedExtensions);
				this.status = PkiStatus.Granted;
				this.AddStatusString("Operation Okay");
				PkiStatusInfo pkiStatusInfo = this.GetPkiStatusInfo();
				ContentInfo instance;
				try
				{
					instance = ContentInfo.GetInstance(Asn1Object.FromByteArray(this.tokenGenerator.Generate(request, serialNumber, genTime.Value).ToCmsSignedData().GetEncoded()));
				}
				catch (IOException e)
				{
					throw new TspException("Timestamp token received cannot be converted to ContentInfo", e);
				}
				resp = new TimeStampResp(pkiStatusInfo, instance);
			}
			catch (TspValidationException ex)
			{
				this.status = PkiStatus.Rejection;
				this.SetFailInfoField(ex.FailureCode);
				this.AddStatusString(ex.Message);
				resp = new TimeStampResp(this.GetPkiStatusInfo(), null);
			}
			TimeStampResponse result;
			try
			{
				result = new TimeStampResponse(resp);
			}
			catch (IOException e2)
			{
				throw new TspException("created badly formatted response!", e2);
			}
			return result;
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x000BA908 File Offset: 0x000B8B08
		public TimeStampResponse GenerateFailResponse(PkiStatus status, int failInfoField, string statusString)
		{
			this.status = status;
			this.SetFailInfoField(failInfoField);
			if (statusString != null)
			{
				this.AddStatusString(statusString);
			}
			TimeStampResp resp = new TimeStampResp(this.GetPkiStatusInfo(), null);
			TimeStampResponse result;
			try
			{
				result = new TimeStampResponse(resp);
			}
			catch (IOException e)
			{
				throw new TspException("created badly formatted response!", e);
			}
			return result;
		}

		// Token: 0x0400183E RID: 6206
		private PkiStatus status;

		// Token: 0x0400183F RID: 6207
		private Asn1EncodableVector statusStrings;

		// Token: 0x04001840 RID: 6208
		private int failInfo;

		// Token: 0x04001841 RID: 6209
		private TimeStampTokenGenerator tokenGenerator;

		// Token: 0x04001842 RID: 6210
		private IList acceptedAlgorithms;

		// Token: 0x04001843 RID: 6211
		private IList acceptedPolicies;

		// Token: 0x04001844 RID: 6212
		private IList acceptedExtensions;

		// Token: 0x020008FD RID: 2301
		private class FailInfo : DerBitString
		{
			// Token: 0x06004E1D RID: 19997 RVA: 0x0016F8CD File Offset: 0x0016DACD
			internal FailInfo(int failInfoValue) : base(failInfoValue)
			{
			}
		}
	}
}
