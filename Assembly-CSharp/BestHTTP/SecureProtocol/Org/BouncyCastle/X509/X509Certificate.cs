using System;
using System.Collections;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000241 RID: 577
	public class X509Certificate : X509ExtensionBase
	{
		// Token: 0x060014BF RID: 5311 RVA: 0x000AB82E File Offset: 0x000A9A2E
		protected X509Certificate()
		{
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x000AB838 File Offset: 0x000A9A38
		public X509Certificate(X509CertificateStructure c)
		{
			this.c = c;
			try
			{
				Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.19"));
				if (extensionValue != null)
				{
					this.basicConstraints = BasicConstraints.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
				}
			}
			catch (Exception arg)
			{
				throw new CertificateParsingException("cannot construct BasicConstraints: " + arg);
			}
			try
			{
				Asn1OctetString extensionValue2 = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.15"));
				if (extensionValue2 != null)
				{
					DerBitString instance = DerBitString.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue2));
					byte[] bytes = instance.GetBytes();
					int num = bytes.Length * 8 - instance.PadBits;
					this.keyUsage = new bool[(num < 9) ? 9 : num];
					for (int num2 = 0; num2 != num; num2++)
					{
						this.keyUsage[num2] = (((int)bytes[num2 / 8] & 128 >> num2 % 8) != 0);
					}
				}
				else
				{
					this.keyUsage = null;
				}
			}
			catch (Exception arg2)
			{
				throw new CertificateParsingException("cannot construct KeyUsage: " + arg2);
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x000AB948 File Offset: 0x000A9B48
		public virtual X509CertificateStructure CertificateStructure
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x000AB950 File Offset: 0x000A9B50
		public virtual bool IsValidNow
		{
			get
			{
				return this.IsValid(DateTime.UtcNow);
			}
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x000AB95D File Offset: 0x000A9B5D
		public virtual bool IsValid(DateTime time)
		{
			return time.CompareTo(this.NotBefore) >= 0 && time.CompareTo(this.NotAfter) <= 0;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x000AB984 File Offset: 0x000A9B84
		public virtual void CheckValidity()
		{
			this.CheckValidity(DateTime.UtcNow);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x000AB994 File Offset: 0x000A9B94
		public virtual void CheckValidity(DateTime time)
		{
			if (time.CompareTo(this.NotAfter) > 0)
			{
				throw new CertificateExpiredException("certificate expired on " + this.c.EndDate.GetTime());
			}
			if (time.CompareTo(this.NotBefore) < 0)
			{
				throw new CertificateNotYetValidException("certificate not valid until " + this.c.StartDate.GetTime());
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x000ABA01 File Offset: 0x000A9C01
		public virtual int Version
		{
			get
			{
				return this.c.Version;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060014C7 RID: 5319 RVA: 0x000ABA0E File Offset: 0x000A9C0E
		public virtual BigInteger SerialNumber
		{
			get
			{
				return this.c.SerialNumber.Value;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060014C8 RID: 5320 RVA: 0x000ABA20 File Offset: 0x000A9C20
		public virtual X509Name IssuerDN
		{
			get
			{
				return this.c.Issuer;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x000ABA2D File Offset: 0x000A9C2D
		public virtual X509Name SubjectDN
		{
			get
			{
				return this.c.Subject;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x000ABA3A File Offset: 0x000A9C3A
		public virtual DateTime NotBefore
		{
			get
			{
				return this.c.StartDate.ToDateTime();
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x000ABA4C File Offset: 0x000A9C4C
		public virtual DateTime NotAfter
		{
			get
			{
				return this.c.EndDate.ToDateTime();
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x000ABA5E File Offset: 0x000A9C5E
		public virtual byte[] GetTbsCertificate()
		{
			return this.c.TbsCertificate.GetDerEncoded();
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x000ABA70 File Offset: 0x000A9C70
		public virtual byte[] GetSignature()
		{
			return this.c.GetSignatureOctets();
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060014CE RID: 5326 RVA: 0x000ABA7D File Offset: 0x000A9C7D
		public virtual string SigAlgName
		{
			get
			{
				return SignerUtilities.GetEncodingName(this.c.SignatureAlgorithm.Algorithm);
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060014CF RID: 5327 RVA: 0x000ABA94 File Offset: 0x000A9C94
		public virtual string SigAlgOid
		{
			get
			{
				return this.c.SignatureAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000ABAAB File Offset: 0x000A9CAB
		public virtual byte[] GetSigAlgParams()
		{
			if (this.c.SignatureAlgorithm.Parameters != null)
			{
				return this.c.SignatureAlgorithm.Parameters.GetDerEncoded();
			}
			return null;
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060014D1 RID: 5329 RVA: 0x000ABAD6 File Offset: 0x000A9CD6
		public virtual DerBitString IssuerUniqueID
		{
			get
			{
				return this.c.TbsCertificate.IssuerUniqueID;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060014D2 RID: 5330 RVA: 0x000ABAE8 File Offset: 0x000A9CE8
		public virtual DerBitString SubjectUniqueID
		{
			get
			{
				return this.c.TbsCertificate.SubjectUniqueID;
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000ABAFA File Offset: 0x000A9CFA
		public virtual bool[] GetKeyUsage()
		{
			if (this.keyUsage != null)
			{
				return (bool[])this.keyUsage.Clone();
			}
			return null;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000ABB18 File Offset: 0x000A9D18
		public virtual IList GetExtendedKeyUsage()
		{
			Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.37"));
			if (extensionValue == null)
			{
				return null;
			}
			IList result;
			try
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
				IList list = Platform.CreateArrayList();
				foreach (object obj in instance)
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
					list.Add(derObjectIdentifier.Id);
				}
				result = list;
			}
			catch (Exception exception)
			{
				throw new CertificateParsingException("error processing extended key usage extension", exception);
			}
			return result;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x000ABBC0 File Offset: 0x000A9DC0
		public virtual int GetBasicConstraints()
		{
			if (this.basicConstraints == null || !this.basicConstraints.IsCA())
			{
				return -1;
			}
			if (this.basicConstraints.PathLenConstraint == null)
			{
				return int.MaxValue;
			}
			return this.basicConstraints.PathLenConstraint.IntValue;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x000ABBFC File Offset: 0x000A9DFC
		public virtual ICollection GetSubjectAlternativeNames()
		{
			return this.GetAlternativeNames("2.5.29.17");
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x000ABC09 File Offset: 0x000A9E09
		public virtual ICollection GetIssuerAlternativeNames()
		{
			return this.GetAlternativeNames("2.5.29.18");
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000ABC18 File Offset: 0x000A9E18
		protected virtual ICollection GetAlternativeNames(string oid)
		{
			Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier(oid));
			if (extensionValue == null)
			{
				return null;
			}
			GeneralNames instance = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
			IList list = Platform.CreateArrayList();
			foreach (GeneralName generalName in instance.GetNames())
			{
				IList list2 = Platform.CreateArrayList();
				list2.Add(generalName.TagNo);
				list2.Add(generalName.Name.ToString());
				list.Add(list2);
			}
			return list;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x000ABC9A File Offset: 0x000A9E9A
		protected override X509Extensions GetX509Extensions()
		{
			if (this.c.Version < 3)
			{
				return null;
			}
			return this.c.TbsCertificate.Extensions;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000ABCBC File Offset: 0x000A9EBC
		public virtual AsymmetricKeyParameter GetPublicKey()
		{
			return PublicKeyFactory.CreateKey(this.c.SubjectPublicKeyInfo);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x000ABCCE File Offset: 0x000A9ECE
		public virtual byte[] GetEncoded()
		{
			return this.c.GetDerEncoded();
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x000ABCDC File Offset: 0x000A9EDC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			X509Certificate x509Certificate = obj as X509Certificate;
			return x509Certificate != null && this.c.Equals(x509Certificate.c);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x000ABD0C File Offset: 0x000A9F0C
		public override int GetHashCode()
		{
			lock (this)
			{
				if (!this.hashValueSet)
				{
					this.hashValue = this.c.GetHashCode();
					this.hashValueSet = true;
				}
			}
			return this.hashValue;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x000ABD68 File Offset: 0x000A9F68
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("  [0]         Version: ").Append(this.Version).Append(newLine);
			stringBuilder.Append("         SerialNumber: ").Append(this.SerialNumber).Append(newLine);
			stringBuilder.Append("             IssuerDN: ").Append(this.IssuerDN).Append(newLine);
			stringBuilder.Append("           Start Date: ").Append(this.NotBefore).Append(newLine);
			stringBuilder.Append("           Final Date: ").Append(this.NotAfter).Append(newLine);
			stringBuilder.Append("            SubjectDN: ").Append(this.SubjectDN).Append(newLine);
			stringBuilder.Append("           Public Key: ").Append(this.GetPublicKey()).Append(newLine);
			stringBuilder.Append("  Signature Algorithm: ").Append(this.SigAlgName).Append(newLine);
			byte[] signature = this.GetSignature();
			stringBuilder.Append("            Signature: ").Append(Hex.ToHexString(signature, 0, 20)).Append(newLine);
			for (int i = 20; i < signature.Length; i += 20)
			{
				int length = Math.Min(20, signature.Length - i);
				stringBuilder.Append("                       ").Append(Hex.ToHexString(signature, i, length)).Append(newLine);
			}
			X509Extensions extensions = this.c.TbsCertificate.Extensions;
			if (extensions != null)
			{
				IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("       Extensions: \n");
				}
				do
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)enumerator.Current;
					X509Extension extension = extensions.GetExtension(derObjectIdentifier);
					if (extension.Value != null)
					{
						Asn1Object asn1Object = Asn1Object.FromByteArray(extension.Value.GetOctets());
						stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
						try
						{
							if (derObjectIdentifier.Equals(X509Extensions.BasicConstraints))
							{
								stringBuilder.Append(BasicConstraints.GetInstance(asn1Object));
							}
							else if (derObjectIdentifier.Equals(X509Extensions.KeyUsage))
							{
								stringBuilder.Append(KeyUsage.GetInstance(asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.NetscapeCertType))
							{
								stringBuilder.Append(new NetscapeCertType((DerBitString)asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.NetscapeRevocationUrl))
							{
								stringBuilder.Append(new NetscapeRevocationUrl((DerIA5String)asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.VerisignCzagExtension))
							{
								stringBuilder.Append(new VerisignCzagExtension((DerIA5String)asn1Object));
							}
							else
							{
								stringBuilder.Append(derObjectIdentifier.Id);
								stringBuilder.Append(" value = ").Append(Asn1Dump.DumpAsString(asn1Object));
							}
						}
						catch (Exception)
						{
							stringBuilder.Append(derObjectIdentifier.Id);
							stringBuilder.Append(" value = ").Append("*****");
						}
					}
					stringBuilder.Append(newLine);
				}
				while (enumerator.MoveNext());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x000AC094 File Offset: 0x000AA294
		public virtual void Verify(AsymmetricKeyParameter key)
		{
			this.CheckSignature(new Asn1VerifierFactory(this.c.SignatureAlgorithm, key));
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x000AC0AD File Offset: 0x000AA2AD
		public virtual void Verify(IVerifierFactoryProvider verifierProvider)
		{
			this.CheckSignature(verifierProvider.CreateVerifierFactory(this.c.SignatureAlgorithm));
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x000AC0C8 File Offset: 0x000AA2C8
		protected virtual void CheckSignature(IVerifierFactory verifier)
		{
			if (!X509Certificate.IsAlgIDEqual(this.c.SignatureAlgorithm, this.c.TbsCertificate.Signature))
			{
				throw new CertificateException("signature algorithm in TBS cert not same as outer cert");
			}
			Asn1Encodable parameters = this.c.SignatureAlgorithm.Parameters;
			IStreamCalculator streamCalculator = verifier.CreateCalculator();
			byte[] tbsCertificate = this.GetTbsCertificate();
			streamCalculator.Stream.Write(tbsCertificate, 0, tbsCertificate.Length);
			Platform.Dispose(streamCalculator.Stream);
			if (!((IVerifier)streamCalculator.GetResult()).IsVerified(this.GetSignature()))
			{
				throw new InvalidKeyException("Public key presented not for certificate signature");
			}
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x000AC160 File Offset: 0x000AA360
		private static bool IsAlgIDEqual(AlgorithmIdentifier id1, AlgorithmIdentifier id2)
		{
			if (!id1.Algorithm.Equals(id2.Algorithm))
			{
				return false;
			}
			Asn1Encodable parameters = id1.Parameters;
			Asn1Encodable parameters2 = id2.Parameters;
			if (parameters == null == (parameters2 == null))
			{
				return object.Equals(parameters, parameters2);
			}
			if (parameters != null)
			{
				return parameters.ToAsn1Object() is Asn1Null;
			}
			return parameters2.ToAsn1Object() is Asn1Null;
		}

		// Token: 0x04001620 RID: 5664
		private readonly X509CertificateStructure c;

		// Token: 0x04001621 RID: 5665
		private readonly BasicConstraints basicConstraints;

		// Token: 0x04001622 RID: 5666
		private readonly bool[] keyUsage;

		// Token: 0x04001623 RID: 5667
		private bool hashValueSet;

		// Token: 0x04001624 RID: 5668
		private int hashValue;
	}
}
