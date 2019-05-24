using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleText : MonoBehaviour {

	[SerializeField, Multiline] private string battleStart = "{0} が あらわれた！";
	[SerializeField, Multiline] private string onAttack = "{0} の こうげき！";
	[SerializeField, Multiline] private string dealDamage = "{0} の ダメージ　を　あたえた！";
	[SerializeField, Multiline] private string takeDamage = "{0} の ダメージ　を　うけた！";
	[SerializeField, Multiline] private string enchantHeal = "{0} は かいふく した！";
	[SerializeField, Multiline] private string healed = "たいりょく　が　{0} かいふく　した！";

	public string BattleStart { get { return battleStart; } }
	public string OnAttack { get { return onAttack; } }
	public string DealDamage { get { return dealDamage; } }
	public string TakeDamage { get { return takeDamage; } }
	public string EnchantHeal { get { return enchantHeal; } }
	public string Healed { get { return healed; } }
}
