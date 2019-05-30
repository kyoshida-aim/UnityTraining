using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIRoutine {
    // 条件式
    [SerializeField, HideInInspector] public bool useTurnValue;
    [SerializeField, HideInInspector] public int turnValue;
    [SerializeField, HideInInspector] public int ConstOrMulti;
    [SerializeField, HideInInspector] public bool enemyHPTrigger;
    [SerializeField, HideInInspector] public int enemyHP_ConditionValue;
    [SerializeField, HideInInspector] public int enemyHP_ConditionRange;
    [SerializeField, HideInInspector] public bool playerHPTrigger;
    [SerializeField, HideInInspector] public int playerHP_ConditionValue;
    [SerializeField, HideInInspector] public int playerHP_ConditionRange;
    [SerializeField, HideInInspector] public bool actionOnce;
    [SerializeField, HideInInspector] public int actionID;

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
    public AIRoutine[] routineList {
        get { return this.routine; }
    }

}
