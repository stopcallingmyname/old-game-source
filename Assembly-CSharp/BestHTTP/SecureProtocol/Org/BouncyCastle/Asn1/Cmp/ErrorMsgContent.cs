using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B5 RID: 1973
	public class ErrorMsgContent : Asn1Encodable
	{
		// Token: 0x0600465B RID: 18011 RVA: 0x00193298 File Offset: 0x00191498
		private ErrorMsgContent(Asn1Sequence seq)
		{
			this.pkiStatusInfo = PkiStatusInfo.GetInstance(seq[0]);
			for (int i = 1; i < seq.Count; i++)
			{
				Asn1Encodable asn1Encodable = seq[i];
				if (asn1Encodable is DerInteger)
				{
					this.errorCode = DerInteger.GetInstance(asn1Encodable);
				}
				else
				{
					this.errorDetails = PkiFreeText.GetInstance(asn1Encodable);
				}
			}
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x001932F8 File Offset: 0x001914F8
		public static ErrorMsgContent GetInstance(object obj)
		{
			if (obj is ErrorMsgContent)
			{
				return (ErrorMsgContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ErrorMsgContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00193337 File Offset: 0x00191537
		public ErrorMsgContent(PkiStatusInfo pkiStatusInfo) : this(pkiStatusInfo, null, null)
		{
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00193342 File Offset: 0x00191542
		public ErrorMsgContent(PkiStatusInfo pkiStatusInfo, DerInteger errorCode, PkiFreeText errorDetails)
		{
			if (pkiStatusInfo == null)
			{
				throw new ArgumentNullException("pkiStatusInfo");
			}
			this.pkiStatusInfo = pkiStatusInfo;
			this.errorCode = errorCode;
			this.errorDetails = errorDetails;
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x0600465F RID: 18015 RVA: 0x0019336D File Offset: 0x0019156D
		public virtual PkiStatusInfo PkiStatusInfo
		{
			get
			{
				return this.pkiStatusInfo;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06004660 RID: 18016 RVA: 0x00193375 File Offset: 0x00191575
		public virtual DerInteger ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06004661 RID: 18017 RVA: 0x0019337D File Offset: 0x0019157D
		public virtual PkiFreeText ErrorDetails
		{
			get
			{
				return this.errorDetails;
			}
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00193388 File Offset: 0x00191588
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.pkiStatusInfo
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.errorCode,
				this.errorDetails
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002DD2 RID: 11730
		private readonly PkiStatusInfo pkiStatusInfo;

		// Token: 0x04002DD3 RID: 11731
		private readonly DerInteger errorCode;

		// Token: 0x04002DD4 RID: 11732
		private readonly PkiFreeText errorDetails;
	}
}
