using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000612 RID: 1554
	public class OriginatorInformation
	{
		// Token: 0x06003ADC RID: 15068 RVA: 0x0016C9ED File Offset: 0x0016ABED
		internal OriginatorInformation(OriginatorInfo originatorInfo)
		{
			this.originatorInfo = originatorInfo;
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x0016C9FC File Offset: 0x0016ABFC
		public virtual IX509Store GetCertificates()
		{
			Asn1Set certificates = this.originatorInfo.Certificates;
			if (certificates != null)
			{
				IList list = Platform.CreateArrayList(certificates.Count);
				foreach (object obj in certificates)
				{
					Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
					if (asn1Object is Asn1Sequence)
					{
						list.Add(new X509Certificate(X509CertificateStructure.GetInstance(asn1Object)));
					}
				}
				return X509StoreFactory.Create("Certificate/Collection", new X509CollectionStoreParameters(list));
			}
			return X509StoreFactory.Create("Certificate/Collection", new X509CollectionStoreParameters(Platform.CreateArrayList()));
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x0016CAAC File Offset: 0x0016ACAC
		public virtual IX509Store GetCrls()
		{
			Asn1Set certificates = this.originatorInfo.Certificates;
			if (certificates != null)
			{
				IList list = Platform.CreateArrayList(certificates.Count);
				foreach (object obj in certificates)
				{
					Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
					if (asn1Object is Asn1Sequence)
					{
						list.Add(new X509Crl(CertificateList.GetInstance(asn1Object)));
					}
				}
				return X509StoreFactory.Create("CRL/Collection", new X509CollectionStoreParameters(list));
			}
			return X509StoreFactory.Create("CRL/Collection", new X509CollectionStoreParameters(Platform.CreateArrayList()));
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x0016CB5C File Offset: 0x0016AD5C
		public virtual OriginatorInfo ToAsn1Structure()
		{
			return this.originatorInfo;
		}

		// Token: 0x0400266D RID: 9837
		private readonly OriginatorInfo originatorInfo;
	}
}
