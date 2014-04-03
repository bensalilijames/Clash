using UnityEngine;
using System.Collections;

public class HitmarkMoving : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast (ray, out hit, 100))
		{
			if (hit.collider.tag == "Plane") {
				transform.position = hit.point;
			}
		}
	}
}
