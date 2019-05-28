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
	string percentageText = "%";
	// private AIRoutine routine;

	public override void OnGUI (Rect position,
		SerializedProperty property, GUIContent label)
	{
		using (new EditorGUI.PropertyScope (position, label, property)) {
			// 各プロパティーの描画範囲を決定
			// "%"を描画するのに必要な領域の計算
			float percentageTextWidth = 0f;
			float _maxWidth = 0f;
			new GUIStyle().CalcMinMaxWidth(new GUIContent(percentageText),
			out percentageTextWidth, out _maxWidth );
			percentageTextWidth *= 2;

			// 条件:ターン数
			var turnTriggerRect = new Rect (position){
				height = EditorGUIUtility.singleLineHeight + 2
			};
			var turnValueRect = new Rect (turnTriggerRect){
				width = position.width / 3 * 2,
				y = turnTriggerRect.y + EditorGUIUtility.singleLineHeight + 2
			};
			var ConstOrMultiRect = new Rect (turnValueRect){
				x = position.x + turnValueRect.width,
				width = position.width - turnValueRect.width
			};

			// 条件:敵のHP
			var enemyHPRect = new Rect(turnTriggerRect) {
				y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 2
			};
			var enemyHP_CVRect = new Rect(turnTriggerRect) {
				y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 3
			};
			var enemyHP_CRRect = new Rect(enemyHP_CVRect){
				width = position.width / 5,
			};
			enemyHP_CRRect.x = position.x +
									position.width -
									enemyHP_CRRect.width;
			enemyHP_CVRect.width = position.width -
										enemyHP_CRRect.width -
										percentageTextWidth ;

			// 条件:プレイヤーのHP
			var playerHPRect = new Rect(turnTriggerRect) {
				y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 4
			};
			var playerHP_CVRect = new Rect(turnTriggerRect) {
				y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 5
			};
			var playerHP_CRRect = new Rect(playerHP_CVRect) {
				width = position.width / 5,
			};
			playerHP_CRRect.x = position.x +
									position.width -
									playerHP_CRRect.width;
			playerHP_CVRect.width = position.width -
										playerHP_CRRect.width -
										percentageTextWidth ;

			// 処理を満たしている場合毎回処理を行うか、一回のみ行うか
			var actionOnceRect = new Rect(turnTriggerRect) {
				y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 6
			};

			// 行動内容
			var actionRect = new Rect(turnTriggerRect) {
				y = turnTriggerRect.y + (EditorGUIUtility.singleLineHeight + 2) * 7
			};

			//各プロパティーの SerializedProperty を求める
			var useTurnValue = property.FindPropertyRelative ("useTurnValue");
			var turnValue = property.FindPropertyRelative ("turnValue");
			var ConstOrMulti = property.FindPropertyRelative ("ConstOrMulti");
			var enemyHPTrigger = property.FindPropertyRelative ("enemyHPTrigger");
			var enemyHP_ConditionValue = property.FindPropertyRelative ("enemyHP_ConditionValue");
			var enemyHP_ConditionRange = property.FindPropertyRelative ("enemyHP_ConditionRange");
			var playerHPTrigger = property.FindPropertyRelative ("playerHPTrigger");
			var playerHP_ConditionValue = property.FindPropertyRelative ("playerHP_ConditionValue");
			var playerHP_ConditionRange = property.FindPropertyRelative ("playerHP_ConditionRange");
			var actionOnce = property.FindPropertyRelative ("actionOnce");
			var actionID = property.FindPropertyRelative("actionID");

			//各プロパティーの GUI を描画

			// 条件:ターン数
			EditorGUI.PropertyField(turnTriggerRect, useTurnValue);
			if (useTurnValue.boolValue) {
				turnValue.intValue = Mathf.Max(
				EditorGUI.IntField(turnValueRect,
					"ターン数が",
					turnValue.intValue), 0);

				ConstOrMulti.intValue = 
					EditorGUI.Popup(ConstOrMultiRect,
					ConstOrMulti.intValue,
					Multiple);

			}

			// 条件:敵のHP
			EditorGUI.PropertyField(enemyHPRect, enemyHPTrigger);
			if (enemyHPTrigger.boolValue) {
				enemyHP_ConditionValue.intValue = Mathf.Clamp(
				EditorGUI.IntField(enemyHP_CVRect,
					"敵のHPが",
					enemyHP_ConditionValue.intValue), 0, 100);

				EditorGUI.LabelField(new Rect(enemyHP_CVRect){
					x = enemyHP_CVRect.x + enemyHP_CVRect.width,
					width = percentageTextWidth,},
					percentageText);
				
				enemyHP_ConditionRange.intValue = 
					EditorGUI.Popup(enemyHP_CRRect,
					enemyHP_ConditionRange.intValue,
					HigherorLower);

			}
			
			// 条件:プレイヤーのHP
			EditorGUI.PropertyField(playerHPRect, playerHPTrigger);
			if (playerHPTrigger.boolValue) {

				playerHP_ConditionValue.intValue = Mathf.Clamp(
					EditorGUI.IntField(playerHP_CVRect,
					"味方のHPが",
					playerHP_ConditionValue.intValue), 0, 100);

				EditorGUI.LabelField(new Rect(playerHP_CVRect){
					x = playerHP_CVRect.x + playerHP_CVRect.width,
					width = percentageTextWidth,},
					percentageText);
				
				playerHP_ConditionRange.intValue = 
					EditorGUI.Popup(playerHP_CRRect,
					playerHP_ConditionRange.intValue,
					HigherorLower);

			}

			// 処理を満たしている場合毎回処理を行うか、一回のみ行うか
			EditorGUI.PropertyField(actionOnceRect, actionOnce);

			// 行動内容
			actionID.intValue = EditorGUI.Popup(actionRect, "Action", actionID.intValue, ActionList);
		}
	}
}
