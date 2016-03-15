using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	private Camera facingCamera;
	// owner is assumed to be the parent of this object
	private Stats ownerStats;
	private RectTransform currentHealthRect;

	void Start () {
		facingCamera = Camera.main;

		ownerStats = GetComponentInParent<Stats>();
		if (ownerStats == null)
		{
			Debug.LogError("Health bar has no parent with a Stats object");
		}

		currentHealthRect = transform.FindChild("CurrentHealth").GetComponent<RectTransform>();
		if (currentHealthRect == null)
		{
			Debug.LogError("Health bar UI element not found");
		}
	}
	
	void Update () {
		float currentHealthPercent = (float)ownerStats.currentHealth / (float)ownerStats.baseHealth;
		currentHealthRect.sizeDelta = new Vector2(currentHealthPercent * 20.0f, 1.0f);

		transform.LookAt(transform.position + facingCamera.transform.rotation * Vector3.forward,
			facingCamera.transform.rotation * Vector3.up);
	}
}
