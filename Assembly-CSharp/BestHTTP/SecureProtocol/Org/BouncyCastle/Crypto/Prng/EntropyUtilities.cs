using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B0 RID: 1200
	public abstract class EntropyUtilities
	{
		// Token: 0x06002EFE RID: 12030 RVA: 0x001237DC File Offset: 0x001219DC
		public static byte[] GenerateSeed(IEntropySource entropySource, int numBytes)
		{
			byte[] array = new byte[numBytes];
			int num;
			for (int i = 0; i < numBytes; i += num)
			{
				Array entropy = entropySource.GetEntropy();
				num = Math.Min(array.Length, numBytes - i);
				Array.Copy(entropy, 0, array, i, num);
			}
			return array;
		}
	}
}
