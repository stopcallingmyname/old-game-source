using System;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class PlayerTrigger : MonoBehaviour
{
	// Token: 0x06000322 RID: 802 RVA: 0x00039A44 File Offset: 0x00037C44
	private void OnTriggerEnter(Collider other)
	{
		if (other.name != "rpg")
		{
			return;
		}
		Rocket component = other.GetComponent<Rocket>();
		if (component == null)
		{
			return;
		}
		component.Explode();
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00039A7C File Offset: 0x00037C7C
	private void OnTriggerStay(Collider other)
	{
		if (other.name != "rpg")
		{
			return;
		}
		Rocket component = other.GetComponent<Rocket>();
		if (component == null)
		{
			return;
		}
		component.Explode();
	}
}
