using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200070D RID: 1805
	public class OcspResponse : Asn1Encodable
	{
		// Token: 0x060041E8 RID: 16872 RVA: 0x001846B0 File Offset: 0x001828B0
		public static OcspResponse GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return OcspResponse.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x001846C0 File Offset: 0x001828C0
		public static OcspResponse GetInstance(object obj)
		{
			if (obj == null || obj is OcspResponse)
			{
				return (OcspResponse)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OcspResponse((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x0018470D File Offset: 0x0018290D
		public OcspResponse(OcspResponseStatus responseStatus, ResponseBytes responseBytes)
		{
			if (responseStatus == null)
			{
				throw new ArgumentNullException("responseStatus");
			}
			this.responseStatus = responseStatus;
			this.responseBytes = responseBytes;
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00184731 File Offset: 0x00182931
		private OcspResponse(Asn1Sequence seq)
		{
			this.responseStatus = new OcspResponseStatus(DerEnumerated.GetInstance(seq[0]));
			if (seq.Count == 2)
			{
				this.responseBytes = ResponseBytes.GetInstance((Asn1TaggedObject)seq[1], true);
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x060041EC RID: 16876 RVA: 0x00184771 File Offset: 0x00182971
		public OcspResponseStatus ResponseStatus
		{
			get
			{
				return this.responseStatus;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x060041ED RID: 16877 RVA: 0x00184779 File Offset: 0x00182979
		public ResponseBytes ResponseBytes
		{
			get
			{
				return this.responseBytes;
			}
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x00184784 File Offset: 0x00182984
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.responseStatus
			});
			if (this.responseBytes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.responseBytes)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AB8 RID: 10936
		private readonly OcspResponseStatus responseStatus;

		// Token: 0x04002AB9 RID: 10937
		private readonly ResponseBytes responseBytes;
	}
}
