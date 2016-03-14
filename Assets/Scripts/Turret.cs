using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
	private int health;
	private float range;
	private TeamTag teamTag;
	private GameObject target;

	private BattleController battleControllerScript;

	void Start()
	{
		battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();

		health = 100;
		range = 6.0f;
	}

	void Update()
	{
		LookForEnemy();
		AttackTarget();
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
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players)
		{
			if (Vector3.Magnitude(player.transform.position - this.transform.position) < range)
			{
				if (battleControllerScript.GetPlayerTeamTag(player) != teamTag)
				{
					target = player;
				}
			}
		}
	}

	private void AttackTarget()
	{
		//attack the target
	}
}
