using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006BA RID: 1722
	public class UserNotice : Asn1Encodable
	{
		// Token: 0x06003F77 RID: 16247 RVA: 0x0017A64C File Offset: 0x0017884C
		public UserNotice(NoticeReference noticeRef, DisplayText explicitText)
		{
			this.noticeRef = noticeRef;
			this.explicitText = explicitText;
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0017A662 File Offset: 0x00178862
		public UserNotice(NoticeReference noticeRef, string str) : this(noticeRef, new DisplayText(str))
		{
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0017A674 File Offset: 0x00178874
		[Obsolete("Use GetInstance() instead")]
		public UserNotice(Asn1Sequence seq)
		{
			if (seq.Count == 2)
			{
				this.noticeRef = NoticeReference.GetInstance(seq[0]);
				this.explicitText = DisplayText.GetInstance(seq[1]);
				return;
			}
			if (seq.Count == 1)
			{
				if (seq[0].ToAsn1Object() is Asn1Sequence)
				{
					this.noticeRef = NoticeReference.GetInstance(seq[0]);
					this.explicitText = null;
					return;
				}
				this.noticeRef = null;
				this.explicitText = DisplayText.GetInstance(seq[0]);
				return;
			}
			else
			{
				if (seq.Count == 0)
				{
					this.noticeRef = null;
					this.explicitText = null;
					return;
				}
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0017A736 File Offset: 0x00178936
		public static UserNotice GetInstance(object obj)
		{
			if (obj is UserNotice)
			{
				return (UserNotice)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new UserNotice(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003F7B RID: 16251 RVA: 0x0017A757 File Offset: 0x00178957
		public virtual NoticeReference NoticeRef
		{
			get
			{
				return this.noticeRef;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003F7C RID: 16252 RVA: 0x0017A75F File Offset: 0x0017895F
		public virtual DisplayText ExplicitText
		{
			get
			{
				return this.explicitText;
			}
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0017A768 File Offset: 0x00178968
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.noticeRef != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.noticeRef
				});
			}
			if (this.explicitText != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.explicitText
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002834 RID: 10292
		private readonly NoticeReference noticeRef;

		// Token: 0x04002835 RID: 10293
		private readonly DisplayText explicitText;
	}
}
