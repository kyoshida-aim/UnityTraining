using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyParams :  ActorParams {

    [SerializeField] private Sprite characterSprite;
    [SerializeField] private AIRoutine[] routine;
    [SerializeField] private int index;
    private bool needRefresh;

    public Sprite CharacterSprite {
        get { return this.characterSprite; }
    }
    public AIRoutine[] RoutineList {
        get { return this.routine; }
    }

    public int Index {
        get { return index; }
        set { index = value; }
    }

    public bool NeedRefresh {
        get { return this.needRefresh; }
        set { this.needRefresh = value; }
    }
}
