using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000583 RID: 1411
	public class RC4Engine : IStreamCipher
	{
		// Token: 0x0600353A RID: 13626 RVA: 0x00145CF2 File Offset: 0x00143EF2
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is KeyParameter)
			{
				this.workingKey = ((KeyParameter)parameters).GetKey();
				this.SetKey(this.workingKey);
				return;
			}
			throw new ArgumentException("invalid parameter passed to RC4 init - " + Platform.GetTypeName(parameters));
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x0600353B RID: 13627 RVA: 0x00145D2F File Offset: 0x00143F2F
		public virtual string AlgorithmName
		{
			get
			{
				return "RC4";
			}
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x00145D38 File Offset: 0x00143F38
		public virtual byte ReturnByte(byte input)
		{
			this.x = (this.x + 1 & 255);
			this.y = ((int)this.engineState[this.x] + this.y & 255);
			byte b = this.engineState[this.x];
			this.engineState[this.x] = this.engineState[this.y];
			this.engineState[this.y] = b;
			return input ^ this.engineState[(int)(this.engineState[this.x] + this.engineState[this.y] & byte.MaxValue)];
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x00145DDC File Offset: 0x00143FDC
		public virtual void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			Check.DataLength(input, inOff, length, "input buffer too short");
			Check.OutputLength(output, outOff, length, "output buffer too short");
			for (int i = 0; i < length; i++)
			{
				this.x = (this.x + 1 & 255);
				this.y = ((int)this.engineState[this.x] + this.y & 255);
				byte b = this.engineState[this.x];
				this.engineState[this.x] = this.engineState[this.y];
				this.engineState[this.y] = b;
				output[i + outOff] = (input[i + inOff] ^ this.engineState[(int)(this.engineState[this.x] + this.engineState[this.y] & byte.MaxValue)]);
			}
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x00145EB7 File Offset: 0x001440B7
		public virtual void Reset()
		{
			this.SetKey(this.workingKey);
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x00145EC8 File Offset: 0x001440C8
		private void SetKey(byte[] keyBytes)
		{
			this.workingKey = keyBytes;
			this.x = 0;
			this.y = 0;
			if (this.engineState == null)
			{
				this.engineState = new byte[RC4Engine.STATE_LENGTH];
			}
			for (int i = 0; i < RC4Engine.STATE_LENGTH; i++)
			{
				this.engineState[i] = (byte)i;
			}
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < RC4Engine.STATE_LENGTH; j++)
			{
				num2 = ((int)((keyBytes[num] & byte.MaxValue) + this.engineState[j]) + num2 & 255);
				byte b = this.engineState[j];
				this.engineState[j] = this.engineState[num2];
				this.engineState[num2] = b;
				num = (num + 1) % keyBytes.Length;
			}
		}

		// Token: 0x040022D3 RID: 8915
		private static readonly int STATE_LENGTH = 256;

		// Token: 0x040022D4 RID: 8916
		private byte[] engineState;

		// Token: 0x040022D5 RID: 8917
		private int x;

		// Token: 0x040022D6 RID: 8918
		private int y;

		// Token: 0x040022D7 RID: 8919
		private byte[] workingKey;
	}
}
