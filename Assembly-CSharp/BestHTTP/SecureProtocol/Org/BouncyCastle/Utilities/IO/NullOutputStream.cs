using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200027B RID: 635
	internal class NullOutputStream : BaseOutputStream
	{
		// Token: 0x06001780 RID: 6016 RVA: 0x0000248C File Offset: 0x0000068C
		public override void WriteByte(byte b)
		{
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0000248C File Offset: 0x0000068C
		public override void Write(byte[] buffer, int offset, int count)
		{
		}
	}
}
