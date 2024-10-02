using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x0200070F RID: 1807
	public class Request : Asn1Encodable
	{
		// Token: 0x060041F1 RID: 16881 RVA: 0x001847D0 File Offset: 0x001829D0
		public static Request GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Request.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x001847E0 File Offset: 0x001829E0
		public static Request GetInstance(object obj)
		{
			if (obj == null || obj is Request)
			{
				return (Request)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Request((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x0018482D File Offset: 0x00182A2D
		public Request(CertID reqCert, X509Extensions singleRequestExtensions)
		{
			if (reqCert == null)
			{
				throw new ArgumentNullException("reqCert");
			}
			this.reqCert = reqCert;
			this.singleRequestExtensions = singleRequestExtensions;
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x00184851 File Offset: 0x00182A51
		private Request(Asn1Sequence seq)
		{
			this.reqCert = CertID.GetInstance(seq[0]);
			if (seq.Count == 2)
			{
				this.singleRequestExtensions = X509Extensions.GetInstance((Asn1TaggedObject)seq[1], true);
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x0018488C File Offset: 0x00182A8C
		public CertID ReqCert
		{
			get
			{
				return this.reqCert;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x00184894 File Offset: 0x00182A94
		public X509Extensions SingleRequestExtensions
		{
			get
			{
				return this.singleRequestExtensions;
			}
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x0018489C File Offset: 0x00182A9C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.reqCert
			});
			if (this.singleRequestExtensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.singleRequestExtensions)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AC0 RID: 10944
		private readonly CertID reqCert;

		// Token: 0x04002AC1 RID: 10945
		private readonly X509Extensions singleRequestExtensions;
	}
}
