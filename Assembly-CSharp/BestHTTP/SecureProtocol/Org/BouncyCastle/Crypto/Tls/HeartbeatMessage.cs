using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000433 RID: 1075
	public class HeartbeatMessage
	{
		// Token: 0x06002AAB RID: 10923 RVA: 0x00113260 File Offset: 0x00111460
		public HeartbeatMessage(byte type, byte[] payload, int paddingLength)
		{
			if (!HeartbeatMessageType.IsValid(type))
			{
				throw new ArgumentException("not a valid HeartbeatMessageType value", "type");
			}
			if (payload == null || payload.Length >= 65536)
			{
				throw new ArgumentException("must have length < 2^16", "payload");
			}
			if (paddingLength < 16)
			{
				throw new ArgumentException("must be at least 16", "paddingLength");
			}
			this.mType = type;
			this.mPayload = payload;
			this.mPaddingLength = paddingLength;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x001132D4 File Offset: 0x001114D4
		public virtual void Encode(TlsContext context, Stream output)
		{
			TlsUtilities.WriteUint8(this.mType, output);
			TlsUtilities.CheckUint16(this.mPayload.Length);
			TlsUtilities.WriteUint16(this.mPayload.Length, output);
			output.Write(this.mPayload, 0, this.mPayload.Length);
			byte[] array = new byte[this.mPaddingLength];
			context.NonceRandomGenerator.NextBytes(array);
			output.Write(array, 0, array.Length);
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x00113340 File Offset: 0x00111540
		public static HeartbeatMessage Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (!HeartbeatMessageType.IsValid(b))
			{
				throw new TlsFatalAlert(47);
			}
			int payloadLength = TlsUtilities.ReadUint16(input);
			HeartbeatMessage.PayloadBuffer payloadBuffer = new HeartbeatMessage.PayloadBuffer();
			Streams.PipeAll(input, payloadBuffer);
			byte[] array = payloadBuffer.ToTruncatedByteArray(payloadLength);
			if (array == null)
			{
				return null;
			}
			TlsUtilities.CheckUint16(payloadBuffer.Length);
			int paddingLength = (int)payloadBuffer.Length - array.Length;
			return new HeartbeatMessage(b, array, paddingLength);
		}

		// Token: 0x04001D51 RID: 7505
		protected readonly byte mType;

		// Token: 0x04001D52 RID: 7506
		protected readonly byte[] mPayload;

		// Token: 0x04001D53 RID: 7507
		protected readonly int mPaddingLength;

		// Token: 0x02000944 RID: 2372
		internal class PayloadBuffer : MemoryStream
		{
			// Token: 0x06004ED9 RID: 20185 RVA: 0x001B33E8 File Offset: 0x001B15E8
			internal byte[] ToTruncatedByteArray(int payloadLength)
			{
				int num = payloadLength + 16;
				if (this.Length < (long)num)
				{
					return null;
				}
				return Arrays.CopyOf(this.GetBuffer(), payloadLength);
			}
		}
	}
}
