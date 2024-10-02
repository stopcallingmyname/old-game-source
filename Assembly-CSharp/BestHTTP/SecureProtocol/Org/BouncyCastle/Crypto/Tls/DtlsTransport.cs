using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000428 RID: 1064
	public class DtlsTransport : DatagramTransport
	{
		// Token: 0x06002A90 RID: 10896 RVA: 0x00112FDF File Offset: 0x001111DF
		internal DtlsTransport(DtlsRecordLayer recordLayer)
		{
			this.mRecordLayer = recordLayer;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x00112FEE File Offset: 0x001111EE
		public virtual int GetReceiveLimit()
		{
			return this.mRecordLayer.GetReceiveLimit();
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x00112FFB File Offset: 0x001111FB
		public virtual int GetSendLimit()
		{
			return this.mRecordLayer.GetSendLimit();
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x00113008 File Offset: 0x00111208
		public virtual int Receive(byte[] buf, int off, int len, int waitMillis)
		{
			int result;
			try
			{
				result = this.mRecordLayer.Receive(buf, off, len, waitMillis);
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				this.mRecordLayer.Fail(tlsFatalAlert.AlertDescription);
				throw tlsFatalAlert;
			}
			catch (IOException ex)
			{
				this.mRecordLayer.Fail(80);
				throw ex;
			}
			catch (Exception alertCause)
			{
				this.mRecordLayer.Fail(80);
				throw new TlsFatalAlert(80, alertCause);
			}
			return result;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x0011308C File Offset: 0x0011128C
		public virtual void Send(byte[] buf, int off, int len)
		{
			try
			{
				this.mRecordLayer.Send(buf, off, len);
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				this.mRecordLayer.Fail(tlsFatalAlert.AlertDescription);
				throw tlsFatalAlert;
			}
			catch (IOException ex)
			{
				this.mRecordLayer.Fail(80);
				throw ex;
			}
			catch (Exception alertCause)
			{
				this.mRecordLayer.Fail(80);
				throw new TlsFatalAlert(80, alertCause);
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x0011310C File Offset: 0x0011130C
		public virtual void Close()
		{
			this.mRecordLayer.Close();
		}

		// Token: 0x04001CED RID: 7405
		private readonly DtlsRecordLayer mRecordLayer;
	}
}
