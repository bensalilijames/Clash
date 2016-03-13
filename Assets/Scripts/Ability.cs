using UnityEngine;
using System.Collections;

public enum AbilityType
{
	InstantEffect,
	TargettedEffect,
	TargettedAreaEffect,
	Toggle
}

public class Ability
{
	public AbilityType abilityType;
	protected float baseDamage;
	protected float cooldown;
	public float currentCooldown;
	protected float currentAbilityLevel;
	protected string name;
	protected string description;
	protected Texture2D icon;
	protected GameObject targetIndicatorPrefab;
	public float range;
	public float castTime;

	public void UpdateCooldown()
	{
		currentCooldown -= Time.deltaTime;
		if (currentCooldown < 0)
			currentCooldown = 0;
	}

	public void DoAbility(GameObject player, Vector3 position)
	{
		
		if (currentCooldown > 0)
		{
			Debug.Log("Tried to do ability when on cooldown");
			return;
		}
	
		if (abilityType == AbilityType.InstantEffect)
		{
			DoInstantEffect(player);
		}
		else if (abilityType == AbilityType.TargettedEffect)
		{
			DoTargettedEffect(player, position);
		}
		else if (abilityType == AbilityType.TargettedAreaEffect)
		{
			DoTargettedEffect(player, position);
		}
		currentCooldown = cooldown;
	}

	virtual public void DoInstantEffect(GameObject player)
	{
		Debug.Log("This ability has no instant effect");
	}

	virtual public void DoTargettedEffect(GameObject player, Vector3 position)
	{
		Debug.Log("This ability has no targetted effect");
	}

	virtual public void ShowTargetIndicator()
	{
		if (targetIndicatorPrefab != null)
		{
			Debug.Log("Showing target indicator");
			targetIndicatorPrefab.SetActive(true);
		}
		else
		{
			Debug.Log("This ability has no target indicator to show");
		}
	}

	virtual public void HideTargetIndicator()
	{
		if (targetIndicatorPrefab != null)
		{
			Debug.Log("Hiding target indicator");
			targetIndicatorPrefab.SetActive(false);
		}
		else
		{
			Debug.Log("This ability has no target indicator to hide");
		}
	}
	
}
