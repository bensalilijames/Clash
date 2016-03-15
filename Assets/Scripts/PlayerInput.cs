using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	private Vector3 lastClientClick = Vector3.zero;
	private RaycastHit hit;
	
	private BattleController battleControllerScript;
	private AbilitiesEngine abilitiesEngineScript;

	void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo msg)
	{
		Camera.main.SendMessage("SetPlayerTarget", transform);
	}

	void Start()
	{
		battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
		abilitiesEngineScript = gameObject.GetComponent<AbilitiesEngine>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				int targetID = battleControllerScript.GetGameObjectID(hit.collider.gameObject);
				if (lastClientClick != hit.point)
				{
					lastClientClick = hit.point;
					uLink.NetworkView.Get(this).RPC("SendMovementInput", uLink.RPCMode.Server, hit.point, hit.collider.name, targetID);
				}

				int shownAbilityIndicator = abilitiesEngineScript.ShownAbilityIndicator();
				if (shownAbilityIndicator != -1)
				{
					abilitiesEngineScript.CastAbility(shownAbilityIndicator, lastClientClick, targetID);
				}
			}
		}

		int abilityClicked;
		// if more than one clicked in the same frame, it doesn't really matter which one we go for
		abilityClicked = Input.GetKeyDown(KeyCode.Q) ? 0 : -1;
		abilityClicked = Input.GetKeyDown(KeyCode.W) ? 1 : abilityClicked;
		abilityClicked = Input.GetKeyDown(KeyCode.E) ? 2 : abilityClicked;
		abilityClicked = Input.GetKeyDown(KeyCode.R) ? 3 : abilityClicked;

		if (abilityClicked != -1)
		{
			Debug.Log("Ability " + abilityClicked + " clicked");
			if (abilitiesEngineScript.IsAbilityOffCooldown(abilityClicked))
			{
				if (abilitiesEngineScript.DoesShowAbilityIndicator(abilityClicked))
				{
					if (abilitiesEngineScript.ShownAbilityIndicator() == abilityClicked)
					{
						abilitiesEngineScript.CastAbility(abilityClicked, Vector3.zero, -1);
					}
					else
					{
						abilitiesEngineScript.ShowAbilityIndicator(abilityClicked);
					}
				}
				else
				{
					abilitiesEngineScript.CastAbility(abilityClicked, Vector3.zero, -1);
				}
			}
			else
			{
				Debug.Log("Ability " + abilityClicked + " on cooldown");
			}
		}
	}
}
