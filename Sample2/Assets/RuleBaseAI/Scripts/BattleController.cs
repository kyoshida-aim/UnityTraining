using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(BattleText))]
public class BattleController : MonoBehaviour {
    int turnCount = 0;
    string enemyAction = "Wait";
    string playerAction = "Wait";
    [SerializeField] private ButtonView buttonView;
    [SerializeField] private MessageView messageView;
    [SerializeField] private EnemyView enemyView;
    BattleText battleText;
    Enemy enemy;
    Player player;
    bool[] activatedActionList;

    void Start () {
        this.enemy = GetComponent<Enemy>();
        this.enemyView.SetSprite(this.enemy.CharacterSprite);
        this.player = GetComponent<Player>();
        this.battleText = GetComponent<BattleText>();
        this.activatedActionList = new bool[this.enemy.routineList.Length];

        messageView.PlayerName = player.ActorName;
        messageView.EnemyName = enemy.ActorName;
        messageView.DisplayMessage(this.battleText.BattleStart);

        buttonView.OnAttackClick.AddListener(CallPlayerAttack);
        buttonView.OnHealClick.AddListener(CallPlayerHeal);
        buttonView.OnResetClick.AddListener(Reset);
    }

    public void Reset() {
        SceneManager.LoadScene("RuleBaseAI");
    }

    public void CallPlayerAttack () {
        this.playerAction = "Attack";
        processingTurnExecution();
    }

    public void CallPlayerHeal () {
        this.playerAction = "Heal";
       processingTurnExecution();
    }
    

    public void processingTurnExecution () {

        // ターンカウントを進める
        this.turnCount++;

        // 敵の行動を決定する
        SetEnemyAction();

        // プレイヤーと敵の双方の行動を実行
        StartCoroutine("ExecuteAction");

    }

    void SetEnemyAction() {
        AIRoutine action = new AIRoutine();
        int counter = 0;
        bool satisfy_all_criteria;

        foreach (var routine in this.enemy.routineList) {
            satisfy_all_criteria= true;

            // ターン数トリガー
            if (routine.useTurnValue){
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.ConstOrMulti == 0) {
                    if (this.turnCount != routine.turnValue) {
                        satisfy_all_criteria = false;
                    }
                } else if (routine.ConstOrMulti == 1) {
                    if (this.turnCount % routine.turnValue != 0) {
                        satisfy_all_criteria = false;
                    }
                }
            }

            // 敵のHPトリガー
            if (routine.enemyHPTrigger) {
                // 0: 以上 1:以下
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.enemyHP_ConditionRange == 0) {
                    if ( !(enemy.CurrentHPPercentage() >= routine.enemyHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                } else if (routine.enemyHP_ConditionRange == 1) {
                    if ( !(enemy.CurrentHPPercentage() <= routine.enemyHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                }
            }

            // 味方のHPトリガー
            if (routine.playerHPTrigger) {
                // 0: 以上 1:以下
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.playerHP_ConditionRange == 0) {
                    if ( !(player.CurrentHPPercentage() >= routine.playerHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                } else if (routine.playerHP_ConditionRange == 1) {
                    if ( !(player.CurrentHPPercentage() <= routine.playerHP_ConditionValue) ) {
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
            messageView.DisplayMessage(this.battleText.OnPlayerAttack, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            messageView.EffectQuantity = this.CalculateDamage(this.player.atk, this.enemy.dfc);
            messageView.DisplayMessage(this.battleText.DealDamage);
            this.enemy.decreaseHP(messageView.EffectQuantity);
        } else if (this.playerAction == "Heal") {
            messageView.DisplayMessage(this.battleText.OnPlayerHeal, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            messageView.EffectQuantity = this.CalculateHealing(this.player.hp, this.player.CurrentHP);
            messageView.DisplayMessage(this.battleText.Healed);
        }

        // 敵側の行動処理に入る前にウェイトを入れる
        yield return new WaitForSeconds (1.0f);

        // 敵側の行動実行(死亡確認も同時に行う)
        if (this.enemy.CurrentHP <= 0) {
            enemyView.OnDefeat();
            // this.enemyView.OnDefeat();
            messageView.DisplayMessage(this.battleText.OnEnemyDefeat, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            messageView.DisplayMessage(this.battleText.YouWin);
        } else if (this.enemyAction == "Attack") {
            messageView.DisplayMessage(this.battleText.OnEnemyAttack, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            messageView.EffectQuantity = this.CalculateDamage(this.enemy.atk, this.player.dfc);
            messageView.DisplayMessage(this.battleText.TakeDamage);
            this.player.decreaseHP(messageView.EffectQuantity);
            if (this.player.CurrentHP <= 0) {
                yield return new WaitForSeconds (1.0f); // 1秒待つ
                messageView.DisplayMessage(this.battleText.OnPlayerDefeat);
                yield return new WaitForSeconds (1.0f);
                messageView.DisplayMessage(this.battleText.YouLose);
            }
        } else if (this.enemyAction == "Heal") {
            messageView.DisplayMessage(this.battleText.OnEnemyHeal, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            messageView.EffectQuantity = this.CalculateHealing(this.enemy.hp, this.enemy.CurrentHP);
            messageView.DisplayMessage(this.battleText.Healed);
            this.enemy.increaseHP(messageView.EffectQuantity);
        } else if (this.enemyAction == "Wait") {
            messageView.DisplayMessage(this.battleText.EnemyWaiting);
        } 
    }

}
