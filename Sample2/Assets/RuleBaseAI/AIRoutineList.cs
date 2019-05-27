using UnityEngine;
using System;

[Serializable]
public class AIRoutine {
    [SerializeField] private bool turnTrigger;
    [SerializeField] private bool enemyHP;
    [SerializeField] private bool playerHP;
    [SerializeField] private bool actionOnce;
    [SerializeField] private int actionID;
}

public class AIRoutineList : MonoBehaviour {
    public AIRoutine[] routine;
}
