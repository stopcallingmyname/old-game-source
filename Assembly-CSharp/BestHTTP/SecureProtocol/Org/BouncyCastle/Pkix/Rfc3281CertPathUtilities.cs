using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002C4 RID: 708
	internal class Rfc3281CertPathUtilities
	{
		// Token: 0x06001A0E RID: 6670 RVA: 0x000C3518 File Offset: 0x000C1718
		internal static void ProcessAttrCert7(IX509AttributeCertificate attrCert, PkixCertPath certPath, PkixCertPath holderCertPath, PkixParameters pkixParams)
		{
			ISet criticalExtensionOids = attrCert.GetCriticalExtensionOids();
			if (criticalExtensionOids.Contains(X509Extensions.TargetInformation.Id))
			{
				try
				{
					TargetInformation.GetInstance(PkixCertPathValidatorUtilities.GetExtensionValue(attrCert, X509Extensions.TargetInformation));
				}
				catch (Exception cause)
				{
					throw new PkixCertPathValidatorException("Target information extension could not be read.", cause);
				}
			}
			criticalExtensionOids.Remove(X509Extensions.TargetInformation.Id);
			foreach (object obj in pkixParams.GetAttrCertCheckers())
			{
				((PkixAttrCertChecker)obj).Check(attrCert, certPath, holderCertPath, criticalExtensionOids);
			}
			if (!criticalExtensionOids.IsEmpty)
			{
				throw new PkixCertPathValidatorException("Attribute certificate contains unsupported critical extensions: " + criticalExtensionOids);
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000C35E0 File Offset: 0x000C17E0
		internal static void CheckCrls(IX509AttributeCertificate attrCert, PkixParameters paramsPKIX, X509Certificate issuerCert, DateTime validDate, IList certPathCerts)
		{
			if (paramsPKIX.IsRevocationEnabled)
			{
				if (attrCert.GetExtensionValue(X509Extensions.NoRevAvail) == null)
				{
					CrlDistPoint crlDistPoint = null;
					try
					{
						crlDistPoint = CrlDistPoint.GetInstance(PkixCertPathValidatorUtilities.GetExtensionValue(attrCert, X509Extensions.CrlDistributionPoints));
					}
					catch (Exception cause)
					{
						throw new PkixCertPathValidatorException("CRL distribution point extension could not be read.", cause);
					}
					try
					{
						PkixCertPathValidatorUtilities.AddAdditionalStoresFromCrlDistributionPoint(crlDistPoint, paramsPKIX);
					}
					catch (Exception cause2)
					{
						throw new PkixCertPathValidatorException("No additional CRL locations could be decoded from CRL distribution point extension.", cause2);
					}
					CertStatus certStatus = new CertStatus();
					ReasonsMask reasonsMask = new ReasonsMask();
					Exception cause3 = null;
					bool flag = false;
					if (crlDistPoint != null)
					{
						DistributionPoint[] array = null;
						try
						{
							array = crlDistPoint.GetDistributionPoints();
						}
						catch (Exception cause4)
						{
							throw new PkixCertPathValidatorException("Distribution points could not be read.", cause4);
						}
						try
						{
							int num = 0;
							while (num < array.Length && certStatus.Status == 11 && !reasonsMask.IsAllReasons)
							{
								PkixParameters paramsPKIX2 = (PkixParameters)paramsPKIX.Clone();
								Rfc3281CertPathUtilities.CheckCrl(array[num], attrCert, paramsPKIX2, validDate, issuerCert, certStatus, reasonsMask, certPathCerts);
								flag = true;
								num++;
							}
						}
						catch (Exception innerException)
						{
							cause3 = new Exception("No valid CRL for distribution point found.", innerException);
						}
					}
					if (certStatus.Status == 11 && !reasonsMask.IsAllReasons)
					{
						try
						{
							Asn1Object name = null;
							try
							{
								name = new Asn1InputStream(attrCert.Issuer.GetPrincipals()[0].GetEncoded()).ReadObject();
							}
							catch (Exception innerException2)
							{
								throw new Exception("Issuer from certificate for CRL could not be reencoded.", innerException2);
							}
							DistributionPoint dp = new DistributionPoint(new DistributionPointName(0, new GeneralNames(new GeneralName(4, name))), null, null);
							PkixParameters paramsPKIX3 = (PkixParameters)paramsPKIX.Clone();
							Rfc3281CertPathUtilities.CheckCrl(dp, attrCert, paramsPKIX3, validDate, issuerCert, certStatus, reasonsMask, certPathCerts);
							flag = true;
						}
						catch (Exception innerException3)
						{
							cause3 = new Exception("No valid CRL for distribution point found.", innerException3);
						}
					}
					if (!flag)
					{
						throw new PkixCertPathValidatorException("No valid CRL found.", cause3);
					}
					if (certStatus.Status != 11)
					{
						string str = certStatus.RevocationDate.Value.ToString("ddd MMM dd HH:mm:ss K yyyy");
						throw new PkixCertPathValidatorException("Attribute certificate revocation after " + str + ", reason: " + Rfc3280CertPathUtilities.CrlReasons[certStatus.Status]);
					}
					if (!reasonsMask.IsAllReasons && certStatus.Status == 11)
					{
						certStatus.Status = 12;
					}
					if (certStatus.Status == 12)
					{
						throw new PkixCertPathValidatorException("Attribute certificate status could not be determined.");
					}
				}
				else if (attrCert.GetExtensionValue(X509Extensions.CrlDistributionPoints) != null || attrCert.GetExtensionValue(X509Extensions.AuthorityInfoAccess) != null)
				{
					throw new PkixCertPathValidatorException("No rev avail extension is set, but also an AC revocation pointer.");
				}
			}
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000C3860 File Offset: 0x000C1A60
		internal static void AdditionalChecks(IX509AttributeCertificate attrCert, PkixParameters pkixParams)
		{
			foreach (object obj in pkixParams.GetProhibitedACAttributes())
			{
				string text = (string)obj;
				if (attrCert.GetAttributes(text) != null)
				{
					throw new PkixCertPathValidatorException("Attribute certificate contains prohibited attribute: " + text + ".");
				}
			}
			foreach (object obj2 in pkixParams.GetNecessaryACAttributes())
			{
				string text2 = (string)obj2;
				if (attrCert.GetAttributes(text2) == null)
				{
					throw new PkixCertPathValidatorException("Attribute certificate does not contain necessary attribute: " + text2 + ".");
				}
			}
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000C3934 File Offset: 0x000C1B34
		internal static void ProcessAttrCert5(IX509AttributeCertificate attrCert, PkixParameters pkixParams)
		{
			try
			{
				attrCert.CheckValidity(PkixCertPathValidatorUtilities.GetValidDate(pkixParams));
			}
			catch (CertificateExpiredException cause)
			{
				throw new PkixCertPathValidatorException("Attribute certificate is not valid.", cause);
			}
			catch (CertificateNotYetValidException cause2)
			{
				throw new PkixCertPathValidatorException("Attribute certificate is not valid.", cause2);
			}
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000C3988 File Offset: 0x000C1B88
		internal static void ProcessAttrCert4(X509Certificate acIssuerCert, PkixParameters pkixParams)
		{
			IEnumerable trustedACIssuers = pkixParams.GetTrustedACIssuers();
			bool flag = false;
			foreach (object obj in trustedACIssuers)
			{
				TrustAnchor trustAnchor = (TrustAnchor)obj;
				IDictionary rfc2253Symbols = X509Name.RFC2253Symbols;
				if (acIssuerCert.SubjectDN.ToString(false, rfc2253Symbols).Equals(trustAnchor.CAName) || acIssuerCert.Equals(trustAnchor.TrustedCert))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				throw new PkixCertPathValidatorException("Attribute certificate issuer is not directly trusted.");
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000C3A20 File Offset: 0x000C1C20
		internal static void ProcessAttrCert3(X509Certificate acIssuerCert, PkixParameters pkixParams)
		{
			if (acIssuerCert.GetKeyUsage() != null && !acIssuerCert.GetKeyUsage()[0] && !acIssuerCert.GetKeyUsage()[1])
			{
				throw new PkixCertPathValidatorException("Attribute certificate issuer public key cannot be used to validate digital signatures.");
			}
			if (acIssuerCert.GetBasicConstraints() != -1)
			{
				throw new PkixCertPathValidatorException("Attribute certificate issuer is also a public key certificate issuer.");
			}
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000C3A60 File Offset: 0x000C1C60
		internal static PkixCertPathValidatorResult ProcessAttrCert2(PkixCertPath certPath, PkixParameters pkixParams)
		{
			PkixCertPathValidator pkixCertPathValidator = new PkixCertPathValidator();
			PkixCertPathValidatorResult result;
			try
			{
				result = pkixCertPathValidator.Validate(certPath, pkixParams);
			}
			catch (PkixCertPathValidatorException cause)
			{
				throw new PkixCertPathValidatorException("Certification path for issuer certificate of attribute certificate could not be validated.", cause);
			}
			return result;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000C3A9C File Offset: 0x000C1C9C
		internal static PkixCertPath ProcessAttrCert1(IX509AttributeCertificate attrCert, PkixParameters pkixParams)
		{
			PkixCertPathBuilderResult pkixCertPathBuilderResult = null;
			ISet set = new HashSet();
			if (attrCert.Holder.GetIssuer() != null)
			{
				X509CertStoreSelector x509CertStoreSelector = new X509CertStoreSelector();
				x509CertStoreSelector.SerialNumber = attrCert.Holder.SerialNumber;
				X509Name[] issuer = attrCert.Holder.GetIssuer();
				for (int i = 0; i < issuer.Length; i++)
				{
					try
					{
						x509CertStoreSelector.Issuer = issuer[i];
						set.AddAll(PkixCertPathValidatorUtilities.FindCertificates(x509CertStoreSelector, pkixParams.GetStores()));
					}
					catch (Exception cause)
					{
						throw new PkixCertPathValidatorException("Public key certificate for attribute certificate cannot be searched.", cause);
					}
				}
				if (set.IsEmpty)
				{
					throw new PkixCertPathValidatorException("Public key certificate specified in base certificate ID for attribute certificate cannot be found.");
				}
			}
			if (attrCert.Holder.GetEntityNames() != null)
			{
				X509CertStoreSelector x509CertStoreSelector2 = new X509CertStoreSelector();
				X509Name[] entityNames = attrCert.Holder.GetEntityNames();
				for (int j = 0; j < entityNames.Length; j++)
				{
					try
					{
						x509CertStoreSelector2.Issuer = entityNames[j];
						set.AddAll(PkixCertPathValidatorUtilities.FindCertificates(x509CertStoreSelector2, pkixParams.GetStores()));
					}
					catch (Exception cause2)
					{
						throw new PkixCertPathValidatorException("Public key certificate for attribute certificate cannot be searched.", cause2);
					}
				}
				if (set.IsEmpty)
				{
					throw new PkixCertPathValidatorException("Public key certificate specified in entity name for attribute certificate cannot be found.");
				}
			}
			PkixBuilderParameters instance = PkixBuilderParameters.GetInstance(pkixParams);
			PkixCertPathValidatorException ex = null;
			foreach (object obj in set)
			{
				X509Certificate certificate = (X509Certificate)obj;
				instance.SetTargetConstraints(new X509CertStoreSelector
				{
					Certificate = certificate
				});
				PkixCertPathBuilder pkixCertPathBuilder = new PkixCertPathBuilder();
				try
				{
					pkixCertPathBuilderResult = pkixCertPathBuilder.Build(PkixBuilderParameters.GetInstance(instance));
				}
				catch (PkixCertPathBuilderException cause3)
				{
					ex = new PkixCertPathValidatorException("Certification path for public key certificate of attribute certificate could not be build.", cause3);
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			return pkixCertPathBuilderResult.CertPath;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x000C3C74 File Offset: 0x000C1E74
		private static void CheckCrl(DistributionPoint dp, IX509AttributeCertificate attrCert, PkixParameters paramsPKIX, DateTime validDate, X509Certificate issuerCert, CertStatus certStatus, ReasonsMask reasonMask, IList certPathCerts)
		{
			if (attrCert.GetExtensionValue(X509Extensions.NoRevAvail) != null)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (validDate.CompareTo(utcNow) > 0)
			{
				throw new Exception("Validation time is in future.");
			}
			IEnumerable completeCrls = PkixCertPathValidatorUtilities.GetCompleteCrls(dp, attrCert, utcNow, paramsPKIX);
			bool flag = false;
			Exception ex = null;
			IEnumerator enumerator = completeCrls.GetEnumerator();
			while (enumerator.MoveNext() && certStatus.Status == 11 && !reasonMask.IsAllReasons)
			{
				try
				{
					X509Crl x509Crl = (X509Crl)enumerator.Current;
					ReasonsMask reasonsMask = Rfc3280CertPathUtilities.ProcessCrlD(x509Crl, dp);
					if (reasonsMask.HasNewReasons(reasonMask))
					{
						ISet keys = Rfc3280CertPathUtilities.ProcessCrlF(x509Crl, attrCert, null, null, paramsPKIX, certPathCerts);
						AsymmetricKeyParameter key = Rfc3280CertPathUtilities.ProcessCrlG(x509Crl, keys);
						X509Crl x509Crl2 = null;
						if (paramsPKIX.IsUseDeltasEnabled)
						{
							x509Crl2 = Rfc3280CertPathUtilities.ProcessCrlH(PkixCertPathValidatorUtilities.GetDeltaCrls(utcNow, paramsPKIX, x509Crl), key);
						}
						if (paramsPKIX.ValidityModel != 1 && attrCert.NotAfter.CompareTo(x509Crl.ThisUpdate) < 0)
						{
							throw new Exception("No valid CRL for current time found.");
						}
						Rfc3280CertPathUtilities.ProcessCrlB1(dp, attrCert, x509Crl);
						Rfc3280CertPathUtilities.ProcessCrlB2(dp, attrCert, x509Crl);
						Rfc3280CertPathUtilities.ProcessCrlC(x509Crl2, x509Crl, paramsPKIX);
						Rfc3280CertPathUtilities.ProcessCrlI(validDate, x509Crl2, attrCert, certStatus, paramsPKIX);
						Rfc3280CertPathUtilities.ProcessCrlJ(validDate, x509Crl, attrCert, certStatus);
						if (certStatus.Status == 8)
						{
							certStatus.Status = 11;
						}
						reasonMask.AddReasons(reasonsMask);
						flag = true;
					}
				}
				catch (Exception ex)
				{
				}
			}
			if (!flag)
			{
				throw ex;
			}
		}
	}
}
