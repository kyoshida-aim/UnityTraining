using System.Collections;
using UnityEngine;
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

    // ラベル設定
    public static readonly GUIContent spriteLabel = EditorGUIUtility.TrTextContent("Sprite", "The Sprite to render", (Texture) null);

    public override void OnInspectorGUI() {
        serializedObject.Update ();

        setting = (CharacterParameterSettings) target;

        EditorGUILayout.PropertyField(this.actorName);
        EditorGUILayout.PropertyField(this.useSprite);
        if (setting.useSprite) {
            EditorGUILayout.PropertyField(this.characterSprite, spriteLabel);
        }
        EditorGUILayout.IntSlider(this.hp, MinHp, MaxHp);
        EditorGUILayout.IntSlider(this.atk, MinParam, MaxParam);
        EditorGUILayout.IntSlider(this.dfc, MinParam, MaxParam);
        int totalparam = setting.hp + setting.atk + setting.dfc;
        EditorGUILayout.LabelField("総合戦闘力", totalparam.ToString());

        serializedObject.ApplyModifiedProperties ();
    }
}