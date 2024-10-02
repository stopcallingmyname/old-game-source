using System;
using System.Collections;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000246 RID: 582
	public class X509CrlEntry : X509ExtensionBase
	{
		// Token: 0x06001511 RID: 5393 RVA: 0x000ACEC4 File Offset: 0x000AB0C4
		public X509CrlEntry(CrlEntry c)
		{
			this.c = c;
			this.certificateIssuer = this.loadCertificateIssuer();
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x000ACEDF File Offset: 0x000AB0DF
		public X509CrlEntry(CrlEntry c, bool isIndirect, X509Name previousCertificateIssuer)
		{
			this.c = c;
			this.isIndirect = isIndirect;
			this.previousCertificateIssuer = previousCertificateIssuer;
			this.certificateIssuer = this.loadCertificateIssuer();
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x000ACF08 File Offset: 0x000AB108
		private X509Name loadCertificateIssuer()
		{
			if (!this.isIndirect)
			{
				return null;
			}
			Asn1OctetString extensionValue = this.GetExtensionValue(X509Extensions.CertificateIssuer);
			if (extensionValue == null)
			{
				return this.previousCertificateIssuer;
			}
			try
			{
				GeneralName[] names = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue)).GetNames();
				for (int i = 0; i < names.Length; i++)
				{
					if (names[i].TagNo == 4)
					{
						return X509Name.GetInstance(names[i].Name);
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000ACF88 File Offset: 0x000AB188
		public X509Name GetCertificateIssuer()
		{
			return this.certificateIssuer;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000ACF90 File Offset: 0x000AB190
		protected override X509Extensions GetX509Extensions()
		{
			return this.c.Extensions;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x000ACFA0 File Offset: 0x000AB1A0
		public byte[] GetEncoded()
		{
			byte[] derEncoded;
			try
			{
				derEncoded = this.c.GetDerEncoded();
			}
			catch (Exception ex)
			{
				throw new CrlException(ex.ToString());
			}
			return derEncoded;
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x000ACFD8 File Offset: 0x000AB1D8
		public BigInteger SerialNumber
		{
			get
			{
				return this.c.UserCertificate.Value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x000ACFEA File Offset: 0x000AB1EA
		public DateTime RevocationDate
		{
			get
			{
				return this.c.RevocationDate.ToDateTime();
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x000ACFFC File Offset: 0x000AB1FC
		public bool HasExtensions
		{
			get
			{
				return this.c.Extensions != null;
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000AD00C File Offset: 0x000AB20C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("        userCertificate: ").Append(this.SerialNumber).Append(newLine);
			stringBuilder.Append("         revocationDate: ").Append(this.RevocationDate).Append(newLine);
			stringBuilder.Append("      certificateIssuer: ").Append(this.GetCertificateIssuer()).Append(newLine);
			X509Extensions extensions = this.c.Extensions;
			if (extensions != null)
			{
				IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("   crlEntryExtensions:").Append(newLine);
					for (;;)
					{
						DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)enumerator.Current;
						X509Extension extension = extensions.GetExtension(derObjectIdentifier);
						if (extension.Value != null)
						{
							Asn1Object asn1Object = Asn1Object.FromByteArray(extension.Value.GetOctets());
							stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
							try
							{
								if (derObjectIdentifier.Equals(X509Extensions.ReasonCode))
								{
									stringBuilder.Append(new CrlReason(DerEnumerated.GetInstance(asn1Object)));
								}
								else if (derObjectIdentifier.Equals(X509Extensions.CertificateIssuer))
								{
									stringBuilder.Append("Certificate issuer: ").Append(GeneralNames.GetInstance((Asn1Sequence)asn1Object));
								}
								else
								{
									stringBuilder.Append(derObjectIdentifier.Id);
									stringBuilder.Append(" value = ").Append(Asn1Dump.DumpAsString(asn1Object));
								}
								stringBuilder.Append(newLine);
								goto IL_1B0;
							}
							catch (Exception)
							{
								stringBuilder.Append(derObjectIdentifier.Id);
								stringBuilder.Append(" value = ").Append("*****").Append(newLine);
								goto IL_1B0;
							}
							goto IL_1A8;
						}
						goto IL_1A8;
						IL_1B0:
						if (!enumerator.MoveNext())
						{
							break;
						}
						continue;
						IL_1A8:
						stringBuilder.Append(newLine);
						goto IL_1B0;
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001630 RID: 5680
		private CrlEntry c;

		// Token: 0x04001631 RID: 5681
		private bool isIndirect;

		// Token: 0x04001632 RID: 5682
		private X509Name previousCertificateIssuer;

		// Token: 0x04001633 RID: 5683
		private X509Name certificateIssuer;
	}
}
