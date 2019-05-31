using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyParams))]
[RequireComponent(typeof(PlayerParams))]
[RequireComponent(typeof(BattleText))]
public class BattleController : MonoBehaviour {
    int turnCount = 0;
    int effectQuantity;
    string enemyAction = "Wait";
    string playerAction = "Wait";
    private readonly string waitText = "∇";
    [SerializeField] private ButtonView buttonView;
    [SerializeField] private MessageView messageView;
    [SerializeField] private EnemyView enemyView;
    BattleText battleText;
    EnemyParams enemyParams;
    Actor enemy;
    PlayerParams playerParams;
    Actor player;

    ConditionChecker conditionChecker;

    void Start () {
        this.conditionChecker = new ConditionChecker();

        this.enemyParams = GetComponent<EnemyParams>();
        this.enemy = new Actor(enemyParams);
        this.enemyView.SetSprite(this.enemyParams.CharacterSprite);

        this.playerParams = GetComponent<PlayerParams>();
        this.player = new Actor(playerParams);

        this.battleText = GetComponent<BattleText>();

        updateMessageView(this.battleText.BattleStart);

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
    
    private void updateMessageView(string message, bool wait = false){
        message = join(message);
        message = addWaitText(message, wait);
        message = Translate(message);
        messageView.UpdateLog(message);
    }

    private string join(string message) {
        if (waitingForNextMessage()) {
            return messageView.Log.Replace(waitText, string.Empty) + "\n" +  message;
        }
        return message;
    }

    private bool waitingForNextMessage() {
        return messageView.Log.Contains(waitText);
    }
    private string addWaitText(string message, bool wait) {
        if (wait) { return message + waitText; }
        return message;
    }

    private string Translate(string message) {
        message = message.Replace("<PlayerName>", this.player.Name)
                    .Replace("<EnemyName>", this.enemy.Name)
                    .Replace("<Points>", effectQuantity.ToString());
        return message;
    }

    void processingTurnExecution () {

        // ターンカウントを進める
        this.turnCount++;

        // TODO : そのうち消す？
        Debug.Log(string.Format("ターンカウント:{0}", turnCount));
        Debug.Log(string.Format("敵の現在HP:{0}%", this.enemy.CurrentHPPercentage));

        // 敵の行動を決定する
        SetEnemyAction();

        // プレイヤーと敵の双方の行動を実行
        StartCoroutine("executeAction");

    }

    void SetEnemyAction() {
        // 長すぎる。インデント整理したい
        var action = conditionChecker.DetermineEnemyAction(enemyParams.RoutineList,
                                                turnCount,
                                                player.CurrentHPPercentage,
                                                enemy.CurrentHPPercentage);
        #if UNITY_EDITOR
            enemyParams.UpdateRoutineListIndex(action);
        #endif
        this.enemyAction = setAction(action.ActionID);
    }

    // TODO : 敵の行動を決定する方法を変更する
    private string setAction(int actionID) {
        if (actionID == 0) {
            return "Attack";
        } else if (actionID == 1) {
           return "Heal";
        }
        return "Wait";
    }
    private int calculateDamage(Actor attacker, Actor opponent) {
        return attacker.CalculateDamageDealtTo(opponent);
    }

    private int calculateHealing(Actor actor) {
        return actor.CalculateHealing();
    }

    private IEnumerator executeAction() {
        // メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する
        // 処理待ち中もボタンを押せるのでボタン連打厳禁

        // プレイヤー側の行動実行
        if (this.playerAction == "Attack") {
            updateMessageView(this.battleText.OnPlayerAttack, true);
            yield return wait(1.0f); // 1秒待つ
            effectQuantity = this.calculateDamage(this.player, this.enemy);
            this.enemy.DecreaseHP(effectQuantity);
            updateMessageView(this.battleText.DealDamage);
        } else if (this.playerAction == "Heal") {
            updateMessageView(this.battleText.OnPlayerHeal, true);
            yield return wait(1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            effectQuantity = this.calculateHealing(this.player);
            this.player.IncreaseHP(effectQuantity);
            updateMessageView(this.battleText.Healed);
        }

        // 敵側の行動処理に入る前にウェイトを入れる
        yield return wait(1.0f);

        // 敵側の行動実行(死亡確認も同時に行う)
        if (this.enemy.IsDead()) {
            enemyView.OnDefeat();
            // this.enemyView.OnDefeat();
            updateMessageView(this.battleText.OnEnemyDefeat, true);
            yield return wait(1.0f); // 1秒待つ
            updateMessageView(this.battleText.YouWin);
        } else if (this.enemyAction == "Attack") {
            updateMessageView(this.battleText.OnEnemyAttack, true);
            yield return wait(1.0f); // 1秒待つ
            effectQuantity = this.calculateDamage(this.enemy, this.player);
            updateMessageView(this.battleText.TakeDamage);
            this.player.DecreaseHP(effectQuantity);
            if (this.player.IsDead()) {
                yield return wait(1.0f); // 1秒待つ
                updateMessageView(this.battleText.OnPlayerDefeat);
                yield return wait(1.0f);
                updateMessageView(this.battleText.YouLose);
            }
        } else if (this.enemyAction == "Heal") {
            updateMessageView(this.battleText.OnEnemyHeal, true);
            yield return wait(1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            effectQuantity = this.calculateHealing(this.enemy);
            updateMessageView(this.battleText.Healed);
            this.enemy.IncreaseHP(effectQuantity);
        } else if (this.enemyAction == "Wait") {
            updateMessageView(this.battleText.EnemyWaiting);
        } 
    }
    private IEnumerator wait(float seconds) {
        yield return new WaitForSeconds(seconds);
    }
}
