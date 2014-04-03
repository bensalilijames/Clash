using UnityEngine;
using System.Collections;

public class AmmoController : MonoBehaviour {

	public GameObject ammoPrefab;

	private GameObject[] ammo;
	private int currentAmmo;
	private int maxAmmo;

	// Use this for initialization
	void Start () {
		ResetCubes(2);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ThrowCube (GameObject target) {
		if (currentAmmo < maxAmmo) {
			ammo [currentAmmo].GetComponent<Ammo> ().fireAt (target);
			currentAmmo++;
		} else {
			ResetCubes(2);
		}
	}

	public void ResetCubes (int size) {
		currentAmmo = 0;
		maxAmmo = size * size * size;
		ammo = new GameObject[maxAmmo];

		Vector3 newPosition;
		GameObject newAmmo;
		for(int i = 0; i < size; i++) {
			for(int j = 0; j < size; j++) {
				for(int k = 0; k < size; k++) {
					newPosition = new Vector3(((float)i)/2 - 0.25f, ((float)j)/2 - 0.25f, ((float)k)/2 - 0.25f);
					newAmmo = (GameObject)Instantiate(ammoPrefab);
					newAmmo.transform.parent = transform.parent;
					newAmmo.transform.localPosition = newPosition;
					newAmmo.transform.localRotation = Quaternion.identity;
					ammo[maxAmmo - (i + k * size + j * size * size) - 1] = newAmmo;
				}
			}
		}
	}
}
