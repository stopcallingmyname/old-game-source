using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007CF RID: 1999
	public class RevRepContentBuilder
	{
		// Token: 0x0600471B RID: 18203 RVA: 0x00195034 File Offset: 0x00193234
		public virtual RevRepContentBuilder Add(PkiStatusInfo status)
		{
			this.status.Add(new Asn1Encodable[]
			{
				status
			});
			return this;
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x0019504C File Offset: 0x0019324C
		public virtual RevRepContentBuilder Add(PkiStatusInfo status, CertId certId)
		{
			if (this.status.Count != this.revCerts.Count)
			{
				throw new InvalidOperationException("status and revCerts sequence must be in common order");
			}
			this.status.Add(new Asn1Encodable[]
			{
				status
			});
			this.revCerts.Add(new Asn1Encodable[]
			{
				certId
			});
			return this;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x001950A7 File Offset: 0x001932A7
		public virtual RevRepContentBuilder AddCrl(CertificateList crl)
		{
			this.crls.Add(new Asn1Encodable[]
			{
				crl
			});
			return this;
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x001950C0 File Offset: 0x001932C0
		public virtual RevRepContent Build()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new DerSequence(this.status)
			});
			if (this.revCerts.Count != 0)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, new DerSequence(this.revCerts))
				});
			}
			if (this.crls.Count != 0)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, new DerSequence(this.crls))
				});
			}
			return RevRepContent.GetInstance(new DerSequence(asn1EncodableVector));
		}

		// Token: 0x04002E62 RID: 11874
		private readonly Asn1EncodableVector status = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());

		// Token: 0x04002E63 RID: 11875
		private readonly Asn1EncodableVector revCerts = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());

		// Token: 0x04002E64 RID: 11876
		private readonly Asn1EncodableVector crls = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
	}
}
