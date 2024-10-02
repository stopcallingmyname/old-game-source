using System;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class MapCharacterCollision
{
	// Token: 0x06000A69 RID: 2665 RVA: 0x00085250 File Offset: 0x00083450
	public static void Collision(Map map, CharacterController collider)
	{
		for (int i = 0; i < 3; i++)
		{
			Contact? closestContact = MapCharacterCollision.GetClosestContact(map, collider);
			if (closestContact == null)
			{
				break;
			}
			Contact value = closestContact.Value;
			collider.transform.position += value.delta;
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x000852A0 File Offset: 0x000834A0
	private static Contact? GetClosestContact(Map map, CharacterController collider)
	{
		Vector3 position = collider.transform.position;
		int num = Mathf.FloorToInt(position.x - collider.radius);
		int num2 = Mathf.FloorToInt(position.y);
		int num3 = Mathf.FloorToInt(position.z - collider.radius);
		int num4 = Mathf.CeilToInt(position.x + collider.radius);
		int num5 = Mathf.CeilToInt(position.y + collider.height);
		int num6 = Mathf.CeilToInt(position.z + collider.radius);
		Contact? result = null;
		for (int i = num; i <= num4; i++)
		{
			for (int j = num2; j <= num5; j++)
			{
				for (int k = num3; k <= num6; k++)
				{
					if (map.GetBlock(i, j, k).IsSolid())
					{
						Contact? contactBlockCharacter = BlockCharacterCollision.GetContactBlockCharacter(new Vector3i(i, j, k), position, collider);
						if (contactBlockCharacter != null && contactBlockCharacter.Value.delta.magnitude > 1E-45f)
						{
							Contact value = contactBlockCharacter.Value;
							if (result == null || value.sqrDistance > result.Value.sqrDistance)
							{
								result = new Contact?(value);
							}
						}
					}
				}
			}
		}
		return result;
	}
}
