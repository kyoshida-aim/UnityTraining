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

        UpdateMessageView(this.battleText.BattleStart);

        buttonView.OnAttackClick.AddListener(CallPlayerAttack);
        buttonView.OnHealClick.AddListener(CallPlayerHeal);
        buttonView.OnResetClick.AddListener(Reset);
    }

    void Reset() {
        SceneManager.LoadScene("RuleBaseAI");
    }

   void CallPlayerAttack () {
        this.playerAction = "Attack";
        ProcessingTurnExecution();
    }

    void CallPlayerHeal () {
        this.playerAction = "Heal";
       ProcessingTurnExecution();
    }
    
    private void UpdateMessageView(string message, bool wait = false){
        message = Join(message);
        message = AddWaitText(message, wait);
        message = Translate(message);
        messageView.UpdateLog(message);
    }

    private string Join(string message) {
        if (WaitingForNextMessage()) {
            return messageView.Log.Replace(waitText, string.Empty) + "\n" +  message;
        }
        return message;
    }

    private bool WaitingForNextMessage() {
        return messageView.Log.Contains(waitText);
    }
    private string AddWaitText(string message, bool wait) {
        if (wait) { return message + waitText; }
        return message;
    }

    private string Translate(string message) {
        message = message.Replace("<PlayerName>", this.player.Name)
                    .Replace("<EnemyName>", this.enemy.Name)
                    .Replace("<Points>", effectQuantity.ToString());
        return message;
    }

    void ProcessingTurnExecution () {

        // ターンカウントを進める
        this.turnCount++;

        // TODO : そのうち消す？
        Debug.Log(string.Format("ターンカウント:{0}", turnCount));
        Debug.Log(string.Format("敵の現在HP:{0}%", this.enemy.CurrentHPPercentage));

        // 敵の行動を決定する
        SetEnemyAction();

        // プレイヤーと敵の双方の行動を実行
        StartCoroutine("ExecuteAction");

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
        this.enemyAction = SetAction(action.ActionID);
    }

    // TODO : 敵の行動を決定する方法を変更する
    private string SetAction(int actionID) {
        if (actionID == 0) {
            return "Attack";
        } else if (actionID == 1) {
           return "Heal";
        }
        return "Wait";
    }
    private int CalculateDamage(Actor attacker, Actor opponent) {
        return attacker.CalculateDamageDealtTo(opponent);
    }

    private int CalculateHealing(Actor actor) {
        return actor.CalculateHealing();
    }

    private IEnumerator ExecuteAction() {
        // メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する
        // 処理待ち中もボタンを押せるのでボタン連打厳禁

        // プレイヤー側の行動実行
        if (this.playerAction == "Attack") {
            UpdateMessageView(this.battleText.OnPlayerAttack, true);
            yield return Wait(1.0f); // 1秒待つ
            effectQuantity = this.CalculateDamage(this.player, this.enemy);
            this.enemy.DecreaseHP(effectQuantity);
            UpdateMessageView(this.battleText.DealDamage);
        } else if (this.playerAction == "Heal") {
            UpdateMessageView(this.battleText.OnPlayerHeal, true);
            yield return Wait(1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            effectQuantity = this.CalculateHealing(this.player);
            this.player.IncreaseHP(effectQuantity);
            UpdateMessageView(this.battleText.Healed);
        }

        // 敵側の行動処理に入る前にウェイトを入れる
        yield return Wait(1.0f);

        // 敵側の行動実行(死亡確認も同時に行う)
        if (this.enemy.IsDead()) {
            enemyView.OnDefeat();
            // this.enemyView.OnDefeat();
            UpdateMessageView(this.battleText.OnEnemyDefeat, true);
            yield return Wait(1.0f); // 1秒待つ
            UpdateMessageView(this.battleText.YouWin);
        } else if (this.enemyAction == "Attack") {
            UpdateMessageView(this.battleText.OnEnemyAttack, true);
            yield return Wait(1.0f); // 1秒待つ
            effectQuantity = this.CalculateDamage(this.enemy, this.player);
            UpdateMessageView(this.battleText.TakeDamage);
            this.player.DecreaseHP(effectQuantity);
            if (this.player.IsDead()) {
                yield return Wait(1.0f); // 1秒待つ
                UpdateMessageView(this.battleText.OnPlayerDefeat);
                yield return Wait(1.0f);
                UpdateMessageView(this.battleText.YouLose);
            }
        } else if (this.enemyAction == "Heal") {
            UpdateMessageView(this.battleText.OnEnemyHeal, true);
            yield return Wait(1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            effectQuantity = this.CalculateHealing(this.enemy);
            UpdateMessageView(this.battleText.Healed);
            this.enemy.IncreaseHP(effectQuantity);
        } else if (this.enemyAction == "Wait") {
            UpdateMessageView(this.battleText.EnemyWaiting);
        } 
    }
    private IEnumerator Wait(float seconds) {
        yield return new WaitForSeconds(seconds);
    }
}
