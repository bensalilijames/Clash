using UnityEngine;
using System.Collections;
using Pathfinding;

public class AIMovement : MovementBase
{
	void Update()
	{
		if (uLink.Network.isServer)
		{
			if (Random.value < 0.01f)
			{
				goalTargetPosition.Set(Random.value * 200.0f, 0.5f, Random.value * 200.0f);
				FindPath();
			}

			MoveAlongPath();
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, rotateSpeed * Time.deltaTime);
		}
	}

	void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.Write(currentTargetPosition);
			stream.Write(currentTargetRotation);
		}
		else
		{
			currentTargetPosition = stream.Read<Vector3>();
			currentTargetRotation = stream.Read<Quaternion>();
		}
	}
}
