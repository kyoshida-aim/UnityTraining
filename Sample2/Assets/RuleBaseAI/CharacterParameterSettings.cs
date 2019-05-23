using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameterSettings : MonoBehaviour {

	// 初期設定
	// Atk(攻撃力)・Dfc(防御力)のRangeはParamで共有する
	private const string defaultName = "キャラクター名";
	private const int MaxHp = 99; private const int MinHp = 1;
	private const int MaxParam = 99; private const int MinParam = 1;
	private const int defaultHp = 10;
	private const int defaultAtk = 10;
	private const int defaultDfc = 10;
	
	// public enum CHARACTERTYPE{
	// 	PLAYER,
	// 	ENEMEY,
	// }

	// Unityエディター側から変更できる部分
	[SerializeField] private string actorName = defaultName;
	[SerializeField] private bool isActor;
	// group 
	[SerializeField] private Sprite characterSprite;
	// group
	[SerializeField, Range(MinHp, MaxHp)] private int Hp = defaultHp;
	[SerializeField, Range(MinHp, MaxHp)] private int Atk = defaultAtk;
	[SerializeField, Range(MinHp, MaxHp)] private int Dfc = defaultDfc;

	// 名前(取得のみ)
	public string ActorName() { return this.actorName; }

	// HP(プロパティ)
	public int hp {
		get { return Hp; }
		set { Hp = Mathf.Clamp(value, MinHp, MaxHp); }
	}

	// Atk・Dfc(取得のみ)
	public int atk() { return this.Atk; }
	public int dfc() { return this.Dfc; }


}
