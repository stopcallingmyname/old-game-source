using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002A5 RID: 677
	public class TimeStampRequestGenerator
	{
		// Token: 0x060018AD RID: 6317 RVA: 0x000BA30E File Offset: 0x000B850E
		public void SetReqPolicy(string reqPolicy)
		{
			this.reqPolicy = new DerObjectIdentifier(reqPolicy);
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x000BA31C File Offset: 0x000B851C
		public void SetCertReq(bool certReq)
		{
			this.certReq = DerBoolean.GetInstance(certReq);
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x000BA32A File Offset: 0x000B852A
		[Obsolete("Use method taking DerObjectIdentifier")]
		public void AddExtension(string oid, bool critical, Asn1Encodable value)
		{
			this.AddExtension(oid, critical, value.GetEncoded());
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x000BA33C File Offset: 0x000B853C
		[Obsolete("Use method taking DerObjectIdentifier")]
		public void AddExtension(string oid, bool critical, byte[] value)
		{
			DerObjectIdentifier derObjectIdentifier = new DerObjectIdentifier(oid);
			this.extensions[derObjectIdentifier] = new X509Extension(critical, new DerOctetString(value));
			this.extOrdering.Add(derObjectIdentifier);
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x000BA375 File Offset: 0x000B8575
		public virtual void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extValue)
		{
			this.AddExtension(oid, critical, extValue.GetEncoded());
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x000BA385 File Offset: 0x000B8585
		public virtual void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extValue)
		{
			this.extensions.Add(oid, new X509Extension(critical, new DerOctetString(extValue)));
			this.extOrdering.Add(oid);
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x000BA3AC File Offset: 0x000B85AC
		public TimeStampRequest Generate(string digestAlgorithm, byte[] digest)
		{
			return this.Generate(digestAlgorithm, digest, null);
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x000BA3B8 File Offset: 0x000B85B8
		public TimeStampRequest Generate(string digestAlgorithmOid, byte[] digest, BigInteger nonce)
		{
			if (digestAlgorithmOid == null)
			{
				throw new ArgumentException("No digest algorithm specified");
			}
			MessageImprint messageImprint = new MessageImprint(new AlgorithmIdentifier(new DerObjectIdentifier(digestAlgorithmOid), DerNull.Instance), digest);
			X509Extensions x509Extensions = null;
			if (this.extOrdering.Count != 0)
			{
				x509Extensions = new X509Extensions(this.extOrdering, this.extensions);
			}
			DerInteger nonce2 = (nonce == null) ? null : new DerInteger(nonce);
			return new TimeStampRequest(new TimeStampReq(messageImprint, this.reqPolicy, nonce2, this.certReq, x509Extensions));
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x000BA42F File Offset: 0x000B862F
		public virtual TimeStampRequest Generate(DerObjectIdentifier digestAlgorithm, byte[] digest)
		{
			return this.Generate(digestAlgorithm.Id, digest);
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x000BA43E File Offset: 0x000B863E
		public virtual TimeStampRequest Generate(DerObjectIdentifier digestAlgorithm, byte[] digest, BigInteger nonce)
		{
			return this.Generate(digestAlgorithm.Id, digest, nonce);
		}

		// Token: 0x04001838 RID: 6200
		private DerObjectIdentifier reqPolicy;

		// Token: 0x04001839 RID: 6201
		private DerBoolean certReq;

		// Token: 0x0400183A RID: 6202
		private IDictionary extensions = Platform.CreateHashtable();

		// Token: 0x0400183B RID: 6203
		private IList extOrdering = Platform.CreateArrayList();
	}
}
