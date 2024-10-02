using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x0200074F RID: 1871
	public class OtherCertID : Asn1Encodable
	{
		// Token: 0x06004379 RID: 17273 RVA: 0x0018A6C0 File Offset: 0x001888C0
		public static OtherCertID GetInstance(object obj)
		{
			if (obj == null || obj is OtherCertID)
			{
				return (OtherCertID)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OtherCertID((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OtherCertID' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0018A710 File Offset: 0x00188910
		private OtherCertID(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.otherCertHash = OtherHash.GetInstance(seq[0].ToAsn1Object());
			if (seq.Count > 1)
			{
				this.issuerSerial = IssuerSerial.GetInstance(seq[1].ToAsn1Object());
			}
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0018A79A File Offset: 0x0018899A
		public OtherCertID(OtherHash otherCertHash) : this(otherCertHash, null)
		{
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0018A7A4 File Offset: 0x001889A4
		public OtherCertID(OtherHash otherCertHash, IssuerSerial issuerSerial)
		{
			if (otherCertHash == null)
			{
				throw new ArgumentNullException("otherCertHash");
			}
			this.otherCertHash = otherCertHash;
			this.issuerSerial = issuerSerial;
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600437D RID: 17277 RVA: 0x0018A7C8 File Offset: 0x001889C8
		public OtherHash OtherCertHash
		{
			get
			{
				return this.otherCertHash;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600437E RID: 17278 RVA: 0x0018A7D0 File Offset: 0x001889D0
		public IssuerSerial IssuerSerial
		{
			get
			{
				return this.issuerSerial;
			}
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0018A7D8 File Offset: 0x001889D8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.otherCertHash.ToAsn1Object()
			});
			if (this.issuerSerial != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.issuerSerial.ToAsn1Object()
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C44 RID: 11332
		private readonly OtherHash otherCertHash;

		// Token: 0x04002C45 RID: 11333
		private readonly IssuerSerial issuerSerial;
	}
}
