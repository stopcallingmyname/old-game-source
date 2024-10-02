using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000747 RID: 1863
	public class CrlIdentifier : Asn1Encodable
	{
		// Token: 0x06004348 RID: 17224 RVA: 0x00189AE8 File Offset: 0x00187CE8
		public static CrlIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is CrlIdentifier)
			{
				return (CrlIdentifier)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CrlIdentifier((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CrlIdentifier' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x00189B38 File Offset: 0x00187D38
		private CrlIdentifier(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 2 || seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.crlIssuer = X509Name.GetInstance(seq[0]);
			this.crlIssuedTime = DerUtcTime.GetInstance(seq[1]);
			if (seq.Count > 2)
			{
				this.crlNumber = DerInteger.GetInstance(seq[2]);
			}
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x00189BCA File Offset: 0x00187DCA
		public CrlIdentifier(X509Name crlIssuer, DateTime crlIssuedTime) : this(crlIssuer, crlIssuedTime, null)
		{
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x00189BD5 File Offset: 0x00187DD5
		public CrlIdentifier(X509Name crlIssuer, DateTime crlIssuedTime, BigInteger crlNumber)
		{
			if (crlIssuer == null)
			{
				throw new ArgumentNullException("crlIssuer");
			}
			this.crlIssuer = crlIssuer;
			this.crlIssuedTime = new DerUtcTime(crlIssuedTime);
			if (crlNumber != null)
			{
				this.crlNumber = new DerInteger(crlNumber);
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x0600434C RID: 17228 RVA: 0x00189C0D File Offset: 0x00187E0D
		public X509Name CrlIssuer
		{
			get
			{
				return this.crlIssuer;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600434D RID: 17229 RVA: 0x00189C15 File Offset: 0x00187E15
		public DateTime CrlIssuedTime
		{
			get
			{
				return this.crlIssuedTime.ToAdjustedDateTime();
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x0600434E RID: 17230 RVA: 0x00189C22 File Offset: 0x00187E22
		public BigInteger CrlNumber
		{
			get
			{
				if (this.crlNumber != null)
				{
					return this.crlNumber.Value;
				}
				return null;
			}
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x00189C3C File Offset: 0x00187E3C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.crlIssuer.ToAsn1Object(),
				this.crlIssuedTime
			});
			if (this.crlNumber != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.crlNumber
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C28 RID: 11304
		private readonly X509Name crlIssuer;

		// Token: 0x04002C29 RID: 11305
		private readonly DerUtcTime crlIssuedTime;

		// Token: 0x04002C2A RID: 11306
		private readonly DerInteger crlNumber;
	}
}
