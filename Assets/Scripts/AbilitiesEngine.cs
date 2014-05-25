using UnityEngine;
using System.Collections;

public class AbilitiesEngine : MonoBehaviour {

	Ability[] abilities = new Ability[4];
	public bool isCurrentlyTargetting = false;
	public Texture2D abilityBarTexture;
	public GUIStyle abilityBarStyle;

	public void SetupAbility(int abilityID) {
		if (abilities[abilityID].abilityType == AbilityType.InstantTargettedEffect ||
		    abilities[abilityID].abilityType == AbilityType.TargettedEffect) {
		    Debug.Log ("Selected targetted effect");
			if (isCurrentlyTargetting == false && abilities[abilityID].currentCooldown == 0) {
				Debug.Log ("Showing target indicator");
				abilities[abilityID].ShowTargetIndicator ();
				isCurrentlyTargetting = true;
			}
		} else {
			Debug.Log ("Selected non-targetted effect or executing targetted effect");
			ExecuteAbility(abilityID, Vector3.zero);
		}
	}
	
	public void ExecuteAbility(int abilityID, Vector3 position) {
		Debug.Log ("Executing ability and hiding potential indicator");
		abilities[abilityID].HideTargetIndicator ();
		isCurrentlyTargetting = false;
		uLink.NetworkView.Get(this).RPC ("SendDoAbility", uLink.RPCMode.Server, abilityID, position);
	}

	// Use this for initialization
	void Start () {
		abilities[0] = new AbilityExample();
		abilities[1] = new AbilityExample2();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < abilities.Length; i++) {
			if(abilities[i] != null) abilities[i].UpdateCooldown();
		}
	}
	
	void OnGUI () {
		GUI.Box (new Rect(Screen.width/2 - 294, Screen.height - 150, 588, 150), abilityBarTexture, abilityBarStyle);
		for (int abilityID = 0; abilityID < abilities.Length; abilityID++) {
			if (abilities[abilityID] != null) {
				GUI.Label (new Rect(Screen.width/2 - 250, Screen.height - 100, 80, 50), abilities[abilityID].currentCooldown.ToString());
			}
		}
	}
	
	[RPC]
	void SendDoAbility(int abilityID, Vector3 position) {
		Debug.Log("Server received request to execute ability");
		abilities[abilityID].DoAbility(gameObject, position);
	}
}
