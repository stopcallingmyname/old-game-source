using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200070C RID: 1804
	public class OcspRequest : Asn1Encodable
	{
		// Token: 0x060041E1 RID: 16865 RVA: 0x00184599 File Offset: 0x00182799
		public static OcspRequest GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return OcspRequest.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x001845A8 File Offset: 0x001827A8
		public static OcspRequest GetInstance(object obj)
		{
			if (obj == null || obj is OcspRequest)
			{
				return (OcspRequest)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OcspRequest((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x001845F5 File Offset: 0x001827F5
		public OcspRequest(TbsRequest tbsRequest, Signature optionalSignature)
		{
			if (tbsRequest == null)
			{
				throw new ArgumentNullException("tbsRequest");
			}
			this.tbsRequest = tbsRequest;
			this.optionalSignature = optionalSignature;
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x00184619 File Offset: 0x00182819
		private OcspRequest(Asn1Sequence seq)
		{
			this.tbsRequest = TbsRequest.GetInstance(seq[0]);
			if (seq.Count == 2)
			{
				this.optionalSignature = Signature.GetInstance((Asn1TaggedObject)seq[1], true);
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x060041E5 RID: 16869 RVA: 0x00184654 File Offset: 0x00182854
		public TbsRequest TbsRequest
		{
			get
			{
				return this.tbsRequest;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x060041E6 RID: 16870 RVA: 0x0018465C File Offset: 0x0018285C
		public Signature OptionalSignature
		{
			get
			{
				return this.optionalSignature;
			}
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x00184664 File Offset: 0x00182864
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.tbsRequest
			});
			if (this.optionalSignature != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.optionalSignature)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AB6 RID: 10934
		private readonly TbsRequest tbsRequest;

		// Token: 0x04002AB7 RID: 10935
		private readonly Signature optionalSignature;
	}
}
