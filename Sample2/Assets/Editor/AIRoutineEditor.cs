using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(AIRoutine))]
public class AIRoutineEditor : Editor {
	ReorderableList reorderableList;

	SerializedProperty prop;

	void OnEnable () {
		this.prop = serializedObject.FindProperty("AIAction");
		reorderableList = new ReorderableList(serializedObject, this.prop);
	}

	public override void OnInspectorGUI () {
        serializedObject.Update ();
		reorderableList.DoLayoutList();
		reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
			var element = this.prop.GetArrayElementAtIndex (index);
			rect.height -= 4;
			rect.y += 2;
			EditorGUI.PropertyField (rect, element);
		};
        serializedObject.ApplyModifiedProperties ();
	}
}
