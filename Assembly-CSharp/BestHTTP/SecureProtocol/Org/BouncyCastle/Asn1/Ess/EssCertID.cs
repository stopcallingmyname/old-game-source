using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x0200073B RID: 1851
	public class EssCertID : Asn1Encodable
	{
		// Token: 0x060042F8 RID: 17144 RVA: 0x0018886C File Offset: 0x00186A6C
		public static EssCertID GetInstance(object o)
		{
			if (o == null || o is EssCertID)
			{
				return (EssCertID)o;
			}
			if (o is Asn1Sequence)
			{
				return new EssCertID((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in 'EssCertID' factory : " + Platform.GetTypeName(o) + ".");
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x001888BC File Offset: 0x00186ABC
		public EssCertID(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.certHash = Asn1OctetString.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.issuerSerial = IssuerSerial.GetInstance(seq[1]);
			}
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x00188929 File Offset: 0x00186B29
		public EssCertID(byte[] hash)
		{
			this.certHash = new DerOctetString(hash);
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x0018893D File Offset: 0x00186B3D
		public EssCertID(byte[] hash, IssuerSerial issuerSerial)
		{
			this.certHash = new DerOctetString(hash);
			this.issuerSerial = issuerSerial;
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00188958 File Offset: 0x00186B58
		public byte[] GetCertHash()
		{
			return this.certHash.GetOctets();
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x00188965 File Offset: 0x00186B65
		public IssuerSerial IssuerSerial
		{
			get
			{
				return this.issuerSerial;
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00188970 File Offset: 0x00186B70
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certHash
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

		// Token: 0x04002C0D RID: 11277
		private Asn1OctetString certHash;

		// Token: 0x04002C0E RID: 11278
		private IssuerSerial issuerSerial;
	}
}
