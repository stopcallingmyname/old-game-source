using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000450 RID: 1104
	public class SupplementalDataEntry
	{
		// Token: 0x06002B5B RID: 11099 RVA: 0x00114A4B File Offset: 0x00112C4B
		public SupplementalDataEntry(int dataType, byte[] data)
		{
			this.mDataType = dataType;
			this.mData = data;
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x00114A61 File Offset: 0x00112C61
		public virtual int DataType
		{
			get
			{
				return this.mDataType;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x00114A69 File Offset: 0x00112C69
		public virtual byte[] Data
		{
			get
			{
				return this.mData;
			}
		}

		// Token: 0x04001E03 RID: 7683
		protected readonly int mDataType;

		// Token: 0x04001E04 RID: 7684
		protected readonly byte[] mData;
	}
}
