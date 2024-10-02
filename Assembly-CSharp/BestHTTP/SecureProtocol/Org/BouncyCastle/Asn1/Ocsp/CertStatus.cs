using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000709 RID: 1801
	public class CertStatus : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060041D2 RID: 16850 RVA: 0x00184251 File Offset: 0x00182451
		public CertStatus()
		{
			this.tagNo = 0;
			this.value = DerNull.Instance;
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x0018426B File Offset: 0x0018246B
		public CertStatus(RevokedInfo info)
		{
			this.tagNo = 1;
			this.value = info;
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x00184281 File Offset: 0x00182481
		public CertStatus(int tagNo, Asn1Encodable value)
		{
			this.tagNo = tagNo;
			this.value = value;
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x00184298 File Offset: 0x00182498
		public CertStatus(Asn1TaggedObject choice)
		{
			this.tagNo = choice.TagNo;
			switch (choice.TagNo)
			{
			case 0:
			case 2:
				this.value = DerNull.Instance;
				return;
			case 1:
				this.value = RevokedInfo.GetInstance(choice, false);
				return;
			default:
				throw new ArgumentException("Unknown tag encountered: " + choice.TagNo);
			}
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x00184308 File Offset: 0x00182508
		public static CertStatus GetInstance(object obj)
		{
			if (obj == null || obj is CertStatus)
			{
				return (CertStatus)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new CertStatus((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060041D7 RID: 16855 RVA: 0x00184355 File Offset: 0x00182555
		public int TagNo
		{
			get
			{
				return this.tagNo;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x0018435D File Offset: 0x0018255D
		public Asn1Encodable Status
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x00184365 File Offset: 0x00182565
		public override Asn1Object ToAsn1Object()
		{
			return new DerTaggedObject(false, this.tagNo, this.value);
		}

		// Token: 0x04002AA8 RID: 10920
		private readonly int tagNo;

		// Token: 0x04002AA9 RID: 10921
		private readonly Asn1Encodable value;
	}
}
