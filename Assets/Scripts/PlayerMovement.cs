using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerMovement : uLink.MonoBehaviour {
	
	public float moveSpeed;
	public int rotateSpeed;
			
	private Vector3 targetPosition;
	private Quaternion targetRotation;
	
	private Combat combatScript;
	
	private Seeker seeker;
	private Path path;
	private int currentWaypoint;
	private bool foundNextTargetWaypoint;
	private bool searchingForPath;
	
	void Start () {
		targetPosition.y = 0.5f;
		seeker = GetComponent<Seeker>();
		combatScript = GetComponent<Combat>();
	}
	
	public void FindPath () {
		searchingForPath = true;
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}
	
	public void OnPathComplete(Path p) {
		searchingForPath = false;
		if (!p.error) {
			path = p;
			currentWaypoint = 1;
			foundNextTargetWaypoint = false;
		}
		else
		{
			Debug.Log (p.error);
		}
	}
	
	void Update () {
		
		if (uLink.Network.isServer) {
			
			if (path != null) {
				if(currentWaypoint < path.vectorPath.Count) {
					if(!foundNextTargetWaypoint) {
						targetPosition = path.vectorPath[currentWaypoint];
						targetPosition.y = 0.5f;
						targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
						foundNextTargetWaypoint = true;
					}
					
					if(currentWaypoint < path.vectorPath.Count - 1) {
						if(Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < 1f) {
							currentWaypoint++;
							foundNextTargetWaypoint = false;
						}
					}
				}
			}
			
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
			
			if (combatScript.target != null) {
				if (!searchingForPath) {
					MoveInRange (combatScript.target);
				}
			}
			
		}
	}
	
	public void MoveInRange (GameObject target) {
		Vector3 distance = target.transform.position - transform.position;
		distance.y = 0;
		
		//If we're within range of the target, stop moving and face them
		float range = combatScript.range;
		if (distance.magnitude < range + 0.01f) {
			path = null;
			targetPosition = transform.position;
			targetRotation = Quaternion.LookRotation(distance);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
		} else {
			distance.Normalize();
			Vector3 temp2 = new Vector3(range, range, range);
			distance.Scale(temp2);
			targetPosition = target.transform.position - distance;
			targetPosition.y = 0.5f;
			FindPath ();
		}
	}
	
	[RPC]
	void SendMovementInput (Vector3 position, string hitName) {
		if (hitName == "Plane") {	
			targetPosition = position;
			targetPosition.y = 0.5f;
			targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
			
			FindPath ();
		}
		else if (hitName == "Turret")
		{
			//combatScript.target = hitCollider.gameObject;
			//MoveInRange (hitCollider.gameObject);
		}
		else if (hitName == "AI")
		{
			//combatScript.target = hit.transform.gameObject;
			//MoveInRange (hitCollider.gameObject);
		}
	}
	
	void uLink_OnSerializeNetworkView (uLink.BitStream stream, uLink.NetworkMessageInfo info) {
		if (stream.isWriting) {
			Debug.Log("Sending position.");
			stream.Write(transform.position);
			stream.Write(transform.rotation);
		} else {
			Vector3 receivedPosition = Vector3.zero;
			Quaternion receivedRotation = Quaternion.identity;
			Debug.Log("Receiving position.");
			receivedPosition = stream.Read<Vector3>();
			receivedRotation = stream.Read<Quaternion>();
			transform.position = receivedPosition;
			transform.rotation = receivedRotation;
		}
	}
	
	
}
