using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {
	// Use this for initialization
	int enemyMaxHp = 10;
	int enemyHp;
	int yourMaxHp = 10;
	int yourHp;
	string enemyAction = "Wait";
	string playerAction = "Wait";
	GameObject battlelog;

	void Start () {
		this.enemyHp = this.enemyMaxHp;
		this.yourHp = this.yourMaxHp;
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
		// メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する
		if (this.playerAction == "Attack") {
			this.battlelog.GetComponent<Text>().text = 
				"あなた　の　こうげき　！　∇";
			// 1秒待つ
			yield return new WaitForSeconds (1.0f);
			// 攻撃力設定も特にしない
			this.enemyHp -= 3;
			this.battlelog.GetComponent<Text>().text = 
				"あなた　の　こうげき　！　\n3　ダメージ　あたえた！";
			if (this.enemyHp < 0) {
				Debug.Log("敵が死亡した");
				this.enemyAction = "Dead";
			}
		} else if (this.playerAction == "Heal") {
			this.battlelog.GetComponent<Text>().text = 
				"あなた　は　かいふくまほう　を　となえた　！ ∇";
			// 1秒待つ
			yield return new WaitForSeconds (1.0f);
			// maxHpを超えての回復はしない
			// 実際の回復量は計算する
			int healedHp = Math.Min(yourMaxHp, yourHp + 3);
			this.battlelog.GetComponent<Text>().text = 
				string.Format("あなた　は　かいふくまほう　を　となえた　！\nたいりょく　が　{0} かいふく　した！", healedHp);
		}
	}
}
