using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000751 RID: 1873
	public class OtherHashAlgAndValue : Asn1Encodable
	{
		// Token: 0x06004387 RID: 17287 RVA: 0x0018A914 File Offset: 0x00188B14
		public static OtherHashAlgAndValue GetInstance(object obj)
		{
			if (obj == null || obj is OtherHashAlgAndValue)
			{
				return (OtherHashAlgAndValue)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OtherHashAlgAndValue((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OtherHashAlgAndValue' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0018A964 File Offset: 0x00188B64
		private OtherHashAlgAndValue(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.hashAlgorithm = AlgorithmIdentifier.GetInstance(seq[0].ToAsn1Object());
			this.hashValue = (Asn1OctetString)seq[1].ToAsn1Object();
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x0018A9DC File Offset: 0x00188BDC
		public OtherHashAlgAndValue(AlgorithmIdentifier hashAlgorithm, byte[] hashValue)
		{
			if (hashAlgorithm == null)
			{
				throw new ArgumentNullException("hashAlgorithm");
			}
			if (hashValue == null)
			{
				throw new ArgumentNullException("hashValue");
			}
			this.hashAlgorithm = hashAlgorithm;
			this.hashValue = new DerOctetString(hashValue);
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x0018AA13 File Offset: 0x00188C13
		public OtherHashAlgAndValue(AlgorithmIdentifier hashAlgorithm, Asn1OctetString hashValue)
		{
			if (hashAlgorithm == null)
			{
				throw new ArgumentNullException("hashAlgorithm");
			}
			if (hashValue == null)
			{
				throw new ArgumentNullException("hashValue");
			}
			this.hashAlgorithm = hashAlgorithm;
			this.hashValue = hashValue;
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x0600438B RID: 17291 RVA: 0x0018AA45 File Offset: 0x00188C45
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0018AA4D File Offset: 0x00188C4D
		public byte[] GetHashValue()
		{
			return this.hashValue.GetOctets();
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0018AA5A File Offset: 0x00188C5A
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.hashAlgorithm,
				this.hashValue
			});
		}

		// Token: 0x04002C48 RID: 11336
		private readonly AlgorithmIdentifier hashAlgorithm;

		// Token: 0x04002C49 RID: 11337
		private readonly Asn1OctetString hashValue;
	}
}
