using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {
	int enemyHp;
	int yourHp;
	string enemyAction = "Wait";
	string playerAction = "Wait";
	GameObject enemyObject;
	GameObject battlelog;
	BattleText battleText;
	public CharacterParameterSettings enemy;
	public CharacterParameterSettings you;
	string waitText = "∇";
	string log;

	void Start () {
		// this.enemyHp = this.enemyMaxHp;
		this.enemyHp = this.enemy.hp;
		this.yourHp = this.you.hp;
		this.enemyObject = GameObject.Find("Enemy");
		this.enemyObject.GetComponent<SpriteRenderer>().sprite = this.enemy.CharacterSprite;
		this.battlelog = GameObject.Find("BattleLog");
		this.battleText = GetComponent<BattleText>();
		this.SetBattleLog(String.Format(this.battleText.BattleStart(), this.enemy.actorName));
	}

	public void Reset() {
		SceneManager.LoadScene("RuleBaseAI");
	}

	public void CallPlayerAction (int actionId) {
		// 敵の行動を決定する
		SetEnemyAction();

		// 入力されたボタンからプレイヤーの行動を決定
		if (actionId == 0) {
			this.playerAction = "Attack";
		} else if (actionId == 1){
			this.playerAction = "Heal";
		}

		//プレイヤーと敵の双方の行動を実行
		StartCoroutine("ExecuteAction");
	}

	void SetEnemyAction() {
		if (this.enemyHp < 4) {
			this.enemyAction = "Heal";
		} else {
			this.enemyAction = "Attack";
		}
	}

	void SetBattleLog(string message, bool wait = false){
		if (wait) {
			message += this.waitText;
		}
		this.battlelog.GetComponent<Text>().text = message;
		this.log = message;
	}

	void RemoveWaitText() {
		this.SetBattleLog(this.log.Replace(this.waitText, ""));
	}

	// ダメージ計算方法の統一のため関数化
	int CalculateDamage(int attack, int defence) {
		return Math.Max(attack - defence, 0);
	}

	int CalculateHealing(int maxhp, int hp) {
		return Mathf.Clamp(maxhp - hp, 0, 3);
	}

	private IEnumerator ExecuteAction() {
		// メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する

		// プレイヤー側の行動実行
		if (this.playerAction == "Attack") {
			this.SetBattleLog(String.Format("{0}　の　こうげき！", this.you.actorName), true);
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			this.RemoveWaitText();
			int damage = this.CalculateDamage(this.you.atk, this.enemy.dfc);
			this.SetBattleLog(this.log + String.Format("\n{0}　ダメージ　あたえた！", damage));
			this.enemyHp -= damage;
		} else if (this.playerAction == "Heal") {
			this.SetBattleLog(String.Format("{0} は かいふくまほう を となえた！ ∇", this.enemy.actorName));
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			// maxHpを超えての回復はしない
			// 実際の回復量は計算する
			int healedHp = this.CalculateHealing(this.you.hp, this.yourHp);
			this.RemoveWaitText();
			this.SetBattleLog(this.log + string.Format("\nたいりょく　が　{0} かいふく　した！", healedHp));
		}

		// 敵側の行動処理に入る前にウェイトを入れる
		yield return new WaitForSeconds (1.0f);

		// 敵側の行動実行(死亡確認も同時に行う)
		if (this.enemyHp <= 0) {
			Destroy(this.enemyObject);
			this.SetBattleLog("てき　が　たおれた！", true);
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			this.SetBattleLog("あなた　の　かち！");
		} else if (this.enemyAction == "Attack") {
			this.SetBattleLog(String.Format("{0}　の　こうげき！　∇", this.you.actorName));
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			this.RemoveWaitText();
			int damage = this.CalculateDamage(this.enemy.atk, this.you.dfc);
			this.SetBattleLog(this.log + String.Format("\n{0}　ダメージ　あたえた！", damage));
			this.yourHp -= damage;
			if (this.yourHp <= 0) {
				yield return new WaitForSeconds (1.0f); // 1秒待つ
				this.SetBattleLog("あなた　の　たいりょく　が　なくなった", true);
				yield return new WaitForSeconds (1.0f);
				this.SetBattleLog("ゲームオーバー");
			}
		} else if (this.enemyAction == "Heal") {
			this.SetBattleLog(	"てき　は　かいふく　した！", true);
			// 1秒待つ
			yield return new WaitForSeconds (1.0f);
			// maxHpを超えての回復はしない
			// 実際の回復量は計算する
			int healedHp = this.CalculateHealing(this.enemy.hp, this.enemyHp);
			this.RemoveWaitText();
			this.SetBattleLog(this.log + string.Format("\nたいりょく　が　{0} かいふく　した！", healedHp));
		}
	}
}
