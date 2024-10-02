using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000239 RID: 569
	public class AttributeCertificateIssuer : IX509Selector, ICloneable
	{
		// Token: 0x06001488 RID: 5256 RVA: 0x000AAC2A File Offset: 0x000A8E2A
		public AttributeCertificateIssuer(AttCertIssuer issuer)
		{
			this.form = issuer.Issuer;
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x000AAC3E File Offset: 0x000A8E3E
		public AttributeCertificateIssuer(X509Name principal)
		{
			this.form = new V2Form(new GeneralNames(new GeneralName(principal)));
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x000AAC5C File Offset: 0x000A8E5C
		private object[] GetNames()
		{
			GeneralNames generalNames;
			if (this.form is V2Form)
			{
				generalNames = ((V2Form)this.form).IssuerName;
			}
			else
			{
				generalNames = (GeneralNames)this.form;
			}
			GeneralName[] names = generalNames.GetNames();
			int num = 0;
			for (int num2 = 0; num2 != names.Length; num2++)
			{
				if (names[num2].TagNo == 4)
				{
					num++;
				}
			}
			object[] array = new object[num];
			int num3 = 0;
			for (int num4 = 0; num4 != names.Length; num4++)
			{
				if (names[num4].TagNo == 4)
				{
					array[num3++] = X509Name.GetInstance(names[num4].Name);
				}
			}
			return array;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x000AAD00 File Offset: 0x000A8F00
		public X509Name[] GetPrincipals()
		{
			object[] names = this.GetNames();
			int num = 0;
			for (int num2 = 0; num2 != names.Length; num2++)
			{
				if (names[num2] is X509Name)
				{
					num++;
				}
			}
			X509Name[] array = new X509Name[num];
			int num3 = 0;
			for (int num4 = 0; num4 != names.Length; num4++)
			{
				if (names[num4] is X509Name)
				{
					array[num3++] = (X509Name)names[num4];
				}
			}
			return array;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x000AAD70 File Offset: 0x000A8F70
		private bool MatchesDN(X509Name subject, GeneralNames targets)
		{
			GeneralName[] names = targets.GetNames();
			for (int num = 0; num != names.Length; num++)
			{
				GeneralName generalName = names[num];
				if (generalName.TagNo == 4)
				{
					try
					{
						if (X509Name.GetInstance(generalName.Name).Equivalent(subject))
						{
							return true;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return false;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x000AADD0 File Offset: 0x000A8FD0
		public object Clone()
		{
			return new AttributeCertificateIssuer(AttCertIssuer.GetInstance(this.form));
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x000AADE4 File Offset: 0x000A8FE4
		public bool Match(X509Certificate x509Cert)
		{
			if (!(this.form is V2Form))
			{
				return this.MatchesDN(x509Cert.SubjectDN, (GeneralNames)this.form);
			}
			V2Form v2Form = (V2Form)this.form;
			if (v2Form.BaseCertificateID != null)
			{
				return v2Form.BaseCertificateID.Serial.Value.Equals(x509Cert.SerialNumber) && this.MatchesDN(x509Cert.IssuerDN, v2Form.BaseCertificateID.Issuer);
			}
			return this.MatchesDN(x509Cert.SubjectDN, v2Form.IssuerName);
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x000AAE74 File Offset: 0x000A9074
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (!(obj is AttributeCertificateIssuer))
			{
				return false;
			}
			AttributeCertificateIssuer attributeCertificateIssuer = (AttributeCertificateIssuer)obj;
			return this.form.Equals(attributeCertificateIssuer.form);
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x000AAEA9 File Offset: 0x000A90A9
		public override int GetHashCode()
		{
			return this.form.GetHashCode();
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x000AAEB6 File Offset: 0x000A90B6
		public bool Match(object obj)
		{
			return obj is X509Certificate && this.Match((X509Certificate)obj);
		}

		// Token: 0x04001616 RID: 5654
		internal readonly Asn1Encodable form;
	}
}
