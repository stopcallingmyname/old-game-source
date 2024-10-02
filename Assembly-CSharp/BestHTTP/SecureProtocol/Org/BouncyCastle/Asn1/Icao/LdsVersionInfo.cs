using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Icao
{
	// Token: 0x02000734 RID: 1844
	public class LdsVersionInfo : Asn1Encodable
	{
		// Token: 0x060042D6 RID: 17110 RVA: 0x00187DCC File Offset: 0x00185FCC
		public LdsVersionInfo(string ldsVersion, string unicodeVersion)
		{
			this.ldsVersion = new DerPrintableString(ldsVersion);
			this.unicodeVersion = new DerPrintableString(unicodeVersion);
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x00187DEC File Offset: 0x00185FEC
		private LdsVersionInfo(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("sequence wrong size for LDSVersionInfo", "seq");
			}
			this.ldsVersion = DerPrintableString.GetInstance(seq[0]);
			this.unicodeVersion = DerPrintableString.GetInstance(seq[1]);
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x00187E3C File Offset: 0x0018603C
		public static LdsVersionInfo GetInstance(object obj)
		{
			if (obj is LdsVersionInfo)
			{
				return (LdsVersionInfo)obj;
			}
			if (obj != null)
			{
				return new LdsVersionInfo(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x00187E5D File Offset: 0x0018605D
		public virtual string GetLdsVersion()
		{
			return this.ldsVersion.GetString();
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x00187E6A File Offset: 0x0018606A
		public virtual string GetUnicodeVersion()
		{
			return this.unicodeVersion.GetString();
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x00187E77 File Offset: 0x00186077
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.ldsVersion,
				this.unicodeVersion
			});
		}

		// Token: 0x04002BA6 RID: 11174
		private DerPrintableString ldsVersion;

		// Token: 0x04002BA7 RID: 11175
		private DerPrintableString unicodeVersion;
	}
}
