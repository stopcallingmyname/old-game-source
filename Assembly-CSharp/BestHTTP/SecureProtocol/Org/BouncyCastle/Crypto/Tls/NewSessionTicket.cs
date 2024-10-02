using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200043B RID: 1083
	public class NewSessionTicket
	{
		// Token: 0x06002ABB RID: 10939 RVA: 0x001133EC File Offset: 0x001115EC
		public NewSessionTicket(long ticketLifetimeHint, byte[] ticket)
		{
			this.mTicketLifetimeHint = ticketLifetimeHint;
			this.mTicket = ticket;
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002ABC RID: 10940 RVA: 0x00113402 File Offset: 0x00111602
		public virtual long TicketLifetimeHint
		{
			get
			{
				return this.mTicketLifetimeHint;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x0011340A File Offset: 0x0011160A
		public virtual byte[] Ticket
		{
			get
			{
				return this.mTicket;
			}
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x00113412 File Offset: 0x00111612
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint32(this.mTicketLifetimeHint, output);
			TlsUtilities.WriteOpaque16(this.mTicket, output);
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x0011342C File Offset: 0x0011162C
		public static NewSessionTicket Parse(Stream input)
		{
			long ticketLifetimeHint = TlsUtilities.ReadUint32(input);
			byte[] ticket = TlsUtilities.ReadOpaque16(input);
			return new NewSessionTicket(ticketLifetimeHint, ticket);
		}

		// Token: 0x04001D9C RID: 7580
		protected readonly long mTicketLifetimeHint;

		// Token: 0x04001D9D RID: 7581
		protected readonly byte[] mTicket;
	}
}
