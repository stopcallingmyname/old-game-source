using System;
using System.Text;
using UnityEngine;

namespace BestHTTP.Logger
{
	// Token: 0x020007DA RID: 2010
	public class DefaultLogger : ILogger
	{
		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004773 RID: 18291 RVA: 0x00196289 File Offset: 0x00194489
		// (set) Token: 0x06004774 RID: 18292 RVA: 0x00196291 File Offset: 0x00194491
		public Loglevels Level { get; set; }

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004775 RID: 18293 RVA: 0x0019629A File Offset: 0x0019449A
		// (set) Token: 0x06004776 RID: 18294 RVA: 0x001962A2 File Offset: 0x001944A2
		public string FormatVerbose { get; set; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06004777 RID: 18295 RVA: 0x001962AB File Offset: 0x001944AB
		// (set) Token: 0x06004778 RID: 18296 RVA: 0x001962B3 File Offset: 0x001944B3
		public string FormatInfo { get; set; }

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06004779 RID: 18297 RVA: 0x001962BC File Offset: 0x001944BC
		// (set) Token: 0x0600477A RID: 18298 RVA: 0x001962C4 File Offset: 0x001944C4
		public string FormatWarn { get; set; }

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x0600477B RID: 18299 RVA: 0x001962CD File Offset: 0x001944CD
		// (set) Token: 0x0600477C RID: 18300 RVA: 0x001962D5 File Offset: 0x001944D5
		public string FormatErr { get; set; }

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x0600477D RID: 18301 RVA: 0x001962DE File Offset: 0x001944DE
		// (set) Token: 0x0600477E RID: 18302 RVA: 0x001962E6 File Offset: 0x001944E6
		public string FormatEx { get; set; }

		// Token: 0x0600477F RID: 18303 RVA: 0x001962F0 File Offset: 0x001944F0
		public DefaultLogger()
		{
			this.FormatVerbose = "[{0}] D [{1}]: {2}";
			this.FormatInfo = "[{0}] I [{1}]: {2}";
			this.FormatWarn = "[{0}] W [{1}]: {2}";
			this.FormatErr = "[{0}] Err [{1}]: {2}";
			this.FormatEx = "[{0}] Ex [{1}]: {2} - Message: {3}  StackTrace: {4}";
			this.Level = (Debug.isDebugBuild ? Loglevels.Warning : Loglevels.Error);
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0019634C File Offset: 0x0019454C
		public void Verbose(string division, string verb)
		{
			if (this.Level <= Loglevels.All)
			{
				try
				{
					Debug.Log(string.Format(this.FormatVerbose, this.GetFormattedTime(), division, verb));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x00196390 File Offset: 0x00194590
		public void Information(string division, string info)
		{
			if (this.Level <= Loglevels.Information)
			{
				try
				{
					Debug.Log(string.Format(this.FormatInfo, this.GetFormattedTime(), division, info));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x001963D4 File Offset: 0x001945D4
		public void Warning(string division, string warn)
		{
			if (this.Level <= Loglevels.Warning)
			{
				try
				{
					Debug.LogWarning(string.Format(this.FormatWarn, this.GetFormattedTime(), division, warn));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x00196418 File Offset: 0x00194618
		public void Error(string division, string err)
		{
			if (this.Level <= Loglevels.Error)
			{
				try
				{
					Debug.LogError(string.Format(this.FormatErr, this.GetFormattedTime(), division, err));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0019645C File Offset: 0x0019465C
		public void Exception(string division, string msg, Exception ex)
		{
			if (this.Level <= Loglevels.Exception)
			{
				try
				{
					string text = string.Empty;
					if (ex == null)
					{
						text = "null";
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						Exception ex2 = ex;
						int num = 1;
						while (ex2 != null)
						{
							stringBuilder.AppendFormat("{0}: {1} {2}", num++.ToString(), ex2.Message, ex2.StackTrace);
							ex2 = ex2.InnerException;
							if (ex2 != null)
							{
								stringBuilder.AppendLine();
							}
						}
						text = stringBuilder.ToString();
					}
					Debug.LogError(string.Format(this.FormatEx, new object[]
					{
						this.GetFormattedTime(),
						division,
						msg,
						text,
						(ex != null) ? ex.StackTrace : "null"
					}));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x00196528 File Offset: 0x00194728
		private string GetFormattedTime()
		{
			return DateTime.Now.Ticks.ToString();
		}
	}
}
