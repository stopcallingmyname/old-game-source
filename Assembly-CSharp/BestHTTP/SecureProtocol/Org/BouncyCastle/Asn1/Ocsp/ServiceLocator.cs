using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000714 RID: 1812
	public class ServiceLocator : Asn1Encodable
	{
		// Token: 0x0600421A RID: 16922 RVA: 0x00184E5C File Offset: 0x0018305C
		public static ServiceLocator GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return ServiceLocator.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x00184E6C File Offset: 0x0018306C
		public static ServiceLocator GetInstance(object obj)
		{
			if (obj == null || obj is ServiceLocator)
			{
				return (ServiceLocator)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ServiceLocator((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x00184EB9 File Offset: 0x001830B9
		public ServiceLocator(X509Name issuer) : this(issuer, null)
		{
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x00184EC3 File Offset: 0x001830C3
		public ServiceLocator(X509Name issuer, Asn1Object locator)
		{
			if (issuer == null)
			{
				throw new ArgumentNullException("issuer");
			}
			this.issuer = issuer;
			this.locator = locator;
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x00184EE7 File Offset: 0x001830E7
		private ServiceLocator(Asn1Sequence seq)
		{
			this.issuer = X509Name.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.locator = seq[1].ToAsn1Object();
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x00184F1C File Offset: 0x0018311C
		public X509Name Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06004220 RID: 16928 RVA: 0x00184F24 File Offset: 0x00183124
		public Asn1Object Locator
		{
			get
			{
				return this.locator;
			}
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x00184F2C File Offset: 0x0018312C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.issuer
			});
			if (this.locator != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.locator
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002ACE RID: 10958
		private readonly X509Name issuer;

		// Token: 0x04002ACF RID: 10959
		private readonly Asn1Object locator;
	}
}
