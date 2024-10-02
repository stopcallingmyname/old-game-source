using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000452 RID: 1106
	public class TlsAeadCipher : TlsCipher
	{
		// Token: 0x06002B5F RID: 11103 RVA: 0x00114A71 File Offset: 0x00112C71
		public TlsAeadCipher(TlsContext context, IAeadBlockCipher clientWriteCipher, IAeadBlockCipher serverWriteCipher, int cipherKeySize, int macSize) : this(context, clientWriteCipher, serverWriteCipher, cipherKeySize, macSize, 1)
		{
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x00114A84 File Offset: 0x00112C84
		internal TlsAeadCipher(TlsContext context, IAeadBlockCipher clientWriteCipher, IAeadBlockCipher serverWriteCipher, int cipherKeySize, int macSize, int nonceMode)
		{
			if (!TlsUtilities.IsTlsV12(context))
			{
				throw new TlsFatalAlert(80);
			}
			this.nonceMode = nonceMode;
			int num;
			if (nonceMode != 1)
			{
				if (nonceMode != 2)
				{
					throw new TlsFatalAlert(80);
				}
				num = 12;
				this.record_iv_length = 0;
			}
			else
			{
				num = 4;
				this.record_iv_length = 8;
			}
			this.context = context;
			this.macSize = macSize;
			int num2 = 2 * cipherKeySize + 2 * num;
			byte[] array = TlsUtilities.CalculateKeyBlock(context, num2);
			int num3 = 0;
			KeyParameter keyParameter = new KeyParameter(array, num3, cipherKeySize);
			num3 += cipherKeySize;
			KeyParameter keyParameter2 = new KeyParameter(array, num3, cipherKeySize);
			num3 += cipherKeySize;
			byte[] array2 = Arrays.CopyOfRange(array, num3, num3 + num);
			num3 += num;
			byte[] array3 = Arrays.CopyOfRange(array, num3, num3 + num);
			num3 += num;
			if (num3 != num2)
			{
				throw new TlsFatalAlert(80);
			}
			KeyParameter key;
			KeyParameter key2;
			if (context.IsServer)
			{
				this.encryptCipher = serverWriteCipher;
				this.decryptCipher = clientWriteCipher;
				this.encryptImplicitNonce = array3;
				this.decryptImplicitNonce = array2;
				key = keyParameter2;
				key2 = keyParameter;
			}
			else
			{
				this.encryptCipher = clientWriteCipher;
				this.decryptCipher = serverWriteCipher;
				this.encryptImplicitNonce = array2;
				this.decryptImplicitNonce = array3;
				key = keyParameter;
				key2 = keyParameter2;
			}
			byte[] nonce = new byte[num + this.record_iv_length];
			this.encryptCipher.Init(true, new AeadParameters(key, 8 * macSize, nonce));
			this.decryptCipher.Init(false, new AeadParameters(key2, 8 * macSize, nonce));
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x00114BDA File Offset: 0x00112DDA
		public virtual int GetPlaintextLimit(int ciphertextLimit)
		{
			return ciphertextLimit - this.macSize - this.record_iv_length;
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x00114BEC File Offset: 0x00112DEC
		public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
		{
			byte[] array = new byte[this.encryptImplicitNonce.Length + this.record_iv_length];
			int num = this.nonceMode;
			if (num != 1)
			{
				if (num != 2)
				{
					throw new TlsFatalAlert(80);
				}
				TlsUtilities.WriteUint64(seqNo, array, array.Length - 8);
				for (int i = 0; i < this.encryptImplicitNonce.Length; i++)
				{
					byte[] array2 = array;
					int num2 = i;
					array2[num2] ^= this.encryptImplicitNonce[i];
				}
			}
			else
			{
				Array.Copy(this.encryptImplicitNonce, 0, array, 0, this.encryptImplicitNonce.Length);
				TlsUtilities.WriteUint64(seqNo, array, this.encryptImplicitNonce.Length);
			}
			int outputSize = this.encryptCipher.GetOutputSize(len);
			byte[] array3 = new byte[this.record_iv_length + outputSize];
			if (this.record_iv_length != 0)
			{
				Array.Copy(array, array.Length - this.record_iv_length, array3, 0, this.record_iv_length);
			}
			int num3 = this.record_iv_length;
			byte[] additionalData = this.GetAdditionalData(seqNo, type, len);
			AeadParameters parameters = new AeadParameters(null, 8 * this.macSize, array, additionalData);
			try
			{
				this.encryptCipher.Init(true, parameters);
				num3 += this.encryptCipher.ProcessBytes(plaintext, offset, len, array3, num3);
				num3 += this.encryptCipher.DoFinal(array3, num3);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(80, alertCause);
			}
			if (num3 != array3.Length)
			{
				throw new TlsFatalAlert(80);
			}
			return array3;
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x00114D60 File Offset: 0x00112F60
		public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
		{
			if (this.GetPlaintextLimit(len) < 0)
			{
				throw new TlsFatalAlert(50);
			}
			byte[] array = new byte[this.decryptImplicitNonce.Length + this.record_iv_length];
			int num = this.nonceMode;
			if (num != 1)
			{
				if (num != 2)
				{
					throw new TlsFatalAlert(80);
				}
				TlsUtilities.WriteUint64(seqNo, array, array.Length - 8);
				for (int i = 0; i < this.decryptImplicitNonce.Length; i++)
				{
					byte[] array2 = array;
					int num2 = i;
					array2[num2] ^= this.decryptImplicitNonce[i];
				}
			}
			else
			{
				Array.Copy(this.decryptImplicitNonce, 0, array, 0, this.decryptImplicitNonce.Length);
				Array.Copy(ciphertext, offset, array, array.Length - this.record_iv_length, this.record_iv_length);
			}
			int inOff = offset + this.record_iv_length;
			int len2 = len - this.record_iv_length;
			int outputSize = this.decryptCipher.GetOutputSize(len2);
			byte[] array3 = new byte[outputSize];
			int num3 = 0;
			byte[] additionalData = this.GetAdditionalData(seqNo, type, outputSize);
			AeadParameters parameters = new AeadParameters(null, 8 * this.macSize, array, additionalData);
			try
			{
				this.decryptCipher.Init(false, parameters);
				num3 += this.decryptCipher.ProcessBytes(ciphertext, inOff, len2, array3, num3);
				num3 += this.decryptCipher.DoFinal(array3, num3);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(20, alertCause);
			}
			if (num3 != array3.Length)
			{
				throw new TlsFatalAlert(80);
			}
			return array3;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x00114ED0 File Offset: 0x001130D0
		protected virtual byte[] GetAdditionalData(long seqNo, byte type, int len)
		{
			byte[] array = new byte[13];
			TlsUtilities.WriteUint64(seqNo, array, 0);
			TlsUtilities.WriteUint8(type, array, 8);
			TlsUtilities.WriteVersion(this.context.ServerVersion, array, 9);
			TlsUtilities.WriteUint16(len, array, 11);
			return array;
		}

		// Token: 0x04001E06 RID: 7686
		public const int NONCE_RFC5288 = 1;

		// Token: 0x04001E07 RID: 7687
		internal const int NONCE_DRAFT_CHACHA20_POLY1305 = 2;

		// Token: 0x04001E08 RID: 7688
		protected readonly TlsContext context;

		// Token: 0x04001E09 RID: 7689
		protected readonly int macSize;

		// Token: 0x04001E0A RID: 7690
		protected readonly int record_iv_length;

		// Token: 0x04001E0B RID: 7691
		protected readonly IAeadBlockCipher encryptCipher;

		// Token: 0x04001E0C RID: 7692
		protected readonly IAeadBlockCipher decryptCipher;

		// Token: 0x04001E0D RID: 7693
		protected readonly byte[] encryptImplicitNonce;

		// Token: 0x04001E0E RID: 7694
		protected readonly byte[] decryptImplicitNonce;

		// Token: 0x04001E0F RID: 7695
		protected readonly int nonceMode;
	}
}
