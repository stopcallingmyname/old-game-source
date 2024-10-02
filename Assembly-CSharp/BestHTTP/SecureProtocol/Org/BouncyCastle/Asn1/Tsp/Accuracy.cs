using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp
{
	// Token: 0x020006D9 RID: 1753
	public class Accuracy : Asn1Encodable
	{
		// Token: 0x06004081 RID: 16513 RVA: 0x0017F560 File Offset: 0x0017D760
		public Accuracy(DerInteger seconds, DerInteger millis, DerInteger micros)
		{
			if (millis != null && (millis.Value.IntValue < 1 || millis.Value.IntValue > 999))
			{
				throw new ArgumentException("Invalid millis field : not in (1..999)");
			}
			if (micros != null && (micros.Value.IntValue < 1 || micros.Value.IntValue > 999))
			{
				throw new ArgumentException("Invalid micros field : not in (1..999)");
			}
			this.seconds = seconds;
			this.millis = millis;
			this.micros = micros;
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x0017F5E4 File Offset: 0x0017D7E4
		private Accuracy(Asn1Sequence seq)
		{
			for (int i = 0; i < seq.Count; i++)
			{
				if (seq[i] is DerInteger)
				{
					this.seconds = (DerInteger)seq[i];
				}
				else if (seq[i] is DerTaggedObject)
				{
					DerTaggedObject derTaggedObject = (DerTaggedObject)seq[i];
					int tagNo = derTaggedObject.TagNo;
					if (tagNo != 0)
					{
						if (tagNo != 1)
						{
							throw new ArgumentException("Invalig tag number");
						}
						this.micros = DerInteger.GetInstance(derTaggedObject, false);
						if (this.micros.Value.IntValue < 1 || this.micros.Value.IntValue > 999)
						{
							throw new ArgumentException("Invalid micros field : not in (1..999).");
						}
					}
					else
					{
						this.millis = DerInteger.GetInstance(derTaggedObject, false);
						if (this.millis.Value.IntValue < 1 || this.millis.Value.IntValue > 999)
						{
							throw new ArgumentException("Invalid millis field : not in (1..999).");
						}
					}
				}
			}
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x0017F6F3 File Offset: 0x0017D8F3
		public static Accuracy GetInstance(object o)
		{
			if (o == null || o is Accuracy)
			{
				return (Accuracy)o;
			}
			if (o is Asn1Sequence)
			{
				return new Accuracy((Asn1Sequence)o);
			}
			throw new ArgumentException("Unknown object in 'Accuracy' factory: " + Platform.GetTypeName(o));
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x0017F730 File Offset: 0x0017D930
		public DerInteger Seconds
		{
			get
			{
				return this.seconds;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06004085 RID: 16517 RVA: 0x0017F738 File Offset: 0x0017D938
		public DerInteger Millis
		{
			get
			{
				return this.millis;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x0017F740 File Offset: 0x0017D940
		public DerInteger Micros
		{
			get
			{
				return this.micros;
			}
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0017F748 File Offset: 0x0017D948
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.seconds != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.seconds
				});
			}
			if (this.millis != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.millis)
				});
			}
			if (this.micros != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.micros)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002924 RID: 10532
		private readonly DerInteger seconds;

		// Token: 0x04002925 RID: 10533
		private readonly DerInteger millis;

		// Token: 0x04002926 RID: 10534
		private readonly DerInteger micros;

		// Token: 0x04002927 RID: 10535
		protected const int MinMillis = 1;

		// Token: 0x04002928 RID: 10536
		protected const int MaxMillis = 999;

		// Token: 0x04002929 RID: 10537
		protected const int MinMicros = 1;

		// Token: 0x0400292A RID: 10538
		protected const int MaxMicros = 999;
	}
}
