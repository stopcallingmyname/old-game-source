using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.Ocsp
{
	// Token: 0x0200072F RID: 1839
	public class RequestedCertificate : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060042B5 RID: 17077 RVA: 0x001876EC File Offset: 0x001858EC
		public static RequestedCertificate GetInstance(object obj)
		{
			if (obj == null || obj is RequestedCertificate)
			{
				return (RequestedCertificate)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RequestedCertificate(X509CertificateStructure.GetInstance(obj));
			}
			if (obj is Asn1TaggedObject)
			{
				return new RequestedCertificate((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x0018774D File Offset: 0x0018594D
		public static RequestedCertificate GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			if (!isExplicit)
			{
				throw new ArgumentException("choice item must be explicitly tagged");
			}
			return RequestedCertificate.GetInstance(obj.GetObject());
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x00187768 File Offset: 0x00185968
		private RequestedCertificate(Asn1TaggedObject tagged)
		{
			RequestedCertificate.Choice tagNo = (RequestedCertificate.Choice)tagged.TagNo;
			if (tagNo == RequestedCertificate.Choice.PublicKeyCertificate)
			{
				this.publicKeyCert = Asn1OctetString.GetInstance(tagged, true).GetOctets();
				return;
			}
			if (tagNo == RequestedCertificate.Choice.AttributeCertificate)
			{
				this.attributeCert = Asn1OctetString.GetInstance(tagged, true).GetOctets();
				return;
			}
			throw new ArgumentException("unknown tag number: " + tagged.TagNo);
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x001877C9 File Offset: 0x001859C9
		public RequestedCertificate(X509CertificateStructure certificate)
		{
			this.cert = certificate;
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x001877D8 File Offset: 0x001859D8
		public RequestedCertificate(RequestedCertificate.Choice type, byte[] certificateOctets) : this(new DerTaggedObject((int)type, new DerOctetString(certificateOctets)))
		{
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060042BA RID: 17082 RVA: 0x001877EC File Offset: 0x001859EC
		public RequestedCertificate.Choice Type
		{
			get
			{
				if (this.cert != null)
				{
					return RequestedCertificate.Choice.Certificate;
				}
				if (this.publicKeyCert != null)
				{
					return RequestedCertificate.Choice.PublicKeyCertificate;
				}
				return RequestedCertificate.Choice.AttributeCertificate;
			}
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x00187804 File Offset: 0x00185A04
		public byte[] GetCertificateBytes()
		{
			if (this.cert != null)
			{
				try
				{
					return this.cert.GetEncoded();
				}
				catch (IOException arg)
				{
					throw new InvalidOperationException("can't decode certificate: " + arg);
				}
			}
			if (this.publicKeyCert != null)
			{
				return this.publicKeyCert;
			}
			return this.attributeCert;
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x00187860 File Offset: 0x00185A60
		public override Asn1Object ToAsn1Object()
		{
			if (this.publicKeyCert != null)
			{
				return new DerTaggedObject(0, new DerOctetString(this.publicKeyCert));
			}
			if (this.attributeCert != null)
			{
				return new DerTaggedObject(1, new DerOctetString(this.attributeCert));
			}
			return this.cert.ToAsn1Object();
		}

		// Token: 0x04002B90 RID: 11152
		private readonly X509CertificateStructure cert;

		// Token: 0x04002B91 RID: 11153
		private readonly byte[] publicKeyCert;

		// Token: 0x04002B92 RID: 11154
		private readonly byte[] attributeCert;

		// Token: 0x020009D6 RID: 2518
		public enum Choice
		{
			// Token: 0x04003796 RID: 14230
			Certificate = -1,
			// Token: 0x04003797 RID: 14231
			PublicKeyCertificate,
			// Token: 0x04003798 RID: 14232
			AttributeCertificate
		}
	}
}
