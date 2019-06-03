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
    private readonly string waitText = "∇";
    [SerializeField] private ButtonView buttonView;
    [SerializeField] private MessageView messageView;
    [SerializeField] private EnemyView enemyView;
    BattleText battleText;
    EnemyParams enemyParams;
    Enemy enemy;
    PlayerParams playerParams;
    Player player;

    void Start () {

        this.enemyParams = GetComponent<EnemyParams>();
        this.enemy = new Enemy(enemyParams);
        this.enemyView.SetSprite(this.enemyParams.CharacterSprite);

        this.playerParams = GetComponent<PlayerParams>();
        this.player = new Player(playerParams);

        this.battleText = GetComponent<BattleText>();

        UpdateMessageView(this.battleText.BattleStart);

        RegisterButtonAction();
    }

    void RegisterButtonAction() {
        EnableButtonAction();
        buttonView.OnResetClick.AddListener(Reset);
    }

    // 攻撃と回復のボタンは有効・無効を切り替えたいので切り分け
    void EnableButtonAction() {
        buttonView.OnAttackClick.AddListener(CallPlayerAttack);
        buttonView.OnHealClick.AddListener(CallPlayerHeal);
    }
    
    // ボタンを無効化するというよりはボタンを押しても何も処理されないように変更する
    void DisableButtonAction() {
        buttonView.OnAttackClick.RemoveAllListeners();
        buttonView.OnHealClick.RemoveAllListeners();
    }

    void Reset() {
        SceneManager.LoadScene("RuleBaseAI");
    }

   void CallPlayerAttack () {
        this.player.SetAttack();
        ProcessingTurnExecution();
    }

    void CallPlayerHeal () {
        this.player.SetHeal();
       ProcessingTurnExecution();
    }
    
    private void UpdateMessageView(string message, int points = 0, bool wait = false){
        message = Join(message);
        message = AddWaitText(message, wait);
        message = Translate(message, points);
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

    private string Translate(string message, int points) {
        message = message.Replace("<PlayerName>", this.player.Name)
                    .Replace("<EnemyName>", this.enemy.Name)
                    .Replace("<Points>", points.ToString());
        return message;
    }

    void ProcessingTurnExecution () {
        // 連打対策にボタンを無効化
        DisableButtonAction();
        
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
        int inspectorIndex = this.enemy.DetermineEnemyAction(turnCount, player.CurrentHPPercentage);
        #if UNITY_EDITOR
            enemyParams.Index = inspectorIndex;
            enemyParams.NeedRefresh = true;
        #endif
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
        // NOTE : 
        // 状態異常等の「死んでないが行動できない」状態を実装するなら行動可能かどうかを取得する関数を用意したい。
        yield return ExecutePlayerAction();

        // 敵側の行動処理に入る前にウェイトを入れる
        yield return Wait(1.0f);

        // 敵側の状態確認
        yield return CheckEnemyStatus();
        
        // 敵が死亡しているならこれ以降の処理は行わない
        if (this.enemy.IsDead()) {
            yield break;
        }

        // 敵側の行動実行
        // NOTE : 
        // 状態異常等の「死んでないが行動できない」状態を実装するなら行動可能かどうかを取得する関数を用意したい。
        if (!this.enemy.IsDead()) {
            yield return ExecuteEnemyAction();
        }

        yield return CheckPlayerStatus();
        // プレイヤーが死亡しているならボタンの有効化を行わない
        if (this.player.IsDead()) {
            yield break;
        }

        // 全ての処理が終わったらボタン入力を受け付けるようにする
        EnableButtonAction();
    }

    private IEnumerator ExecutePlayerAction() {
        switch(this.player.Action)
        {
            case ActionList.Attack:
                yield return PlayerAttack();
                break;
            case ActionList.Heal:
                yield return PlayerHeal();
                break;
        }

        // Unityエディター側の警告回避用。これが呼び出されることがあってはいけない
        yield return null;
    }

    private IEnumerator PlayerAttack() {
        UpdateMessageView(this.battleText.OnPlayerAttack, wait: true);
        yield return Wait(1.0f);
        int effectQuantity = this.CalculateDamage(this.player, this.enemy);
        this.enemy.DecreaseHP(effectQuantity);
        UpdateMessageView(this.battleText.DealDamage, points: effectQuantity);
    }

    private IEnumerator PlayerHeal() {
        UpdateMessageView(this.battleText.OnPlayerHeal, wait: true);
        yield return Wait(1.0f);
        // maxHpを超えての回復はしない
        // 実際の回復量は計算する
        int effectQuantity = this.CalculateHealing(this.player);
        this.player.IncreaseHP(effectQuantity);
        UpdateMessageView(this.battleText.Healed, points: effectQuantity);
    }

    private IEnumerator CheckEnemyStatus() {
        if (this.enemy.IsDead()) {
            enemyView.OnDefeat();
            UpdateMessageView(this.battleText.OnEnemyDefeat, wait: true);
            yield return Wait(1.0f);
            UpdateMessageView(this.battleText.YouWin);
        }
    }

    private IEnumerator ExecuteEnemyAction() {
        switch(this.enemy.Action)
        {
            case ActionList.Attack:
                yield return EnemyAttack();
                break;
            case ActionList.Heal:
                yield return EnemyHeal();
                break;
            case ActionList.Wait:
                yield return EnemyWait();
                break;
        }
        // Unityエディター側の警告回避用。これが呼び出されることがあってはいけない
        yield return null;
    }

    private IEnumerator EnemyAttack() {
        UpdateMessageView(this.battleText.OnEnemyAttack, wait: true);
        yield return Wait(1.0f);
        int effectQuantity = this.CalculateDamage(this.enemy, this.player);
        UpdateMessageView(this.battleText.TakeDamage, points: effectQuantity);
        this.player.DecreaseHP(effectQuantity);
    }

    private IEnumerator EnemyHeal() {
        UpdateMessageView(this.battleText.OnEnemyHeal, wait: true);
        yield return Wait(1.0f);
        // maxHpを超えての回復はしない
        // 実際の回復量は計算する
        int effectQuantity = this.CalculateHealing(this.enemy);
        UpdateMessageView(this.battleText.Healed, points: effectQuantity);
        this.enemy.IncreaseHP(effectQuantity);
    }

    private IEnumerator EnemyWait() {
        UpdateMessageView(this.battleText.EnemyWaiting);
        yield return null;
    }

    private IEnumerator CheckPlayerStatus() {
        if (this.player.IsDead()) {
            yield return Wait(1.0f);
            UpdateMessageView(this.battleText.OnPlayerDefeat);
            yield return Wait(1.0f);
            UpdateMessageView(this.battleText.YouLose);
        }
    }
    private IEnumerator Wait(float seconds) {
        yield return new WaitForSeconds(seconds);
    }
}
