using UnityEngine;
using System.Collections;
using Pathfinding;

public class MovementBase : uLink.MonoBehaviour
{
	public float moveSpeed;
	public int rotateSpeed;

	protected Vector3 goalTargetPosition;
	protected Quaternion goalTargetRotation;
	protected Vector3 currentTargetPosition;
	protected Quaternion currentTargetRotation;

	protected Combat combatScript;
	protected BattleController battleControllerScript;
	protected AbilitiesEngine abilitiesEngineScript;

	protected Seeker seeker;
	protected Path path;
	protected int currentWaypoint;
	protected bool foundNextTargetWaypoint;
	protected bool searchingForPath;

	void Start()
	{
		currentTargetPosition.y = 0.5f;
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

	protected void MoveAlongPath()
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
}
