using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{
	public int baseHealth;
	public int currentHealth;

	public float baseAttackSpeed;
	public float baseAttackDamage;
	public float baseDefencePotential;
	public float baseMovementSpeed;

	public float attackSpeed;
	public float attackDamage;
	public float defencePotential;
	public float movementSpeed;
	
	public Buff[] buffs = new Buff[5];
	private bool updateStatsRequired = true;

	public void DoDamage(int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			currentHealth = baseHealth;
			// death
		}
	}
		
	void UpdateStats()
	{
		attackSpeed = baseAttackSpeed;
		attackDamage = baseAttackDamage;
		defencePotential = baseDefencePotential;
		movementSpeed = baseMovementSpeed;
		
		for (int buffID = 0; buffID < buffs.Length; buffID++)
		{
			if (buffs[buffID] != null && buffs[buffID].statModifiers != null)
			{
				for (int modifierID = 0; modifierID < buffs[buffID].statModifiers.Length; modifierID++)
				{
					//if (buffs[buffID].statModifiers[modifierID].stat != null) {
					StatModifier currentStatModifier = buffs[buffID].statModifiers[modifierID];
						
					switch (currentStatModifier.stat)
					{
						case Stat.AttackDamage:
							attackDamage += currentStatModifier.modifier;
							break;
						case Stat.AttackSpeed:
							attackSpeed += currentStatModifier.modifier;
							break;
						case Stat.DefencePotential:
							defencePotential += currentStatModifier.modifier;
							break;
						case Stat.MovementSpeed:
							movementSpeed += currentStatModifier.modifier;
							break;
					}
						
					//}
				}
			}
		}
		
		updateStatsRequired = false;
	}

	public void UpdateBuffs()
	{
		for (int buffID = 0; buffID < buffs.Length; buffID++)
		{
			if (buffs[buffID] != null)
			{
				buffs[buffID].OnTickEffect(gameObject);
				buffs[buffID].timeOfBuffRemaining -= Time.deltaTime;
				if (buffs[buffID].timeOfBuffRemaining <= 0.0f)
				{
					buffs[buffID].OnEndEffect(gameObject);
					ClearBuff(buffID);
				}
			}
		}
	}

	[RPC]
	public void ApplyBuff(Buff buffToSet)
	{
		Debug.Log("Applying buff to stats");
		for (int buffID = 0; buffID < buffs.Length; buffID++)
		{
			if (buffs[buffID] == null)
			{
				buffs[buffID] = buffToSet;
				Debug.Log("Set buff successfully");
				break;
			}
		}
		updateStatsRequired = true;
	}

	public void ClearBuff(int buffID)
	{
		buffs[buffID] = null;
		updateStatsRequired = true;
	}

	public void ClearAllBuffs()
	{
		for (int buffID = 0; buffID < buffs.Length; buffID++)
		{
			buffs[buffID] = null;
		}
		updateStatsRequired = true;
	}

	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (updateStatsRequired)
		{
			UpdateStats();
		}
		UpdateBuffs();
	}

	void OnGUI()
	{
		for (int buffID = 0; buffID < buffs.Length; buffID++)
		{
			if (buffs[buffID] != null)
			{
				GUI.Label(new Rect(100.0f, 200.0f, 100.0f, 100.0f), buffs[buffID].name);
			}
		}
	}
}
