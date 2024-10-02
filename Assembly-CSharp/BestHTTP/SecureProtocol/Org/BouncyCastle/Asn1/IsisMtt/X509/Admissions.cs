using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x02000726 RID: 1830
	public class Admissions : Asn1Encodable
	{
		// Token: 0x06004270 RID: 17008 RVA: 0x00186384 File Offset: 0x00184584
		public static Admissions GetInstance(object obj)
		{
			if (obj == null || obj is Admissions)
			{
				return (Admissions)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Admissions((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x001863D4 File Offset: 0x001845D4
		private Admissions(Asn1Sequence seq)
		{
			if (seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			Asn1Encodable asn1Encodable = (Asn1Encodable)enumerator.Current;
			if (asn1Encodable is Asn1TaggedObject)
			{
				int tagNo = ((Asn1TaggedObject)asn1Encodable).TagNo;
				if (tagNo != 0)
				{
					if (tagNo != 1)
					{
						throw new ArgumentException("Bad tag number: " + ((Asn1TaggedObject)asn1Encodable).TagNo);
					}
					this.namingAuthority = NamingAuthority.GetInstance((Asn1TaggedObject)asn1Encodable, true);
				}
				else
				{
					this.admissionAuthority = GeneralName.GetInstance((Asn1TaggedObject)asn1Encodable, true);
				}
				enumerator.MoveNext();
				asn1Encodable = (Asn1Encodable)enumerator.Current;
			}
			if (asn1Encodable is Asn1TaggedObject)
			{
				int tagNo = ((Asn1TaggedObject)asn1Encodable).TagNo;
				if (tagNo != 1)
				{
					throw new ArgumentException("Bad tag number: " + ((Asn1TaggedObject)asn1Encodable).TagNo);
				}
				this.namingAuthority = NamingAuthority.GetInstance((Asn1TaggedObject)asn1Encodable, true);
				enumerator.MoveNext();
				asn1Encodable = (Asn1Encodable)enumerator.Current;
			}
			this.professionInfos = Asn1Sequence.GetInstance(asn1Encodable);
			if (enumerator.MoveNext())
			{
				throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(enumerator.Current));
			}
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x0018652C File Offset: 0x0018472C
		public Admissions(GeneralName admissionAuthority, NamingAuthority namingAuthority, ProfessionInfo[] professionInfos)
		{
			this.admissionAuthority = admissionAuthority;
			this.namingAuthority = namingAuthority;
			this.professionInfos = new DerSequence(professionInfos);
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06004273 RID: 17011 RVA: 0x0018655B File Offset: 0x0018475B
		public virtual GeneralName AdmissionAuthority
		{
			get
			{
				return this.admissionAuthority;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06004274 RID: 17012 RVA: 0x00186563 File Offset: 0x00184763
		public virtual NamingAuthority NamingAuthority
		{
			get
			{
				return this.namingAuthority;
			}
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x0018656C File Offset: 0x0018476C
		public ProfessionInfo[] GetProfessionInfos()
		{
			ProfessionInfo[] array = new ProfessionInfo[this.professionInfos.Count];
			int num = 0;
			foreach (object obj in this.professionInfos)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				array[num++] = ProfessionInfo.GetInstance(obj2);
			}
			return array;
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x001865E4 File Offset: 0x001847E4
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.admissionAuthority != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.admissionAuthority)
				});
			}
			if (this.namingAuthority != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.namingAuthority)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.professionInfos
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002B64 RID: 11108
		private readonly GeneralName admissionAuthority;

		// Token: 0x04002B65 RID: 11109
		private readonly NamingAuthority namingAuthority;

		// Token: 0x04002B66 RID: 11110
		private readonly Asn1Sequence professionInfos;
	}
}
