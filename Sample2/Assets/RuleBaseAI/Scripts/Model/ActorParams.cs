using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorParams : MonoBehaviour {
    [SerializeField] protected string actorName = "キャラクター名";
    [SerializeField] protected int hp = 10;
    [SerializeField] protected int atk = 6;
    [SerializeField] protected int dfc = 3;

    public string ActorName { get { return actorName; } }
    public int HP { get { return hp; } }
    public int Atk { get { return atk; } }
    public int Dfc { get { return dfc; } }

}
