using UnityEngine;
using System.Collections;

public class AbilityExample : Ability {

	public AbilityExample () {
		name = "Example ability";
		abilityType = AbilityType.InstantEffect;
		cooldown = 8.0f;
		description = "An example ability";
		icon = null; //TODO: Fix this!
		targetIndicatorPrefab = null; //TODO: And this!
	}
	
	public override void DoInstantEffect (GameObject player) {
		Debug.Log ("Attempting to apply buff from AbilityExample");
		player.GetComponent<Stats>().ApplyBuff(new BuffExample());
	}

}

public class AbilityExample2 : Ability {
	
	private GameObject healthHitmark;
	
	public AbilityExample2 () {
		name = "Example ability2";
		abilityType = AbilityType.InstantTargettedEffect;
		cooldown = 8.0f;
		description = "An example ability2";
		icon = null; //TODO: Fix this!
		targetIndicatorPrefab = (GameObject)GameObject.Instantiate(Resources.Load ("MarkerHitmark", typeof(GameObject)));
		targetIndicatorPrefab.SetActive(false);
	}
	
	public override void DoInstantEffect (GameObject player) {
		Debug.Log ("Attempting to apply buff from AbilityExample");
		player.GetComponent<Stats>().ApplyBuff(new BuffExample());
	}
	
	public override void DoTargettedEffect (GameObject player, Vector3 position) {
		Debug.Log ("Attempting to use targetted effect");
		healthHitmark = (GameObject)uLink.Network.Instantiate(Resources.Load("HealthHitmark", typeof(GameObject)), position, Quaternion.identity, 0);
	}
	
}