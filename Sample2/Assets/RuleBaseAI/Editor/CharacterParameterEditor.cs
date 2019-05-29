using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterParameterSettings))]
[CanEditMultipleObjects]
public class CharacterParameterEditor : Editor {
    
    // シリアライズプロパティ
    protected SerializedProperty actorName;
    protected SerializedProperty hp;
    protected SerializedProperty atk;
    protected SerializedProperty dfc;

    public virtual void OnEnable () {
        actorName = serializedObject.FindProperty("actorName");
        hp = serializedObject.FindProperty("hp");
        atk = serializedObject.FindProperty("atk");
        dfc = serializedObject.FindProperty("dfc");
    }

	// 初期設定
	// Atk(攻撃力)・Dfc(防御力)のRangeはParamで共有する
	// DefではなくDfcにしてるのはDefineと略称が被るのが嫌なため
	protected const int MaxHp = 50; protected const int MinHp = 1;
	protected const int MaxParam = 50; protected const int MinParam = 1;
}