using System;
using System.Threading;
using System.Threading.Tasks;

namespace BestHTTP.PlatformSupport.Threading
{
	// Token: 0x020007D5 RID: 2005
	public static class ThreadedRunner
	{
		// Token: 0x06004735 RID: 18229 RVA: 0x001957C8 File Offset: 0x001939C8
		public static void RunShortLiving<T>(Action<T> job, T param)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				job(param);
			});
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x001957EE File Offset: 0x001939EE
		public static void RunShortLiving<T1, T2>(Action<T1, T2> job, T1 param1, T2 param2)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				job(param1, param2);
			});
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x0019581B File Offset: 0x00193A1B
		public static void RunShortLiving<T1, T2, T3>(Action<T1, T2, T3> job, T1 param1, T2 param2, T3 param3)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				job(param1, param2, param3);
			});
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x0019584F File Offset: 0x00193A4F
		public static void RunShortLiving(Action job)
		{
			ThreadPool.QueueUserWorkItem(delegate(object param)
			{
				job();
			});
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x0019586E File Offset: 0x00193A6E
		public static void RunLongLiving(Action job)
		{
			Task task = new Task(delegate()
			{
				job();
			}, TaskCreationOptions.LongRunning);
			task.ConfigureAwait(false);
			task.Start();
		}
	}
}
