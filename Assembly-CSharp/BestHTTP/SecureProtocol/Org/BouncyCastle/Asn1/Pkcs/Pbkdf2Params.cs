using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F9 RID: 1785
	public class Pbkdf2Params : Asn1Encodable
	{
		// Token: 0x06004149 RID: 16713 RVA: 0x001821A0 File Offset: 0x001803A0
		public static Pbkdf2Params GetInstance(object obj)
		{
			if (obj == null || obj is Pbkdf2Params)
			{
				return (Pbkdf2Params)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Pbkdf2Params((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x001821F0 File Offset: 0x001803F0
		public Pbkdf2Params(Asn1Sequence seq)
		{
			if (seq.Count < 2 || seq.Count > 4)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.octStr = (Asn1OctetString)seq[0];
			this.iterationCount = (DerInteger)seq[1];
			Asn1Encodable asn1Encodable = null;
			Asn1Encodable asn1Encodable2 = null;
			if (seq.Count > 3)
			{
				asn1Encodable = seq[2];
				asn1Encodable2 = seq[3];
			}
			else if (seq.Count > 2)
			{
				if (seq[2] is DerInteger)
				{
					asn1Encodable = seq[2];
				}
				else
				{
					asn1Encodable2 = seq[2];
				}
			}
			if (asn1Encodable != null)
			{
				this.keyLength = (DerInteger)asn1Encodable;
			}
			if (asn1Encodable2 != null)
			{
				this.prf = AlgorithmIdentifier.GetInstance(asn1Encodable2);
			}
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x001822AF File Offset: 0x001804AF
		public Pbkdf2Params(byte[] salt, int iterationCount)
		{
			this.octStr = new DerOctetString(salt);
			this.iterationCount = new DerInteger(iterationCount);
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x001822CF File Offset: 0x001804CF
		public Pbkdf2Params(byte[] salt, int iterationCount, int keyLength) : this(salt, iterationCount)
		{
			this.keyLength = new DerInteger(keyLength);
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x001822E5 File Offset: 0x001804E5
		public Pbkdf2Params(byte[] salt, int iterationCount, int keyLength, AlgorithmIdentifier prf) : this(salt, iterationCount, keyLength)
		{
			this.prf = prf;
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x001822F8 File Offset: 0x001804F8
		public Pbkdf2Params(byte[] salt, int iterationCount, AlgorithmIdentifier prf) : this(salt, iterationCount)
		{
			this.prf = prf;
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x00182309 File Offset: 0x00180509
		public byte[] GetSalt()
		{
			return this.octStr.GetOctets();
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x00182316 File Offset: 0x00180516
		public BigInteger IterationCount
		{
			get
			{
				return this.iterationCount.Value;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x00182323 File Offset: 0x00180523
		public BigInteger KeyLength
		{
			get
			{
				if (this.keyLength != null)
				{
					return this.keyLength.Value;
				}
				return null;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x0018233A File Offset: 0x0018053A
		public bool IsDefaultPrf
		{
			get
			{
				return this.prf == null || this.prf.Equals(Pbkdf2Params.algid_hmacWithSHA1);
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06004153 RID: 16723 RVA: 0x00182356 File Offset: 0x00180556
		public AlgorithmIdentifier Prf
		{
			get
			{
				if (this.prf == null)
				{
					return Pbkdf2Params.algid_hmacWithSHA1;
				}
				return this.prf;
			}
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x0018236C File Offset: 0x0018056C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.octStr,
				this.iterationCount
			});
			if (this.keyLength != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.keyLength
				});
			}
			if (!this.IsDefaultPrf)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.prf
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040029CC RID: 10700
		private static AlgorithmIdentifier algid_hmacWithSHA1 = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdHmacWithSha1, DerNull.Instance);

		// Token: 0x040029CD RID: 10701
		private readonly Asn1OctetString octStr;

		// Token: 0x040029CE RID: 10702
		private readonly DerInteger iterationCount;

		// Token: 0x040029CF RID: 10703
		private readonly DerInteger keyLength;

		// Token: 0x040029D0 RID: 10704
		private readonly AlgorithmIdentifier prf;
	}
}
