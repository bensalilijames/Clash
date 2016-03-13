using UnityEngine;
using System.Collections;

public class Combat : MonoBehaviour
{
	public float attackSpeed;
	public float attackDamage;
	public int health;
	public GameObject target;
	public float range;
	public AudioClip shootSound;

	private float cooldown;
	
	private BattleController battleControllerScript;

	// Use this for initialization
	void Start()
	{
		battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
		if (battleControllerScript == null)
		{
			Debug.Log("Battle Controller not found");
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		Attack();
	}

	private void Attack()
	{
		if (uLink.Network.isServer)
		{
			if (cooldown > 0f)
			{
				cooldown -= attackSpeed * Time.deltaTime;
				return;
			}
			if (target == null)
			{
				return;
			}
			Vector3 correctedTargetPosition = target.transform.position;
			correctedTargetPosition.y = 0.5f;
			if ((correctedTargetPosition - transform.position).magnitude > range + 0.01f)
			{
				return;
			}
			cooldown = 100.0f;
			
			if (gameObject == null)
			{
				Debug.Log("GameObject not found!");
			}
			
			int attackerID = battleControllerScript.GetGameObjectID(gameObject);
			Debug.Log("GameObject ID for attacker: " + attackerID);
			int targetID = battleControllerScript.GetGameObjectID(target);
			Debug.Log("GameObject ID for target: " + targetID);
			uLinkNetworkView.Get(this).RPC("sendAttack", uLink.RPCMode.Others, attackerID, targetID);
		}
	}

	[RPC]
	public void sendAttack(int attackerID, int targetID)
	{
		GameObject targetToHit = battleControllerScript.GetGameObject(targetID);
		if (targetToHit == null)
		{
			Debug.Log("No target from server RPC");
			return;
		}
		gameObject.GetComponentInChildren<AmmoController>().ThrowCube(targetToHit);
		GetComponent<AudioSource>().clip = shootSound;
		GetComponent<AudioSource>().Play();
		Debug.Log("Done " + attackDamage + " damage to " + targetToHit.name);
	}
}
