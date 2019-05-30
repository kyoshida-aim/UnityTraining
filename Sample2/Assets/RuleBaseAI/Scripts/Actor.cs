using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {
    public string ActorName = "キャラクター名";
    public int Hp = 10;
    public int Atk = 6;
    public int Dfc = 3;


    // NOTE : モデル部分の切り出しをする際に大きく仕様変更する可能性アリ
    [HideInInspector]public int CurrentHP;

    void Start() {
        CurrentHP = Hp;
    }

    public void IncreaseHP(int amount) {
        CurrentHP = Math.Min(CurrentHP + amount, Hp);
    }
    public void DecreaseHP(int amount) {
        CurrentHP = Math.Max(CurrentHP - amount, 0);
    }

    public float CurrentHPPercentage() {
        return 100 * this.CurrentHP / this.Hp;
    }
}
