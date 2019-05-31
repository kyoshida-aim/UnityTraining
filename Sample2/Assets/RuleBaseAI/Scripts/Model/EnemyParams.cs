using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyParams :  ActorParams {

    [SerializeField] private Sprite characterSprite;
    [SerializeField] private AIRoutine[] routine;
    [SerializeField] private int index;

    public Sprite CharacterSprite {
        // TODO : 挙動に影響しないなら消去する
        // set { this.characterSprite = value; }
        get { return this.characterSprite; }
    }

    public AIRoutine[] RoutineList { get { return this.routine; } }
    public int Index { get { return index; } }

    public void UpdateRoutineListIndex(AIRoutineModel action)
    {
        index = -1;
        int counter = 0;
        foreach(var routine in RoutineList)
        {
            if (action.IsSame(routine))
            {
                index = counter;
                break;
            } 
            counter++;
        }
    }
}
