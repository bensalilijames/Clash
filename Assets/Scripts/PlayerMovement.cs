using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerMovement : uLink.MonoBehaviour
{
	public float moveSpeed;
	public int rotateSpeed;
			
	private Vector3 goalTargetPosition;
	private Quaternion goalTargetRotation;
	private Vector3 currentTargetPosition;
	private Quaternion currentTargetRotation;
	
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
		goalTargetPosition.y = 0.5f;
		seeker = GetComponent<Seeker>();
		combatScript = GetComponent<Combat>();
		battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
		abilitiesEngineScript = gameObject.GetComponent<AbilitiesEngine>();
	}

	public void FindPath()
	{
		searchingForPath = true;
		seeker.StartPath(transform.position, goalTargetPosition, OnPathComplete);
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
						currentTargetPosition = path.vectorPath[currentWaypoint];
						currentTargetPosition.y = 0.5f;
						currentTargetRotation = Quaternion.LookRotation(currentTargetPosition - transform.position);
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
			
			transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, rotateSpeed * Time.deltaTime);

			int queuedAbilityID = abilitiesEngineScript.queuedAbilityID;
			if (queuedAbilityID != -1)
			{
				AbilityType abilityType = abilitiesEngineScript.abilities[abilitiesEngineScript.queuedAbilityID].abilityType;
				if (abilityType == AbilityType.TargettedAreaEffect)
				{
					float abilityRange = abilitiesEngineScript.GetAbilityRange(queuedAbilityID);
					MoveInRange(abilitiesEngineScript.queuedAbilityPosition, abilityRange);
				}
				else if (abilityType == AbilityType.TargettedEffect)
				{
					GameObject targetPlayer = battleControllerScript.GetGameObject(abilitiesEngineScript.queuedAbilityTarget);
					float abilityRange = abilitiesEngineScript.GetAbilityRange(queuedAbilityID);
					MoveInRange(targetPlayer.transform.position, abilityRange);
				}
			}
			else if (combatScript.target != null)
			{
				if (!searchingForPath)
				{
					MoveInRange(combatScript.target.transform.position, combatScript.range);
				}
			}
			
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, rotateSpeed * Time.deltaTime);
		}
	}

	
	public void MoveInRange(Vector3 target, float range)
	{
		Vector3 distance = target - transform.position;
		distance.y = 0;

		//If we're within range of the target, stop moving and face them
		if (distance.magnitude < range)
		{
			path = null;
			currentTargetPosition = transform.position;
			currentTargetRotation = Quaternion.LookRotation(distance);
			transform.rotation = Quaternion.Slerp(transform.rotation, currentTargetRotation, rotateSpeed * Time.deltaTime);
		}
		else
		{
			distance.Normalize();
			Vector3 temp2 = new Vector3(range, range, range);
			distance.Scale(temp2);
			goalTargetPosition = target - distance;
			goalTargetPosition.y = 0.5f;
			FindPath();
		}
	}

	[RPC]
	void SendMovementInput(Vector3 position, string hitName, int ID)
	{
		if (hitName == "Plane")
		{	
			goalTargetPosition = position;
			goalTargetPosition.y = 0.5f;
			goalTargetRotation = Quaternion.LookRotation(goalTargetPosition - transform.position);
			
			combatScript.SetCombatTarget(null);
			
			FindPath();
		}
		else if (hitName.Contains("Turret") || hitName.Contains("AI") || hitName.Contains("Player"))
		{
			GameObject target = battleControllerScript.GetGameObject(ID);
			if (target != gameObject)
			{
				combatScript.SetCombatTarget(target);
				MoveInRange(target.transform.position, combatScript.range);
			}
		}
	}

	void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.Write(currentTargetPosition);
			stream.Write(currentTargetRotation);
		}
		else
		{
			currentTargetPosition = stream.Read<Vector3>();
			currentTargetRotation = stream.Read<Quaternion>();
		}
	}
	
	
}
