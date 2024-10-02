using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
[RequireComponent(typeof(vp_Shooter))]
public class vp_SimpleAITurret : MonoBehaviour
{
	// Token: 0x0600088A RID: 2186 RVA: 0x0007B90E File Offset: 0x00079B0E
	private void Start()
	{
		this.m_Shooter = base.GetComponent<vp_Shooter>();
		this.m_Transform = base.transform;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0007B928 File Offset: 0x00079B28
	private void Update()
	{
		if (!this.m_Timer.Active)
		{
			vp_Timer.In(this.WakeInterval, delegate()
			{
				if (this.m_Target == null)
				{
					this.m_Target = this.ScanForLocalPlayer();
					return;
				}
				this.m_Target = null;
			}, this.m_Timer);
		}
		if (this.m_Target != null)
		{
			this.AttackTarget();
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0007B968 File Offset: 0x00079B68
	private Transform ScanForLocalPlayer()
	{
		foreach (Collider collider in Physics.OverlapSphere(this.m_Transform.position, this.ViewRange, 1073741824))
		{
			RaycastHit raycastHit;
			Physics.Linecast(this.m_Transform.position, collider.transform.position + Vector3.up, out raycastHit);
			if (!(raycastHit.collider != null) || !(raycastHit.collider != collider))
			{
				return collider.transform;
			}
		}
		return null;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0007B9F4 File Offset: 0x00079BF4
	private void AttackTarget()
	{
		Quaternion to = Quaternion.LookRotation(this.m_Target.position - this.m_Transform.position);
		this.m_Transform.rotation = Quaternion.RotateTowards(this.m_Transform.rotation, to, Time.deltaTime * this.AimSpeed);
		this.m_Shooter.TryFire();
	}

	// Token: 0x04000F0D RID: 3853
	public float ViewRange = 10f;

	// Token: 0x04000F0E RID: 3854
	public float AimSpeed = 50f;

	// Token: 0x04000F0F RID: 3855
	public float WakeInterval = 2f;

	// Token: 0x04000F10 RID: 3856
	protected vp_Shooter m_Shooter;

	// Token: 0x04000F11 RID: 3857
	protected Transform m_Transform;

	// Token: 0x04000F12 RID: 3858
	protected Transform m_Target;

	// Token: 0x04000F13 RID: 3859
	protected vp_Timer.Handle m_Timer = new vp_Timer.Handle();
}
