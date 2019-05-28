using UnityEngine;
using System;

[Serializable]
public class AIRoutine {
    // 条件式
    [SerializeField] public bool useTurnValue;
    [SerializeField] public int turnValue;
    [SerializeField] public int ConstOrMulti;
    [SerializeField] public bool enemyHPTrigger;
    [SerializeField] public int enemyHP_ConditionValue;
    [SerializeField] public int enemyHP_ConditionRange;
    [SerializeField] public bool playerHPTrigger;
    [SerializeField] public int playerHP_ConditionValue;
    [SerializeField] public int playerHP_ConditionRange;
    [SerializeField] public bool actionOnce;
    [SerializeField] public int actionID;

}

public class AIRoutineList : MonoBehaviour {
    public AIRoutine[] routine;

    public AIRoutine[] list() {
        return routine;
    }
    public AIRoutine list(int index) {
        return routine[index];
    }
    public int size() {
        return routine.Length;
    }

}
