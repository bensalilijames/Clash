using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	private Vector3 lastClientClick = Vector3.zero;
	private int lastAbilitySelection = -1;
	private RaycastHit hit;
	
	private BattleController battleControllerScript;
	private AbilitiesEngine abilitiesEngineScript;
	
	void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo msg) 
	{
		Camera.main.SendMessage("SetPlayerTarget", transform);
	}

	void Start () {
		battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
		abilitiesEngineScript = gameObject.GetComponent<AbilitiesEngine>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
//			movementScript.combatScript.target = null;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast (ray, out hit, 100.0f)) {
				if (lastClientClick != hit.point) {
					lastClientClick = hit.point;
					int IDToSend = battleControllerScript.GetGameObjectID(hit.collider.gameObject);
					uLink.NetworkView.Get(this).RPC("SendMovementInput", uLink.RPCMode.Server, hit.point, hit.collider.name, IDToSend);
				}
				
				if(abilitiesEngineScript.isCurrentlyTargetting) {
					abilitiesEngineScript.ExecuteAbility(lastAbilitySelection, lastClientClick);
				}
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Q)) {
			Debug.Log("Ability 0 clicked");
			lastAbilitySelection = 0;
			abilitiesEngineScript.SetupAbility(0);
		}
		
		if (Input.GetKeyDown(KeyCode.W)) {
			Debug.Log("Ability 1 clicked");
			lastAbilitySelection = 1;
			abilitiesEngineScript.SetupAbility(1);
		}
	}
}
