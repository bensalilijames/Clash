using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerMovement : uLink.MonoBehaviour
{
	public float moveSpeed;
	public int rotateSpeed;
			
	private Vector3 targetPosition;
	private Quaternion targetRotation;
	
	private Combat combatScript;
	private BattleController battleControllerScript;
	private AbilitiesEngine abilitiesEngineScript;
	
	private Seeker seeker;
	private Path path;
	private int currentWaypoint;
	private bool foundNextTargetWaypoint;
	private bool searchingForPath;

	void Start()
	{
		targetPosition.y = 0.5f;
		seeker = GetComponent<Seeker>();
		combatScript = GetComponent<Combat>();
		battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
		abilitiesEngineScript = gameObject.GetComponent<AbilitiesEngine>();
	}

	public void FindPath()
	{
		searchingForPath = true;
		seeker.StartPath(transform.position, targetPosition, OnPathComplete);
	}

	public void OnPathComplete(Path p)
	{
		searchingForPath = false;
		if (!p.error)
		{
			path = p;
			currentWaypoint = 1;
			foundNextTargetWaypoint = false;
		}
		else
		{
			Debug.Log(p.error);
		}
	}

	void Update()
	{
		
		if (uLink.Network.isServer)
		{
			
			if (path != null)
			{
				if (currentWaypoint < path.vectorPath.Count)
				{
					if (!foundNextTargetWaypoint)
					{
						targetPosition = path.vectorPath[currentWaypoint];
						targetPosition.y = 0.5f;
						targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
						foundNextTargetWaypoint = true;
					}
					
					if (currentWaypoint < path.vectorPath.Count - 1)
					{
						if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < 1f)
						{
							currentWaypoint++;
							foundNextTargetWaypoint = false;
						}
					}
				}
			}
			
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
			
			if (abilitiesEngineScript.queuedAbilityID != -1)
			{
				AbilityType abilityType = abilitiesEngineScript.abilities[abilitiesEngineScript.queuedAbilityID].abilityType;
				if (abilityType == AbilityType.TargettedAreaEffect)
				{
					MoveInRange(abilitiesEngineScript.queuedAbilityPosition);
				}
				else if (abilityType == AbilityType.TargettedEffect)
				{
					GameObject targetPlayer = battleControllerScript.GetGameObject(abilitiesEngineScript.queuedAbilityTarget);
					MoveInRange(targetPlayer.transform.position);
				}
			}
			else if (combatScript.target != null)
			{
				if (!searchingForPath)
				{
					MoveInRange(combatScript.target.transform.position);
				}
			}
			
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
		}
	}

	
	public void MoveInRange(Vector3 target)
	{
		Vector3 distance = target - transform.position;
		distance.y = 0;
		
		//If we're within range of the target, stop moving and face them
		float range = combatScript.range;
		if (distance.magnitude < range + 0.01f)
		{
			path = null;
			targetPosition = transform.position;
			targetRotation = Quaternion.LookRotation(distance);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
		}
		else
		{
			distance.Normalize();
			Vector3 temp2 = new Vector3(range, range, range);
			distance.Scale(temp2);
			targetPosition = target - distance;
			targetPosition.y = 0.5f;
			FindPath();
		}
	}

	[RPC]
	void SendMovementInput(Vector3 position, string hitName, int ID)
	{
		if (hitName == "Plane")
		{	
			targetPosition = position;
			targetPosition.y = 0.5f;
			targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
			
			combatScript.target = null;
			
			FindPath();
		}
		else if (hitName.Contains("Turret") || hitName.Contains("AI") || hitName.Contains("Player"))
		{
			GameObject target = battleControllerScript.GetGameObject(ID);
			if (target != gameObject)
			{
				combatScript.target = target;
				MoveInRange(target.transform.position);
			}
		}
	}

	void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.Write(targetPosition);
			stream.Write(targetRotation);
		}
		else
		{
			targetPosition = stream.Read<Vector3>();
			targetRotation = stream.Read<Quaternion>();
		}
	}
	
	
}
