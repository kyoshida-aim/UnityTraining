using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EnemyParams))]
[CanEditMultipleObjects]
public class EnemyParamsEditor : ActorEditor {

    ReorderableList reorderableList;
    SerializedProperty characterSprite;
    EnemyParams settings;

    public override void OnEnable () {
        base.OnEnable();
        characterSprite = serializedObject.FindProperty("characterSprite");
        
        var prop = serializedObject.FindProperty ("routine");
        reorderableList = new ReorderableList (serializedObject, prop);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 9;
        reorderableList.drawElementCallback =
        (rect, index, isActive, isFocused) => {
            var element = prop.GetArrayElementAtIndex (index);
            rect.height -= 4;
            rect.y += 2;
            EditorGUI.PropertyField (rect, element);
        };

        var defaultColor = GUI.backgroundColor;

        reorderableList.drawHeaderCallback = (rect) =>
        EditorGUI.LabelField (rect, prop.displayName);
    }

    // ラベル設定
    public static readonly GUIContent spriteLabel = EditorGUIUtility.TrTextContent("Sprite", "The Sprite to render", (Texture) null);

    public override void OnInspectorGUI() {
        serializedObject.Update ();

        EditorGUILayout.PropertyField(this.actorName);
        EditorGUILayout.PropertyField(this.characterSprite, spriteLabel);
        EditorGUILayout.IntSlider(this.hp, MinHp, MaxHp);
        EditorGUILayout.IntSlider(this.atk, MinParam, MaxParam);
        EditorGUILayout.IntSlider(this.dfc, MinParam, MaxParam);

        updateIndex();
        reorderableList.DoLayoutList ();

        serializedObject.ApplyModifiedProperties ();
    }

    private void updateIndex() {
        settings = (EnemyParams) target;
        Debug.Log(settings.Index == reorderableList.index);
        if (settings.Index != reorderableList.index){
            reorderableList.index = settings.Index;
        }
    }
}
