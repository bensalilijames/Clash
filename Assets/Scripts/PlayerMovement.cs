using UnityEngine;
using System.Collections;
using Pathfinding;

public class PlayerMovement : MovementBase
{
	void Update()
	{
		if (uLink.Network.isServer)
		{
			MoveAlongPath();

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
