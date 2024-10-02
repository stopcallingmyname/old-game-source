using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000560 RID: 1376
	public class Poly1305KeyGenerator : CipherKeyGenerator
	{
		// Token: 0x060033B5 RID: 13237 RVA: 0x00136AB3 File Offset: 0x00134CB3
		protected override void engineInit(KeyGenerationParameters param)
		{
			this.random = param.Random;
			this.strength = 32;
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x00136AC9 File Offset: 0x00134CC9
		protected override byte[] engineGenerateKey()
		{
			byte[] array = base.engineGenerateKey();
			Poly1305KeyGenerator.Clamp(array);
			return array;
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x00136AD8 File Offset: 0x00134CD8
		public static void Clamp(byte[] key)
		{
			if (key.Length != 32)
			{
				throw new ArgumentException("Poly1305 key must be 256 bits.");
			}
			int num = 3;
			key[num] &= 15;
			int num2 = 7;
			key[num2] &= 15;
			int num3 = 11;
			key[num3] &= 15;
			int num4 = 15;
			key[num4] &= 15;
			int num5 = 4;
			key[num5] &= 252;
			int num6 = 8;
			key[num6] &= 252;
			int num7 = 12;
			key[num7] &= 252;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x00136B68 File Offset: 0x00134D68
		public static void CheckKey(byte[] key)
		{
			if (key.Length != 32)
			{
				throw new ArgumentException("Poly1305 key must be 256 bits.");
			}
			Poly1305KeyGenerator.CheckMask(key[3], 15);
			Poly1305KeyGenerator.CheckMask(key[7], 15);
			Poly1305KeyGenerator.CheckMask(key[11], 15);
			Poly1305KeyGenerator.CheckMask(key[15], 15);
			Poly1305KeyGenerator.CheckMask(key[4], 252);
			Poly1305KeyGenerator.CheckMask(key[8], 252);
			Poly1305KeyGenerator.CheckMask(key[12], 252);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x00136BD9 File Offset: 0x00134DD9
		private static void CheckMask(byte b, byte mask)
		{
			if ((b & ~(mask != 0)) != 0)
			{
				throw new ArgumentException("Invalid format for r portion of Poly1305 key.");
			}
		}

		// Token: 0x040021C9 RID: 8649
		private const byte R_MASK_LOW_2 = 252;

		// Token: 0x040021CA RID: 8650
		private const byte R_MASK_HIGH_4 = 15;
	}
}
