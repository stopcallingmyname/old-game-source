using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B3 RID: 1715
	public class Target : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003F3C RID: 16188 RVA: 0x00179C4D File Offset: 0x00177E4D
		public static Target GetInstance(object obj)
		{
			if (obj is Target)
			{
				return (Target)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new Target((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x00179C8C File Offset: 0x00177E8C
		private Target(Asn1TaggedObject tagObj)
		{
			Target.Choice tagNo = (Target.Choice)tagObj.TagNo;
			if (tagNo == Target.Choice.Name)
			{
				this.targetName = GeneralName.GetInstance(tagObj, true);
				return;
			}
			if (tagNo != Target.Choice.Group)
			{
				throw new ArgumentException("unknown tag: " + tagObj.TagNo);
			}
			this.targetGroup = GeneralName.GetInstance(tagObj, true);
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x00179CE5 File Offset: 0x00177EE5
		public Target(Target.Choice type, GeneralName name) : this(new DerTaggedObject((int)type, name))
		{
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06003F3F RID: 16191 RVA: 0x00179CF4 File Offset: 0x00177EF4
		public virtual GeneralName TargetGroup
		{
			get
			{
				return this.targetGroup;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x00179CFC File Offset: 0x00177EFC
		public virtual GeneralName TargetName
		{
			get
			{
				return this.targetName;
			}
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x00179D04 File Offset: 0x00177F04
		public override Asn1Object ToAsn1Object()
		{
			if (this.targetName != null)
			{
				return new DerTaggedObject(true, 0, this.targetName);
			}
			return new DerTaggedObject(true, 1, this.targetGroup);
		}

		// Token: 0x04002817 RID: 10263
		private readonly GeneralName targetName;

		// Token: 0x04002818 RID: 10264
		private readonly GeneralName targetGroup;

		// Token: 0x020009A4 RID: 2468
		public enum Choice
		{
			// Token: 0x04003729 RID: 14121
			Name,
			// Token: 0x0400372A RID: 14122
			Group
		}
	}
}
