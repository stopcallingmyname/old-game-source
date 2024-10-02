using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002AA RID: 682
	public class TimeStampTokenInfo
	{
		// Token: 0x060018E1 RID: 6369 RVA: 0x000BB054 File Offset: 0x000B9254
		public TimeStampTokenInfo(TstInfo tstInfo)
		{
			this.tstInfo = tstInfo;
			try
			{
				this.genTime = tstInfo.GenTime.ToDateTime();
			}
			catch (Exception ex)
			{
				throw new TspException("unable to parse genTime field: " + ex.Message);
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x000BB0A8 File Offset: 0x000B92A8
		public bool IsOrdered
		{
			get
			{
				return this.tstInfo.Ordering.IsTrue;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x000BB0BA File Offset: 0x000B92BA
		public Accuracy Accuracy
		{
			get
			{
				return this.tstInfo.Accuracy;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x000BB0C7 File Offset: 0x000B92C7
		public DateTime GenTime
		{
			get
			{
				return this.genTime;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x000BB0CF File Offset: 0x000B92CF
		public GenTimeAccuracy GenTimeAccuracy
		{
			get
			{
				if (this.Accuracy != null)
				{
					return new GenTimeAccuracy(this.Accuracy);
				}
				return null;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x000BB0E6 File Offset: 0x000B92E6
		public string Policy
		{
			get
			{
				return this.tstInfo.Policy.Id;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x000BB0F8 File Offset: 0x000B92F8
		public BigInteger SerialNumber
		{
			get
			{
				return this.tstInfo.SerialNumber.Value;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x000BB10A File Offset: 0x000B930A
		public GeneralName Tsa
		{
			get
			{
				return this.tstInfo.Tsa;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060018E9 RID: 6377 RVA: 0x000BB117 File Offset: 0x000B9317
		public BigInteger Nonce
		{
			get
			{
				if (this.tstInfo.Nonce != null)
				{
					return this.tstInfo.Nonce.Value;
				}
				return null;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x000BB138 File Offset: 0x000B9338
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.tstInfo.MessageImprint.HashAlgorithm;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x000BB14A File Offset: 0x000B934A
		public string MessageImprintAlgOid
		{
			get
			{
				return this.tstInfo.MessageImprint.HashAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x000BB166 File Offset: 0x000B9366
		public byte[] GetMessageImprintDigest()
		{
			return this.tstInfo.MessageImprint.GetHashedMessage();
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x000BB178 File Offset: 0x000B9378
		public byte[] GetEncoded()
		{
			return this.tstInfo.GetEncoded();
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x000BB185 File Offset: 0x000B9385
		public TstInfo TstInfo
		{
			get
			{
				return this.tstInfo;
			}
		}

		// Token: 0x04001856 RID: 6230
		private TstInfo tstInfo;

		// Token: 0x04001857 RID: 6231
		private DateTime genTime;
	}
}
