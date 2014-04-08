using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	private Vector3 offset;
	public Transform playerTarget;

	// Use this for initialization
	void Start () {
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerTarget != null) {
			transform.position = playerTarget.position + offset;
		} else {
			transform.position = offset;
		}
	}
	
	void SetPlayerTarget (Transform playerTargetToSet) {
		playerTarget = playerTargetToSet;
	}
}
