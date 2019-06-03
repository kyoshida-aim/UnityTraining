﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyParams :  ActorParams {

    [SerializeField] private Sprite characterSprite;
    [SerializeField] private AIRoutine[] routine;
    [SerializeField] private int index;

    public Sprite CharacterSprite {
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
