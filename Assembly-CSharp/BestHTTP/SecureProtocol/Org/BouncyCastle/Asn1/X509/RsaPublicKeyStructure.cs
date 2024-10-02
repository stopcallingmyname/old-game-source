using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006AF RID: 1711
	public class RsaPublicKeyStructure : Asn1Encodable
	{
		// Token: 0x06003F1C RID: 16156 RVA: 0x0017973D File Offset: 0x0017793D
		public static RsaPublicKeyStructure GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return RsaPublicKeyStructure.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0017974B File Offset: 0x0017794B
		public static RsaPublicKeyStructure GetInstance(object obj)
		{
			if (obj == null || obj is RsaPublicKeyStructure)
			{
				return (RsaPublicKeyStructure)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RsaPublicKeyStructure((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid RsaPublicKeyStructure: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x00179788 File Offset: 0x00177988
		public RsaPublicKeyStructure(BigInteger modulus, BigInteger publicExponent)
		{
			if (modulus == null)
			{
				throw new ArgumentNullException("modulus");
			}
			if (publicExponent == null)
			{
				throw new ArgumentNullException("publicExponent");
			}
			if (modulus.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA modulus", "modulus");
			}
			if (publicExponent.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA public exponent", "publicExponent");
			}
			this.modulus = modulus;
			this.publicExponent = publicExponent;
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x001797F8 File Offset: 0x001779F8
		private RsaPublicKeyStructure(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.modulus = DerInteger.GetInstance(seq[0]).PositiveValue;
			this.publicExponent = DerInteger.GetInstance(seq[1]).PositiveValue;
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06003F20 RID: 16160 RVA: 0x0017985D File Offset: 0x00177A5D
		public BigInteger Modulus
		{
			get
			{
				return this.modulus;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x00179865 File Offset: 0x00177A65
		public BigInteger PublicExponent
		{
			get
			{
				return this.publicExponent;
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0017986D File Offset: 0x00177A6D
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				new DerInteger(this.Modulus),
				new DerInteger(this.PublicExponent)
			});
		}

		// Token: 0x04002811 RID: 10257
		private BigInteger modulus;

		// Token: 0x04002812 RID: 10258
		private BigInteger publicExponent;
	}
}
