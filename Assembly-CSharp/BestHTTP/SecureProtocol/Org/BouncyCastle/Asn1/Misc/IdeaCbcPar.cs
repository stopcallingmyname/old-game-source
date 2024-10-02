using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x0200071D RID: 1821
	public class IdeaCbcPar : Asn1Encodable
	{
		// Token: 0x06004257 RID: 16983 RVA: 0x00185C7F File Offset: 0x00183E7F
		public static IdeaCbcPar GetInstance(object o)
		{
			if (o is IdeaCbcPar)
			{
				return (IdeaCbcPar)o;
			}
			if (o is Asn1Sequence)
			{
				return new IdeaCbcPar((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in IDEACBCPar factory");
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00185CAE File Offset: 0x00183EAE
		public IdeaCbcPar(byte[] iv)
		{
			this.iv = new DerOctetString(iv);
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00185CC2 File Offset: 0x00183EC2
		private IdeaCbcPar(Asn1Sequence seq)
		{
			if (seq.Count == 1)
			{
				this.iv = (Asn1OctetString)seq[0];
			}
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x00185CE5 File Offset: 0x00183EE5
		public byte[] GetIV()
		{
			if (this.iv != null)
			{
				return this.iv.GetOctets();
			}
			return null;
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x00185CFC File Offset: 0x00183EFC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.iv != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.iv
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002B18 RID: 11032
		internal Asn1OctetString iv;
	}
}
