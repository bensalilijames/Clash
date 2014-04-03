using UnityEngine;
using System.Collections;

public class Combat : MonoBehaviour {

	public float attackSpeed;
	public float attackDamage;
	public int health;
	public GameObject target;
	public float range;
	public AudioClip shootSound;
	public GameObject hitmarkPrefab;

	private float cooldown;
	private bool showHitmark;
	private GameObject hitmark;

	// Use this for initialization
	void Start () {
		hitmark = (GameObject)Instantiate(hitmarkPrefab);
		hitmark.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			hitmark.SetActive(true);
		}
		if (Input.GetMouseButtonDown(0)) {
			hitmark.SetActive(false);
		}
		Attack ();
	}

	private void Attack () {
		if (cooldown > 0f) {
			cooldown -= attackSpeed * Time.deltaTime;
			return;
		}
		if (target == null) {
			return;
		}
		Vector3 correctedTargetPosition = target.transform.position;
		correctedTargetPosition.y = 0.5f;
		if ((correctedTargetPosition - transform.position).magnitude > range + 0.01f) {
			return;
		}
		Debug.Log ("Attacking Target!");
		gameObject.GetComponentInChildren<AmmoController> ().ThrowCube (target);
		audio.clip = shootSound;
		audio.Play();
		cooldown = 100f;
	}
}
