using UnityEngine;
using System.Collections;

public class ProjectileBase : MonoBehaviour
{
	private GameObject target;
	private Vector3 to;
	private float speed;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if (target != null)
		{
			to = target.transform.position;
		}

		if ((to - transform.position).magnitude < speed * Time.deltaTime)
		{
			// projectile will finish moving to its target this frame, do complete
			transform.position = to;
			OnMovementComplete();
		}
		else
		{
			transform.position = transform.position + (to - transform.position) * speed * Time.deltaTime;
		}
	}

	protected virtual void OnMovementComplete()
	{
		if (uLink.Network.isServer)
		{
			if (target != null)
			{
				Combat combatComponent = target.GetComponent<Combat>();
				if (combatComponent != null)
				{
					combatComponent.DoDamage(20);
				}
			}
			uLink.Network.Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (uLink.Network.isServer)
		{
			GameObject collider = col.gameObject;
			if (collider.name.Contains("Player"))
			{
				// only collide against players
				OnCollision(collider);
			}
		}
	}

	protected virtual void OnCollision(GameObject collider)
	{
		if (uLink.Network.isServer)
		{
			if (collider != null)
			{
				Combat combatComponent = target.GetComponent<Combat>();
				if (combatComponent != null)
				{
					combatComponent.DoDamage(50);
					uLink.Network.Destroy(gameObject);
				}
			}
		}
	}

	public void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
	{
		BattleController battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
		int targetID = info.networkView.initialData.Read<int>();
		target = battleControllerScript.GetGameObject(targetID);
		to = info.networkView.initialData.Read<Vector3>();
		speed = info.networkView.initialData.Read<float>();
	}
}
