using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Camera facingCamera;
	// owner is assumed to be the parent of this object
	private Stats ownerStats;
	public RectTransform currentHealthRect;

	void Start () {
		// camera is orthographic so this can be called at Start() instead of Update()
		transform.LookAt(transform.position + facingCamera.transform.rotation * Vector3.forward,
			facingCamera.transform.rotation * Vector3.up);

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
	}
}
