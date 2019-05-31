using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyParams :  ActorParams {

    [SerializeField] private Sprite characterSprite;
    [SerializeField] private AIRoutine[] routine;
    private int index = -1;

    public Sprite CharacterSprite {
        // TODO : 挙動に影響しないなら消去する
        // set { this.characterSprite = value; }
        get { return this.characterSprite; }
    }
    public AIRoutine[] RoutineList {
        get { return this.routine; }
    }

    public void UpdateRoutineListIndex(AIRoutine action) {
        index = -1;
        int counter = 0;
        foreach(var routine in RoutineList)
        {
            if (routine == action)
            {
                index = counter;
                break;
            } 
            counter++;
        }
    }

    public int Index {
        get { return index; }
    }
}
