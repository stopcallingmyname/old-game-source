using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000711 RID: 1809
	public class ResponseBytes : Asn1Encodable
	{
		// Token: 0x060041FF RID: 16895 RVA: 0x001849E9 File Offset: 0x00182BE9
		public static ResponseBytes GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return ResponseBytes.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x001849F8 File Offset: 0x00182BF8
		public static ResponseBytes GetInstance(object obj)
		{
			if (obj == null || obj is ResponseBytes)
			{
				return (ResponseBytes)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ResponseBytes((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x00184A45 File Offset: 0x00182C45
		public ResponseBytes(DerObjectIdentifier responseType, Asn1OctetString response)
		{
			if (responseType == null)
			{
				throw new ArgumentNullException("responseType");
			}
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			this.responseType = responseType;
			this.response = response;
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x00184A78 File Offset: 0x00182C78
		private ResponseBytes(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.responseType = DerObjectIdentifier.GetInstance(seq[0]);
			this.response = Asn1OctetString.GetInstance(seq[1]);
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x00184AC8 File Offset: 0x00182CC8
		public DerObjectIdentifier ResponseType
		{
			get
			{
				return this.responseType;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x00184AD0 File Offset: 0x00182CD0
		public Asn1OctetString Response
		{
			get
			{
				return this.response;
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00184AD8 File Offset: 0x00182CD8
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.responseType,
				this.response
			});
		}

		// Token: 0x04002AC3 RID: 10947
		private readonly DerObjectIdentifier responseType;

		// Token: 0x04002AC4 RID: 10948
		private readonly Asn1OctetString response;
	}
}
