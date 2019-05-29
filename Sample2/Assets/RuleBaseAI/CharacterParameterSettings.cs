using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameterSettings : MonoBehaviour {

	protected const string defaultName = "キャラクター名";
	protected const int defaultHp = 10;
	protected const int defaultAtk = 6;
	protected const int defaultDfc = 3;

	public string actorName = defaultName;
	public int hp = defaultHp;
	public int atk = defaultAtk;
	public int dfc = defaultDfc;

}
