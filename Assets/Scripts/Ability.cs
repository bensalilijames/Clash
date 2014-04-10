using UnityEngine;
using System.Collections;

public enum AbilityType {
	InstantEffect,
	TargettedEffect,
	InstantTargettedEffect
}

public class Ability {

	protected AbilityType abilityType;
	protected float baseDamage;
	protected float cooldown;
	protected float currentCooldown;
	protected float currentAbilityLevel;
	protected string description;
	protected Texture2D icon;
	protected GameObject targetIndicatorPrefab;
	
	public void UpdateCooldown () {
		currentCooldown -= Time.deltaTime;
		if (currentCooldown < 0) currentCooldown = 0;
	}
	
	public void DoAbility (GameObject player) {
		if (currentCooldown > 0) return;
	
		if (abilityType == AbilityType.InstantEffect) {
			DoInstantEffect(player);
		} else if (abilityType == AbilityType.TargettedEffect) {
			DoTargettedEffect(player);
		} else if (abilityType == AbilityType.InstantTargettedEffect) {
			DoInstantEffect(player);
			DoTargettedEffect(player);
		}
		currentCooldown = cooldown;
	}
	
	virtual public void DoInstantEffect (GameObject player) {
		Debug.Log ("This ability has no instant effect");
	}
	
	virtual public void DoTargettedEffect (GameObject player) {
		Debug.Log ("This ability has no targetted effect");
	}

	virtual public void ShowTargetIndicator () {
		Debug.Log ("This ability has no target indicator to show");
	}

	virtual public void HideTargetIndicator () {
		Debug.Log ("This ability has no target indicator to hide");
	}
	
}
