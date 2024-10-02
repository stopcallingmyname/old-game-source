using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000686 RID: 1670
	public class AttCertIssuer : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003DE2 RID: 15842 RVA: 0x00175750 File Offset: 0x00173950
		public static AttCertIssuer GetInstance(object obj)
		{
			if (obj is AttCertIssuer)
			{
				return (AttCertIssuer)obj;
			}
			if (obj is V2Form)
			{
				return new AttCertIssuer(V2Form.GetInstance(obj));
			}
			if (obj is GeneralNames)
			{
				return new AttCertIssuer((GeneralNames)obj);
			}
			if (obj is Asn1TaggedObject)
			{
				return new AttCertIssuer(V2Form.GetInstance((Asn1TaggedObject)obj, false));
			}
			if (obj is Asn1Sequence)
			{
				return new AttCertIssuer(GeneralNames.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x001757DC File Offset: 0x001739DC
		public static AttCertIssuer GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return AttCertIssuer.GetInstance(obj.GetObject());
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x001757E9 File Offset: 0x001739E9
		public AttCertIssuer(GeneralNames names)
		{
			this.obj = names;
			this.choiceObj = this.obj.ToAsn1Object();
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00175809 File Offset: 0x00173A09
		public AttCertIssuer(V2Form v2Form)
		{
			this.obj = v2Form;
			this.choiceObj = new DerTaggedObject(false, 0, this.obj);
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x0017582B File Offset: 0x00173A2B
		public Asn1Encodable Issuer
		{
			get
			{
				return this.obj;
			}
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00175833 File Offset: 0x00173A33
		public override Asn1Object ToAsn1Object()
		{
			return this.choiceObj;
		}

		// Token: 0x04002778 RID: 10104
		internal readonly Asn1Encodable obj;

		// Token: 0x04002779 RID: 10105
		internal readonly Asn1Object choiceObj;
	}
}
