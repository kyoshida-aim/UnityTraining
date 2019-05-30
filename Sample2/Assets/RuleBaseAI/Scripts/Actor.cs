using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {
    public string actorName = "キャラクター名";
    public int hp = 10;
    public int atk = 6;
    public int dfc = 3;

    [HideInInspector]public int CurrentHP;

    void Start() {
        CurrentHP = hp;
    }

    public void increaseHP(int amount) {
        CurrentHP = Math.Min(CurrentHP + amount, hp);
    }
    public void decreaseHP(int amount) {
        CurrentHP = Math.Max(CurrentHP - amount, 0);
    }

    public float CurrentHPPercentage() {
        return 100 * this.CurrentHP / this.hp;
    }
}
