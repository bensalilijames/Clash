using UnityEngine;
using System.Collections;

public class AbilitiesEngine : MonoBehaviour {

	Ability[] abilities = new Ability[4];
	public bool isCurrentlyTargettingEnemies = false;
	public Texture2D abilityBarTexture;
	public GUIStyle abilityBarStyle;

	public void DoAbility(int abilityID) {
		abilities[abilityID].DoAbility(gameObject);
	}	

	// Use this for initialization
	void Start () {
		abilities[0] = new AbilityExample();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < abilities.Length; i++) {
			if(abilities[i] != null) abilities[i].UpdateCooldown();
		}
	}
	
	void OnGUI () {
		GUI.Box (new Rect(Screen.width/2 - 294, Screen.height - 150, 588, 150), abilityBarTexture, abilityBarStyle);
	}
}
