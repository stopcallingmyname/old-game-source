using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x0200076D RID: 1901
	public class EncKeyWithID : Asn1Encodable
	{
		// Token: 0x06004446 RID: 17478 RVA: 0x0018D4DB File Offset: 0x0018B6DB
		public static EncKeyWithID GetInstance(object obj)
		{
			if (obj is EncKeyWithID)
			{
				return (EncKeyWithID)obj;
			}
			if (obj != null)
			{
				return new EncKeyWithID(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0018D4FC File Offset: 0x0018B6FC
		private EncKeyWithID(Asn1Sequence seq)
		{
			this.privKeyInfo = PrivateKeyInfo.GetInstance(seq[0]);
			if (seq.Count <= 1)
			{
				this.identifier = null;
				return;
			}
			if (!(seq[1] is DerUtf8String))
			{
				this.identifier = GeneralName.GetInstance(seq[1]);
				return;
			}
			this.identifier = seq[1];
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0018D560 File Offset: 0x0018B760
		public EncKeyWithID(PrivateKeyInfo privKeyInfo)
		{
			this.privKeyInfo = privKeyInfo;
			this.identifier = null;
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0018D576 File Offset: 0x0018B776
		public EncKeyWithID(PrivateKeyInfo privKeyInfo, DerUtf8String str)
		{
			this.privKeyInfo = privKeyInfo;
			this.identifier = str;
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0018D576 File Offset: 0x0018B776
		public EncKeyWithID(PrivateKeyInfo privKeyInfo, GeneralName generalName)
		{
			this.privKeyInfo = privKeyInfo;
			this.identifier = generalName;
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600444B RID: 17483 RVA: 0x0018D58C File Offset: 0x0018B78C
		public virtual PrivateKeyInfo PrivateKey
		{
			get
			{
				return this.privKeyInfo;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600444C RID: 17484 RVA: 0x0018D594 File Offset: 0x0018B794
		public virtual bool HasIdentifier
		{
			get
			{
				return this.identifier != null;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600444D RID: 17485 RVA: 0x0018D59F File Offset: 0x0018B79F
		public virtual bool IsIdentifierUtf8String
		{
			get
			{
				return this.identifier is DerUtf8String;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600444E RID: 17486 RVA: 0x0018D5AF File Offset: 0x0018B7AF
		public virtual Asn1Encodable Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0018D5B8 File Offset: 0x0018B7B8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.privKeyInfo
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.identifier
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CD0 RID: 11472
		private readonly PrivateKeyInfo privKeyInfo;

		// Token: 0x04002CD1 RID: 11473
		private readonly Asn1Encodable identifier;
	}
}
