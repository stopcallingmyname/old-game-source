using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class vp_WaypointGizmo : MonoBehaviour
{
	// Token: 0x060006FE RID: 1790 RVA: 0x0006F2C8 File Offset: 0x0006D4C8
	public void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_GizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0006F32C File Offset: 0x0006D52C
	public void OnDrawGizmosSelected()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = this.m_SelectedGizmoColor;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.color = new Color(0f, 0f, 0f, 1f);
		Gizmos.DrawLine(Vector3.zero, Vector3.forward);
	}

	// Token: 0x04000CD4 RID: 3284
	protected Color m_GizmoColor = new Color(1f, 1f, 1f, 0.4f);

	// Token: 0x04000CD5 RID: 3285
	protected Color m_SelectedGizmoColor = new Color32(160, byte.MaxValue, 100, 100);
}
