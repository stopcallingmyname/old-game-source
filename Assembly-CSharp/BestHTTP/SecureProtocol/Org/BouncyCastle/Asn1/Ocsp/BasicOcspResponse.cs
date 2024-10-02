using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000707 RID: 1799
	public class BasicOcspResponse : Asn1Encodable
	{
		// Token: 0x060041BB RID: 16827 RVA: 0x00183F91 File Offset: 0x00182191
		public static BasicOcspResponse GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return BasicOcspResponse.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x00183FA0 File Offset: 0x001821A0
		public static BasicOcspResponse GetInstance(object obj)
		{
			if (obj == null || obj is BasicOcspResponse)
			{
				return (BasicOcspResponse)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new BasicOcspResponse((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x00183FED File Offset: 0x001821ED
		public BasicOcspResponse(ResponseData tbsResponseData, AlgorithmIdentifier signatureAlgorithm, DerBitString signature, Asn1Sequence certs)
		{
			this.tbsResponseData = tbsResponseData;
			this.signatureAlgorithm = signatureAlgorithm;
			this.signature = signature;
			this.certs = certs;
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00184014 File Offset: 0x00182214
		private BasicOcspResponse(Asn1Sequence seq)
		{
			this.tbsResponseData = ResponseData.GetInstance(seq[0]);
			this.signatureAlgorithm = AlgorithmIdentifier.GetInstance(seq[1]);
			this.signature = (DerBitString)seq[2];
			if (seq.Count > 3)
			{
				this.certs = Asn1Sequence.GetInstance((Asn1TaggedObject)seq[3], true);
			}
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x0018407E File Offset: 0x0018227E
		[Obsolete("Use TbsResponseData property instead")]
		public ResponseData GetTbsResponseData()
		{
			return this.tbsResponseData;
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x0018407E File Offset: 0x0018227E
		public ResponseData TbsResponseData
		{
			get
			{
				return this.tbsResponseData;
			}
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x00184086 File Offset: 0x00182286
		[Obsolete("Use SignatureAlgorithm property instead")]
		public AlgorithmIdentifier GetSignatureAlgorithm()
		{
			return this.signatureAlgorithm;
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x00184086 File Offset: 0x00182286
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.signatureAlgorithm;
			}
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x0018408E File Offset: 0x0018228E
		[Obsolete("Use Signature property instead")]
		public DerBitString GetSignature()
		{
			return this.signature;
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x0018408E File Offset: 0x0018228E
		public DerBitString Signature
		{
			get
			{
				return this.signature;
			}
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x00184096 File Offset: 0x00182296
		public byte[] GetSignatureOctets()
		{
			return this.signature.GetOctets();
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x001840A3 File Offset: 0x001822A3
		[Obsolete("Use Certs property instead")]
		public Asn1Sequence GetCerts()
		{
			return this.certs;
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x001840A3 File Offset: 0x001822A3
		public Asn1Sequence Certs
		{
			get
			{
				return this.certs;
			}
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x001840AC File Offset: 0x001822AC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.tbsResponseData,
				this.signatureAlgorithm,
				this.signature
			});
			if (this.certs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.certs)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AA0 RID: 10912
		private readonly ResponseData tbsResponseData;

		// Token: 0x04002AA1 RID: 10913
		private readonly AlgorithmIdentifier signatureAlgorithm;

		// Token: 0x04002AA2 RID: 10914
		private readonly DerBitString signature;

		// Token: 0x04002AA3 RID: 10915
		private readonly Asn1Sequence certs;
	}
}
