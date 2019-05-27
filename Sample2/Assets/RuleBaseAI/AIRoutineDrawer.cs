using UnityEngine;
using UnityEditor;

// public enum ACTION {
// 	ATTACK = 0,
// 	HEAL = 1,
// }

[CustomPropertyDrawer (typeof(AIRoutine))]
public class AIRoutineDrawer : PropertyDrawer {

	string[] ActionList = {"ATTACK", "HEAL" };

	private AIRoutine routine;

	public override void OnGUI (Rect position,
		SerializedProperty property, GUIContent label)
	{
		using (new EditorGUI.PropertyScope (position, label, property)) {
		//各プロパティーの SerializedProperty を求める
		var turnTrigger = property.FindPropertyRelative ("turnTrigger");
		var enemyHP = property.FindPropertyRelative ("enemyHP");
		var playerHP = property.FindPropertyRelative ("playerHP");
		var actionOnce = property.FindPropertyRelative ("actionOnce");
		var actionID = property.FindPropertyRelative("actionID");

		// 各プロパティーの描画範囲を決定
		var turnTriggerRect = new Rect (position){
			height = EditorGUIUtility.singleLineHeight + 2
		};
		var enemyHPRect = new Rect(turnTriggerRect) {
			y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 2
		};
		var playerHPRect = new Rect(enemyHPRect) {
			y = enemyHPRect.y +  (EditorGUIUtility.singleLineHeight + 2) * 2
		};
		var actionOnceRect = new Rect(playerHPRect) {
			y = playerHPRect.y +  (EditorGUIUtility.singleLineHeight + 2) * 2
		};
		var actionRect = new Rect(actionOnceRect) {
			y = actionOnceRect.y +  EditorGUIUtility.singleLineHeight + 2
		};

		//各プロパティーの GUI を描画
		EditorGUI.PropertyField(turnTriggerRect, turnTrigger);
		EditorGUI.PropertyField(enemyHPRect, enemyHP);
		EditorGUI.PropertyField(playerHPRect, playerHP);
		EditorGUI.PropertyField(actionOnceRect, actionOnce);
		actionID.intValue = EditorGUI.Popup(actionRect, "Action", actionID.intValue, ActionList);
		}
	}
}
