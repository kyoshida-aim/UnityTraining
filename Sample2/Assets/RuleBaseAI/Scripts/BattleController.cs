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
        this.activatedActionList = new bool[this.enemy.RoutineList.Length];

        messageView.PlayerName = player.ActorName;
        messageView.EnemyName = enemy.ActorName;
        messageView.DisplayMessage(this.battleText.BattleStart);

        buttonView.OnAttackClick.AddListener(callPlayerAttack);
        buttonView.OnHealClick.AddListener(callPlayerHeal);
        buttonView.OnResetClick.AddListener(reset);
    }

    void reset() {
        SceneManager.LoadScene("RuleBaseAI");
    }

   void callPlayerAttack () {
        this.playerAction = "Attack";
        processingTurnExecution();
    }

    void callPlayerHeal () {
        this.playerAction = "Heal";
       processingTurnExecution();
    }
    

    void processingTurnExecution () {

        // ターンカウントを進める
        this.turnCount++;

        // 敵の行動を決定する
        SetEnemyAction();

        // プレイヤーと敵の双方の行動を実行
        StartCoroutine("executeAction");

    }

    void SetEnemyAction() {
        AIRoutine action = new AIRoutine();
        int counter = 0;
        bool satisfy_all_criteria;

        foreach (var routine in this.enemy.RoutineList) {
            satisfy_all_criteria= true;

            // ターン数トリガー
            if (routine.UseTurnValue){
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.ConstOrMulti == 0) {
                    if (this.turnCount != routine.TurnValue) {
                        satisfy_all_criteria = false;
                    }
                } else if (routine.ConstOrMulti == 1) {
                    if (this.turnCount % routine.TurnValue != 0) {
                        satisfy_all_criteria = false;
                    }
                }
            }

            // 敵のHPトリガー
            if (routine.EnemyHPTrigger) {
                // 0: 以上 1:以下
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.EnemyHP_ConditionRange == 0) {
                    if ( !(enemy.CurrentHPPercentage() >= routine.EnemyHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                } else if (routine.EnemyHP_ConditionRange == 1) {
                    if ( !(enemy.CurrentHPPercentage() <= routine.EnemyHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                }
            }

            // 味方のHPトリガー
            if (routine.PlayerHPTrigger) {
                // 0: 以上 1:以下
                //条件を満たしていない時だけ次のループに移動する 
                if (routine.PlayerHP_ConditionRange == 0) {
                    if ( !(player.CurrentHPPercentage() >= routine.PlayerHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                } else if (routine.PlayerHP_ConditionRange == 1) {
                    if ( !(player.CurrentHPPercentage() <= routine.PlayerHP_ConditionValue) ) {
                            satisfy_all_criteria = false;
                    }
                }
            }
            if ( satisfy_all_criteria && routine.ActionOnce) {
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
        if (action.ActionID == 0) {
            this.enemyAction = "Attack";
        } else if (action.ActionID == 1) {
            this.enemyAction = "Heal";
        } else if (action.ActionID == 2) {
            this.enemyAction = "Wait";
        }
    }

    // ダメージ計算方法の統一のため関数化
    int calculateDamage(int attack, int defence) {
        return Math.Max(attack - defence, 0);
    }

    int calculateHealing(int maxhp, int hp) {
        return Mathf.Clamp(maxhp - hp, 0, 3);
    }

    private IEnumerator executeAction() {
        // メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する
        // 処理待ち中もボタンを押せるのでボタン連打厳禁

        // プレイヤー側の行動実行
        if (this.playerAction == "Attack") {
            messageView.DisplayMessage(this.battleText.OnPlayerAttack, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            messageView.EffectQuantity = this.calculateDamage(this.player.Atk, this.enemy.Dfc);
            messageView.DisplayMessage(this.battleText.DealDamage);
            this.enemy.DecreaseHP(messageView.EffectQuantity);
        } else if (this.playerAction == "Heal") {
            messageView.DisplayMessage(this.battleText.OnPlayerHeal, true);
            yield return new WaitForSeconds (1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            messageView.EffectQuantity = this.calculateHealing(this.player.Hp, this.player.CurrentHP);
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
            messageView.EffectQuantity = this.calculateDamage(this.enemy.Atk, this.player.Dfc);
            messageView.DisplayMessage(this.battleText.TakeDamage);
            this.player.DecreaseHP(messageView.EffectQuantity);
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
            messageView.EffectQuantity = this.calculateHealing(this.enemy.Hp, this.enemy.CurrentHP);
            messageView.DisplayMessage(this.battleText.Healed);
            this.enemy.IncreaseHP(messageView.EffectQuantity);
        } else if (this.enemyAction == "Wait") {
            messageView.DisplayMessage(this.battleText.EnemyWaiting);
        } 
    }

}
