using UnityEngine;
using System.Collections;

public class BuffExample : Buff
{
	
	public BuffExample()
	{
		name = "Example buff";
		buffType = BuffType.Timed;
		timeOfBuff = 5.0f;
		timeOfBuffRemaining = 5.0f;
		statModifiers = new StatModifier[2];
		statModifiers[0] = new StatModifier(Stat.AttackSpeed, 5.0f);
		statModifiers[1] = new StatModifier(Stat.MovementSpeed, 5.0f);
	}
	
}
