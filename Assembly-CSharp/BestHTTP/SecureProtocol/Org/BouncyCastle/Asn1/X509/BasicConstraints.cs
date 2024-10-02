using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200068E RID: 1678
	public class BasicConstraints : Asn1Encodable
	{
		// Token: 0x06003E26 RID: 15910 RVA: 0x001762C6 File Offset: 0x001744C6
		public static BasicConstraints GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return BasicConstraints.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x001762D4 File Offset: 0x001744D4
		public static BasicConstraints GetInstance(object obj)
		{
			if (obj == null || obj is BasicConstraints)
			{
				return (BasicConstraints)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new BasicConstraints((Asn1Sequence)obj);
			}
			if (obj is X509Extension)
			{
				return BasicConstraints.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x0017633C File Offset: 0x0017453C
		private BasicConstraints(Asn1Sequence seq)
		{
			if (seq.Count > 0)
			{
				if (seq[0] is DerBoolean)
				{
					this.cA = DerBoolean.GetInstance(seq[0]);
				}
				else
				{
					this.pathLenConstraint = DerInteger.GetInstance(seq[0]);
				}
				if (seq.Count > 1)
				{
					if (this.cA == null)
					{
						throw new ArgumentException("wrong sequence in constructor", "seq");
					}
					this.pathLenConstraint = DerInteger.GetInstance(seq[1]);
				}
			}
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x001763BF File Offset: 0x001745BF
		public BasicConstraints(bool cA)
		{
			if (cA)
			{
				this.cA = DerBoolean.True;
			}
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x001763D5 File Offset: 0x001745D5
		public BasicConstraints(int pathLenConstraint)
		{
			this.cA = DerBoolean.True;
			this.pathLenConstraint = new DerInteger(pathLenConstraint);
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x001763F4 File Offset: 0x001745F4
		public bool IsCA()
		{
			return this.cA != null && this.cA.IsTrue;
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06003E2C RID: 15916 RVA: 0x0017640B File Offset: 0x0017460B
		public BigInteger PathLenConstraint
		{
			get
			{
				if (this.pathLenConstraint != null)
				{
					return this.pathLenConstraint.Value;
				}
				return null;
			}
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x00176424 File Offset: 0x00174624
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.cA != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.cA
				});
			}
			if (this.pathLenConstraint != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.pathLenConstraint
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x0017647C File Offset: 0x0017467C
		public override string ToString()
		{
			if (this.pathLenConstraint == null)
			{
				return "BasicConstraints: isCa(" + this.IsCA().ToString() + ")";
			}
			return string.Concat(new object[]
			{
				"BasicConstraints: isCa(",
				this.IsCA().ToString(),
				"), pathLenConstraint = ",
				this.pathLenConstraint.Value
			});
		}

		// Token: 0x0400278F RID: 10127
		private readonly DerBoolean cA;

		// Token: 0x04002790 RID: 10128
		private readonly DerInteger pathLenConstraint;
	}
}
