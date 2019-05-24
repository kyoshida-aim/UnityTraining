using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameterSettings : MonoBehaviour {

	public string actorName = "キャラクター名";
	public bool useSprite;
	[SerializeField] private Sprite characterSprite;
	public int hp = 10;
	public int atk = 10;
	public int dfc = 10;

	public Sprite CharacterSprite {
		set { this.characterSprite = value; }
		get { return this.characterSprite; }
	}
}
