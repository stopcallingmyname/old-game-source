using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000473 RID: 1139
	public class TlsNullCipher : TlsCipher
	{
		// Token: 0x06002C7C RID: 11388 RVA: 0x001185D3 File Offset: 0x001167D3
		public TlsNullCipher(TlsContext context)
		{
			this.context = context;
			this.writeMac = null;
			this.readMac = null;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x001185F0 File Offset: 0x001167F0
		public TlsNullCipher(TlsContext context, IDigest clientWriteDigest, IDigest serverWriteDigest)
		{
			if (clientWriteDigest == null != (serverWriteDigest == null))
			{
				throw new TlsFatalAlert(80);
			}
			this.context = context;
			TlsMac tlsMac = null;
			TlsMac tlsMac2 = null;
			if (clientWriteDigest != null)
			{
				int num = clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
				byte[] key = TlsUtilities.CalculateKeyBlock(context, num);
				int num2 = 0;
				tlsMac = new TlsMac(context, clientWriteDigest, key, num2, clientWriteDigest.GetDigestSize());
				num2 += clientWriteDigest.GetDigestSize();
				tlsMac2 = new TlsMac(context, serverWriteDigest, key, num2, serverWriteDigest.GetDigestSize());
				num2 += serverWriteDigest.GetDigestSize();
				if (num2 != num)
				{
					throw new TlsFatalAlert(80);
				}
			}
			if (context.IsServer)
			{
				this.writeMac = tlsMac2;
				this.readMac = tlsMac;
				return;
			}
			this.writeMac = tlsMac;
			this.readMac = tlsMac2;
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x001186A8 File Offset: 0x001168A8
		public virtual int GetPlaintextLimit(int ciphertextLimit)
		{
			int num = ciphertextLimit;
			if (this.writeMac != null)
			{
				num -= this.writeMac.Size;
			}
			return num;
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x001186D0 File Offset: 0x001168D0
		public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
		{
			if (this.writeMac == null)
			{
				return Arrays.CopyOfRange(plaintext, offset, offset + len);
			}
			byte[] array = this.writeMac.CalculateMac(seqNo, type, plaintext, offset, len);
			byte[] array2 = new byte[len + array.Length];
			Array.Copy(plaintext, offset, array2, 0, len);
			Array.Copy(array, 0, array2, len, array.Length);
			return array2;
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x0011872C File Offset: 0x0011692C
		public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
		{
			if (this.readMac == null)
			{
				return Arrays.CopyOfRange(ciphertext, offset, offset + len);
			}
			int size = this.readMac.Size;
			if (len < size)
			{
				throw new TlsFatalAlert(50);
			}
			int num = len - size;
			byte[] a = Arrays.CopyOfRange(ciphertext, offset + num, offset + len);
			byte[] b = this.readMac.CalculateMac(seqNo, type, ciphertext, offset, num);
			if (!Arrays.ConstantTimeAreEqual(a, b))
			{
				throw new TlsFatalAlert(20);
			}
			return Arrays.CopyOfRange(ciphertext, offset, offset + num);
		}

		// Token: 0x04001E4B RID: 7755
		protected readonly TlsContext context;

		// Token: 0x04001E4C RID: 7756
		protected readonly TlsMac writeMac;

		// Token: 0x04001E4D RID: 7757
		protected readonly TlsMac readMac;
	}
}
