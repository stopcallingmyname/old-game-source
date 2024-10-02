using System;
using System.Collections;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000245 RID: 581
	public class X509Crl : X509ExtensionBase
	{
		// Token: 0x060014FA RID: 5370 RVA: 0x000AC6A8 File Offset: 0x000AA8A8
		public X509Crl(CertificateList c)
		{
			this.c = c;
			try
			{
				this.sigAlgName = X509SignatureUtilities.GetSignatureName(c.SignatureAlgorithm);
				if (c.SignatureAlgorithm.Parameters != null)
				{
					this.sigAlgParams = c.SignatureAlgorithm.Parameters.GetDerEncoded();
				}
				else
				{
					this.sigAlgParams = null;
				}
				this.isIndirect = this.IsIndirectCrl;
			}
			catch (Exception arg)
			{
				throw new CrlException("CRL contents invalid: " + arg);
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x000AC730 File Offset: 0x000AA930
		protected override X509Extensions GetX509Extensions()
		{
			if (this.c.Version < 2)
			{
				return null;
			}
			return this.c.TbsCertList.Extensions;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000AC754 File Offset: 0x000AA954
		public virtual byte[] GetEncoded()
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

		// Token: 0x060014FD RID: 5373 RVA: 0x000AC78C File Offset: 0x000AA98C
		public virtual void Verify(AsymmetricKeyParameter publicKey)
		{
			this.Verify(new Asn1VerifierFactoryProvider(publicKey));
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x000AC79A File Offset: 0x000AA99A
		public virtual void Verify(IVerifierFactoryProvider verifierProvider)
		{
			this.CheckSignature(verifierProvider.CreateVerifierFactory(this.c.SignatureAlgorithm));
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x000AC7B4 File Offset: 0x000AA9B4
		protected virtual void CheckSignature(IVerifierFactory verifier)
		{
			if (!this.c.SignatureAlgorithm.Equals(this.c.TbsCertList.Signature))
			{
				throw new CrlException("Signature algorithm on CertificateList does not match TbsCertList.");
			}
			Asn1Encodable parameters = this.c.SignatureAlgorithm.Parameters;
			IStreamCalculator streamCalculator = verifier.CreateCalculator();
			byte[] tbsCertList = this.GetTbsCertList();
			streamCalculator.Stream.Write(tbsCertList, 0, tbsCertList.Length);
			Platform.Dispose(streamCalculator.Stream);
			if (!((IVerifier)streamCalculator.GetResult()).IsVerified(this.GetSignature()))
			{
				throw new InvalidKeyException("CRL does not verify with supplied public key.");
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x000AC849 File Offset: 0x000AAA49
		public virtual int Version
		{
			get
			{
				return this.c.Version;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x000AC856 File Offset: 0x000AAA56
		public virtual X509Name IssuerDN
		{
			get
			{
				return this.c.Issuer;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x000AC863 File Offset: 0x000AAA63
		public virtual DateTime ThisUpdate
		{
			get
			{
				return this.c.ThisUpdate.ToDateTime();
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x000AC875 File Offset: 0x000AAA75
		public virtual DateTimeObject NextUpdate
		{
			get
			{
				if (this.c.NextUpdate != null)
				{
					return new DateTimeObject(this.c.NextUpdate.ToDateTime());
				}
				return null;
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000AC89C File Offset: 0x000AAA9C
		private ISet LoadCrlEntries()
		{
			ISet set = new HashSet();
			IEnumerable revokedCertificateEnumeration = this.c.GetRevokedCertificateEnumeration();
			X509Name previousCertificateIssuer = this.IssuerDN;
			foreach (object obj in revokedCertificateEnumeration)
			{
				X509CrlEntry x509CrlEntry = new X509CrlEntry((CrlEntry)obj, this.isIndirect, previousCertificateIssuer);
				set.Add(x509CrlEntry);
				previousCertificateIssuer = x509CrlEntry.GetCertificateIssuer();
			}
			return set;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000AC920 File Offset: 0x000AAB20
		public virtual X509CrlEntry GetRevokedCertificate(BigInteger serialNumber)
		{
			IEnumerable revokedCertificateEnumeration = this.c.GetRevokedCertificateEnumeration();
			X509Name previousCertificateIssuer = this.IssuerDN;
			foreach (object obj in revokedCertificateEnumeration)
			{
				CrlEntry crlEntry = (CrlEntry)obj;
				X509CrlEntry x509CrlEntry = new X509CrlEntry(crlEntry, this.isIndirect, previousCertificateIssuer);
				if (serialNumber.Equals(crlEntry.UserCertificate.Value))
				{
					return x509CrlEntry;
				}
				previousCertificateIssuer = x509CrlEntry.GetCertificateIssuer();
			}
			return null;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x000AC9B4 File Offset: 0x000AABB4
		public virtual ISet GetRevokedCertificates()
		{
			ISet set = this.LoadCrlEntries();
			if (set.Count > 0)
			{
				return set;
			}
			return null;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x000AC9D4 File Offset: 0x000AABD4
		public virtual byte[] GetTbsCertList()
		{
			byte[] derEncoded;
			try
			{
				derEncoded = this.c.TbsCertList.GetDerEncoded();
			}
			catch (Exception ex)
			{
				throw new CrlException(ex.ToString());
			}
			return derEncoded;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x000ACA10 File Offset: 0x000AAC10
		public virtual byte[] GetSignature()
		{
			return this.c.GetSignatureOctets();
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x000ACA1D File Offset: 0x000AAC1D
		public virtual string SigAlgName
		{
			get
			{
				return this.sigAlgName;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x000ACA25 File Offset: 0x000AAC25
		public virtual string SigAlgOid
		{
			get
			{
				return this.c.SignatureAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x000ACA3C File Offset: 0x000AAC3C
		public virtual byte[] GetSigAlgParams()
		{
			return Arrays.Clone(this.sigAlgParams);
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x000ACA4C File Offset: 0x000AAC4C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			X509Crl x509Crl = obj as X509Crl;
			return x509Crl != null && this.c.Equals(x509Crl.c);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x000ACA7C File Offset: 0x000AAC7C
		public override int GetHashCode()
		{
			return this.c.GetHashCode();
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x000ACA8C File Offset: 0x000AAC8C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("              Version: ").Append(this.Version).Append(newLine);
			stringBuilder.Append("             IssuerDN: ").Append(this.IssuerDN).Append(newLine);
			stringBuilder.Append("          This update: ").Append(this.ThisUpdate).Append(newLine);
			stringBuilder.Append("          Next update: ").Append(this.NextUpdate).Append(newLine);
			stringBuilder.Append("  Signature Algorithm: ").Append(this.SigAlgName).Append(newLine);
			byte[] signature = this.GetSignature();
			stringBuilder.Append("            Signature: ");
			stringBuilder.Append(Hex.ToHexString(signature, 0, 20)).Append(newLine);
			for (int i = 20; i < signature.Length; i += 20)
			{
				int length = Math.Min(20, signature.Length - i);
				stringBuilder.Append("                       ");
				stringBuilder.Append(Hex.ToHexString(signature, i, length)).Append(newLine);
			}
			X509Extensions extensions = this.c.TbsCertList.Extensions;
			if (extensions != null)
			{
				IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("           Extensions: ").Append(newLine);
				}
				for (;;)
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)enumerator.Current;
					X509Extension extension = extensions.GetExtension(derObjectIdentifier);
					if (extension.Value != null)
					{
						Asn1Object asn1Object = X509ExtensionUtilities.FromExtensionValue(extension.Value);
						stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
						try
						{
							if (derObjectIdentifier.Equals(X509Extensions.CrlNumber))
							{
								stringBuilder.Append(new CrlNumber(DerInteger.GetInstance(asn1Object).PositiveValue)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.DeltaCrlIndicator))
							{
								stringBuilder.Append("Base CRL: " + new CrlNumber(DerInteger.GetInstance(asn1Object).PositiveValue)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.IssuingDistributionPoint))
							{
								stringBuilder.Append(IssuingDistributionPoint.GetInstance((Asn1Sequence)asn1Object)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.CrlDistributionPoints))
							{
								stringBuilder.Append(CrlDistPoint.GetInstance((Asn1Sequence)asn1Object)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.FreshestCrl))
							{
								stringBuilder.Append(CrlDistPoint.GetInstance((Asn1Sequence)asn1Object)).Append(newLine);
							}
							else
							{
								stringBuilder.Append(derObjectIdentifier.Id);
								stringBuilder.Append(" value = ").Append(Asn1Dump.DumpAsString(asn1Object)).Append(newLine);
							}
							goto IL_2EE;
						}
						catch (Exception)
						{
							stringBuilder.Append(derObjectIdentifier.Id);
							stringBuilder.Append(" value = ").Append("*****").Append(newLine);
							goto IL_2EE;
						}
						goto IL_2E6;
					}
					goto IL_2E6;
					IL_2EE:
					if (!enumerator.MoveNext())
					{
						break;
					}
					continue;
					IL_2E6:
					stringBuilder.Append(newLine);
					goto IL_2EE;
				}
			}
			ISet revokedCertificates = this.GetRevokedCertificates();
			if (revokedCertificates != null)
			{
				foreach (object obj in revokedCertificates)
				{
					X509CrlEntry value = (X509CrlEntry)obj;
					stringBuilder.Append(value);
					stringBuilder.Append(newLine);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x000ACE24 File Offset: 0x000AB024
		public virtual bool IsRevoked(X509Certificate cert)
		{
			CrlEntry[] revokedCertificates = this.c.GetRevokedCertificates();
			if (revokedCertificates != null)
			{
				BigInteger serialNumber = cert.SerialNumber;
				for (int i = 0; i < revokedCertificates.Length; i++)
				{
					if (revokedCertificates[i].UserCertificate.Value.Equals(serialNumber))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x000ACE70 File Offset: 0x000AB070
		protected virtual bool IsIndirectCrl
		{
			get
			{
				Asn1OctetString extensionValue = this.GetExtensionValue(X509Extensions.IssuingDistributionPoint);
				bool result = false;
				try
				{
					if (extensionValue != null)
					{
						result = IssuingDistributionPoint.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue)).IsIndirectCrl;
					}
				}
				catch (Exception arg)
				{
					throw new CrlException("Exception reading IssuingDistributionPoint" + arg);
				}
				return result;
			}
		}

		// Token: 0x0400162C RID: 5676
		private readonly CertificateList c;

		// Token: 0x0400162D RID: 5677
		private readonly string sigAlgName;

		// Token: 0x0400162E RID: 5678
		private readonly byte[] sigAlgParams;

		// Token: 0x0400162F RID: 5679
		private readonly bool isIndirect;
	}
}
