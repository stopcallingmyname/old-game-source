using System;

namespace BestHTTP.Logger
{
	// Token: 0x020007DC RID: 2012
	public interface ILogger
	{
		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06004786 RID: 18310
		// (set) Token: 0x06004787 RID: 18311
		Loglevels Level { get; set; }

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06004788 RID: 18312
		// (set) Token: 0x06004789 RID: 18313
		string FormatVerbose { get; set; }

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x0600478A RID: 18314
		// (set) Token: 0x0600478B RID: 18315
		string FormatInfo { get; set; }

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600478C RID: 18316
		// (set) Token: 0x0600478D RID: 18317
		string FormatWarn { get; set; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600478E RID: 18318
		// (set) Token: 0x0600478F RID: 18319
		string FormatErr { get; set; }

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004790 RID: 18320
		// (set) Token: 0x06004791 RID: 18321
		string FormatEx { get; set; }

		// Token: 0x06004792 RID: 18322
		void Verbose(string division, string verb);

		// Token: 0x06004793 RID: 18323
		void Information(string division, string info);

		// Token: 0x06004794 RID: 18324
		void Warning(string division, string warn);

		// Token: 0x06004795 RID: 18325
		void Error(string division, string err);

		// Token: 0x06004796 RID: 18326
		void Exception(string division, string msg, Exception ex);
	}
}
