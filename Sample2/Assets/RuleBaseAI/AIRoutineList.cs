using UnityEngine;
using System;

[Serializable]
public class AIRoutine {
    // 条件式
    [SerializeField] private bool turn;
    [SerializeField] private int[] turnFormula = new int[2];
    [SerializeField] private bool enemyHP;
    [SerializeField] private int[] enemyHPFormula = new int[2];
    [SerializeField] private bool playerHP;
    [SerializeField] private int[] playerHPFormula = new int[2];
    [SerializeField] private bool actionOnce;
    [SerializeField] private int actionID;
}

public class AIRoutineList : MonoBehaviour {
    public AIRoutine[] routine;
}
