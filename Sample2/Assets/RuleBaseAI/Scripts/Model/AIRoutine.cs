using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIRoutine {
    // 条件式
    [SerializeField] private bool useTurnValue;
    [SerializeField] private int turnValue;
    [SerializeField] private int constOrMulti;
    [SerializeField] private bool enemyHPTrigger;
    [SerializeField] private int enemyHP_ConditionValue;
    [SerializeField] private int enemyHP_ConditionRange;
    [SerializeField] private bool playerHPTrigger;
    [SerializeField] private int playerHP_ConditionValue;
    [SerializeField] private int playerHP_ConditionRange;
    [SerializeField] private bool actionOnce;
    [SerializeField] private int actionID;

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
