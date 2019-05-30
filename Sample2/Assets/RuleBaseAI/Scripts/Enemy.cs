using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIRoutine {
    // 条件式
    [SerializeField] private bool useTurnValue;
    [SerializeField] private int turnValue;
    [SerializeField, HideInInspector] private int constOrMulti;
    [SerializeField, HideInInspector] private bool enemyHPTrigger;
    [SerializeField, HideInInspector] private int enemyHP_ConditionValue;
    [SerializeField, HideInInspector] private int enemyHP_ConditionRange;
    [SerializeField, HideInInspector] private bool playerHPTrigger;
    [SerializeField, HideInInspector] private int playerHP_ConditionValue;
    [SerializeField, HideInInspector] private int playerHP_ConditionRange;
    [SerializeField, HideInInspector] private bool actionOnce;
    [SerializeField, HideInInspector] private int actionID;

    public bool UseTurnValue { get { return this.useTurnValue; } }
    public int TurnValue { get { return this.turnValue; } }
    public int ConstOrMulti { get { return this.constOrMulti; } }
    public bool EnemyHPTrigger { get { return this.enemyHPTrigger; } }
    public int EnemyHP_ConditionValue { get { return this.enemyHP_ConditionValue; } }
    public int EnemyHP_ConditionRange { get { return this.enemyHP_ConditionRange; } }
    public bool PlayerHPTrigger { get { return this.playerHPTrigger; } }
    public int PlayerHP_ConditionValue { get { return this.playerHP_ConditionValue; } }
    public int PlayerHP_ConditionRange { get { return this.playerHP_ConditionRange; } }
    public bool ActionOnce { get { return this.actionOnce; } }
    public int ActionID { get { return this.actionID; } }
}

[DisallowMultipleComponent]
public class Enemy :  Actor {

    [SerializeField] private Sprite characterSprite;
    [SerializeField] private AIRoutine[] routine;
    [HideInInspector] public int routineIndex = 0;
    [HideInInspector] public bool needRefresh = false;

    public Sprite CharacterSprite {
        set { this.characterSprite = value; }
        get { return this.characterSprite; }
    }
    public AIRoutine[] RoutineList {
        get { return this.routine; }
    }

}
