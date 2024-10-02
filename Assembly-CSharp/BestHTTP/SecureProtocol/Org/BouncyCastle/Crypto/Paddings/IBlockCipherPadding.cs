using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings
{
	// Token: 0x02000509 RID: 1289
	public interface IBlockCipherPadding
	{
		// Token: 0x060030EC RID: 12524
		void Init(SecureRandom random);

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060030ED RID: 12525
		string PaddingName { get; }

		// Token: 0x060030EE RID: 12526
		int AddPadding(byte[] input, int inOff);

		// Token: 0x060030EF RID: 12527
		int PadCount(byte[] input);
	}
}
