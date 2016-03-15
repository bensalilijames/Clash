using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public Vector3 offset;
	public Transform playerTarget;

	void Update()
	{
		if (playerTarget != null)
		{
			transform.position = playerTarget.position + offset;
		}
		else
		{
			transform.position = offset;
		}
	}

	void SetPlayerTarget(Transform playerTargetToSet)
	{
		playerTarget = playerTargetToSet;
	}
}
