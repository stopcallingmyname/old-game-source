using System;
using System.Threading;

// Token: 0x0200004B RID: 75
public class ThreadedJob
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000246 RID: 582 RVA: 0x0002B8C0 File Offset: 0x00029AC0
	// (set) Token: 0x06000247 RID: 583 RVA: 0x0002B904 File Offset: 0x00029B04
	public bool IsDone
	{
		get
		{
			object handle = this.m_Handle;
			bool isDone;
			lock (handle)
			{
				isDone = this.m_IsDone;
			}
			return isDone;
		}
		set
		{
			object handle = this.m_Handle;
			lock (handle)
			{
				this.m_IsDone = value;
			}
		}
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0002B948 File Offset: 0x00029B48
	public virtual void Start()
	{
		this.m_Thread = new Thread(new ThreadStart(this.Run));
		this.m_Thread.Start();
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0002B96C File Offset: 0x00029B6C
	public virtual void Abort()
	{
		this.m_Thread.Abort();
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000248C File Offset: 0x0000068C
	public virtual void Restart()
	{
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void ThreadFunction()
	{
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void OnFinished()
	{
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0002B979 File Offset: 0x00029B79
	public virtual bool Update()
	{
		if (this.IsDone)
		{
			this.OnFinished();
			this.IsDone = false;
			return true;
		}
		return false;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0002B993 File Offset: 0x00029B93
	private void Run()
	{
		this.ThreadFunction();
		this.IsDone = true;
	}

	// Token: 0x040002F5 RID: 757
	private bool m_IsDone;

	// Token: 0x040002F6 RID: 758
	private object m_Handle = new object();

	// Token: 0x040002F7 RID: 759
	private Thread m_Thread;
}
