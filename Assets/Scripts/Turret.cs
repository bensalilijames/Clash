using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
	private int health;

	void Start()
	{
		health = 100;
	}

	void Update()
	{
		LookForEnemy();
	}

	public void LoseHealth(int healthLost)
	{
		health = health - Mathf.Min(healthLost, health);
		Debug.Log("Lost health!");
		if (health == 0)
		{
			Debug.Log("Turret Destroyed");
			Destroy(gameObject);
		}
	}

	private void LookForEnemy()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null)
		{
			if (Vector3.Magnitude(player.transform.position - this.transform.position) < 6)
			{
				//TODO: Lose player health
			}
		}
	}
}
