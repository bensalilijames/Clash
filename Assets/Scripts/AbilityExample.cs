using UnityEngine;
using System.Collections;

public class AbilityExample : Ability {

	public AbilityExample () {
		abilityType = AbilityType.InstantEffect;
		cooldown = 5.0f;
		description = "An example ability";
		icon = null; //TODO: Fix this!
		targetIndicatorPrefab = null; //TODO: And this!
	}
	
	public override void DoInstantEffect (GameObject player) {
		player.GetComponent<Combat>().attackDamage += 20.0f;
	}

}
