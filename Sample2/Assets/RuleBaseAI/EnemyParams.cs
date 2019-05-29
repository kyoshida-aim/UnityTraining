using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyParams :  CharacterParameterSettings {

	[SerializeField] private Sprite characterSprite;

	public Sprite CharacterSprite {
		set { this.characterSprite = value; }
		get { return this.characterSprite; }
	}
}
