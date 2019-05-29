using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BattleText : MonoBehaviour {

	// [SerializeField, Multiline] private string battleStart = "{0} が あらわれた！";
	[SerializeField, BattleTextWithPreview] private string battleStart = "<EnemyName> が あらわれた！";
	[SerializeField, BattleTextWithPreview] private string enemyWaiting= "<EnemyName> は ようすを　みている...";
	[SerializeField, BattleTextWithPreview] private string onPlayerAttack = "<PlayerName> の こうげき！";
	[SerializeField, BattleTextWithPreview] private string onEnemyAttack = "<EnemyName> の こうげき！";
	[SerializeField, BattleTextWithPreview] private string dealDamage = "<Points> の ダメージ　を　あたえた！";
	[SerializeField, BattleTextWithPreview] private string takeDamage = "<Points> の ダメージ　を　うけた！";
	[SerializeField, BattleTextWithPreview] private string onPlayerHeal = "<PlayerName> は かいふく した！";
	[SerializeField, BattleTextWithPreview] private string onEnemyHeal = "<EnemyName> は かいふく した！";
	[SerializeField, BattleTextWithPreview] private string healed = "たいりょく　が　<Points> かいふく　した！";
	[SerializeField, BattleTextWithPreview] private string onEnemyDefeat = "<EnemyName> は たおれた！";
	[SerializeField, BattleTextWithPreview] private string youWin = "<PlayerName> の かち！";
	[SerializeField, BattleTextWithPreview] private string onPlayerDefeat = "<PlayerName> の たいりょく　が　なくなった";
	[SerializeField, BattleTextWithPreview] private string youLose = "ゲームオーバー";

	public string BattleStart { get { return battleStart; } }
	public string EnemyWaiting { get { return enemyWaiting; } }
	public string OnPlayerAttack { get { return onPlayerAttack; } }
	public string OnEnemyAttack { get { return onEnemyAttack; } }
	public string DealDamage { get { return dealDamage; } }
	public string TakeDamage { get { return takeDamage; } }
	public string OnPlayerHeal { get { return onPlayerHeal; } }
	public string OnEnemyHeal { get { return onEnemyHeal; } }
	public string Healed { get { return healed; } }
	public string OnEnemyDefeat {get { return onEnemyDefeat; } }
	public string YouWin { get { return youWin; } }
	public string OnPlayerDefeat { get { return onPlayerDefeat; } }
	public string YouLose { get { return youLose; } }
	
}
