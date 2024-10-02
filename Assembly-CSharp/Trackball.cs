using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class Trackball : MonoBehaviour
{
	// Token: 0x0600003E RID: 62 RVA: 0x00002DC8 File Offset: 0x00000FC8
	private void Update()
	{
		if (Input.GetMouseButton(1))
		{
			float num = (float)Mathf.Max(Screen.width, Screen.height);
			float num2 = (Input.mousePosition.x - (float)(Screen.width / 2)) / num * 2f;
			float num3 = (Input.mousePosition.y - (float)(Screen.height / 2)) / num * 2f;
			num2 = Mathf.Clamp(num2, -1f, 1f);
			num3 = Mathf.Clamp(num3, -1f, 1f);
			Vector3 lhs = new Vector3(num2, num3, 0f);
			lhs.z = -Mathf.Clamp01(1f - lhs.magnitude);
			lhs.Normalize();
			Vector3 rhs = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			Vector3 vector = Vector3.Cross(lhs, rhs);
			vector = Camera.main.transform.TransformDirection(vector);
			base.transform.Rotate(vector, rhs.magnitude * 5f, Space.World);
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002ECD File Offset: 0x000010CD
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(base.transform.position, 0.1f);
	}
}
