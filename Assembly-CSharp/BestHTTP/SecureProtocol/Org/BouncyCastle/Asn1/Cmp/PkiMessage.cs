using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C2 RID: 1986
	public class PkiMessage : Asn1Encodable
	{
		// Token: 0x060046CA RID: 18122 RVA: 0x00194394 File Offset: 0x00192594
		private PkiMessage(Asn1Sequence seq)
		{
			this.header = PkiHeader.GetInstance(seq[0]);
			this.body = PkiBody.GetInstance(seq[1]);
			for (int i = 2; i < seq.Count; i++)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[i].ToAsn1Object();
				if (asn1TaggedObject.TagNo == 0)
				{
					this.protection = DerBitString.GetInstance(asn1TaggedObject, true);
				}
				else
				{
					this.extraCerts = Asn1Sequence.GetInstance(asn1TaggedObject, true);
				}
			}
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x00194412 File Offset: 0x00192612
		public static PkiMessage GetInstance(object obj)
		{
			if (obj is PkiMessage)
			{
				return (PkiMessage)obj;
			}
			if (obj != null)
			{
				return new PkiMessage(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x00194434 File Offset: 0x00192634
		public PkiMessage(PkiHeader header, PkiBody body, DerBitString protection, CmpCertificate[] extraCerts)
		{
			this.header = header;
			this.body = body;
			this.protection = protection;
			if (extraCerts != null)
			{
				this.extraCerts = new DerSequence(extraCerts);
			}
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x0019446F File Offset: 0x0019266F
		public PkiMessage(PkiHeader header, PkiBody body, DerBitString protection) : this(header, body, protection, null)
		{
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x0019447B File Offset: 0x0019267B
		public PkiMessage(PkiHeader header, PkiBody body) : this(header, body, null, null)
		{
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060046CF RID: 18127 RVA: 0x00194487 File Offset: 0x00192687
		public virtual PkiHeader Header
		{
			get
			{
				return this.header;
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060046D0 RID: 18128 RVA: 0x0019448F File Offset: 0x0019268F
		public virtual PkiBody Body
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060046D1 RID: 18129 RVA: 0x00194497 File Offset: 0x00192697
		public virtual DerBitString Protection
		{
			get
			{
				return this.protection;
			}
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x001944A0 File Offset: 0x001926A0
		public virtual CmpCertificate[] GetExtraCerts()
		{
			if (this.extraCerts == null)
			{
				return null;
			}
			CmpCertificate[] array = new CmpCertificate[this.extraCerts.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = CmpCertificate.GetInstance(this.extraCerts[i]);
			}
			return array;
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x001944EB File Offset: 0x001926EB
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector v = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.header,
				this.body
			});
			PkiMessage.AddOptional(v, 0, this.protection);
			PkiMessage.AddOptional(v, 1, this.extraCerts);
			return new DerSequence(v);
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x00194101 File Offset: 0x00192301
		private static void AddOptional(Asn1EncodableVector v, int tagNo, Asn1Encodable obj)
		{
			if (obj != null)
			{
				v.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, tagNo, obj)
				});
			}
		}

		// Token: 0x04002E38 RID: 11832
		private readonly PkiHeader header;

		// Token: 0x04002E39 RID: 11833
		private readonly PkiBody body;

		// Token: 0x04002E3A RID: 11834
		private readonly DerBitString protection;

		// Token: 0x04002E3B RID: 11835
		private readonly Asn1Sequence extraCerts;
	}
}
