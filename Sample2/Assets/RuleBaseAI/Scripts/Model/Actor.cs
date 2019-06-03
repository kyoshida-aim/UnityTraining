using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{
}

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
        this.action = ActionList.Heal;
    }

    // NOTE : モデル部分の切り出しをする際に大きく仕様変更する可能性アリ
    protected int currentHP;
    public MyIntEvent OnHeal = new MyIntEvent();
    public MyIntEvent OnDamage = new MyIntEvent();

    public float CurrentHPPercentage { get{ return 100 * this.currentHP / this.maxHP; } }

    public void IncreaseHP(int amount) {
        currentHP += amount;
    }

    public void DecreaseHP(int amount) {
        currentHP -= amount;
    }

    public void Heal()
    {
        int amount = CalculateHealing();
        IncreaseHP(amount);
        OnHeal.Invoke(amount);
    }
    public void Damage<T>(ref T opponent) where T : Actor
    {
        int amount = CalculateDamageDealtTo<T>(ref opponent);
        DecreaseHP(amount);
        OnDamage.Invoke(amount);
    }

    public int CalculateDamageDealtTo<T> (ref T opponent) where T : Actor
    {
        return Mathf.Max(this.atk - opponent.Dfc, 0);
    }

    public int CalculateHealing() {
        return Mathf.Min(this.maxHP - this.currentHP, 3);
    }

    public bool IsDead() {
        return currentHP <= 0;
    }
}