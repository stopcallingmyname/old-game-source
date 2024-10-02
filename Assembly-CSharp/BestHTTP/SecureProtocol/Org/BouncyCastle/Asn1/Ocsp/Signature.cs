using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000715 RID: 1813
	public class Signature : Asn1Encodable
	{
		// Token: 0x06004222 RID: 16930 RVA: 0x00184F71 File Offset: 0x00183171
		public static Signature GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Signature.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x00184F80 File Offset: 0x00183180
		public static Signature GetInstance(object obj)
		{
			if (obj == null || obj is Signature)
			{
				return (Signature)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Signature((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x00184FCD File Offset: 0x001831CD
		public Signature(AlgorithmIdentifier signatureAlgorithm, DerBitString signatureValue) : this(signatureAlgorithm, signatureValue, null)
		{
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x00184FD8 File Offset: 0x001831D8
		public Signature(AlgorithmIdentifier signatureAlgorithm, DerBitString signatureValue, Asn1Sequence certs)
		{
			if (signatureAlgorithm == null)
			{
				throw new ArgumentException("signatureAlgorithm");
			}
			if (signatureValue == null)
			{
				throw new ArgumentException("signatureValue");
			}
			this.signatureAlgorithm = signatureAlgorithm;
			this.signatureValue = signatureValue;
			this.certs = certs;
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x00185014 File Offset: 0x00183214
		private Signature(Asn1Sequence seq)
		{
			this.signatureAlgorithm = AlgorithmIdentifier.GetInstance(seq[0]);
			this.signatureValue = (DerBitString)seq[1];
			if (seq.Count == 3)
			{
				this.certs = Asn1Sequence.GetInstance((Asn1TaggedObject)seq[2], true);
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06004227 RID: 16935 RVA: 0x0018506C File Offset: 0x0018326C
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.signatureAlgorithm;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06004228 RID: 16936 RVA: 0x00185074 File Offset: 0x00183274
		public DerBitString SignatureValue
		{
			get
			{
				return this.signatureValue;
			}
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x0018507C File Offset: 0x0018327C
		public byte[] GetSignatureOctets()
		{
			return this.signatureValue.GetOctets();
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x0600422A RID: 16938 RVA: 0x00185089 File Offset: 0x00183289
		public Asn1Sequence Certs
		{
			get
			{
				return this.certs;
			}
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x00185094 File Offset: 0x00183294
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.signatureAlgorithm,
				this.signatureValue
			});
			if (this.certs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.certs)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AD0 RID: 10960
		internal AlgorithmIdentifier signatureAlgorithm;

		// Token: 0x04002AD1 RID: 10961
		internal DerBitString signatureValue;

		// Token: 0x04002AD2 RID: 10962
		internal Asn1Sequence certs;
	}
}
