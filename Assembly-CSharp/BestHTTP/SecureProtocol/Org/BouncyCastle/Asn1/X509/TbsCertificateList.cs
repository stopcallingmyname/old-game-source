using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B8 RID: 1720
	public class TbsCertificateList : Asn1Encodable
	{
		// Token: 0x06003F62 RID: 16226 RVA: 0x0017A218 File Offset: 0x00178418
		public static TbsCertificateList GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return TbsCertificateList.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0017A228 File Offset: 0x00178428
		public static TbsCertificateList GetInstance(object obj)
		{
			TbsCertificateList tbsCertificateList = obj as TbsCertificateList;
			if (obj == null || tbsCertificateList != null)
			{
				return tbsCertificateList;
			}
			if (obj is Asn1Sequence)
			{
				return new TbsCertificateList((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x0017A274 File Offset: 0x00178474
		internal TbsCertificateList(Asn1Sequence seq)
		{
			if (seq.Count < 3 || seq.Count > 7)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			int num = 0;
			this.seq = seq;
			if (seq[num] is DerInteger)
			{
				this.version = DerInteger.GetInstance(seq[num++]);
			}
			else
			{
				this.version = new DerInteger(0);
			}
			this.signature = AlgorithmIdentifier.GetInstance(seq[num++]);
			this.issuer = X509Name.GetInstance(seq[num++]);
			this.thisUpdate = Time.GetInstance(seq[num++]);
			if (num < seq.Count && (seq[num] is DerUtcTime || seq[num] is DerGeneralizedTime || seq[num] is Time))
			{
				this.nextUpdate = Time.GetInstance(seq[num++]);
			}
			if (num < seq.Count && !(seq[num] is DerTaggedObject))
			{
				this.revokedCertificates = Asn1Sequence.GetInstance(seq[num++]);
			}
			if (num < seq.Count && seq[num] is DerTaggedObject)
			{
				this.crlExtensions = X509Extensions.GetInstance(seq[num]);
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x0017A3D0 File Offset: 0x001785D0
		public int Version
		{
			get
			{
				return this.version.Value.IntValue + 1;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x0017A3E4 File Offset: 0x001785E4
		public DerInteger VersionNumber
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003F67 RID: 16231 RVA: 0x0017A3EC File Offset: 0x001785EC
		public AlgorithmIdentifier Signature
		{
			get
			{
				return this.signature;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x0017A3F4 File Offset: 0x001785F4
		public X509Name Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003F69 RID: 16233 RVA: 0x0017A3FC File Offset: 0x001785FC
		public Time ThisUpdate
		{
			get
			{
				return this.thisUpdate;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x0017A404 File Offset: 0x00178604
		public Time NextUpdate
		{
			get
			{
				return this.nextUpdate;
			}
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x0017A40C File Offset: 0x0017860C
		public CrlEntry[] GetRevokedCertificates()
		{
			if (this.revokedCertificates == null)
			{
				return new CrlEntry[0];
			}
			CrlEntry[] array = new CrlEntry[this.revokedCertificates.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new CrlEntry(Asn1Sequence.GetInstance(this.revokedCertificates[i]));
			}
			return array;
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x0017A461 File Offset: 0x00178661
		public IEnumerable GetRevokedCertificateEnumeration()
		{
			if (this.revokedCertificates == null)
			{
				return EmptyEnumerable.Instance;
			}
			return new TbsCertificateList.RevokedCertificatesEnumeration(this.revokedCertificates);
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x0017A47C File Offset: 0x0017867C
		public X509Extensions Extensions
		{
			get
			{
				return this.crlExtensions;
			}
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x0017A484 File Offset: 0x00178684
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x0400282B RID: 10283
		internal Asn1Sequence seq;

		// Token: 0x0400282C RID: 10284
		internal DerInteger version;

		// Token: 0x0400282D RID: 10285
		internal AlgorithmIdentifier signature;

		// Token: 0x0400282E RID: 10286
		internal X509Name issuer;

		// Token: 0x0400282F RID: 10287
		internal Time thisUpdate;

		// Token: 0x04002830 RID: 10288
		internal Time nextUpdate;

		// Token: 0x04002831 RID: 10289
		internal Asn1Sequence revokedCertificates;

		// Token: 0x04002832 RID: 10290
		internal X509Extensions crlExtensions;

		// Token: 0x020009A5 RID: 2469
		private class RevokedCertificatesEnumeration : IEnumerable
		{
			// Token: 0x0600501B RID: 20507 RVA: 0x001B8878 File Offset: 0x001B6A78
			internal RevokedCertificatesEnumeration(IEnumerable en)
			{
				this.en = en;
			}

			// Token: 0x0600501C RID: 20508 RVA: 0x001B8887 File Offset: 0x001B6A87
			public IEnumerator GetEnumerator()
			{
				return new TbsCertificateList.RevokedCertificatesEnumeration.RevokedCertificatesEnumerator(this.en.GetEnumerator());
			}

			// Token: 0x0400372B RID: 14123
			private readonly IEnumerable en;

			// Token: 0x02000A16 RID: 2582
			private class RevokedCertificatesEnumerator : IEnumerator
			{
				// Token: 0x060050E2 RID: 20706 RVA: 0x001BA58A File Offset: 0x001B878A
				internal RevokedCertificatesEnumerator(IEnumerator e)
				{
					this.e = e;
				}

				// Token: 0x060050E3 RID: 20707 RVA: 0x001BA599 File Offset: 0x001B8799
				public bool MoveNext()
				{
					return this.e.MoveNext();
				}

				// Token: 0x060050E4 RID: 20708 RVA: 0x001BA5A6 File Offset: 0x001B87A6
				public void Reset()
				{
					this.e.Reset();
				}

				// Token: 0x17000C6C RID: 3180
				// (get) Token: 0x060050E5 RID: 20709 RVA: 0x001BA5B3 File Offset: 0x001B87B3
				public object Current
				{
					get
					{
						return new CrlEntry(Asn1Sequence.GetInstance(this.e.Current));
					}
				}

				// Token: 0x040037E6 RID: 14310
				private readonly IEnumerator e;
			}
		}
	}
}
