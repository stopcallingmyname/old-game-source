using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000471 RID: 1137
	public class TlsMac
	{
		// Token: 0x06002C74 RID: 11380 RVA: 0x001183B0 File Offset: 0x001165B0
		public TlsMac(TlsContext context, IDigest digest, byte[] key, int keyOff, int keyLen)
		{
			this.context = context;
			KeyParameter keyParameter = new KeyParameter(key, keyOff, keyLen);
			this.secret = Arrays.Clone(keyParameter.GetKey());
			if (digest is LongDigest)
			{
				this.digestBlockSize = 128;
				this.digestOverhead = 16;
			}
			else
			{
				this.digestBlockSize = 64;
				this.digestOverhead = 8;
			}
			if (TlsUtilities.IsSsl(context))
			{
				this.mac = new Ssl3Mac(digest);
				if (digest.GetDigestSize() == 20)
				{
					this.digestOverhead = 4;
				}
			}
			else
			{
				this.mac = new HMac(digest);
			}
			this.mac.Init(keyParameter);
			this.macLength = this.mac.GetMacSize();
			if (context.SecurityParameters.truncatedHMac)
			{
				this.macLength = Math.Min(this.macLength, 10);
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x00118482 File Offset: 0x00116682
		public virtual byte[] MacSecret
		{
			get
			{
				return this.secret;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002C76 RID: 11382 RVA: 0x0011848A File Offset: 0x0011668A
		public virtual int Size
		{
			get
			{
				return this.macLength;
			}
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x00118494 File Offset: 0x00116694
		public virtual byte[] CalculateMac(long seqNo, byte type, byte[] message, int offset, int length)
		{
			ProtocolVersion serverVersion = this.context.ServerVersion;
			bool isSsl = serverVersion.IsSsl;
			byte[] array = new byte[isSsl ? 11 : 13];
			TlsUtilities.WriteUint64(seqNo, array, 0);
			TlsUtilities.WriteUint8(type, array, 8);
			if (!isSsl)
			{
				TlsUtilities.WriteVersion(serverVersion, array, 9);
			}
			TlsUtilities.WriteUint16(length, array, array.Length - 2);
			this.mac.BlockUpdate(array, 0, array.Length);
			this.mac.BlockUpdate(message, offset, length);
			return this.Truncate(MacUtilities.DoFinal(this.mac));
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x0011851C File Offset: 0x0011671C
		public virtual byte[] CalculateMacConstantTime(long seqNo, byte type, byte[] message, int offset, int length, int fullLength, byte[] dummyData)
		{
			byte[] result = this.CalculateMac(seqNo, type, message, offset, length);
			int num = TlsUtilities.IsSsl(this.context) ? 11 : 13;
			int num2 = this.GetDigestBlockCount(num + fullLength) - this.GetDigestBlockCount(num + length);
			while (--num2 >= 0)
			{
				this.mac.BlockUpdate(dummyData, 0, this.digestBlockSize);
			}
			this.mac.Update(dummyData[0]);
			this.mac.Reset();
			return result;
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x0011859A File Offset: 0x0011679A
		protected virtual int GetDigestBlockCount(int inputLength)
		{
			return (inputLength + this.digestOverhead) / this.digestBlockSize;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x001185AB File Offset: 0x001167AB
		protected virtual byte[] Truncate(byte[] bs)
		{
			if (bs.Length <= this.macLength)
			{
				return bs;
			}
			return Arrays.CopyOf(bs, this.macLength);
		}

		// Token: 0x04001E45 RID: 7749
		protected readonly TlsContext context;

		// Token: 0x04001E46 RID: 7750
		protected readonly byte[] secret;

		// Token: 0x04001E47 RID: 7751
		protected readonly IMac mac;

		// Token: 0x04001E48 RID: 7752
		protected readonly int digestBlockSize;

		// Token: 0x04001E49 RID: 7753
		protected readonly int digestOverhead;

		// Token: 0x04001E4A RID: 7754
		protected readonly int macLength;
	}
}
