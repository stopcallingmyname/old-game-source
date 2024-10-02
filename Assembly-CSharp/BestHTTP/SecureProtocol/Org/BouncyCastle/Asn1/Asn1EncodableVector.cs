using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000623 RID: 1571
	public class Asn1EncodableVector : IEnumerable
	{
		// Token: 0x06003B45 RID: 15173 RVA: 0x0016E298 File Offset: 0x0016C498
		public static Asn1EncodableVector FromEnumerable(IEnumerable e)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in e)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					asn1Encodable
				});
			}
			return asn1EncodableVector;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x0016E304 File Offset: 0x0016C504
		public Asn1EncodableVector(params Asn1Encodable[] v)
		{
			this.Add(v);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x0016E320 File Offset: 0x0016C520
		public void Add(params Asn1Encodable[] objs)
		{
			foreach (Asn1Encodable value in objs)
			{
				this.v.Add(value);
			}
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x0016E350 File Offset: 0x0016C550
		public void AddOptional(params Asn1Encodable[] objs)
		{
			if (objs != null)
			{
				foreach (Asn1Encodable asn1Encodable in objs)
				{
					if (asn1Encodable != null)
					{
						this.v.Add(asn1Encodable);
					}
				}
			}
		}

		// Token: 0x170007B0 RID: 1968
		public Asn1Encodable this[int index]
		{
			get
			{
				return (Asn1Encodable)this.v[index];
			}
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x0016E397 File Offset: 0x0016C597
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable Get(int index)
		{
			return this[index];
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06003B4B RID: 15179 RVA: 0x0016E3A0 File Offset: 0x0016C5A0
		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.v.Count;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x0016E3A0 File Offset: 0x0016C5A0
		public int Count
		{
			get
			{
				return this.v.Count;
			}
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x0016E3AD File Offset: 0x0016C5AD
		public IEnumerator GetEnumerator()
		{
			return this.v.GetEnumerator();
		}

		// Token: 0x04002697 RID: 9879
		private IList v = Platform.CreateArrayList();
	}
}
