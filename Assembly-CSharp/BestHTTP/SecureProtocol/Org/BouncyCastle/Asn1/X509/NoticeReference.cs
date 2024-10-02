using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A6 RID: 1702
	public class NoticeReference : Asn1Encodable
	{
		// Token: 0x06003EE4 RID: 16100 RVA: 0x00178CCC File Offset: 0x00176ECC
		private static Asn1EncodableVector ConvertVector(IList numbers)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in numbers)
			{
				DerInteger derInteger;
				if (obj is BigInteger)
				{
					derInteger = new DerInteger((BigInteger)obj);
				}
				else
				{
					if (!(obj is int))
					{
						throw new ArgumentException();
					}
					derInteger = new DerInteger((int)obj);
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					derInteger
				});
			}
			return asn1EncodableVector;
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x00178D68 File Offset: 0x00176F68
		public NoticeReference(string organization, IList numbers) : this(organization, NoticeReference.ConvertVector(numbers))
		{
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x00178D77 File Offset: 0x00176F77
		public NoticeReference(string organization, Asn1EncodableVector noticeNumbers) : this(new DisplayText(organization), noticeNumbers)
		{
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x00178D86 File Offset: 0x00176F86
		public NoticeReference(DisplayText organization, Asn1EncodableVector noticeNumbers)
		{
			this.organization = organization;
			this.noticeNumbers = new DerSequence(noticeNumbers);
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x00178DA4 File Offset: 0x00176FA4
		private NoticeReference(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.organization = DisplayText.GetInstance(seq[0]);
			this.noticeNumbers = Asn1Sequence.GetInstance(seq[1]);
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x00178E04 File Offset: 0x00177004
		public static NoticeReference GetInstance(object obj)
		{
			if (obj is NoticeReference)
			{
				return (NoticeReference)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new NoticeReference(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06003EEA RID: 16106 RVA: 0x00178E25 File Offset: 0x00177025
		public virtual DisplayText Organization
		{
			get
			{
				return this.organization;
			}
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x00178E30 File Offset: 0x00177030
		public virtual DerInteger[] GetNoticeNumbers()
		{
			DerInteger[] array = new DerInteger[this.noticeNumbers.Count];
			for (int num = 0; num != this.noticeNumbers.Count; num++)
			{
				array[num] = DerInteger.GetInstance(this.noticeNumbers[num]);
			}
			return array;
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x00178E79 File Offset: 0x00177079
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.organization,
				this.noticeNumbers
			});
		}

		// Token: 0x040027F3 RID: 10227
		private readonly DisplayText organization;

		// Token: 0x040027F4 RID: 10228
		private readonly Asn1Sequence noticeNumbers;
	}
}
