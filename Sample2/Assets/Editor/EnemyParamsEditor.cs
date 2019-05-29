using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyParamsEditor))]
[CanEditMultipleObjects]
public class EnemyParamsEditor : CharacterParameterEditor {

    EnemyParams setting = null;

    void OnEnable () {
        actorName = serializedObject.FindProperty("actorName");
        characterSprite = serializedObject.FindProperty("characterSprite");
        hp = serializedObject.FindProperty("hp");
        atk = serializedObject.FindProperty("atk");
        dfc = serializedObject.FindProperty("dfc");
    }

    // ラベル設定
    public static readonly GUIContent spriteLabel = EditorGUIUtility.TrTextContent("Sprite", "The Sprite to render", (Texture) null);

    public override void OnInspectorGUI() {
        serializedObject.Update ();

        setting = (EnemyParams) target;

        EditorGUILayout.PropertyField(this.actorName);
        EditorGUILayout.PropertyField(this.characterSprite, spriteLabel);
        EditorGUILayout.IntSlider(this.hp, MinHp, MaxHp);
        EditorGUILayout.IntSlider(this.atk, MinParam, MaxParam);
        EditorGUILayout.IntSlider(this.dfc, MinParam, MaxParam);
        int totalparam = setting.hp + setting.atk + setting.dfc;
        EditorGUILayout.LabelField("総合戦闘力", totalparam.ToString());

        serializedObject.ApplyModifiedProperties ();
    }
}
