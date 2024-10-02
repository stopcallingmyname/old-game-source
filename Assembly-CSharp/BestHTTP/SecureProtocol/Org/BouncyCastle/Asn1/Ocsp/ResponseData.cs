using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000712 RID: 1810
	public class ResponseData : Asn1Encodable
	{
		// Token: 0x06004206 RID: 16902 RVA: 0x00184AF7 File Offset: 0x00182CF7
		public static ResponseData GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return ResponseData.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x00184B08 File Offset: 0x00182D08
		public static ResponseData GetInstance(object obj)
		{
			if (obj == null || obj is ResponseData)
			{
				return (ResponseData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ResponseData((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x00184B55 File Offset: 0x00182D55
		public ResponseData(DerInteger version, ResponderID responderID, DerGeneralizedTime producedAt, Asn1Sequence responses, X509Extensions responseExtensions)
		{
			this.version = version;
			this.responderID = responderID;
			this.producedAt = producedAt;
			this.responses = responses;
			this.responseExtensions = responseExtensions;
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x00184B82 File Offset: 0x00182D82
		public ResponseData(ResponderID responderID, DerGeneralizedTime producedAt, Asn1Sequence responses, X509Extensions responseExtensions) : this(ResponseData.V1, responderID, producedAt, responses, responseExtensions)
		{
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x00184B94 File Offset: 0x00182D94
		private ResponseData(Asn1Sequence seq)
		{
			int num = 0;
			Asn1Encodable asn1Encodable = seq[0];
			if (asn1Encodable is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
				if (asn1TaggedObject.TagNo == 0)
				{
					this.versionPresent = true;
					this.version = DerInteger.GetInstance(asn1TaggedObject, true);
					num++;
				}
				else
				{
					this.version = ResponseData.V1;
				}
			}
			else
			{
				this.version = ResponseData.V1;
			}
			this.responderID = ResponderID.GetInstance(seq[num++]);
			this.producedAt = (DerGeneralizedTime)seq[num++];
			this.responses = (Asn1Sequence)seq[num++];
			if (seq.Count > num)
			{
				this.responseExtensions = X509Extensions.GetInstance((Asn1TaggedObject)seq[num], true);
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x00184C5D File Offset: 0x00182E5D
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x00184C65 File Offset: 0x00182E65
		public ResponderID ResponderID
		{
			get
			{
				return this.responderID;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x0600420D RID: 16909 RVA: 0x00184C6D File Offset: 0x00182E6D
		public DerGeneralizedTime ProducedAt
		{
			get
			{
				return this.producedAt;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x00184C75 File Offset: 0x00182E75
		public Asn1Sequence Responses
		{
			get
			{
				return this.responses;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x0600420F RID: 16911 RVA: 0x00184C7D File Offset: 0x00182E7D
		public X509Extensions ResponseExtensions
		{
			get
			{
				return this.responseExtensions;
			}
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x00184C88 File Offset: 0x00182E88
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.versionPresent || !this.version.Equals(ResponseData.V1))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.version)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.responderID,
				this.producedAt,
				this.responses
			});
			if (this.responseExtensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.responseExtensions)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AC5 RID: 10949
		private static readonly DerInteger V1 = new DerInteger(0);

		// Token: 0x04002AC6 RID: 10950
		private readonly bool versionPresent;

		// Token: 0x04002AC7 RID: 10951
		private readonly DerInteger version;

		// Token: 0x04002AC8 RID: 10952
		private readonly ResponderID responderID;

		// Token: 0x04002AC9 RID: 10953
		private readonly DerGeneralizedTime producedAt;

		// Token: 0x04002ACA RID: 10954
		private readonly Asn1Sequence responses;

		// Token: 0x04002ACB RID: 10955
		private readonly X509Extensions responseExtensions;
	}
}
