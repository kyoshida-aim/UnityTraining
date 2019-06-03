using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerParams))]
[CanEditMultipleObjects]
public class PlayerParamsEditor : ActorEditor {
	
    public override void OnInspectorGUI() {
        serializedObject.Update ();

        EditorGUILayout.PropertyField(this.actorName);
        EditorGUILayout.IntSlider(this.hp, MinHp, MaxHp);
        EditorGUILayout.IntSlider(this.atk, MinParam, MaxParam);
        EditorGUILayout.IntSlider(this.dfc, MinParam, MaxParam);

        serializedObject.ApplyModifiedProperties ();
    }

}
