using UnityEngine;
using System.Collections;

public enum BuffType
{
	Timed,
	Ammo
}

public enum Stat
{
	AttackSpeed,
	AttackDamage,
	DefencePotential,
	MovementSpeed
}

public struct StatModifier
{
	public StatModifier(Stat statToSet, float modifierToSet)
	{
		stat = statToSet;
		modifier = modifierToSet;
	}

	public Stat stat;
	public float modifier;
}

public class Buff
{

	public string name;
	public BuffType buffType;
	public float timeOfBuff;
	public float timeOfBuffRemaining;
	public StatModifier[] statModifiers;

	virtual public void OnStartEffect(GameObject player)
	{
	
	}

	virtual public void OnTickEffect(GameObject player)
	{
		
	}

	virtual public void OnEndEffect(GameObject player)
	{
	
	}

}
