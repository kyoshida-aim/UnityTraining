using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {
	int enemyCurrentHP;
	int playerCurrentHP;
	int points;
	int turncount = 0;
	string enemyAction = "Wait";
	string playerAction = "Wait";
	GameObject enemyObject;
	GameObject battlelog;
	BattleText battleText;
	EnemyParams enemy;
	PlayerParams player;
	string waitText = "∇";
	string log;
	bool addNextText = false;
	AIRoutineList routineList;
	bool[] activatedActionList;

	void Start () {
		this.enemy = GetComponent<EnemyParams>();
		this.player = GetComponent<PlayerParams>();
		this.enemyCurrentHP = this.enemy.hp;
		this.playerCurrentHP = this.player.hp;
		this.enemyObject = GameObject.Find("Enemy");
		this.enemyObject.GetComponent<SpriteRenderer>().sprite = this.enemy.CharacterSprite;
		this.battlelog = GameObject.Find("BattleLog");
		this.battleText = GetComponent<BattleText>();
		this.routineList = GetComponent<AIRoutineList>();
		this.activatedActionList = new bool[this.routineList.size()];
		this.SetBattleLog(this.battleText.BattleStart);
	}

	public void Reset() {
		SceneManager.LoadScene("RuleBaseAI");
	}

	public void CallPlayerAction (int actionId) {
		// ターンカウントを進める
		this.turncount++;
		Debug.Log(string.Format("ターンカウント:{0}", this.turncount));
		Debug.Log(string.Format("敵のHP:{0}", this.enemyCurrentHP));
		// 敵の行動を決定する
		SetEnemyAction();

		// 入力されたボタンからプレイヤーの行動を決定
		if (actionId == 0) {
			this.playerAction = "Attack";
		} else if (actionId == 1){
			this.playerAction = "Heal";
		}

		// プレイヤーと敵の双方の行動を実行
		StartCoroutine("ExecuteAction");

	}

	void SetEnemyAction() {
		// if (this.enemyCurrentHP < 4) {
		// 	this.enemyAction = "Heal";
		// } else {
		// 	this.enemyAction = "Attack";
		// }

		// this.routineListから敵の行動を生成する。
		AIRoutine action = new AIRoutine();
		int counter = 0;
		float enemyHPPercentage = this.enemyCurrentHP * 100 / this.enemy.hp;
		float playerHPPercentage = this.playerCurrentHP * 100 / this.player.hp;
		bool satisfy_all_criteria;

		foreach (var routine in this.routineList.list()) {
			satisfy_all_criteria= true;

			// ターン数トリガー
			if (routine.useTurnValue){
				if (routine.ConstOrMulti == 0) {
					if (this.turncount != routine.turnValue) {
						satisfy_all_criteria = false;
					}
				} else if (routine.ConstOrMulti == 1) {
					if (this.turncount % routine.turnValue != 0) {
						satisfy_all_criteria = false;
					}
				}
			}

			// 敵のHPトリガー
			if (routine.enemyHPTrigger) {
				// 0: 以上 1:以下
				if (routine.enemyHP_ConditionRange == 0) {
					//条件を満たしていない時だけ次のループに移動する 
					if ( !(enemyHPPercentage >= routine.enemyHP_ConditionValue) ) {
							satisfy_all_criteria = false;
					}
				} else if (routine.enemyHP_ConditionRange == 1) {
					//条件を満たしていない時だけ次のループに移動する 
					if ( !(enemyHPPercentage <= routine.enemyHP_ConditionValue) ) {
							satisfy_all_criteria = false;
					}
				}
			}

			// 味方のHPトリガー
			if (routine.playerHPTrigger) {
				// 0: 以上 1:以下
				if (routine.playerHP_ConditionRange == 0) {
					//条件を満たしていない時だけ次のループに移動する 
					if ( !(playerHPPercentage >= routine.playerHP_ConditionValue) ) {
							satisfy_all_criteria = false;
					}
				} else if (routine.playerHP_ConditionRange == 1) {
					//条件を満たしていない時だけ次のループに移動する 
					if ( !(playerHPPercentage <= routine.playerHP_ConditionValue) ) {
							satisfy_all_criteria = false;
					}
				}
			}
			if ( satisfy_all_criteria && routine.actionOnce) {
				if (activatedActionList[counter]) {
					satisfy_all_criteria = false;
				}
				activatedActionList[counter] = true;
			}
			counter++;
			// ターンカウントを進めてからループ処理を進めるかの判定を入れる
			if (!satisfy_all_criteria) {
				continue;
			}

			// ここまできたら上記全ての条件をクリアーしている
			// actionに行動内容を登録して終了
			action = routine;
			routineList.selected = counter - 1;
			routineList.needRefresh = true;
			break;
		}
		if (action.actionID == 0) {
			this.enemyAction = "Attack";
		} else if (action.actionID == 1) {
			this.enemyAction = "Heal";
		} else if (action.actionID == 2) {
			this.enemyAction = "Wait";
		}
	}

	void SetBattleLog(string message, bool wait = false){
		if (this.addNextText) {
			message = this.log.Replace(this.waitText, "") + "\n" +  message;
			this.addNextText = false;
		}
		if (wait) {
			this.addNextText = true;
			message += this.waitText;
		}
		this.battlelog.GetComponent<Text>().text = MessageTranslate(message);
		this.log = message;
	}

	private string MessageTranslate(string message) {
        message = message.Replace("<PlayerName>", player.actorName);
        message = message.Replace("<EnemyName>", enemy.actorName);
        message = message.Replace("<Points>", this.points.ToString());
        return message;
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
			this.SetBattleLog(this.battleText.OnPlayerAttack, true);
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			this.points = this.CalculateDamage(this.player.atk, this.enemy.dfc);
			this.SetBattleLog(this.battleText.DealDamage);
			this.enemyCurrentHP -= this.points;
		} else if (this.playerAction == "Heal") {
			this.SetBattleLog(this.battleText.OnPlayerHeal, true);
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			// maxHpを超えての回復はしない
			// 実際の回復量は計算する
			this.points = this.CalculateHealing(this.player.hp, this.playerCurrentHP);
			this.SetBattleLog(this.battleText.Healed);
		}

		// 敵側の行動処理に入る前にウェイトを入れる
		yield return new WaitForSeconds (1.0f);

		// 敵側の行動実行(死亡確認も同時に行う)
		if (this.enemyCurrentHP <= 0) {
			Destroy(this.enemyObject);
			this.SetBattleLog(this.battleText.OnEnemyDefeat, true);
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			this.SetBattleLog(this.battleText.YouWin);
		} else if (this.enemyAction == "Attack") {
			this.SetBattleLog(this.battleText.OnEnemyAttack, true);
			yield return new WaitForSeconds (1.0f); // 1秒待つ
			this.points = this.CalculateDamage(this.enemy.atk, this.player.dfc);
			this.SetBattleLog(this.battleText.TakeDamage);
			this.playerCurrentHP -= this.points;
			if (this.playerCurrentHP <= 0) {
				yield return new WaitForSeconds (1.0f); // 1秒待つ
				this.SetBattleLog(this.battleText.OnPlayerDefeat);
				yield return new WaitForSeconds (1.0f);
				this.SetBattleLog(this.battleText.YouLose);
			}
		} else if (this.enemyAction == "Heal") {
			this.SetBattleLog(this.battleText.OnEnemyHeal, true);
			// 1秒待つ
			yield return new WaitForSeconds (1.0f);
			// maxHpを超えての回復はしない
			// 実際の回復量は計算する
			this.points = this.CalculateHealing(this.enemy.hp, this.enemyCurrentHP);
			this.SetBattleLog(this.battleText.Healed);
			this.enemyCurrentHP += this.points;
		} else if (this.enemyAction == "Wait") {
			this.SetBattleLog(this.battleText.EnemyWaiting);
		} 
	}

}
