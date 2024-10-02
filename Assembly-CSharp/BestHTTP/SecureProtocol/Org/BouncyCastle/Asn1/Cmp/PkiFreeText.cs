using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007BF RID: 1983
	public class PkiFreeText : Asn1Encodable
	{
		// Token: 0x06004698 RID: 18072 RVA: 0x00193CF6 File Offset: 0x00191EF6
		public static PkiFreeText GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return PkiFreeText.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x00193D04 File Offset: 0x00191F04
		public static PkiFreeText GetInstance(object obj)
		{
			if (obj is PkiFreeText)
			{
				return (PkiFreeText)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PkiFreeText((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x00193D44 File Offset: 0x00191F44
		public PkiFreeText(Asn1Sequence seq)
		{
			using (IEnumerator enumerator = seq.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current is DerUtf8String))
					{
						throw new ArgumentException("attempt to insert non UTF8 STRING into PkiFreeText");
					}
				}
			}
			this.strings = seq;
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x00193DAC File Offset: 0x00191FAC
		public PkiFreeText(DerUtf8String p)
		{
			this.strings = new DerSequence(p);
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x0600469C RID: 18076 RVA: 0x00193DC0 File Offset: 0x00191FC0
		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.strings.Count;
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x0600469D RID: 18077 RVA: 0x00193DC0 File Offset: 0x00191FC0
		public int Count
		{
			get
			{
				return this.strings.Count;
			}
		}

		// Token: 0x17000A56 RID: 2646
		public DerUtf8String this[int index]
		{
			get
			{
				return (DerUtf8String)this.strings[index];
			}
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x00193DE0 File Offset: 0x00191FE0
		[Obsolete("Use 'object[index]' syntax instead")]
		public DerUtf8String GetStringAt(int index)
		{
			return this[index];
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x00193DE9 File Offset: 0x00191FE9
		public override Asn1Object ToAsn1Object()
		{
			return this.strings;
		}

		// Token: 0x04002E1C RID: 11804
		internal Asn1Sequence strings;
	}
}
