using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {
	int enemyMaxHp;
	int enemyHp;
	int yourMaxHp = 10;
	int yourHp;
	string enemyAction = "Wait";
	string playerAction = "Wait";
	GameObject enemyObject;
	GameObject battlelog;
	public GameObject EnemySettings;

	void Start () {
		// this.enemyHp = this.enemyMaxHp;
		this.enemyMaxHp = EnemySettings.GetComponent<CharacterParameterSettings>().hp;
		this.enemyHp = EnemySettings.GetComponent<CharacterParameterSettings>().hp;
		this.yourHp = this.yourMaxHp;
		this.enemyObject = GameObject.Find("Enemy");
		this.battlelog = GameObject.Find("BattleLog");
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

	private IEnumerator ExecuteAction() {
		int healedHp;
		// メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する

		// プレイヤー側の行動実行
		if (this.playerAction == "Attack") {
			this.battlelog.GetComponent<Text>().text = 
				"あなた　の　こうげき！　∇";
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			// 攻撃力設定も特にしない
			this.enemyHp -= 3;
			this.battlelog.GetComponent<Text>().text =
				this.battlelog.GetComponent<Text>().text.Replace("∇", "");
			this.battlelog.GetComponent<Text>().text += "\n3　ダメージ　あたえた！";
		} else if (this.playerAction == "Heal") {
			this.battlelog.GetComponent<Text>().text = 
				"あなた　は　かいふくまほう　を　となえた！ ∇";
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			// maxHpを超えての回復はしない
			// 実際の回復量は計算する
			healedHp = Mathf.Clamp(this.yourMaxHp - this.yourHp, 0, 3);
			this.battlelog.GetComponent<Text>().text =
				this.battlelog.GetComponent<Text>().text.Replace("∇", "");
			this.battlelog.GetComponent<Text>().text += string.Format("\nたいりょく　が　{0} かいふく　した！", healedHp);
		}

		// 敵側の行動処理に入る前にウェイトを入れる
		yield return new WaitForSeconds (1.0f);

		// 敵側の行動実行(死亡確認も同時に行う)
		if (this.enemyHp <= 0) {
			Destroy(this.enemyObject);
			this.battlelog.GetComponent<Text>().text = 
				"てき　が　たおれた！　∇";
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			this.battlelog.GetComponent<Text>().text = "あなた　の　かち！";
		} else if (this.enemyAction == "Attack") {
			this.battlelog.GetComponent<Text>().text = 	"てき　の　こうげき！　∇";
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			// 攻撃力設定も特にしない
			this.yourHp -= 3;
			this.battlelog.GetComponent<Text>().text =
				this.battlelog.GetComponent<Text>().text.Replace("∇", "");
			this.battlelog.GetComponent<Text>().text += "\n3　ダメージ　うけた！";
			if (this.yourHp <= 0) {
				yield return new WaitForSeconds (1.0f); // 1秒待つ
				this.battlelog.GetComponent<Text>().text = 	"あなた　の　たいりょく　が　なくなった∇";
				yield return new WaitForSeconds (1.0f);
				this.battlelog.GetComponent<Text>().text = 	"ゲームオーバー";
			}
		} else if (this.enemyAction == "Heal") {
			this.battlelog.GetComponent<Text>().text = 	"てき　は　かいふく　した！　∇";
			// 1秒待つ
			yield return new WaitForSeconds (1.0f);
			// maxHpを超えての回復はしない
			// 実際の回復量は計算する
			healedHp = Mathf.Clamp(this.enemyMaxHp - this.enemyHp, 0, 3);
			this.battlelog.GetComponent<Text>().text =
				this.battlelog.GetComponent<Text>().text.Replace("∇", "");
			this.battlelog.GetComponent<Text>().text += string.Format("\nたいりょく　が　{0} かいふく　した！", healedHp);
		}
	}
}
