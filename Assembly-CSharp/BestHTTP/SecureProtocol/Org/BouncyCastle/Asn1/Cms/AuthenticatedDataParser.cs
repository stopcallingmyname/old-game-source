using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200077E RID: 1918
	public class AuthenticatedDataParser
	{
		// Token: 0x060044CA RID: 17610 RVA: 0x0018EC20 File Offset: 0x0018CE20
		public AuthenticatedDataParser(Asn1SequenceParser seq)
		{
			this.seq = seq;
			this.version = (DerInteger)seq.ReadObject();
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060044CB RID: 17611 RVA: 0x0018EC40 File Offset: 0x0018CE40
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x0018EC48 File Offset: 0x0018CE48
		public OriginatorInfo GetOriginatorInfo()
		{
			this.originatorInfoCalled = true;
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject is Asn1TaggedObjectParser && ((Asn1TaggedObjectParser)this.nextObject).TagNo == 0)
			{
				IAsn1Convertible asn1Convertible = (Asn1SequenceParser)((Asn1TaggedObjectParser)this.nextObject).GetObjectParser(16, false);
				this.nextObject = null;
				return OriginatorInfo.GetInstance(asn1Convertible.ToAsn1Object());
			}
			return null;
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x0018ECBF File Offset: 0x0018CEBF
		public Asn1SetParser GetRecipientInfos()
		{
			if (!this.originatorInfoCalled)
			{
				this.GetOriginatorInfo();
			}
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			Asn1SetParser result = (Asn1SetParser)this.nextObject;
			this.nextObject = null;
			return result;
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x0018ECFC File Offset: 0x0018CEFC
		public AlgorithmIdentifier GetMacAlgorithm()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject != null)
			{
				IAsn1Convertible asn1Convertible = (Asn1SequenceParser)this.nextObject;
				this.nextObject = null;
				return AlgorithmIdentifier.GetInstance(asn1Convertible.ToAsn1Object());
			}
			return null;
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x0018ED48 File Offset: 0x0018CF48
		public AlgorithmIdentifier GetDigestAlgorithm()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject is Asn1TaggedObjectParser)
			{
				AlgorithmIdentifier instance = AlgorithmIdentifier.GetInstance((Asn1TaggedObject)this.nextObject.ToAsn1Object(), false);
				this.nextObject = null;
				return instance;
			}
			return null;
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x0018ED9A File Offset: 0x0018CF9A
		public ContentInfoParser GetEnapsulatedContentInfo()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject != null)
			{
				Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)this.nextObject;
				this.nextObject = null;
				return new ContentInfoParser(asn1SequenceParser);
			}
			return null;
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0018EDD8 File Offset: 0x0018CFD8
		public Asn1SetParser GetAuthAttrs()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject is Asn1TaggedObjectParser)
			{
				IAsn1Convertible asn1Convertible = this.nextObject;
				this.nextObject = null;
				return (Asn1SetParser)((Asn1TaggedObjectParser)asn1Convertible).GetObjectParser(17, false);
			}
			return null;
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x0018EE2C File Offset: 0x0018D02C
		public Asn1OctetString GetMac()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			IAsn1Convertible asn1Convertible = this.nextObject;
			this.nextObject = null;
			return Asn1OctetString.GetInstance(asn1Convertible.ToAsn1Object());
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x0018EE60 File Offset: 0x0018D060
		public Asn1SetParser GetUnauthAttrs()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject != null)
			{
				IAsn1Convertible asn1Convertible = this.nextObject;
				this.nextObject = null;
				return (Asn1SetParser)((Asn1TaggedObjectParser)asn1Convertible).GetObjectParser(17, false);
			}
			return null;
		}

		// Token: 0x04002D08 RID: 11528
		private Asn1SequenceParser seq;

		// Token: 0x04002D09 RID: 11529
		private DerInteger version;

		// Token: 0x04002D0A RID: 11530
		private IAsn1Convertible nextObject;

		// Token: 0x04002D0B RID: 11531
		private bool originatorInfoCalled;
	}
}
