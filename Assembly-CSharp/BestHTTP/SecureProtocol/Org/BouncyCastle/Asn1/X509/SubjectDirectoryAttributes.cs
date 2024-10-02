using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B0 RID: 1712
	public class SubjectDirectoryAttributes : Asn1Encodable
	{
		// Token: 0x06003F23 RID: 16163 RVA: 0x00179898 File Offset: 0x00177A98
		public static SubjectDirectoryAttributes GetInstance(object obj)
		{
			if (obj == null || obj is SubjectDirectoryAttributes)
			{
				return (SubjectDirectoryAttributes)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SubjectDirectoryAttributes((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x001798E8 File Offset: 0x00177AE8
		private SubjectDirectoryAttributes(Asn1Sequence seq)
		{
			this.attributes = Platform.CreateArrayList();
			foreach (object obj in seq)
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(obj);
				this.attributes.Add(AttributeX509.GetInstance(instance));
			}
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x00179958 File Offset: 0x00177B58
		[Obsolete]
		public SubjectDirectoryAttributes(ArrayList attributes) : this(attributes)
		{
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x00179961 File Offset: 0x00177B61
		public SubjectDirectoryAttributes(IList attributes)
		{
			this.attributes = Platform.CreateArrayList(attributes);
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00179978 File Offset: 0x00177B78
		public override Asn1Object ToAsn1Object()
		{
			AttributeX509[] array = new AttributeX509[this.attributes.Count];
			for (int i = 0; i < this.attributes.Count; i++)
			{
				array[i] = (AttributeX509)this.attributes[i];
			}
			Asn1Encodable[] v = array;
			return new DerSequence(v);
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06003F28 RID: 16168 RVA: 0x001799C8 File Offset: 0x00177BC8
		public IEnumerable Attributes
		{
			get
			{
				return new EnumerableProxy(this.attributes);
			}
		}

		// Token: 0x04002813 RID: 10259
		private readonly IList attributes;
	}
}
