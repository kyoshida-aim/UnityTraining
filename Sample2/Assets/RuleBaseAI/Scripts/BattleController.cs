using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(MessageView))]
[RequireComponent(typeof(EnemyView))]
[RequireComponent(typeof(BattleText))]
public class BattleController : MonoBehaviour {
    int enemyCurrentHP;
    int playerCurrentHP;
    int points;
    int turncount = 0;
    string enemyAction = "Wait";
    string playerAction = "Wait";
    MessageView messageView;
    EnemyView enemyView;
    BattleText battleText;
    Enemy enemy;
    Player player;
    bool[] activatedActionList;

    void Start () {
        this.enemy = GetComponent<Enemy>();
        this.enemyView = GetComponent<EnemyView>();
        this.enemyView.setSprite(this.enemy.CharacterSprite);
        this.player = GetComponent<Player>();
        this.enemyCurrentHP = this.enemy.hp;
        this.playerCurrentHP = this.player.hp;
        this.battleText = GetComponent<BattleText>();
        this.activatedActionList = new bool[this.enemy.routineList.Length];

        messageView = GetComponent<MessageView>();
        messageView.Set(this.battleText.BattleStart);
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
        AIRoutine action = new AIRoutine();
        int counter = 0;
        float enemyHPPercentage = this.enemyCurrentHP * 100 / this.enemy.hp;
        float playerHPPercentage = this.playerCurrentHP * 100 / this.player.hp;
        bool satisfy_all_criteria;

        foreach (var routine in this.enemy.routineList) {
            satisfy_all_criteria= true;

            // ターン数トリガー
            if (routine.useTurnValue){
                //条件を満たしていない時だけ次のループに移動する 
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
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.enemyHP_ConditionRange == 0) {
                    if ( !(enemyHPPercentage >= routine.enemyHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                } else if (routine.enemyHP_ConditionRange == 1) {
                    if ( !(enemyHPPercentage <= routine.enemyHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                }
            }

            // 味方のHPトリガー
            if (routine.playerHPTrigger) {
                // 0: 以上 1:以下
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.playerHP_ConditionRange == 0) {
                    if ( !(playerHPPercentage >= routine.playerHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                } else if (routine.playerHP_ConditionRange == 1) {
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
            this.enemy.routineIndex = counter;
            counter++;
            // ターンカウントを進めてからループ処理を進めるかの判定を入れる
            if (!satisfy_all_criteria) {
                continue;
            }

            // ここまできたら上記全ての条件をクリアーしている
            // actionに行動内容を登録して終了
            action = routine;
            this.enemy.needRefresh = true;
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

    // ダメージ計算方法の統一のため関数化
    int CalculateDamage(int attack, int defence) {
        return Math.Max(attack - defence, 0);
    }

    int CalculateHealing(int maxhp, int hp) {
        return Mathf.Clamp(maxhp - hp, 0, 3);
    }

    private IEnumerator ExecuteAction() {
        // メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する
        // 処理待ち中もボタンを押せるのでボタン連打厳禁

        // プレイヤー側の行動実行
        if (this.playerAction == "Attack") {
            messageView.Set(this.battleText.OnPlayerAttack, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            this.points = this.CalculateDamage(this.player.atk, this.enemy.dfc);
            messageView.Set(this.battleText.DealDamage);
            this.enemyCurrentHP -= this.points;
        } else if (this.playerAction == "Heal") {
            messageView.Set(this.battleText.OnPlayerHeal, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            this.points = this.CalculateHealing(this.player.hp, this.playerCurrentHP);
            messageView.Set(this.battleText.Healed);
        }

        // 敵側の行動処理に入る前にウェイトを入れる
        yield return new WaitForSeconds (1.0f);

        // 敵側の行動実行(死亡確認も同時に行う)
        if (this.enemyCurrentHP <= 0) {
            // this.enemyView.OnDefeat();
            messageView.Set(this.battleText.OnEnemyDefeat, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            messageView.Set(this.battleText.YouWin);
        } else if (this.enemyAction == "Attack") {
            messageView.Set(this.battleText.OnEnemyAttack, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            this.points = this.CalculateDamage(this.enemy.atk, this.player.dfc);
            messageView.Set(this.battleText.TakeDamage);
            this.playerCurrentHP -= this.points;
            if (this.playerCurrentHP <= 0) {
                yield return new WaitForSeconds (1.0f); // 1秒待つ
                messageView.Set(this.battleText.OnPlayerDefeat);
                yield return new WaitForSeconds (1.0f);
                messageView.Set(this.battleText.YouLose);
            }
        } else if (this.enemyAction == "Heal") {
            messageView.Set(this.battleText.OnEnemyHeal, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            this.points = this.CalculateHealing(this.enemy.hp, this.enemyCurrentHP);
            messageView.Set(this.battleText.Healed);
            this.enemyCurrentHP += this.points;
        } else if (this.enemyAction == "Wait") {
            messageView.Set(this.battleText.EnemyWaiting);
        } 
    }

}
