using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class BlockCharacterCollision
{
	// Token: 0x06000A63 RID: 2659 RVA: 0x000850A4 File Offset: 0x000832A4
	public static Contact? GetContactBlockCharacter(Vector3 blockPos, Vector3 pos, CharacterController collider)
	{
		if (!collider)
		{
			return null;
		}
		Contact closestPoint = BlockCharacterCollision.GetClosestPoint(blockPos, pos + Vector3.up * (collider.height - collider.radius));
		Contact closestPoint2 = BlockCharacterCollision.GetClosestPoint(blockPos, pos + Vector3.up * collider.radius);
		Contact contact = closestPoint;
		if (closestPoint2.sqrDistance < contact.sqrDistance)
		{
			contact = closestPoint2;
		}
		if (contact.sqrDistance > collider.radius * collider.radius)
		{
			return null;
		}
		Vector3 b = contact.delta.normalized * collider.radius;
		Vector3 b2 = contact.b + b;
		contact.b = b2;
		return new Contact?(contact);
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00085170 File Offset: 0x00083370
	private static Contact GetClosestPoint(Vector3 blockPos, Vector3 point)
	{
		Vector3 vector = blockPos - Vector3.one / 2f;
		Vector3 vector2 = blockPos + Vector3.one / 2f;
		Vector3 a = point;
		for (int i = 0; i < 3; i++)
		{
			if (a[i] > vector2[i])
			{
				a[i] = vector2[i];
			}
			if (a[i] < vector[i])
			{
				a[i] = vector[i];
			}
		}
		return new Contact(a, point);
	}
}
