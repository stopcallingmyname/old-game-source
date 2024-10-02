using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F6 RID: 1782
	public class MacData : Asn1Encodable
	{
		// Token: 0x06004136 RID: 16694 RVA: 0x00181E45 File Offset: 0x00180045
		public static MacData GetInstance(object obj)
		{
			if (obj is MacData)
			{
				return (MacData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new MacData((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00181E84 File Offset: 0x00180084
		private MacData(Asn1Sequence seq)
		{
			this.digInfo = DigestInfo.GetInstance(seq[0]);
			this.salt = ((Asn1OctetString)seq[1]).GetOctets();
			if (seq.Count == 3)
			{
				this.iterationCount = ((DerInteger)seq[2]).Value;
				return;
			}
			this.iterationCount = BigInteger.One;
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00181EEC File Offset: 0x001800EC
		public MacData(DigestInfo digInfo, byte[] salt, int iterationCount)
		{
			this.digInfo = digInfo;
			this.salt = (byte[])salt.Clone();
			this.iterationCount = BigInteger.ValueOf((long)iterationCount);
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06004139 RID: 16697 RVA: 0x00181F19 File Offset: 0x00180119
		public DigestInfo Mac
		{
			get
			{
				return this.digInfo;
			}
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x00181F21 File Offset: 0x00180121
		public byte[] GetSalt()
		{
			return (byte[])this.salt.Clone();
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x0600413B RID: 16699 RVA: 0x00181F33 File Offset: 0x00180133
		public BigInteger IterationCount
		{
			get
			{
				return this.iterationCount;
			}
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00181F3C File Offset: 0x0018013C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.digInfo,
				new DerOctetString(this.salt)
			});
			if (!this.iterationCount.Equals(BigInteger.One))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerInteger(this.iterationCount)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040029C5 RID: 10693
		internal DigestInfo digInfo;

		// Token: 0x040029C6 RID: 10694
		internal byte[] salt;

		// Token: 0x040029C7 RID: 10695
		internal BigInteger iterationCount;
	}
}
