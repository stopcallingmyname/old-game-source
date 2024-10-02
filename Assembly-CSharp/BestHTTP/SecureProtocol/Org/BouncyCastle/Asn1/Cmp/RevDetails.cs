using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007CD RID: 1997
	public class RevDetails : Asn1Encodable
	{
		// Token: 0x0600470D RID: 18189 RVA: 0x00194D84 File Offset: 0x00192F84
		private RevDetails(Asn1Sequence seq)
		{
			this.certDetails = CertTemplate.GetInstance(seq[0]);
			this.crlEntryDetails = ((seq.Count <= 1) ? null : X509Extensions.GetInstance(seq[1]));
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x00194DBC File Offset: 0x00192FBC
		public static RevDetails GetInstance(object obj)
		{
			if (obj is RevDetails)
			{
				return (RevDetails)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RevDetails((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00194DFB File Offset: 0x00192FFB
		public RevDetails(CertTemplate certDetails) : this(certDetails, null)
		{
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x00194E05 File Offset: 0x00193005
		public RevDetails(CertTemplate certDetails, X509Extensions crlEntryDetails)
		{
			this.certDetails = certDetails;
			this.crlEntryDetails = crlEntryDetails;
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06004711 RID: 18193 RVA: 0x00194E1B File Offset: 0x0019301B
		public virtual CertTemplate CertDetails
		{
			get
			{
				return this.certDetails;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06004712 RID: 18194 RVA: 0x00194E23 File Offset: 0x00193023
		public virtual X509Extensions CrlEntryDetails
		{
			get
			{
				return this.crlEntryDetails;
			}
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x00194E2C File Offset: 0x0019302C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certDetails
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.crlEntryDetails
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002E5D RID: 11869
		private readonly CertTemplate certDetails;

		// Token: 0x04002E5E RID: 11870
		private readonly X509Extensions crlEntryDetails;
	}
}
