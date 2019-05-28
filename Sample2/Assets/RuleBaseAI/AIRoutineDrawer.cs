using UnityEngine;
using UnityEditor;

// public enum ACTION {
// 	ATTACK = 0,
// 	HEAL = 1,
// }

[CustomPropertyDrawer (typeof(AIRoutine))]
public class AIRoutineDrawer : PropertyDrawer {

	string[] ActionList = {"ATTACK", "HEAL" };
	string[] Multiple = {"の時", "の倍数の時" };
	string[] HigherorLower = {"以上", "以下" };
	private AIRoutine routine;

	public override void OnGUI (Rect position,
		SerializedProperty property, GUIContent label)
	{
		using (new EditorGUI.PropertyScope (position, label, property)) {
		//各プロパティーの SerializedProperty を求める
		var turn = property.FindPropertyRelative ("turn");
		var turnFormula = property.FindPropertyRelative ("turnFormula");
		var enemyHP = property.FindPropertyRelative ("enemyHP");
		var enemyHPFormula = property.FindPropertyRelative ("enemyHPFormula");
		var playerHP = property.FindPropertyRelative ("playerHP");
		var playerHPFormula = property.FindPropertyRelative ("playerHPFormula");
		var actionOnce = property.FindPropertyRelative ("actionOnce");
		var actionID = property.FindPropertyRelative("actionID");

		// 各プロパティーの描画範囲を決定
		var turnTriggerRect = new Rect (position){
			height = EditorGUIUtility.singleLineHeight + 2
		};
		var turnFormulaRect1 = new Rect (turnTriggerRect){
			width = position.width / 3 * 2,
			y = turnTriggerRect.y + EditorGUIUtility.singleLineHeight + 2
		};
		var turnFormulaRect2 = new Rect (turnFormulaRect1){
			x = position.x + turnFormulaRect1.width,
			width = position.width - turnFormulaRect1.width
		};
		var enemyHPRect = new Rect(turnTriggerRect) {
			y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 2
		};
		var enemyHPFormulaRect1 = new Rect(turnTriggerRect) {
			width = position.width / 3 * 2,
			y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 3
		};
		var enemyHPFormulaRect2 = new Rect(enemyHPFormulaRect1) {
			x = position.x + enemyHPFormulaRect1.width,
			width = position.width - enemyHPFormulaRect1.width
		};
		var playerHPRect = new Rect(turnTriggerRect) {
			y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 4
		};
		var playerHPFormulaRect1 = new Rect(turnTriggerRect) {
			width = position.width / 3 * 2,
			y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 5
		};
		var playerHPFormulaRect2 = new Rect(playerHPFormulaRect1) {
			x = position.x + playerHPFormulaRect1.width,
			width = position.width - playerHPFormulaRect1.width
		};
		var actionOnceRect = new Rect(turnTriggerRect) {
			y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 6
		};
		var actionRect = new Rect(turnTriggerRect) {
			y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 7
		};

		//各プロパティーの GUI を描画

		EditorGUI.PropertyField(turnTriggerRect, turn);
		if (turn.boolValue) {
			EditorGUI.IntField(turnFormulaRect1,
				"ターン数が",
				turnFormula.GetArrayElementAtIndex(0).intValue);
			turnFormula.GetArrayElementAtIndex(1).intValue = 
				EditorGUI.Popup(turnFormulaRect2,
				turnFormula.GetArrayElementAtIndex(1).intValue,
				Multiple);
		}

		EditorGUI.PropertyField(enemyHPRect, enemyHP);
		if (enemyHP.boolValue) {
			EditorGUI.IntField(enemyHPFormulaRect1,
				"敵のHPが",
				enemyHPFormula.GetArrayElementAtIndex(0).intValue);
			enemyHPFormula.GetArrayElementAtIndex(1).intValue = 
				EditorGUI.Popup(enemyHPFormulaRect2,
				// "%",
				enemyHPFormula.GetArrayElementAtIndex(1).intValue,
				HigherorLower);
		}

		EditorGUI.PropertyField(playerHPRect, playerHP);
		if (playerHP.boolValue) {
			EditorGUI.IntField(playerHPFormulaRect1,
				"味方のHPが",
				playerHPFormula.GetArrayElementAtIndex(0).intValue);
			playerHPFormula.GetArrayElementAtIndex(1).intValue = 
				EditorGUI.Popup(playerHPFormulaRect2,
				// "%",
				playerHPFormula.GetArrayElementAtIndex(1).intValue,
				HigherorLower);
		}


		EditorGUI.PropertyField(actionOnceRect, actionOnce);
		actionID.intValue = EditorGUI.Popup(actionRect, "Action", actionID.intValue, ActionList);
		}
	}
}
