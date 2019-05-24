using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleText : MonoBehaviour {

	[SerializeField, Multiline] private string battleStart = "{0} が あらわれた！";

	public string BattleStart() {return battleStart;}
}
