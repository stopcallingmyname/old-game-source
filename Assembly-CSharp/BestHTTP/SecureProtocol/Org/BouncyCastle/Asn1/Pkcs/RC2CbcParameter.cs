using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006FE RID: 1790
	public class RC2CbcParameter : Asn1Encodable
	{
		// Token: 0x06004171 RID: 16753 RVA: 0x0018306E File Offset: 0x0018126E
		public static RC2CbcParameter GetInstance(object obj)
		{
			if (obj is Asn1Sequence)
			{
				return new RC2CbcParameter((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x0018309E File Offset: 0x0018129E
		public RC2CbcParameter(byte[] iv)
		{
			this.iv = new DerOctetString(iv);
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x001830B2 File Offset: 0x001812B2
		public RC2CbcParameter(int parameterVersion, byte[] iv)
		{
			this.version = new DerInteger(parameterVersion);
			this.iv = new DerOctetString(iv);
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x001830D4 File Offset: 0x001812D4
		private RC2CbcParameter(Asn1Sequence seq)
		{
			if (seq.Count == 1)
			{
				this.iv = (Asn1OctetString)seq[0];
				return;
			}
			this.version = (DerInteger)seq[0];
			this.iv = (Asn1OctetString)seq[1];
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06004175 RID: 16757 RVA: 0x00183127 File Offset: 0x00181327
		public BigInteger RC2ParameterVersion
		{
			get
			{
				if (this.version != null)
				{
					return this.version.Value;
				}
				return null;
			}
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x0018313E File Offset: 0x0018133E
		public byte[] GetIV()
		{
			return Arrays.Clone(this.iv.GetOctets());
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x00183150 File Offset: 0x00181350
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.version != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.version
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.iv
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002A6A RID: 10858
		internal DerInteger version;

		// Token: 0x04002A6B RID: 10859
		internal Asn1OctetString iv;
	}
}
