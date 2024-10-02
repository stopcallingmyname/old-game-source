using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007BC RID: 1980
	public class PkiBody : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600468B RID: 18059 RVA: 0x00193A83 File Offset: 0x00191C83
		public static PkiBody GetInstance(object obj)
		{
			if (obj is PkiBody)
			{
				return (PkiBody)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new PkiBody((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x00193AC2 File Offset: 0x00191CC2
		private PkiBody(Asn1TaggedObject tagged)
		{
			this.tagNo = tagged.TagNo;
			this.body = PkiBody.GetBodyForType(this.tagNo, tagged.GetObject());
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x00193AED File Offset: 0x00191CED
		public PkiBody(int type, Asn1Encodable content)
		{
			this.tagNo = type;
			this.body = PkiBody.GetBodyForType(type, content);
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x00193B0C File Offset: 0x00191D0C
		private static Asn1Encodable GetBodyForType(int type, Asn1Encodable o)
		{
			switch (type)
			{
			case 0:
				return CertReqMessages.GetInstance(o);
			case 1:
				return CertRepMessage.GetInstance(o);
			case 2:
				return CertReqMessages.GetInstance(o);
			case 3:
				return CertRepMessage.GetInstance(o);
			case 4:
				return CertificationRequest.GetInstance(o);
			case 5:
				return PopoDecKeyChallContent.GetInstance(o);
			case 6:
				return PopoDecKeyRespContent.GetInstance(o);
			case 7:
				return CertReqMessages.GetInstance(o);
			case 8:
				return CertRepMessage.GetInstance(o);
			case 9:
				return CertReqMessages.GetInstance(o);
			case 10:
				return KeyRecRepContent.GetInstance(o);
			case 11:
				return RevReqContent.GetInstance(o);
			case 12:
				return RevRepContent.GetInstance(o);
			case 13:
				return CertReqMessages.GetInstance(o);
			case 14:
				return CertRepMessage.GetInstance(o);
			case 15:
				return CAKeyUpdAnnContent.GetInstance(o);
			case 16:
				return CmpCertificate.GetInstance(o);
			case 17:
				return RevAnnContent.GetInstance(o);
			case 18:
				return CrlAnnContent.GetInstance(o);
			case 19:
				return PkiConfirmContent.GetInstance(o);
			case 20:
				return PkiMessages.GetInstance(o);
			case 21:
				return GenMsgContent.GetInstance(o);
			case 22:
				return GenRepContent.GetInstance(o);
			case 23:
				return ErrorMsgContent.GetInstance(o);
			case 24:
				return CertConfirmContent.GetInstance(o);
			case 25:
				return PollReqContent.GetInstance(o);
			case 26:
				return PollRepContent.GetInstance(o);
			default:
				throw new ArgumentException("unknown tag number: " + type, "type");
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x00193C67 File Offset: 0x00191E67
		public virtual int Type
		{
			get
			{
				return this.tagNo;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06004690 RID: 18064 RVA: 0x00193C6F File Offset: 0x00191E6F
		public virtual Asn1Encodable Content
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x00193C77 File Offset: 0x00191E77
		public override Asn1Object ToAsn1Object()
		{
			return new DerTaggedObject(true, this.tagNo, this.body);
		}

		// Token: 0x04002DE4 RID: 11748
		public const int TYPE_INIT_REQ = 0;

		// Token: 0x04002DE5 RID: 11749
		public const int TYPE_INIT_REP = 1;

		// Token: 0x04002DE6 RID: 11750
		public const int TYPE_CERT_REQ = 2;

		// Token: 0x04002DE7 RID: 11751
		public const int TYPE_CERT_REP = 3;

		// Token: 0x04002DE8 RID: 11752
		public const int TYPE_P10_CERT_REQ = 4;

		// Token: 0x04002DE9 RID: 11753
		public const int TYPE_POPO_CHALL = 5;

		// Token: 0x04002DEA RID: 11754
		public const int TYPE_POPO_REP = 6;

		// Token: 0x04002DEB RID: 11755
		public const int TYPE_KEY_UPDATE_REQ = 7;

		// Token: 0x04002DEC RID: 11756
		public const int TYPE_KEY_UPDATE_REP = 8;

		// Token: 0x04002DED RID: 11757
		public const int TYPE_KEY_RECOVERY_REQ = 9;

		// Token: 0x04002DEE RID: 11758
		public const int TYPE_KEY_RECOVERY_REP = 10;

		// Token: 0x04002DEF RID: 11759
		public const int TYPE_REVOCATION_REQ = 11;

		// Token: 0x04002DF0 RID: 11760
		public const int TYPE_REVOCATION_REP = 12;

		// Token: 0x04002DF1 RID: 11761
		public const int TYPE_CROSS_CERT_REQ = 13;

		// Token: 0x04002DF2 RID: 11762
		public const int TYPE_CROSS_CERT_REP = 14;

		// Token: 0x04002DF3 RID: 11763
		public const int TYPE_CA_KEY_UPDATE_ANN = 15;

		// Token: 0x04002DF4 RID: 11764
		public const int TYPE_CERT_ANN = 16;

		// Token: 0x04002DF5 RID: 11765
		public const int TYPE_REVOCATION_ANN = 17;

		// Token: 0x04002DF6 RID: 11766
		public const int TYPE_CRL_ANN = 18;

		// Token: 0x04002DF7 RID: 11767
		public const int TYPE_CONFIRM = 19;

		// Token: 0x04002DF8 RID: 11768
		public const int TYPE_NESTED = 20;

		// Token: 0x04002DF9 RID: 11769
		public const int TYPE_GEN_MSG = 21;

		// Token: 0x04002DFA RID: 11770
		public const int TYPE_GEN_REP = 22;

		// Token: 0x04002DFB RID: 11771
		public const int TYPE_ERROR = 23;

		// Token: 0x04002DFC RID: 11772
		public const int TYPE_CERT_CONFIRM = 24;

		// Token: 0x04002DFD RID: 11773
		public const int TYPE_POLL_REQ = 25;

		// Token: 0x04002DFE RID: 11774
		public const int TYPE_POLL_REP = 26;

		// Token: 0x04002DFF RID: 11775
		private int tagNo;

		// Token: 0x04002E00 RID: 11776
		private Asn1Encodable body;
	}
}
