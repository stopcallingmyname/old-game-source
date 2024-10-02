using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x02000255 RID: 597
	public class X509AttrCertStoreSelector : IX509Selector, ICloneable
	{
		// Token: 0x060015A3 RID: 5539 RVA: 0x000AEC00 File Offset: 0x000ACE00
		public X509AttrCertStoreSelector()
		{
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x000AEC20 File Offset: 0x000ACE20
		private X509AttrCertStoreSelector(X509AttrCertStoreSelector o)
		{
			this.attributeCert = o.attributeCert;
			this.attributeCertificateValid = o.attributeCertificateValid;
			this.holder = o.holder;
			this.issuer = o.issuer;
			this.serialNumber = o.serialNumber;
			this.targetGroups = new HashSet(o.targetGroups);
			this.targetNames = new HashSet(o.targetNames);
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x000AECA8 File Offset: 0x000ACEA8
		public bool Match(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			IX509AttributeCertificate ix509AttributeCertificate = obj as IX509AttributeCertificate;
			if (ix509AttributeCertificate == null)
			{
				return false;
			}
			if (this.attributeCert != null && !this.attributeCert.Equals(ix509AttributeCertificate))
			{
				return false;
			}
			if (this.serialNumber != null && !ix509AttributeCertificate.SerialNumber.Equals(this.serialNumber))
			{
				return false;
			}
			if (this.holder != null && !ix509AttributeCertificate.Holder.Equals(this.holder))
			{
				return false;
			}
			if (this.issuer != null && !ix509AttributeCertificate.Issuer.Equals(this.issuer))
			{
				return false;
			}
			if (this.attributeCertificateValid != null && !ix509AttributeCertificate.IsValid(this.attributeCertificateValid.Value))
			{
				return false;
			}
			if (this.targetNames.Count > 0 || this.targetGroups.Count > 0)
			{
				Asn1OctetString extensionValue = ix509AttributeCertificate.GetExtensionValue(X509Extensions.TargetInformation);
				if (extensionValue != null)
				{
					TargetInformation instance;
					try
					{
						instance = TargetInformation.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
					}
					catch (Exception)
					{
						return false;
					}
					Targets[] targetsObjects = instance.GetTargetsObjects();
					if (this.targetNames.Count > 0)
					{
						bool flag = false;
						int num = 0;
						while (num < targetsObjects.Length && !flag)
						{
							Target[] targets = targetsObjects[num].GetTargets();
							for (int i = 0; i < targets.Length; i++)
							{
								GeneralName targetName = targets[i].TargetName;
								if (targetName != null && this.targetNames.Contains(targetName))
								{
									flag = true;
									break;
								}
							}
							num++;
						}
						if (!flag)
						{
							return false;
						}
					}
					if (this.targetGroups.Count <= 0)
					{
						return true;
					}
					bool flag2 = false;
					int num2 = 0;
					while (num2 < targetsObjects.Length && !flag2)
					{
						Target[] targets2 = targetsObjects[num2].GetTargets();
						for (int j = 0; j < targets2.Length; j++)
						{
							GeneralName targetGroup = targets2[j].TargetGroup;
							if (targetGroup != null && this.targetGroups.Contains(targetGroup))
							{
								flag2 = true;
								break;
							}
						}
						num2++;
					}
					if (!flag2)
					{
						return false;
					}
					return true;
				}
			}
			return true;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x000AEE9C File Offset: 0x000AD09C
		public object Clone()
		{
			return new X509AttrCertStoreSelector(this);
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x000AEEA4 File Offset: 0x000AD0A4
		// (set) Token: 0x060015A8 RID: 5544 RVA: 0x000AEEAC File Offset: 0x000AD0AC
		public IX509AttributeCertificate AttributeCert
		{
			get
			{
				return this.attributeCert;
			}
			set
			{
				this.attributeCert = value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x000AEEB5 File Offset: 0x000AD0B5
		// (set) Token: 0x060015AA RID: 5546 RVA: 0x000AEEBD File Offset: 0x000AD0BD
		[Obsolete("Use AttributeCertificateValid instead")]
		public DateTimeObject AttribueCertificateValid
		{
			get
			{
				return this.attributeCertificateValid;
			}
			set
			{
				this.attributeCertificateValid = value;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x000AEEB5 File Offset: 0x000AD0B5
		// (set) Token: 0x060015AC RID: 5548 RVA: 0x000AEEBD File Offset: 0x000AD0BD
		public DateTimeObject AttributeCertificateValid
		{
			get
			{
				return this.attributeCertificateValid;
			}
			set
			{
				this.attributeCertificateValid = value;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060015AD RID: 5549 RVA: 0x000AEEC6 File Offset: 0x000AD0C6
		// (set) Token: 0x060015AE RID: 5550 RVA: 0x000AEECE File Offset: 0x000AD0CE
		public AttributeCertificateHolder Holder
		{
			get
			{
				return this.holder;
			}
			set
			{
				this.holder = value;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060015AF RID: 5551 RVA: 0x000AEED7 File Offset: 0x000AD0D7
		// (set) Token: 0x060015B0 RID: 5552 RVA: 0x000AEEDF File Offset: 0x000AD0DF
		public AttributeCertificateIssuer Issuer
		{
			get
			{
				return this.issuer;
			}
			set
			{
				this.issuer = value;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060015B1 RID: 5553 RVA: 0x000AEEE8 File Offset: 0x000AD0E8
		// (set) Token: 0x060015B2 RID: 5554 RVA: 0x000AEEF0 File Offset: 0x000AD0F0
		public BigInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
			set
			{
				this.serialNumber = value;
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000AEEF9 File Offset: 0x000AD0F9
		public void AddTargetName(GeneralName name)
		{
			this.targetNames.Add(name);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000AEF07 File Offset: 0x000AD107
		public void AddTargetName(byte[] name)
		{
			this.AddTargetName(GeneralName.GetInstance(Asn1Object.FromByteArray(name)));
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000AEF1A File Offset: 0x000AD11A
		public void SetTargetNames(IEnumerable names)
		{
			this.targetNames = this.ExtractGeneralNames(names);
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000AEF29 File Offset: 0x000AD129
		public IEnumerable GetTargetNames()
		{
			return new EnumerableProxy(this.targetNames);
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x000AEF36 File Offset: 0x000AD136
		public void AddTargetGroup(GeneralName group)
		{
			this.targetGroups.Add(group);
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x000AEF44 File Offset: 0x000AD144
		public void AddTargetGroup(byte[] name)
		{
			this.AddTargetGroup(GeneralName.GetInstance(Asn1Object.FromByteArray(name)));
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x000AEF57 File Offset: 0x000AD157
		public void SetTargetGroups(IEnumerable names)
		{
			this.targetGroups = this.ExtractGeneralNames(names);
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x000AEF66 File Offset: 0x000AD166
		public IEnumerable GetTargetGroups()
		{
			return new EnumerableProxy(this.targetGroups);
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x000AEF74 File Offset: 0x000AD174
		private ISet ExtractGeneralNames(IEnumerable names)
		{
			ISet set = new HashSet();
			if (names != null)
			{
				foreach (object obj in names)
				{
					if (obj is GeneralName)
					{
						set.Add(obj);
					}
					else
					{
						set.Add(GeneralName.GetInstance(Asn1Object.FromByteArray((byte[])obj)));
					}
				}
			}
			return set;
		}

		// Token: 0x0400165D RID: 5725
		private IX509AttributeCertificate attributeCert;

		// Token: 0x0400165E RID: 5726
		private DateTimeObject attributeCertificateValid;

		// Token: 0x0400165F RID: 5727
		private AttributeCertificateHolder holder;

		// Token: 0x04001660 RID: 5728
		private AttributeCertificateIssuer issuer;

		// Token: 0x04001661 RID: 5729
		private BigInteger serialNumber;

		// Token: 0x04001662 RID: 5730
		private ISet targetNames = new HashSet();

		// Token: 0x04001663 RID: 5731
		private ISet targetGroups = new HashSet();
	}
}
