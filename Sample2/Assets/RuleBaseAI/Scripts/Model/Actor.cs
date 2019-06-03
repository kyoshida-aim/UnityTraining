using UnityEngine;
public class Actor {

    private string name;
    private int maxHP;
    private int atk;
    private int dfc;
    protected ActionList action;
    public ActionList Action {
        get { return action; }
    }

    public string Name { get { return name; } }
    private int MaxHP { get { return maxHP; } }
    private int Atk { get { return atk; } }
    private int Dfc { get { return dfc; } }

    // 初期化
    public Actor(ActorParams param) {
        name = param.ActorName;
        maxHP = param.HP;
        currentHP = param.HP;
        atk = param.Atk;
        dfc = param.Dfc;
    }

    public void SetAttack() {
        this.action = ActionList.Attack;
    }
    
    public void SetHeal() {
        this.action = ActionList.Attack;
    }

    public void SetWait() {
        this.action = ActionList.Wait;
    }

    // NOTE : モデル部分の切り出しをする際に大きく仕様変更する可能性アリ
    protected int currentHP;

    public float CurrentHPPercentage { get{ return 100 * this.currentHP / this.maxHP; } }

    public void IncreaseHP(int amount) {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    public void DecreaseHP(int amount) {
        currentHP = Mathf.Max(currentHP - amount, 0); // 計算処理を外部に持たせる
    }

    public int CalculateDamageDealtTo (Actor opponent) {
        return Mathf.Max(this.atk - opponent.Dfc, 0);
    }

    public int CalculateHealing() {
        return Mathf.Clamp(this.maxHP - this.currentHP, 0, 3);
    }

    public bool IsDead() {
        return currentHP <= 0;
    }
}