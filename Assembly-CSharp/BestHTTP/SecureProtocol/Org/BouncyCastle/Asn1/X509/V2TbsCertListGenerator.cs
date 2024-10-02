using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006BE RID: 1726
	public class V2TbsCertListGenerator
	{
		// Token: 0x06003FA1 RID: 16289 RVA: 0x0017AC7F File Offset: 0x00178E7F
		public void SetSignature(AlgorithmIdentifier signature)
		{
			this.signature = signature;
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x0017AC88 File Offset: 0x00178E88
		public void SetIssuer(X509Name issuer)
		{
			this.issuer = issuer;
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x0017AC91 File Offset: 0x00178E91
		public void SetThisUpdate(DerUtcTime thisUpdate)
		{
			this.thisUpdate = new Time(thisUpdate);
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0017AC9F File Offset: 0x00178E9F
		public void SetNextUpdate(DerUtcTime nextUpdate)
		{
			this.nextUpdate = ((nextUpdate != null) ? new Time(nextUpdate) : null);
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0017ACB3 File Offset: 0x00178EB3
		public void SetThisUpdate(Time thisUpdate)
		{
			this.thisUpdate = thisUpdate;
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0017ACBC File Offset: 0x00178EBC
		public void SetNextUpdate(Time nextUpdate)
		{
			this.nextUpdate = nextUpdate;
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0017ACC5 File Offset: 0x00178EC5
		public void AddCrlEntry(Asn1Sequence crlEntry)
		{
			if (this.crlEntries == null)
			{
				this.crlEntries = Platform.CreateArrayList();
			}
			this.crlEntries.Add(crlEntry);
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0017ACE7 File Offset: 0x00178EE7
		public void AddCrlEntry(DerInteger userCertificate, DerUtcTime revocationDate, int reason)
		{
			this.AddCrlEntry(userCertificate, new Time(revocationDate), reason);
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0017ACF7 File Offset: 0x00178EF7
		public void AddCrlEntry(DerInteger userCertificate, Time revocationDate, int reason)
		{
			this.AddCrlEntry(userCertificate, revocationDate, reason, null);
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x0017AD04 File Offset: 0x00178F04
		public void AddCrlEntry(DerInteger userCertificate, Time revocationDate, int reason, DerGeneralizedTime invalidityDate)
		{
			IList list = Platform.CreateArrayList();
			IList list2 = Platform.CreateArrayList();
			if (reason != 0)
			{
				CrlReason crlReason = new CrlReason(reason);
				try
				{
					list.Add(X509Extensions.ReasonCode);
					list2.Add(new X509Extension(false, new DerOctetString(crlReason.GetEncoded())));
				}
				catch (IOException arg)
				{
					throw new ArgumentException("error encoding reason: " + arg);
				}
			}
			if (invalidityDate != null)
			{
				try
				{
					list.Add(X509Extensions.InvalidityDate);
					list2.Add(new X509Extension(false, new DerOctetString(invalidityDate.GetEncoded())));
				}
				catch (IOException arg2)
				{
					throw new ArgumentException("error encoding invalidityDate: " + arg2);
				}
			}
			if (list.Count != 0)
			{
				this.AddCrlEntry(userCertificate, revocationDate, new X509Extensions(list, list2));
				return;
			}
			this.AddCrlEntry(userCertificate, revocationDate, null);
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0017ADDC File Offset: 0x00178FDC
		public void AddCrlEntry(DerInteger userCertificate, Time revocationDate, X509Extensions extensions)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				userCertificate,
				revocationDate
			});
			if (extensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					extensions
				});
			}
			this.AddCrlEntry(new DerSequence(asn1EncodableVector));
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0017AE1C File Offset: 0x0017901C
		public void SetExtensions(X509Extensions extensions)
		{
			this.extensions = extensions;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0017AE28 File Offset: 0x00179028
		public TbsCertificateList GenerateTbsCertList()
		{
			if (this.signature == null || this.issuer == null || this.thisUpdate == null)
			{
				throw new InvalidOperationException("Not all mandatory fields set in V2 TbsCertList generator.");
			}
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.signature,
				this.issuer,
				this.thisUpdate
			});
			if (this.nextUpdate != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.nextUpdate
				});
			}
			if (this.crlEntries != null)
			{
				Asn1Sequence[] array = new Asn1Sequence[this.crlEntries.Count];
				for (int i = 0; i < this.crlEntries.Count; i++)
				{
					array[i] = (Asn1Sequence)this.crlEntries[i];
				}
				Asn1EncodableVector asn1EncodableVector2 = asn1EncodableVector;
				Asn1Encodable[] array2 = new Asn1Encodable[1];
				int num = 0;
				Asn1Encodable[] v = array;
				array2[num] = new DerSequence(v);
				asn1EncodableVector2.Add(array2);
			}
			if (this.extensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(0, this.extensions)
				});
			}
			return new TbsCertificateList(new DerSequence(asn1EncodableVector));
		}

		// Token: 0x0400284B RID: 10315
		private DerInteger version = new DerInteger(1);

		// Token: 0x0400284C RID: 10316
		private AlgorithmIdentifier signature;

		// Token: 0x0400284D RID: 10317
		private X509Name issuer;

		// Token: 0x0400284E RID: 10318
		private Time thisUpdate;

		// Token: 0x0400284F RID: 10319
		private Time nextUpdate;

		// Token: 0x04002850 RID: 10320
		private X509Extensions extensions;

		// Token: 0x04002851 RID: 10321
		private IList crlEntries;
	}
}
