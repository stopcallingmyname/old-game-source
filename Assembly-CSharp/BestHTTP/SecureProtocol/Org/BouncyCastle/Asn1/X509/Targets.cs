using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B5 RID: 1717
	public class Targets : Asn1Encodable
	{
		// Token: 0x06003F48 RID: 16200 RVA: 0x00179DEB File Offset: 0x00177FEB
		public static Targets GetInstance(object obj)
		{
			if (obj is Targets)
			{
				return (Targets)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Targets((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x00179E2A File Offset: 0x0017802A
		private Targets(Asn1Sequence targets)
		{
			this.targets = targets;
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x00179E3C File Offset: 0x0017803C
		public Targets(Target[] targets)
		{
			this.targets = new DerSequence(targets);
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x00179E60 File Offset: 0x00178060
		public virtual Target[] GetTargets()
		{
			Target[] array = new Target[this.targets.Count];
			for (int i = 0; i < this.targets.Count; i++)
			{
				array[i] = Target.GetInstance(this.targets[i]);
			}
			return array;
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00179EA9 File Offset: 0x001780A9
		public override Asn1Object ToAsn1Object()
		{
			return this.targets;
		}

		// Token: 0x0400281A RID: 10266
		private readonly Asn1Sequence targets;
	}
}
