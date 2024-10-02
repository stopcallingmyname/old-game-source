using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000238 RID: 568
	public class AttributeCertificateHolder : IX509Selector, ICloneable
	{
		// Token: 0x06001473 RID: 5235 RVA: 0x000AA734 File Offset: 0x000A8934
		internal AttributeCertificateHolder(Asn1Sequence seq)
		{
			this.holder = Holder.GetInstance(seq);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x000AA748 File Offset: 0x000A8948
		public AttributeCertificateHolder(X509Name issuerName, BigInteger serialNumber)
		{
			this.holder = new Holder(new IssuerSerial(this.GenerateGeneralNames(issuerName), new DerInteger(serialNumber)));
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x000AA770 File Offset: 0x000A8970
		public AttributeCertificateHolder(X509Certificate cert)
		{
			X509Name issuerX509Principal;
			try
			{
				issuerX509Principal = PrincipalUtilities.GetIssuerX509Principal(cert);
			}
			catch (Exception ex)
			{
				throw new CertificateParsingException(ex.Message);
			}
			this.holder = new Holder(new IssuerSerial(this.GenerateGeneralNames(issuerX509Principal), new DerInteger(cert.SerialNumber)));
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000AA7CC File Offset: 0x000A89CC
		public AttributeCertificateHolder(X509Name principal)
		{
			this.holder = new Holder(this.GenerateGeneralNames(principal));
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000AA7E6 File Offset: 0x000A89E6
		public AttributeCertificateHolder(int digestedObjectType, string digestAlgorithm, string otherObjectTypeID, byte[] objectDigest)
		{
			this.holder = new Holder(new ObjectDigestInfo(digestedObjectType, otherObjectTypeID, new AlgorithmIdentifier(new DerObjectIdentifier(digestAlgorithm)), Arrays.Clone(objectDigest)));
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x000AA814 File Offset: 0x000A8A14
		public int DigestedObjectType
		{
			get
			{
				ObjectDigestInfo objectDigestInfo = this.holder.ObjectDigestInfo;
				if (objectDigestInfo != null)
				{
					return objectDigestInfo.DigestedObjectType.Value.IntValue;
				}
				return -1;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x000AA844 File Offset: 0x000A8A44
		public string DigestAlgorithm
		{
			get
			{
				ObjectDigestInfo objectDigestInfo = this.holder.ObjectDigestInfo;
				if (objectDigestInfo != null)
				{
					return objectDigestInfo.DigestAlgorithm.Algorithm.Id;
				}
				return null;
			}
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x000AA874 File Offset: 0x000A8A74
		public byte[] GetObjectDigest()
		{
			ObjectDigestInfo objectDigestInfo = this.holder.ObjectDigestInfo;
			if (objectDigestInfo != null)
			{
				return objectDigestInfo.ObjectDigest.GetBytes();
			}
			return null;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x000AA8A0 File Offset: 0x000A8AA0
		public string OtherObjectTypeID
		{
			get
			{
				ObjectDigestInfo objectDigestInfo = this.holder.ObjectDigestInfo;
				if (objectDigestInfo != null)
				{
					return objectDigestInfo.OtherObjectTypeID.Id;
				}
				return null;
			}
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x000AA8C9 File Offset: 0x000A8AC9
		private GeneralNames GenerateGeneralNames(X509Name principal)
		{
			return new GeneralNames(new GeneralName(principal));
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000AA8D8 File Offset: 0x000A8AD8
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

		// Token: 0x0600147E RID: 5246 RVA: 0x000AA938 File Offset: 0x000A8B38
		private object[] GetNames(GeneralName[] names)
		{
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

		// Token: 0x0600147F RID: 5247 RVA: 0x000AA9A4 File Offset: 0x000A8BA4
		private X509Name[] GetPrincipals(GeneralNames names)
		{
			object[] names2 = this.GetNames(names.GetNames());
			int num = 0;
			for (int num2 = 0; num2 != names2.Length; num2++)
			{
				if (names2[num2] is X509Name)
				{
					num++;
				}
			}
			X509Name[] array = new X509Name[num];
			int num3 = 0;
			for (int num4 = 0; num4 != names2.Length; num4++)
			{
				if (names2[num4] is X509Name)
				{
					array[num3++] = (X509Name)names2[num4];
				}
			}
			return array;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x000AAA18 File Offset: 0x000A8C18
		public X509Name[] GetEntityNames()
		{
			if (this.holder.EntityName != null)
			{
				return this.GetPrincipals(this.holder.EntityName);
			}
			return null;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x000AAA3A File Offset: 0x000A8C3A
		public X509Name[] GetIssuer()
		{
			if (this.holder.BaseCertificateID != null)
			{
				return this.GetPrincipals(this.holder.BaseCertificateID.Issuer);
			}
			return null;
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x000AAA61 File Offset: 0x000A8C61
		public BigInteger SerialNumber
		{
			get
			{
				if (this.holder.BaseCertificateID != null)
				{
					return this.holder.BaseCertificateID.Serial.Value;
				}
				return null;
			}
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x000AAA87 File Offset: 0x000A8C87
		public object Clone()
		{
			return new AttributeCertificateHolder((Asn1Sequence)this.holder.ToAsn1Object());
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x000AAAA0 File Offset: 0x000A8CA0
		public bool Match(X509Certificate x509Cert)
		{
			try
			{
				if (this.holder.BaseCertificateID != null)
				{
					return this.holder.BaseCertificateID.Serial.Value.Equals(x509Cert.SerialNumber) && this.MatchesDN(PrincipalUtilities.GetIssuerX509Principal(x509Cert), this.holder.BaseCertificateID.Issuer);
				}
				if (this.holder.EntityName != null && this.MatchesDN(PrincipalUtilities.GetSubjectX509Principal(x509Cert), this.holder.EntityName))
				{
					return true;
				}
				if (this.holder.ObjectDigestInfo != null)
				{
					IDigest digest = null;
					try
					{
						digest = DigestUtilities.GetDigest(this.DigestAlgorithm);
					}
					catch (Exception)
					{
						return false;
					}
					int digestedObjectType = this.DigestedObjectType;
					if (digestedObjectType != 0)
					{
						if (digestedObjectType == 1)
						{
							byte[] encoded = x509Cert.GetEncoded();
							digest.BlockUpdate(encoded, 0, encoded.Length);
						}
					}
					else
					{
						byte[] encoded2 = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(x509Cert.GetPublicKey()).GetEncoded();
						digest.BlockUpdate(encoded2, 0, encoded2.Length);
					}
					if (!Arrays.AreEqual(DigestUtilities.DoFinal(digest), this.GetObjectDigest()))
					{
						return false;
					}
				}
			}
			catch (CertificateEncodingException)
			{
				return false;
			}
			return false;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x000AABD0 File Offset: 0x000A8DD0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (!(obj is AttributeCertificateHolder))
			{
				return false;
			}
			AttributeCertificateHolder attributeCertificateHolder = (AttributeCertificateHolder)obj;
			return this.holder.Equals(attributeCertificateHolder.holder);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x000AAC05 File Offset: 0x000A8E05
		public override int GetHashCode()
		{
			return this.holder.GetHashCode();
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x000AAC12 File Offset: 0x000A8E12
		public bool Match(object obj)
		{
			return obj is X509Certificate && this.Match((X509Certificate)obj);
		}

		// Token: 0x04001615 RID: 5653
		internal readonly Holder holder;
	}
}
