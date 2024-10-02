using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp
{
	// Token: 0x020006DB RID: 1755
	public class TimeStampReq : Asn1Encodable
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x0017F8A7 File Offset: 0x0017DAA7
		public static TimeStampReq GetInstance(object o)
		{
			if (o == null || o is TimeStampReq)
			{
				return (TimeStampReq)o;
			}
			if (o is Asn1Sequence)
			{
				return new TimeStampReq((Asn1Sequence)o);
			}
			throw new ArgumentException("Unknown object in 'TimeStampReq' factory: " + Platform.GetTypeName(o));
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x0017F8E4 File Offset: 0x0017DAE4
		private TimeStampReq(Asn1Sequence seq)
		{
			int count = seq.Count;
			int num = 0;
			this.version = DerInteger.GetInstance(seq[num++]);
			this.messageImprint = MessageImprint.GetInstance(seq[num++]);
			for (int i = num; i < count; i++)
			{
				if (seq[i] is DerObjectIdentifier)
				{
					this.tsaPolicy = DerObjectIdentifier.GetInstance(seq[i]);
				}
				else if (seq[i] is DerInteger)
				{
					this.nonce = DerInteger.GetInstance(seq[i]);
				}
				else if (seq[i] is DerBoolean)
				{
					this.certReq = DerBoolean.GetInstance(seq[i]);
				}
				else if (seq[i] is Asn1TaggedObject)
				{
					Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[i];
					if (asn1TaggedObject.TagNo == 0)
					{
						this.extensions = X509Extensions.GetInstance(asn1TaggedObject, false);
					}
				}
			}
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x0017F9D4 File Offset: 0x0017DBD4
		public TimeStampReq(MessageImprint messageImprint, DerObjectIdentifier tsaPolicy, DerInteger nonce, DerBoolean certReq, X509Extensions extensions)
		{
			this.version = new DerInteger(1);
			this.messageImprint = messageImprint;
			this.tsaPolicy = tsaPolicy;
			this.nonce = nonce;
			this.certReq = certReq;
			this.extensions = extensions;
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06004091 RID: 16529 RVA: 0x0017FA0D File Offset: 0x0017DC0D
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06004092 RID: 16530 RVA: 0x0017FA15 File Offset: 0x0017DC15
		public MessageImprint MessageImprint
		{
			get
			{
				return this.messageImprint;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06004093 RID: 16531 RVA: 0x0017FA1D File Offset: 0x0017DC1D
		public DerObjectIdentifier ReqPolicy
		{
			get
			{
				return this.tsaPolicy;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06004094 RID: 16532 RVA: 0x0017FA25 File Offset: 0x0017DC25
		public DerInteger Nonce
		{
			get
			{
				return this.nonce;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x0017FA2D File Offset: 0x0017DC2D
		public DerBoolean CertReq
		{
			get
			{
				return this.certReq;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x0017FA35 File Offset: 0x0017DC35
		public X509Extensions Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x0017FA40 File Offset: 0x0017DC40
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.messageImprint
			});
			if (this.tsaPolicy != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.tsaPolicy
				});
			}
			if (this.nonce != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.nonce
				});
			}
			if (this.certReq != null && this.certReq.IsTrue)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.certReq
				});
			}
			if (this.extensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.extensions)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x0400292D RID: 10541
		private readonly DerInteger version;

		// Token: 0x0400292E RID: 10542
		private readonly MessageImprint messageImprint;

		// Token: 0x0400292F RID: 10543
		private readonly DerObjectIdentifier tsaPolicy;

		// Token: 0x04002930 RID: 10544
		private readonly DerInteger nonce;

		// Token: 0x04002931 RID: 10545
		private readonly DerBoolean certReq;

		// Token: 0x04002932 RID: 10546
		private readonly X509Extensions extensions;
	}
}
