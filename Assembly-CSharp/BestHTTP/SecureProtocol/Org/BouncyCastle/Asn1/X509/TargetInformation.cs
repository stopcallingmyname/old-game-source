using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B4 RID: 1716
	public class TargetInformation : Asn1Encodable
	{
		// Token: 0x06003F42 RID: 16194 RVA: 0x00179D29 File Offset: 0x00177F29
		public static TargetInformation GetInstance(object obj)
		{
			if (obj is TargetInformation)
			{
				return (TargetInformation)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new TargetInformation((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x00179D68 File Offset: 0x00177F68
		private TargetInformation(Asn1Sequence targets)
		{
			this.targets = targets;
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x00179D78 File Offset: 0x00177F78
		public virtual Targets[] GetTargetsObjects()
		{
			Targets[] array = new Targets[this.targets.Count];
			for (int i = 0; i < this.targets.Count; i++)
			{
				array[i] = Targets.GetInstance(this.targets[i]);
			}
			return array;
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x00179DC1 File Offset: 0x00177FC1
		public TargetInformation(Targets targets)
		{
			this.targets = new DerSequence(targets);
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x00179DD5 File Offset: 0x00177FD5
		public TargetInformation(Target[] targets) : this(new Targets(targets))
		{
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x00179DE3 File Offset: 0x00177FE3
		public override Asn1Object ToAsn1Object()
		{
			return this.targets;
		}

		// Token: 0x04002819 RID: 10265
		private readonly Asn1Sequence targets;
	}
}
