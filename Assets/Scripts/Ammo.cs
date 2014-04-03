using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public float moveSpeed;
	public int rotateSpeed;

	private GameObject target;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
			Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
			if ((target.transform.position - transform.position).magnitude < 0.1) {
				//Todo: Replace with collision checks
				GameObject.Destroy(gameObject);
			}
		}
	}

	public void fireAt (GameObject targetToSet) {
		target = targetToSet;
		transform.parent = null;
	}

}
