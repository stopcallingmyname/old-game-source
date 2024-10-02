using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B0 RID: 1968
	public class CertStatus : Asn1Encodable
	{
		// Token: 0x06004642 RID: 17986 RVA: 0x00192D50 File Offset: 0x00190F50
		private CertStatus(Asn1Sequence seq)
		{
			this.certHash = Asn1OctetString.GetInstance(seq[0]);
			this.certReqId = DerInteger.GetInstance(seq[1]);
			if (seq.Count > 2)
			{
				this.statusInfo = PkiStatusInfo.GetInstance(seq[2]);
			}
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x00192DA2 File Offset: 0x00190FA2
		public CertStatus(byte[] certHash, BigInteger certReqId)
		{
			this.certHash = new DerOctetString(certHash);
			this.certReqId = new DerInteger(certReqId);
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x00192DC2 File Offset: 0x00190FC2
		public CertStatus(byte[] certHash, BigInteger certReqId, PkiStatusInfo statusInfo)
		{
			this.certHash = new DerOctetString(certHash);
			this.certReqId = new DerInteger(certReqId);
			this.statusInfo = statusInfo;
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x00192DE9 File Offset: 0x00190FE9
		public static CertStatus GetInstance(object obj)
		{
			if (obj is CertStatus)
			{
				return (CertStatus)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertStatus((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06004646 RID: 17990 RVA: 0x00192E28 File Offset: 0x00191028
		public virtual Asn1OctetString CertHash
		{
			get
			{
				return this.certHash;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06004647 RID: 17991 RVA: 0x00192E30 File Offset: 0x00191030
		public virtual DerInteger CertReqID
		{
			get
			{
				return this.certReqId;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06004648 RID: 17992 RVA: 0x00192E38 File Offset: 0x00191038
		public virtual PkiStatusInfo StatusInfo
		{
			get
			{
				return this.statusInfo;
			}
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x00192E40 File Offset: 0x00191040
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certHash,
				this.certReqId
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.statusInfo
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002DAF RID: 11695
		private readonly Asn1OctetString certHash;

		// Token: 0x04002DB0 RID: 11696
		private readonly DerInteger certReqId;

		// Token: 0x04002DB1 RID: 11697
		private readonly PkiStatusInfo statusInfo;
	}
}
