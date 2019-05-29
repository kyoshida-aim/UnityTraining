using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerParams))]
[CanEditMultipleObjects]
public class PlayerParamsEditor : CharacterParameterEditor {
	
    PlayerParams setting = null;


    public override void OnInspectorGUI() {
        serializedObject.Update ();

        setting = (PlayerParams) target;

        EditorGUILayout.PropertyField(this.actorName);
        EditorGUILayout.IntSlider(this.hp, MinHp, MaxHp);
        EditorGUILayout.IntSlider(this.atk, MinParam, MaxParam);
        EditorGUILayout.IntSlider(this.dfc, MinParam, MaxParam);
        int totalparam = setting.hp + setting.atk + setting.dfc;
        EditorGUILayout.LabelField("総合戦闘力", totalparam.ToString());

        serializedObject.ApplyModifiedProperties ();
    }
}
