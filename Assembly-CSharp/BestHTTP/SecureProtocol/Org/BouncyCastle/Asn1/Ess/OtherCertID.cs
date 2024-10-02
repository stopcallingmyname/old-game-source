using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x0200073D RID: 1853
	[Obsolete("Use version in Asn1.Esf instead")]
	public class OtherCertID : Asn1Encodable
	{
		// Token: 0x0600430A RID: 17162 RVA: 0x00188B9C File Offset: 0x00186D9C
		public static OtherCertID GetInstance(object o)
		{
			if (o == null || o is OtherCertID)
			{
				return (OtherCertID)o;
			}
			if (o is Asn1Sequence)
			{
				return new OtherCertID((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in 'OtherCertID' factory : " + Platform.GetTypeName(o) + ".");
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x00188BEC File Offset: 0x00186DEC
		public OtherCertID(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			if (seq[0].ToAsn1Object() is Asn1OctetString)
			{
				this.otherCertHash = Asn1OctetString.GetInstance(seq[0]);
			}
			else
			{
				this.otherCertHash = DigestInfo.GetInstance(seq[0]);
			}
			if (seq.Count > 1)
			{
				this.issuerSerial = IssuerSerial.GetInstance(Asn1Sequence.GetInstance(seq[1]));
			}
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00188C85 File Offset: 0x00186E85
		public OtherCertID(AlgorithmIdentifier algId, byte[] digest)
		{
			this.otherCertHash = new DigestInfo(algId, digest);
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x00188C9A File Offset: 0x00186E9A
		public OtherCertID(AlgorithmIdentifier algId, byte[] digest, IssuerSerial issuerSerial)
		{
			this.otherCertHash = new DigestInfo(algId, digest);
			this.issuerSerial = issuerSerial;
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x0600430E RID: 17166 RVA: 0x00188CB6 File Offset: 0x00186EB6
		public AlgorithmIdentifier AlgorithmHash
		{
			get
			{
				if (this.otherCertHash.ToAsn1Object() is Asn1OctetString)
				{
					return new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1);
				}
				return DigestInfo.GetInstance(this.otherCertHash).AlgorithmID;
			}
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x00188CE5 File Offset: 0x00186EE5
		public byte[] GetCertHash()
		{
			if (this.otherCertHash.ToAsn1Object() is Asn1OctetString)
			{
				return ((Asn1OctetString)this.otherCertHash.ToAsn1Object()).GetOctets();
			}
			return DigestInfo.GetInstance(this.otherCertHash).GetDigest();
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06004310 RID: 17168 RVA: 0x00188D1F File Offset: 0x00186F1F
		public IssuerSerial IssuerSerial
		{
			get
			{
				return this.issuerSerial;
			}
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x00188D28 File Offset: 0x00186F28
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.otherCertHash
			});
			if (this.issuerSerial != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.issuerSerial
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C13 RID: 11283
		private Asn1Encodable otherCertHash;

		// Token: 0x04002C14 RID: 11284
		private IssuerSerial issuerSerial;
	}
}
