using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000248 RID: 584
	public abstract class X509ExtensionBase : IX509Extension
	{
		// Token: 0x06001526 RID: 5414
		protected abstract X509Extensions GetX509Extensions();

		// Token: 0x06001527 RID: 5415 RVA: 0x000AD47C File Offset: 0x000AB67C
		protected virtual ISet GetExtensionOids(bool critical)
		{
			X509Extensions x509Extensions = this.GetX509Extensions();
			if (x509Extensions != null)
			{
				HashSet hashSet = new HashSet();
				foreach (object obj in x509Extensions.ExtensionOids)
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
					if (x509Extensions.GetExtension(derObjectIdentifier).IsCritical == critical)
					{
						hashSet.Add(derObjectIdentifier.Id);
					}
				}
				return hashSet;
			}
			return null;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x000AD500 File Offset: 0x000AB700
		public virtual ISet GetNonCriticalExtensionOids()
		{
			return this.GetExtensionOids(false);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x000AD509 File Offset: 0x000AB709
		public virtual ISet GetCriticalExtensionOids()
		{
			return this.GetExtensionOids(true);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000AD512 File Offset: 0x000AB712
		[Obsolete("Use version taking a DerObjectIdentifier instead")]
		public Asn1OctetString GetExtensionValue(string oid)
		{
			return this.GetExtensionValue(new DerObjectIdentifier(oid));
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x000AD520 File Offset: 0x000AB720
		public virtual Asn1OctetString GetExtensionValue(DerObjectIdentifier oid)
		{
			X509Extensions x509Extensions = this.GetX509Extensions();
			if (x509Extensions != null)
			{
				X509Extension extension = x509Extensions.GetExtension(oid);
				if (extension != null)
				{
					return extension.Value;
				}
			}
			return null;
		}
	}
}
