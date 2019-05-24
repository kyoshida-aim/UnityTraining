using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CharacterParameterSettings))]
[CanEditMultipleObjects]
public class CharacterParameterEditor : Editor {
    
    // シリアライズプロパティ
    SerializedProperty actorName;
    SerializedProperty useSprite;
    SerializedProperty characterSprite;
    SerializedProperty hp;
    SerializedProperty atk;
    SerializedProperty dfc;

	// 初期設定
	// Atk(攻撃力)・Dfc(防御力)のRangeはParamで共有する
	// DefではなくDfcにしてるのはDefineと略称が被るのが嫌なため
	private const string defaultName = "キャラクター名";
	private const int MaxHp = 50; private const int MinHp = 1;
	private const int MaxParam = 50; private const int MinParam = 1;
	private const int defaultHp = 10;
	private const int defaultAtk = 10;
	private const int defaultDfc = 10;

    CharacterParameterSettings setting = null;

    void OnEnable () {
        actorName = serializedObject.FindProperty("actorName");
        useSprite = serializedObject.FindProperty("useSprite");
        characterSprite = serializedObject.FindProperty("characterSprite");
        hp = serializedObject.FindProperty("hp");
        atk = serializedObject.FindProperty("atk");
        dfc = serializedObject.FindProperty("dfc");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update ();
        CharacterParameterSettings settings = (CharacterParameterSettings) target;
        settings.actorName = EditorGUILayout.TextField("キャラクター名", settings.actorName);
        settings.useSprite = EditorGUILayout.Toggle("スプライトを使用", settings.useSprite);
        if (settings.useSprite) {
            settings.characterSprite = EditorGUILayout.ObjectField(
                "スプライト",
                settings.characterSprite,
                typeof(Sprite),
                false) as Sprite;
        }
        settings.hp = EditorGUILayout.IntSlider("体力", settings.hp, MinHp, MaxHp);
        settings.atk = EditorGUILayout.IntSlider("攻撃力", settings.atk, MinParam, MaxParam);
        settings.dfc = EditorGUILayout.IntSlider("防御力", settings.dfc, MinParam, MaxParam);
        int totalparam = settings.hp + settings.atk + settings.dfc;
        EditorGUILayout.LabelField("総合戦闘力", totalparam.ToString());
        serializedObject.ApplyModifiedProperties ();
    }
}