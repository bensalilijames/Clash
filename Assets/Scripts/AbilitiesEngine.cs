using UnityEngine;
using System.Collections;

public class AbilitiesEngine : MonoBehaviour
{
	public Ability[] abilities = new Ability[4];
	public bool isCurrentlyTargetting = false;
	public Texture2D abilityBarTexture;
	public GUIStyle abilityBarStyle;
	
	public Vector3 queuedAbilityPosition;
	public int queuedAbilityTarget = -1;
	public int queuedAbilityID = -1;

	private float timeToCastAbility;
	private bool needToSetCastTimer;

	private int shownAbilityIndicator = -1;
	
	private BattleController battleControllerScript;

	// Client-side method to cast an ability.
	public void CastAbility(int abilityID, Vector3 position, int targetID)
	{
		Debug.Log("Casting ability " + abilityID);
		HideCurrentAbilityIndicator();
		uLink.NetworkView.Get(this).RPC("SendCastAbility", uLink.RPCMode.Server, abilityID, position, targetID);
	}

	private void ExecuteQueuedAbility()
	{
		Debug.Log("Actually executing ability now in range");
		abilities[queuedAbilityID].DoAbility(gameObject, queuedAbilityPosition);
		queuedAbilityID = -1;
		queuedAbilityPosition = Vector3.zero;
		queuedAbilityTarget = -1;
	}

	public bool DoesShowAbilityIndicator(int abilityID)
	{
		if (abilities[abilityID].abilityType == AbilityType.InstantEffect ||
		    abilities[abilityID].abilityType == AbilityType.Toggle)
		{
			return false;
		}
		return true;
	}

	public void ShowAbilityIndicator(int abilityID)
	{
		foreach (Ability ability in abilities)
		{
			if (ability != null)
			{
				ability.HideTargetIndicator();
			}
		}
		Debug.Log("showing ability indicator for " + abilityID);
		abilities[abilityID].ShowTargetIndicator();
		shownAbilityIndicator = abilityID;
	}

	public void HideCurrentAbilityIndicator()
	{
		if (shownAbilityIndicator != -1)
		{
			abilities[shownAbilityIndicator].HideTargetIndicator();
			shownAbilityIndicator = -1;
		}
	}

	public int ShownAbilityIndicator()
	{
		return shownAbilityIndicator;
	}

	public bool IsAbilityOffCooldown(int abilityID)
	{
		return abilities[abilityID].currentCooldown == 0.0f;
	}

	private void BeginCastTimer() {
		timeToCastAbility = abilities[queuedAbilityID].castTime;
	}

	public void UpdateCastTimer() {
		timeToCastAbility -= Time.deltaTime;
		if (timeToCastAbility < 0.0f) timeToCastAbility = 0.0f;
	}

	// Use this for initialization
	void Start()
	{
		abilities[0] = new AbilityExample();
		abilities[1] = new AbilityExample2();
		battleControllerScript = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
	}
	
	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < abilities.Length; i++)
		{
			if (abilities[i] != null)
				abilities[i].UpdateCooldown();
		}
		
		if (queuedAbilityID != -1)
		{
			if (needToSetCastTimer) {
				if (abilities[queuedAbilityID].abilityType == AbilityType.TargettedAreaEffect)
				{
					if (Vector3.Distance(transform.position, queuedAbilityPosition) < abilities[queuedAbilityID].range)
					{
						BeginCastTimer();
					}
				}
				else if (abilities[queuedAbilityID].abilityType == AbilityType.TargettedEffect)
				{
					Vector3 targetPosition = battleControllerScript.GetGameObject(queuedAbilityTarget).transform.position;
					if (Vector3.Distance(transform.position, targetPosition) < abilities[queuedAbilityID].range)
					{
						BeginCastTimer();
					}
				}
				else
				{
					BeginCastTimer();
				}
				needToSetCastTimer = false;
			}

			UpdateCastTimer();

			if (timeToCastAbility == 0.0f) {
				ExecuteQueuedAbility();
			}
		}
	}

	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width / 2 - 294, Screen.height - 150, 588, 150), abilityBarTexture, abilityBarStyle);
		for (int abilityID = 0; abilityID < abilities.Length; abilityID++)
		{
			if (abilities[abilityID] != null)
			{
				GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height - 100, 80, 50), abilities[abilityID].currentCooldown.ToString());
			}
		}
	}

	[RPC]
	void SendCastAbility(int abilityID, Vector3 position, int targetID)
	{
		Debug.Log("Server received request to cast ability " + abilityID);
		queuedAbilityID = abilityID;
		queuedAbilityPosition = position;
		queuedAbilityTarget = targetID;
		needToSetCastTimer = true;
	}
}
