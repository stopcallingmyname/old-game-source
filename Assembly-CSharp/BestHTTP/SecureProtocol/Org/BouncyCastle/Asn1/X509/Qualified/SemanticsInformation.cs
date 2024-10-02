using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006D3 RID: 1747
	public class SemanticsInformation : Asn1Encodable
	{
		// Token: 0x06004052 RID: 16466 RVA: 0x0017E198 File Offset: 0x0017C398
		public static SemanticsInformation GetInstance(object obj)
		{
			if (obj == null || obj is SemanticsInformation)
			{
				return (SemanticsInformation)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SemanticsInformation(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0017E1E8 File Offset: 0x0017C3E8
		public SemanticsInformation(Asn1Sequence seq)
		{
			if (seq.Count < 1)
			{
				throw new ArgumentException("no objects in SemanticsInformation");
			}
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			object obj = enumerator.Current;
			if (obj is DerObjectIdentifier)
			{
				this.semanticsIdentifier = DerObjectIdentifier.GetInstance(obj);
				if (enumerator.MoveNext())
				{
					obj = enumerator.Current;
				}
				else
				{
					obj = null;
				}
			}
			if (obj != null)
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(obj);
				this.nameRegistrationAuthorities = new GeneralName[instance.Count];
				for (int i = 0; i < instance.Count; i++)
				{
					this.nameRegistrationAuthorities[i] = GeneralName.GetInstance(instance[i]);
				}
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0017E28B File Offset: 0x0017C48B
		public SemanticsInformation(DerObjectIdentifier semanticsIdentifier, GeneralName[] generalNames)
		{
			this.semanticsIdentifier = semanticsIdentifier;
			this.nameRegistrationAuthorities = generalNames;
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0017E2A1 File Offset: 0x0017C4A1
		public SemanticsInformation(DerObjectIdentifier semanticsIdentifier)
		{
			this.semanticsIdentifier = semanticsIdentifier;
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0017E2B0 File Offset: 0x0017C4B0
		public SemanticsInformation(GeneralName[] generalNames)
		{
			this.nameRegistrationAuthorities = generalNames;
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x0017E2BF File Offset: 0x0017C4BF
		public DerObjectIdentifier SemanticsIdentifier
		{
			get
			{
				return this.semanticsIdentifier;
			}
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0017E2C7 File Offset: 0x0017C4C7
		public GeneralName[] GetNameRegistrationAuthorities()
		{
			return this.nameRegistrationAuthorities;
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x0017E2D0 File Offset: 0x0017C4D0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.semanticsIdentifier != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.semanticsIdentifier
				});
			}
			if (this.nameRegistrationAuthorities != null)
			{
				Asn1EncodableVector asn1EncodableVector2 = asn1EncodableVector;
				Asn1Encodable[] array = new Asn1Encodable[1];
				int num = 0;
				Asn1Encodable[] v = this.nameRegistrationAuthorities;
				array[num] = new DerSequence(v);
				asn1EncodableVector2.Add(array);
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040028F3 RID: 10483
		private readonly DerObjectIdentifier semanticsIdentifier;

		// Token: 0x040028F4 RID: 10484
		private readonly GeneralName[] nameRegistrationAuthorities;
	}
}
