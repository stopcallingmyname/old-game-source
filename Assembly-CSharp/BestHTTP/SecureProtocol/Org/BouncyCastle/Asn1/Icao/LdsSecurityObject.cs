using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Icao
{
	// Token: 0x02000733 RID: 1843
	public class LdsSecurityObject : Asn1Encodable
	{
		// Token: 0x060042CC RID: 17100 RVA: 0x00187B94 File Offset: 0x00185D94
		public static LdsSecurityObject GetInstance(object obj)
		{
			if (obj is LdsSecurityObject)
			{
				return (LdsSecurityObject)obj;
			}
			if (obj != null)
			{
				return new LdsSecurityObject(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x00187BB8 File Offset: 0x00185DB8
		private LdsSecurityObject(Asn1Sequence seq)
		{
			if (seq == null || seq.Count == 0)
			{
				throw new ArgumentException("null or empty sequence passed.");
			}
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.version = DerInteger.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			this.digestAlgorithmIdentifier = AlgorithmIdentifier.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			Asn1Sequence instance = Asn1Sequence.GetInstance(enumerator.Current);
			if (this.version.Value.Equals(BigInteger.One))
			{
				enumerator.MoveNext();
				this.versionInfo = LdsVersionInfo.GetInstance(enumerator.Current);
			}
			this.CheckDatagroupHashSeqSize(instance.Count);
			this.datagroupHash = new DataGroupHash[instance.Count];
			for (int i = 0; i < instance.Count; i++)
			{
				this.datagroupHash[i] = DataGroupHash.GetInstance(instance[i]);
			}
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x00187CA8 File Offset: 0x00185EA8
		public LdsSecurityObject(AlgorithmIdentifier digestAlgorithmIdentifier, DataGroupHash[] datagroupHash)
		{
			this.version = new DerInteger(0);
			this.digestAlgorithmIdentifier = digestAlgorithmIdentifier;
			this.datagroupHash = datagroupHash;
			this.CheckDatagroupHashSeqSize(datagroupHash.Length);
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x00187CDF File Offset: 0x00185EDF
		public LdsSecurityObject(AlgorithmIdentifier digestAlgorithmIdentifier, DataGroupHash[] datagroupHash, LdsVersionInfo versionInfo)
		{
			this.version = new DerInteger(1);
			this.digestAlgorithmIdentifier = digestAlgorithmIdentifier;
			this.datagroupHash = datagroupHash;
			this.versionInfo = versionInfo;
			this.CheckDatagroupHashSeqSize(datagroupHash.Length);
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x00187D1D File Offset: 0x00185F1D
		private void CheckDatagroupHashSeqSize(int size)
		{
			if (size < 2 || size > 16)
			{
				throw new ArgumentException("wrong size in DataGroupHashValues : not in (2.." + 16 + ")");
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060042D1 RID: 17105 RVA: 0x00187D44 File Offset: 0x00185F44
		public BigInteger Version
		{
			get
			{
				return this.version.Value;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060042D2 RID: 17106 RVA: 0x00187D51 File Offset: 0x00185F51
		public AlgorithmIdentifier DigestAlgorithmIdentifier
		{
			get
			{
				return this.digestAlgorithmIdentifier;
			}
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00187D59 File Offset: 0x00185F59
		public DataGroupHash[] GetDatagroupHash()
		{
			return this.datagroupHash;
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060042D4 RID: 17108 RVA: 0x00187D61 File Offset: 0x00185F61
		public LdsVersionInfo VersionInfo
		{
			get
			{
				return this.versionInfo;
			}
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x00187D6C File Offset: 0x00185F6C
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] v = this.datagroupHash;
			DerSequence derSequence = new DerSequence(v);
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.digestAlgorithmIdentifier,
				derSequence
			});
			if (this.versionInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.versionInfo
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002BA1 RID: 11169
		public const int UBDataGroups = 16;

		// Token: 0x04002BA2 RID: 11170
		private DerInteger version = new DerInteger(0);

		// Token: 0x04002BA3 RID: 11171
		private AlgorithmIdentifier digestAlgorithmIdentifier;

		// Token: 0x04002BA4 RID: 11172
		private DataGroupHash[] datagroupHash;

		// Token: 0x04002BA5 RID: 11173
		private LdsVersionInfo versionInfo;
	}
}
