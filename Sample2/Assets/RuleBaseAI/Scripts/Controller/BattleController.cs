using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EnemyParams))]
[RequireComponent(typeof(PlayerParams))]
[RequireComponent(typeof(BattleText))]
public class BattleController : MonoBehaviour
{
    [SerializeField] private ButtonView buttonView;
    [SerializeField] private MessageView messageView;
    [SerializeField] private EnemyView enemyView;

    int turnCount = 0;

    int effectQuantity;

    BattleText battleText;

    EnemyParams enemyParams;
    Actor enemy;
    string enemyAction = "Wait";

    PlayerParams playerParams;
    Actor player;
    string playerAction = "Wait";

    AIRoutineCollectionModel aiRoutineCollectionModel;
    private BattleLogGenerator logGenerator;

    void Start()
    {
        this.enemyParams = GetComponent<EnemyParams>();
        this.enemy = new Actor(enemyParams);
        this.enemyView.SetSprite(this.enemyParams.CharacterSprite);

        this.playerParams = GetComponent<PlayerParams>();
        this.player = new Actor(playerParams);

        this.battleText = GetComponent<BattleText>();

        this.aiRoutineCollectionModel = new AIRoutineCollectionModel(enemyParams.RoutineList);
        logGenerator = new BattleLogGenerator(battleText, player, enemy);

        updateMessageView(this.battleText.BattleStart);

        buttonView.OnAttackClick.AddListener(callPlayerAttack);
        buttonView.OnHealClick.AddListener(callPlayerHeal);
        buttonView.OnResetClick.AddListener(reset);
    }

    void reset()
    {
        SceneManager.LoadScene("RuleBaseAI");
    }

    void callPlayerAttack()
    {
        this.playerAction = "Attack";
        processingTurnExecution();
    }

    void callPlayerHeal()
    {
        this.playerAction = "Heal";
        processingTurnExecution();
    }

    private void updateMessageView(string message, bool wait = false)
    {
        var processedMessage = logGenerator.Generate(messageView.Log, message, effectQuantity, wait);
        messageView.UpdateLog(processedMessage);
    }

    void processingTurnExecution()
    {
        // ターンカウントを進める
        this.turnCount++;

        // TODO : そのうち消す？
        Debug.Log(string.Format("ターンカウント:{0}", turnCount));
        Debug.Log(string.Format("敵の現在HP:{0}%", this.enemy.CurrentHPPercentage));

        // 敵の行動を決定する
        SetEnemyAction();

        // プレイヤーと敵の双方の行動を実行
        StartCoroutine(executeAction());
    }

    void SetEnemyAction()
    {
        // 長すぎる。インデント整理したい
        var action = aiRoutineCollectionModel
            .FindActionBy(turnCount, player.CurrentHPPercentage, enemy.CurrentHPPercentage);

#if UNITY_EDITOR
        enemyParams.UpdateRoutineListIndex(action);
#endif
        this.enemyAction = action.CreateAction();
    }

    private IEnumerator executeAction()
    {
        // メッセージを時間差で変更するいい方法が思いつかなかったので楽な方法で処理する
        // 処理待ち中もボタンを押せるのでボタン連打厳禁

        yield return RunPlayerAction();

        // 敵側の行動処理に入る前にウェイトを入れる
        yield return Wait(1.0f);

        yield return RunEnemyAction();
    }

    IEnumerator RunPlayerAction()
    {
        // プレイヤー側の行動実行
        if (this.playerAction == "Attack")
        {
            updateMessageView(this.battleText.OnPlayerAttack, true);
            yield return Wait(1.0f); // 1秒待つ
            effectQuantity = player.CalculateDamageDealtTo(this.enemy);
            this.enemy.DecreaseHP(effectQuantity);
            updateMessageView(this.battleText.DealDamage);
        }
        else if (this.playerAction == "Heal")
        {
            updateMessageView(this.battleText.OnPlayerHeal, true);
            yield return Wait(1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            effectQuantity = player.CalculateHealing();
            this.player.IncreaseHP(effectQuantity);
            updateMessageView(this.battleText.Healed);
        }
    }

    IEnumerator RunEnemyAction()
    {
        // 敵側の行動実行(死亡確認も同時に行う)
        if (this.enemy.IsDead())
        {
            enemyView.OnDefeat();
            // this.enemyView.OnDefeat();
            updateMessageView(this.battleText.OnEnemyDefeat, true);
            yield return Wait(1.0f); // 1秒待つ
            updateMessageView(this.battleText.YouWin);
        }
        else if (this.enemyAction == "Attack")
        {
            updateMessageView(this.battleText.OnEnemyAttack, true);
            yield return Wait(1.0f); // 1秒待つ
            effectQuantity = enemy.CalculateDamageDealtTo(player);
            updateMessageView(this.battleText.TakeDamage);
            this.player.DecreaseHP(effectQuantity);
            
            if (player.IsDead())
            {
                yield return Wait(1.0f); // 1秒待つ
                updateMessageView(this.battleText.OnPlayerDefeat);
                yield return Wait(1.0f);
                updateMessageView(this.battleText.YouLose);
            }
        }
        else if (this.enemyAction == "Heal")
        {
            updateMessageView(this.battleText.OnEnemyHeal, true);
            yield return Wait(1.0f); // 1秒待つ
            // maxHpを超えての回復はしない
            // 実際の回復量は計算する
            effectQuantity = enemy.CalculateHealing();
            updateMessageView(this.battleText.Healed);
            this.enemy.IncreaseHP(effectQuantity);
        }
        else if (this.enemyAction == "Wait")
        {
            updateMessageView(this.battleText.EnemyWaiting);
        }
    }

    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
