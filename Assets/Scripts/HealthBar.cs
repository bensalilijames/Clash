using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Camera facingCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		transform.LookAt(transform.position + facingCamera.transform.rotation * Vector3.forward,
			facingCamera.transform.rotation * Vector3.up);
	}
}
