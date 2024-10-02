using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A0 RID: 1696
	public class IetfAttrSyntax : Asn1Encodable
	{
		// Token: 0x06003EBC RID: 16060 RVA: 0x001781B4 File Offset: 0x001763B4
		public IetfAttrSyntax(Asn1Sequence seq)
		{
			int num = 0;
			if (seq[0] is Asn1TaggedObject)
			{
				this.policyAuthority = GeneralNames.GetInstance((Asn1TaggedObject)seq[0], false);
				num++;
			}
			else if (seq.Count == 2)
			{
				this.policyAuthority = GeneralNames.GetInstance(seq[0]);
				num++;
			}
			if (!(seq[num] is Asn1Sequence))
			{
				throw new ArgumentException("Non-IetfAttrSyntax encoding");
			}
			seq = (Asn1Sequence)seq[num];
			foreach (object obj in seq)
			{
				Asn1Object asn1Object = (Asn1Object)obj;
				int num2;
				if (asn1Object is DerObjectIdentifier)
				{
					num2 = 2;
				}
				else if (asn1Object is DerUtf8String)
				{
					num2 = 3;
				}
				else
				{
					if (!(asn1Object is DerOctetString))
					{
						throw new ArgumentException("Bad value type encoding IetfAttrSyntax");
					}
					num2 = 1;
				}
				if (this.valueChoice < 0)
				{
					this.valueChoice = num2;
				}
				if (num2 != this.valueChoice)
				{
					throw new ArgumentException("Mix of value types in IetfAttrSyntax");
				}
				this.values.Add(new Asn1Encodable[]
				{
					asn1Object
				});
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06003EBD RID: 16061 RVA: 0x00178300 File Offset: 0x00176500
		public GeneralNames PolicyAuthority
		{
			get
			{
				return this.policyAuthority;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003EBE RID: 16062 RVA: 0x00178308 File Offset: 0x00176508
		public int ValueType
		{
			get
			{
				return this.valueChoice;
			}
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x00178310 File Offset: 0x00176510
		public object[] GetValues()
		{
			if (this.ValueType == 1)
			{
				Asn1OctetString[] array = new Asn1OctetString[this.values.Count];
				for (int num = 0; num != array.Length; num++)
				{
					array[num] = (Asn1OctetString)this.values[num];
				}
				return array;
			}
			if (this.ValueType == 2)
			{
				DerObjectIdentifier[] array2 = new DerObjectIdentifier[this.values.Count];
				for (int num2 = 0; num2 != array2.Length; num2++)
				{
					array2[num2] = (DerObjectIdentifier)this.values[num2];
				}
				return array2;
			}
			DerUtf8String[] array3 = new DerUtf8String[this.values.Count];
			for (int num3 = 0; num3 != array3.Length; num3++)
			{
				array3[num3] = (DerUtf8String)this.values[num3];
			}
			return array3;
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x001783E4 File Offset: 0x001765E4
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.policyAuthority != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(0, this.policyAuthority)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new DerSequence(this.values)
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040027CB RID: 10187
		public const int ValueOctets = 1;

		// Token: 0x040027CC RID: 10188
		public const int ValueOid = 2;

		// Token: 0x040027CD RID: 10189
		public const int ValueUtf8 = 3;

		// Token: 0x040027CE RID: 10190
		internal readonly GeneralNames policyAuthority;

		// Token: 0x040027CF RID: 10191
		internal readonly Asn1EncodableVector values = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());

		// Token: 0x040027D0 RID: 10192
		internal int valueChoice = -1;
	}
}
